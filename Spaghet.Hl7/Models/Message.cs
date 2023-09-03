using System.Collections.Immutable;
using System.Text;

namespace Spaghet.Hl7; 

/// <summary>
/// The HL7 message is the top level Hl7.
/// The Hl7 message is a collection of Hl7 <see cref="Segment"/>s or <see cref="Group"/>s.
/// </summary>
public readonly struct Message : IHl7Part {

    internal readonly Delimiters Delimiters;
    public ImmutableArray<IHl7Part> Entries { get; }
    public IHl7Part this[int index] => 
        index >= Entries.Length ? Empty : Entries[index];
    
    public IHl7Part this[string key] { get {
        try {
            return Entries
                .First(f => f switch {
                    Group g => g.SegmentHeader == key,
                    Segment s => s.SegmentHeader == key,
                    _ => false
                });
        } catch {
            return Empty;
        }
    } }
    
    private Message(string message) {
        var msh1 = message[3..8];
        var delimiters = msh1 switch {
            [var field, var component, var repetition, var escape, var subcomponent] 
                when field == '|' => new Delimiters(field, component, repetition, escape, subcomponent),
            _ => Delimiters.Default
        };
        Delimiters = delimiters;
        Entries = message.Split('\n')
            .Select(Entry.Create)
            .Select(s => Segment.Create(s, delimiters))
            .GroupBy(s => s.SegmentHeader)
            .Select(s => (IHl7Part) (s.Count() > 1 ? new Group(s.Cast<IHl7Part>(), s.Key) : s.First()))
            .ToImmutableArray();
    }
    
    public static readonly IHl7Part Empty = Hl7.Empty.Default;

    /// <summary>
    /// Converts a string into an HL7 message. The string must be delimited by newlines.
    /// <para>The first line must be the MSH segment.</para>
    /// <para>
    /// The start, end, and carriage return end bytes Should not be passed.
    /// </para>
    /// </summary>
    /// <param name="message"></param>
    /// <exception cref="ArgumentException">
    /// Thrown when the first line is not the MSH segment.
    /// </exception>
    /// <returns></returns>
    public static Message FromString(string message) {
        if(message[..3] != "MSH") 
            throw new ArgumentException("The first line must be the MSH segment.");
        if((byte) message.First() == 0x0B)
            message = message[1..];
        message = message switch {
            [.. _, var end, var carriage] 
                when (end, carriage) == (0x1C, 0x0D) =>
                message[..^2],
            _ => message
        };
        return new Message(message);
    }

    /// <summary>
    /// Converts a span of bytes into an HL7 message.
    /// </summary>
    /// <param name="message">
    ///  The bytes to convert.
    /// </param>
    /// <returns></returns>
    public static IHl7Part FromBytes(Span<byte> message) {
        var (startByte, endByte, delimiterByte) = 
            (0x0B, 0x1C, 0x0D);
        return message switch {
            [var start, ..var contents, var end, var carriage] 
                when start == startByte && (end, carriage) == (endByte, delimiterByte) => 
                FromString(Encoding.ASCII.GetString(contents)),
            _ => Empty
        };
    }
    
    /// <summary>
    /// Converts a read-only span of bytes into an HL7 message.
    /// </summary>
    /// <param name="message">
    /// The bytes to convert.
    /// </param>
    /// <returns></returns>
    public static IHl7Part FromBytes(ReadOnlySpan<byte> message) =>
        FromBytes(message.ToArray());
    
    /// <summary>
    /// Converts a span of bytes into an HL7 message. 
    /// </summary>
    /// <param name="message">
    /// The bytes to convert.
    /// </param>
    /// <returns></returns>
    public static IHl7Part FromBytes(byte[] message) =>
        FromBytes(message.AsSpan());
    
    /// <summary>
    /// Converts a memory bytes into an HL7 message.
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public static IHl7Part FromBytes(Memory<byte> message) =>
        FromBytes(message.Span);
    
    /// <summary>
    /// Converts a read-only memory bytes into an HL7 message.
    /// </summary>
    /// <param name="message">
    /// The bytes to convert.
    /// </param>
    /// <returns></returns>
    public static IHl7Part FromBytes(ReadOnlyMemory<byte> message) =>
        FromBytes(message.Span);
    
    /// <summary>
    /// Converts a stream of bytes into an HL7 message.
    /// </summary>
    /// <param name="message">
    /// The byte stream to convert.
    /// </param>
    /// <returns></returns>
    public static IHl7Part FromStream(Stream message) {
        using var memory = new MemoryStream();
        message.CopyTo(memory);
        return FromBytes(memory.ToArray());
    }
    
    /// <summary>
    /// Converts a stream of bytes asynchronously into an HL7 message.
    /// </summary>
    /// <param name="message">
    /// The byte stream to convert.
    /// </param>
    /// <returns></returns>
    public static async ValueTask<IHl7Part> FromStreamAsync(Stream message) {
        using var memory = new MemoryStream();
        await message.CopyToAsync(memory);
        return FromBytes(memory.ToArray());
    }

    public override string ToString() =>
        string.Join('\n', Entries);
}