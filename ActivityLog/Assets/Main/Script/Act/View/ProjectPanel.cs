using UnityEngine;
using UImage = UnityEngine.UI.Image;
using UColor = UnityEngine.Color;
using System.Collections.Generic;
using static du.Ex.ExVector;

namespace Main.Act.View {

    public static class ConstPAList {
        #region const
        /// <summary> 第一階層PanelのLocalWidth </summary>
        public const float RootWidth = 840;
        /// <summary> インデント1段階辺りの字下げ幅 </summary>
        public const float IndentWidth = 40;
        /// <summary> 限界インデント深度 </summary>
        public const int IndentMax = 4;
        #endregion
    }

    public interface IROProjectPanel {
        // int Indent { get; }
    }

    public interface IProjectPanel : IROProjectPanel {
        void Initialize(IProject proj, Transform parent, IROProjectPanel parentProj = null);
    }

    public class ProjectPanel : MonoBehaviour, IProjectPanel {
        #region field
        IProject m_proj = null;
        // IROProjectPanel m_parentProj = null;
        ProjectPanelImpl m_ppi = null;
        #endregion

        // #region getter
        // public int Indent { get { return m_parentProj == null ? 0 : m_parentProj.Indent; } }
        // #endregion

        #region mono
        private void Awake() {
            m_ppi = transform.GetComponentInChildren<ProjectPanelImpl>();
        }
        #endregion

        #region public
        public void Initialize(IProject proj, Transform parent, IROProjectPanel parentProj = null) {
            if (m_proj == null) {
                m_proj = proj;
                // m_parentProj = parentProj;
                m_ppi.SetProject(m_proj); // 内容、色
                m_ppi.SetIndent(proj.ParentCount); // 親子関係、RecT
            }
        }
        #endregion

        #region private
        private void DropDown() {

        }
        #endregion
    }

}
