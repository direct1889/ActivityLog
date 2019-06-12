using UnityEngine;
// using UImage = UnityEngine.UI.Image;
using System.Collections.Generic;
using static du.Ex.ExList;
using System.Linq;
using System;
using UniRx;

namespace Main.Act.View {

    /// <summary>
    /// Content一覧
    /// Content = Project, Activity
    /// 事前登録済Contentの管理、Content実行時の選択UIの提供
    /// </summary>
    public interface IRxContentList {
        // IObservable<IRxContentPanel> CreatedPanel { get; }
        IObservable<IROContent> ActivityChosen { get; }
    }

    public interface IContentListAsParent {
        void OnChosenActivity(IROContent content);
    }

    public interface IContentList {
    }

    // public interface IContentList : IROContentList { }

    public class ContentList : MonoBehaviour, IRxContentList, IContentListAsParent, IContentList {
        #region field
        IDictionary<string, IContentPanel> m_contentPanels = new Dictionary<string, IContentPanel>();
        Subject<IROContent> m_chosenActStream = new Subject<IROContent>();

        [SerializeField] GameObject m_panelsParent;
        [SerializeField] GameObject m_projPanelPref;
        [SerializeField] GameObject m_actPanelPref;
        #endregion

        #region getter
        public IObservable<IROContent> ActivityChosen => m_chosenActStream;
        #endregion

        #region mono
        private void Start() { Initialize(); }
        #endregion

        #region public
        public void OnChosenActivity(IROContent content) {
            m_chosenActStream.OnNext(content);
        }
        #endregion

        #region private
        private void Initialize() {
            DB.ContentDB.Act.RxAdded.Subscribe(content => CreatePanel(content)).AddTo(this);
            // nullを渡してRootNodeから始める
            foreach (var child in DB.ContentDB.Proj.Sorted(null)) {
                CreatePanels(child);
            }
        }
        private void CreatePanels(IContent content) {
            CreatePanel(content);
            if (content.IsProj) {
                foreach (var child in DB.ContentDB.Proj.Sorted(content.Proj)) {
                    CreatePanels(child);
                }
            }
        }
        private void CreatePanel(IContent content) {
            if (content.IsProj) { CreatePanel(content.Proj, m_projPanelPref); }
            else { CreatePanel(content.Act, m_actPanelPref); }
        }
        private void CreatePanel(IROContent content, GameObject panelPref) {
            var panel = Instantiate<GameObject>(panelPref, m_panelsParent.transform);
            m_contentPanels.Add(content.Key, panel.GetComponent<ContentPanel>());
            m_contentPanels[content.Key].Initialize(content, this);
        }
        #endregion
    }

}
