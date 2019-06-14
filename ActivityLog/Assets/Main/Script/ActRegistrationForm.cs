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
                .Subscribe(_ => Register())
                .AddTo(this);
            m_IFProject.OnValueChangedAsObservable()
                .Subscribe(_ => UpdateButtonStatus())
                .AddTo(this);
            m_IFActivity.OnValueChangedAsObservable()
                .Subscribe(_ => UpdateButtonStatus())
                .AddTo(this);
        }
        #endregion

        #region private
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
                CDB.Proj.Add(new Project(ProjText, null, ThemeColors.Default, true));
            }
            // Activity を追加
            else {
                var proj = CDB.Proj.AtProjByGenealogy(ProjText);
                CDB.Act.Add(new Activity(proj, ActText));
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
                    return !CDB.Proj.ProjHasExist(ProjText, null);
                }
                // Activityを追加
                else {
                    var proj = CDB.Proj.AtProjByGenealogy(ProjText);
                    return !(proj is null) &&
                        !CDB.Act.ActHasExist(ActText, proj);
                }
            }
        }
        #endregion
    }

}
