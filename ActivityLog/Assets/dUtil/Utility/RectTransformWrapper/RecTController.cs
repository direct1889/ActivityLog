using UnityEngine;
using static du.Ex.ExRecT;
using static du.Ex.ExVector;


namespace du.Cmp.RecT {

    public interface IRecTController {}

    public class RecTController : IRecTController {

        protected RectTransform RecT { get; }

        protected float OffmX { get { return RecT.offsetMin.x; } set { RecT.offsetMin = new Vector2(value, RecT.offsetMin.y); } }
        protected float OffmY { get { return RecT.offsetMin.y; } set { RecT.offsetMin = new Vector2(RecT.offsetMin.x, value); } }
        protected float OffMX { get { return RecT.offsetMax.x; } set { RecT.offsetMax = new Vector2(value, RecT.offsetMax.y); } }
        protected float OffMY { get { return RecT.offsetMax.y; } set { RecT.offsetMax = new Vector2(RecT.offsetMax.x, value); } }

        protected float LocalX { get { return RecT.localPosition.x; } set { RecT.localPosition = new Vector3(value, RecT.localPosition.y, RecT.localPosition.z); }}
        protected float LocalY { get { return RecT.localPosition.y; } set { RecT.localPosition = new Vector3(RecT.localPosition.x, value, RecT.localPosition.z); }}

        protected RecTController(RectTransform rect) { RecT = rect; }

    }

    public class RecTHorStretchBottom : RecTController {

        public float Left   { get { return OffmX; } set { OffmX = value; } }
        public float Right  { get { return OffMX; } set { OffMX = value; } }
        public float PosY   { get { return OffmY; } set { Height += value - OffmY; OffmY = value; } }
        public float Height { get { return OffMY - OffmY; } set { OffMY = value + OffmY; } }

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

}