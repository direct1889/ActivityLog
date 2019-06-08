using UnityEngine;
using System.Collections.Generic;
using static du.Ex.ExList;

namespace Main.Act.View {

    public interface IActivitiesCylinder {
        Vector2 RectSize { get; }
    }

    public interface IActivitiesGraph {
        void CreateBlock(IROActivity act);
    }

    public class ActivitiesGraph : MonoBehaviour, IActivitiesCylinder, IActivitiesGraph {
        #region field
        RectTransform m_rect;
        IList<IActivityBlock> m_blocks = new List<IActivityBlock>();

        [SerializeField]GameObject m_prefActBlock;
        #endregion

        #region getter
        public Vector2 RectSize => m_rect.sizeDelta;
        #endregion

        #region mono
        void Awake() { m_rect = GetComponent<RectTransform>(); }
        #endregion

        #region public
        public void CreateBlock(IROActivity act) {
            IActivityBlock block = Instantiate(m_prefActBlock, transform).GetComponent<ActivityBlock>();
            block.Initialize(act, this, transform);
            if (m_blocks.Count > 0) { m_blocks.Back()?.RefreshSize(); }
            m_blocks.Add(block);
        }
        #endregion
    }

}
