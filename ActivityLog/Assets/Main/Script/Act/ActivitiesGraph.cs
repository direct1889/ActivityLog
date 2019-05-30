﻿using UnityEngine;

namespace Main.Act.View {

	public interface IActivitiesCylinder {
		Vector2 RectSize { get; }
	}

	public interface IActivities {
		void CreateBlock(IROActivity act);
	}

	public class Activities : MonoBehaviour, IActivitiesCylinder, IActivities {

		#region field
		RectTransform m_rect = null;
		[SerializeField]GameObject m_prefActBlock = null;
		#endregion

		#region getter
		public Vector2 RectSize { get { return m_rect.sizeDelta; } }
		#endregion

		#region mono
		void Awake() {
			m_rect = GetComponent<RectTransform>();
		}
		#endregion

		#region public
		public void CreateBlock(IROActivity act) {
			GameObject goBlock = Instantiate(m_prefActBlock);
			goBlock.transform.SetParent(transform);
			IActivityBlock block = goBlock.GetComponent<ActivityBlock>();
			block.Initialize(act, this, transform);
		}
		#endregion

	}

}
