using UnityEngine;
using UGUI = UnityEngine.UI;

namespace ShaderAttachment {

    public class Outline : MonoBehaviour {
        void Awake() {
            var mat = GetComponent<UGUI.Image>().material;
            var recT = GetComponent<RectTransform>();
            mat.SetFloat("_ObjSizeX", recT.rect.width);
            mat.SetFloat("_ObjSizeY", recT.rect.height);
        }
    }

}
