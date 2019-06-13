
namespace du.Test {

    public class DebugAssistant : App.SingletonMonoBehaviour<DebugAssistant> {
        #region field
        ITestCode m_test;
        #endregion

        #region field property
        public TestLogger TestLog { get; private set; }
        public Audio.SoundAsset Sound { get; private set; }
        #endregion

        #region mono
        private void Awake() {
            LLog.Boot.Log("DebugAssistant awoke.");
            Instance.m_test = new TestCodeCalledByAppMgr();
        }
        private void Start() => Instance.m_test?.OnStart();
        private void Update() => m_test?.OnUpdate();
        #endregion

        #region public
        public void SetTestLog(TestLogger log) => TestLog = log;
        public void SetAudioAsset(Audio.SoundAsset sndAsset) => Sound = sndAsset;
        #endregion

    }

}
