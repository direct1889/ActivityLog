using UnityEngine;
using UnityEngine.UI;

namespace Main {

	public class AddBlock : MonoBehaviour {

		[SerializeField] InputField m_IFProject = null;
		[SerializeField] InputField m_IFActivity = null;
		[SerializeField] InputField m_IFDuration = null;

		[SerializeField] IActivities m_acts = null;

		public void CreateActivity() {
			Activity act = new Activity(m_IFProject.text, m_IFActivity.text);
			string log = string.Format("{0}::{1} at {2}", m_IFProject.text, m_IFActivity.text, m_IFDuration.text);
			Debug.Log(log);
		}

	}

}
