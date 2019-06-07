
namespace du.Cmp {

    public abstract class EqualsComparable<T> {
        #region override
        public override bool Equals(object obj) {
            if (obj == null || GetType() != obj.GetType()) { return false; }
            return GetHashCode() == ((T)obj).GetHashCode();
        }
        public abstract override int GetHashCode();
        #endregion

        #region operator
        public static bool operator== (EqualsComparable<T> m, EqualsComparable<T> n) { return m.Equals(n); }
        public static bool operator!= (EqualsComparable<T> m, EqualsComparable<T> n) { return m.Equals(n); }
        #endregion
    }

}
