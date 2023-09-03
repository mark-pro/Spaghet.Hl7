using System.Collections.Immutable;

namespace Spaghet.Hl7; 

/// <summary>
/// Hl7 Group is a collection of Hl7 <see cref="Segment"> that represent such as MSH, PID, etc.
/// </summary>
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