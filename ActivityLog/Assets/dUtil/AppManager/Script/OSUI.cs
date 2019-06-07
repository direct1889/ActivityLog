using UnityEngine;


namespace du.App {

    public class OSUI : MonoBehaviour {
        #region field
        [SerializeField] du.di.Cursor m_cursor;
        #endregion

        #region mono
        private void Start() {
            if (du.Mgr.App.MouseCursorMode == du.App.MouseCursorMode.Detail) {
                Debug.Log("OSUI booted.");
            }
            else {
                SetEnable(false);
            }
        }
        #endregion

        #region public
        public void SetEnable(bool isEnable) {
            m_cursor.gameObject.SetActive(isEnable);
        }
        #endregion

    }

}
