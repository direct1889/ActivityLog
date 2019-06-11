using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UGUI = UnityEngine.UI;
using UniRx;

namespace du.dui {

    public class DispToggleWithButton : MonoBehaviour {
        #region field
        [SerializeField] UGUI.Button m_toggleFactor;
        #endregion

        #region mono
        private void Awake() {
            m_toggleFactor
                .OnClickAsObservable()
                .Subscribe(_ => gameObject.SetActive(gameObject.activeSelf))
                .AddTo(this);
        }
        #endregion
    }

}
