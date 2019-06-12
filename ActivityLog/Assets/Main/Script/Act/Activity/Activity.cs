using UColor = UnityEngine.Color;

namespace Main.Act {

    /// <summary> アクティビティの内容 </summary>
    public interface IROContent : du.Cmp.IHashTreeDataType<IProject, string> {
        // IProject   Parent      { get; }
        string     Name        { get; }
        bool       IsEffective { get; }

        ThemeColor Color       { get; }
        int        ParentCount { get; }
        /// <value>
        /// 辞書に入れるときkeyとして利用する
        /// a.key == b.key <=必要十分=> IsOverlap(a, b)
        /// </value>
        // string     Key         { get; }
    }

    public interface IActivity : IROContent {}

    /// <summary>
    /// アクティビティの時刻/前後情報
    /// 時間の単位はすべて minute (本アプリに秒の分解能はない)
    /// </summary>
    public interface IROContext {
        IROActRecord  NextAct   { get; }
        MinuteOfDay  BeginTime { get; }
        /// <value> まだ終了していなければ null </value>
        MinuteOfDay? EndTime   { get; }
        /// <summary> 未終了のときは現在時刻まで </summary>
        int          Duration  { get; }
        bool         HasEnded  { get; }
    }
    public interface IContext : IROContext {
        void ResetPrecedeAct(IIndependentActRecord precedeAct);
        void ResetPrecedeAct(MinuteOfDay newBeginTime);
        void ResetFollowAct(IROActRecord followAct);
        void Resume();
    }
    /// <returns> 自己完結型時系列情報 </returns>
    public interface IIndependentContext {
        MinuteOfDay BeginTime { get; }
        /// <returns> 必ず終了済みのため常に確定している </returns>
        MinuteOfDay EndTime   { get; }
        int         Duration  { get; }

        IContext MakeDepend(IROActRecord followAct);
    }

    /// <summary> アクティビティ </summary>
    public interface IROActRecord {
        /// <summary> 内容 </summary>
        IActivity Content { get; }
        /// <summary> 時系列情報 </summary>
        IROContext Context { get; }
    }
    public interface IActRecord : IROActRecord {
        IContext MutableContext { get; }
        void ResetContent(IActivity cnt);
    }
    /// <summary> 自己完結型アクティビティ </summary>
    public interface IIndependentActRecord {
        /// <summary> 内容 </summary>
        IActivity Content { get; }
        /// <summary> 時系列情報 </summary>
        IIndependentContext IndependentContext { get; }

        IActRecord MakeDepend(IActRecord followAct);
    }

}
