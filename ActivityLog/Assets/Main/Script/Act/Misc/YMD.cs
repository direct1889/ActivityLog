using static UnityEngine.Mathf;

namespace Main {
    public struct YMD {
        #region field
        private System.DateTime Impl { get; }
        #endregion

        #region getter
        /// <value> 年(西暦) </value>
        public int Year  => Impl.Year;
        /// <value> 月∈[1,12] </value>
        public int Month => Impl.Month;
        /// <value> 日∈[1,31] </value>
        public int Date  => Impl.Day;

        public YMD AddDays(int days) {
            return new YMD(Impl.AddDays(days));
        }
        #endregion

        #region ctor/dtor
        public YMD(int year, int month, int date) {
            Impl = new System.DateTime(year, month, date);
        }
        public YMD(System.DateTime time) {
            Impl = time.Date;
        }
        #endregion

        #region override
        public override bool Equals(object obj) {
            if (obj == null || GetType() != obj.GetType()) { return false; }
            return Impl.Date == ((YMD)obj).Impl.Date;
        }
        public override int GetHashCode() => Impl.Date.GetHashCode();
        public override string ToString() => Impl.Date.ToShortDateString();
        #endregion

        #region operator
        public static bool operator< (YMD m, YMD n)
            => m.Impl < n.Impl;
        public static bool operator> (YMD m, YMD n)
            => m.Impl > n.Impl;
        public static bool operator== (YMD m, YMD n) => m.Equals(n);
        public static bool operator!= (YMD m, YMD n) => !m.Equals(n);
        public static bool operator<= (YMD m, YMD n)
            => m.Impl <= n.Impl;
        public static bool operator>= (YMD m, YMD n)
            => m.Impl >= n.Impl;
        #endregion

        #region static
        /// <param name="month"> ∈ [1, 12] 範囲外だとreturn 0 </param>
        private static int LastDate(int month) {
            if (0 < month && month <= 12) {
                if      (month <= 2) { return 28; }
                else if (month <= 7) { return (month % 2 == 1) ? 31 : 30; }
                else                 { return (month % 2 == 1) ? 30 : 31; }
            }
            else { return 0; }
        }

        /// <param name="month"> ∈ [1, 12] 範囲外だとreturn 0 </param>

        public static YMD Today   => new YMD(System.DateTime.Now);
        // public static YMD Today   => Dev.VirtualChronos.Now;
        // public static YMD Today   => End;
        public static YMD Tomorrow => Today.AddDays(1);
        public static YMD Yesterday => Today.AddDays(-1);
        #endregion
    }
}
