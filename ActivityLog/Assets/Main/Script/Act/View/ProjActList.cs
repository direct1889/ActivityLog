using UnityEngine;
// using UImage = UnityEngine.UI.Image;
using System.Collections.Generic;
using static du.Ex.ExList;
using System.Linq;
using UniRx;

namespace Main.Act.View {

    // public interface IROProjActList { }

    // public interface IProjActList : IROProjActList { }

    public class ProjActList : MonoBehaviour {// , IProjActPanel {
        #region field
        IList<IProjectPanel> m_projActPanels = null;

        [SerializeField] GameObject m_panelsParent = null;
        [SerializeField] GameObject m_projPanelPref = null;
        [SerializeField] GameObject m_actPanelPref = null;
        #endregion

        #region getter
        #endregion

        #region mono
        private void Start() {
            m_projActPanels = new List<IProjectPanel>();
            Initialize();
        }
        #endregion

        #region private
        private void Initialize() {
            foreach (var proj in ProjActDB.Proj.Projects()) {
                CreateProjectPanel(proj);
            }
        }
        private void CreateProjectPanel(IProject proj) {
            var panel = Instantiate<GameObject>(m_projPanelPref, m_panelsParent.transform);
            m_projActPanels.Add(panel.GetComponent<ProjectPanel>());
            m_projActPanels.Back().Initialize(proj, m_panelsParent.transform);
        }
        private void CreateActivityPanel(IProject proj) {
            var panel = Instantiate<GameObject>(m_projPanelPref, m_panelsParent.transform);
            m_projActPanels.Add(panel.GetComponent<ProjectPanel>());
            m_projActPanels.Back().Initialize(proj, m_panelsParent.transform);
        }
        #endregion
    }

}
