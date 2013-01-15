namespace PerformanceStubs.Tests.GettingObjectPropertiesByName {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using PerformanceStubs.Core;
    using System.Web.Routing;
    using FastMember;
    using System.Dynamic;

    public abstract class Test : PerformanceTestOneInOneOut<object, Tuple<string, int?>> {
        protected override string Title {
            get {
                return "Getting object properties by name at runtime";
            }
        }
        protected override string Caption {
            get {
                return string.Format("For {0} iterations.", Iterations);
            }
        }
        protected override List<Func<object, Tuple<string, int?>>> TestCandidates {
            get {
                return (new Func<object, Tuple<string, int?>>[] {
                    IDictionaryRouteValueDictionaryLookup,
                    ObjectAccessorLookup
                }).ToList();
            }
        }
        public static Tuple<string, int?> IDictionaryRouteValueDictionaryLookup(object source) {
            IDictionary<string, object> dictionarySource = source as IDictionary<string, object> ?? new RouteValueDictionary(source);
            return new Tuple<string, int?>(dictionarySource["Id"] as string, dictionarySource["Value"] as int?);
        }
        public static Tuple<string, int?> ObjectAccessorLookup(object source) {
            var wrapped = ObjectAccessor.Create(source);
            return new Tuple<string, int?>(wrapped["Id"] as string, wrapped["Value"] as int?);
        }

        protected abstract override object Input1 { get; }
        protected override long Iterations {
            get {
                return 100000;
            }
        }
        protected override bool OutputComparer(Tuple<string, int?> left, Tuple<string, int?> right) {
            return left.Item1.Equals(right.Item1) && left.Item2.Equals(right.Item2);
        }
    }

    public class TestConcreteType : Test {
        protected override string Title {
            get {
                return base.Title + " (concrete type)";
            }
        }
        protected override object Input1 {
            get {
                return new Something() { Id = "testClass", Value = 3 };
            }
        }
    }
    public class TestDynamic : Test {
        protected override string Title {
            get {
                return base.Title + " (dynamic ExpandoObject)";
            }
        }
        protected override object Input1 {
            get {
                dynamic input = new ExpandoObject();
                input.Id = "testExpando";
                input.Value = 1;
                return input;
            }
        }
    }
    public class TestAnonymousType : Test {
        protected override string Title {
            get {
                return base.Title + " (anonymous type)";
            }
        }
        protected override object Input1 {
            get {
                return new { Id = "testAnon", Value = 2 };
            }
        }
    }
}