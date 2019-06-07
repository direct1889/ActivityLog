using UnityEngine;
using UGUI = UnityEngine.UI;


namespace du.di {

    public class Cursor : MonoBehaviour {
        #region constant
        Color ColorValid { get; } = new Color(1f, 0.8f, 0.3f, 0.7f);
        Color ColorInvalid { get; } = new Color(1f, 0.8f, 0.3f, 0.5f);
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
            UpdateColor(Mgr.Touch.IsTouch);
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
