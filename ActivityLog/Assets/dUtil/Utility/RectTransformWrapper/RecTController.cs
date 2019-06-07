using UnityEngine;
using static du.Ex.ExRecT;
using static du.Ex.ExVector;


namespace du.Cmp.RecT {

    public interface IRecTController {}

    public class RecTController : IRecTController {

        protected RectTransform RecT { get; }

        protected float OffmX { get => RecT.offsetMin.x; set => RecT.offsetMin = new Vector2(value, RecT.offsetMin.y); }
        protected float OffmY { get => RecT.offsetMin.y; set => RecT.offsetMin = new Vector2(RecT.offsetMin.x, value); }
        protected float OffMX { get => RecT.offsetMax.x; set => RecT.offsetMax = new Vector2(value, RecT.offsetMax.y); }
        protected float OffMY { get => RecT.offsetMax.y; set => RecT.offsetMax = new Vector2(RecT.offsetMax.x, value); }

        protected float LocalX { get => RecT.localPosition.x; set => RecT.localPosition = new Vector3(value, RecT.localPosition.y, RecT.localPosition.z); }
        protected float LocalY { get => RecT.localPosition.y; set => RecT.localPosition = new Vector3(RecT.localPosition.x, value, RecT.localPosition.z); }

        protected RecTController(RectTransform rect) { RecT = rect; }

        public override string ToString() { return $"Offset[Min({OffmX}, {OffmY}), Max({OffMX}, {OffMY})]"; }

    }

    /// <summary> H:ストレッチ,V:Bottom,Pivot:底辺中央 </summary>
    public class RecTHorStretchBottom : RecTController {

        public float Left   { get => OffmX;         set => OffmX = value; }
        public float Right  { get => OffMX;         set => OffMX = value; }
        public float PosY   { get => OffmY;         set { Height += value - OffmY; OffmY = value; } }
        public float Height { get => OffMY - OffmY; set => OffMY = value + OffmY; }

        public RecTHorStretchBottom(RectTransform rect) : base(rect) {}

        public void Initialize(Transform parent) {
            RecT.SetParent(parent);
            RecT.localScale = Vector3.one;
            RecT.SetPivot(Pivot.BottomCenter);
            RecT.SetAnchor(Anchor.HorStretchBottom);
        }
        public void Set(float left, float right, float posY, float height) {
            Left   = left  ; Right  = right ;
            PosY   = posY  ; Height = height;
        }

    }

    /// <summary> 右上原点 </summary>
    public class RecTRightTop : RecTController {

        /// <value> 右上頂点のX座標 </value>
        public float PosX   { get => OffMX; set => OffMX = value; }
        /// <value> 右上頂点のY座標 </value>
        public float PosY   { get => OffMY; set => OffMY = value; }

        public float Width  { get => OffMX - OffmX; set => OffmX = OffMX - value; }
        public float Height { get => OffMY - OffmY; set => OffmY = OffMY - value; }

        /// <summary> Transform親子関係の設定も合わせて行う </summary>
        public RecTRightTop(RectTransform rect, Transform parent) : base(rect) {
            Initialize(parent);
        }
        /// <summary> 事前に親設定済みの場合に使用 </summary>
        public RecTRightTop(RectTransform rect) : base(rect) {
            Initialize();
        }

        public void Initialize() {
            if (RecT.parent == null) { Debug.LogError("ParentIsNull!!"); }
            RecT.localScale = Vector3.one;
            RecT.SetPivot(Pivot.TopRight);
            RecT.SetAnchor(Anchor.TopRight);
            RecT.position = RecT.position.ReZ(0);
        }
        public void Initialize(Transform parent) {
            RecT.SetParent(parent);
            Initialize();
        }
        public void Set(float posX, float posY, float width, float height) {
            PosX = posX; Width  = width ;
            PosY = posY; Height = height;
        }

        public override string ToString() { return $"RT[Pos({PosX}, {PosY}), Size({Width}, {Height})]"; }
    }

}