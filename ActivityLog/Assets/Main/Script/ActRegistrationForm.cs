using UnityEngine;
using UnityEngine.UI;
using UniRx;
using static du.Ex.ExString;
using System.Linq;

namespace Main.Act.View {

    public class ActRegistrationForm : MonoBehaviour {
        #region field
        [SerializeField] InputField m_IFProject;
        [SerializeField] InputField m_IFActivity;
        [SerializeField] Button m_bnCreate;
        #endregion

        #region private getter
        private string ProjText => m_IFProject.text;
        private string ActText => m_IFActivity.text;
        #endregion

        #region mono
        private void OnEnable() {
            m_IFProject.text = "";
            m_IFActivity.text = "";
        }
        private void Start() {
            m_bnCreate
                .OnClickAsObservable()
                .Subscribe(_ => OnButtonPressed())
                .AddTo(this);
            m_IFProject.OnValueChangedAsObservable()
                .Subscribe(_ => UpdateButtonStatus())
                .AddTo(this);
            m_IFActivity.OnValueChangedAsObservable()
                .Subscribe(_ => UpdateButtonStatus())
                .AddTo(this);
        }
        public void OnButtonPressed() {
            if (!ProjText.IsEmpty()) {
                Register();
            }
        }
        #endregion

        #region private
        // TODO:
        private static IProject Convert(string projText) {
            return null;
        }

        private void UpdateButtonStatus() {
            if (m_bnCreate.IsInteractable()) {
                if (!IsReady()) { m_bnCreate.interactable = false; }
            }
            else {
                if (IsReady()) { m_bnCreate.interactable = true; }
            }
        }
        private void Register() {
            // Project を追加
            if (ActText.IsEmpty()) {
                DB.ContentDB.Tree.AddProj(new Project(ProjText, null, ThemeColors.Default, true));
            }
            // Activity を追加
            else {
                var proj = DB.ContentDB.Tree.AtByKey(ProjText);
                DB.ContentDB.Tree.AddAct(new Activity(proj, ActText));
            }
        }
        /// <summary>
        /// 入力内容が登録可能か
        /// (名前の重複がない / 親Projectが存在する)
        /// </summary>
        private bool IsReady() {
            if (ProjText.IsEmpty()) { return false; }
            else {
                // Projectを追加
                if (ActText.IsEmpty()) {
                    return !DB.ContentDB.Tree.ProjHasExist(ProjText, null);
                }
                // Activityを追加
                else {
                    var proj = DB.ContentDB.Tree.AtByKey(ProjText);
                    return !(proj is null) &&
                        !DB.ContentDB.Tree.ActHasExist(ActText, proj);
                }
            }
        }
        #endregion
    }

}
