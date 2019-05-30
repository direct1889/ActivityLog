using UnityEngine;
using UnityEngine.UI;

namespace Main.Act.View {

	public class BlockCreater4Test : MonoBehaviour {

		[SerializeField] InputField m_IFProject = null;
		[SerializeField] InputField m_IFActivity = null;
		[SerializeField] InputField m_IFDuration = null;

		[SerializeField] ActivitiesGraph m_acts = null;

		MinuteOfDay m_tempTimeSign;
		ActivitiesMgr4Test m_actMgr = null;

		private void Awake() {
			m_tempTimeSign = MinuteOfDay.Begin;
			m_actMgr = new ActivitiesMgr4Test();
		}

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
			m_actMgr.BeginNewAct(ProjectDB.At(proj), actName, m_tempTimeSign);
			Debug.LogError($"Acts[{m_actMgr.Activities.Count}]");
			Debug.LogError($"FirstAct[{m_actMgr.Activities[0].Context.EndTime}]");
			m_acts.CreateBlock(m_actMgr.Activities.Back);
			m_tempTimeSign.EnsuiteMinute += d;
			du.Test.LLog.Debug.Log($"{proj}::{actName} at {duration}");
		}

	}

}
