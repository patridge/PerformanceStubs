namespace PerformanceStubs.Core {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class AssemblyExtensions {
        public static IEnumerable<Type> GetTests(this Assembly assembly, string @namespace = null) {
            // Derived from two StackOverflow questions: http://stackoverflow.com/a/79738/48700 && http://stackoverflow.com/a/949285/48700.
            return assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract && typeof(IPerformanceTest).IsAssignableFrom(t) && (@namespace == null || @namespace.StartsWith(t.Namespace, StringComparison.Ordinal)));
        }
    }
}