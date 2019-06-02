using UnityEngine;


namespace Main.Act {

    public interface IGraphMgr {
    }

    public class GraphMgr : MonoBehaviour, IGraphMgr {
        private void Start() {
            ProjectDB.Initialize();
        }
    }

}