using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Reflection;
using PerformanceStubs.Core;
using System.IO;
using System.Text;

namespace PerformanceStubs {
    class Program {
        class TestSuiteRunResults {
            public SystemInfo SystemInfo { get; set; }
            public List<string> Errors { get; set; }
            public List<PerformanceTestSummary> TestRuns { get; set; }

            public TestSuiteRunResults() {
                SystemInfo = new SystemInfo();
                Errors = new List<string>();
                TestRuns = new List<PerformanceTestSummary>();
            }
        }
        class SystemInfo {
            public string ProcessorName { get; set; }
            public string Cores { get; set; }
            public string CurrentClockSpeed { get; set; }
            public string MaxClockSpeed { get; set; }
            public override string ToString() {
                return string.Format("{0}\n\tCores: {1}\n\tCurrent Clock Speed: {2}\n\tMax Clock Speed: {3}", ProcessorName, Cores, CurrentClockSpeed, MaxClockSpeed);
            }
        }
        static void Main(string[] args) {
            TestSuiteRunResults results = new TestSuiteRunResults();
            FluentTagBuilder systemDetails = new FluentTagBuilder("div");
            try {
                FluentTagBuilder ul = new FluentTagBuilder("ul");
                // I don't know enough about WMI yet to trust this code running well outside my own machine. Omitting these details if we have any exceptions.
                // NOTE: Totally fails on Mono for totally expected reasons.
                foreach (ManagementObject mo in new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor").Get()) {
                    // Gleaned mostly from a StackOverflow question: http://stackoverflow.com/a/2670568/48700.
                    results.SystemInfo.ProcessorName = mo["Name"].ToString();
                    results.SystemInfo.Cores = mo["NumberOfCores"].ToString();
                    results.SystemInfo.CurrentClockSpeed = mo["CurrentClockSpeed"].ToString();
                    results.SystemInfo.MaxClockSpeed = mo["MaxClockSpeed"].ToString();

                    Console.WriteLine(results.SystemInfo.ToString());
                    ul.AddChild(new FluentTagBuilder("li", mo["Name"].ToString()).AddChild("ul", new[] { new FluentTagBuilder("li", "Cores: " + mo["NumberOfCores"]),
                                                                                                         new FluentTagBuilder("li", "Current Clock Speed: " + mo["CurrentClockSpeed"]),
                                                                                                         new FluentTagBuilder("li", "Max Clock Speed: " + mo["MaxClockSpeed"]) }));
                }
                systemDetails.AddChild(ul);
            }
            catch {
                string errorMsg = "Failed to retrieve system details.";
                results.Errors.Add(errorMsg);
                Console.WriteLine(errorMsg);
                systemDetails.AddChild(new FluentTagBuilder("p").WithText("Failed to retrieve system details."));
            }

            IEnumerable<Type> tests = Assembly.GetExecutingAssembly().GetTests();
            List<PerformanceTestSummary> testSummaries = new List<PerformanceTestSummary>();
            results.TestRuns.AddRange(testSummaries);
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
                double worstAverage = testSummary.WorstAverage;
                foreach (var candidateResultAndRatio in testSummary.Results.Select(result => new { Ratio = result.GetRatioOfWorst(worstAverage), Result = result }).OrderByDescending(resultAndRatio => resultAndRatio.Ratio)) {
                    string ratio = string.Format("{0:0.0}X", candidateResultAndRatio.Ratio);
                    var candidateResult = candidateResultAndRatio.Result;
                    table.AddChild("tr", new[] { new FluentTagBuilder("td", string.Format("{0} ticks", candidateResult.Average.ToString("N"))),
                                                 new FluentTagBuilder("td", candidateResult.Title),
                                                 new FluentTagBuilder("td", ratio) });
                    Console.WriteLine("{0}: {1} average ticks (over {2} runs), {3}", candidateResult.Title, candidateResult.Average.ToString("N"), testSummary.Iterations, ratio);
                }
                resultHtmlContent.Add(table);
            }
            // HTML results used, manually, to generate markup for README.markdown.
            string htmlResults = FluentTagBuilder.MakeIntoHtml5Page("Results", resultHtmlContent.ToArray());
            File.WriteAllText(Environment.CurrentDirectory + Path.DirectorySeparatorChar + "output.html", htmlResults, Encoding.UTF8);
            Console.ReadKey();
        }
    }
}
