using UnityEngine;


namespace du.Ex {

    public static class ExString {

        public static bool IsEmpty(this string str) {
            return str is null || str == "";
        }

    }

}