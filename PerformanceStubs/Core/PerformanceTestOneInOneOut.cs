namespace PerformanceStubs.Core {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    
    public abstract class PerformanceTestOneInOneOut<TInput1, TResult> : IPerformanceTest {
        protected abstract bool OutputComparer(TResult left, TResult right);
        protected abstract List<Func<TInput1, TResult>> TestCandidates { get; }
        protected abstract TInput1 Input1 { get; }
        protected abstract long Iterations { get; }
        protected abstract string Title { get; }
        protected abstract string Caption { get; }
        
        private PerformanceTestCandidateResult Run(Func<TInput1, TResult> operation, TInput1 victim, TResult expectedOutput, long iterations) {
            TResult testResult = operation(victim); // also primes anything that may be a one-time cost
            Debug.Assert(this.OutputComparer(expectedOutput, testResult));

            List<long> elapsedTicksCollection = new List<long>();
            Stopwatch timer = new Stopwatch();
            for (long i = 1; i <= iterations; i++) {
                timer.Reset();
                timer.Start();
                operation(victim);
                timer.Stop();
                elapsedTicksCollection.Add(timer.ElapsedTicks);
            }
            return new PerformanceTestCandidateResult(operation.Method.Name, elapsedTicksCollection);
        }

        public PerformanceTestSummary Run() {
            List<PerformanceTestCandidateResult> candidateResults = new List<PerformanceTestCandidateResult>();
            PerformanceTestSummary testSummary = new PerformanceTestSummary(this.Title, this.Caption, candidateResults, this.Iterations);
            if (!this.TestCandidates.AnyNotNull()) {
                return testSummary;
            }

            TResult typicalAnswer = this.TestCandidates.First()(this.Input1);
            foreach (Func<TInput1, TResult> testCandidate in this.TestCandidates) {
                PerformanceTestCandidateResult candidateResult = this.Run(testCandidate, this.Input1, typicalAnswer, this.Iterations);
                candidateResults.Add(candidateResult);
            }
            return testSummary;
        }
    }
}
