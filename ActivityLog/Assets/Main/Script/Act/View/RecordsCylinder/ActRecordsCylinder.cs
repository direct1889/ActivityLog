using UnityEngine;
using System.Collections.Generic;
using static du.Ex.ExList;
using System;
using UniRx;

namespace Main.Act.View {

    /// <summary> ActRecordの累積CylinderのUI詳細 </summary>
    public interface IActRecordsCylinderUI {
        Vector2 RectSize { get; }
    }

    /// <summary> ActRecordの累積Cylinder </summary>
    public interface IActRecordsCylinder {
        void RefreshSizeAll();
        void CreateBlock(IROActRecord act);
        void Clear();
    }

    /// <summary> ActRecordの累積Cylinder </summary>
    public class ActRecordsCylinder : MonoBehaviour, IActRecordsCylinderUI, IActRecordsCylinder {
        #region field
        RectTransform m_rect;
        IList<IActRecordBlock> m_blocks = new List<IActRecordBlock>();

        [SerializeField]STrack.SceneCylinderMgr m_mgr;
        [SerializeField]GameObject m_prefActBlock;
        #endregion

        #region getter
        public Vector2 RectSize => m_rect.sizeDelta;
        #endregion

        #region mono
        private void Awake() { m_rect = GetComponent<RectTransform>(); }
        private void Update() { m_blocks.Back()?.Refresh(); }
        private void OnApplicationQuit() { CDB.Save(); }
        #endregion

        #region public
        public void RefreshSizeAll() {
            for (int i = m_blocks.Count - 1; i >= 0; --i) {
                if (!m_blocks[i].Refresh()) {
                    m_blocks[i].Destroy();
                    m_blocks.RemoveAt(i);
                }
            }
        }
        public void CreateBlock(IROActRecord act) {
            IActRecordBlock block = Instantiate(m_prefActBlock, transform).GetComponent<ActRecordBlock>();
            block.Initialize(act, this);
            m_blocks.Back()?.Refresh();
            m_blocks.Add(block);
            m_blocks.Back().OnWantToChangeBeginTime
                // .Subscribe((act, newBegin) => m_mgr.ChangeBorder(act, newBegin));
                .Subscribe(tuple => {
                    m_mgr.ChangeBorder(tuple.act, tuple.minute);
                });
        }
        public void Clear() {
            foreach (var b in m_blocks) { b.Destroy(); }
            m_blocks = new List<IActRecordBlock>();
        }
        #endregion
    }

}
