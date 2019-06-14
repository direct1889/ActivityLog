using UnityEngine;

namespace Main.Sys {

    public class Clock : MonoBehaviour {
        TMPro.TMP_Text m_clock;

        private void Awake() {
            m_clock = GetComponent<TMPro.TMP_Text>();
        }
        private void Update() {
            m_clock.text = Chronos.Now.ToString();
        }

    }

}
