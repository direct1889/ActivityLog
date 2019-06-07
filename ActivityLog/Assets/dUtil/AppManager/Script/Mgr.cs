
namespace du {

    public static class Mgr {
        #region getter
        public static App.AppManager App => du.App.AppManager.Instance;
        public static Test.DebugAssistant Debug => Test.DebugAssistant.Instance;
        public static di.TouchMgr Touch => di.TouchMgr.Instance;
        // public static App.OSUI OSUI { private set; get; }
        #endregion
    }

}
