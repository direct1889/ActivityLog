
namespace Main.Act {

    /// <summary> アクティビティ系列/日を操作する </summary>
    public class ActivitiesMgr : IActivitiesMgr {
        #region property
        public IROActSequence Activities => Acts;
        protected IActSequence Acts { get; }
        #endregion

        #region ctor/dtor
        public ActivitiesMgr() { Acts = new ActSequence(); }
        #endregion

        #region public
        public virtual void BeginNewAct(IROContent content) {
            Acts.PushBack(new Activity(content, Context.BeginFromNow));
        }
        public virtual void BeginNewAct(IProject proj, string name, bool isEffective) {
            BeginNewAct(new Content(proj, name, isEffective));
        }
        public virtual void BeginNewAct(IProject proj, string name) {
            BeginNewAct(new Content(proj, name));
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
        MinuteOfDay m_tempTimeSign = MinuteOfDay.Begin;
        int duration = 100;

        public override void BeginNewAct(IROContent content) {
            Acts.PushBack(new Activity(content, m_tempTimeSign));
            m_tempTimeSign.EnsuiteMinute += duration;
        }
        public override void BeginNewAct(IProject proj, string name, bool isEffective) {
            Acts.PushBack(new Activity(proj, name, isEffective, m_tempTimeSign));
            m_tempTimeSign.EnsuiteMinute += duration;
        }
        public override void BeginNewAct(IProject proj, string name) {
            Acts.PushBack(new Activity(proj, name, m_tempTimeSign));
            m_tempTimeSign.EnsuiteMinute += duration;
        }
    }

}
