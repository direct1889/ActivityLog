using UnityEngine;
using UTouch = UnityEngine.Touch;
using System;
using static du.di.ExTouchInfo;
using static du.Ex.ExVector;

namespace du.di {

	public interface ITouchEvent {

	}

	public class TouchEvent : ITouchEvent {

		#region property

		public Vector3 Enter { private set; get; }
		public Vector3 Current { private set; get; }
		public Vector3 Exit { private set; get; }
		public bool HasExited { private set; get; }

		#endregion


		#region ctor/dtor

		public TouchEvent(Vector3 enter) {
			Enter = enter;
			PositionUpdate(enter);
		}

		#endregion


		#region public

		public void PositionUpdate(Vector3 current) {
			Current = current;
		}

		public void OnExit(Vector3 exit) {
			Exit = exit;
			HasExited = true;
		}

		#endregion

	}

	public static class Touch {

		#region field

		static Vector3 TouchPosition = Vector3.zero;
		static Vector3 PreviousPosition = Vector3.zero;

		#endregion


		#region property

		public static Vector3 LastTouchedPosition {
			get { return TouchPosition; }
		}

		public static Vector3 LastTouchedPositionP {
			get { return PreviousPosition; }
		}

		public static int touchCount {
			get {
				if (Application.isEditor) {
					if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0)) {
						return 1;
					}
					else {
						return 0;
					}
				}
				else {
					return Input.touchCount;
				}
			}
		}

		#endregion


		#region public

		/// <summary>
		/// タッチ情報を取得(エディタと実機を考慮)
		/// </summary>
		/// <returns>タッチ情報。タッチされていない場合は null</returns>
		public static TouchInfo GetTouch(int i) {
			if (Application.isEditor) {
				if (Input.GetMouseButtonDown(0)) { return TouchInfo.Began; }
				if (Input.GetMouseButton(0)) { return TouchInfo.Moved; }
				if (Input.GetMouseButtonUp(0)) { return TouchInfo.Ended; }
			}
			else {
				if (Input.touchCount >= i) {
					return Phase2Info(Input.GetTouch(i).phase);
				}
			}
			return TouchInfo.None;
		}

		/// <summary>
		/// タッチポジションを取得(エディタと実機を考慮)
		/// </summary>
		/// <returns>タッチポジション。タッチされていない場合は (0, 0, 0)</returns>
		public static Vector3 GetTouchPosition(int i) {
			if (Application.isEditor) {
				TouchInfo touch = GetTouch(i);
				if (touch.IsTouching()) {
					PreviousPosition = Input.mousePosition;
					return PreviousPosition;
				}
			}
			else {
				if (Input.touchCount >= i) {
					UTouch touch = Input.GetTouch(i);
					TouchPosition.x = touch.position.x;
					TouchPosition.y = touch.position.y;
					return TouchPosition;
				}
			}
			return Vector3.zero;
		}
		public static Vector2 GetTouchPosition2D(int i) {
			return GetTouchPosition(i);
		}

		public static Vector3 GetDeltaPosition(int i) {
			if (Application.isEditor) {
				TouchInfo info = GetTouch(i);
				if (info.IsTouching()) {
					Vector3 currentPosition = Input.mousePosition;
					Vector3 delta = currentPosition - PreviousPosition;
					PreviousPosition = currentPosition;
					return delta;
				}
			}
			else {
				if (Input.touchCount >= i) {
					UTouch touch = Input.GetTouch(i);
					PreviousPosition.x = touch.deltaPosition.x;
					PreviousPosition.y = touch.deltaPosition.y;
					return PreviousPosition;
				}
			}
			return Vector3.zero;
		}

		public static int GetFingerId(int i) {
			if (Application.isEditor) {
				if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0)) {
					return 1;
				}
				else {
					return 0;
				}
			}
			else {
				return Input.GetTouch(i).fingerId;
			}
		}

		/// <summary>
		/// タッチワールドポジションを取得(エディタと実機を考慮)
		/// </summary>
		/// <param name='camera'>カメラ</param>
		/// <returns>タッチワールドポジション。タッチされていない場合は (0, 0, 0)</returns>
		public static Vector3 GetTouchWorldPosition(Camera camera, int i) {
			return camera.ScreenToWorldPoint(GetTouchPosition(i));
		}
		public static Vector3 GetTouchWorldPosition(int i) {
			return Camera.main.ScreenToWorldPoint(GetTouchPosition(i));
		}

		public static Vector2 GetTouchWorldPosition2D(Camera camera, int i) {
			return camera.ScreenToWorldPoint(GetTouchPosition(i).Vec3to2());
		}
		public static Vector2 GetTouchWorldPosition2D(int i) {
			return Camera.main.ScreenToWorldPoint(GetTouchPosition(i).Vec3to2());
		}


		public static bool IsTouch { get { return GetTouch(0).IsTouching(); }}
		// public static bool IsTouch { get { return GetTouch(0) != TouchInfo.None; } }

		#endregion


		#region private

		private static void FixedUpdate() {
			switch (Touch.GetTouch(0)) {
				case TouchInfo.None: break;
				case TouchInfo.Began: {
						// s_onTouchEnter.OnNext(Touch.LastTouchedPosition);
					}
					break;
				case TouchInfo.Moved: break;
				case TouchInfo.Stationary: break;
				case TouchInfo.Ended: {
						// s_onTouchExit.OnNext(Touch.LastTouchedPosition);
					}
					break;
				case TouchInfo.Canceled: break;
			}
		}

		#endregion

	}

	/// <summary>
	/// タッチ情報。UnityEngine.TouchPhase に None の情報を追加拡張。
	/// </summary>
	[Flags]
	public enum TouchInfo {
		/// <summary>
		/// タッチなし
		/// </summary>
		None = 0,

		// 以下は UnityEngine.TouchPhase の値に対応
		/// <summary>
		/// タッチ開始
		/// </summary>
		Began = 1, // 0b0001,
		/// <summary>
		/// タッチ中かつ移動中
		/// </summary>
		Moved = 3, // 0b0011,
		/// <summary>
		/// タッチ中かつ静止中
		/// </summary>
		Stationary = 5, // 0b0101
		/// <summary>
		/// タッチ終了
		/// </summary>
		Ended = 2, // 0b0010,
		/// <summary>
		/// システムがタッチの追跡をキャンセルしたとき
		/// </summary>
		Canceled = 4, // 0b0100,

		// -----------------------------------
		// ビットフラグ用
		// 末尾の桁が1か否かで区別
		Touching = 1, // 0b0001
	}

	public static class ExTouchInfo {

		public static TouchInfo Phase2Info(TouchPhase phase) {
			switch (phase) {
				case TouchPhase.Began		: return TouchInfo.Began;
				case TouchPhase.Moved		: return TouchInfo.Moved;
				case TouchPhase.Stationary	: return TouchInfo.Stationary;
				case TouchPhase.Ended		: return TouchInfo.Ended;
				case TouchPhase.Canceled	: return TouchInfo.Canceled;
				default						: return TouchInfo.None;
			}
		}

		public static bool IsTouching(this TouchInfo own) {
			return (own & TouchInfo.Touching) != 0;
		}

	}

}
