using System.Collections.Generic;
using System.Linq;


namespace du.Test {

    public static class ExMiscLogger {

        public static string dLog<T1, T2>(this KeyValuePair<T1, T2> pair) {
            return $"<{pair.Key.ToString()}; {pair.Value.ToString()}>";
        }

        public static string dLog<T1, T2>(this IEnumerable<KeyValuePair<T1, T2>> dic) {
            string ret = "";
            foreach (var ipair in dic) {
                ret += $"{ipair.dLog()}\n";
            }
            return ret;
        }
        public static string dLog<T>(this IEnumerable<T> enumerable) {
            string ret = "";
            foreach (var i in enumerable) {
                ret += $"{i.ToString()}\n";
            }
            return ret;
        }
        public static string dLogIsNull(this object obj) {
            return obj is null ? " is null." : " is not null.";
        }

    }

}
