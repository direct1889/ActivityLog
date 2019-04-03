using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Linq;
using UniRx;


namespace du.App {

    public enum MouseCursorMode {
        Invisible, Visible, Detail
    }

    public class AppManager : MonoBehaviour {

        #region singleton
        public static AppManager Instance { get; private set; } = null;
        #endregion

        #region field
        // [SerializeField] List<Initializable> m_initializables = null;

        [SerializeField] string m_pilotScene = null;
        [SerializeField] bool m_isMute = true;
        [SerializeField] float m_masterVolume = 0.01f;
        [SerializeField] bool m_isDebugMode = false;
        [SerializeField] MouseCursorMode m_mcmode = MouseCursorMode.Visible;

        IList<Action> m_fixedUpdateActs = null;
        // ISubject<MouseCursorMode> m_rxMCMode = new Subject<MouseCursorMode>();
        #endregion

        #region property
        public MouseCursorMode MouseCursorMode { get { return m_mcmode; } }
        // public IReadOnlyReactiveProperty<MouseCursorMode> RxMCMode {  }
        #endregion

        #region mono
        private void Awake() {
            if (Instance != null) { return; }

            Instance = this;
            Mgr.RegisterMgr(Instance);
            DontDestroyOnLoad(gameObject);

            Boot();
        }
        private void FixedUpdate() {
            if (m_fixedUpdateActs != null) {
                for (int i = 0; i < m_fixedUpdateActs.Count; ++i) {
                    m_fixedUpdateActs[i]();
                }
            }
        }
        #endregion

        #region public
        public void RegisterFixedUpdatedAction(Action act) {
            m_fixedUpdateActs.Add(act);
        }
        #endregion

        #region private
        private void Boot() {
            Debug.Log("Boot Apprication");

            m_fixedUpdateActs = new List<Action>();
            GetComponent<Test.LayerdLogMgr>().InitializeLLog();
            Test.DebugAssistant.Instance.gameObject.SetActive(m_isDebugMode);
            di.RxTouchInput.Initialize();

            Cursor.visible = m_mcmode == MouseCursorMode.Visible;
            // OSUI.Instance.SetEnable(m_mcmode == MouseCursorMode.Detail);

            // DG.Tweening.DOTween.Init();

            // for (int i = 0; i < m_initializables.Count; ++i) {
            // 	m_initializables[i].Initialize();
            // }
            /*
			du.Input.InputManager.Initialize();
			du.Input.Id.IdConverter.SetPlayer2GamePad(
				dutil.Input.Id.GamePad._1P,
				dutil.Input.Id.GamePad._2P,
				dutil.Input.Id.GamePad._3P,
				dutil.Input.Id.GamePad._4P
				);
			*/

            // UI.UIAsset.Initialize();

            // utility.sound.SoundManager.Init();
            // utility.sound.SoundManager.BGM
            // .MasterVolumeSet(
            float volume = m_isMute ? 0f : m_masterVolume;
            // );

            // GlobalStore.IsMute = m_isMute;

            if (Enumerable.Range(0, SceneManager.sceneCount)
                .Select(SceneManager.GetSceneAt)
                .All(scn => { return scn.name != m_pilotScene; }))
            {
                SceneManager.LoadSceneAsync(m_pilotScene, LoadSceneMode.Additive);
            }
        }

        #endregion

    }

}
