using static Main.Act.DB.ExContentCSV;

namespace Main.Act {

    /// <summary>
    /// ActRecord系列(/日)の操作IFを提供するラッパー
    /// - 実データ系列(<see>IActSequence</see>)を保持
    /// </summary>
    public class ActSequenceMgr : IActSequenceMgr {
        #region field-property
        private IActRecordSequence Acts { get; set; }
        #endregion

        #region getter
        public IROActRecordSequence Activities => Acts;
        public string Dump() { return Acts.Dump(); }
        #endregion

        #region ctor
        public ActSequenceMgr() { Acts = new ActRecordSequence(); }
        #endregion

        #region public
        public virtual void BeginNewAct(IActivity act) {
            Acts.PushBack(new ActRecord(act, Context.BeginFromNow));
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
                // Border を過去に移動 -> 直後のActRecordを過去に伸ばす
                // 直前のActRecordは消滅する可能性があるため、確実に残る直後のBeginを変える
                Acts.OverwriteBeginTime(indexJustAfterBorder, newMinute);
            }
            else if (Acts[indexJustAfterBorder].Context.BeginTime < newMinute) {
                // Border を未来に移動 -> 直前のActRecordを未来に伸ばす
                // 直後のActRecordは消滅する可能性があるため、確実に残る直前のEndを変える
                Acts.OverwriteEndTime(indexJustAfterBorder - 1, newMinute);
            }
        }

        public void Package(YMD ymd) {
            IActivity currentAct = Acts.Back.Activity;
            Save(ymd);
            Acts = new ActRecordSequence();
            Acts.PushBack(new ActRecord(currentAct, new Context(MinuteOfDay.Begin)));
        }

        public void Load(YMD ymd) {
            var filePath = $"System/TrackLog/{ymd}";
            if (!System.IO.File.Exists(du.App.AppManager.DataPath + filePath + ".csv")) { return; }
            using (var r = new du.File.CSVReader<ActRecordDesc>(filePath, true)) {
                foreach (var desc in r) {
                    Acts.PushBack(desc.Instantiate());
                }
            }
        }
        public void Save(YMD ymd) {
            using (du.File.IFWriter w = du.File.FWriter.OpenFile4Rewrite($"System/TrackLog/{ymd}.csv")) {
                w.Write(ActRecord.CSVLabels);
                for (int i = 0; i < Acts.Count; ++i) {
                    w.Write(Acts[i].ToCSV());
                }
            }
        }
        #endregion
    }

    /// <summary> CSVを読み込みActRecordに変換 </summary>
    public class ActRecordDesc {
        #region field
        /// <value> SerialNumber </value>
        [du.File.CSVColAttr(0,0)] public int serialNumber;
        /// <value> BeginTime as ensuiteTime </value>
        [du.File.CSVColAttr(1,0)] public int beginTime;
        #endregion

        #region getter
        public override string ToString() {
            return $"SN:{serialNumber}({new MinuteOfDay(beginTime)}〜)";
        }

        /// <summary> CSVを読み込みIActRecordに変換 </summary>
        public IActRecord Instantiate() {
            return new ActRecord(CDB.Content.AtBySerialNumber(serialNumber).Act, new Context(new MinuteOfDay(beginTime)));
        }
        #endregion
    }

}
