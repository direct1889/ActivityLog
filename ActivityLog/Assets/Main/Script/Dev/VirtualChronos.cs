using UnityEngine;
using Main;

namespace Dev {

    public class VirtualChronos : MonoBehaviour {
        TMPro.TMP_Text m_clock;

        private void Awake() {
            m_clock = GetComponent<TMPro.TMP_Text>();
        }
        private void Update() {
            m_clock.text = Now.ToString();
        }

        /// <summary> 分→時 / 秒→分 </summary>
        public static MinuteOfDay Now {
            get {
                var now = System.DateTime.Now;
                return new MinuteOfDay(now.Minute % 24, now.Second);
            }
        }

    }

}
