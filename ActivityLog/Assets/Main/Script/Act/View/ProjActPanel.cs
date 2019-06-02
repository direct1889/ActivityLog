using UnityEngine;
using UImage = UnityEngine.UI.Image;
using System.Collections.Generic;

namespace Main.Act.View {

    public interface IROProjActPanel {
    }

    public interface IProjActPanel : IROProjActPanel {
    }

    public class ProjActPanel : MonoBehaviour, IProjActPanel {
        #region field
        IProject m_proj = null;

        UImage m_frame = null;
        [SerializeField] TMPro.TMP_Text m_name = null;
        [SerializeField] GameObject m_effectiveCert = null;
        #endregion

        #region getter
        #endregion

        #region mono
        private void Awake() {
            m_frame = GetComponent<UImage>();
        }
        #endregion

        #region public
        private void Initialize(IProject proj) {
            if (m_proj == null) {
                m_proj = proj;
                m_name.text = proj.Name;
                m_frame.material.color = m_proj.Color;
                m_effectiveCert.SetActive(m_proj.IsEffectiveDefault);
            }
        }
        #endregion
    }

}
