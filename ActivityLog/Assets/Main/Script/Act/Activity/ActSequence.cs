
namespace Main.Act {

    /// <summary> ActRecordの系列(日毎)(読み取り専用) </summary>
    public interface IROActRecordSequence {
        /// <summary> 時系列順で index 番目 </summary>
        /// <param name="index"> index 番目が存在しない場合例外を投げる </param>
        IROActRecord this [int index] { get; }

        /// <summary> 指定時刻に行われていたActRecord </summary>
        /// <param name="time"> 未来の場合例外を投げる </param>
        IROActRecord this [MinuteOfDay time] { get; }

        /// <summary> 指定時刻に行われていたActRecordの index </summary>
        int IndexOf(MinuteOfDay time);

        /// <summary> ActRecord数 </summary>
        int Count { get; }

        /// <summary> 末尾のActRecord </summary>
        IROActRecord Back { get; }

        /// <summary> 内容詳細文字列 </summary>
        string Dump();
    }

    /// <summary> ActRecordの系列(日毎) </summary>
    public interface IActRecordSequence : IROActRecordSequence {
        /// <summary> 末尾に追加 </summary>
        void PushBack(IActRecord act);

        /// <summary>
        /// 指定した時間に挿入
        /// 完全に包含されるActRecordは消滅、前後は押しのける
        /// </summary>
        void Insert(MinuteOfDay begin, MinuteOfDay end, IActivity cnt);

        /// <summary> 直前のActRecordが伸びる </summary>
        void RemoveAt(int index);

        /// <summary> 登録済みのActRecordを移動させる </summary>
        /// <param name="index"> 変更対象の index </param>
        void Move(int index, MinuteOfDay newBegin, MinuteOfDay? newEnd);

        /// <summary> 登録済みのActRecordの内容を書き換える </summary>
        void OverwriteCnt(int index, IActivity newContent);
        /// <summary>
        /// 登録済みのActRecordの時刻を変更
        /// 飲み込まれたActRecordは消滅
        /// </summary>
        void OverwriteBeginTime(int index, MinuteOfDay newBegin);
        void OverwriteEndTime(int index, MinuteOfDay? newEnd);

    }

}
