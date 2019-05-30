

namespace Main.Graph.Act {

	/// <summary> アクティビティの内容 </summary>
	public interface IContent {
		IProject Project     { get; }
		string   Name        { get; }
		bool     IsEffective { get; }
	}

	/// <summary>
	/// アクティビティの時刻/前後情報
	/// 時間の単位はすべて minute (本アプリに秒の分解能はない)
	/// </summary>
	public interface IContext {
		IActivity   NextAct   { get; }
		MinuteOfDay BeginTime { get; }
        /// <returns> まだ終了していなければ null </returns>
		MinuteOfDay EndTime   { get; }
        /// <summary> TODO:未終了のとき、現在時刻までにするか null か </summary>
		int         Duration  { get; }
		bool        HasEnded  { get; }
	}
	public interface IMutableContext : IContext {
		void ResetPrecedeAct(IIndependentActivity precedeAct);
		void ResetPrecedeAct(MinuteOfDay newBeginTime);
		void ResetFollowAct(IActivity followAct);
		void SaikaiSuru();
	}
	/// <returns> 自己完結型時系列情報 </returns>
	public interface IIndependentContext {
		MinuteOfDay BeginTime { get; }
        /// <returns> 必ず終了済みのため常に確定している </returns>
		MinuteOfDay EndTime   { get; }
		int         Duration  { get; }

		IMutableContext MakeDepend(IActivity followAct);
	}

	/// <summary> アクティビティ </summary>
	public interface IActivity {
		/// <summary> 内容 </summary>
		IContent Content { get; }
		/// <summary> 時系列情報 </summary>
		IContext Context { get; }
	}
	public interface IMutableActivity : IActivity {
		IMutableContext MutableContext { get; }
		void ResetContent(IContent cnt);
	}
	/// <summary> アクティビティ </summary>
	public interface IIndependentActivity {
		/// <summary> 内容 </summary>
		IContent Content { get; }
		/// <summary> 時系列情報 </summary>
		IIndependentContext IndependentContext { get; }

		IMutableActivity MakeDepend(IMutableActivity followAct);
	}

}
