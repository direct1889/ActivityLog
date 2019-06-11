using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace du.dui {

    public static class RxButtonMgr {
        #region field
        static IDictionary<string, RxButton> m_buttons = new Dictionary<string, RxButton>();
        #endregion

        #region public
        public static System.IObservable<Unit> OnClickAsObservable(string key) {
            return m_buttons[key]?.OnClickAsObservable;
        }
        public static void RegisterButton(string key, RxButton rxButton) {
            m_buttons[key] = rxButton;
        }
        #endregion
    }

}
