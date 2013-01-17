#Performance Stubs

For a while, I was writing blog posts around identifing the fastest method for accomplishing particular goals. They can be found on my blog under the ["performance stub" tag](http://www.patridgedev.com/tag/performance-stub/). This is the repository where I will collect all of the test code along with the framework I used to run the tests.

##Background

As I code, I like to make some light notes of alternatives while driving forward with the first implementation that makes it from my brain to my fingers. When I get the chance, I can go back and flesh out the two versions and drop them into some basic Stopwatch timing to determine which is better in terms of raw speed. Factoring those results with clarity of code, I have a method I will likely choose the next time I need the same feature.

##Disclaimer

Don't take these test results as any end-all answer. These are the numbers I got on some random computer I was using at the time. Feel free to use them as a starting point. If you are really digging into performance, you already know that everything can change once you put the code in question in its natural surroundings (e.g., Windows .NET vs. MonoTouch) or put it under load.

###Last Run

<div><ul><li>AMD Phenom(tm) 9750 Quad-Core Processor<ul><li>Cores: 4</li><li>Current Clock Speed: 2400</li><li>Max Clock Speed: 2400</li></ul></li></ul></div>

##Tests (so far)

###Getting object properties by name at runtime (concrete type)

<table><caption>&quot;Getting object properties by name at runtime (concrete type)&quot; (For 100000 iterations.)</caption><tr><th>Average</th><th>Method</th><th>Ratio</th></tr><tr><td>1.18 ticks</td><td>ObjectAccessorLookup</td><td>12.8X</td></tr><tr><td>15.04 ticks</td><td>IDictionaryRouteValueDictionaryLookup</td><td>1.0X</td></tr></table>

###Getting object properties by name at runtime (dynamic ExpandoObject)

<table><caption>&quot;Getting object properties by name at runtime (dynamic ExpandoObject)&quot; (For 100000 iterations.)</caption><tr><th>Average</th><th>Method</th><th>Ratio</th></tr><tr><td>0.77 ticks</td><td>IDictionaryRouteValueDictionaryLookup</td><td>2.3X</td></tr><tr><td>1.79 ticks</td><td>ObjectAccessorLookup</td><td>1.0X</td></tr></table>

###Getting object properties by name at runtime (anonymous type)

<table><caption>&quot;Getting object properties by name at runtime (anonymous type)&quot; (For 100000 iterations.)</caption><tr><th>Average</th><th>Method</th><th>Ratio</th></tr><tr><td>1.51 ticks</td><td>ObjectAccessorLookup</td><td>11.7X</td></tr><tr><td>17.65 ticks</td><td>IDictionaryRouteValueDictionaryLookup</td><td>1.0X</td></tr></table>

###Getting first subtype item from a list

<table><caption>&quot;Getting first subtype item from a list&quot; (For n=1,000,000 (first subtype at 999,900), 100 iterations.)</caption><tr><th>Average</th><th>Method</th><th>Ratio</th></tr><tr><td>74,929.18 ticks</td><td>FirstOrDefaultAs</td><td>1.8X</td></tr><tr><td>135,752.02 ticks</td><td>SelectAsWhereNotNullFirstOrDefault</td><td>1.0X</td></tr></table>

###Getting all subtype items from a list

<table><caption>&quot;Getting all subtype items from a list&quot; (For n=1,000,000 (half items are subtype), 1000 iterations.)</caption><tr><th>Average</th><th>Method</th><th>Ratio</th></tr><tr><td>0.90 ticks</td><td>SelectAsWhereNotNull</td><td>1.9X</td></tr><tr><td>1.70 ticks</td><td>WhereIsCast</td><td>1.0X</td></tr></table>

###[Converting a byte array to a hexadecimal string](http://stackoverflow.com/a/624379/48700)

<table><caption>Text (n=1,238,957 bytes), 150 iterations.</caption><tr><th>Average</th><th>Method</th><th>Ratio</th></tr><tr><td>56,286.27 ticks</td><td>ByteArrayToHexViaByteManipulation2</td><td>22.4X</td></tr><tr><td>91,113.89 ticks</td><td>ByteArrayToHexViaByteManipulation</td><td>13.8X</td></tr><tr><td>163,516.47 ticks</td><td>ByteArrayToHexStringViaBitConverter</td><td>7.7X</td></tr><tr><td>342,291.12 ticks</td><td>ByteArrayToHexViaSoapHexBinary</td><td>3.7X</td></tr><tr><td>422,203.16 ticks</td><td>ByteArrayToHexStringViaStringBuilderForEachByteToString</td><td>3.0X</td></tr><tr><td>470,619.16 ticks</td><td>ByteArrayToHexStringViaStringBuilderAggregateByteToString</td><td>2.7X</td></tr><tr><td>932,242.66 ticks</td><td>ByteArrayToHexStringViaStringJoinArrayConvertAll</td><td>1.4X</td></tr><tr><td>945,120.53 ticks</td><td>ByteArrayToHexStringViaStringConcatArrayConvertAll</td><td>1.3X</td></tr><tr><td>1,174,170.17 ticks</td><td>ByteArrayToHexStringViaStringBuilderForEachAppendFormat</td><td>1.1X</td></tr><tr><td>1,258,624.65 ticks</td><td>ByteArrayToHexStringViaStringBuilderAggregateAppendFormat</td><td>1.0X</td></tr></table>

<table><caption>Text (n=61 bytes), 1000 iterations.</caption><tr><th>Average</th><th>Method</th><th>Ratio</th></tr><tr><td>4.06 ticks</td><td>ByteArrayToHexViaByteManipulation2</td><td>14.9X</td></tr><tr><td>4.68 ticks</td><td>ByteArrayToHexViaByteManipulation</td><td>12.9X</td></tr><tr><td>9.53 ticks</td><td>ByteArrayToHexStringViaBitConverter</td><td>6.3X</td></tr><tr><td>18.29 ticks</td><td>ByteArrayToHexViaSoapHexBinary</td><td>3.3X</td></tr><tr><td>21.29 ticks</td><td>ByteArrayToHexStringViaStringBuilderForEachByteToString</td><td>2.8X</td></tr><tr><td>23.69 ticks</td><td>ByteArrayToHexStringViaStringBuilderAggregateByteToString</td><td>2.5X</td></tr><tr><td>25.62 ticks</td><td>ByteArrayToHexStringViaStringConcatArrayConvertAll</td><td>2.4X</td></tr><tr><td>27.92 ticks</td><td>ByteArrayToHexStringViaStringJoinArrayConvertAll</td><td>2.2X</td></tr><tr><td>55.42 ticks</td><td>ByteArrayToHexStringViaStringBuilderForEachAppendFormat</td><td>1.1X</td></tr><tr><td>60.38 ticks</td><td>ByteArrayToHexStringViaStringBuilderAggregateAppendFormat</td><td>1.0X</td></tr></table>

Byte manipulation, while harder to read, is definitely the fastest approach, with the newest version added taking the lead quite significantly over the earlier version. `BitConverter` is second, even with the `.Replace("-", "")` to match its output with the rest. `SoapHexBinary` took over the third place position when it was added, bumping the two `Array.ConvertAll` variants.

##Contributions

If you find something wrong with this stuff or have recommendations for the testing framework, don't hesitate to bring it up. If you have a test idea that would add some value to the world, feel free to write up something that implements `IPerformanceTest` and submit it. New issues and pull requests are always welcome. If you submit it, I will assume you don't mind it becoming part of this project and subject to its MIT license.

##License

[MIT license](http://opensource.org/licenses/MIT). If you do something cool with it, though, I'd love to hear about it.