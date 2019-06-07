using System.Collections.Generic;
using UC = UnityEngine.Color;

namespace Main {

    public struct ThemeColor {
        readonly string name;
        public ThemeColor(string name) { this.name = name; }
        public override string ToString() { return name; }
        public static implicit operator UC(ThemeColor color) {
            return ThemeColors.At(color.name);
        }
    }

    public static class ThemeColors {
        static IDictionary<string, UC> m_colors = new Dictionary<string, UC>{
            { "Red"    , new UC(1.0f, 0.8f, 0.5f) },
            { "Blue"   , new UC(0.6f, 0.6f, 1.0f) },
            { "Green"  , new UC(0.5f, 0.8f, 0.6f) },
            { "Brown"  , new UC(0.5f, 0.2f, 0.2f) },
            { "Gray"   , new UC(0.7f, 0.7f, 0.7f) },
        };
        const string defaultName = "Gray";

        /// <summary> 登録されていない名前の場合DefaultColorを返す
        public static UC At(string name) {
            if (m_colors.ContainsKey(name)) {
                return m_colors[name];
            }
            else { return Default; }
        }

        public static ThemeColor Get(string name) {
            return new ThemeColor(m_colors.ContainsKey(name) ? name : defaultName);
        }

        public static ThemeColor Red     { get { return new ThemeColor("Red"); } }
        public static ThemeColor Blue    { get { return new ThemeColor("Blue"); } }
        public static ThemeColor Green   { get { return new ThemeColor("Green"); } }
        public static ThemeColor Brown   { get { return new ThemeColor("Brown"); } }
        public static ThemeColor Gray    { get { return new ThemeColor("Gray"); } }
        public static ThemeColor Default { get { return new ThemeColor(defaultName); } }
    }

}
