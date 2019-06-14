using UnityEngine;
using System.Collections.Generic;
using static du.Ex.ExList;

namespace Main.Act.View {

    /// <summary> ActRecordの累積CylinderのUI詳細 </summary>
    public interface IActRecordsCylinderUI {
        Vector2 RectSize { get; }
    }

    /// <summary> ActRecordの累積Cylinder </summary>
    public interface IActRecordsCylinder {
        void CreateBlock(IROActRecord act);
    }

    /// <summary> ActRecordの累積Cylinder </summary>
    public class ActRecordsCylinder : MonoBehaviour, IActRecordsCylinderUI, IActRecordsCylinder {
        #region field
        RectTransform m_rect;
        IList<IActRecordBlock> m_blocks = new List<IActRecordBlock>();

        [SerializeField]GameObject m_prefActBlock;
        #endregion

        #region getter
        public Vector2 RectSize => m_rect.sizeDelta;
        #endregion

        #region mono
        private void Awake() { m_rect = GetComponent<RectTransform>(); }
        private void Update() { m_blocks.Back()?.RefreshSize(); }
        private void OnApplicationQuit() { CDB.Save(); }
        #endregion

        #region public
        public void CreateBlock(IROActRecord act) {
            IActRecordBlock block = Instantiate(m_prefActBlock, transform).GetComponent<ActRecordBlock>();
            block.Initialize(act, this);
            m_blocks.Back()?.RefreshSize();
            m_blocks.Add(block);
        }
        #endregion
    }

}
