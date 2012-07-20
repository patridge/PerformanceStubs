namespace PerformanceStubs {
    using System.Collections.Generic;

    public class PerformanceTestSummary {
        public string Title { get; set; }
        public string Caption { get; set; }
        public List<PerformanceTestCandidateResult> Results { get; set; }
        public long Iterations { get; set; }

        public PerformanceTestSummary(string title, string caption, List<PerformanceTestCandidateResult> results, long iterations) {
            this.Title = title;
            this.Caption = caption;
            this.Results = results;
            this.Iterations = iterations;
        }
    }
}
