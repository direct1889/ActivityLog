using UnityEngine;
using static du.Ex.ExVector;
using TouchScript.Gestures.TransformGestures;
using static UniRx.UnityEventExtensions;
using System;
using UniRx;
using GestureState = TouchScript.Gestures.Gesture.GestureState;

namespace Main.Act.View {

    public interface IActBlockBeginMarker {
        IObservable<float> OnTransformComplete { get; }
        void ResetPos();
    }

    public class ActBlockBeginMarker : MonoBehaviour, IActBlockBeginMarker {
        #region const
        static Vector3 DefaultLocalPos { get; } = new Vector3(250f, 0f, 0f);
        #endregion

        #region field
        RectTransform m_recT;
        TransformGesture m_gesture;
        #endregion

        #region getter
        public IObservable<float> OnTransformComplete {
            get => m_gesture.OnTransformComplete.AsObservable().Select(_ => transform.localPosition.y);
        }
        #endregion

        #region public
        public void ResetPos() {
            if (!(m_recT is null)) {
                if (m_gesture.State != GestureState.Changed) {
                    m_recT.localPosition = DefaultLocalPos;
                }
            }
        }
        #endregion

        #region mono
        private void Awake() {
            m_recT = GetComponent<RectTransform>();
            m_gesture = GetComponent<TransformGesture>();
        }
        private void Update() {
            if (m_gesture.State == GestureState.Changed) {
                m_recT.localPosition = m_recT.localPosition.ReX(DefaultLocalPos.x);
            }
        }
        #endregion
    }

}