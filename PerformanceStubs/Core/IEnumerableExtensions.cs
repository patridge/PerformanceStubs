namespace PerformanceStubs.Core {
    using System.Collections.Generic;
    using System.Linq;

    public static class IEnumerableExtensions {
        public static bool AnyNotNull<T>(this IEnumerable<T> source) {
            return source != null && source.Any();
        }
    }
}
