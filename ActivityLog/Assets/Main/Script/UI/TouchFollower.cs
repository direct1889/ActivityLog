#if false
using UnityEngine;
using UGUI = UnityEngine.UI;

namespace du.di {

    public class TouchFollower : MonoBehaviour {
        #region constant
        enum State { }
        #endregion

        #region field
        RectTransform m_rectTf;
        [SerializeField] UGUI.Image m_image;
        #endregion

        #region mono
        void Awake() {
            m_rectTf = GetComponent<RectTransform>();
        }
        void Update() {
            UpdateColor(TouchMgr.IsTouch);
            // m_rectTf.position = Touch.GetTouchPosition(0);
            m_rectTf.position = Input.mousePosition;
        }
        #endregion

        #region private
        private void UpdateColor(bool isActive) {
            m_image.color = isActive ? ColorValid : ColorInvalid;
        }
        #endregion
    }

}
#endif