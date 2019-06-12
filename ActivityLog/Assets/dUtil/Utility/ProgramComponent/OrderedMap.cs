using System.Collections.Generic;
using System.Linq;
using static du.Ex.ExDictionary;
using System;
using UniRx;
using static du.Ex.ExList;

namespace du.Cmp {

    /// <summary>
    /// 順序付き連想配列
    /// Dicと別にキーのListを持ってるだけ
    /// </summary>
    public interface IOrderedMap<T, Key> where T : class {
        /// <returns> 見つからなければ null </returns>
        T At(int i);
        /// <returns> 見つからなければ null </returns>
        T At(Key key);
        /// <summary> 該当するキーが登録されているか </summary>
        bool ContainsKey(Key key);
        /// <summary> ActivityをEnumerableで一括取得 </summary>
        IEnumerable<T> Sorted();

        void Add(Key key, T value);
        void Add(Key key, T value, int index);
    }

    public class OrderedMap<T, Key> : IOrderedMap<T, Key> where T : class {
        #region field
        IList<Key> m_order = new List<Key>();
        IDictionary<Key, T> m_data = new Dictionary<Key, T>();
        #endregion

        #region protected property
        protected IList<Key> Order => m_order;
        protected IDictionary<Key, T> Data => m_data;
        #endregion

        #region public
        /// <returns> 見つからなければ null </returns>
        public T At(int i) {
            if (0 <= i && i < m_data.Count) { return m_data[m_order[i]]; }
            else { return null; }
        }
        /// <returns> 見つからなければ null </returns>
        public T At(Key key) => m_data.At(key);
        /// <summary> 該当するキーが登録されているか </summary>
        public bool ContainsKey(Key key) => m_data.ContainsKey(key);
        /// <summary> ActivityをEnumerableで一括取得 </summary>
        public IEnumerable<T> Sorted() => m_order.Select(name => m_data[name]);

        public virtual void Add(Key key, T value) {
            m_order.Add(key);
            m_data.Add(key, value);
        }
        public virtual void Add(Key key, T value, int index) {
            if (m_order.IsValidIndex(index)) {
                m_order.Insert(index, key);
            }
            else { m_order.Add(key); }
            m_data.Add(key, value);
        }
        #endregion
    }

    /// <summary> 要素の追加/削除/変更時に通知を流す </summary>
    public interface IRxOrderedMap<T, Key> : IOrderedMap<T, Key> where T : class {
        IObservable<T> RxAdded { get; }
        // IObservable<T> RxRemoved { get; }
        // IObservable<T> RxChanged { get; }
    }

    public class RxOrderedMap<T, Key> : OrderedMap<T, Key>, IRxOrderedMap<T, Key> where T : class {
        #region field
        Subject<T> m_addedStream = new Subject<T>();
        // Subject<T> m_removedStream = new Subject<T>();
        // Subject<T> m_changedStream = new Subject<T>();
        #endregion

        #region getter
        public IObservable<T> RxAdded => m_addedStream;
        // public IObservable<T> RxRemoved => m_removedStream;
        // public IObservable<T> RxChanged => m_changedStream;
        #endregion

        #region protected
        public override void Add(Key key, T value) {
            base.Add(key, value);
            m_addedStream.OnNext(value);
        }
        public override void Add(Key key, T value, int index) {
            base.Add(key, value, index);
            m_addedStream.OnNext(value);
        }
        #endregion
    }

}
