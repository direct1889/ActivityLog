using UnityEngine;
using UnityEngine.UI;

namespace Main.Act {

	public class AddBlock : MonoBehaviour {

		[SerializeField] InputField m_IFProject = null;
		[SerializeField] InputField m_IFActivity = null;
		[SerializeField] InputField m_IFDuration = null;

		[SerializeField] Activities m_acts = null;

		float m_tempTimeSign = 0f;


		public void CreateActivity() {
			if (m_IFProject.text == "") {
				CreateActivityBlockImpl("TestSample", "Test", "60");
			}
			else {
				CreateActivityBlockImpl(m_IFProject.text, m_IFActivity.text, m_IFDuration.text);
			}
		}
		private void CreateActivityBlockImpl(string proj, string actName, string duration) {
			float d = float.Parse(duration);
			IROActivity act = new Activity(proj, actName, m_tempTimeSign, d);
			m_acts.CreateBlock(act);
			m_tempTimeSign += d;
			du.Test.LLog.Debug.Log($"{proj}::{actName} at {duration}");
		}

	}

}
