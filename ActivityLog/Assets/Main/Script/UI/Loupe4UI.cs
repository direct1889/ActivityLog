using System.Collections;
using UnityEngine;
using UGUI = UnityEngine.UI;

namespace Main.UI {

    // 使用時はCanvasのRenderModeをOverlayにすること
    public class Loupe4UI : MonoBehaviour {

        Material m_mat = null;
        RectTransform m_recT = null;
        RectTransform m_parentRecT = null;

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
    }

}
