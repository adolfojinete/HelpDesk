using ClosedXML.Excel;
using Newtonsoft.Json;
using System.Configuration;
using System.Data;
using System.Text.RegularExpressions;
using System.Drawing;

namespace UI;

public partial class Index : Form
{
    private const string DockerPsIdNamesCommand = "docker ps --format '{{.ID}}\t{{.Names}}'";
    private const string DockerRestartAllCommand = "docker restart $(docker ps -q)";

    private bool _running = false;
    private HashSet<string> _lineasMostradas = new HashSet<string>();

    private bool _syncingDockerChecks = false;
    private readonly List<DockerRowState> _dockerRows = new();

    /// <summary>Esquema usado en la última conexión exitosa a tablas (para Generar Script).</summary>
    private string? _sqlEsquemaActual;

    /// <summary>Índice de fila donde se abrió el menú contextual (clic derecho); más fiable que CurrentRow tras Show.</summary>
    private int _sqlMenuFilaIndice = -1;

    /// <summary>Evita resetear la UI al asignar el DataSource del combo al iniciar.</summary>
    private bool _suprimirEventoConcentrador;

    /// <summary>Último host con el que se alineó la UI; evita fallos de SelectedValue en el mismo tick que SelectedIndexChanged.</summary>
    private string? _hostConcentradorTrackeado;

    private sealed class DockerRowState
    {
        public required Panel RowPanel { get; init; }
        public required CheckBox Check { get; init; }
        public required Label Status { get; init; }
        public required string Name { get; init; }
        public required string IdPrefix12 { get; init; }
    }

    // 🔍 búsqueda
    private List<int> _matches = new List<int>();
    private int _currentMatchIndex = -1;

    public Index()
    {
        InitializeComponent();
        this.Load += Index_Load;
        this.Shown += (_, _) => AjustarSplittersSql();

        this.KeyPreview = true;
        this.KeyDown += Index_KeyDown;
    }

    /// <summary>
    /// Evita InvalidOperationException si el diseñador deja SplitterDistance fuera de rango respecto a los MinSize.
    /// </summary>
    private void AjustarSplittersSql()
    {
        try
        {
            var w = splitContainerSql.Width;
            var sw = splitContainerSql.SplitterWidth;
            if (w > 0)
            {
                var max1 = w - splitContainerSql.Panel2MinSize - sw;
                var min1 = splitContainerSql.Panel1MinSize;
                if (max1 >= min1)
                    splitContainerSql.SplitterDistance = Math.Clamp(320, min1, max1);
            }
        }
        catch
        {
            /* tamaño inicial del host aún no aplicado */
        }

        try
        {
            var h = splitSqlEditorResultados.Height;
            var sw = splitSqlEditorResultados.SplitterWidth;
            if (h > 0)
            {
                var max1 = h - splitSqlEditorResultados.Panel2MinSize - sw;
                var min1 = splitSqlEditorResultados.Panel1MinSize;
                if (max1 >= min1)
                    splitSqlEditorResultados.SplitterDistance = Math.Clamp(200, min1, max1);
            }
        }
        catch
        {
        }
    }

