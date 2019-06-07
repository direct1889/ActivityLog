using UnityEngine;
using System.Collections.Generic;
using static du.Ex.ExDictionary;


namespace du.Test {

    public static class Misc {
        public static void IsNull(object obj, string name) {
            if (obj is null) { Debug.LogError($"{name} is null."); }
            else { Debug.LogError($"{name} is not null."); }
        }
    }

    public interface ILogLayer {
        void Log(object message);
        void LogError(object message);
        void LogAssertion(object message);
    }

    public interface IOperableLogLayer : ILogLayer {
        void SetActive(bool isActive);
    }

    public class LogLayer : IOperableLogLayer {
        readonly string m_label = "";
        bool m_isActive = true;

        public LogLayer(string label, bool isActive = true) {
            m_label = label;
            m_isActive = isActive;
        }

        public void SetActive(bool isActive) { m_isActive = isActive; }
        public void Log            (object message) { if (m_isActive) { Debug.Log         ($"[{m_label}]::{message}"); } }
        public void LogError    (object message) { if (m_isActive) { Debug.LogError    ($"[{m_label}]::{message}"); } }
        public void LogAssertion(object message) { if (m_isActive) { Debug.LogAssertion($"[{m_label}]::{message}"); } }
    }

    public static class LayeredLog {
        static IDictionary<string, IOperableLogLayer> m_layers;

        public static void Initialize(bool isActiveDefault = true) {
            if (m_layers == null) {
                m_layers = new Dictionary<string, IOperableLogLayer>();
                Add("BOOT"   , isActiveDefault);
                Add("DEBUG"  , isActiveDefault);
                Add("MISC"   , isActiveDefault);
                Add("MAIN:BOOT", isActiveDefault);
                LLog.Boot.Log("LayeredLog booted.");
            }
        }
        public static void SetActive(string layerLabel, bool isActive) {
            m_layers.At(layerLabel)?.SetActive(isActive);
        }
        public static void SetActiveAll(bool isActive) {
            foreach (var i in m_layers.Values) { i.SetActive(isActive); }
        }
        public static ILogLayer At(string layerLabel) {
            if (m_layers.ContainsKey(layerLabel)) {
                return m_layers[layerLabel];
            }
            else {
                Debug.LogAssertion($"Layer \"{layerLabel}\" doesn't exist.");
                return null;
            }
        }

        private static void Add(string label, bool isActive = true) {
            if (m_layers.ContainsKey(label)) { SetActive(label, isActive); }
            else { m_layers.Add(label, new LogLayer(label, isActive)); }
        }
    }

    public static class LLog {
        public static ILogLayer Boot  => LayeredLog.At("BOOT");
        public static ILogLayer Debug => LayeredLog.At("DEBUG");
        public static ILogLayer Misc  => LayeredLog.At("MISC");
        public static ILogLayer MBoot => LayeredLog.At("MAIN:BOOT");
    }

    public static class Log {

        public static bool IsNull(object obj, string name) {
            if (obj == null) {
                Debug.Log(name + " is null!!");
                return true;
            }
            else {
                Debug.Log(name + " is not null.");
                return false;
            }
        }

        public static T TL<T>(this T obj) {
            Debug.Log(obj);
            return obj;
        }
        public static T TL<T>(this T obj, string foreword) {
            Debug.Log(foreword + obj.ToString());
            return obj;
        }
        public static T TL<T>(this T obj, string foreword, string afterword) {
            Debug.Log(foreword + obj.ToString() + afterword);
            return obj;
        }

        public static T TLC<T>(this T obj, string colorTag) {
            return obj.TL("<color=" + colorTag + ">", "</color>");
        }
        public static T TLC<T>(this T obj, string colorTag, string foreword) {
            return obj.TL("<color=" + colorTag + ">" + foreword, "</color>");
        }
        public static T TLC<T>(this T obj, string colorTag, string foreword, string afterword) {
            return obj.TL("<color=" + colorTag + ">" + foreword, afterword + "</color>");
        }

        public static T TLCRed<T>(this T obj) {
            return obj.TL("<color=red>", "</color>");
        }
        public static T TLCRed<T>(this T obj, string foreword) {
            return obj.TL("<color=red>" + foreword, "</color>");
        }
        public static T TLCRed<T>(this T obj, string foreword, string afterword) {
            return obj.TL("<color=red>" + foreword, afterword + "</color>");
        }


    }

}

