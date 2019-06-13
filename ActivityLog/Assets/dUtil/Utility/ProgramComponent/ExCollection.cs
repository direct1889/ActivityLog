using System.Collections.Generic;


namespace du.Ex {

    public static class ExCollection {

        public static bool IsValidIndex<T>(this ICollection<T> list, int index) {
            return !(list is null) && 0 <= index && index < list.Count;
        }
        public static bool IsValidIndex<T>(this IReadOnlyCollection<T> list, int index) {
            return !(list is null) && 0 <= index && index < list.Count;
        }

        public static bool IsEmpty<T>(this ICollection<T> list) {
            return list is null || list.Count == 0;
        }
        public static bool IsEmpty<T>(this IReadOnlyCollection<T> list) {
            return list is null || list.Count == 0;
        }

    }

    public static class ExList {

        /// <returns> listが空の場合はnull </returns>
        public static T Back<T>(this IList<T> list) where T : class {
            if (list.IsEmpty()) { return null; }
            else { return list[list.Count - 1]; }
        }

    }

    public static class ExDictionary {

        /// <summary> 指定したキーが存在しなければAdd、存在すればSet </summary>
        public static void AddSet<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, TValue value) {
            if (dic.ContainsKey(key)) { dic[key] = value; }
            else { dic.Add(key, value); }
        }

        /// <summary> 指定したキーが存在しなければnullを返すAt </summary>
        public static TValue At<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key) where TValue : class {
            return dic.ContainsKey(key) ? dic[key] : null;
        }
        /// <summary> 指定したキーが存在しなかった場合の返り値を指定するAt </summary>
        public static TValue At<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, TValue defaultValue) {
            return dic.ContainsKey(key) ? dic[key] : defaultValue;
        }

    }

}
