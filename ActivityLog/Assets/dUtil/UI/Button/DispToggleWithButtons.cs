using UnityEngine;
using UGUI = UnityEngine.UI;
using UniRx;

namespace du.dui {

    /// <summary> ボタンが押されるたびにアクティブ/非アクティブを切り替える </summary>
    public class DispToggleWithButtons : MonoBehaviour {
        #region field
        [SerializeField] UGUI.Button m_activateFactor;
        [SerializeField] UGUI.Button m_inactivateFactor;
        /// <summary>
        /// デフォルトで非アクティブにしたい場合はfalseにする
        /// - Hierarchy上で非アクティブにするとAwake()が呼ばれず非アクティブのまま
        /// </summary>
        [SerializeField] bool m_defaultIsActive;
        #endregion

        #region mono
        private void Awake() {
            m_activateFactor
                .OnClickAsObservable()
                .Subscribe(_ => gameObject.SetActive(true))
                .AddTo(this);
            m_inactivateFactor
                .OnClickAsObservable()
                .Subscribe(_ => gameObject.SetActive(false))
                .AddTo(this);
            gameObject.SetActive(m_defaultIsActive);
        }
        #endregion
    }

}
