using UnityEngine;


namespace du.Audio {

    public interface ISoundAsset {
        AudioSource TempSE { get; }
    }

    public class SoundAsset : MonoBehaviour, ISoundAsset {

        [SerializeField] AudioSource m_tempSE = null;
        public AudioSource TempSE { get { return m_tempSE; } }

        private void Awake() {
            Test.LLog.Boot.Log("SoundAsset awake.");
			Mgr.Debug.SetAudioAsset(this);
        }

    }

}
