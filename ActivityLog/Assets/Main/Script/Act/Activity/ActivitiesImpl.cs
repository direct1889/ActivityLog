using System.Collections.Generic;
using UnityEngine;

namespace Main.Act {

    /// <summary> アクティビティ系列/日を操作する </summary>
    public class ActivitiesMgr : IActivitiesMgr {
        #region property
        public IROActSequence Activities { get { return Acts; } }
        protected IActSequence Acts { get; }
        #endregion

        #region ctor/dtor
        public ActivitiesMgr() { Acts = new ActSequence(); }
        #endregion

        #region public
        public void BeginNewAct(IProject proj, string name, bool isEffective) {
            Acts.PushBack(new Activity(new Content(proj, name, isEffective), Context.BeginFromNow));
        }
        public void BeginNewAct(IProject proj, string name) {
            Acts.PushBack(new Activity(new Content(proj, name), Context.BeginFromNow));
        }
        public void ChangeBorder(int indexJustAfterBorder, MinuteOfDay newMinute) {
            if (indexJustAfterBorder <= 0) { return; }
            else if (Acts[indexJustAfterBorder].Context.BeginTime > newMinute) {
                // Border を過去に移動 -> 直後のアクティビティを過去に伸ばす
                Acts.OverwriteBeginTime(indexJustAfterBorder, newMinute);
            }
            else if (Acts[indexJustAfterBorder].Context.BeginTime < newMinute) {
                // Border を未来に移動 -> 直前のアクティビティを未来に伸ばす
                Acts.OverwriteEndTime(indexJustAfterBorder - 1, newMinute);
            }
        }
        #endregion
    }

    public class ActivitiesMgr4Test : ActivitiesMgr {
        public void BeginNewAct(IProject proj, string name, bool isEffective, MinuteOfDay beginTime) {
            Acts.PushBack(new Activity(new Content(proj, name, isEffective), new Context(beginTime)));
        }
        public void BeginNewAct(IProject proj, string name, MinuteOfDay beginTime) {
            if (Acts.Count > 0) {
                Debug.LogError($"BeginNewActBefore[{Acts.Back.Context.BeginTime}><{beginTime}]");
            }
            Acts.PushBack(new Activity(new Content(proj, name), new Context(beginTime)));
            Debug.LogError($"BeginNewActAfter[{Acts.Back.Context.BeginTime}><{beginTime}]");
        }
    }

}
