using System.Collections.Immutable;

namespace Spaghet.Hl7; 

/// <summary>
/// The entry is the smallest part of an Hl7 message.
/// The entry is a single value and can be at the base of a
/// <see cref="Field"/>, <see cref="Component"/>, <see cref="SubComponent"/> or <see cref="Repetition"/>
/// </summary>
public readonly struct Entry : IHl7Part {
    private readonly string _entry;
    public ImmutableArray<IHl7Part> Entries =>
        string.IsNullOrEmpty(_entry) 
            ? ImmutableArray.Create<IHl7Part>(Empty.Default) 
            : ImmutableArray.Create<IHl7Part>(this);
    
    public IHl7Part this[int _] => Entries[0];

    private Entry(string entry) =>
        _entry = entry;

    public static Entry Create(string entry) =>
        new(entry);
    
    public override string ToString() => _entry;
    
    public static implicit operator string(Entry entry) => entry.ToString();
}