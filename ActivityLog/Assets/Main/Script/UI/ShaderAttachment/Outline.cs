using System.Collections.Generic;
using UnityEngine;
using UGUI = UnityEngine.UI;

namespace ShaderAttachment {

    public class Outline : MonoBehaviour {
        Material m_mat = null;
        RectTransform m_recT = null;
        void Awake() {
            m_mat = GetComponent<UGUI.Image>().material;
            m_recT = GetComponent<RectTransform>();
            m_mat.SetFloat("_ObjSizeX", m_recT.rect.width);
            m_mat.SetFloat("_ObjSizeY", m_recT.rect.height);
        }
    }

}
