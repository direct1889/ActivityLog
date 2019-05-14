using System.Collections;
using UnityEngine;
using UGUI = UnityEngine.UI;

namespace Main.UI {

    // 使用時はCanvasのRenderModeをCameraにすること
    public class Loupe4Sprite : MonoBehaviour {

        Material m_mat = null;
        RectTransform m_recT = null;
        RectTransform m_parentRecT = null;

        void Awake() {
            m_mat = GetComponent<SpriteRenderer>().material;
            //m_mat = GetComponent<UGUI.Image>().material;
            m_recT = GetComponentInParent<RectTransform>();
            m_parentRecT = transform.parent.GetComponentInParent<RectTransform>();
        }

        void Update() {
            //var uv = m_recT.position / m_parentRecT.rect.size;
            var posOnScreen = Camera.main.WorldToScreenPoint(transform.position);
            m_mat.SetFloat("_UVx", posOnScreen.x / Screen.width);
            m_mat.SetFloat("_UVy", 1f - posOnScreen.y / Screen.height);  // UI座標はyが上から
        }
    }

}
