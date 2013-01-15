namespace PerformanceStubs.Tests.ListFirstSubtypeItem {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using PerformanceStubs.Core;

    public class Test: PerformanceTestOneInOneOut<IEnumerable<Something>, SubSomething> {
        protected override string Title {
            get {
                return "Getting first subtype item from a list";
            }
        }
        protected override string Caption {
            get {
                return string.Format("For n={0:0,0} (first subtype at 999,900), {1} iterations.", Input1.Count(), Iterations);
            }
        }
        protected override List<Func<IEnumerable<Something>, SubSomething>> TestCandidates {
            get {
                return (new Func<IEnumerable<Something>, SubSomething>[] {
                    FirstOrDefaultAs,
                    SelectAsWhereNotNullFirstOrDefault
                }).ToList();
            }
        }
        public static SubSomething FirstOrDefaultAs(IEnumerable<Something> source) {
            return source.FirstOrDefault(item => item is SubSomething) as SubSomething;
        }
        public static SubSomething SelectAsWhereNotNullFirstOrDefault(IEnumerable<Something> source) {
            return source.Select(item => item as SubSomething).Where(item => item != null).FirstOrDefault();
        }

        private static IEnumerable<Something> GenerateTestInput() {
            List<Something> data = new List<Something>();
            foreach (int i in Enumerable.Range(0, 1000000)) {
                Something item = new Something(i.ToString());
                if (i == 999900) {
                    item = new SubSomething(i.ToString());
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
                return 100;
            }
        }
        protected override bool OutputComparer(SubSomething left, SubSomething right) {
            return left.Id == right.Id;
        }
    }
}
