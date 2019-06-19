using UnityEngine;
using UniRx;
using System.Linq;

namespace Main.STrack {

    public interface ISceneCylinderMgrAsParent {
        void ChangeBorder(Act.IROActRecord act, MinuteOfDay newMinute);
    }

    /// <summary>
    /// CylinderシーンのTopManager
    /// - ActRecordの実データ系列IActSequenceMgrを持ち、Cylinder/doActListとの橋渡しを担う
    /// </summary>
    public class SceneCylinderMgr : MonoBehaviour, ISceneCylinderMgrAsParent {
        #region field
        /// <summary> アクティビティの実データ系列 </summary>
        Act.IActSequenceMgr Acts { get; } = new Act.ActSequenceMgr();
        // Act.IActSequenceMgr Acts { get; } = new Act.ActSequenceMgr4Test();

        /// <summary> アクティビティのグラフ </summary>
        [SerializeField] Act.View.ActRecordsCylinder m_recCylinder = null;
        /// <summary> アクティビティを選択するリスト </summary>
        [SerializeField] Act.View.DoActList m_doActList = null;

        /// <summary> アクティビティのグラフ </summary>
        Act.View.IActRecordsCylinder RecCylinder => m_recCylinder;
        /// <summary> アクティビティを選択するリスト </summary>
        Act.View.IRxDoActList DoActList => m_doActList;

        GameObject DoActCanvas => m_doActList.gameObject;
        #endregion

        #region mono
        private void Awake() {
            du.Test.LLog.MBoot.Log("ActRecordCylinderMgr awoke");
            Act.CDB.Initialize();
            // DoActListから入力があったら
            DoActList.ActivityChosen
                .Subscribe(act => BeginNewAct(act))
                .AddTo(this);

            DoActCanvas.SetActive(false);
            Acts.Load(YMD.Today);
            string s = "// Borders";
            for (int i = 0; i < Acts.Activities.Count; ++i) {
                s += "\n" + Acts.Activities[i].Context.BeginTime;
            }
            Debug.LogAssertion(s);
        }
        private void Start() {
            // 日付が変わったら
            Sys.Chronos.Instance.OnDateHasChanged
                .Subscribe(_ => {
                    // 前日の分をパッケージ
                    Acts.Package(YMD.Yesterday);
                    // Cylinderを一旦空っぽに
                    RecCylinder.Clear();
                    // 日付変更時に行っていたActivityを改めて00:00から開始
                    RecCylinder.CreateBlock(Acts.Activities.Back);
                })
                .AddTo(this);

            for (int i = 0; i < Acts.Activities.Count; ++i) {
                RecCylinder.CreateBlock(Acts.Activities[i]);
            }
            du.dui.RxButtonMgr.OnClickAsObservable("DoActivity")
                .Subscribe(_ => DoActCanvas.SetActive(!DoActCanvas.activeSelf))
                .AddTo(this);
        }
        private void OnApplicationQuit() {
            // Acts.Save(YMD.Today);
        }
        private void Update () {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Return)) {
                Debug.LogError("RefreshAllBlocks");
                m_recCylinder.RefreshSizeAll();
                Debug.LogError(Acts.Activities.Dump());
            }
        }
        #endregion

        public void ChangeBorder(Act.IROActRecord act, MinuteOfDay newMinute) {
            Acts.ChangeBorder(
                du.Test.Log.TL(Acts.Activities.IndexOf(act.Context.BeginTime), "index is "),
                newMinute);
            m_recCylinder.RefreshSizeAll();
            string s = "// Borders";
            for (int i = 0; i < Acts.Activities.Count; ++i) {
                s += "\n" + Acts.Activities[i].Context.BeginTime;
            }
            Debug.LogAssertion(s);
        }

        #region private
        /// <summary>
        /// DoActCanvasでActが選択されたときの処理
        /// - DoActCanvasを非表示に
        /// - 選択されたActに基づくBlockを生成
        /// </summary>
        private void BeginNewAct(Act.IActivity act) {
            DoActCanvas.SetActive(false);
            Acts.BeginNewAct(act);
            RecCylinder.CreateBlock(Acts.Activities.Back);
        }
        #endregion
    }

}
