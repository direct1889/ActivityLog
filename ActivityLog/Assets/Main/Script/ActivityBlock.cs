using UnityEngine;
using UImage = UnityEngine.UI.Image;
using du.Cmp.RecT;


namespace Main.Graph {

	public interface IActivityBlock {
		void Initialize(IActivity act, IActivitiesCylinder cylinder, Transform parent);
	}

	public class ActivityBlock : MonoBehaviour, IActivityBlock {

		#region field
		UImage m_image = null;
		RecTHorStretchBottom m_recT = null;
		IActivitiesCylinder m_cylinder = null;
		#endregion

		#region property
		public IActivity Act { get; private set; }
		#endregion

		#region mono
		private void Awake() {
			gameObject.SetActive(false);
			m_image = GetComponent<UImage>();
			m_recT = new RecTHorStretchBottom(GetComponent<RectTransform>());
		}
		#endregion

		#region public
		public void Initialize(IActivity act, IActivitiesCylinder cylinder, Transform parent) {
			if (!gameObject.activeSelf) {
				Act = act;
				m_image.color = act.Project.Color;
				m_cylinder = cylinder;
				// var size = m_cylinder.RectSize;
				// size.y *= Act.Duration / (24f * 60f);
				m_recT.Initialize(parent);
				m_recT.Set(0f, 0f, Time2LocalYinCylinder(Act.BeginTime), Time2LocalYinCylinder(Act.EndTime ?? 1000f));
				// m_rect.sizeDelta = size;
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
