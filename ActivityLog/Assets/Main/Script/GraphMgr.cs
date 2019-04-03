using UnityEngine;


namespace Main.Graph {

    public class GraphMgr : MonoBehaviour {
        private void Awake() {
            ProjectsDB.Initialize();
        }
    }

}