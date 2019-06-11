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
            // m_IFProject.OnEndEditAsObservable()
            //     .Subscribe(_ => UpdateButtonStatus())
            //     .AddTo(this);
            // m_IFActivity.OnEndEditAsObservable()
            //     .Subscribe(_ => UpdateButtonStatus())
            //     .AddTo(this);
        }
        public void OnButtonPressed() {
            if (!ProjText.IsEmpty()) {
                Register();
            }
        }
        #endregion

        #region private
        private void UpdateButtonStatus() {
            if (m_bnCreate.IsInteractable()) {
                if (!IsFilledIF()) { m_bnCreate.interactable = true; }
            }
            else {
                if (IsFilledIF()) { m_bnCreate.interactable = false; }
            }
        }
        private void Register() {
            // Project を追加
            if (ActText.IsEmpty()) {
                DB.ContentDB.Proj.AddProj(new Project(ProjText, null, ThemeColors.Default, true));
            }
            // Activity を追加
            else {
                var proj = DB.ContentDB.Proj.At(ProjText);
                DB.ContentDB.Act.AddAct(new Content(proj, ActText));
            }
        }
        /// <summary> Activityとして登録可能な情報が揃っているかどうか </summary>
        private bool IsFilledIF() {
            var proj = DB.ContentDB.Proj.At(m_IFProject.text);
            return !(proj is null)                      // Projectが有効
                && !m_IFActivity.text.IsEmpty()         // Act名が入力済
                && DB.ContentDB.Act.Sorted(proj).Any(act => act.Name == m_IFActivity.text); // 同名のActが無い
        }
        #endregion
    }

}
