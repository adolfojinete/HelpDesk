using System.Data;
using System.Text.RegularExpressions;

namespace UI;

/// <summary>
/// Convierte la salida en modo batch de mysql/mariadb (<c>-B</c>, sin <c>-N</c>) en un <see cref="DataTable"/>.
/// </summary>
internal static class MariaDbBatchTsvParser
{
    private static readonly Regex AnsiCsi = new(
        @"\x1B\[[\d?;]*[A-Za-z]",
        RegexOptions.Compiled);

    public static (DataTable? Table, string? ErrorMessage) TryParse(string rawOutput)
    {
        if (string.IsNullOrWhiteSpace(rawOutput))
            return (CrearTablaVacia(), null);

        var cleaned = AnsiCsi.Replace(rawOutput, "");
        cleaned = Regex.Replace(cleaned, @"\x1B\][^\x07\x1B]*(\x07|\\)", "");

        var lines = cleaned
            .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(l => l.TrimEnd('\r'))
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .ToList();

        var filtered = new List<string>();
        foreach (var line in lines)
        {
            var t = line.Trim();
            if (EsRuidoShell(t))
                continue;

            if (t.StartsWith("ERROR ", StringComparison.OrdinalIgnoreCase) ||
                t.StartsWith("ERROR\t", StringComparison.OrdinalIgnoreCase))
                return (null, t);

            filtered.Add(t);
        }

        QuitarPrefijoBannerSesion(ref filtered);

        if (filtered.Count == 0)
            return (CrearTablaVacia(), null);

        try
        {
            return (ConstruirTabla(filtered), null);
        }
        catch (Exception ex)
        {
            return (null, "No se pudo interpretar la salida: " + ex.Message);
        }
    }

    private static DataTable CrearTablaVacia()
    {
        return new DataTable();
    }

    private static bool EsRuidoShell(string t)
    {
        // Filas TSV (cabecera o datos): nunca descartar aunque una celda parezca "mysql" o "docker".
        if (t.Contains('\t'))
            return false;

        // Eco del comando (transporte SQL por stdin vía base64 en el shell remoto).
        if (t.TrimStart('+', ' ', '\t').StartsWith("printf ", StringComparison.OrdinalIgnoreCase))
            return true;
        if (t.IndexOf("| base64 -d |", StringComparison.OrdinalIgnoreCase) >= 0 &&
            t.IndexOf("docker exec", StringComparison.OrdinalIgnoreCase) >= 0)
            return true;

        if (t.StartsWith("docker ", StringComparison.OrdinalIgnoreCase) ||
            t.StartsWith("mariadb", StringComparison.OrdinalIgnoreCase) ||
            t.StartsWith("mysql", StringComparison.OrdinalIgnoreCase) ||
            t.StartsWith("OCI runtime", StringComparison.OrdinalIgnoreCase) ||
            t.Contains("exec failed", StringComparison.OrdinalIgnoreCase))
            return true;

        if (t.Contains('┌') || t.Contains('└') || t.Contains("──"))
            return true;

        if (t.Contains('@', StringComparison.Ordinal) &&
            (t.EndsWith("#", StringComparison.Ordinal) || t.EndsWith("$", StringComparison.Ordinal) || t.Contains("└──>", StringComparison.Ordinal)))
            return true;

        return false;
    }

    /// <summary>
    /// Quita al inicio líneas de neofetch/MOTD que a veces se mezclan con la salida del comando (misma lectura SSH).
    /// Solo el prefijo: no toca filas de datos que casualmente parezcan un banner.
    /// </summary>
    private static void QuitarPrefijoBannerSesion(ref List<string> filtered)
    {
        while (filtered.Count > 0)
        {
            var x = filtered[0].Trim();
            if (x.Contains('\t'))
                break;
            if (!EsLineaBannerNeofetchOMotd(x))
                break;
            filtered.RemoveAt(0);
        }
    }

    /// <summary>
    /// Líneas típicas de neofetch/fastfetch/MOTD al abrir sesión en el concentrador (sin tabulador = no son TSV SQL).
    /// </summary>
    private static bool EsLineaBannerNeofetchOMotd(string t)
    {
        if (t.Length == 0)
            return false;

        // Prompts decorativos (kali, starship, etc.)
        if (t.Contains('㉿', StringComparison.Ordinal))
            return true;
        if (t.Contains("(root@", StringComparison.Ordinal) && (t.Contains('┌') || t.Contains('├') || t.Contains('└') || t.Contains('─')))
            return true;

        static bool Pref(string s, string p) => s.StartsWith(p, StringComparison.OrdinalIgnoreCase);

        if (Pref(t, "GPU:") || Pref(t, "Memory:") || Pref(t, "Swap:") || Pref(t, "Disk (") ||
            Pref(t, "Local IP") || Pref(t, "Locale:") || Pref(t, "OS:") || Pref(t, "Host:") ||
            Pref(t, "Kernel:") || Pref(t, "Uptime:") || Pref(t, "Packages:") || Pref(t, "Shell:") ||
            Pref(t, "Resolution:") || Pref(t, "DE:") || Pref(t, "WM:") || Pref(t, "Terminal:") ||
            Pref(t, "CPU:") || Pref(t, "Board:") || Pref(t, "BIOS:"))
            return true;

        return false;
    }

    private static DataTable ConstruirTabla(List<string> lineas)
    {
        var dt = new DataTable();
        var header = lineas[0];
        var sep = '\t';
        var cols = SplitTsvLine(header, sep);

        foreach (var c in cols)
            dt.Columns.Add(string.IsNullOrEmpty(c) ? " " : c, typeof(string));

        if (dt.Columns.Count == 0)
            return dt;

        for (var i = 1; i < lineas.Count; i++)
        {
            var cells = SplitTsvLine(lineas[i], sep);
            if (cells.Count == 0)
                continue;

            while (cells.Count < dt.Columns.Count)
                cells.Add("");
            if (cells.Count > dt.Columns.Count)
                cells = cells.Take(dt.Columns.Count).ToList();

            var row = dt.NewRow();
            for (var c = 0; c < dt.Columns.Count; c++)
                row[c] = NormalizarCelda(cells[c]);
            dt.Rows.Add(row);
        }

        return dt;
    }

    private static List<string> SplitTsvLine(string line, char sep) =>
        line.Split(sep).Select(NormalizarCelda).ToList();

    private static string NormalizarCelda(string s)
    {
        s = s.Trim();
        return s == @"\N" ? "" : s;
    }
}
