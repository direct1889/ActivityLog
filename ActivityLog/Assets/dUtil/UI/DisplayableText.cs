using System.Collections.Generic;
using UnityEngine;
using UGUI = UnityEngine.UI;
using UniRx;
using System;

namespace du.dui {

    public interface IDisplayableText : ITextArea {
        void DisplayGradual(string text);
        void DisplayGradual(string text, Action actionOnComplete);
    }

    public class DisplayableText : MonoBehaviour, IDisplayableText {

        #region static
        static float? sIntervalMS = null;
        // null指定でグローバル設定を無効化
        public static void SetGlobalInterval(float? intervalMS) { sIntervalMS = intervalMS; }
        #endregion

        #region field
        [SerializeField] int m_maxTextLength = 90;
        [SerializeField] int m_maxLine = 3;
        [SerializeField] float m_intervalMS = 0.05f;

        [SerializeField] string Msg;

        UGUI.Text m_textUI;
        string m_message;

        int m_hasDispLength = 0;   // 現在表示されている文字数
        int m_hasDispLine = 0;     // 現在表示されている行数
        int m_dispIt = 0;          // 今見ている文字番号
        IReactiveProperty<bool> m_hasDispAll = new ReactiveProperty<bool>(false);      // メッセージ全文表示完了

        IDisposable m_displayStream;

        // マウスクリックを促すアイコン
        // private Image clickIcon;
        // クリックアイコンの点滅秒数
        // [SerializeField] private float clickFlashTime = 0.2f;
        #endregion

        #region mono
        void Awake () {
            if (sIntervalMS != null) {
                m_intervalMS = (float)sIntervalMS;
            }
        }
        void Start () {
            m_textUI = GetComponentInChildren<UGUI.Text>();
            Text = "";
            if (Msg != "") { DisplayGradual(Msg); }
        }
        #endregion

        #region public
        public string Text {
            set => m_textUI.text = value;
            get => m_textUI.text;
        }
        public void Add(string text){ m_textUI.text += text; }
        public void Add(char c)     { m_textUI.text += c;    }
        public void DisplayGradual(string text) { DisplayGradual(text, null); }
        public void DisplayGradual(string text, Action actionOnComplete) {
            // 一定時間ごとにテキストを順に表示するストリーム
            m_displayStream =
                Observable
                    .Interval(TimeSpan.FromMilliseconds(m_intervalMS))
                    .Subscribe(l => DispNextChar())
                    .AddTo(this);
            // テキストが全て表示し終わったら表示ストリームを停止する
            m_hasDispAll
                .Where(hasDisp => hasDisp)
                .Subscribe(_ => {
                    m_displayStream.Dispose();
                    m_displayStream = null;
                    actionOnComplete?.Invoke();
                })
                .AddTo(this);
            m_message = text;
        }
        #endregion

        #region private
        bool IsDisplaying => m_displayStream != null;
        void DispNextChar() {
            if (m_message == null) { return; }

            Add(NextChar);
            if (NextChar == '\n') { ++m_hasDispLine; }
            ++m_dispIt;
            ++m_hasDispLength;

            if (m_dispIt >= m_message.Length
                || m_hasDispLength >= m_maxTextLength
                || m_hasDispLine >= m_maxLine)
            { m_hasDispAll.Value = true; }
        }
        char NextChar => m_message[m_dispIt];
        #endregion

    }

}
