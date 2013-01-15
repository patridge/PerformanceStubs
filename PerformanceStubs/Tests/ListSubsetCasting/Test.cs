namespace PerformanceStubs.Tests.ListSubsetCasting {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using PerformanceStubs.Core;

    public class Test: PerformanceTestOneInOneOut<IEnumerable<Something>, IEnumerable<SubSomething>> {
        protected override string Title {
            get {
                return "Getting all subtype items from a list";
            }
        }
        protected override string Caption {
            get {
                return string.Format("For n={0:0,0} (half items are subtype), {1} iterations.", Input1.Count(), Iterations);
            }
        }
        protected override List<Func<IEnumerable<Something>, IEnumerable<SubSomething>>> TestCandidates {
            get {
                return (new Func<IEnumerable<Something>, IEnumerable<SubSomething>>[] {
                    SelectAsWhereNotNull,
                    WhereIsCast
                }).ToList();
            }
        }
        public static IEnumerable<SubSomething> SelectAsWhereNotNull(IEnumerable<Something> source) {
            return source.Select(item => item as SubSomething).Where(item => item != null);
        }
        public static IEnumerable<SubSomething> WhereIsCast(IEnumerable<Something> source) {
            return source.Where(item => item is SubSomething).Cast<SubSomething>();
        }

        private static IEnumerable<Something> GenerateTestInput() {
            List<Something> data = new List<Something>();
            foreach (int i in Enumerable.Range(0, 1000000)) {
                Something item = new Something();
                if (i % 2 == 0) {
                    item = new SubSomething();
                }
                data.Add(item);
            }
            return data;
        }
        private IEnumerable<Something> _Intput1 = null;
        protected override IEnumerable<Something> Input1 {
            get {
                return _Intput1 ?? (_Intput1 = GenerateTestInput());
            }
        }
        protected override long Iterations {
            get {
                return 1000;
            }
        }
        protected override bool OutputComparer(IEnumerable<SubSomething> left, IEnumerable<SubSomething> right) {
            int leftCount = left.Count();
            if (leftCount != right.Count()) {
                return false;
            }

            // RESEARCH: May be able to try default EqualityComparer for IEnumerable<SubSomething> directly.
            SubSomething[] leftArray = left.ToArray();
            SubSomething[] rightArray = right.ToArray();
            for (int i = 0; i < leftCount; i++) {
                if (!EqualityComparer<SubSomething>.Default.Equals(leftArray[i], rightArray[i])) {
                    return false;
                }
            }
            return true;
        }
    }
}
