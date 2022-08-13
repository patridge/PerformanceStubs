namespace PerformanceStubs
{
   using System;
   using System.Collections.Generic;
   using System.IO;
   using System.Linq;
   using System.Management;
   using System.Reflection;
   using System.Text;
   using PerformanceStubs.Core;

   class Program {
       static void Main(string[] args) {
           FluentTagBuilder systemDetails = new FluentTagBuilder("div");
           try {
               // I don't know enough about WMI yet to trust this code running well outside my own machine. Omitting these details if we have any exceptions.
               FluentTagBuilder ul = new FluentTagBuilder("ul");
               foreach (ManagementObject mo in new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor").Get()) {
                   // Gleaned mostly from a StackOverflow question: http://stackoverflow.com/a/2670568/48700.
                   Console.WriteLine(mo["Name"]);
                   Console.WriteLine("\tCores: " + mo["NumberOfCores"]);
                   Console.WriteLine("\tCurrent Clock Speed: " + mo["CurrentClockSpeed"]);
                   Console.WriteLine("\tMax Clock Speed: " + mo["MaxClockSpeed"]);
                   ul.AddChild(new FluentTagBuilder("li", mo["Name"].ToString()).AddChild("ul", new[] { new FluentTagBuilder("li", "Cores: " + mo["NumberOfCores"]),
                                                                                                        new FluentTagBuilder("li", "Current Clock Speed: " + mo["CurrentClockSpeed"]),
                                                                                                        new FluentTagBuilder("li", "Max Clock Speed: " + mo["MaxClockSpeed"]) }));
               }
               systemDetails.AddChild(ul);
           }
           catch {
               systemDetails.AddChild(new FluentTagBuilder("p").WithText("Failed to retrieve system details."));
           }

           IEnumerable<Type> tests = Assembly.GetExecutingAssembly().GetTests();
           List<PerformanceTestSummary> testSummaries = new List<PerformanceTestSummary>();
           foreach (Type testType in tests) {
               IPerformanceTest test = (IPerformanceTest)testType.GetConstructor(Type.EmptyTypes).Invoke((object[])null);
               testSummaries.Add(test.Run());
           }
           List<FluentTagBuilder> resultHtmlContent = new List<FluentTagBuilder>() { systemDetails };
           foreach (PerformanceTestSummary testSummary in testSummaries) {
               FluentTagBuilder table = new FluentTagBuilder("table").AddChild("caption", string.Format("\"{0}\" ({1})", testSummary.Title, testSummary.Caption))
                                                                     .AddChild("tr", new[] { new FluentTagBuilder("th", "Average"),
                                                                                             new FluentTagBuilder("th", "Method"),
                                                                                             new FluentTagBuilder("th", "Ratio") });
               Console.WriteLine("--------------------");
               Console.WriteLine(testSummary.Title);
               Console.WriteLine("--------------------");
               double worstAverage = testSummary.Results.Max(result => result.ElapsedTicksCollection.Average());
               foreach (Tuple<PerformanceTestCandidateResult, double> resultAndAverage in testSummary.Results.Select(result => new Tuple<PerformanceTestCandidateResult, double>(result, result.ElapsedTicksCollection.Average())).OrderBy(tuple => tuple.Item2)) {
                   string ratio = string.Format("{0:0.0}X", worstAverage / resultAndAverage.Item2);
                   table.AddChild("tr", new[] { new FluentTagBuilder("td", string.Format("{0} ticks", resultAndAverage.Item2.ToString("N"))),
                                                new FluentTagBuilder("td", resultAndAverage.Item1.Title),
                                                new FluentTagBuilder("td", ratio) });
                   Console.WriteLine("{0}: {1} average ticks (over {2} runs), {3}", resultAndAverage.Item1.Title, resultAndAverage.Item2.ToString("N"), testSummary.Iterations, ratio);
               }
               resultHtmlContent.Add(table);
           }
           // HTML results used, manually, to generate markup for README.markdown.
           string results = FluentTagBuilder.MakeIntoHtml5Page("Results", resultHtmlContent.ToArray());
           File.WriteAllText(Environment.CurrentDirectory + Path.DirectorySeparatorChar + "output.html", results, Encoding.UTF8);
           Console.ReadKey();
       }
   }
}
