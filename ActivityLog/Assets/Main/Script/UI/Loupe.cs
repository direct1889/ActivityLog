using System.Collections;
using UnityEngine;
using UGUI = UnityEngine.UI;

namespace Main.UI {

    public class Loupe : MonoBehaviour {

        Material m_mat = null;
        RectTransform m_recT = null;
        RectTransform m_parentRecT = null;
        float m_countX = 0;
        float m_countY = 0;

        void Awake() {
            m_mat = GetComponent<UGUI.Image>().material;
            m_recT = GetComponentInParent<RectTransform>();
            m_parentRecT = transform.parent.GetComponentInParent<RectTransform>();
        }

        void Update() {
            //Debug.Log($"UVx : {m_recT.position.x} / W : {m_parentRecT.rect.width}");
            m_countX += 0.001f;
            //m_countY += 0.03f;
            var uv = m_recT.position / m_parentRecT.rect.size;
            m_mat.SetFloat("_UVx", uv.x);
            m_mat.SetFloat("_UVy", 1f - uv.y);
            //m_mat.SetFloat("_UVx", m_countX % 1);
            //m_mat.SetFloat("_UVy", m_countY % 1);
            // m_mat.SetVector("_Pos", new Vector4(
            //     m_recT.position.x / m_parentRecT.rect.width,
            //     m_recT.position.y / m_parentRecT.rect.height,
            //     0, 0
            // ));
        }
    }

}
