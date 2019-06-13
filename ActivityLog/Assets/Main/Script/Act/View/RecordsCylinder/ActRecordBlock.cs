using UnityEngine;
using UGUI = UnityEngine.UI;
using du.Cmp.RecT;
using UTMP = TMPro.TextMeshProUGUI;

namespace Main.Act.View {

    /// <summary> ActRecordを表す Block UI </summary>
    public interface IActRecordBlock {
        /// <summary> 初期化 </summary>
        void Initialize(IROActRecord act, IActRecordsCylinderUI cylinder);
        /// <summary>
        /// UIサイズを更新
        /// - 毎フレーム呼ぶと無駄なため、
        /// </summary>
        void RefreshSize();
    }

    public class ActRecordBlock : MonoBehaviour, IActRecordBlock {

        #region field
        RecTHorStretchBottom m_recT;
        IActRecordsCylinderUI m_cylinder;

        [SerializeField] UTMP m_text;
        #endregion

        #region property
        public IROActRecord Act { get; private set; }
        #endregion

        #region mono
        private void Awake() {
            gameObject.SetActive(false);
            m_recT = new RecTHorStretchBottom(GetComponent<RectTransform>());
        }
        #endregion

        #region public
        public void Initialize(IROActRecord act, IActRecordsCylinderUI cylinder) {
            if (!gameObject.activeSelf) {
                Act = act;
                m_cylinder = cylinder;

                GetComponent<UGUI.Image>().color = Act.Activity.Parent.Color;
                m_text.text = Act.Activity.Name;
                RefreshSize();

                gameObject.SetActive(true);
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
