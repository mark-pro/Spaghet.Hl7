using System.Collections.Immutable;

namespace Spaghet.Hl7; 

public readonly struct Segment : IHl7Part {
    private readonly Delimiters _delimiters;
    public ImmutableArray<IHl7Part> Entries { get; }
    public IHl7Part this[int index] => 
        index >= Entries.Length ? Empty.Default : Entries[index][0];

    public readonly Entry SegmentHeader;

    private Segment(Entry message, Delimiters delimiters) {
        var segments = message.Hl7Split(delimiters.Field);
        SegmentHeader = Entry.Create(segments[0]);
        _delimiters = delimiters;
        Entries = segments
            .Skip(1)
            .Select(Entry.Create)
            .Select(s => Field.Create(s, delimiters))
            .Cast<IHl7Part>()
            .ToImmutableArray();
    }

    /// <summary>
    /// Creates a segment from a string.
    /// <para>The delimiters will be passed down to the subcomponents.</para>
    /// </summary>
    /// <param name="entry">
    /// The string to parse.
    /// <para>
    /// Example: "MSH|^~\&amp;|EPIC|SCO|SC_ORM|SCO_EI|20200526031616||ORM^O01|liwDKNhRsCyRrSYbOvI0|P|2.3||||||UNICODE"
    /// </para>
    /// </param>
    /// <param name="delimiters">
    /// The delimiters to use generally coming from the message header or MSH.
    /// <para>Example: (^, ~, \,&amp;)</para>  
    /// </param>
    public static Segment Create(Entry entry, Delimiters delimiters) =>
        new(entry, delimiters);

    public override string ToString() => 
        SegmentHeader + _delimiters.Field + string.Join(_delimiters.Field, Entries);
}