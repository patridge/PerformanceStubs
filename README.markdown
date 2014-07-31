#Performance Stubs

For a while, I was writing blog posts around identifing the fastest method for accomplishing particular goals. They can be found on my blog under the ["performance stub" tag](http://www.patridgedev.com/tag/performance-stub/). This is the repository where I will collect all of the test code along with the framework I used to run the tests.

##Background

As I code, I like to make some light notes of alternatives while driving forward with the first implementation that makes it from my brain to my fingers. When I get the chance, I can go back and flesh out the two versions and drop them into some basic Stopwatch timing to determine which is better in terms of raw speed. Factoring those results with clarity of code, I have a method I will likely choose the next time I need the same feature.

##Disclaimer

Don't take these test results as any end-all answer. These are the numbers I got on some random computer I was using at the time. Feel free to use them as a starting point. If you are really digging into performance, you already know that everything can change once you put the code in question in its natural surroundings (e.g., Windows .NET vs. MonoTouch) or put it under load.

###Last Run

<div><ul><li>Intel(R) Core(TM) i5 CPU         760  @ 2.80GHz<ul><li>Cores: 4</li><li>Current Clock Speed: 2793</li><li>Max Clock Speed: 2793</li></ul></li></ul></div>

##Tests (so far)

###Getting object properties by name at runtime (concrete type)

<table><caption>&quot;Getting object properties by name at runtime (concrete type)&quot; (For 100000 iterations.)</caption><tr><th>Average</th><th>Method</th><th>Ratio</th></tr><tr><td>1.03 ticks</td><td>ObjectAccessorLookup</td><td>10.5X</td></tr><tr><td>10.82 ticks</td><td>IDictionaryRouteValueDictionar<WBR>yLookup</td><td>1.0X</td></tr></table>

###Getting object properties by name at runtime (dynamic ExpandoObject)

<table><caption>&quot;Getting object properties by name at runtime (dynamic ExpandoObject)&quot; (For 100000 iterations.)</caption><tr><th>Average</th><th>Method</th><th>Ratio</th></tr><tr><td>0.78 ticks</td><td>IDictionaryRouteValueDictionar<WBR>yLookup</td><td>1.6X</td></tr><tr><td>1.26 ticks</td><td>ObjectAccessorLookup</td><td>1.0X</td></tr></table>

###Getting object properties by name at runtime (anonymous type)

<table><caption>&quot;Getting object properties by name at runtime (anonymous type)&quot; (For 100000 iterations.)</caption><tr><th>Average</th><th>Method</th><th>Ratio</th></tr><tr><td>1.03 ticks</td><td>ObjectAccessorLookup</td><td>12.3X</td></tr><tr><td>12.65 ticks</td><td>IDictionaryRouteValueDictionar<WBR>yLookup</td><td>1.0X</td></tr></table>

###Getting first subtype item from a list

<table><caption>&quot;Getting first subtype item from a list&quot; (For n=1,000,000 (first subtype at 999,900), 100 iterations.)</caption><tr><th>Average</th><th>Method</th><th>Ratio</th></tr><tr><td>62,489.38 ticks</td><td>FirstOrDefaultAs</td><td>1.4X</td></tr><tr><td>86,723.12 ticks</td><td>SelectAsWhereNotNullFirstOrDef<WBR>ault</td><td>1.0X</td></tr></table>

###Getting all subtype items from a list

<table><caption>&quot;Getting all subtype items from a list&quot; (For n=1,000,000 (half items are subtype), 1000 iterations.)</caption><tr><th>Average</th><th>Method</th><th>Ratio</th></tr><tr><td>0.55 ticks</td><td>SelectAsWhereNotNull</td><td>2.0X</td></tr><tr><td>1.12 ticks</td><td>WhereIsCast</td><td>1.0X</td></tr></table>

###[Converting a byte array to a hexadecimal string](http://stackoverflow.com/a/624379/48700)

<table><caption>&quot;Converting array of bytes into hexadecimal string representation&quot; (Text (n=1,238,958 bytes), 1000 iterations.)</caption><tr><th>Average</th><th>Method</th><th>Ratio</th></tr><tr><td>44,831.45 ticks</td><td>ByteArrayToHexViaLookupPerByte</td><td>16.8X</td></tr><tr><td>48,303.33 ticks</td><td>ByteArrayToHexViaByteManipulation2</td><td>15.6X</td></tr><tr><td>61,927.58 ticks</td><td>ByteArrayToHexViaLookup</td><td>12.1X</td></tr><tr><td>78,086.87 ticks</td><td>ByteArrayToHexViaByteManipulation</td><td>9.6X</td></tr><tr><td>82,795.12 ticks</td><td>ByteArrayToHexViaLookupAndShift</td><td>9.1X</td></tr><tr><td>135,130.89 ticks</td><td>ByteArrayToHexStringViaBitConverter</td><td>5.6X</td></tr><tr><td>279,660.83 ticks</td><td>ByteArrayToHexViaSoapHexBinary</td><td>2.7X</td></tr><tr><td>308,805.38 ticks</td><td>ByteArrayToHexStringViaStringBuilderForEachByteToString</td><td>2.4X</td></tr><tr><td>352,828.20 ticks</td><td>ByteArrayToHexStringViaStringBuilderAggregateByteToString</td><td>2.1X</td></tr><tr><td>672,115.77 ticks</td><td>ByteArrayToHexStringViaStringBuilderForEachAppendFormat</td><td>1.1X</td></tr><tr><td>675,451.57 ticks</td><td>ByteArrayToHexStringViaStringJoinArrayConvertAll</td><td>1.1X</td></tr><tr><td>718,380.63 ticks</td><td>ByteArrayToHexStringViaStringBuilderAggregateAppendFormat</td><td>1.0X</td></tr><tr><td>752,078.70 ticks</td><td>ByteArrayToHexStringViaStringConcatArrayConvertAll</td><td>1.0X</td></tr></table>

<table><caption>&quot;Converting array of bytes into hexadecimal string representation&quot; (Text (n=61 bytes), 1000 iterations.)</caption><tr><th>Average</th><th>Method</th><th>Ratio</th></tr><tr><td>2.03 ticks</td><td>ByteArrayToHexViaLookupPerByte</td><td>19.5X</td></tr><tr><td>2.26 ticks</td><td>ByteArrayToHexViaByteManipulat<WBR>ion2</td><td>17.6X</td></tr><tr><td>3.79 ticks</td><td>ByteArrayToHexViaByteManipulat<WBR>ion</td><td>10.5X</td></tr><tr><td>4.09 ticks</td><td>ByteArrayToHexViaLookupAndShif<WBR>t</td><td>9.7X</td></tr><tr><td>6.18 ticks</td><td>ByteArrayToHexViaLookup</td><td>6.4X</td></tr><tr><td>7.83 ticks</td><td>ByteArrayToHexStringViaBitConv<WBR>erter</td><td>5.1X</td></tr><tr><td>12.50 ticks</td><td>ByteArrayToHexViaSoapHexBinary</td><td>3.2X</td></tr><tr><td>16.87 ticks</td><td>ByteArrayToHexStringViaStringB<WBR>uilderAggregateByteToString</td><td>2.4X</td></tr><tr><td>16.89 ticks</td><td>ByteArrayToHexStringViaStringB<WBR>uilderForEachByteToString</td><td>2.4X</td></tr><tr><td>17.95 ticks</td><td>ByteArrayToHexStringViaStringJ<WBR>oinArrayConvertAll</td><td>2.2X</td></tr><tr><td>18.28 ticks</td><td>ByteArrayToHexStringViaStringC<WBR>oncatArrayConvertAll</td><td>2.2X</td></tr><tr><td>36.82 ticks</td><td>ByteArrayToHexStringViaStringB<WBR>uilderForEachAppendFormat</td><td>1.1X</td></tr><tr><td>39.71 ticks</td><td>ByteArrayToHexStringViaStringB<WBR>uilderAggregateAppendFormat</td><td>1.0X</td></tr></table>

Lookup tables have taken the lead over byte manipulation. Basically, there is some form of precomputing what any given nibble or byte will be in hex. Then, as you rip through the data, you simply look up the next portion to see what hex string it would be. That value is then added to the resulting string output in some fashion. For a long time byte manipulation, potentially harder to read by some developers, was the top-performing approach.

Your best bet is still going to be finding some representative data and trying it out in a production-like environment. If you have different memory constraints, you may prefer a method with fewer allocations to one that would be faster but consume more memory.

##Contributions

If you find something wrong with this stuff or have recommendations for the testing framework, don't hesitate to bring it up. If you have a test idea that would add some value to the world, feel free to write up something that implements `IPerformanceTest` and submit it. New issues and pull requests are always welcome. If you submit it, I will assume you don't mind it becoming part of this project and subject to its MIT license.

##License

[MIT license](http://opensource.org/licenses/MIT). If you do something cool with it, though, I'd love to hear about it.
