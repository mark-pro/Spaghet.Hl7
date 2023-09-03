using System.Collections.Immutable;

namespace Spaghet.Hl7; 

/// <summary>
/// Hl7 Field is a group typically between pipes (|) such as PID|1|12345| where 12345 is a field.
/// A field can container a <see cref="Component">, <see cref="Repetition">, or a <see cref="Component"/> with <see cref="SubComponent"/>
/// </summary>
public readonly struct Field : IHl7Part {
    private readonly Delimiters _delimiters;
    public ImmutableArray<IHl7Part> Entries { get; }
    public IHl7Part this[int index] => Entries[index];

    private Field(Entry entry, Delimiters delimiters) {
        (Entries, _delimiters) = 
            (entry.Entries
                .Select(hl7 =>
                    hl7 switch {
                        Entry e when IgnoreMsh1(e.ToString(), delimiters) => hl7,
                        Entry e => GetEitherRepetitionOrComponent(e, delimiters),
                        _ => hl7
                    })
                .ToImmutableArray()
            , delimiters);
    }

    public static Field Create(Entry entry, Delimiters delimiters) =>
        new(entry, delimiters);
    
    public static bool IgnoreMsh1(string line, Delimiters delimiters) =>
        line == delimiters.ToString();

    private static IHl7Part GetEitherRepetitionOrComponent(
        Entry entry,
        Delimiters delimiters
    ) => entry.ToString() switch {
            var e when e.Contains(delimiters.Repetition) =>
                Repetition.Create(e.Hl7Split(delimiters.Repetition)
                    .Select(Entry.Create), delimiters),
            var e when e.Contains(delimiters.Component) =>
                new Component(e
                        .Hl7Split(delimiters.Component)
                        .Select(Entry.Create)
                    , delimiters),
            "" => Empty.Default,
            _ => entry
        };
    
    public override string ToString() => 
        string.Join(_delimiters.Field, Entries);
}
