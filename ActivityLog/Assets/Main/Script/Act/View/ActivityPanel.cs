using UnityEngine;
using UImage = UnityEngine.UI.Image;
using UColor = UnityEngine.Color;
using System.Collections.Generic;
using static du.Ex.ExVector;

namespace Main.Act.View {

    public interface IROActivityPanel {
        // int Indent { get; }
    }

    public interface IActivityPanel : IROActivityPanel {
        void Initialize(IProject proj, Transform parent, IROProjectPanel parentProj = null);
    }

    public class ActivityPanel : MonoBehaviour, IActivityPanel {
        #region const
        /// <summary> 第一階層PanelのLocalWidth </summary>
        const float WidthMax = 840;
        /// <summary> インデント1段階辺りの字下げ幅 </summary>
        const float IndentWidth = 40;
        /// <summary> 限界インデント深度 </summary>
        const int IndentMax = 4;
        #endregion

        #region field
        IProject m_proj = null;

        UImage m_frame = null;
        [SerializeField] TMPro.TMP_Text m_name = null;
        [SerializeField] GameObject m_effectiveCert = null;
        #endregion

        // #region getter
        // public int Indent { get { return ParentProj == null ? 0 : ParentProj.Indent; } }
        // #endregion

        #region private property
        private du.Cmp.RecT.RecTRightTop RecTLT { get; set; }
        private IROProjectPanel ParentProj { get; set; }
        // private UColor Color { get { return m_frame.color; } set { m_frame.color = value; } }
        private bool IsDropDown { get; set; } = false;
        #endregion

        #region mono
        private void Awake() {
            m_frame = GetComponent<UImage>();
            RecTLT = new du.Cmp.RecT.RecTRightTop(GetComponent<RectTransform>());
        }
        #endregion

        #region public
        public void Initialize(IProject proj, Transform parent, IROProjectPanel parentProj = null) {
            if (m_proj == null) {
                // 内容、色
                m_proj = proj;
                ReflectProjOnView();
                // 親子関係、RecT
                ParentProj = parentProj;
                RecTLT.Initialize(parent);
            }
        }
        #endregion

        #region private
        /// <summary> IProjectに合わせてラベル/フレーム色/チェック有無 </summary>
        private void ReflectProjOnView() {
            m_name.text = m_proj.Name;
            m_frame.color = m_proj.Color;
            m_effectiveCert.SetActive(m_proj.IsEffective);
        }
        /// <summary> Indentに合わせてパネルを縮小 </summary>
        private void ReflectIndentOnView() {
            // if (Indent > 0) {
                // RecTLT.Width = WidthMax - IndentWidth * Indent;
            // }
        }
        #endregion
    }

}
