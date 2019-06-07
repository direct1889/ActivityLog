using UnityEngine;
using System.Collections.Generic;

namespace Main.Act.View {

    public interface IActivitiesCylinder {
        Vector2 RectSize { get; }
    }

    public interface IActivitiesGraph {
        void CreateBlock(IROActivity act);
    }

    public class ActivitiesGraph : MonoBehaviour, IActivitiesCylinder, IActivitiesGraph {
        #region field
        RectTransform m_rect = null;
        IList<IActivityBlock> m_blocks = null;
        [SerializeField]GameObject m_prefActBlock = null;
        #endregion

        #region getter
        public Vector2 RectSize => m_rect.sizeDelta;
        #endregion

        #region mono
        void Awake() {
            m_rect = GetComponent<RectTransform>();
            m_blocks = new List<IActivityBlock>();
        }
        #endregion

        #region public
        public void CreateBlock(IROActivity act) {
            GameObject goBlock = Instantiate(m_prefActBlock);
            goBlock.transform.SetParent(transform);
            IActivityBlock block = goBlock.GetComponent<ActivityBlock>();
            block.Initialize(act, this, transform);
            if (m_blocks.Count > 0) { m_blocks[m_blocks.Count - 1].RefreshSize(); }
            m_blocks.Add(block);
        }
        #endregion
    }

}
