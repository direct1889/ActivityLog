
namespace Main.Act {

    /// <summary>
    /// ActRecord系列(/日)の操作IFを提供するラッパー
    /// - 実データ系列(<see>IActSequence</see>)を保持
    /// </summary>
    public interface IActSequenceMgr {
        IROActRecordSequence Activities { get; }

        /// <summary>
        /// 新たなActRecordを開始
        /// - 現在のActRecordを終了、開始時刻は現在時刻を使用
        /// </summary>
        void BeginNewAct(IActivity act);
        void BeginNewAct(IProject proj, string name, bool isEffective);
        void BeginNewAct(IProject proj, string name);

        /// <summary>
        /// 連続したActRecord間の時刻境界を操作
        /// - 潰れたActRecordは消滅
        /// </summary>
        /// <param name="indexJustAfterBorder">
        /// 操作したい境界の直後のActRecordの index
        /// </param>
        void ChangeBorder(int indexJustAfterBorder, MinuteOfDay newMinute);

        /// <summary> ある日の系列をファイルから読み込み </summary>
        void Load(YMD ymd);
        /// <summary> 保持している系列をファイルへ書き出し </summary>
        void Save(YMD ymd);
    }


}
