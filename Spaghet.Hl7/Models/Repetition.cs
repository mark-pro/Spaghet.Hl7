using System.Collections.Immutable;

namespace Spaghet.Hl7; 

public readonly struct Repetition : IHl7Part {
    private readonly Delimiters _delimiters;
    public ImmutableArray<IHl7Part> Entries { get; }
    public IHl7Part this[int index] => Entries[index];

    private Repetition(IEnumerable<Entry> entries, Delimiters delimiters) {
        _delimiters = delimiters;
        Entries = entries.Select(e => (IHl7Part) (
            e.ToString() switch {
                "" => Empty.Default,
                _ => e
            })).ToImmutableArray();
    }
    
    public static Repetition Create(IEnumerable<Entry> entries, Delimiters delimiters) =>
        new(entries, delimiters);
    
    public override string ToString() => 
        string.Join(_delimiters.Repetition, Entries);
}