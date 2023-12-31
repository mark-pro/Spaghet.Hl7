using System.Collections.Immutable;

namespace Spaghet.Hl7; 

/// <summary>
/// The subcomponent is the smallest part of an Hl7 message.
/// The subcomponent is contained within a component and typically delimited by the ampersand &.
/// <example>|a^1&2^c|</example>
/// </summary>
public readonly struct SubComponent : IHl7Part {
    private readonly Delimiters _delimiters;
    public ImmutableArray<IHl7Part> Entries { get; }
    public IHl7Part this[int index] => Entries[index];

    private SubComponent(IEnumerable<Entry> entries, Delimiters delimiters) {
        (Entries, _delimiters) = 
            (entries.Select(e => (IHl7Part) (
                e.ToString() switch {
                    "" => Empty.Default,
                    _ => e
                })).ToImmutableArray(), 
            delimiters);
    }
    
    public static SubComponent Create(IEnumerable<Entry> entries, Delimiters delimiters) =>
        new(entries, delimiters);

    public override string ToString() => string.Join(_delimiters.Subcomponent, Entries);
}