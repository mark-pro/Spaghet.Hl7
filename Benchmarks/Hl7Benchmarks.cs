using System.Text;
using BenchmarkDotNet.Attributes;
using Spaghet.Hl7;

namespace Benchmarks; 

[MemoryDiagnoser]
public class Hl7Benchmarks {
    [Benchmark]
    public IHl7Part GenerateMessage() =>
        Message.FromString(Samples.Sample1);

    [Benchmark]
    public ReadOnlySpan<byte> MessageBytes() =>
        Encoding.ASCII.GetBytes(Samples.Sample1).AsSpan();
}