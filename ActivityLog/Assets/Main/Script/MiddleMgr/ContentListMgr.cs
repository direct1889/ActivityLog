using UnityEngine;
using UniRx;

namespace Main.STrack {

    public class ContentListMgr : MonoBehaviour {
        #region field
        Act.ActivitiesMgr4Test m_actMgr = null;
        [SerializeField] Act.View.ActivitiesGraph m_acts = null;
        MinuteOfDay m_tempTimeSign;

        [SerializeField] Act.View.ContentList m_list = null;
        #endregion

        #region getter
        protected Act.View.IRxContentList RxList => m_list;
        #endregion

        #region mono
        private void Awake() {
            RxList.CreatedPanel
                .Subscribe(panel => panel.Pressed.Subscribe(c => Chosen(c)).AddTo(this))
                .AddTo(this);
        }
        #endregion

        #region private
        private void Chosen(Act.IROContent content) {
            gameObject.SetActive(false);
        }
        // private void CreateActivityBlockImpl(Act.IROContent act, string duration) {
            // int d = int.Parse(duration);
            // m_actMgr.BeginNewAct(Act.DB.ContentDB.Proj.At(proj), actName, m_tempTimeSign);
            // m_acts.CreateBlock(m_actMgr.Activities.Back);
            // m_tempTimeSign.EnsuiteMinute += d;
        // }
        // private void sCreateActivityBlockImpl(string proj, string actName, string duration) {
            // int d = int.Parse(duration);
            // m_actMgr.BeginNewAct(Act.DB.ContentDB.Proj.At(proj), actName, m_tempTimeSign);
            // m_acts.CreateBlock(m_actMgr.Activities.Back);
            // m_tempTimeSign.EnsuiteMinute += d;
        // }
        #endregion
    }

}
