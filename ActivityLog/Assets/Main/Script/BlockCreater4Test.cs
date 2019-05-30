using UnityEngine;
using UnityEngine.UI;

namespace Main.Act.View {

	public class BlockCreater4Test : MonoBehaviour {

		[SerializeField] InputField m_IFProject = null;
		[SerializeField] InputField m_IFActivity = null;
		[SerializeField] InputField m_IFDuration = null;

		[SerializeField] Activities m_acts = null;

		MinuteOfDay m_tempTimeSign;

		private void Awake() {
			m_tempTimeSign = MinuteOfDay.Begin;
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
			IROActivity act = new Activity(
				new Content(ProjectDB.At(proj), actName),
				new Context(m_tempTimeSign)
			);
			m_acts.CreateBlock(act);
			m_tempTimeSign.EnsuiteMinute += d;
			du.Test.LLog.Debug.Log($"{proj}::{actName} at {duration}");
		}

	}

}
