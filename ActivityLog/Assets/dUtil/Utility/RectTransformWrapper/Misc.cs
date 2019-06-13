using UnityEngine;
using System.Collections.Generic;
using System.Linq;


namespace du.Misc {

    public static class Misc {

        //! The value exists in source => true
        public static bool IsIn<T>(T value, IEnumerable<T> source) {
            return source.Contains(value);
        }

        public static bool IsIn<T>(T value, T first, T second, params T[] sources) {
            return IsIn(value, sources);
        }


        public static Vector2 PolarToVector2(float theta, float r) {
            return new Vector2(r * Mathf.Cos(theta), r * Mathf.Sin(theta));
        }

        public static string Float2Str(float value) {
            return Mathf.Approximately(value, 0f) ? "0"
                : Mathf.Approximately(value, 1.0f) ? "1"
                : Mathf.Approximately(value, -1.0f) ? "-1"
                : value.ToString();
        }

        public static bool IsSameQuadrant(Vector2 v1, Vector2 v2) {
            return v1.x * v2.x >= 0 && v1.y * v2.y >= 0;
        }

        // public static Vector3 PolarToVector3(float theta, float phi, float r) {
        //    return new Vector3(r * Mathf.Cos(theta), r * Mathf.Sin(theta));
        // }

    }

}