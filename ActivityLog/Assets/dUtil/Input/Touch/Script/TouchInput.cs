/*
* AUTHORITY : tempura
* URL : https://qiita.com/tempura/items/4a5482ff6247ec8873df
* LICENSE : UNK
*/

using UnityEngine;
using UTouch = UnityEngine.Touch;
using System;
using static du.di.ExTouchStep;
using static du.Ex.ExVector;

namespace du.di {

	/// <summary>
	/// タッチ情報管理の委譲先インタフェース
	/// Editorか実機かで派生クラスを差し替える
	/// </summary>
	public interface ITouchMgrImpl {
		/// <summary>
		/// 現在何点タッチされているか
		/// Editorでは1が最大
		/// </summary>
		int TouchCount { get; }
		/// <summary>
		///
		/// </summary>
		int GetFingerId(int i);
		/// <summary>
		/// タッチ段階
		/// </summary>
		TouchStep GetTouch(int i);
		/// <summary>
		/// タッチ座標(タッチされていなければnull)
		/// </summary>
		Vector3? GetTouchPosition(int i);
		/// <summary>
		/// 直前フレームとのスワイプ距離(タッチされていなければnull)
		/// タッチ開始フレームなら0
		/// </summary>
		Vector3? GetDeltaPosition(int i);
	}

	/// <summary>
	/// タッチ情報管理の委譲先抽象クラス
	/// Editorか実機かで派生クラスを差し替える
	/// </summary>
	public abstract class TouchMgrImpl : ITouchMgrImpl {
		#region public
		public abstract int TouchCount { get; }
		public abstract int GetFingerId(int i);
		public abstract TouchStep GetTouch(int i);
		public abstract Vector3? GetTouchPosition(int i);
		public abstract Vector3? GetDeltaPosition(int i);
		#endregion

		#region property
		protected Vector3? CurrentPos { get; set; } = null;
		protected Vector3? PreviousPos { get; set; } = null;
		protected TouchStep Step { get; set; } = TouchStep.None;
		protected UTouch UTouchInfo { get { return Input.GetTouch(0); }}
		protected UTouch UTouchInfoMulti(int index) { return Input.GetTouch(index); }
		#endregion
	}
	/// <summary>
	/// タッチ情報管理の委譲先具象クラス
	/// Editor用
	/// </summary>
	public abstract class TouchMgrImpl4Editor : TouchMgrImpl {
        public override int TouchCount
        {
            get {
                if (Input.GetMouseButton(0)
				    || Input.GetMouseButtonDown(0)
					|| Input.GetMouseButtonUp(0))
                { return 1; }
                else { return 0; }
            }
        }
		public override int GetFingerId(int i) {
			return TouchCount;
		}
		public override TouchStep GetTouch(int i) {
			if (Input.GetMouseButtonDown(0)) { return TouchStep.Began; }
			if (Input.GetMouseButton(0))     { return TouchStep.Moved; }
			if (Input.GetMouseButtonUp(0))   { return TouchStep.Ended; }
			return TouchStep.None;
		}
		public override Vector3? GetTouchPosition(int i) {
			TouchStep touch = GetTouch(i);
			if (touch.IsTouching()) {
				return CurrentPos;
			}
			return null;
		}
		public override Vector3? GetDeltaPosition(int i) {
			TouchStep info = GetTouch(i);
			if (info.IsTouching()) {
				CurrentPos = Input.mousePosition;
				return CurrentPos.DisN0() - PreviousPos.DisN0();
			}
			else { return null; }
		}
	}

	/// <summary>
	/// タッチ情報管理の委譲先具象クラス
	/// 実機用
	/// </summary>
	public abstract class TouchMgrImpl4ActualMachine : TouchMgrImpl {
        public override int TouchCount { get { return Input.touchCount; } }
		public override int GetFingerId(int i) {
			return Input.GetTouch(i).fingerId;
		}
		public override TouchStep GetTouch(int i) {
			if (Input.touchCount >= i) {
				return Input.GetTouch(i).phase.Phase2Step();
			}
			else { return TouchStep.None; }
		}
		public override Vector3? GetTouchPosition(int i) {
			if (Input.touchCount >= i) {
				return CurrentPos;
			}
			else { return null; }
		}
		public override Vector3? GetDeltaPosition(int i) {
			if (Input.touchCount >= i) {
				UTouch touch = Input.GetTouch(i);
				return touch.deltaPosition;
			}
			else { return null; }
		}
	}

