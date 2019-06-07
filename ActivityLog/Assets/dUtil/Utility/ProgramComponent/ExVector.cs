using UnityEngine;


namespace du.Ex {

    public static class ExVector {

        public static Vector2 Vec3to2(this Vector3 vec3) {
            return new Vector2(vec3.x, vec3.y);
        }
        public static Vector3 AddZ(this Vector2 vec2, float z) {
            return new Vector3(vec2.x, vec2.y, z);
        }

        public static Vector2 DisX(this Vector3 vec3) {
            return new Vector2(vec3.y, vec3.z);
        }
        public static Vector2 DisY(this Vector3 vec3) {
            return new Vector2(vec3.x, vec3.z);
        }
        public static Vector2 DisZ(this Vector3 vec3) {
            return new Vector2(vec3.x, vec3.y);
        }

        public static Vector3 ReX(this Vector3 vec3, float x) {
            return new Vector3(x, vec3.y, vec3.z);
        }
        public static Vector3 ReY(this Vector3 vec3, float y) {
            return new Vector3(vec3.x, y, vec3.z);
        }
        public static Vector3 ReZ(this Vector3 vec3, float z) {
            return new Vector3(vec3.x, vec3.y, z);
        }

        public static Vector2 ReX(this Vector2 vec2, float x) {
            return new Vector2(x, vec2.y);
        }
        public static Vector2 ReY(this Vector2 vec2, float y) {
            return new Vector2(vec2.x, y);
        }

        public static Vector2 Clamped(this Vector2 vec2, float min, float max) {
            return new Vector2(Mathf.Clamp(vec2.x, min, max), Mathf.Clamp(vec2.y, min, max));
        }

        public static float Theta(this Vector2 vec2) {
            return Mathf.Atan2(vec2.y, vec2.x);
        }

        public static Vector2 Resized(this Vector2 vec2, float length) {
            if (Mathf.Approximately(vec2.magnitude, 0f)) {
                return Vector2.zero;
            }
            else {
                return vec2 * length / vec2.magnitude;
            }
        }

        public static Vector3 DisN(this Vector3? nullable, Vector3 defaultValue) {
            return nullable ?? defaultValue;
        }
        public static Vector2 DisN(this Vector2? nullable, Vector2 defaultValue) {
            return nullable ?? defaultValue;
        }
        public static Vector3 DisN0(this Vector3? nullable) {
            return nullable ?? Vector3.zero;
        }
        public static Vector2 DisN0(this Vector2? nullable) {
            return nullable ?? Vector2.zero;
        }


    }

}