using UnityEngine;
using UImage = UnityEngine.UI.Image;
using UColor = UnityEngine.Color;
using System.Collections.Generic;
using static du.Ex.ExVector;

namespace Main.Act.View {

    public interface IProjectPanelImpl {
        #region public
        /// <summary> IProjectに合わせてラベル/フレーム色/チェック有無 </summary>
        void SetProject(IProject proj);
        /// <summary> Indentに合わせてパネルを縮小 </summary>
        void SetIndent(int indent);
        #endregion
    }

    public class ProjectPanelImpl : MonoBehaviour, IProjectPanelImpl {
        #region field
        UImage m_frame = null;
        du.Cmp.RecT.RecTRightTop m_recTRT = null;

        [SerializeField] TMPro.TMP_Text m_name = null;
        [SerializeField] GameObject m_effectiveCert = null;
        #endregion

        #region mono
        private void Awake() {
            m_frame = GetComponent<UImage>();
            // Prefabのため親は常に設定済
            m_recTRT = new du.Cmp.RecT.RecTRightTop(GetComponent<RectTransform>());
        }
        private void Update() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Return)) {
                Debug.LogError($"{m_name.text}:{m_recTRT}");
            }
            if (UnityEngine.Input.GetKeyDown(KeyCode.LeftArrow)) {
            }
        }
        #endregion

        #region public
        /// <summary> IProjectに合わせてラベル/フレーム色/チェック有無 </summary>
        public void SetProject(IProject proj) {
            m_name.text = proj.Name;
            m_frame.color = proj.Color;
            m_effectiveCert.SetActive(proj.IsEffectiveDefault);
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
