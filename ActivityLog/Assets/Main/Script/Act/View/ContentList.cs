using UnityEngine;
// using UImage = UnityEngine.UI.Image;
using System.Collections.Generic;
using static du.Ex.ExList;
using System.Linq;
using System;
using UniRx;

namespace Main.Act.View {

    public interface IRxContentList {
        IObservable<IRxContentPanel> CreatedPanel { get; }
    }

    // public interface IContentList : IROContentList { }

    public class ContentList : MonoBehaviour, IRxContentList {
        #region field
        IList<IContentPanel> m_contentPanels = new List<IContentPanel>();
        Subject<IRxContentPanel> m_createdPanelStream = new Subject<IRxContentPanel>();

        [SerializeField] GameObject m_panelsParent = null;
        [SerializeField] GameObject m_projPanelPref = null;
        [SerializeField] GameObject m_actPanelPref = null;
        #endregion

        #region getter
        public IObservable<IRxContentPanel> CreatedPanel => m_createdPanelStream;
        #endregion

        #region mono
        private void Start() { Initialize(); }
        #endregion

        #region private
        private void Initialize() {
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
            m_createdPanelStream.OnNext(m_contentPanels.Back());
        }
        private void CreateContentPanel(IROContent content, GameObject panelPref) {
            var panel = Instantiate<GameObject>(panelPref, m_panelsParent.transform);
            m_contentPanels.Add(panel.GetComponent<ContentPanel>());
            m_contentPanels.Back().Initialize(content);
        }
        #endregion
    }

}
