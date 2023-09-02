using System.Text;
using System.Text.RegularExpressions;

namespace Spaghet.Hl7; 

public static class Core {
    public static string[] Hl7Split(this string s, char c) =>
        Regex.Split(s, @$"(?<!\\)\{c}");
    
    public static string[] Hl7Split(this Entry s, char c) =>
        s.ToString().Hl7Split(c);
}