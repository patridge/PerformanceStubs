#Performance Stubs

I'm starting a series of blog posts around identifing the fastest method for accomplishing a particular goal. They can be found on my blog under the ["performance stub" tag](http://www.patridgedev.com/tag/performance-stub/). This is the repository where I will collect all of the test code along with the framework I used to run the tests.

##Background

As I code, I like to make some light notes of alternatives while driving forward with the first implementation that makes it from my brain to my fingers. When I get the chance, I can go back and flesh out the two versions and drop them into some basic Stopwatch timing to determine which is better in terms of raw speed. Factoring those results with clarity of code, I have a method I will likely choose the next time I need the same feature.

##Tests (so far)

###Getting all subtype items from a list

<table><caption>For n=1,000,000 (half subtype) and 1,000 iterations.</caption><tr><th>Average</th><th>Method</th></tr></tr><td>0.41 ticks</td><td>SelectAsWhereNotNull</td></tr><tr><td>1.83 ticks</td><td>WhereIsCast</td></tr></table>

###Getting first subtype item from a list

<table><caption>For n=1,000,000 (first subtype at 999,900) and 1,000 iterations.</caption><tr><th>Average</th><th>Method</th></tr></tr><td>63,307.67 ticks</td><td>FirstOrDefaultAs</td></tr><tr><td>104,421.76 ticks</td><td>SelectAsWhereNotNullFirstOrDefault</td></tr></table>

###Converting a byte array to a hexadecimal string

I ran each of the various conversion methods through some crude `Stopwatch` performance testing, a run with a random sentence (n=98, 1000 iterations) and a run with a Project Gutenburg text (n=1,189,578, 150 iterations). Here are the results, roughly from fastest to slowest. All measurements are in ticks ([10,000 ticks = 1 ms](http://msdn.microsoft.com/en-us/library/system.timespan.tickspermillisecond.aspx)) and all relative notes are compared to the [slowest] `StringBuilder` implementation.

<table><caption>Text (n=1,238,957 bytes), 150 iterations.</caption><tr><th>Average</th><th>Method</th></tr></tr><td>46,890.06 ticks</td><td>ByteArrayToHexViaByteManipulation</td></tr><tr><td>101,423.99 ticks</td><td>ByteArrayToHexStringViaBitConverter</td></tr><tr><td>215,539.02 ticks</td><td>ByteArrayToHexViaSoapHexBinary</td></tr><tr><td>245,618.19 ticks</td><td>ByteArrayToHexStringViaStringBuilderForEachByteToString</td></tr><tr><td>302,274.76 ticks</td><td>ByteArrayToHexStringViaStringBuilderAggregateByteToString</td></tr><tr><td>565,503.04 ticks</td><td>ByteArrayToHexStringViaStringBuilderForEachAppendFormat</td></tr><tr><td>581,972.65 ticks</td><td>ByteArrayToHexStringViaStringJoinArrayConvertAll</td></tr><tr><td>591,382.22 ticks</td><td>ByteArrayToHexStringViaStringConcatArrayConvertAll</td></tr><tr><td>621,137.07 ticks</td><td>ByteArrayToHexStringViaStringBuilderAggregateAppendFormat</td></tr></table>

<table><caption>Text (n=61 bytes), 150 iterations.</caption><tr><th>Average</th><th>Method</th></tr></tr><td>2.21 ticks</td><td>ByteArrayToHexViaByteManipulation</td></tr><tr><td>6.37 ticks</td><td>ByteArrayToHexStringViaBitConverter</td></tr><tr><td>10.59 ticks</td><td>ByteArrayToHexViaSoapHexBinary</td></tr><tr><td>12.55 ticks</td><td>ByteArrayToHexStringViaStringBuilderForEachByteToString</td></tr><tr><td>15.30 ticks</td><td>ByteArrayToHexStringViaStringBuilderAggregateByteToString</td></tr><tr><td>16.13 ticks</td><td>ByteArrayToHexStringViaStringJoinArrayConvertAll</td></tr><tr><td>16.56 ticks</td><td>ByteArrayToHexStringViaStringConcatArrayConvertAll</td></tr><tr><td>33.50 ticks</td><td>ByteArrayToHexStringViaStringBuilderAggregateAppendFormat</td></tr><tr><td>380.50 ticks</td><td>ByteArrayToHexStringViaStringBuilderForEachAppendFormat</td></tr></table>

 - Byte Manipulation
   - Text: 90,465 (12.06X faster)
   - Sentence: 4.3 (14.16X faster)
 - `BitConverter`
   - Text: 162,098 (6.73X faster)
   - Sentence: 10 (6.09X faster)
 - `{SoapHexBinary}.ToString`
   - Text: 338,721.1 (3.22X faster)
   - Sentence: 18.2 (3.35X faster)
 - `{byte}.ToString("X2")` (using `foreach`)
   - Text: 380,966 (2.86X faster)
   - Sentence: 20.1 (3.03X faster)
 - `{byte}.ToString("X2")` (using `{IEnumerable}.Aggregate`, requires System.Linq)
   - Text: 470,690 (2.32X faster)
   - Sentence: 23.1 (2.64X faster)
 - `Array.ConvertAll` (using `string.Join`)
   - Text: 952,553 (1.15X faster)
   - Sentence: 31 (1.96X faster)
 - `Array.ConvertAll` (using `string.Concat`, requires .NET 4.0)
   - Text: 974,442 (1.12X faster)
   - Sentence: 24.7 (2.47X faster)
 - `{StringBuilder}.AppendFormat` (using `foreach`)
   - Text: 999,868 (1.09X faster)
   - Sentence: 53.2 (1.14X faster)
 - `{StringBuilder}.AppendFormat` (using `{IEnumerable}.Aggregate`, requires System.Linq)
   - Text: 1,091,130 (1X)
   - Sentence: 60.9 (1X)

*NOTE: All tests on AMD Phenom 9750 2.40GHz.*

Byte manipulation, while harder to read, is definitely the fastest approach. `BitConverter` is second, even with the `.Replace("-", "")` to match its output with the rest. `SoapHexBinary` took over the third place position when it was added, bumping the two `Array.ConvertAll` variants.

##Disclaimer

Don't take these test results as any end-all answer. These are the numbers I got on some random computer I was using at the time. Feel free to use them as a starting point. If you are really digging into performance, you already know that everything can change once you put the code in question in its natural surroundings or put it under load.

##Contributions

If you find something wrong with this stuff or have recommendations for the testing framework, don't hesitate to bring it up. If you have a test idea that would add some value to the world, feel free to write up something that implements `IPerformanceTest` and submit it. New issues and pull requests are always welcome. If you submit it, I will assume you don't mind it becoming part of this project and subject to its MIT license.

##License

MIT license. If you do something cool with it, though, I'd love to hear about it.