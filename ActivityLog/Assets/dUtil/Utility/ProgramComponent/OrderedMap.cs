
using System.Collections.Generic;
using UnityEngine;
using UColor = UnityEngine.Color;
using System.Linq;

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
        /// <summary> ActivityをEnumerableで一括取得 </summary>
        IEnumerable<T> Sorted();
    }

    public class OrderedMap<T> : IOrderedMap<T> where T : class {
        #region field
        IList<string> m_order = null;
        IDictionary<string, T> m_data = null;
        #endregion

        #region protected property
        protected IList<string> Order { get { return m_order; } }
        protected IDictionary<string, T> Data { get { return m_data; } }
        #endregion

        #region ctor/dtor
        public OrderedMap() {
            m_order = new List<string>();
            m_data = new Dictionary<string, T>();
        }
        #endregion

        #region public
        /// <returns> 見つからなければ null </returns>
        public T At(int i) {
            if (0 <= i && i < m_data.Count) { return m_data[m_order[i]]; }
            else { return null; }
        }
        /// <returns> 見つからなければ null </returns>
        public T At(string key) {
            if (m_data.ContainsKey(key)) { return m_data[key]; }
            else { return null; }
        }
        /// <summary> ActivityをEnumerableで一括取得 </summary>
        public IEnumerable<T> Sorted() {
            return m_order.Select(name => m_data[name]);
        }
        #endregion

        #region protected
        protected void Add(string key, T value) {
            m_order.Add(key);
            m_data.Add(key, value);
        }
        #endregion
    }

}
