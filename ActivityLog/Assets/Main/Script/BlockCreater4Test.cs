using UnityEngine;
using UnityEngine.UI;

namespace Main.Act.View {

    public class BlockCreater4Test : MonoBehaviour {

        [SerializeField] InputField m_IFProject;
        [SerializeField] InputField m_IFActivity;
        [SerializeField] InputField m_IFDuration;

        [SerializeField] ActivitiesGraph m_acts;

        MinuteOfDay m_tempTimeSign = MinuteOfDay.Begin;
        ActivitiesMgr4Test m_actMgr = new ActivitiesMgr4Test();

        public void CreateActivity() {
            if (m_IFProject.text == "") {
                CreateActivityBlockImpl("TestSample", "Test", "60");
            }
            else {
                CreateActivityBlockImpl(m_IFProject.text, m_IFActivity.text, m_IFDuration.text);
            }
        }
        private void CreateActivityBlockImpl(string proj, string actName, string duration) {
            int d = int.Parse(duration);
            Debug.LogError($"Acts[{m_actMgr.Activities.Count}]");
            m_actMgr.BeginNewAct(DB.ContentDB.Proj.At(proj), actName, m_tempTimeSign);
            Debug.LogError($"Acts[{m_actMgr.Activities.Count}]");
            Debug.LogError($"FirstAct[{m_actMgr.Activities[0].Context.EndTime}]");
            m_acts.CreateBlock(m_actMgr.Activities.Back);
            m_tempTimeSign.EnsuiteMinute += d;
            du.Test.LLog.Debug.Log($"{proj}::{actName} at {duration}");
        }

    }

}
