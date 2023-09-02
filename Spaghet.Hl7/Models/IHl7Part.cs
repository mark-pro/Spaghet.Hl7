using System.Collections.Immutable;

namespace Spaghet.Hl7; 

public interface IHl7Part {
    ImmutableArray<IHl7Part> Entries { get; }
    IHl7Part this[int index] { get; }
}