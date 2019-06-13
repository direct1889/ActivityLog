/*
* AUTHORITY : tempura
* URL : https://qiita.com/tempura/items/4a5482ff6247ec8873df
* LICENSE : UNK
*/

using UnityEngine;
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
        int Count { get; }
        /// <summary>
        ///
        /// </summary>
        int FingerId(int index);
        /// <summary>
        /// タッチ段階
        /// </summary>
        TouchStep Step(int index);
        /// <summary>
        /// タッチ座標(タッチされていなければnull)
        /// </summary>
        Vector3? Position(int index);
        /// <summary>
        /// 直前フレームとのスワイプ距離(タッチされていなければnull)
        /// タッチ開始フレームなら0
        /// </summary>
        // Vector3? GetDeltaPosition(int index);
    }

    /// <summary>
    /// タッチ情報管理の委譲先抽象クラス
    /// Editorか実機かで派生クラスを差し替える
    /// </summary>
    public abstract class TouchMgrImpl : ITouchMgrImpl {
        #region public
        public abstract int Count { get; }
        public abstract int FingerId(int index);
        public abstract TouchStep Step(int index);
        public abstract Vector3? Position(int index);
        // public abstract Vector3? GetDeltaPosition(int index);
        #endregion
    }
    /// <summary>
    /// タッチ情報管理の委譲先具象クラス
    /// Editor用
    /// </summary>
    public class TouchMgrImpl4Editor : TouchMgrImpl {
        public override int Count => Step(0).IsTouching() ? 1 : 0;
        public override int FingerId(int index) => Count;
        public override TouchStep Step(int index) {
            if (Input.GetMouseButtonDown(0)) { return TouchStep.Began; }
            if (Input.GetMouseButton(0))     { return TouchStep.Moved; }
            if (Input.GetMouseButtonUp(0))   { return TouchStep.Ended; }
            return TouchStep.None;
        }
        public override Vector3? Position(int index) {
            return Step(index).IsTouching() ? Input.mousePosition : (Vector3?)null;
        }
        // public override Vector3? GetDeltaPosition(int i) {
            // return GetTouch(i).IsTouching() ? : (Vector3?)null;
        // }
    }

    /// <summary>
    /// タッチ情報管理の委譲先具象クラス
    /// 実機用
    /// </summary>
    public class TouchMgrImpl4ActualMachine : TouchMgrImpl {
        public override int Count => Input.touchCount;
        public override int FingerId(int index) => Input.GetTouch(index).fingerId;
        public override TouchStep Step(int index) {
            if (Input.touchCount >= index) {
                return Input.GetTouch(index).phase.Phase2Step();
            }
            else { return TouchStep.None; }
        }
        public override Vector3? Position(int index) {
            if (Input.touchCount >= index) {
                return Input.GetTouch(index).position;
            }
            else { return null; }
        }
        // public override Vector3? GetDeltaPosition(int index) {
        //     if (Input.touchCount >= index) {
        //         return Input.GetTouch(index).deltaPosition;
        //     }
        //     else { return null; }
        // }
    }

    public class TouchMgr : du.App.SingletonMonoBehaviour<TouchMgr> {

        #region field
        ITouchMgrImpl m_impl;
        #endregion

        #region property
        public int       TouchCount  => m_impl.Count;
        public TouchStep Step0       => Step           (0);
        public bool      IsTouch     => Step(0).IsTouching();
        public Vector2?  Pos0        => Position       (0);
        public Vector3?  Pos3D0      => Position3D     (0);
        public Vector2?  WorldPos0   => WorldPosition  (0);
        public Vector3?  WorldPos3D0 => WorldPosition3D(0);
        #endregion

        #region mono
        private void Awake() {
            if (Application.isEditor) { m_impl = new TouchMgrImpl4Editor(); }
            else { m_impl = new TouchMgrImpl4ActualMachine(); }
        }
        #endregion

        #region public
        /// <summary>
        /// 例えば2本指でタッチしているとき、同じ指から発せられるタッチイベントのindexはフレーム間で必ずしも維持されない
        /// 一方、FingerIdは維持される
        /// </summary>
        public int FingerId(int index) => m_impl.FingerId(index);

        /// <summary>
        /// タッチ情報を取得(エディタと実機を考慮)
        /// </summary>
        /// <returns>タッチ情報。タッチされていない場合は null</returns>
        public TouchStep Step(int index) => m_impl.Step(index);

        /// <summary>
        /// タッチポジションを取得(エディタと実機を考慮)
        /// </summary>
        /// <returns>タッチポジション。タッチされていない場合は null</returns>
        public Vector2? Position(int index) => m_impl.Position(index);
        public Vector3? Position3D(int index) => m_impl.Position(index);

        /// <summary>
        /// 直前の移動距離
        /// </summary>
        /// <returns>タッチポジション。タッチされていない場合は null</returns>
        // public Vector3? GetDeltaPosition(int index) {
            // return m_impl.GetDeltaPosition(index);
        // }

        /// <summary>
        /// タッチワールドポジションを取得(エディタと実機を考慮)
        /// </summary>
        /// <param name='camera'>カメラ</param>
        /// <returns>タッチワールドポジション。タッチされていない場合は null</returns>
        public Vector2? WorldPosition(Camera camera, int i) {
            var pos = Position(i);
            if (pos == null) { return null; }
            else { return camera.ScreenToWorldPoint(pos.DisN0()); }
        }
        public Vector2? WorldPosition(int i) {
            var pos = Position(i);
            if (pos == null) { return null; }
            else { return Camera.main.ScreenToWorldPoint(pos.DisN0()); }
        }

        public Vector3? WorldPosition3D(Camera camera, int i) {
            var pos = Position3D(i);
            if (pos == null) { return null; }
            else { return camera.ScreenToWorldPoint(pos.DisN0()); }
        }
        public Vector3? WorldPosition3D(int i) {
            var pos = Position3D(i);
            if (pos == null) { return null; }
            else { return Camera.main.ScreenToWorldPoint(pos.DisN0()); }
        }
        #endregion

    }

}
