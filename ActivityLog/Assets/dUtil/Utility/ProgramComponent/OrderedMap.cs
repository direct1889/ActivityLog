using System.Collections.Generic;
using System.Linq;
using static du.Ex.ExDictionary;
using System;
using UniRx;

namespace du.Cmp {

    /// <summary>
    /// 順序付き連想配列
    /// Dicと別にキーのListを持ってるだけ
    /// </summary>
    public interface IOrderedMap<T> where T : class {
        /// <returns> 見つからなければ null </returns>
        T At(int i);
        /// <returns> 見つからなければ null </returns>
        T At(string key);
        /// <summary> 該当するキーが登録されているか </summary>
        bool ContainsKey(string key);
        /// <summary> ActivityをEnumerableで一括取得 </summary>
        IEnumerable<T> Sorted();
    }

    public class OrderedMap<T> : IOrderedMap<T> where T : class {
        #region field
        IList<string> m_order = new List<string>();
        IDictionary<string, T> m_data = new Dictionary<string, T>();
        #endregion

        #region protected property
        protected IList<string> Order => m_order;
        protected IDictionary<string, T> Data => m_data;
        #endregion

        #region public
        /// <returns> 見つからなければ null </returns>
        public T At(int i) {
            if (0 <= i && i < m_data.Count) { return m_data[m_order[i]]; }
            else { return null; }
        }
        /// <returns> 見つからなければ null </returns>
        public T At(string key) => m_data.At(key);
        /// <summary> 該当するキーが登録されているか </summary>
        public bool ContainsKey(string key) => m_data.ContainsKey(key);
        /// <summary> ActivityをEnumerableで一括取得 </summary>
        public IEnumerable<T> Sorted() => m_order.Select(name => m_data[name]);
        #endregion

        #region protected
        protected virtual void Add(string key, T value) {
            m_order.Add(key);
            m_data.Add(key, value);
        }
        #endregion
    }

    /// <summary> 要素の追加/削除/変更時に通知を流す </summary>
    public interface IRxOrderedMap<T> : IOrderedMap<T> where T : class {
        IObservable<T> RxAdded { get; }
        IObservable<T> RxRemoved { get; }
        IObservable<T> RxChanged { get; }
    }

    public class RxOrderedMap<T> : OrderedMap<T>, IRxOrderedMap<T> where T : class {
        #region field
        Subject<T> m_addedStream = new Subject<T>();
        Subject<T> m_removedStream = new Subject<T>();
        Subject<T> m_changedStream = new Subject<T>();
        #endregion

        #region getter
        public IObservable<T> RxAdded => m_addedStream;
        public IObservable<T> RxRemoved => m_removedStream;
        public IObservable<T> RxChanged => m_changedStream;
        #endregion

        #region protected
        protected override void Add(string key, T value) {
            Add(key, value);
            m_addedStream.OnNext(value);
        }
        #endregion
    }

}
