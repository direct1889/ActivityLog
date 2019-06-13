/*  参考:RectTransformのAnchorのプリセットをスクリプトから「インスペクタ上のenumと同じように」指定する為の拡張クラス
*   https://gist.github.com/neon-izm/512a439fe6d07348f6f421c6061338e3
*/
using UnityEngine;
using Anchor = du.Cmp.RecT.Anchor;
using Pivot = du.Cmp.RecT.Pivot;
using RT = UnityEngine.RectTransform;

namespace du.Cmp.RecT {

    /// <summary>
    /// RectTransformのAnchorのプリセットをスクリプトから「インスペクタ上のenumと同じように」
    /// 指定する為の拡張クラス
    /// プロジェクト内の適当な場所に保存しておくと、以下のようにRectTransformに対してSetAnchor()メソッドが使えるようになる
    /// </summary>

    /* usage
      var rect = GetComponent<RectTransform>();
      rect.SetAnchor(Anchor.MiddleCenter);
      rect.localPosition = Vector3.zero;
      */
    public enum Anchor {
        TopLeft, TopCenter, TopRight,
        MiddleLeft, MiddleCenter, MiddleRight,
        BottomLeft, BottonCenter, BottomRight, BottomStretch,
        VertStretchLeft, VertStretchRight, VertStretchCenter,
        HorStretchTop, HorStretchMiddle, HorStretchBottom,
        StretchAll
    }

    public enum Pivot {
        TopLeft, TopCenter, TopRight,
        MiddleLeft, MiddleCenter, MiddleRight,
        BottomLeft, BottomCenter, BottomRight,
    }

}


namespace du.Ex {

    public static class ExRecT {

        public static void AnchorMM(this RT src, float minX, float minY, float maxX, float maxY) {
            // 点で指定     => Min = Max
            // ストレッチ   => Min = 0, Max = 1
            src.anchorMin = new Vector2(minX, minY);
            src.anchorMax = new Vector2(maxX, maxY);
        }
        // 一点指定アンカー(ストレッチしない)
        public static void AnchorPoint(this RT src, float x, float y) {
            src.anchorMin = src.anchorMax = new Vector2(x, y);
        }
        public static void AnchorStretchX(this RT src, float y) { src.AnchorMM(0, y, 1, y); }
        public static void AnchorStretchY(this RT src, float x) { src.AnchorMM(x, 0, x, 1); }
        public static void AnchorStretch(this RT src) { src.AnchorMM(0, 0, 1, 1); }

        public static void PivotXY(this RT src, float x, float y) {
            src.pivot = new Vector2(x, y);
        }

        public static void OffsetMM(this RT src, float left, float bottom, float right, float top) {
            src.offsetMin = new Vector2(left, bottom);
            src.offsetMax = new Vector2(right, top);
        }

        public static void SetAnchor(this RT src, Anchor allign, int offsetX = 0, int offsetY = 0) {
            src.anchoredPosition = new Vector3(offsetX, offsetY, 0);
            switch (allign) {
                case (Anchor.TopLeft          ): src.AnchorPoint(0.0f, 1.0f); break;
                case (Anchor.TopCenter        ): src.AnchorPoint(0.5f, 1.0f); break;
                case (Anchor.TopRight         ): src.AnchorPoint(1.0f, 1.0f); break;
                case (Anchor.MiddleLeft       ): src.AnchorPoint(0.0f, 0.5f); break;
                case (Anchor.MiddleCenter     ): src.AnchorPoint(0.5f, 0.5f); break;
                case (Anchor.MiddleRight      ): src.AnchorPoint(1.0f, 0.5f); break;
                case (Anchor.BottomLeft       ): src.AnchorPoint(0.0f, 0.0f); break;
                case (Anchor.BottonCenter     ): src.AnchorPoint(0.5f, 0.0f); break;
                case (Anchor.BottomRight      ): src.AnchorPoint(1.0f, 0.0f); break;
                case (Anchor.HorStretchTop    ): src.AnchorStretchX(1.0f); break;
                case (Anchor.HorStretchMiddle ): src.AnchorStretchX(0.5f); break;
                case (Anchor.HorStretchBottom ): src.AnchorStretchX(0.0f); break;
                case (Anchor.VertStretchLeft  ): src.AnchorStretchY(0.0f); break;
                case (Anchor.VertStretchCenter): src.AnchorStretchY(0.5f); break;
                case (Anchor.VertStretchRight ): src.AnchorStretchY(1.0f); break;
                case (Anchor.StretchAll       ): src.AnchorStretch(); break;
            }
        }

        public static void SetPivot(this RT src, Pivot preset) {
            switch (preset) {
                case (Pivot.TopLeft     ): src.PivotXY(0.0f, 1.0f); break;
                case (Pivot.TopCenter   ): src.PivotXY(0.5f, 1.0f); break;
                case (Pivot.TopRight    ): src.PivotXY(1.0f, 1.0f); break;
                case (Pivot.MiddleLeft  ): src.PivotXY(0.0f, 0.5f); break;
                case (Pivot.MiddleCenter): src.PivotXY(0.5f, 0.5f); break;
                case (Pivot.MiddleRight ): src.PivotXY(1.0f, 0.5f); break;
                case (Pivot.BottomLeft  ): src.PivotXY(0.0f, 0.0f); break;
                case (Pivot.BottomCenter): src.PivotXY(0.5f, 0.0f); break;
                case (Pivot.BottomRight ): src.PivotXY(1.0f, 0.0f); break;
            }
        }

    }

}
