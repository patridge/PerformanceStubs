# Performance Stubs

For a while, I was writing blog posts around identifing the fastest method for accomplishing particular goals. They can be found on my blog under the ["performance stub" tag](http://www.patridgedev.com/tag/performance-stub/). This is the repository where I will collect all of the test code along with the framework I used to run the tests.

## Background

As I code, I like to make some light notes of alternatives while driving forward with the first implementation that makes it from my brain to my fingers. When I get the chance, I can go back and flesh out the two versions and drop them into some basic Stopwatch timing to determine which is better in terms of raw speed. Factoring those results with clarity of code, I have a method I will likely choose the next time I need the same feature.

## Disclaimer

Don't take these test results as any end-all answer. These are the numbers I got on some random computer I was using at the time. Feel free to use them as a starting point. If you are really digging into performance, you already know that everything can change once you put the code in question in its natural surroundings (e.g., Windows .NET vs. MonoTouch) or put it under load.

### Last Run

<div><ul><li>Intel(R) Core(TM) i7-4850HQ CPU @ 2.30GHz [NOTE: Windows 8.1 running under VMware Fusion]<ul><li>Cores: 4</li><li>Current Clock Speed: 2295</li><li>Max Clock Speed: 2295</li></ul></li></ul></div>

## Tests (so far)

### Getting object properties by name at runtime (concrete type)

<table><caption>&quot;Getting object properties by name at runtime (concrete type)&quot; (For 100000 iterations.)</caption><tr><th>Average</th><th>Method</th><th>Ratio</th></tr><tr><td>0.42 ticks</td><td>ObjectAccessorLookup</td><td>12.4X</td></tr><tr><td>5.26 ticks</td><td>IDictionaryRouteValueDictionaryLookup</td><td>1.0X</td></tr></table>

### Getting object properties by name at runtime (dynamic ExpandoObject)

<table><caption>&quot;Getting object properties by name at runtime (dynamic ExpandoObject)&quot; (For 100000 iterations.)</caption><tr><th>Average</th><th>Method</th><th>Ratio</th></tr><tr><td>0.23 ticks</td><td>IDictionaryRouteValueDictionaryLookup</td><td>1.9X</td></tr><tr><td>0.43 ticks</td><td>ObjectAccessorLookup</td><td>1.0X</td></tr></table>

### Getting object properties by name at runtime (anonymous type)

<table><caption>&quot;Getting object properties by name at runtime (anonymous type)&quot; (For 100000 iterations.)</caption><tr><th>Average</th><th>Method</th><th>Ratio</th></tr><tr><td>0.44 ticks</td><td>ObjectAccessorLookup</td><td>13.1X</td></tr><tr><td>5.72 ticks</td><td>IDictionaryRouteValueDictionaryLookup</td><td>1.0X</td></tr></table>

### Getting first subtype item from a list

<table><caption>&quot;Getting first subtype item from a list&quot; (For n=1,000,000 (first subtype at 999,900), 100 iterations.)</caption><tr><th>Average</th><th>Method</th><th>Ratio</th></tr><tr><td>44,991.67 ticks</td><td>SimpleLoop</td><td>1.6X</td></tr><tr><td>47,769.28 ticks</td><td>FirstOrDefaultAs</td><td>1.5X</td></tr><tr><td>61,196.91 ticks</td><td>OfTypeFirstOrDefault</td><td>1.2X</td></tr><tr><td>69,718.62 ticks</td><td>SelectAsFirstOrDefaultNotNull</td><td>1.0X</td></tr><tr><td>71,449.59 ticks</td><td>SelectAsWhereNotNullFirstOrDefault</td><td>1.0X</td></tr></table>

### Getting all subtype items from a list

<table><caption>&quot;Getting all subtype items from a list&quot; (For n=1,000,000 (half items are subtype), 100 iterations.)</caption><tr><th>Average</th><th>Method</th><th>Ratio</th></tr><tr><td>73,176.10 ticks</td><td>YieldReturnLoop</td><td>1.4X</td></tr><tr><td>88,929.40 ticks</td><td>WhereIsCast</td><td>1.1X</td></tr><tr><td>89,636.05 ticks</td><td>OfType</td><td>1.1X</td></tr><tr><td>99,422.73 ticks</td><td>SelectAsWhereNotNull</td><td>1.0X</td></tr></table>

### [Converting a byte array to a hexadecimal string](http://stackoverflow.com/a/624379/48700)

<table><caption>&quot;Converting array of bytes into hexadecimal string representation&quot; (Text (n=1,214,268 bytes), 1000 iterations.)</caption><tr><th>Average</th><th>Method</th><th>Ratio</th></tr><tr><td>4,727.85 ticks</td><td>ByteArrayToHexViaLookup32UnsafeDirect</td><td>105.2X</td></tr><tr><td>10,853.96 ticks</td><td>ByteArrayToHexViaLookupPerByte</td><td>45.8X</td></tr><tr><td>12,967.69 ticks</td><td>ByteArrayToHexViaByteManipulation2</td><td>38.4X</td></tr><tr><td>16,846.64 ticks</td><td>ByteArrayToHexViaByteManipulation</td><td>29.5X</td></tr><tr><td>23,201.23 ticks</td><td>ByteArrayToHexViaLookupAndShift</td><td>21.4X</td></tr><tr><td>23,879.41 ticks</td><td>ByteArrayToHexViaLookup</td><td>20.8X</td></tr><tr><td>113,269.34 ticks</td><td>ByteArrayToHexStringViaBitConverter</td><td>4.4X</td></tr><tr><td>178,601.39 ticks</td><td>ByteArrayToHexViaSoapHexBinary</td><td>2.8X</td></tr><tr><td>203,871.66 ticks</td><td>ByteArrayToHexStringViaStringBuilderForEachByteToString</td><td>2.4X</td></tr><tr><td>227,942.39 ticks</td><td>ByteArrayToHexStringViaStringBuilderAggregateByteToString</td><td>2.2X</td></tr><tr><td>452,639.34 ticks</td><td>ByteArrayToHexStringViaStringBuilderForEachAppendFormat</td><td>1.1X</td></tr><tr><td>479,832.66 ticks</td><td>ByteArrayToHexStringViaStringBuilderAggregateAppendFormat</td><td>1.0X</td></tr><tr><td>484,575.84 ticks</td><td>ByteArrayToHexStringViaStringConcatArrayConvertAll</td><td>1.0X</td></tr><tr><td>497,343.99 ticks</td><td>ByteArrayToHexStringViaStringJoinArrayConvertAll</td><td>1.0X</td></tr></table>

<table><caption>&quot;Converting array of bytes into hexadecimal string representation&quot; (Text (n=61 bytes), 1000 iterations.)</caption><tr><th>Average</th><th>Method</th><th>Ratio</th></tr><tr><td>0.28 ticks</td><td>ByteArrayToHexViaLookup32UnsafeDirect</td><td>99.7X</td></tr><tr><td>0.65 ticks</td><td>ByteArrayToHexViaLookupPerByte</td><td>42.7X</td></tr><tr><td>0.70 ticks</td><td>ByteArrayToHexViaByteManipulation</td><td>39.5X</td></tr><tr><td>0.73 ticks</td><td>ByteArrayToHexViaByteManipulation2</td><td>37.9X</td></tr><tr><td>1.15 ticks</td><td>ByteArrayToHexViaLookup</td><td>23.9X</td></tr><tr><td>1.24 ticks</td><td>ByteArrayToHexViaLookupAndShift</td><td>22.3X</td></tr><tr><td>9.98 ticks</td><td>ByteArrayToHexStringViaBitConverter</td><td>2.8X</td></tr><tr><td>9.98 ticks</td><td>ByteArrayToHexStringViaStringBuilderForEachByteToString</td><td>2.8X</td></tr><tr><td>10.68 ticks</td><td>ByteArrayToHexViaSoapHexBinary</td><td>2.6X</td></tr><tr><td>14.27 ticks</td><td>ByteArrayToHexStringViaStringBuilderAggregateByteToString</td><td>1.9X</td></tr><tr><td>14.88 ticks</td><td>ByteArrayToHexStringViaStringJoinArrayConvertAll</td><td>1.8X</td></tr><tr><td>23.74 ticks</td><td>ByteArrayToHexStringViaStringConcatArrayConvertAll</td><td>1.2X</td></tr><tr><td>24.93 ticks</td><td>ByteArrayToHexStringViaStringBuilderAggregateAppendFormat</td><td>1.1X</td></tr><tr><td>27.51 ticks</td><td>ByteArrayToHexStringViaStringBuilderForEachAppendFormat</td><td>1.0X</td></tr></table><table>

Lookup tables have taken the lead over byte manipulation, especially if you are willing to play in the unsafe realm. Basically, there is some form of precomputing what any given nibble or byte will be in hex. Then, as you rip through the data, you simply look up the next portion to see what hex string it would be. That value is then added to the resulting string output in some fashion. For a long time byte manipulation, potentially harder to read by some developers, was the top-performing approach.

Your best bet is still going to be finding some representative data and trying it out in a production-like environment. If you have different memory constraints, you may prefer a method with fewer allocations to one that would be faster but consume more memory.

## Contributions

If you find something wrong with this stuff or have recommendations for the testing framework, don't hesitate to bring it up. If you have a test idea that would add some value to the world, feel free to write up something that implements `IPerformanceTest` and submit it. New issues and pull requests are always welcome. If you submit it, I will assume you don't mind it becoming part of this project and subject to its MIT license.

## License

[MIT license](http://opensource.org/licenses/MIT). If you do something cool with it, though, I'd love to hear about it.
