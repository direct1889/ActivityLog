using System.Collections.Generic;
using System;

namespace du.Cmp {

    /// <summary>
    /// 読み取り専用順序付き連想配列
    /// - Dicと別にキーのListを持ってるだけ
    /// </summary>
    public interface IReadOnlyOrderedMap<TKey, TValue> : IReadOnlyCollection<TValue> where TValue : class {
        /// <returns> 見つからなければ null </returns>
        TValue At(int i);
        /// <returns> 見つからなければ null </returns>
        TValue At(TKey key);
        /// <returns> 見つからなければ null </returns>
        int? IndexOf(TKey key);
        /// <summary> 該当するキーが登録されているか </summary>
        bool ContainsKey(TKey key);
        /// <returns> 空ならtrue </returns>
        bool IsEmpty { get; }
        /// <returns> 空ならnull </returns>
        TValue Front { get; }
        /// <returns> 空ならnull </returns>
        TValue Back { get; }
    }

    /// <summary>
    /// 順序付き連想配列
    /// - Dicと別にキーのListを持ってるだけ
    /// </summary>
    public interface IOrderedMap<TKey, TValue> : IReadOnlyOrderedMap<TKey, TValue> where TValue : class {
        /// <summary> 要素を末尾に追加 </summary>
        void Add(TKey key, TValue value);
        /// <summary> 要素をindex番目に追加 </summary>
        void Add(TKey key, TValue value, int index);
        /// <summary>
        /// index番目の要素を削除
        /// - index番目が範囲外の場合は何もしない
        /// </summary>
        void RemoveAt(int index);
        /// <summary>
        /// keyに対応する要素を削除
        /// - keyが存在しない場合は何もしない
        /// </summary>
        void Remove(TKey key);
        /// <summary> リストを空にする </summary>
        void Clear();
    }

    /// <summary> 要素の追加/削除/変更時に通知を流す </summary>
    public interface IRxOrderedMap<TKey, T> : IOrderedMap<TKey, T> where T : class {
        IObservable<T> RxAdded { get; }
        IObservable<T> RxRemoved { get; }
        // IObservable<T> RxChanged { get; }
    }

}
