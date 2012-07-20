namespace PerformanceStubs {
    using System.Collections.Generic;

    public class PerformanceTestCandidateResult {
        public string Title { get; set; }
        public List<long> ElapsedTicksCollection { get; set; }

        public PerformanceTestCandidateResult(string title, List<long> elapsedTicksCollection) {
            this.Title = title;
            this.ElapsedTicksCollection = elapsedTicksCollection;
        }
    }
}
