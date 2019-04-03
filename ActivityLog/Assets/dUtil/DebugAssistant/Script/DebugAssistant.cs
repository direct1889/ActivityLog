using UnityEngine;

namespace du.Test {

	public class DebugAssistant : App.SingletonMonoBehaviour<DebugAssistant> {

		#region field

		public TestLogger TestLog { private set; get; } = null;
		ITestCode m_test = null;
		public Audio.SoundAsset Sound { private set; get; } = null;

		#endregion


		#region mono

		private void Awake() {
			LLog.Boot.Log("DebugAssistant awoke.");
			Instance.m_test = new TestCodeCalledByAppMgr();
			Mgr.RegisterMgr(Instance);
		}

		private void Start() {
			Instance.m_test?.OnStart();
		}

		private void Update() {
			m_test?.OnUpdate();
		}

		#endregion


		#region public

		public void SetTestLog(TestLogger log) {
			TestLog = log;
		}
		public void SetAudioAsset(Audio.SoundAsset sndAsset) {
			Sound = sndAsset;
		}

		#endregion

	}

}
