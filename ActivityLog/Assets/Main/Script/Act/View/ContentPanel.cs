using UnityEngine;
using UGUI = UnityEngine.UI;
using System.Collections.Generic;
using static du.Ex.ExVector;
using System;
using UniRx;

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

    public interface IRxContentPanel {
        IObservable<IROContent> Pressed { get; }
    }

    public interface IContentPanel : IRxContentPanel {
        void Initialize(IROContent content);
    }

    public class ContentPanel : MonoBehaviour, IContentPanel {
        #region field
        IROContent m_content = null;
        ContentPanelImpl m_ppi = null;
        UGUI.Button m_button = null;
        Subject<IROContent> m_subject = null;
        #endregion

        #region getter
        public IObservable<IROContent> Pressed => m_button.OnClickAsObservable().Select(_ => m_content);
        #endregion

        #region mono
        private void Awake() {
            m_ppi = transform.GetComponentInChildren<ContentPanelImpl>();
            m_button = transform.GetComponentInChildren<UGUI.Button>();
        }
        #endregion

        #region public
        public void Initialize(IROContent content) {
            if (m_content == null) {
                m_content = content;
                m_ppi.SetContent(m_content); // 内容、色
                m_ppi.SetIndent(content.ParentCount); // 親子関係、RecT
            }
        }
        #endregion

        #region private
        private void DropDown() {

        }
        #endregion
    }

}
