using System.Collections.Immutable;

namespace Spaghet.Hl7; 

public readonly struct Empty : IHl7Part {
    
    public static readonly Empty Default = new ();
    
    public ImmutableArray<IHl7Part> Entries =>
        ImmutableArray<IHl7Part>.Empty;
    
    public IHl7Part this[int _] => Default;
    
    public override string ToString() => string.Empty;
}