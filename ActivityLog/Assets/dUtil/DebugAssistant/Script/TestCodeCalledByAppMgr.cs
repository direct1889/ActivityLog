using UnityEngine;
using UniRx;
using System.Collections.Generic;
using System.Linq;


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
			Mgr.Debug.TestLog?.SetText("IsTouch", di.Touch.GetTouch(0));
			Mgr.Debug.TestLog?.SetText("TouchPos", di.Touch.GetTouchPosition(0));
			Mgr.Debug.TestLog?.SetText("TouchWPos2D", di.Touch.GetTouchWorldPosition2D(0));
		}

	}

}

