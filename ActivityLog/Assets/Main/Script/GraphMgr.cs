using UnityEngine;


namespace Main.Act {

    public class GraphMgr : MonoBehaviour {
        private void Start() {
            ProjectsDB.Initialize();
        }
    }

}