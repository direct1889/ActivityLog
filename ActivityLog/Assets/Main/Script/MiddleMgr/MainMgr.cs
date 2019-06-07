using UnityEngine;
using UniRx;

namespace Main {

    public class MainMgr : MonoBehaviour {
        #region field
        [SerializeField] GameObject m_doActCanvas = null;
        #endregion

        #region mono
        private void Start() {
            Act.DB.ContentDB.Initialize();
            du.dui.RxButtonMgr.OnClickAsObservable("DoActivity")
                .Subscribe(_ => m_doActCanvas.SetActive(!m_doActCanvas.activeSelf))
                .AddTo(this);
        }
        #endregion
    }

}
