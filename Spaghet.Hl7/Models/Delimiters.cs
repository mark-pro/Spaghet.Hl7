namespace Spaghet.Hl7; 

/// <summary>
/// A series of characters that are used to separate fields, components, repetitions, escape characters, and subcomponents.
/// </summary>
/// <param name="Field">The character that deliminates a field. Typically (|)</param>
/// <param name="Component">The character that deliminates a component. Typically (^)</param>
/// <param name="Repetition">The character that deliminates a Repetition. Typically (~)</param>
/// <param name="Escape">The escape character. Typically (\)</param>
/// <param name="Subcomponent">The character that deliminates a subcomponent. Typically (&)</param>
public record Delimiters(char Field, char Component, char Repetition, 
    char Escape, char Subcomponent) {
    public override string ToString() =>
        $"{Field}{Component}{Repetition}{Escape}{Subcomponent}";
    
    public static readonly Delimiters Default = 
        new ('|', '^', '~', '\\', '&');
}