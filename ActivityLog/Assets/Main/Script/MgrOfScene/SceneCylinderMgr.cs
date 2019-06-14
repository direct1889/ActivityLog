using UnityEngine;
using UniRx;

namespace Main.STrack {

    /// <summary>
    /// CylinderシーンのTopManager
    /// - ActRecordの実データ系列IActSequenceMgrを持ち、Cylinder/doActListとの橋渡しを担う
    /// </summary>
    public class SceneCylinderMgr : MonoBehaviour {
        #region field
        /// <summary> アクティビティの実データ系列 </summary>
        Act.IActSequenceMgr Acts { get; } = new Dev.ActSequenceMgrBasedOnVirtualChronos();
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
            Acts.Load(2019, 6, 14);
        }
        private void Start() {
            for (int i = 0; i < Acts.Activities.Count; ++i) {
                RecCylinder.CreateBlock(Acts.Activities[i]);
            }
            du.dui.RxButtonMgr.OnClickAsObservable("DoActivity")
                .Subscribe(_ => DoActCanvas.SetActive(!DoActCanvas.activeSelf))
                .AddTo(this);
        }
        private void OnApplicationQuit() {
            Acts.Save(2019, 6, 14);
        }
        #endregion

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
