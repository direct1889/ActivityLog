using System.Collections.Generic;
using UnityEngine;

namespace Main {

	public interface IActivities {

		Vector2 RectSize { get; }

	}

	public class Activities : MonoBehaviour, IActivities {


		#region field

		RectTransform m_rect = null;
		[SerializeField]GameObject m_prefActBlock = null;

		#endregion

		#region getter

		public Vector2 RectSize { get { return m_rect.sizeDelta; } }

		#endregion

		#region mono

		void Start() {
			m_rect = GetComponent<RectTransform>();
		}

		void Update() { }

		#endregion

		#region private

		private void CreateBlock(IActivity act) {
			GameObject goBlock = Instantiate(m_prefActBlock);
			goBlock.transform.SetParent(transform);
			IActivityBlock block = goBlock.GetComponent<ActivityBlock>();
			block.Initialize(act);
		}

		#endregion


	}

}
