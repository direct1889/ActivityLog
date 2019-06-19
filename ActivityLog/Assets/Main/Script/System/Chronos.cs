using UnityEngine;
using System;
using UniRx;

namespace Main.Sys {

    public interface IChronos {
        IObservable<YMD> OnDateHasChanged { get; }
    }

    public interface IChronometer {
        MinuteOfDay Now { get; }
    }

    public class Chronos : MonoBehaviour, IChronos {

        #region field
        Subject<YMD> m_dateHasChangedStream = new Subject<YMD>();
        YMD m_todayCache;
        #endregion

        #region getter
        public IObservable<YMD> OnDateHasChanged => m_dateHasChangedStream;
        #endregion

        #region mono
        private void Awake() {
            Instance = this;
            m_todayCache = YMD.Today;
        }
        private void Update() {
            if (m_todayCache != YMD.Today) {
                du.Test.LLog.Misc.Log($"Date has changed from {m_todayCache} to {YMD.Today}.");
                m_todayCache = YMD.Today;
                m_dateHasChangedStream.OnNext(m_todayCache);
            }
        }
        #endregion

        #region static
        public static MinuteOfDay Now => Chronometer.Now;
        #endregion

        #region singleton風
        public static IChronos Instance { get; private set; }
        private static IChronometer Chronometer { get; }
        static Chronos() {
            if (Chronometer is null) {
                Chronometer = new Chronometer();
            }
        }
        #endregion

    }

    /// <summary> 正確な時計 </summary>
    public class Chronometer : IChronometer {
        #region getter
        public MinuteOfDay Now => new MinuteOfDay(System.DateTime.Now);
        #endregion
    }

    /// <summary>
    /// 1秒で1分経過する
    /// - 24分毎に一日経過
    /// </summary>
    public class VirtualChronometer : IChronometer {
        #region getter
        /// <summary> 分→時 / 秒→分 </summary>
        public MinuteOfDay Now {
            get {
                var now = System.DateTime.Now;
                return new MinuteOfDay(now.Minute % 24, now.Second);
            }
        }
        #endregion
    }

}
