using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace du.Test {

    public class LayerdLogMgr : MonoBehaviour {
		[SerializeField] bool m_isLogEnable = true;

        [SerializeField] bool m_boot = true;
        [SerializeField] bool m_debug = true;
        [SerializeField] bool m_misc = true;

        [SerializeField] bool m_iaBoot = true;
        [SerializeField] bool m_iAct = true;
        [SerializeField] bool m_expLog = true;
        [SerializeField] bool m_mainBoot = true;

        public void InitializeLLog() {
			LayeredLog.Initialize(m_isLogEnable);
            LayeredLog.SetActive("BOOT", m_boot);
            LayeredLog.SetActive("DEBUG", m_debug);
            LayeredLog.SetActive("MISC", m_misc);
            LayeredLog.SetActive("IA:BOOT", m_iaBoot);
            LayeredLog.SetActive("IA:IACT", m_iAct);
            LayeredLog.SetActive("EXPLOG", m_expLog);
            LayeredLog.SetActive("MAIN:BOOT", m_mainBoot);
        }
    }

}
