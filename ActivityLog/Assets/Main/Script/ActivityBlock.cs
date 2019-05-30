using UnityEngine;
using UGUI = UnityEngine.UI;
using du.Cmp.RecT;
using UTMP = TMPro.TextMeshProUGUI;
using IActivity = Main.Act.IActivity;
// using IActivities = Main.Graph.Act.IActivities;

namespace Main.Act {

	public interface IActivityBlock {
		void Initialize(IROActivity act, IActivitiesCylinder cylinder, Transform parent);
	}

	public class ActivityBlock : MonoBehaviour, IActivityBlock {

		#region field
		UGUI.Image m_image = null;
		RecTHorStretchBottom m_recT = null;
		IActivitiesCylinder m_cylinder = null;
		[SerializeField] UTMP m_text = null;
		#endregion

		#region property
		public IROActivity Act { get; private set; }
		#endregion

		#region mono
		private void Awake() {
			gameObject.SetActive(false);
			m_image = GetComponent<UGUI.Image>();
			m_recT = new RecTHorStretchBottom(GetComponent<RectTransform>());
		}
		#endregion

		#region public
		public void Initialize(IROActivity act, IActivitiesCylinder cylinder, Transform parent) {
			if (!gameObject.activeSelf) {
				Act = act;
				m_image.color = act.Project.Color;
				m_cylinder = cylinder;
				m_recT.Initialize(parent);
				m_recT.Set(0f, 0f, Time2LocalYinCylinder(Act.BeginTime), Time2LocalYinCylinder(Act.EndTime ?? 1000f));
				m_text.text = Act.Name;
				gameObject.SetActive(true);
				Debug.Log("ActivityBlock initialized.");
			}
		}
		#endregion

		#region static
		private float Time2LocalYinCylinder(float time) {
			return time * m_cylinder.RectSize.y / 1440f; // 1440 = 24h * 60m
		}
		#endregion

	}

}
