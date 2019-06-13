
namespace Main {
    public struct MinuteOfDay {
        #region field
        private int m_ensuiteMinute;
        #endregion

        #region property
        public int EnsuiteMinute {
            get => m_ensuiteMinute;
            set {
                if (0 <= value && value <= 24*60) {
                    m_ensuiteMinute = value;
                }
                else {
                    UnityEngine.Debug.LogError($"ArgumentException:Value must be between 0 to {24*60} (but value == {value}).");
                }
            }
        }
        #endregion

        #region getter-property
        public int Hour => EnsuiteMinute / 60;
        public int Minute => EnsuiteMinute % 60;
        #endregion

        #region ctor/dtor
        public MinuteOfDay(int ensuiteMinute) : this() { EnsuiteMinute = ensuiteMinute; }
        public MinuteOfDay(int hour, int minute) : this() { EnsuiteMinute = hour * 60 + minute; }
        public MinuteOfDay(System.DateTime time) : this(time.Hour, time.Minute) {}
        #endregion

        #region override
        public override bool Equals(object obj) {
            if (obj == null || GetType() != obj.GetType()) { return false; }
            return EnsuiteMinute == ((MinuteOfDay)obj).EnsuiteMinute;
        }
        public override int GetHashCode() => EnsuiteMinute;
        public override string ToString() => $"{Hour:00}:{Minute:00}";
        #endregion

        #region operator
        public static int operator- (MinuteOfDay end, MinuteOfDay begin)
            => end.EnsuiteMinute - begin.EnsuiteMinute;
        public static bool operator< (MinuteOfDay m, MinuteOfDay n)
            => m.EnsuiteMinute < n.EnsuiteMinute;
        public static bool operator> (MinuteOfDay m, MinuteOfDay n)
            => m.EnsuiteMinute > n.EnsuiteMinute;
        public static bool operator== (MinuteOfDay m, MinuteOfDay n) => m.Equals(n);
        public static bool operator!= (MinuteOfDay m, MinuteOfDay n) => !m.Equals(n);
        public static bool operator<= (MinuteOfDay m, MinuteOfDay n)
            => m.EnsuiteMinute <= n.EnsuiteMinute;
        public static bool operator>= (MinuteOfDay m, MinuteOfDay n)
            => m.EnsuiteMinute >= n.EnsuiteMinute;
        #endregion

        #region static
        public static MinuteOfDay Now   => new MinuteOfDay(System.DateTime.Now);
        public static MinuteOfDay Begin => new MinuteOfDay(0);
        public static MinuteOfDay End   => new MinuteOfDay(24, 0);
        #endregion
    }
}