    private void Index_Load(object? sender, EventArgs e)
    {
        this.WindowState = FormWindowState.Maximized;

        CargarConcentradores();

        // 🔥 MODO TERMINAL
        richTextBox1.ReadOnly = true;
        richTextBox1.Font = new Font("Consolas", 10);
        richTextBox1.BackColor = Color.Black;
        richTextBox1.ForeColor = Color.White;
        richTextBox1.BorderStyle = BorderStyle.None;

        btnDetener.Enabled = false;

        flowLayoutPanelDockers.Resize += (_, _) => AjustarAnchoFilasDocker();

        progressBarReinicio.Style = ProgressBarStyle.Marquee;
        progressBarReinicio.MarqueeAnimationSpeed = 35;
        progressBarReinicio.Visible = false;
        lblReinicioBusy.Visible = false;
        panelReinicioPie.Visible = false;

        progressBarCargarDockers.Style = ProgressBarStyle.Marquee;
        progressBarCargarDockers.MarqueeAnimationSpeed = 35;
        progressBarCargarDockers.Visible = false;
        lblCargarDockersBusy.Visible = false;

        progressBarSql.Style = ProgressBarStyle.Marquee;
        progressBarSql.MarqueeAnimationSpeed = 35;
        progressBarSql.Visible = false;
        lblSqlBusy.Visible = false;

        progressBarSqlConsulta.Style = ProgressBarStyle.Marquee;
        progressBarSqlConsulta.MarqueeAnimationSpeed = 35;
        progressBarSqlConsulta.Visible = false;
        lblSqlConsultaBusy.Visible = false;

        dgvSqlResultados.ColumnHeadersDefaultCellStyle.Font = new Font(dgvSqlResultados.Font, FontStyle.Bold);

        contextMenuSqlTablas.Closed += (_, _) => _sqlMenuFilaIndice = -1;

        dgvSqlListaTablas.AutoGenerateColumns = false;
        dgvSqlListaTablas.Columns.Clear();
        dgvSqlListaTablas.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "colTablaSql",
            HeaderText = "Tablas",
            ReadOnly = true,
            SortMode = DataGridViewColumnSortMode.Automatic,
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            MinimumWidth = 60,
            FillWeight = 100,
        });
        dgvSqlListaTablas.ColumnHeadersDefaultCellStyle.Font = new Font(dgvSqlListaTablas.Font, FontStyle.Bold);
        dgvSqlListaTablas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
    }

    // 🔹 JSON
    private void CargarConcentradores()
    {
        try
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "lstConcentradores.json");

            var json = File.ReadAllText(path);
            var config = JsonConvert.DeserializeObject<Config>(json);

            _suprimirEventoConcentrador = true;
            try
            {
                ddlConcentrador.DataSource = config?.concentradores;
                ddlConcentrador.DisplayMember = "nombre";
                ddlConcentrador.ValueMember = "host";
            }
            finally
            {
                _suprimirEventoConcentrador = false;
            }

            _hostConcentradorTrackeado = ddlConcentrador.SelectedValue?.ToString();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error cargando concentradores: " + ex.Message);
        }
    }

    private void ddlConcentrador_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_suprimirEventoConcentrador)
            return;
        // Con DataSource, SelectedValue a veces aún no está actualizado en este mismo mensaje.
        BeginInvoke(new Action(NotificarCambioConcentradorSiCorresponde));
    }

    private void ddlConcentrador_SelectionChangeCommitted(object? sender, EventArgs e)
    {
        if (_suprimirEventoConcentrador)
            return;
        NotificarCambioConcentradorSiCorresponde();
    }

    private void NotificarCambioConcentradorSiCorresponde()
    {
        if (_suprimirEventoConcentrador)
            return;

        var host = ddlConcentrador.SelectedValue?.ToString();
        if (string.IsNullOrWhiteSpace(host))
            return;

        if (string.Equals(host, _hostConcentradorTrackeado, StringComparison.Ordinal))
            return;

        _hostConcentradorTrackeado = host;
        ResetUiTrasCambioConcentrador();
    }

    /// <summary>
    /// Vuelve Logs, Reinicio y SQL al estado inicial al elegir otro concentrador (datos del anterior no aplican al nuevo host).
    /// </summary>
    private void ResetUiTrasCambioConcentrador()
    {
        DetenerMonitoreoApiLog(soloSiCorre: true, escribirEnLog: false);

        _lineasMostradas.Clear();
        _matches.Clear();
        _currentMatchIndex = -1;
        richTextBox1.Clear();
        txtSearchLog.Clear();

        LimpiarPanelDockers();
        lblReinicioSinDockers.Visible = false;
        panelReinicioTodosBar.Visible = false;
        flowLayoutPanelDockers.Visible = false;
        btnReiniciar.Visible = false;
        panelReinicioPie.Visible = false;
        progressBarReinicio.Visible = false;
        lblReinicioBusy.Visible = false;
        btnCargarDockers.Enabled = true;
        SetCargarDockersBusy(false);

        _sqlEsquemaActual = null;
        _sqlMenuFilaIndice = -1;
        dgvSqlListaTablas.Rows.Clear();
        dgvSqlListaTablas.ClearSelection();
        LimpiarGridResultadosSql();
        dgvSqlResultados.ClearSelection();
        txtSqlEditor.Clear();
        SetSqlConexionBusy(false);
        SetSqlConsultaBusy(false);
        btnSqlConectar.Enabled = true;
        btnSqlEjecutar.Enabled = true;
        btnSqlExportarExcel.Enabled = true;
    }

    // 🔥 APILOG
    private async void btnApiLog_Click(object sender, EventArgs e)
    {
        btnApiLog.Enabled = false;
        btnDetener.Enabled = true;

        _running = true;
        _lineasMostradas.Clear();
        richTextBox1.Clear();

        var host = ddlConcentrador.SelectedValue?.ToString();

        Log("Iniciando monitoreo...");

        await Task.Run(() =>
        {
            using var ssh = new SshManager();

            ssh.ConnectFullPipeline(host);

            LogSafe("Conectado. Monitoreando...");

            while (_running)
            {
                var result = ssh.Execute("apilog");

                ProcesarLog(result);

                Thread.Sleep(5000);
            }
        });
    }

    private void ProcesarLog(string log)
    {
        log = CleanOutput(log);

        var lines = log
            .Split('\n')
            .Select(l => l.Trim())
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .ToList();

        foreach (var line in lines)
        {
            if (!_lineasMostradas.Contains(line))
            {
                _lineasMostradas.Add(line);
                LogSafe(line);
            }
        }
    }

    // 🔴 DETENER
    private void btnDetener_Click(object sender, EventArgs e)
    {
        DetenerMonitoreoApiLog("Monitoreo detenido.");
    }

    /// <summary>Detiene el bucle de ApiLog y restaura botones. Si <paramref name="soloSiCorre"/> es true, no hace nada si no había monitoreo activo.</summary>
    private void DetenerMonitoreoApiLog(string? mensaje = null, bool soloSiCorre = false, bool escribirEnLog = true)
    {
        if (soloSiCorre && !_running)
            return;

        _running = false;
        btnApiLog.Enabled = true;
        btnDetener.Enabled = false;
        if (escribirEnLog && !string.IsNullOrEmpty(mensaje))
            Log(mensaje);
    }

    private void tabControl1_SelectedIndexChanged(object? sender, EventArgs e)
    {
        // Reinicio = 1, SQL = 2 — al salir de Logs se detiene ApiLog si estaba en marcha
        var ix = tabControl1.SelectedIndex;
        if (ix != 1 && ix != 2)
            return;

        DetenerMonitoreoApiLog("Monitoreo detenido (cambio de pestaña).", soloSiCorre: true);
    }

    // 🔍 BUSCAR
    private void btnBuscarEnLog_Click(object sender, EventArgs e)
    {
        var texto = txtSearchLog.Text;

        if (string.IsNullOrWhiteSpace(texto))
            return;

        if (_running)
        {
            var r = MessageBox.Show(
                "Se detendrá el monitoreo para buscar. ¿Continuar?",
                "Confirmación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (r == DialogResult.No)
                return;

            DetenerMonitoreoApiLog("Monitoreo detenido por búsqueda.");
        }

        BuscarYResaltar(texto);
    }

    // 🔍 PROCESO DE BÚSQUEDA
    private void BuscarYResaltar(string texto)
    {
        _matches.Clear();
        _currentMatchIndex = -1;

        // 🔥 limpiar fondo sin romper colores
        richTextBox1.SelectAll();
        richTextBox1.SelectionBackColor = Color.Black;

        int index = 0;

        while ((index = richTextBox1.Text.IndexOf(texto, index, StringComparison.OrdinalIgnoreCase)) != -1)
        {
            _matches.Add(index);

            richTextBox1.Select(index, texto.Length);
            richTextBox1.SelectionBackColor = Color.DarkGoldenrod;

            index += texto.Length;
        }

        if (_matches.Count == 0)
        {
            MessageBox.Show("No se encontraron resultados.");
            return;
        }

        _currentMatchIndex = 0;
        IrAMatch(texto);
    }

    // 🔁 NAVEGAR (↑ ↓)
    private void Index_KeyDown(object? sender, KeyEventArgs e)
    {
        if (_matches.Count == 0) return;

        if (e.KeyCode == Keys.Down)
        {
            _currentMatchIndex++;
            if (_currentMatchIndex >= _matches.Count)
                _currentMatchIndex = 0;

            IrAMatch(txtSearchLog.Text);
        }
        else if (e.KeyCode == Keys.Up)
        {
            _currentMatchIndex--;
            if (_currentMatchIndex < 0)
                _currentMatchIndex = _matches.Count - 1;

            IrAMatch(txtSearchLog.Text);
        }
    }

    private void IrAMatch(string texto)
    {
        int pos = _matches[_currentMatchIndex];

        richTextBox1.Select(pos, texto.Length);
        richTextBox1.SelectionBackColor = Color.OrangeRed;

        richTextBox1.ScrollToCaret();

        this.Text = $"HelpDesk Busmatick  [{_currentMatchIndex + 1} / {_matches.Count}]";
    }

    // 🎨 LOG COLOREADO
    private void Log(string texto)
    {
        LogColoreado(texto);
    }

    private void LogSafe(string texto)
    {
        if (InvokeRequired)
            Invoke(new Action<string>(LogColoreado), texto);
        else
            LogColoreado(texto);
    }

    private void LogColoreado(string texto)
    {
        Color color = Color.White; // 🔥 default blanco

        if (texto.Contains("ERROR", StringComparison.OrdinalIgnoreCase))
            color = Color.Red;
        else if (texto.Contains("WARN", StringComparison.OrdinalIgnoreCase))
            color = Color.Orange;
        else if (texto.Contains("INFO", StringComparison.OrdinalIgnoreCase))
            color = Color.Lime; // 🔥 verde tipo terminal

        richTextBox1.SelectionStart = richTextBox1.TextLength;
        richTextBox1.SelectionLength = 0;

        richTextBox1.SelectionColor = color;
        richTextBox1.AppendText(texto + Environment.NewLine);

        richTextBox1.SelectionColor = richTextBox1.ForeColor;
        richTextBox1.ScrollToCaret();
    }

    // 🔹 LIMPIAR ANSI (CSI con ? p. ej. bracketed paste)
    private string CleanOutput(string input)
    {
        return Regex.Replace(input, @"\x1B\[[\d?;]*[A-Za-z]", "");
    }

    private async void btnCargarDockers_Click(object? sender, EventArgs e)
    {
        var host = ddlConcentrador.SelectedValue?.ToString();
        if (string.IsNullOrWhiteSpace(host))
        {
            MessageBox.Show("Seleccione un concentrador.", "Reinicio", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        btnCargarDockers.Enabled = false;
        SetCargarDockersBusy(true);
        try
        {
            string raw = "";
            await Task.Run(() =>
            {
                using var ssh = new SshManager();
                ssh.ConnectFullPipeline(host);
                raw = ssh.Execute(DockerPsIdNamesCommand);
            });

            var pairs = DockerPsOutputParser.ParseContainerIdNameLines(CleanOutput(raw));
            MostrarListaDockers(pairs);
        }
        catch (Exception ex)
        {
            MessageBox.Show("No se pudo obtener la lista de Docker: " + ex.Message, "Reinicio", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            SetCargarDockersBusy(false);
            btnCargarDockers.Enabled = true;
        }
    }

    private void SetCargarDockersBusy(bool busy)
    {
        progressBarCargarDockers.Visible = busy;
        lblCargarDockersBusy.Visible = busy;
        if (busy)
            progressBarCargarDockers.Style = ProgressBarStyle.Marquee;
    }

    private void MostrarListaDockers(IReadOnlyList<(string Id, string Name)> contenedores)
    {
        LimpiarPanelDockers();

        if (contenedores.Count == 0)
        {
            lblReinicioSinDockers.Visible = true;
            panelReinicioTodosBar.Visible = false;
            flowLayoutPanelDockers.Visible = false;
            btnReiniciar.Visible = false;
            panelReinicioPie.Visible = false;
            return;
        }

        lblReinicioSinDockers.Visible = false;
        panelReinicioTodosBar.Visible = true;
        flowLayoutPanelDockers.Visible = true;
        btnReiniciar.Visible = true;
        panelReinicioPie.Visible = true;

        _syncingDockerChecks = true;
        chkTodosDocker.Checked = true;

        const int anchoColumnaEstado = 130;

        foreach (var (id, nombre) in contenedores)
        {
            var id12 = id.Length >= 12 ? id[..12] : id;

            var rowPanel = new Panel
            {
                Height = 32,
                Margin = new Padding(3, 4, 3, 0),
                Padding = new Padding(0),
            };

            var lblEstado = new Label
            {
                Dock = DockStyle.Right,
                Width = anchoColumnaEstado,
                TextAlign = ContentAlignment.MiddleLeft,
                Text = "",
                ForeColor = Color.ForestGreen,
                AutoEllipsis = true,
                Padding = new Padding(6, 0, 0, 0),
            };

            var cb = new CheckBox
            {
                Dock = DockStyle.Fill,
                Text = nombre,
                Checked = true,
                AutoEllipsis = true,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(2, 4, 8, 0),
            };
            cb.CheckedChanged += DockerIndividual_CheckedChanged;

            rowPanel.Controls.Add(cb);
            rowPanel.Controls.Add(lblEstado);

            var state = new DockerRowState
            {
                RowPanel = rowPanel,
                Check = cb,
                Status = lblEstado,
                Name = nombre,
                IdPrefix12 = id12,
            };
            _dockerRows.Add(state);
            flowLayoutPanelDockers.Controls.Add(rowPanel);
        }

        _syncingDockerChecks = false;
        AjustarAnchoFilasDocker();
    }

    private void AjustarAnchoFilasDocker()
    {
        if (_dockerRows.Count == 0)
            return;

        var w = Math.Max(80, flowLayoutPanelDockers.ClientSize.Width - 24);
        foreach (var r in _dockerRows)
            r.RowPanel.Width = w;
    }

    private void LimpiarPanelDockers()
    {
        foreach (var row in _dockerRows)
            row.Check.CheckedChanged -= DockerIndividual_CheckedChanged;

        _dockerRows.Clear();
        flowLayoutPanelDockers.Controls.Clear();
    }

    private void chkTodosDocker_CheckedChanged(object? sender, EventArgs e)
    {
        if (_syncingDockerChecks)
            return;

        _syncingDockerChecks = true;
        foreach (var row in _dockerRows)
            row.Check.Checked = chkTodosDocker.Checked;
        _syncingDockerChecks = false;
    }

    private void DockerIndividual_CheckedChanged(object? sender, EventArgs e)
    {
        if (_syncingDockerChecks)
            return;

        _syncingDockerChecks = true;
        var todosMarcados = _dockerRows.Count > 0 && _dockerRows.All(r => r.Check.Checked);
        chkTodosDocker.Checked = todosMarcados;
        _syncingDockerChecks = false;
    }

    private async void btnReiniciar_Click(object? sender, EventArgs e)
    {
        var host = ddlConcentrador.SelectedValue?.ToString();
        if (string.IsNullOrWhiteSpace(host))
        {
            MessageBox.Show("Seleccione un concentrador.", "Reinicio", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (_dockerRows.Count == 0)
            return;

        var modoTodos = chkTodosDocker.Checked;
        var seleccion = modoTodos ? new List<DockerRowState>() : _dockerRows.Where(r => r.Check.Checked).ToList();

        if (!modoTodos && seleccion.Count == 0)
        {
            MessageBox.Show("Marque al menos un contenedor.", "Reinicio", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        ReinicioEstadosVacios();

        SetReinicioBusy(true);
        chkTodosDocker.Enabled = false;
        flowLayoutPanelDockers.Enabled = false;

        btnReiniciar.Enabled = false;
        btnCargarDockers.Enabled = false;
        try
        {
            await Task.Run(() =>
            {
                using var ssh = new SshManager();
                ssh.ConnectFullPipeline(host);

                if (modoTodos)
                {
                    ssh.ExecuteStreamingLines(DockerRestartAllCommand, line =>
                    {
                        if (!SshManager.IsDockerContainerIdLine(line))
                            return;

                        var row = BuscarFilaPorId(line);
                        if (row is null)
                            return;

                        BeginInvoke(new Action(() => MarcarReiniciado(row)));
                    });
                }
                else
                {
                    foreach (var row in seleccion)
                    {
                        var cmd = $"docker restart {SshManager.BashSingleQuote(row.Name)}";
                        ssh.ExecuteStreamingLines(cmd, line =>
                        {
                            if (!SshManager.IsDockerContainerIdLine(line))
                                return;

                            if (!CoincideIdConFila(row, line))
                                return;

                            BeginInvoke(new Action(() => MarcarReiniciado(row)));
                        });
                    }
                }
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error al reiniciar: " + ex.Message, "Reinicio", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            SetReinicioBusy(false);
            chkTodosDocker.Enabled = true;
            flowLayoutPanelDockers.Enabled = true;
            btnReiniciar.Enabled = true;
            btnCargarDockers.Enabled = true;
        }
    }

    private void SetReinicioBusy(bool busy)
    {
        progressBarReinicio.Visible = busy;
        lblReinicioBusy.Visible = busy;
        if (busy)
            progressBarReinicio.Style = ProgressBarStyle.Marquee;
    }

    private void ReinicioEstadosVacios()
    {
        foreach (var row in _dockerRows)
        {
            row.Status.Text = "";
            row.Status.ForeColor = SystemColors.GrayText;
        }
    }

    private static void MarcarReiniciado(DockerRowState row)
    {
        row.Status.Text = "Reiniciado";
        row.Status.ForeColor = Color.ForestGreen;
    }

    private DockerRowState? BuscarFilaPorId(string idLine)
    {
        var id = NormalizarId(idLine);
        foreach (var r in _dockerRows)
        {
            var p = NormalizarId(r.IdPrefix12);
            if (id == p || id.StartsWith(p, StringComparison.Ordinal) || p.StartsWith(id, StringComparison.Ordinal))
                return r;
        }
        return null;
    }

    private static bool CoincideIdConFila(DockerRowState row, string idLine)
    {
        return NormalizarId(idLine) == NormalizarId(row.IdPrefix12);
    }

    private static string NormalizarId(string id)
    {
        id = id.Trim().ToLowerInvariant();
        return id.Length > 12 ? id[..12] : id;
    }

    private async void btnSqlConectar_Click(object? sender, EventArgs e)
    {
        var host = ddlConcentrador.SelectedValue?.ToString();
        if (string.IsNullOrWhiteSpace(host))
        {
            MessageBox.Show("Seleccione un concentrador.", "SQL", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var container = ConfigurationManager.AppSettings["DbContainerName"]?.Trim() ?? "concentrador_db";
        var schema = ConfigurationManager.AppSettings["DbSchema"]?.Trim() ?? "concentrador";
        var user = ConfigurationManager.AppSettings["DbUser"]?.Trim() ?? "root";
        var clientBin = ConfigurationManager.AppSettings["DbClientBinary"]?.Trim();
        if (string.IsNullOrWhiteSpace(clientBin))
            clientBin = "mariadb";
        var password = ObtenerPasswordDb();

        if (string.IsNullOrWhiteSpace(password))
        {
            MessageBox.Show(
                "Configure DbPassword en App.config o defina GlobalPassword.",
                "SQL",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        if (!EsIdentificadorSqlSimple(schema))
        {
            MessageBox.Show(
                "DbSchema solo puede contener letras, números y guión bajo.",
                "SQL",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        if (!EsNombreContenedorDb(container))
        {
            MessageBox.Show("DbContainerName no es válido.", "SQL", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (!Regex.IsMatch(user, @"^[a-zA-Z0-9_.-]+$"))
        {
            MessageBox.Show("DbUser no es válido.", "SQL", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (!EsClienteDbBinarioPermitido(clientBin))
        {
            MessageBox.Show(
                "DbClientBinary debe ser un nombre seguro (p. ej. mariadb, mysql) o una ruta absoluta dentro del contenedor (p. ej. /usr/bin/mariadb).",
                "SQL",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        btnSqlConectar.Enabled = false;
        SetSqlConexionBusy(true);
        try
        {
            var cmd = ComandoDockerMysqlShowTables(container, clientBin, user, password, schema);
            string raw = "";
            await Task.Run(() =>
            {
                using var ssh = new SshManager();
                ssh.ConnectFullPipeline(host);
                raw = ssh.Execute(cmd);
            });

            var clean = CleanOutput(raw);
            var tablas = ShowTablesOutputParser.Parse(clean);

            dgvSqlListaTablas.Rows.Clear();
            foreach (var t in tablas)
                dgvSqlListaTablas.Rows.Add(t);

            _sqlEsquemaActual = schema;

            if (tablas.Count == 0)
            {
                var preview = clean.Length > 400 ? clean[..400] + "…" : clean;
                MessageBox.Show(
                    "No se obtuvo ninguna tabla. Revise el contenedor, credenciales y que el esquema exista.\n\nSalida (recorte):\n" + preview,
                    "SQL",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error al conectar o consultar: " + ex.Message, "SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            SetSqlConexionBusy(false);
            btnSqlConectar.Enabled = true;
        }
    }

    private void SetSqlConexionBusy(bool busy)
    {
        progressBarSql.Visible = busy;
        lblSqlBusy.Visible = busy;
        if (busy)
            progressBarSql.Style = ProgressBarStyle.Marquee;
    }

    private static string ObtenerPasswordDb()
    {
        var p = ConfigurationManager.AppSettings["DbPassword"];
        if (string.IsNullOrWhiteSpace(p))
            p = ConfigurationManager.AppSettings["GlobalPassword"];
        return p ?? "";
    }

    private static bool EsIdentificadorSqlSimple(string s) =>
        !string.IsNullOrWhiteSpace(s) && Regex.IsMatch(s, @"^[a-zA-Z0-9_]+$");

    private static bool EsNombreContenedorDb(string s) =>
        !string.IsNullOrWhiteSpace(s) && Regex.IsMatch(s, @"^[a-zA-Z0-9_.-]+$");

    /// <summary>Solo nombre de ejecutable o ruta absoluta sin metacaracteres de shell.</summary>
    private static bool EsClienteDbBinarioPermitido(string s) =>
        Regex.IsMatch(s, @"^[a-zA-Z][a-zA-Z0-9_.-]*$", RegexOptions.CultureInvariant)
        || Regex.IsMatch(s, @"^/[a-zA-Z0-9./_-]+$", RegexOptions.CultureInvariant);

    private static string ComandoDockerMysqlShowTables(string contenedor, string clienteBinario, string usuario, string password, string esquema)
    {
        var sql = $"USE `{esquema}`; SHOW TABLES;";
        return $"docker exec {contenedor} {clienteBinario} --user={usuario} --password={SshManager.BashSingleQuote(password)} -N -B -e {SshManager.BashSingleQuote(sql)}";
    }

    private static string ComandoDockerMysqlEjecutar(string contenedor, string clienteBinario, string usuario, string password, string sqlConsulta)
    {
        sqlConsulta = NormalizarSqlParaEnvioPorShell(sqlConsulta);
        return $"docker exec {contenedor} {clienteBinario} --user={usuario} --password={SshManager.BashSingleQuote(password)} -B -e {SshManager.BashSingleQuote(sqlConsulta)}";
    }

    /// <summary>
    /// El comando se envía por shell con WriteLine; si el SQL lleva \n, el intérprete corta la orden antes del cierre de comillas de -e.
    /// Se reemplazan saltos de línea por espacios (equivalente habitual para el cliente mysql/mariadb).
    /// </summary>
    private static string NormalizarSqlParaEnvioPorShell(string sql)
    {
        sql = sql.Trim();
        return sql.Length == 0 ? sql : Regex.Replace(sql, @"[\r\n]+", " ");
    }

    private void dgvSqlListaTablas_MouseDown(object? sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Right)
            return;

        var hit = dgvSqlListaTablas.HitTest(e.X, e.Y);
        if (hit.RowIndex < 0 || hit.RowIndex >= dgvSqlListaTablas.Rows.Count)
            return;

        _sqlMenuFilaIndice = hit.RowIndex;
        dgvSqlListaTablas.ClearSelection();
        dgvSqlListaTablas.Rows[hit.RowIndex].Selected = true;
        if (dgvSqlListaTablas.Columns.Count > 0)
        {
            var colIdx = hit.ColumnIndex >= 0 ? hit.ColumnIndex : 0;
            if (colIdx >= dgvSqlListaTablas.Columns.Count)
                colIdx = 0;
            dgvSqlListaTablas.CurrentCell = dgvSqlListaTablas.Rows[hit.RowIndex].Cells[colIdx];
        }

        contextMenuSqlTablas.Show(dgvSqlListaTablas, new Point(e.X, e.Y));
    }

    private async void toolStripMenuItemSqlGenerar_Click(object? sender, EventArgs e)
    {
        await GenerarScriptYEjecutarAsync();
    }

    private async void btnSqlEjecutar_Click(object? sender, EventArgs e)
    {
        await EjecutarConsultaSqlAsync();
    }

    private async Task GenerarScriptYEjecutarAsync()
    {
        var idx = _sqlMenuFilaIndice;
        if (idx < 0 && dgvSqlListaTablas.CurrentRow != null)
            idx = dgvSqlListaTablas.CurrentRow.Index;
        if (idx < 0 || idx >= dgvSqlListaTablas.Rows.Count)
        {
            MessageBox.Show("Seleccione una tabla (clic derecho sobre la fila).", "SQL", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var nombreTabla = dgvSqlListaTablas.Rows[idx].Cells[0].Value?.ToString()?.Trim();
        if (string.IsNullOrEmpty(nombreTabla) || !EsIdentificadorSqlSimple(nombreTabla))
        {
            MessageBox.Show("Seleccione una fila con un nombre de tabla válido.", "SQL", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var schema = _sqlEsquemaActual ?? ConfigurationManager.AppSettings["DbSchema"]?.Trim() ?? "concentrador";
        if (!EsIdentificadorSqlSimple(schema))
            schema = "concentrador";

        txtSqlEditor.Text = $"SELECT * FROM `{schema}`.`{nombreTabla}` LIMIT 300;";
        await EjecutarConsultaSqlAsync();
    }

    private async Task EjecutarConsultaSqlAsync()
    {
        var sql = NormalizarSqlParaEnvioPorShell(txtSqlEditor.Text);
        if (string.IsNullOrWhiteSpace(sql))
        {
            MessageBox.Show("Escriba una consulta SQL.", "SQL", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var host = ddlConcentrador.SelectedValue?.ToString();
        if (string.IsNullOrWhiteSpace(host))
        {
            MessageBox.Show("Seleccione un concentrador.", "SQL", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var container = ConfigurationManager.AppSettings["DbContainerName"]?.Trim() ?? "concentrador_db";
        var user = ConfigurationManager.AppSettings["DbUser"]?.Trim() ?? "root";
        var clientBin = ConfigurationManager.AppSettings["DbClientBinary"]?.Trim();
        if (string.IsNullOrWhiteSpace(clientBin))
            clientBin = "mariadb";
        var password = ObtenerPasswordDb();

        if (string.IsNullOrWhiteSpace(password))
        {
            MessageBox.Show("Configure DbPassword o GlobalPassword.", "SQL", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (!EsNombreContenedorDb(container) || !Regex.IsMatch(user, @"^[a-zA-Z0-9_.-]+$") || !EsClienteDbBinarioPermitido(clientBin))
        {
            MessageBox.Show("Revise DbContainerName, DbUser y DbClientBinary en App.config.", "SQL", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        btnSqlEjecutar.Enabled = false;
        btnSqlConectar.Enabled = false;
        btnSqlExportarExcel.Enabled = false;
        SetSqlConsultaBusy(true);
        try
        {
            var cmd = ComandoDockerMysqlEjecutar(container, clientBin, user, password, sql);
            string raw = "";
            await Task.Run(() =>
            {
                using var ssh = new SshManager();
                ssh.ConnectFullPipeline(host);
                raw = ssh.ExecuteWithDrainedOutput(cmd, initialWaitMs: null, quietWaitMs: 2000, maxMsBeforeFirstByte: 90_000);
            });

            var clean = CleanOutput(raw);
            var (tabla, err) = MariaDbBatchTsvParser.TryParse(clean);

            if (!string.IsNullOrEmpty(err))
            {
                MessageBox.Show(
                    "La base de datos devolvió un error o no se pudo interpretar la salida:\n\n" + err,
                    "SQL",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                LimpiarGridResultadosSql();
                return;
            }

            if (tabla is null)
            {
                LimpiarGridResultadosSql();
                return;
            }

            if (tabla.Columns.Count == 0)
            {
                var preview = clean.Length > 700 ? "…" + clean[^700..] : clean;
                preview = string.IsNullOrWhiteSpace(preview)
                    ? "(sin texto en la salida; el comando puede haber tardado demasiado o no haber devuelto datos)."
                    : preview.Trim();
                MessageBox.Show(
                    "No se pudieron leer columnas del resultado (salida vacía o solo avisos del shell).\n\n"
                    + "Revise la consulta, la conexión SSH y, si hace falta, aumente CommandWaitMs en App.config.\n\n"
                    + "Recorte de salida recibida:\n" + preview,
                    "SQL",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                LimpiarGridResultadosSql();
                return;
            }

            dgvSqlResultados.AutoGenerateColumns = true;
            dgvSqlResultados.DataSource = null;
            dgvSqlResultados.Columns.Clear();
            dgvSqlResultados.DataSource = tabla;

            AjustarColumnasResultadosSqlDespuesDeCargar();

            if (tabla.Rows.Count == 0)
            {
                MessageBox.Show(
                    "La consulta se ejecutó correctamente pero no devolvió filas.",
                    "SQL",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error al ejecutar la consulta: " + ex.Message, "SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            LimpiarGridResultadosSql();
        }
        finally
        {
            SetSqlConsultaBusy(false);
            btnSqlEjecutar.Enabled = true;
            btnSqlConectar.Enabled = true;
            btnSqlExportarExcel.Enabled = true;
        }
    }

    private void SetSqlConsultaBusy(bool busy)
    {
        progressBarSqlConsulta.Visible = busy;
        lblSqlConsultaBusy.Visible = busy;
        if (busy)
            progressBarSqlConsulta.Style = ProgressBarStyle.Marquee;
    }

    /// <summary>
    /// Cabeceras en negrita y anchos iniciales; luego el usuario puede redimensionar columnas a mano.
    /// </summary>
    private void AjustarColumnasResultadosSqlDespuesDeCargar()
    {
        dgvSqlResultados.ColumnHeadersDefaultCellStyle.Font = new Font(dgvSqlResultados.Font, FontStyle.Bold);
        foreach (DataGridViewColumn c in dgvSqlResultados.Columns)
            c.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        dgvSqlResultados.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        foreach (DataGridViewColumn c in dgvSqlResultados.Columns)
        {
            c.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            c.MinimumWidth = 56;
        }
    }

    private void btnSqlExportarExcel_Click(object? sender, EventArgs e)
    {
        if (dgvSqlResultados.Columns.Count == 0 || dgvSqlResultados.Rows.Count == 0)
        {
            MessageBox.Show(
                "No hay datos en la grilla para exportar. Ejecute una consulta primero.",
                "SQL",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            return;
        }

        using var dlg = new SaveFileDialog
        {
            Filter = "Excel (*.xlsx)|*.xlsx",
            DefaultExt = "xlsx",
            FileName = "resultados_sql.xlsx",
            Title = "Exportar a Excel",
        };
        if (dlg.ShowDialog(this) != DialogResult.OK)
            return;

        try
        {
            using var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Resultados");
            var colCount = dgvSqlResultados.Columns.Count;
            var rowCount = dgvSqlResultados.Rows.Count;

            for (var c = 0; c < colCount; c++)
            {
                var cell = ws.Cell(1, c + 1);
                cell.Value = dgvSqlResultados.Columns[c].HeaderText;
                cell.Style.Font.Bold = true;
            }

            for (var r = 0; r < rowCount; r++)
            {
                for (var c = 0; c < colCount; c++)
                {
                    var v = dgvSqlResultados.Rows[r].Cells[c].Value;
                    ws.Cell(r + 2, c + 1).Value = v?.ToString() ?? "";
                }
            }

            ws.Columns().AdjustToContents();
            wb.SaveAs(dlg.FileName);
            MessageBox.Show("Archivo guardado:\n" + dlg.FileName, "SQL", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show("No se pudo exportar: " + ex.Message, "SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void LimpiarGridResultadosSql()
    {
        dgvSqlResultados.DataSource = null;
        dgvSqlResultados.Columns.Clear();
    }
}