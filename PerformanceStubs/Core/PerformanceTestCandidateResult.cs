using System.Collections.Generic;
using System.Linq;

namespace PerformanceStubs {
    public class PerformanceTestCandidateResult {
        public string Title { get; set; }
        public List<long> ElapsedTicksCollection { get; set; }
        public double Average {
            get {
                return ElapsedTicksCollection != null ? ElapsedTicksCollection.Average() : 0;
            }
        }
        public double GetRatioOfWorst(double worstAverage) {
            return worstAverage / Average;
        }

        public PerformanceTestCandidateResult(string title, List<long> elapsedTicksCollection) {
            this.Title = title;
            this.ElapsedTicksCollection = elapsedTicksCollection;
        }
    }
}
