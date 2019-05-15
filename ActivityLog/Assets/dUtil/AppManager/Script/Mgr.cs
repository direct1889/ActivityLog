using UnityEngine;


namespace du {

	public static class Mgr {


		#region field

		// static AppManager s_appMgr = null;
		// static Test.DebugAssistant s_debugAssistant = null;

		#endregion


		#region property

		// public static AppManager App { get { return s_appMgr; } }
		// public static Test.DebugAssistant Debug { get { return s_debugAssistant; } }

		public static App.AppManager App { private set; get; }
		public static Test.DebugAssistant Debug { private set; get; }
		public static di.TouchMgr Touch { private set; get; }
		// public static App.OSUI OSUI { private set; get; }

		#endregion


		#region public

		public static void RegisterMgr(App.AppManager appMgr) { App = appMgr; }
		public static void RegisterMgr(Test.DebugAssistant debugAsst) { Debug = debugAsst; }
		// public static void RegisterMgr(App.OSUI osui) { OSUI = osui; }

		#endregion


	}

}
