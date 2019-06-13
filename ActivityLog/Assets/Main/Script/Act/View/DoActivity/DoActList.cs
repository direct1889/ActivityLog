using UnityEngine;
// using UImage = UnityEngine.UI.Image;
using System.Collections.Generic;
using static du.Ex.ExCollection;
using System.Linq;
using System;
using UniRx;
using du.Cmp;

namespace Main.Act.View {

    #if false
    public interface IContentPanelAdapter : IHashTreeDataType<IContentPanelAdapter, string> {
        IContentPanel Impl { get; }
    }
    public class ContentPanelAdapter : IContentPanelAdapter {
        public IContentPanelAdapter Parent { get; }
        public string Key { get; }
        public IContentPanel Impl { get; }
    }
    #endif

    /// <summary>
    /// ContentPanel一覧
    /// - Activityが選択されたら通知する
    /// </summary>
    public interface IRxDoActList {
        /// <summary> Panelが押されたことを通知 </summary>
        IObservable<IActivity> ActivityChosen { get; }
    }

    /// <summary>
    /// ContentPanel一覧
    /// - Panelから見た親としてのList
    /// </summary>
    public interface IDoActListAsParent {
        /// <summary> 押されたPanelからの報告を受け取る </summary>
        void OnChosenActivity(IActivity content);
    }

    /// <summary>
    /// ContentPanel一覧
    /// Content実行時の選択UI(ContentPanelリスト)の提供
    /// </summary>
    public class DoActList : MonoBehaviour, IRxDoActList, IDoActListAsParent {
        #region field
        // IHashTree<IContentPanelAdapter, IContentPanelAdapter, string> m_panels = new HashTree<IContentPanelAdapter, IContentPanelAdapter, string>();

        IDictionary<string, IContentPanel> m_contentPanels = new Dictionary<string, IContentPanel>();
        Subject<IActivity> m_chosenActStream = new Subject<IActivity>();

        [SerializeField] GameObject m_panelsParent;
        [SerializeField] GameObject m_projPanelPref;
        [SerializeField] GameObject m_actPanelPref;
        #endregion

        #region IRxContentPanelList
        public IObservable<IActivity> ActivityChosen => m_chosenActStream;
        #endregion

        #region IContentPanelListAsParent
        public void OnChosenActivity(IActivity content) {
            m_chosenActStream.OnNext(content);
        }
        #endregion

        #region mono
        private void Start() { Initialize(); }
        #endregion

        #region private
        /// <summary>
        /// - データベースにContentが追加されたときに自動でPanelを生成
        /// - CSVファイルからPanelを一括生成
        /// </summary>
        private void Initialize() {
            CDB.Content.RxAdded.Subscribe(content => CreatePanel(content)).AddTo(this);
            // nullを渡してRootNodeから始める
            foreach (var child in CDB.Content.OrderedValues(null)) {
                CreatePanels(child);
            }
        }
        /// <summary> 再帰的にPanelを生成 </summary>
        private void CreatePanels(IContentAdapter content) {
            CreatePanel(content);
            if (content.IsProj) {
                foreach (var child in CDB.Content.OrderedValues(content.Proj)) {
                    CreatePanels(child);
                }
            }
        }
        /// <summary>
        /// Panelを生成
        /// - contentがDBに含まれていないと例外を投げる
        /// </summary>
        private void CreatePanel(IContentAdapter content) {
            var panel = Instantiate<GameObject>(
                content.IsProj ? m_projPanelPref : m_actPanelPref,
                m_panelsParent.transform);
            panel.transform.SetSiblingIndex((int)CDB.Content.SerialNumber(content));
            m_contentPanels.Add(content.Key, panel.GetComponent<ContentPanel>());
            m_contentPanels[content.Key].Initialize(content, this);
        }
        #endregion
    }

}
