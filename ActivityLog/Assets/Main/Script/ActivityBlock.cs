using UnityEngine;
using UGUI = UnityEngine.UI;
using du.Cmp.RecT;
using UTMP = TMPro.TextMeshProUGUI;

namespace Main.Act.View {

    /// <summary> アクティビティを表す Block UI </summary>
    public interface IActivityBlock {
        void Initialize(IROActivity act, IActivitiesCylinder cylinder, Transform parent);
        void RefreshSize();
    }

    public class ActivityBlock : MonoBehaviour, IActivityBlock {

        #region field
        UGUI.Image m_image;
        RecTHorStretchBottom m_recT;
        IActivitiesCylinder m_cylinder;

        [SerializeField] UTMP m_text;
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
                m_image.color = act.Content.Parent.Color;
                m_cylinder = cylinder;
                m_recT.Initialize(parent);
                RefreshSize();
                m_text.text = Act.Content.Name;
                gameObject.SetActive(true);
                Debug.Log("ActivityBlock initialized.");
            }
        }
        public void RefreshSize() {
            m_recT.Set(0f, 0f,
                Time2LocalYinCylinder(Act.Context.BeginTime),
                Time2LocalYinCylinder(Act.Context.EndTime ?? MinuteOfDay.Now) - Time2LocalYinCylinder(Act.Context.BeginTime)
                );
        }
        #endregion

        #region private
        /// <summary> 時刻をCylinder上でのy座標値に変換 </summary>
        private float Time2LocalYinCylinder(MinuteOfDay time) {
            return m_cylinder.RectSize.y * time.EnsuiteMinute / MinuteOfDay.End.EnsuiteMinute;
        }
        #endregion

    }

}
