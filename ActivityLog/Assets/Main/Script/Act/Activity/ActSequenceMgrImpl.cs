
namespace Main.Act {

    /// <summary> アクティビティ系列/日を操作する </summary>
    public class ActSequenceMgr : IActSequenceMgr {
        #region field-property
        protected IActRecordSequence Acts { get; }
        #endregion

        #region getter
        public IROActRecordSequence Activities => Acts;
        #endregion

        #region ctor
        public ActSequenceMgr() { Acts = new ActRecordSequence(); }
        #endregion

        #region public
        public virtual void BeginNewAct(IActivity content) {
            Acts.PushBack(new ActRecord(content, Context.BeginFromNow));
        }
        public virtual void BeginNewAct(IProject proj, string name, bool isEffective) {
            BeginNewAct(new Activity(proj, name, isEffective));
        }
        public virtual void BeginNewAct(IProject proj, string name) {
            BeginNewAct(new Activity(proj, name));
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

    public class ActSequenceMgr4Test : ActSequenceMgr {
        MinuteOfDay m_tempTimeSign = MinuteOfDay.Begin;
        int duration = 100;

        public override void BeginNewAct(IActivity content) {
            Acts.PushBack(new ActRecord(content, m_tempTimeSign));
            m_tempTimeSign.EnsuiteMinute += duration;
        }
        public override void BeginNewAct(IProject proj, string name, bool isEffective) {
            Acts.PushBack(new ActRecord(proj, name, isEffective, m_tempTimeSign));
            m_tempTimeSign.EnsuiteMinute += duration;
        }
        public override void BeginNewAct(IProject proj, string name) {
            Acts.PushBack(new ActRecord(proj, name, m_tempTimeSign));
            m_tempTimeSign.EnsuiteMinute += duration;
        }
    }

}
