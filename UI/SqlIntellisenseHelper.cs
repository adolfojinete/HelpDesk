using System.Text.RegularExpressions;

namespace UI;

/// <summary>
/// Análisis ligero del SQL para sugerencias (MVP: primer <c>FROM</c> simple, sin JOINs complejos).
/// </summary>
internal static class SqlIntellisenseHelper
{
    private static readonly Regex RxEspacios = new(@"\s+", RegexOptions.Compiled);
    private static readonly Regex RxFrom = new(
        @"\bFROM\s+(?:(\w+)\.)?(\w+)(?:\s+(?:AS\s+)?(\w+))?\b",
        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);

    public static string ColapsarEspacios(string? sql) => RxEspacios.Replace(sql ?? "", " ").Trim();

    /// <summary>
    /// Resuelve el primer <c>FROM esquema.tabla</c> o <c>FROM tabla alias</c>.
    /// </summary>
    public static bool TryParseFromPrimario(
        string sqlColapsado,
        string defaultSchema,
        out string schema,
        out string tabla,
        out Dictionary<string, (string Sch, string Tbl)> aliasOTabla)
    {
        schema = defaultSchema;
        tabla = "";
        aliasOTabla = new Dictionary<string, (string, string)>(StringComparer.OrdinalIgnoreCase);
        var m = RxFrom.Match(sqlColapsado);
        if (!m.Success || string.IsNullOrEmpty(m.Groups[2].Value))
            return false;

        if (!string.IsNullOrEmpty(m.Groups[1].Value))
            schema = m.Groups[1].Value;
        tabla = m.Groups[2].Value;
        var alias = m.Groups[3].Value;

        aliasOTabla[tabla] = (schema, tabla);
        if (!string.IsNullOrEmpty(alias))
            aliasOTabla[alias] = (schema, tabla);
        return true;
    }

    public static (int start, int endExclusive) TokenEnCaret(string text, int caret)
    {
        if (string.IsNullOrEmpty(text) || caret < 0)
            return (0, 0);
        if (caret > text.Length)
            caret = text.Length;
        var a = caret;
        var b = caret;
        while (a > 0 && EsId(text[a - 1]))
            a--;
        while (b < text.Length && EsId(text[b]))
            b++;
        return (a, b);
    }

    public static bool EsId(char c) => char.IsLetterOrDigit(c) || c == '_';

    /// <summary>Identificador inmediatamente antes del último punto antes de <paramref name="caret"/>.</summary>
    public static bool CalificadorAntesDelPunto(string text, int caret, out string calificador)
    {
        calificador = "";
        if (caret < 2 || text[caret - 1] != '.')
            return false;
        var end = caret - 2;
        var s = end;
        while (s >= 0 && EsId(text[s]))
            s--;
        calificador = text.Substring(s + 1, end - s);
        return calificador.Length > 0;
    }

    public static string PrefijoTrasPosicion(string text, int desde, int hasta)
    {
        if (desde < 0 || desde >= text.Length || hasta <= desde)
            return "";
        if (hasta > text.Length)
            hasta = text.Length;
        return text.Substring(desde, hasta - desde);
    }

    public static bool PrefijoSoloIdentificador(string s)
    {
        foreach (var c in s)
        {
            if (!EsId(c))
                return false;
        }
        return true;
    }
}
