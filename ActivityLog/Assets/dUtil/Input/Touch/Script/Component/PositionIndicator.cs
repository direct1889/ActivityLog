using UnityEngine;
using UGUI = UnityEngine.UI;
using static du.Ex.ExVector;

namespace du.di {

    public class PositionIndicator : MonoBehaviour {
        #region field
        UGUI.Text m_text;
        #endregion

        #region mono
        void Awake() {
            m_text = GetComponent<UGUI.Text>();
        }
        void Update() {
            m_text.text = UnityEngine.Input.mousePosition.DisZ().ToString();
        }
        #endregion
    }

}
