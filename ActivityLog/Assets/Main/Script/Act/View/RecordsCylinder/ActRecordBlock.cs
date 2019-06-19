using UnityEngine;
using UGUI = UnityEngine.UI;
using du.Cmp.RecT;
using UTMP = TMPro.TextMeshProUGUI;
using System;
using UniRx;

namespace Dev {
    public class Tuple_RecAndMOD {
        public Main.Act.IROActRecord act;
        public Main.MinuteOfDay minute;
        public Tuple_RecAndMOD(Main.Act.IROActRecord act, Main.MinuteOfDay minute) {
            this.act = act;
            this.minute = minute;
        }
    }
}

namespace Main.Act.View {

    /// <summary> ActRecordを表す Block UI </summary>
    public interface IActRecordBlock {
        /// <summary> 開始時刻変更申請 </summary>
        // IObservable<float> OnWantToChangeBeginTime { get; }
        IObservable<Dev.Tuple_RecAndMOD> OnWantToChangeBeginTime { get; }
        // IObservable<(Main.Act.IROActRecord, Main.MinuteOfDay)> OnWantToChangeBeginTime { get; }
        /// <summary> 初期化 </summary>
        void Initialize(IROActRecord act, IActRecordsCylinderUI cylinder);
        /// <summary>
        /// UIサイズを更新
        /// - 毎フレーム呼ぶと無駄なため、
        /// </summary>
        /// <returns> 参照先のActRecordが無効な場合false </returns>
        bool Refresh();
        /// <summary> オブジェクト削除 </summary>
        void Destroy();
    }

    public class ActRecordBlock : MonoBehaviour, IActRecordBlock {

        #region field
        RecTHorStretchBottom m_recT;
        IActRecordsCylinderUI m_cylinder;
        IActBlockBeginMarker m_beginMarker;

        [SerializeField] UTMP m_text;
        #endregion

        #region property
        public IROActRecord Act { get; private set; }
        // public IObservable<float> OnWantToChangeBeginTime {
        public IObservable<Dev.Tuple_RecAndMOD> OnWantToChangeBeginTime {
        // public IObservable<(Main.Act.IROActRecord, Main.MinuteOfDay)> OnWantToChangeBeginTime {
            get {
                return m_beginMarker.OnTransformComplete
                    .Select(localY => new Dev.Tuple_RecAndMOD(Act, LocalYinCylinder2Time(m_recT.PosY + localY)));
                    // .Select(_ => new Dev.Tuple_RecAndMOD(Act, LocalYinCylinder2Time(m_recT.PosY)));
            }
        }
        #endregion

        #region mono
        private void Awake() {
            gameObject.SetActive(false);
            m_recT = new RecTHorStretchBottom(GetComponent<RectTransform>());
            m_beginMarker = GetComponentInChildren<ActBlockBeginMarker>();
        }
        #endregion

        #region public
        public void Initialize(IROActRecord act, IActRecordsCylinderUI cylinder) {
            if (!gameObject.activeSelf) {
                Act = act;
                m_cylinder = cylinder;

                GetComponent<UGUI.Image>().color = Act.Activity.Parent.Color;
                m_text.text = Act.Activity.Name;
                Refresh();

                gameObject.SetActive(true);
            }
        }
        public bool Refresh() {
            if (Act.IsInvalid) { return false; }
            else {
                m_recT.Set(0f, 0f,
                    Time2LocalYinCylinder(Act.Context.BeginTime),
                    Time2LocalYinCylinder(Act.Context.EndTime ?? Sys.Chronos.Now) - Time2LocalYinCylinder(Act.Context.BeginTime)
                    );
                // Debug.LogError($"Refresh by {Act}({LocalYinCylinder2Time(m_recT.PosY)}, {LocalYinCylinder2Time(m_recT.PosY + m_recT.Height)})");
                m_beginMarker.ResetPos();
                return true;
            }
        }
        public void Destroy() => Destroy(gameObject);
        #endregion

        #region private
        /// <summary> 時刻をCylinder上でのy座標値に変換 </summary>
        private float Time2LocalYinCylinder(MinuteOfDay time) {
            return m_cylinder.RectSize.y * time.EnsuiteMinute / MinuteOfDay.End.EnsuiteMinute;
        }
        /// <summary> Cylinder上でのy座標値を時刻に変換 </summary>
        private MinuteOfDay LocalYinCylinder2Time(float y) {
            return new MinuteOfDay((int)(MinuteOfDay.End.EnsuiteMinute * y / m_cylinder.RectSize.y));
        }
        #endregion

    }

}
