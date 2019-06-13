using UnityEngine;
using UGUI = UnityEngine.UI;
using System.Collections.Generic;
using static du.Ex.ExVector;
using System;
using UniRx;

namespace Main.Act.View {

    /// <summary> ContentPanelListに関する定数 </summary>
    public static class ConstContentList {
        #region const
        /// <summary> 第一階層PanelのLocalWidth </summary>
        public const float RootWidth = 840;
        /// <summary> インデント1段階辺りの字下げ幅 </summary>
        public const float IndentWidth = 40;
        /// <summary> 限界インデント深度 </summary>
        public const int IndentMax = 4;
        #endregion
    }

    /// <summary> ContentPanelListを構成するPanel </summary>
    public interface IContentPanel {
        /// <summary> GameObjectの有効/無効を切り替える </summary>
        void SetActive(bool value);
        /// <summary> 初期化 </summary>
        void Initialize(IContentAdapter content, IDoActListAsParent parent);
    }

    /// <summary> ContentPanelListを構成するPanel </summary>
    public class ContentPanel : MonoBehaviour, IContentPanel {
        #region field
        IContentAdapter m_content;
        ContentPanelUI m_ui;
        IDoActListAsParent m_parent;
        bool m_isDropDown = true;
        #endregion

        #region mono
        private void Awake() {
            m_ui = transform.GetComponentInChildren<ContentPanelUI>();
        }
        /// <summary> UGUI.Buttonから呼ばれる </summary>
        public void OnClicked() => m_parent.OnChosenActivity(m_content.Act);
        public void OnClickedDropDonw() {
            m_parent.OnPressedDropDown(m_content, !m_isDropDown);
            m_isDropDown = !m_isDropDown;
        }
        #endregion

        #region public
        public void SetActive(bool value) {
            gameObject.SetActive(value);
        }
        public void Initialize(IContentAdapter content, IDoActListAsParent parent) {
            if (m_content == null) {
                m_content = content;
                m_ui.SetContent(m_content.Data); // 内容、色
                m_ui.SetIndent(m_content.Data.ParentCount); // 親子関係、RecT
                m_parent = parent;
            }
        }
        #endregion

        #region private
        private void RollUp() {
        }
        private void DropDown() {
        }
        #endregion
    }

}
