using System.Collections.Generic;
using System;
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
    }

    public class HashTreeNode<T, TParent, TKey> : IHashTreeNode<T, TParent, TKey>
        where T : class, IHashTreeDataType<TParent, TKey>
        where TParent : class, IHashTreeDataType<TParent, TKey>
    {
        #region public field property
        public T Value { get; }
        public IOrderedMap<TKey, IHashTreeNode<T, TParent, TKey>> Children { get; } = new OrderedMap<TKey, IHashTreeNode<T, TParent, TKey>>();
        #endregion

        #region ctor
        public HashTreeNode(T value) { Value = value; }
        #endregion

        #region public
        /// <returns> 見つからなければ null </returns>
        public IHashTreeNode<T, TParent, TKey> At(TKey key) {
            return Children.At(key);
        }
        #endregion

        #region public
        public void Add(T value) {
            Children.Add(value.Key, new HashTreeNode<T, TParent, TKey>(value));
        }
        public void Add(T value, int index) {
            Children.Add(value.Key, new HashTreeNode<T, TParent, TKey>(value));
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
        du.Cmp.IHashTreeNode<T, TParent, TKey> At(TParent proj);
    }

    public class HashTree<T, TParent, TKey> : IHashTree<T, TParent, TKey>
        where T : class, IHashTreeDataType<TParent, TKey>
        where TParent : class, IHashTreeDataType<TParent, TKey>
    {
        #region field
        // IHashTreeNode<T, TParent, TKey> m_root;
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
        public virtual void Add(T value) {
            At(value.Parent)?.Children.Add(value.Key, new HashTreeNode<T, TParent, TKey>(value));
        }
        public virtual void Add(T value, int index) {
            At(value.Parent)?.Children.Add(value.Key, new HashTreeNode<T, TParent, TKey>(value), index);
        }
        /// <param name="proj"> nullのときはRootNodeを返す </param>
        /// <returns> projがnullでなく、見つからないときはnull </returns>
        public du.Cmp.IHashTreeNode<T, TParent, TKey> At(TParent proj) {
            if (proj is null) { return Root; }
            else { return At(proj.Parent)?.Children.At(proj.Key); }
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
