using System.Text.RegularExpressions;

namespace Spaghet.Hl7;

public static class Core {

    /// <summary>
    /// Converts a string to a byte array. The string won't split on if the character is escaped with backslash (\).
    /// </summary>
    /// <param name="s">The string to split.</param>
    /// <param name="c">The character to split on</param>
    /// <returns></returns>
    public static string[] Hl7Split(this string s, char c) =>
        Regex.Split(s, @$"(?<!\\)\{c}");
    
    /// <summary>
    /// Converts an entry to a byte array. The string won't split on if the character is escaped with backslash (\).
    /// </summary>
    /// <param name="s">The string to split.</param>
    /// <param name="c">The character to split on.</param>
    /// <returns></returns>
    public static string[] Hl7Split(this Entry s, char c) =>
        s.ToString().Hl7Split(c);
}