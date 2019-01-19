using UnityEngine;
using UImage = UnityEngine.UI.Image;


namespace Main {

	public interface IActivityBlock {

		void Initialize(IActivity act);

	}

	public class ActivityBlock : MonoBehaviour, IActivityBlock {

		//! ----- field -----

		UImage m_image = null;
		RectTransform m_rect = null;
		IActivities m_sylinder = null;


		//! ----- property -----

		public IActivity Act { get; private set; }


		//! ----- mono -----

		void Start() {
			gameObject.SetActive(false);
			m_image = GetComponent<UImage>();
			m_rect = GetComponent<RectTransform>();
		}

		void Update() { }


		//! ----- public -----

		public void Initialize(IActivity act) {
			if (gameObject.activeSelf) {
				Act = act;
				m_image.color = act.Project.Color;
				var size = m_sylinder.RectSize;
				size.y *= Act.Duration / (24f * 60f);
				m_rect.sizeDelta = size;
				gameObject.SetActive(true);
				Debug.Log("ActivityBlock initialized.");
			}
		}

	}

}
