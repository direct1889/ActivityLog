﻿using UnityEngine;


namespace Main.Act {

    public interface IGraphMgr {
    }

    public class GraphMgr : MonoBehaviour, IGraphMgr {
        private void Start() {
            DB.ContentDB.Initialize();
        }
    }

}