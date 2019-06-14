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
        bool HasChildren { get; }
        void Add(T value);
        void Add(T value, int index);
        /// <summary> 子孫の数 </summary>
        int DescendantCount();
    }


    public interface IHashTree<T, TParent, TKey>
        : IReadOnlyCollection<T>
        where T : class, IHashTreeDataType<TParent, TKey>
        where TParent : class, IHashTreeDataType<TParent, TKey>
    {
        void Add(T value);
        void Add(T value, int index);
        /// <param name="proj"> nullのときはRootNodeを返す </param>
        /// <returns> projがnullでなく、見つからないときはnull </returns>
        du.Cmp.IHashTreeNode<T, TParent, TKey> At(IHashTreeDataType<TParent, TKey> value);
        /// <summary> SerialNumberから要素を引く </summary>
        /// <returns> 見つからないときはnull </returns>
        T AtBySerialNumber(int sn);
        /// <summary> valueがRoot(0)から数えて何番目か </summary>
        /// <returns> valueがnullでなく、見つからないときはnull </returns>
        int? SerialNumber(IHashTreeDataType<TParent, TKey> value);
    }


    /// <summary> 要素の追加/削除/変更時に通知を流す </summary>
    public interface IRxHashTree<T, TParent, TKey> : IHashTree<T, TParent, TKey>
        where T : class, IHashTreeDataType<TParent, TKey>
        where TParent : class, IHashTreeDataType<TParent, TKey>
    {
        IObservable<T> RxAdded { get; }
        // IObservable<T> RxRemoved { get; }
        // IObservable<T> RxChanged { get; }
    }

}
