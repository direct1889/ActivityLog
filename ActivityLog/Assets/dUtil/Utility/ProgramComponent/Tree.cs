using System.Collections.Generic;
using System;
using System.Linq;
using UniRx;

namespace du.Cmp {

    /// <summary> HashTreeの実データ要件 </summary>
    public interface IHashTreeDataType<T, TKey>
        where T : class, IHashTreeDataType<T, TKey>
    {
        /// <value> 親がいない場合 null </value>
        T Parent { get; }
        TKey Key { get; }
    }

    /// <param name="T"> IContent </param>
    /// <param name="TParent"> IProject </param>
    /// <param name="TKey"> string </param>
    public interface IHashTreeNode<T, TParent, TKey>
        where T : class, IHashTreeDataType<TParent, TKey>
        where TParent : class, IHashTreeDataType<TParent, TKey>
    {
        T Value { get; }
        IOrderedMap<TKey, IHashTreeNode<T, TParent, TKey>> Children { get; }
        void Add(T value);
        void Add(T value, int index);
        int DescendantCount();
    }

    public class HashTreeNode<T, TParent, TKey> : IHashTreeNode<T, TParent, TKey>
        where T : class, IHashTreeDataType<TParent, TKey>
        where TParent : class, IHashTreeDataType<TParent, TKey>
    {
        #region field
        IOrderedMap<TKey, IHashTreeNode<T, TParent, TKey>> m_children;
        #endregion

        #region public field property
        public T Value { get; }
        public IOrderedMap<TKey, IHashTreeNode<T, TParent, TKey>> Children => m_children;
        #endregion

        #region ctor
        public HashTreeNode(T value) { Value = value; }
        #endregion

        #region getter
        /// <returns> 見つからなければ null </returns>
        public int DescendantCount() {
            if (Children is null) { return 0; }
            else {
                return Children.Select(node => node.DescendantCount()).Sum() + Children.Count;
            }
        }
        #endregion

        #region public
        public void Add(T value) {
            if (Children is null) {
                m_children = new OrderedMap<TKey, IHashTreeNode<T, TParent, TKey>>();
            }
            Children.Add(value.Key, new HashTreeNode<T, TParent, TKey>(value));
        }
        public void Add(T value, int index) {
            if (Children is null) {
                m_children = new OrderedMap<TKey, IHashTreeNode<T, TParent, TKey>>();
            }
            Children.Add(value.Key, new HashTreeNode<T, TParent, TKey>(value), index);
        }
        #endregion
    }


    public interface IHashTree<T, TParent, TKey>
        where T : class, IHashTreeDataType<TParent, TKey>
        where TParent : class, IHashTreeDataType<TParent, TKey>
    {
        void Add(T value);
        void Add(T value, int index);
        /// <param name="proj"> nullのときはRootNodeを返す </param>
        /// <returns> projがnullでなく、見つからないときはnull </returns>
        du.Cmp.IHashTreeNode<T, TParent, TKey> At(IHashTreeDataType<TParent, TKey> value);
        /// <summary> valueがRoot(0)から数えて何番目か </summary>
        /// <returns> valueがnullでなく、見つからないときはnull </returns>
        int? SerialNumber(IHashTreeDataType<TParent, TKey> value);
    }

    public class HashTree<T, TParent, TKey> : IHashTree<T, TParent, TKey>
        where T : class, IHashTreeDataType<TParent, TKey>
        where TParent : class, IHashTreeDataType<TParent, TKey>
    {
        #region field
        protected IHashTreeNode<T, TParent, TKey> Root { get; }
        #endregion

        #region ctor
        public HashTree() {
            Root = new HashTreeNode<T, TParent, TKey>(null);
        }
        public HashTree(T root) {
            Root = new HashTreeNode<T, TParent, TKey>(root);
        }
        #endregion

        #region public
        public virtual void Add(T value) => At(value.Parent)?.Add(value);
        public virtual void Add(T value, int index) => At(value.Parent)?.Add(value, index);
        /// <param name="value"> nullのときはRootNodeを返す </param>
        /// <returns> valueがnullでなく、見つからないときはnull </returns>
        public du.Cmp.IHashTreeNode<T, TParent, TKey> At(IHashTreeDataType<TParent, TKey> value) {
            if (value is null) { return Root; }
            else { return At(value.Parent)?.Children?.At(value.Key); }
        }
        /// <summary> valueがRoot(0)から数えて何番目か </summary>
        /// <returns> valueがnullでなく、見つからないときはnull </returns>
        public int? SerialNumber(IHashTreeDataType<TParent, TKey> value) {
            // value is null ならRootとみなす
            if (value is null) { return 0; }
            var parentNode = At(value.Parent);
            int? siblingNum = parentNode?.Children?.IndexOf(value.Key);
            int? sum = SerialNumber(value.Parent) + siblingNum + 1;
            for (int i = 0; i < siblingNum; ++i) {
                sum += parentNode?.Children?.At(i).DescendantCount();
            }
            // SN(node) == SN(node.Parent) + 第n兄弟 + 第n-1兄弟目までの子
            return sum;
        }
        #endregion
    }


    /// <summary> 要素の追加/削除/変更時に通知を流す </summary>
    public interface IRxHashTree<T, TParent, TKey>// : IHashTree<T, TParent, TKey>
        where T : class, IHashTreeDataType<TParent, TKey>
        where TParent : class, IHashTreeDataType<TParent, TKey>
    {
        IObservable<T> RxAdded { get; }
        // IObservable<T> RxRemoved { get; }
        // IObservable<T> RxChanged { get; }
    }

    public class RxHashTree<T, TParent, TKey>
    : HashTree<T, TParent, TKey>, IRxHashTree<T, TParent, TKey>
        where T : class, IHashTreeDataType<TParent, TKey>
        where TParent : class, IHashTreeDataType<TParent, TKey>
    {
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
        public override void Add(T value) {
            base.Add(value);
            m_addedStream.OnNext(value);
        }
        public override void Add(T value, int index) {
            base.Add(value, index);
            m_addedStream.OnNext(value);
        }
        #endregion
    }


}
