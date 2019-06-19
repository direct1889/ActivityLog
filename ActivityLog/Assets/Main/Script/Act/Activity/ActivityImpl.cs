using UColor = UnityEngine.Color;


namespace Main.Act {

    /// <summary> アクティビティの内容 </summary>
    public class Activity : IActivity {
        #region field-property
        public IProject Parent      { get; private set; }
        public string   Name        { get; private set; }
        public bool     IsEffective { get; private set; }
        #endregion

        #region getter
        public ThemeColor Color       => Parent.Color;
        public int        ParentCount => Parent.ParentCount + 1;
        public string     Key         => $"{Parent.Key}::{Name}";
        #endregion

        #region ctor/dtor
        public Activity(IProject proj, string name, bool isEffective) {
            Parent = proj; Name = name; IsEffective = isEffective;
        }
        public Activity(IProject proj, string name) : this(proj, name, proj.IsEffective) {}
        #endregion

        #region override
        public override string ToString() => $"{Key}({IsEffective})";
        #endregion
    }

    /// <summary>
    /// アクティビティの時刻/前後情報
    /// 時間の単位はすべて minute (本アプリに秒の分解能はない)
    /// </summary>
    public class Context : IContext {
        #region property
        /// <summary>
        /// 後続のアクティビティ
        /// まだ終了していなければ null
        /// </summary>
        public IROActRecord NextAct { get; private set; }
        public MinuteOfDay BeginTime { get; private set; }
        /// <summary> まだ終了していなければ null </summary>
        public MinuteOfDay? EndTime => NextAct?.Context?.BeginTime;
        /// <summary> 未終了のときは現在時刻まで </summary>
        public int Duration => (EndTime ?? Sys.Chronos.Now) - BeginTime;
        public bool HasEnded => NextAct != null;
        #endregion

        #region getter
        public override string ToString() => $"[{BeginTime}, " + (EndTime?.ToString() ?? "cont.") + "]";
        #endregion

        #region ctor/dtor
        public Context(MinuteOfDay beginTime) { BeginTime = beginTime; }
        public Context(MinuteOfDay beginTime, IROActRecord followAct) {
            BeginTime = beginTime;
            NextAct = followAct;
        }
        #endregion

        #region public
        public void ResetPrecedeAct(IIndependentActRecord precedeAct) {
            BeginTime = precedeAct.IndependentContext.EndTime;
        }
        public void ResetPrecedeAct(MinuteOfDay newBeginTime) {
            BeginTime = newBeginTime;
        }
        public void ResetFollowAct(IROActRecord followAct) {
            NextAct = followAct;
        }
        public void Resume() {
            NextAct = null;
        }
        #endregion

        #region static
        public static IContext BeginFromNow => new Context(Sys.Chronos.Now);
        #endregion
    }

    /// <summary>
    /// アクティビティの時刻/前後情報 独立版
    /// 時間の単位はすべて minute (本アプリに秒の分解能はない)
    /// </summary>
    public class IndependentContext : IIndependentContext {
        #region property
        public MinuteOfDay BeginTime { get; private set; }
        public MinuteOfDay EndTime { get; private set; }
        public int Duration => EndTime - BeginTime;
        #endregion

        #region ctor/dtor
        public IndependentContext(MinuteOfDay beginTime, MinuteOfDay endTime) {
            BeginTime = beginTime;
            EndTime = endTime;
        }
        #endregion

        #region public
        public IContext MakeDepend(IROActRecord followAct) {
            return new Context(BeginTime, followAct);
        }
        #endregion
    }

    /// <summary> アクティビティ実績 </summary>
    public class ActRecord : IActRecord {

        #region property
        public IActivity Activity { get; private set; }
        public IROContext Context => MutableContext;
        public IContext MutableContext { get; private set; }
        public bool IsInvalid => Activity is null;
        #endregion

        #region ctor/dtor
        public ActRecord(IActivity act, IContext context) {
            Activity = act; MutableContext = context;
        }
        public ActRecord(IActivity act, MinuteOfDay beginTime) {
            Activity = act; MutableContext = new Context(beginTime);
        }
        public ActRecord(IProject proj, string name, bool isEffective, MinuteOfDay beginTime)
            : this(new Activity(proj, name, isEffective), new Context(beginTime)) {}
        public ActRecord(IProject proj, string name, MinuteOfDay beginTime)
            : this(new Activity(proj, name), new Context(beginTime)) {}
        #endregion

        #region public
        public void ResetAct(IActivity cnt) { Activity = cnt; }
        #endregion

        #region getter
        public override string ToString() => (Activity?.ToString() ?? "Invalid") + Context;
        /// <value> CSVファイルのラベル行 </value>
        public static string CSVLabels => "SN,BeginTime";
        #endregion
    }

    /// <summary> アクティビティ実績 </summary>
    public class IndependentActRecord : IIndependentActRecord {

        #region property
        public IActivity Activity { get; private set; }
        public IIndependentContext IndependentContext { get; private set; }
        #endregion

        #region ctor/dtor
        public IndependentActRecord(IActivity act, IIndependentContext context) {
            Activity = act; IndependentContext = context;
        }
        public IndependentActRecord(IProject proj, string name, bool isEffective, MinuteOfDay beginTime, MinuteOfDay endTime)
            : this(new Activity(proj, name, isEffective), new IndependentContext(beginTime, endTime)) {}
        public IndependentActRecord(IProject proj, string name, MinuteOfDay beginTime, MinuteOfDay endTime)
            : this(new Activity(proj, name), new IndependentContext(beginTime, endTime)) {}
        #endregion

        #region public
        public IActRecord MakeDepend(IActRecord followAct) {
            return new ActRecord(Activity, IndependentContext.MakeDepend(followAct));
        }
        #endregion
    }

}
