using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Text;

var summary = BenchmarkRunner.Run<ByteArrayToHexStringBenchmarks>();

public class ByteArrayToHexStringBenchmarks
{
    static byte[] GenerateTestInput()
    {
        //return GetSource(@"Tests\ConvertByteArrayToHexString\SmallText.txt");
        //return GetSource(@"Tests\ConvertByteArrayToHexString\LargeText.txt");
        return Encoding.ASCII.GetBytes("put any sample string you want to test here instead of a file");
    }

    //ByteArrayToHexStringViaStringJoinArrayConvertAll,
    //ByteArrayToHexStringViaStringConcatArrayConvertAll,
    //ByteArrayToHexStringViaBitConverter,
    //ByteArrayToHexStringViaStringBuilderAggregateByteToString,
    //ByteArrayToHexStringViaStringBuilderAggregateAppendFormat,
    //ByteArrayToHexStringViaStringBuilderForEachByteToString,
    //ByteArrayToHexStringViaStringBuilderForEachAppendFormat,
    //ByteArrayToHexViaByteManipulation,
    //ByteArrayToHexViaByteManipulation2,
    ////ByteArrayToHexViaSoapHexBinary,
    //ByteArrayToHexViaLookupAndShift,
    //ByteArrayToHexViaLookup,
    //ByteArrayToHexViaLookupPerByte,
    //ByteArrayToHexViaLookup32UnsafeDirect,

    [Benchmark]
    public string StringJoinArrayConvertAll() => PerformanceStubs.Candidates.ConvertByteArrayToHexString.Implementations.ByteArrayToHexStringViaStringJoinArrayConvertAll(GenerateTestInput());
    [Benchmark]
    public string StringViaStringConcatArrayConvertAll() => PerformanceStubs.Candidates.ConvertByteArrayToHexString.Implementations.ByteArrayToHexStringViaStringConcatArrayConvertAll(GenerateTestInput());

    [Benchmark]
    public string StringViaBitConverter() => PerformanceStubs.Candidates.ConvertByteArrayToHexString.Implementations.ByteArrayToHexStringViaBitConverter(GenerateTestInput());
    [Benchmark]
    public string StringBuilderAggregateByteToString() => PerformanceStubs.Candidates.ConvertByteArrayToHexString.Implementations.ByteArrayToHexStringViaStringBuilderAggregateByteToString(GenerateTestInput());
    [Benchmark]
    public string StringBuilderAggregateAppendFormat() => PerformanceStubs.Candidates.ConvertByteArrayToHexString.Implementations.ByteArrayToHexStringViaStringBuilderAggregateAppendFormat(GenerateTestInput());
    [Benchmark]
    public string StringBuilderForEachByteToString() => PerformanceStubs.Candidates.ConvertByteArrayToHexString.Implementations.ByteArrayToHexStringViaStringBuilderForEachByteToString(GenerateTestInput());
    [Benchmark]
    public string StringBuilderForEachAppendFormat() => PerformanceStubs.Candidates.ConvertByteArrayToHexString.Implementations.ByteArrayToHexStringViaStringBuilderForEachAppendFormat(GenerateTestInput());
    [Benchmark]
    public string ByteManipulation() => PerformanceStubs.Candidates.ConvertByteArrayToHexString.Implementations.ByteArrayToHexViaByteManipulation(GenerateTestInput());
    [Benchmark]
    public string ByteManipulation2() => PerformanceStubs.Candidates.ConvertByteArrayToHexString.Implementations.ByteArrayToHexViaByteManipulation2(GenerateTestInput());
    //[Benchmark]
    //public string SoapHexBinary() => PerformanceStubs.Candidates.ConvertByteArrayToHexString.Implementations.ByteArrayToHexViaSoapHexBinary(GenerateTestInput());
    [Benchmark]
    public string LookupAndShift() => PerformanceStubs.Candidates.ConvertByteArrayToHexString.Implementations.ByteArrayToHexViaLookupAndShift(GenerateTestInput());
    [Benchmark]
    public string Lookup() => PerformanceStubs.Candidates.ConvertByteArrayToHexString.Implementations.ByteArrayToHexViaLookup(GenerateTestInput());
    [Benchmark]
    public string LookupPerByte() => PerformanceStubs.Candidates.ConvertByteArrayToHexString.Implementations.ByteArrayToHexViaLookupPerByte(GenerateTestInput());
    [Benchmark]
    public string Lookup32UnsafeDirect() => PerformanceStubs.Candidates.ConvertByteArrayToHexString.Implementations.ByteArrayToHexViaLookup32UnsafeDirect(GenerateTestInput());
}
