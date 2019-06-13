using UnityEngine;
using UGUI = UnityEngine.UI;

namespace Main.UI {

    /// <summary> 使用時はCanvasのRenderModeをOverlayにすること </summary>
    public class Loupe4UI : MonoBehaviour {
        #region field
        Material m_mat;
        RectTransform m_recT;
        RectTransform m_parentRecT;
        #endregion

        #region mono
        void Awake() {
            m_mat = GetComponent<UGUI.Image>().material;
            m_recT = GetComponentInParent<RectTransform>();
            m_parentRecT = transform.parent.GetComponentInParent<RectTransform>();
        }

        void Update() {
            var uv = m_recT.position / m_parentRecT.rect.size;
            m_mat.SetFloat("_UVx", uv.x);
            m_mat.SetFloat("_UVy", 1f - uv.y);  // UI座標はyが上から
        }
        #endregion
    }

}
