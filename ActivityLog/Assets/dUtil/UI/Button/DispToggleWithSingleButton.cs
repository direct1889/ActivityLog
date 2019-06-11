using UnityEngine;
using UGUI = UnityEngine.UI;
using UniRx;

namespace du.dui {

    /// <summary> ボタンが押されるたびにアクティブ/非アクティブを切り替える </summary>
    public class DispToggleWithSingleButton : MonoBehaviour {
        #region field
        [SerializeField] UGUI.Button m_toggleFactor;
        /// <summary>
        /// デフォルトで非アクティブにしたい場合はfalseにする
        /// - Hierarchy上で非アクティブにするとAwake()が呼ばれず非アクティブのまま
        /// </summary>
        [SerializeField] bool m_defaultIsActive;
        #endregion

        #region mono
        private void Awake() {
            m_toggleFactor
                .OnClickAsObservable()
                .Subscribe(_ => gameObject.SetActive(!gameObject.activeSelf))
                .AddTo(this);
            gameObject.SetActive(m_defaultIsActive);
        }
        #endregion
    }

}
