namespace PerformanceStubs.Tests.GettingObjectPropertiesByName {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    //using PerformanceStubs.Core;
    //using FastMember;
    using System.Dynamic;
    using Microsoft.AspNetCore.Routing;

    public abstract class GetObjectPropertiesByName {
        protected abstract string Title { get; }
        protected string Caption {
            get {
                return string.Format("For {0} iterations.", Iterations);
            }
        }
        //protected List<Func<object, Tuple<string, int?>>> TestCandidates {
        //    get {
        //        return (new Func<object, Tuple<string, int?>>[] {
        //            IDictionaryRouteValueDictionaryLookup,
        //            ObjectAccessorLookup
        //        }).ToList();
        //    }
        //}
        public static Tuple<string, int?> IDictionaryRouteValueDictionaryLookup(object source)
        {
            IDictionary<string, object> dictionarySource = source as IDictionary<string, object> ?? new RouteValueDictionary(source);
            return new Tuple<string, int?>(dictionarySource["Id"] as string, dictionarySource["Value"] as int?);
        }
        public static Tuple<string, int?> ObjectAccessorLookup(object source)
        {
            var wrapped = ObjectAccessor.Create(source);
            return new Tuple<string, int?>(wrapped["Id"] as string, wrapped["Value"] as int?);
        }

        protected abstract object Input1 { get; }
        protected long Iterations {
            get {
                return 100000;
            }
        }
        protected bool OutputComparer(Tuple<string, int?> left, Tuple<string, int?> right) {
            return left.Item1.Equals(right.Item1) && left.Item2.Equals(right.Item2);
        }
    }

    public class TestConcreteType : GetObjectPropertiesByName {
        protected override string Title {
            get {
                return "Getting object properties by name at runtime (concrete type)";
            }
        }
        protected override object Input1 {
            get {
                return new Something() { Id = "testClass", Value = 3 };
            }
        }
    }
    public class TestDynamic : GetObjectPropertiesByName {
        protected override string Title {
            get {
                return "Getting object properties by name at runtime (dynamic ExpandoObject)";
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
    public class TestAnonymousType : GetObjectPropertiesByName {
        protected override string Title {
            get {
                return "Getting object properties by name at runtime (anonymous type)";
            }
        }
        protected override object Input1 {
            get {
                return new { Id = "testAnon", Value = 2 };
            }
        }
    }
}