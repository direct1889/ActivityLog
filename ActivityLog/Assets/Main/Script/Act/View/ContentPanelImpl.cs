using UnityEngine;
using UImage = UnityEngine.UI.Image;
using UColor = UnityEngine.Color;
using System.Collections.Generic;
using static du.Ex.ExVector;

namespace Main.Act.View {

    public interface IContentPanelImpl {
        #region public
        /// <summary> IProjectに合わせてラベル/フレーム色/チェック有無 </summary>
        void SetContent(IROContent content);
        /// <summary> Indentに合わせてパネルを縮小 </summary>
        void SetIndent(int indent);
        #endregion
    }

    public class ContentPanelImpl : MonoBehaviour, IContentPanelImpl {
        #region field
        UImage m_frame;
        du.Cmp.RecT.RecTRightTop m_recTRT;

        [SerializeField] TMPro.TMP_Text m_name;
        [SerializeField] GameObject m_effectiveCert;
        #endregion

        #region mono
        private void Awake() {
            m_frame = GetComponent<UImage>();
            // Prefabのため親は常に設定済
            m_recTRT = new du.Cmp.RecT.RecTRightTop(GetComponent<RectTransform>());
        }
        #endregion

        #region public
        /// <summary> IContentに合わせてラベル/フレーム色/チェック有無 </summary>
        public void SetContent(IROContent content) {
            m_name.text = content.Name;
            m_frame.color = content.Color;
            m_effectiveCert.SetActive(content.IsEffective);
        }
        /// <summary> Indentに合わせてパネルを縮小 </summary>
        public void SetIndent(int indent) {
            if (indent > 0) {
                m_recTRT.Width = ConstPAList.RootWidth - ConstPAList.IndentWidth * indent;
            }
        }
        #endregion
    }

}
