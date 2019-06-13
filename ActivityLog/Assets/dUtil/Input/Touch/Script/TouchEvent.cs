#if false
using UnityEngine;

namespace du.di {

    public interface IROTouchEvent {
        Vector3 Enter    { get; } // 開始座標
        Vector3? Current { get; } // 現在座標(すでに終了済みならnull)
        Vector3? Exit    { get; } // 終了座標(まだ終了していなければnull)
        bool HasExited   { get; } // すでに終了したか
    }
    public interface ITouchEvent : IROTouchEvent {
        void PositionUpdate(Vector3 current);
        void OnExit(Vector3 exit);
    }

    public class TouchEvent : ITouchEvent {

        #region property
        public Vector3 Enter    { private set; get; } // 開始座標
        public Vector3? Current { private set; get; } // 現在座標(すでに終了済みならnull)
        public Vector3? Exit    { private set; get; } // 終了座標(まだ終了していなければnull)
        public bool HasExited   => Exit != null;      // すでに終了したか
        #endregion

        #region ctor/dtor
        public TouchEvent(Vector3 enter) {
            Enter = enter;
            Exit = null;
            PositionUpdate(enter);
        }
        #endregion

        #region public
        public void PositionUpdate(Vector3 current) {
            if (!HasExited) {
                Current = current;
            }
        }
        public void OnExit(Vector3 exit) {
            if (!HasExited) {
                Exit = exit;
                Current = null;
            }
        }
        #endregion

    }

}
#endif
