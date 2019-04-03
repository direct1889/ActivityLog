using System.Collections.Generic;


namespace du.Ex {

    public static class ExList {

        //! 空の場合はnull
        public static T Back<T>(this IList<T> list) where T : class {
            if (list == null || list.Count == 0) { return null; }
            else { return list[list.Count - 1]; }
        }

    }

    public static class ExDictionary {

        //! 指定したキーが存在しなければAdd、存在すればSet
        public static void AddSet<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, TValue value) {
            if (dic.ContainsKey(key)) { dic[key] = value; }
            else { dic.Add(key, value); }
        }

        //! 指定したキーが存在しなければnullを返すAt
        public static TValue At<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key) where TValue : class {
            return dic.ContainsKey(key) ? dic[key] : null;
        }
        //! 指定したキーが存在しなかった場合の返り値を指定するAt
        public static TValue At<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, TValue defaultValue) {
            return dic.ContainsKey(key) ? dic[key] : defaultValue;
        }

    }

}
