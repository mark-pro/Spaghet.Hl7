using System.Collections.Immutable;

namespace Spaghet.Hl7; 

/// <summary>
/// The Hl7 Component is a collection of Hl7 <see cref="Entry"/> or <see cref="SubComponent"/>
/// The component is contained within a field and typically delimited by the caret (^).
/// <example>|John^Doe|</example>
/// </summary>
public readonly struct Component : IHl7Part {
    private readonly Delimiters _delimiters;
    public ImmutableArray<IHl7Part> Entries { get; }
    public IHl7Part this[int index] => Entries[index];

    public Component(IEnumerable<Entry> entries, Delimiters delimiters) {
        (Entries, _delimiters) =
            (entries.Select(e => (IHl7Part) (
                e.ToString() switch {
                    var s when s.Contains(delimiters.Subcomponent) =>
                        SubComponent.Create(s
                            .Hl7Split(delimiters.Subcomponent)
                            .Select(Entry.Create), delimiters),
                    "" => Empty.Default,
                    _ => e
                })).ToImmutableArray()
            , delimiters);
    }

    public override string ToString() =>
        string.Join(_delimiters.Component, Entries);
}