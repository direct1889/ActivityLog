
namespace Main.Act {

    /// <summary> アクティビティの系列(日毎)(読み取り専用) </summary>
    public interface IROActSequence {
        /// <summary> 時系列順で index 番目 </summary>
        /// <param name="index"> index 番目が存在しない場合例外を投げる </param>
        IROActivity this [int index] { get; }

        /// <summary> 指定時刻に行われていたアクティビティ </summary>
        /// <param name="time"> 未来の場合例外を投げる </param>
        IROActivity this [MinuteOfDay time] { get; }

        /// <summary> 指定時刻に行われていたアクティビティの index </summary>
        int IndexOf(MinuteOfDay time);

        /// <summary> アクティビティ数 </summary>
        int Count { get; }

        /// <summary> 末尾のアクティビティ </summary>
        IROActivity Back { get; }
    }

    /// <summary> アクティビティの系列(日毎) </summary>
    public interface IActSequence : IROActSequence {
        /// <summary> 末尾に追加 </summary>
        void PushBack(IActivity act);

        /// <summary>
        /// 指定した時間に挿入
        /// 完全に包含されるアクティビティは消滅、前後は押しのける
        /// </summary>
        void Insert(MinuteOfDay begin, MinuteOfDay end, IROContent cnt);

        /// <summary> 直前のアクティビティが伸びる </summary>
        void RemoveAt(int index);

        /// <summary> 登録済みのアクティビティを移動させる </summary>
        /// <param name="index"> 変更対象の index </param>
        void Move(int index, MinuteOfDay newBegin, MinuteOfDay? newEnd);

        /// <summary> 登録済みのアクティビティの内容を書き換える </summary>
        void OverwriteCnt(int index, IROContent newContent);
        /// <summary>
        /// 登録済みのアクティビティの時刻を変更
        /// 飲み込まれたアクティビティは消滅
        /// </summary>
        void OverwriteBeginTime(int index, MinuteOfDay newBegin);
        void OverwriteEndTime(int index, MinuteOfDay? newEnd);

    }

}
