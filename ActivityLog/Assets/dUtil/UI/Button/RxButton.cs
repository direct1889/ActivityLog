using UnityEngine;
using UButtion = UnityEngine.UI.Button;
using UniRx;

namespace du.dui {

    public interface IRxButton {
        System.IObservable<Unit> OnClickAsObservable { get; }
    }

    public class RxButton : MonoBehaviour, IRxButton {
        #region field
        UButtion m_button;

        [SerializeField] string m_label = "";
        #endregion

        #region getter
        public System.IObservable<Unit> OnClickAsObservable => m_button.OnClickAsObservable();
        #endregion

        #region mono
        void Awake() {
            m_button = GetComponent<UButtion>();
            if (m_label != "") {
                RxButtonMgr.RegisterButton(m_label, this);
            }
        }
        #endregion
    }

}
