using System.Collections.Immutable;

namespace Spaghet.Hl7; 

public readonly struct Group : IHl7Part {
    public Entry SegmentHeader { get; }
     public ImmutableArray<IHl7Part> Entries { get; }
    public IHl7Part this[int index] => 
        index >= Entries.Length ? Empty.Default : Entries[index];
    public Group(IEnumerable<IHl7Part> components, Entry segmentHeader) =>
        (Entries, SegmentHeader) = 
        (components.ToImmutableArray(), segmentHeader);
    
    public override string ToString() =>
        string.Join('\n', Entries);
}