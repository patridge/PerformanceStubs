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

<table><caption>&quot;Converting array of bytes into hexadecimal string representation&quot; (Text (n=1,238,957 bytes), 1000 iterations.)</caption><tr><th>Average</th><th>Method</th><th>Ratio</th></tr><tr><td>35,168.92 ticks</td><td>ByteArrayToHexViaByteManipulation2</td><td>32.8X</td></tr><tr><td>39,684.97 ticks</td><td>ByteArrayToHexViaByteManipulation</td><td>29.1X</td></tr><tr><td>59,394.60 ticks</td><td>ByteArrayToHexViaLookupAndShift</td><td>19.4X</td></tr><tr><td>63,976.69 ticks</td><td>ByteArrayToHexViaLookup</td><td>18.1X</td></tr><tr><td>159,597.60 ticks</td><td>ByteArrayToHexStringViaBitConverter</td><td>7.2X</td></tr><tr><td>354,476.75 ticks</td><td>ByteArrayToHexViaSoapHexBinary</td><td>3.3X</td></tr><tr><td>389,335.34 ticks</td><td>ByteArrayToHexStringViaStringBuilderForEachByteToString</td><td>3.0X</td></tr><tr><td>428,269.23 ticks</td><td>ByteArrayToHexStringViaStringBuilderAggregateByteToString</td><td>2.7X</td></tr><tr><td>923,338.05 ticks</td><td>ByteArrayToHexStringViaStringJoinArrayConvertAll</td><td>1.3X</td></tr><tr><td>939,506.37 ticks</td><td>ByteArrayToHexStringViaStringConcatArrayConvertAll</td><td>1.2X</td></tr><tr><td>1,102,453.15 ticks</td><td>ByteArrayToHexStringViaStringBuilderForEachAppendFormat</td><td>1.0X</td></tr><tr><td>1,154,897.49 ticks</td><td>ByteArrayToHexStringViaStringBuilderAggregateAppendFormat</td><td>1.0X</td></tr></table>

<table><caption>&quot;Converting array of bytes into hexadecimal string representation&quot; (Text (n=61 bytes), 1000 iterations.)</caption><tr><th>Average</th><th>Method</th><th>Ratio</th></tr><tr><td>1.59 ticks</td><td>ByteArrayToHexViaByteManipulation2</td><td>36.9X</td></tr><tr><td>1.74 ticks</td><td>ByteArrayToHexViaByteManipulation</td><td>33.7X</td></tr><tr><td>3.47 ticks</td><td>ByteArrayToHexViaLookupAndShift</td><td>16.9X</td></tr><tr><td>9.11 ticks</td><td>ByteArrayToHexStringViaBitConverter</td><td>6.4X</td></tr><tr><td>9.19 ticks</td><td>ByteArrayToHexViaLookup</td><td>6.4X</td></tr><tr><td>16.79 ticks</td><td>ByteArrayToHexViaSoapHexBinary</td><td>3.5X</td></tr><tr><td>19.14 ticks</td><td>ByteArrayToHexStringViaStringBuilderForEachByteToString</td><td>3.1X</td></tr><tr><td>21.48 ticks</td><td>ByteArrayToHexStringViaStringBuilderAggregateByteToString</td><td>2.7X</td></tr><tr><td>22.34 ticks</td><td>ByteArrayToHexStringViaStringConcatArrayConvertAll</td><td>2.6X</td></tr><tr><td>22.45 ticks</td><td>ByteArrayToHexStringViaStringJoinArrayConvertAll</td><td>2.6X</td></tr><tr><td>53.94 ticks</td><td>ByteArrayToHexStringViaStringBuilderForEachAppendFormat</td><td>1.1X</td></tr><tr><td>58.65 ticks</td><td>ByteArrayToHexStringViaStringBuilderAggregateAppendFormat</td><td>1.0X</td></tr></table>

Byte manipulation, while harder to read, is definitely the fastest approach, with the newest version added taking the lead quite significantly over the earlier version. For the large string, a string lookup array was a speedy second. `BitConverter` is third, even with the `.Replace("-", "")` to match its output with the rest. `SoapHexBinary` took over the fourth place position when it was added, bumping the two `Array.ConvertAll` variants.

##Contributions

If you find something wrong with this stuff or have recommendations for the testing framework, don't hesitate to bring it up. If you have a test idea that would add some value to the world, feel free to write up something that implements `IPerformanceTest` and submit it. New issues and pull requests are always welcome. If you submit it, I will assume you don't mind it becoming part of this project and subject to its MIT license.

##License

[MIT license](http://opensource.org/licenses/MIT). If you do something cool with it, though, I'd love to hear about it.