using UColor = UnityEngine.Color;

namespace Main.Act {

    /// <summary> アクティビティ/プロジェクトの内容 </summary>
    public interface IContent : du.Cmp.IHashTreeDataType<IProject, string> {
        /// <value> 名称 </value>
        string     Name        { get; }
        /// <value> 有益か </value>
        bool       IsEffective { get; }
        /// <value> 色 </value>
        ThemeColor Color       { get; }
        /// <value> ルートからの世代数 </value>
        int        ParentCount { get; }

        // IProject   Parent      { get; }
        // string     Key         { get; }
    }

    /// <summary> アクティビティの内容 </summary>
    public interface IActivity : IContent {}

    /// <summary>
    /// アクティビティの時刻/前後情報
    /// - 時間の単位はすべて minute (本アプリに秒の分解能はない)
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
    /// <summary>
    /// アクティビティの時刻/前後情報
    /// - 時間の単位はすべて minute (本アプリに秒の分解能はない)
    /// </summary>
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

    /// <summary> アクティビティ実績 </summary>
    public interface IROActRecord {
        /// <summary> 内容 </summary>
        IActivity Activity { get; }
        /// <summary> 時系列情報 </summary>
        IROContext Context { get; }
    }
    /// <summary> アクティビティ実績 </summary>
    public interface IActRecord : IROActRecord {
        IContext MutableContext { get; }
        void ResetAct(IActivity cnt);
    }
    /// <summary> 自己完結型アクティビティ実績 </summary>
    public interface IIndependentActRecord {
        /// <summary> 内容 </summary>
        IActivity Activity { get; }
        /// <summary> 時系列情報 </summary>
        IIndependentContext IndependentContext { get; }

        IActRecord MakeDepend(IActRecord followAct);
    }

}
