using UColor = UnityEngine.Color;


namespace Main.Act {

    /// <summary> アクティビティの内容 </summary>
    public interface IROContent {
        IProject   Parent      { get; }
        string     Name        { get; }
        bool       IsEffective { get; }

        ThemeColor Color       { get; }
        int        ParentCount { get; }
    }

    /// <summary>
    /// アクティビティの時刻/前後情報
    /// 時間の単位はすべて minute (本アプリに秒の分解能はない)
    /// </summary>
    public interface IROContext {
        IROActivity  NextAct   { get; }
        MinuteOfDay  BeginTime { get; }
        /// <returns> まだ終了していなければ null </returns>
        MinuteOfDay? EndTime   { get; }
        /// <summary> 未終了のときは現在時刻まで </summary>
        int          Duration  { get; }
        bool         HasEnded  { get; }
    }
    public interface IContext : IROContext {
        void ResetPrecedeAct(IIndependentActivity precedeAct);
        void ResetPrecedeAct(MinuteOfDay newBeginTime);
        void ResetFollowAct(IROActivity followAct);
        void SaikaiSuru();
    }
    /// <returns> 自己完結型時系列情報 </returns>
    public interface IIndependentContext {
        MinuteOfDay BeginTime { get; }
        /// <returns> 必ず終了済みのため常に確定している </returns>
        MinuteOfDay EndTime   { get; }
        int         Duration  { get; }

        IContext MakeDepend(IROActivity followAct);
    }

    /// <summary> アクティビティ </summary>
    public interface IROActivity {
        /// <summary> 内容 </summary>
        IROContent Content { get; }
        /// <summary> 時系列情報 </summary>
        IROContext Context { get; }
    }
    public interface IActivity : IROActivity {
        IContext MutableContext { get; }
        void ResetContent(IROContent cnt);
    }
    /// <summary> アクティビティ </summary>
    public interface IIndependentActivity {
        /// <summary> 内容 </summary>
        IROContent Content { get; }
        /// <summary> 時系列情報 </summary>
        IIndependentContext IndependentContext { get; }

        IActivity MakeDepend(IActivity followAct);
    }

}
