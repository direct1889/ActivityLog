using UnityEngine;
using UGUI = UnityEngine.UI;


namespace du.dui {

    public class TextBox : MonoBehaviour {
        #region field
        [SerializeField] UGUI.Text m_text;
        RectTransform m_rect;
        #endregion

        #region mono
        private void Awake() {
            m_rect = GetComponent<RectTransform>();
        }
        #endregion

        #region public
        public void locate(int id) {
            m_rect.anchorMin = new Vector2(0f, 1f);
            m_rect.anchorMax = new Vector2(0f, 1f);
            m_rect.pivot     = new Vector2(0f, 1f);
            m_rect.anchoredPosition = new Vector2(310 * (id / 10), -110 * (id % 10));
            m_rect.sizeDelta = new Vector2(300f, 100f);
        }
        public void SetText(string text) {
            m_text.text = text;
        }
        #endregion
    }

}
