using System.Collections.Generic;
using UnityEngine;
using UGUI = UnityEngine.UI;
using UniRx;

namespace du.dui {

    /// <summary> ボタンが押されるたびにアクティブ/非アクティブを切り替える </summary>
    public class DispToggleWithManyButtons : MonoBehaviour {
        #region field
        [SerializeField] List<UGUI.Button> m_activateFactors;
        [SerializeField] List<UGUI.Button> m_inactivateFactors;
        /// <summary>
        /// デフォルトで非アクティブにしたい場合はfalseにする
        /// - Hierarchy上で非アクティブにするとAwake()が呼ばれず非アクティブのまま
        /// </summary>
        [SerializeField] bool m_defaultIsActive;
        #endregion

        #region mono
        private void Awake() {
            if (!(m_activateFactors is null)) {
                foreach (var factor in m_activateFactors) {
                    factor
                        .OnClickAsObservable()
                        .Subscribe(_ => gameObject.SetActive(true))
                        .AddTo(this);
                }
            }
            if (!(m_inactivateFactors is null)) {
                foreach (var factor in m_inactivateFactors) {
                    factor
                        .OnClickAsObservable()
                        .Subscribe(_ => gameObject.SetActive(false))
                        .AddTo(this);
                }
            }
            gameObject.SetActive(m_defaultIsActive);
        }
        #endregion
    }

}
