namespace Spaghet.Hl7; 

public record Delimiters(char Field, char Component, char Repetition, 
    char Escape, char Subcomponent) {
    public override string ToString() =>
        $"{Field}{Component}{Repetition}{Escape}{Subcomponent}";
    
    public static readonly Delimiters Default = 
        new ('|', '^', '~', '\\', '&');
}