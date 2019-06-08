using UnityEngine;
using UniRx;

namespace Main.STrack {

    public class ContentListMgr : MonoBehaviour {
        #region field
        /// <summary> アクティビティの実データ系列 </summary>
        Act.IActivitiesMgr m_acts = new Act.ActivitiesMgr4Test();

        /// <summary> アクティビティのグラフ </summary>
        [SerializeField] Act.View.ActivitiesGraph m_graph = null;
        /// <summary> アクティビティを選択するリスト </summary>
        [SerializeField] Act.View.ContentList m_list = null;
        #endregion

        #region getter
        protected Act.View.IRxContentList RxList => m_list;
        #endregion

        #region mono
        private void Awake() {
            RxList.ActivityChosen
                .Subscribe(c => Chosen(c))
                .AddTo(this);
        }
        #endregion

        #region private
        private void Chosen(Act.IROContent content) {
            gameObject.SetActive(false);
            CreateActivityBlockImpl(content);
        }
        private void CreateActivityBlockImpl(Act.IROContent content) {
            m_acts.BeginNewAct(content);
            m_graph.CreateBlock(m_acts.Activities.Back);
        }
        private void CreateActivityBlockImpl(string proj, string actName, string duration) {
            m_acts.BeginNewAct(Act.DB.ContentDB.Proj.At(proj), actName);
            m_graph.CreateBlock(m_acts.Activities.Back);
        }
        #endregion
    }

}
