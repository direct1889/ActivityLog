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
        IList<IProjActPanel> m_projActPanels = null;

        [SerializeField] GameObject m_panelsParent = null;
        [SerializeField] GameObject m_panelPref = null;
        #endregion

        #region getter
        #endregion

        #region mono
        private void Start() {
            m_projActPanels = new List<IProjActPanel>();
            Initialize();
        }
        #endregion

        #region private
        private void Initialize() {
            foreach (var proj in ProjectDB.Projects()) {
                CreatePanel(proj);
            }
        }
        private void CreatePanel(IProject proj) {
            var panel = Instantiate<GameObject>(m_panelPref);
            panel.transform.parent = m_panelsParent.transform;
            m_projActPanels.Add(panel.GetComponent<ProjActPanel>());
            m_projActPanels.Back().Initialize(proj);
        }
        #endregion
    }

}
