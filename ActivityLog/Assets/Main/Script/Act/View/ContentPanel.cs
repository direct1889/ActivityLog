﻿using UnityEngine;
using UGUI = UnityEngine.UI;
using System.Collections.Generic;
using static du.Ex.ExVector;
using System;
using UniRx;

namespace Main.Act.View {

    /// <summary> ContentListに関する定数 </summary>
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

    public interface IRxContentPanel {
        // IObservable<IActivity> Pressed { get; }
    }

    public interface IContentPanel : IRxContentPanel {
        void Initialize(IContentProxy content, IContentListAsParent parent);
    }

    /// <summary> ContentListを構成するPanel </summary>
    public class ContentPanel : MonoBehaviour, IContentPanel {
        #region field
        IContentProxy m_content;
        ContentPanelUI m_ui;
        // UGUI.Button m_button;
        IContentListAsParent m_parent;
        #endregion

        #region getter
        // public IObservable<IROContent> Pressed => m_button.OnClickAsObservable().Select(_ => m_content);
        #endregion

        #region mono
        private void Awake() {
            m_ui = transform.GetComponentInChildren<ContentPanelUI>();
            // m_button = transform.GetComponentInChildren<UGUI.Button>();
        }
        /// <summary> UGUI.Buttonから呼ばれる </summary>
        public void OnClicked() {
            m_parent.OnChosenActivity(m_content.Act);
        }
        #endregion

        #region public
        public void Initialize(IContentProxy content, IContentListAsParent parent) {
            IReadOnlyList<string> a = new List<string>();
            Dictionary<string, string> b = new Dictionary<string, string>();
            IDictionary<string, string> c = b;
            IReadOnlyDictionary<string, string> d = b;
            c["hoge"] = "";
            if (m_content == null) {
                m_content = content;
                m_ui.SetContent(m_content.Data); // 内容、色
                m_ui.SetIndent(m_content.Data.ParentCount); // 親子関係、RecT
                m_parent = parent;
            }
        }
        #endregion

        #region private
        private void DropDown() {

        }
        #endregion
    }

}
