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
                .Subscribe(act => Chosen(act))
                .AddTo(this);
        }
        #endregion

        #region private
        private void Chosen(Act.IActivity act) {
            gameObject.SetActive(false);
            CreateActivityBlockImpl(act);
        }
        private void CreateActivityBlockImpl(Act.IActivity act) {
            m_acts.BeginNewAct(act);
            m_graph.CreateBlock(m_acts.Activities.Back);
        }
        private void CreateActivityBlockImpl(Act.IProject proj, string actName, string duration) {
            m_acts.BeginNewAct(Act.DB.ContentDB.Tree.AtProj(proj.Key, proj.Parent), actName);
            m_graph.CreateBlock(m_acts.Activities.Back);
        }
        #endregion
    }

}
