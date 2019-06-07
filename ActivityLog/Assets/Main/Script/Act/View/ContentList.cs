using UnityEngine;
// using UImage = UnityEngine.UI.Image;
using System.Collections.Generic;
using static du.Ex.ExList;
using System.Linq;
using UniRx;

namespace Main.Act.View {

    // public interface IROContentList { }

    // public interface IContentList : IROContentList { }

    public class ContentList : MonoBehaviour {// , IContentPanel {
        #region field
        IList<IContentPanel> m_contentPanels = null;

        [SerializeField] GameObject m_panelsParent = null;
        [SerializeField] GameObject m_projPanelPref = null;
        [SerializeField] GameObject m_actPanelPref = null;
        #endregion

        #region getter
        #endregion

        #region mono
        private void Start() {
            m_contentPanels = new List<IContentPanel>();
            Initialize();
        }
        #endregion

        #region private
        private void Initialize() {
            foreach (var proj in ContentDB.Proj.Sorted()) {
                CreateContentPanel(proj, m_projPanelPref);
                foreach (var act in ContentDB.Act.Sorted(proj)) {
                    CreateContentPanel(act, m_actPanelPref);
                }
            }
        }
        private void CreateContentPanel(IROContent content, GameObject panelPref) {
            var panel = Instantiate<GameObject>(panelPref, m_panelsParent.transform);
            m_contentPanels.Add(panel.GetComponent<ContentPanel>());
            m_contentPanels.Back().Initialize(content);
        }
        #endregion
    }

}
