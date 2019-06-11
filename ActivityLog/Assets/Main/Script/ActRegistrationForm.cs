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

        // [SerializeField] ActivitiesGraph m_acts;
        #endregion

        #region mono
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
            Debug.LogError("OnButtonPressed()");
            if (!m_IFProject.text.IsEmpty() && !m_IFActivity.text.IsEmpty()) {
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
            var proj = DB.ContentDB.Proj.At(m_IFProject.text);
            var act = m_IFActivity.text;
            DB.ContentDB.Act.AddAct(new Content(proj, act));
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