	public class TouchMgr : du.App.SingletonMonoBehaviour<TouchMgr> {

		#region field
		ITouchMgrImpl m_impl = null;
		Vector3 TouchPosition = Vector3.zero;
		Vector3 PreviousPosition = Vector3.zero;
		#endregion

		#region property
		public Vector3 LastTouchedPosition {
			get { return TouchPosition; }
		}
		public Vector3 LastTouchedPositionP {
			get { return PreviousPosition; }
		}
		public int TouchCount {
			get { return m_impl.TouchCount; }
		}

		#endregion


		#region public
		/// <summary>
		/// タッチ情報を取得(エディタと実機を考慮)
		/// </summary>
		/// <returns>タッチ情報。タッチされていない場合は null</returns>
		public TouchStep GetTouch(int i) {
			return m_impl.GetTouch(i);
		}

		/// <summary>
		/// タッチポジションを取得(エディタと実機を考慮)
		/// </summary>
		/// <returns>タッチポジション。タッチされていない場合は null</returns>
		public Vector3? GetTouchPosition(int i) {
			return m_impl.GetTouchPosition(i);
		}
		public Vector2? GetTouchPosition2D(int i) {
			return m_impl.GetTouchPosition(i);
		}

		/// <summary>
		/// 直前の移動距離
		/// </summary>
		/// <returns>タッチポジション。タッチされていない場合は null</returns>
		public Vector3? GetDeltaPosition(int i) {
			return m_impl.GetDeltaPosition(i);
		}

		public int GetFingerId(int i) {
			return m_impl.GetFingerId(i);
		}

		/// <summary>
		/// タッチワールドポジションを取得(エディタと実機を考慮)
		/// </summary>
		/// <param name='camera'>カメラ</param>
		/// <returns>タッチワールドポジション。タッチされていない場合は null</returns>
		public Vector3? GetTouchWorldPosition(Camera camera, int i) {
			var pos = GetTouchPosition(i);
			if (pos == null) { return null; }
			else { return camera.ScreenToWorldPoint(pos.DisN0()); }
		}
		public Vector3? GetTouchWorldPosition(int i) {
			var pos = GetTouchPosition(i);
			if (pos == null) { return null; }
			else { return Camera.main.ScreenToWorldPoint(pos.DisN0()); }
		}

		public Vector2? GetTouchWorldPosition2D(Camera camera, int i) {
			var pos = GetTouchPosition2D(i);
			if (pos == null) { return null; }
			else { return camera.ScreenToWorldPoint(pos.DisN0()); }
		}
		public Vector2? GetTouchWorldPosition2D(int i) {
			var pos = GetTouchPosition2D(i);
			if (pos == null) { return null; }
			else { return Camera.main.ScreenToWorldPoint(pos.DisN0()); }
		}

		public bool IsTouch { get { return GetTouch(0).IsTouching(); }}
		#endregion

		#region mono
		private void FixedUpdate() {
			switch (GetTouch(0)) {
				case TouchStep.None: break;
				case TouchStep.Began: {
						// s_onTouchEnter.OnNext(Touch.LastTouchedPosition);
						// UTouch touch = Input.GetTouch(0);
						// TouchPosition.x = touch.position.x;
						// TouchPosition.y = touch.position.y;
						// return TouchPosition;
					}
					break;
				case TouchStep.Moved: break;
				case TouchStep.Stationary: break;
				case TouchStep.Ended: {
						// s_onTouchExit.OnNext(Touch.LastTouchedPosition);
					}
					break;
				case TouchStep.Canceled: break;
			}
		}
		#endregion

	}

}
