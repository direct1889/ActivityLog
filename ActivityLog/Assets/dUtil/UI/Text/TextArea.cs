using UnityEngine;
using UGUI = UnityEngine.UI;

namespace du.dui {

    public interface ITextArea {
        string Text { set; get; }
        void Add(string text);
        void Add(char c);
    }

    public class TextArea : MonoBehaviour, ITextArea {
        #region field
        UGUI.Text m_textUI;
        RectTransform m_rect;
        #endregion

        #region property
        public string Text {
            set => m_textUI.text = value;
            get => m_textUI.text;
        }
        #endregion

        #region mono
        private void Awake() {
            Debug.LogError("TextArea awake.");
            m_textUI = transform.GetComponentInChildren<UGUI.Text>();
            m_rect = GetComponent<RectTransform>();
        }
        #endregion

        #region public
        public void Add(string text){ m_textUI.text += text; }
        public void Add(char c)     { m_textUI.text += c;    }
        #endregion
    }

}
