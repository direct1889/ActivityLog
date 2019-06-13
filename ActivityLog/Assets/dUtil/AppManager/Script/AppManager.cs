using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Linq;
using UniRx;


namespace du.App {

    public enum MouseCursorMode {
        Invisible, Visible, Detail
    }

    [Serializable]
    public class AudioDesc {
        public bool isMute = true;
        public float masterVolume = 0.01f;
    }
    [Serializable]
    public class ResolutionDesc {
        public int width;
        public int height;
        public bool isFullscreen = false;
        public int preferredRefreshRate = 60;

        public void SetResolution() {
            Screen.SetResolution(width, height, isFullscreen, preferredRefreshRate);
        }
    }

    public interface IAppManager {
        MouseCursorMode MouseCursorMode { get; }
    }

    public class AppManager : SingletonMonoBehaviour<AppManager>, IAppManager {
        #region field
        [SerializeField] string m_pilotScene;
        [SerializeField] AudioDesc m_audioDesc;
        [SerializeField] ResolutionDesc m_resolutionDesc;
        [SerializeField] bool m_isDebugMode = false;
        [SerializeField] MouseCursorMode m_mcmode = MouseCursorMode.Visible;
        #endregion

        #region getter property
        public MouseCursorMode MouseCursorMode => m_mcmode;
        #endregion

        #region mono
        private void Awake() {
            DontDestroyOnLoad(gameObject);
            Boot();
        }
        private void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                Application.Quit();
            }
        }
        #endregion

        #region private
        private void Boot() {
            Debug.Log("Boot Apprication");
            m_resolutionDesc.SetResolution();
            InitializeDebugger();
            // di.RxTouchInput.Initialize();
            InitializeCursor();
            // DG.Tweening.DOTween.Init();
            // Initialize_di();
            InitializeAudio();
            InitializeScene();
        }
        private void InitializeDebugger() {
            GetComponent<Test.LayerdLogMgr>().InitializeLLog();
            Test.DebugAssistant.Instance.gameObject.SetActive(m_isDebugMode);
        }
        private void InitializeCursor() {
            // Cursor.visible = m_mcmode == MouseCursorMode.Visible;
            Cursor.visible = m_mcmode != MouseCursorMode.Invisible;
            // OSUI.Instance.SetEnable(m_mcmode == MouseCursorMode.Detail);
        }
        private void Initialize_di() {
            //  du.Input.InputManager.Initialize();
            //  du.Input.Id.IdConverter.SetPlayer2GamePad(
                //  dutil.Input.Id.GamePad._1P,
                //  dutil.Input.Id.GamePad._2P,
                //  dutil.Input.Id.GamePad._3P,
                //  dutil.Input.Id.GamePad._4P
                //  );
        }
        private void InitializeAudio() {
            // utility.sound.SoundManager.Init();
            // utility.sound.SoundManager.BGM
            // .MasterVolumeSet(
            float volume = m_audioDesc.isMute ? 0f : m_audioDesc.masterVolume;
            // );
        }
        private void InitializeScene() {
            if (Enumerable.Range(0, SceneManager.sceneCount)
                .Select(SceneManager.GetSceneAt)
                .All(scn => { return scn.name != m_pilotScene; }))
            {
                SceneManager.LoadSceneAsync(m_pilotScene, LoadSceneMode.Additive);
            }
        }
        #endregion

        #region static
        /// <summary> データファイルはここに置く </summary>
        /// <value> "Assets/MyData/" </value>
        public static string DataPath => Application.dataPath + "/MyData/";
        #endregion

    }

}
