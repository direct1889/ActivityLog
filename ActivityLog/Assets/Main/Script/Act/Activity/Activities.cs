
namespace Main.Act {

    /// <summary>
    /// アクティビティ系列/日を操作する
    /// - <see> IActSequence </see> のラッパー
    /// </summary>
    public interface IActivitiesMgr {
        IROActRecordSequence Activities { get; }

        /// <summary>
        /// 新たなアクティビティを開始
        /// 現在のアクティビティを終了、開始時刻は現在時刻を使用
        /// </summary>
        void BeginNewAct(IActivity act);
        void BeginNewAct(IProject proj, string name, bool isEffective);
        void BeginNewAct(IProject proj, string name);

        /// <summary>
        /// 連続したアクティビティ間の時刻境界を操作
        /// - 潰れたアクティビティは消滅
        /// </summary>
        /// <param name="indexJustAfterBorder">
        /// 操作したい境界の直後のアクティビティの index
        /// </param>
        void ChangeBorder(int indexJustAfterBorder, MinuteOfDay newMinute);
    }


}
