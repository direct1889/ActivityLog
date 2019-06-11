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
        IList<IContentPanel> m_contentPanels = new List<IContentPanel>();
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
            DB.ContentDB.Act.RxAdded.Subscribe(act => CreateActPanel(act)).AddTo(this);
            DB.ContentDB.Proj.RxAdded.Subscribe(proj => CreateProjPanel(proj)).AddTo(this);
            foreach (var proj in DB.ContentDB.Proj.Sorted()) {
                CreateProjPanel(proj);
                foreach (var act in DB.ContentDB.Act.Sorted(proj)) {
                    CreateActPanel(act);
                }
            }
        }
        private void CreateProjPanel(IProject content) {
            CreateContentPanel(content, m_projPanelPref);
        }
        private void CreateActPanel(IROContent content) {
            CreateContentPanel(content, m_actPanelPref);
            // m_createdPanelStream.OnNext(m_contentPanels.Back());
        }
        private void CreateContentPanel(IROContent content, GameObject panelPref) {
            var panel = Instantiate<GameObject>(panelPref, m_panelsParent.transform);
            m_contentPanels.Add(panel.GetComponent<ContentPanel>());
            m_contentPanels.Back().Initialize(content, this);
        }
        #endregion
    }

}
