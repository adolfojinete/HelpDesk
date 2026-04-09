using System.Text.RegularExpressions;

namespace UI;

/// <summary>
/// Extrae nombres de contenedores desde la salida del shell tras ejecutar docker ps (con --format).
/// </summary>
internal static class DockerPsOutputParser
{
    private static readonly Regex ValidName = new(
        @"^[\w][\w._-]*$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

    /// <summary>Secuencias CSI/ANSI (incl. bracketed paste <c>?2004h</c>) y ruido común del shell.</summary>
    private static readonly Regex AnsiCsi = new(
        @"\x1B\[[\d?;]*[A-Za-z]",
        RegexOptions.Compiled);

    private static readonly Regex ConcatIdName = new(
        @"^([a-fA-F0-9]{12})([a-zA-Z0-9][a-zA-Z0-9_.-]*)$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

    private static string StripTerminalEscapes(string raw)
    {
        if (string.IsNullOrEmpty(raw))
            return raw;

        var s = AnsiCsi.Replace(raw, "");
        s = Regex.Replace(s, @"\x1B\][^\x07\x1B]*(\x07|\\)", "");
        return s;
    }

    public static IReadOnlyList<string> ParseContainerNames(string rawOutput)
    {
        if (string.IsNullOrWhiteSpace(rawOutput))
            return Array.Empty<string>();

        var cleaned = StripTerminalEscapes(rawOutput);
        var seen = new HashSet<string>(StringComparer.Ordinal);
        var list = new List<string>();

        foreach (var line in cleaned.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
        {
            var s = line.Trim();
            if (s.Length == 0)
                continue;

            if (s.Contains(' ', StringComparison.Ordinal) || s.Contains('\t', StringComparison.Ordinal))
                continue;

            if (s.Equals("NAMES", StringComparison.OrdinalIgnoreCase))
                continue;

            if (s.Equals("docker", StringComparison.OrdinalIgnoreCase) || s.Equals("ps", StringComparison.OrdinalIgnoreCase))
                continue;

            if (s.StartsWith("CONTAINER", StringComparison.OrdinalIgnoreCase))
                continue;

            if (s.Contains('@', StringComparison.Ordinal))
                continue;

            if (!ValidName.IsMatch(s))
                continue;

            if (seen.Add(s))
                list.Add(s);
        }

        return list;
    }

    /// <summary>
    /// Parsea la salida de <c>docker ps --format '{{.ID}}\t{{.Names}}'</c>.
    /// Acepta tab entre ID y nombre, o ID corto (12 hex) pegado al nombre (p. ej. <c>91bb1d5378cfconcentrador_worker</c>)
    /// cuando el TTY no conserva el tabulador.
    /// </summary>
    public static List<(string Id, string Name)> ParseContainerIdNameLines(string rawOutput)
    {
        var result = new List<(string, string)>();
        if (string.IsNullOrWhiteSpace(rawOutput))
            return result;

        var cleaned = StripTerminalEscapes(rawOutput);

        foreach (var line in cleaned.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
        {
            var s = line.Trim();
            if (s.Length < 13)
                continue;

            if (s.Contains("docker ps", StringComparison.OrdinalIgnoreCase) ||
                s.Contains("--format", StringComparison.OrdinalIgnoreCase) ||
                s.Contains('┌') || s.Contains('└') || s.Contains("──", StringComparison.Ordinal))
                continue;

            if (s.Contains('@', StringComparison.Ordinal) && (s.Contains(':', StringComparison.Ordinal) || s.Contains('#', StringComparison.Ordinal) || s.Contains('$', StringComparison.Ordinal)))
                continue;

            string id;
            string name;

            var tab = s.IndexOf('\t');
            if (tab > 0)
            {
                id = s[..tab].Trim();
                name = s[(tab + 1)..].Trim();
            }
            else
            {
                var m = ConcatIdName.Match(s);
                if (!m.Success)
                    continue;

                id = m.Groups[1].Value;
                name = m.Groups[2].Value;
            }

            if (id.Length == 0 || name.Length == 0)
                continue;

            if (!Regex.IsMatch(id, @"^[a-fA-F0-9]+$"))
                continue;

            if (!ValidName.IsMatch(name))
                continue;

            result.Add((id, name));
        }

        return result;
    }
}
