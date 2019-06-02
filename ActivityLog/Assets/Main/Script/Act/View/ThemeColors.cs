using System.Collections.Generic;
using UC = UnityEngine.Color;

namespace Main {

    public static class ThemeColors {
        public static UC Red   { get { return new UC(1.0f, 0.8f, 0.5f); } }
        public static UC Blue  { get { return new UC(0.6f, 0.6f, 1.0f); } }
        public static UC Green { get { return new UC(0.5f, 0.8f, 0.6f); } }
        public static UC Brown { get { return new UC(0.5f, 0.2f, 0.2f); } }
        public static UC Gray  { get { return new UC(0.7f, 0.7f, 0.7f); } }
    }

}
