using UnityEngine;
using UniRx;

namespace du.Test {

	public interface ITestCode {
		void OnStart();
		void OnUpdate();
	}

	public class TestCodeCalledByAppMgr : ITestCode {

		IReactiveProperty<Vector2> m_pos = new ReactiveProperty<Vector2>();

		public void OnStart() {
			LLog.Boot.Log("TestCodeCalledByAppMgr on start.");
		}

		public void OnUpdate() {
			Mgr.Debug.TestLog?.SetText("IsTouch", Mgr.Touch.Step0);
			Mgr.Debug.TestLog?.SetText("TouchPos", Mgr.Touch.Pos3D0);
			Mgr.Debug.TestLog?.SetText("TouchWPos2D", Mgr.Touch.WorldPos0);
		}

	}

}

