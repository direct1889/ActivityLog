
namespace Main {
    public class MinuteOfDay {
        #region field
        int m_enusuiteMinute = 0;
        #endregion

        #region property
        public int EnsuiteMinute {
            get { return m_enusuiteMinute; }
            set {
                if (0 <= value && value <= 24*60) {
                    m_enusuiteMinute = value;
                }
                else {
                    UnityEngine.Debug.LogError($"ArgumentException:Value must be between 0 to {24*60} (but value == {value}).");
                }
            }
        }
        #endregion

        #region getter-property
        public int Hour { get { return EnsuiteMinute / 60; } }
        public int Minute { get { return EnsuiteMinute % 60; } }
        #endregion

        #region ctor/dtor
        public MinuteOfDay(int ensuiteMinute) { EnsuiteMinute = ensuiteMinute; }
        public MinuteOfDay(int hour, int minute) { EnsuiteMinute = hour * 60 + minute; }
        public MinuteOfDay(System.DateTime time) : this(time.Hour, time.Minute) {}
        #endregion

        #region operator
        public static int operator- (MinuteOfDay end, MinuteOfDay begin) {
            return end.EnsuiteMinute - begin.EnsuiteMinute;
        }
        public static bool operator< (MinuteOfDay m, MinuteOfDay n) {
            return m.EnsuiteMinute < n.EnsuiteMinute;
        }
        public static bool operator> (MinuteOfDay m, MinuteOfDay n) {
            return m.EnsuiteMinute > n.EnsuiteMinute;
        }
        public static bool operator== (MinuteOfDay m, MinuteOfDay n) {
            return m.EnsuiteMinute == n.EnsuiteMinute;
        }
        public static bool operator!= (MinuteOfDay m, MinuteOfDay n) {
            return m.EnsuiteMinute != n.EnsuiteMinute;
        }
        public static bool operator<= (MinuteOfDay m, MinuteOfDay n) {
            return m.EnsuiteMinute <= n.EnsuiteMinute;
        }
        public static bool operator>= (MinuteOfDay m, MinuteOfDay n) {
            return m.EnsuiteMinute >= n.EnsuiteMinute;
        }
        #endregion

        #region static
        public static MinuteOfDay Now {
            get { return new MinuteOfDay(System.DateTime.Now); }
        }
        public static MinuteOfDay Zero {
            get { return new MinuteOfDay(0); }
        }
        #endregion
    }
}
