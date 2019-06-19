using UnityEngine;
using static du.Ex.ExVector;
using TouchScript.Gestures.TransformGestures;
using static UniRx.UnityEventExtensions;
using System;
using UniRx;

namespace Main.Act.View {

    public interface IActBlockBeginMarker {
        IObservable<float> OnTransformComplete { get; }
        void ResetPos();
    }

    public class ActBlockBeginMarker : MonoBehaviour, IActBlockBeginMarker {
        float localX;
        RectTransform m_recT;
        TransformGesture m_gesture;

        static readonly Vector3 s_defaultLocalPos = new Vector3(250f, 0f, 0f);

        public Transform Transform { get { return transform; } }
        public IObservable<float> OnTransformComplete {
        // public IObservable<TouchScript.Gestures.Gesture> OnTransformComplete {
            get => m_gesture.OnTransformComplete.AsObservable().Select(_ => transform.localPosition.y);
            // get => m_gesture.OnTransformComplete.AsObservable();
        }

        public void ResetPos() {
            if (!(m_recT is null)) {
                m_recT.localPosition = s_defaultLocalPos;
            }
        }

        private void Awake() {
            m_recT = GetComponent<RectTransform>();
            localX = m_recT.localPosition.x;
            m_gesture = GetComponent<TransformGesture>();
        }
        private void Update() {
            m_recT.localPosition = m_recT.localPosition.ReX(localX);
        }
    }

}