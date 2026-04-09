using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Drawing;

namespace UI;

public partial class Index : Form
{
    private bool _running = false;
    private HashSet<string> _lineasMostradas = new HashSet<string>();

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

            ssh.ConnectMain();
            ssh.ConnectBastion();
            ssh.ConnectToConcentrador(host);
            ssh.BecomeRoot();

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

    // 🔹 LIMPIAR ANSI
    private string CleanOutput(string input)
    {
        return Regex.Replace(input, @"\x1B\[[0-9;]*[A-Za-z]", "");
    }
}