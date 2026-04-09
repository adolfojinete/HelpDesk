using Newtonsoft.Json;
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

        this.KeyPreview = true;
        this.KeyDown += Index_KeyDown;
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
    }

    // 🔹 JSON
    private void CargarConcentradores()
    {
        try
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "lstConcentradores.json");

            var json = File.ReadAllText(path);
            var config = JsonConvert.DeserializeObject<Config>(json);

            ddlConcentrador.DataSource = config?.concentradores;
            ddlConcentrador.DisplayMember = "nombre";
            ddlConcentrador.ValueMember = "host";
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error cargando concentradores: " + ex.Message);
        }
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
        btnApiLog.Enabled = true;
        btnDetener.Enabled = false;

        _running = false;
        Log("Monitoreo detenido.");
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

            _running = false;
            btnApiLog.Enabled = true;
            btnDetener.Enabled = false;

            Log("Monitoreo detenido por búsqueda.");
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
            btnCargarDockers.Enabled = true;
        }
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
            return;
        }

        lblReinicioSinDockers.Visible = false;
        panelReinicioTodosBar.Visible = true;
        flowLayoutPanelDockers.Visible = true;
        btnReiniciar.Visible = true;

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
}