using System.Text.RegularExpressions;

namespace UI;

/// <summary>
/// Interpreta la salida de <c>mysql -N -B -e "SHOW TABLES"</c> tras limpiar ruido del shell.
/// </summary>
internal static class ShowTablesOutputParser
{
    private static readonly Regex AnsiCsi = new(
        @"\x1B\[[\d?;]*[A-Za-z]",
        RegexOptions.Compiled);

    private static readonly Regex TableName = new(
        @"^[a-zA-Z0-9_]+$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

    public static List<string> Parse(string rawOutput)
    {
        var result = new List<string>();
        if (string.IsNullOrWhiteSpace(rawOutput))
            return result;

        var cleaned = AnsiCsi.Replace(rawOutput, "");
        cleaned = Regex.Replace(cleaned, @"\x1B\][^\x07\x1B]*(\x07|\\)", "");

        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var line in cleaned.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
        {
            var s = line.Trim();
            if (s.Length == 0)
                continue;

            if (s.StartsWith("docker", StringComparison.OrdinalIgnoreCase) ||
                s.StartsWith("mysql", StringComparison.OrdinalIgnoreCase) ||
                s.StartsWith("mariadb", StringComparison.OrdinalIgnoreCase) ||
                s.TrimStart('+', ' ', '\t').StartsWith("printf", StringComparison.OrdinalIgnoreCase) ||
                (s.Contains("| base64 -d |", StringComparison.OrdinalIgnoreCase) &&
                 s.Contains("docker exec", StringComparison.OrdinalIgnoreCase)) ||
                s.Contains("Tables_in_", StringComparison.OrdinalIgnoreCase))
                continue;

            if (s.StartsWith("ERROR", StringComparison.OrdinalIgnoreCase) ||
                s.StartsWith("WARN", StringComparison.OrdinalIgnoreCase) ||
                s.StartsWith("Warning", StringComparison.OrdinalIgnoreCase) ||
                s.Contains("Access denied", StringComparison.OrdinalIgnoreCase))
                continue;

            if (s.Contains('┌') || s.Contains('└') || s.Contains("──") ||
                s.Contains('@', StringComparison.Ordinal) && (s.Contains('#', StringComparison.Ordinal) || s.Contains('$', StringComparison.Ordinal)))
                continue;

            if (s.Contains('\t'))
            {
                var parts = s.Split('\t', StringSplitOptions.RemoveEmptyEntries);
                foreach (var p in parts)
                {
                    var t = p.Trim();
                    if (TableName.IsMatch(t) && seen.Add(t))
                        result.Add(t);
                }
                continue;
            }

            if (TableName.IsMatch(s) && seen.Add(s))
                result.Add(s);
        }

        return result;
    }
}
