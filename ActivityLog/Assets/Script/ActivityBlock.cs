using UnityEngine;
using UImage = UnityEngine.UI.Image;


namespace Main.Graph {

	public interface IActivityBlock {
		void Initialize(IActivity act, IActivitiesCylinder cylinder);
	}

	public class ActivityBlock : MonoBehaviour, IActivityBlock {

		#region field
		UImage m_image = null;
		RectTransform m_rect = null;
		IActivitiesCylinder m_cylinder = null;
		#endregion

		#region property
		public IActivity Act { get; private set; }
		#endregion

		#region mono
		private void Awake() {
			gameObject.SetActive(false);
			m_image = GetComponent<UImage>();
			m_rect = GetComponent<RectTransform>();
		}
		#endregion

		#region public
		public void Initialize(IActivity act, IActivitiesCylinder cylinder) {
			if (!gameObject.activeSelf) {
				Act = act;
				m_image.color = act.Project.Color;
				m_cylinder = cylinder;
				var size = m_cylinder.RectSize;
				size.y *= Act.Duration / (24f * 60f);
				m_rect.sizeDelta = size;
				gameObject.SetActive(true);
				Debug.Log("ActivityBlock initialized.");
			}
		}
		#endregion

	}

}
