

namespace Main.Act {

	/// <summary> アクティビティの内容 </summary>
	public class Content : IROContent {
		#region field-property
		public IProject	Project		{ get; private set; }
		public string	Name		{ get; private set; }
		public bool		IsEffective	{ get; private set; }
		#endregion

		#region ctor/dtor
		public Content(IProject proj, string name, bool isEffective) {
			Project = proj; Name = name; IsEffective = isEffective;
		}
		public Content(IProject proj, string name) {
			Project = proj; Name = name; IsEffective = proj.IsEffectiveDefault;
		}
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
        public IROActivity NextAct { get; private set; } = null;
        public MinuteOfDay BeginTime { get; private set; }
        /// <summary> まだ終了していなければ null </summary>
        public MinuteOfDay EndTime { get { return NextAct?.Context?.BeginTime; } }
        /// <summary> TODO:未終了のとき、現在時刻までにするか null か </summary>
        public int Duration { get { return /* TODO */EndTime - BeginTime; } }
        public bool HasEnded { get { return NextAct != null; } }
		#endregion

		#region ctor/dtor
		public Context(MinuteOfDay beginTime) { BeginTime = beginTime; }
		public Context(MinuteOfDay beginTime, IROActivity followAct) {
			BeginTime = beginTime;
			NextAct = followAct;
		}
		#endregion

		#region public
		/// <summary> アクティビティを終了させる </summary>
		// public void Finish(IActivity nextAct) {
			// if (!HasEnded) { NextAct = nextAct; }
		// }
		public void ResetPrecedeAct(IIndependentActivity precedeAct) {
			BeginTime = precedeAct.IndependentContext.EndTime;
		}
		public void ResetPrecedeAct(MinuteOfDay newBeginTime) {
			BeginTime = newBeginTime;
		}
		public void ResetFollowAct(IROActivity followAct) {
			NextAct = followAct;
		}
		public void SaikaiSuru() {
			NextAct = null;
		}
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
        public int Duration { get { return EndTime - BeginTime; } }
		#endregion

		#region ctor/dtor
		public IndependentContext(MinuteOfDay beginTime, MinuteOfDay endTime) {
			BeginTime = beginTime;
			EndTime = endTime;
		}
		#endregion

		#region public
		public IContext MakeDepend(IROActivity followAct) {
			return new Context(BeginTime, followAct);
		}
		#endregion
	}

	/// <summary> アクティビティ </summary>
	public class Activity : IActivity {

		#region property
		public IROContent Content { get; private set; }
		public IROContext Context { get { return MutableContext; } }
		public IContext MutableContext { get; private set; }
		#endregion

		#region ctor/dtor
		public Activity(IROContent content, IContext context) {
			Content = content; MutableContext = context;
		}
		public Activity(IProject proj, string name, bool isEffective, MinuteOfDay beginTime)
			: this(new Content(proj, name, isEffective), new Context(beginTime)) {}
		public Activity(IProject proj, string name, MinuteOfDay beginTime)
			: this(new Content(proj, name), new Context(beginTime)) {}
		#endregion

		#region public
		public void ResetContent(IROContent cnt) { Content = cnt; }
		#endregion
	}

	/// <summary> アクティビティ </summary>
	public class IndependentActivity : IIndependentActivity {

		#region property
		public IROContent Content { get; private set; }
		public IIndependentContext IndependentContext { get; private set; }
		#endregion

		#region ctor/dtor
		public IndependentActivity(IROContent content, IIndependentContext context) {
			Content = content; IndependentContext = context;
		}
		public IndependentActivity(IProject proj, string name, bool isEffective, MinuteOfDay beginTime, MinuteOfDay endTime)
			: this(new Content(proj, name, isEffective), new IndependentContext(beginTime, endTime)) {}
		public IndependentActivity(IProject proj, string name, MinuteOfDay beginTime, MinuteOfDay endTime)
			: this(new Content(proj, name), new IndependentContext(beginTime, endTime)) {}
		#endregion

		#region public
		public IActivity MakeDepend(IActivity followAct) {
			return new Activity(Content, IndependentContext.MakeDepend(followAct));
		}
		#endregion
	}

}
