using System.Collections.Generic;
using System.Linq;
using static du.Ex.ExDictionary;
using System;
using UniRx;
using static du.Ex.ExCollection;

namespace du.Cmp {

    /// <summary>
    /// 順序付き連想配列
    /// - Dicと別にキーのListを持ってるだけ
    /// </summary>
    public class OrderedMap<TKey, TValue> : IOrderedMap<TKey, TValue> where TValue : class {

        #region field
        IList<TKey> Order { get; } = new List<TKey>();
        IDictionary<TKey, TValue> Data { get; } = new Dictionary<TKey, TValue>();
        #endregion

        #region IEnumerable
        /// <summary> 順序付きで走査する </summary>
        public class OMEnumerator : IEnumerator<TValue> {
            IReadOnlyOrderedMap<TKey, TValue> Source { get; }
            int index = -1;

            public OMEnumerator(IReadOnlyOrderedMap<TKey, TValue> source) {
                Source = source;
            }
            public bool MoveNext() {
                if (index > Source.Count) { return false; }
                return Source.Count > ++index;
            }
            public TValue Current => Source.At(index);
            object System.Collections.IEnumerator.Current => Current;
            public void Dispose() {}
            public void Reset() { index = -1; }
        }

        public IEnumerator<TValue> GetEnumerator() => new OMEnumerator(this);
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
        #endregion

        #region IReadOnlyCollection
        public int Count => Order.Count;
        #endregion

        #region IReadOnlyOrderedMap
        /// <returns> 見つからなければ null </returns>
        public TValue At(int i) {
            if (Order.IsValidIndex(i)) { return Data[Order[i]]; }
            else { return null; }
        }
        /// <returns> 見つからなければ null </returns>
        public TValue At(TKey key) => Data.At(key);
        /// <returns> 見つからなければ null </returns>
        public int? IndexOf(TKey key) {
            if (ContainsKey(key)) { return Order.IndexOf(key); }
            else { return null; }
        }
        /// <summary> 該当するキーが登録されているか </summary>
        public bool ContainsKey(TKey key) => Data.ContainsKey(key);
        /// <returns> 空ならtrue </returns>
        public bool IsEmpty => Count == 0;
        /// <returns> 空ならnull </returns>
        public TValue Front => At(0);
        /// <returns> 空ならnull </returns>
        public TValue Back => At(Count - 1);
        #endregion

        #region IOrderedMap
        /// <summary> 要素を末尾に追加 </summary>
        public virtual void Add(TKey key, TValue value) {
            Order.Add(key);
            Data.Add(key, value);
        }
        /// <summary> 要素をindex番目に追加 </summary>
        public virtual void Add(TKey key, TValue value, int index) {
            if (Order.IsValidIndex(index)) {
                Order.Insert(index, key);
            }
            else { Order.Add(key); }
            Data.Add(key, value);
        }
        /// <summary> index番目の要素を削除 </summary>
        public virtual void RemoveAt(int index) {
            if (Order.IsValidIndex(index)) {
                Data.Remove(Order[index]);
                Order.RemoveAt(index);
            }
        }
        /// <summary> keyに対応する要素を削除 </summary>
        public virtual void Remove(TKey key) {
            if (ContainsKey(key)) {
                Order.Remove(key);
                Data.Remove(key);
            }
        }
        /// <summary> リストを空にする </summary>
        public void Clear() {
            Order.Clear();
            Data.Clear();
        }
        #endregion
    }

    /// <summary> 要素の追加/削除/変更時に通知を流す </summary>
    public class RxOrderedMap<TKey, T>
    : OrderedMap<TKey, T>, IRxOrderedMap<TKey, T> where T : class
    {
        #region field
        Subject<T> m_addedStream = new Subject<T>();
        Subject<T> m_removedStream = new Subject<T>();
        // Subject<T> m_changedStream = new Subject<T>();
        #endregion

        #region getter
        public IObservable<T> RxAdded => m_addedStream;
        public IObservable<T> RxRemoved => m_removedStream;
        // public IObservable<T> RxChanged => m_changedStream;
        #endregion

        #region protected
        public override void Add(TKey key, T value) {
            base.Add(key, value);
            m_addedStream.OnNext(value);
        }
        public override void Add(TKey key, T value, int index) {
            base.Add(key, value, index);
            m_addedStream.OnNext(value);
        }
        public override void RemoveAt(int index) {
            if (this.IsValidIndex(index)) {
                m_removedStream.OnNext(At(index));
                base.RemoveAt(index);
            }
        }
        public override void Remove(TKey key) {
            if (ContainsKey(key)) {
                m_removedStream.OnNext(At(key));
                base.Remove(key);
            }
        }
        #endregion
    }

}
