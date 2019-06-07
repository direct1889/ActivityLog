
using System.Collections.Generic;
using UnityEngine;
using UColor = UnityEngine.Color;
using System.Linq;
using static Main.Act.DB.ExProject;

namespace Main.Act.DB {

    public static class ContentDB {
        #region property
        public static IProjectDB Proj { get; } = new ProjectDB();
        public static IActivityDB Act { get; } = new ActivityDB();
        #endregion

        #region public
        public static void Initialize() {
            du.Test.LLog.MBoot.Log("Initialized ContentDB.");
            Proj.Initialize();  // 必ずProjを先に初期化
            Act.Initialize();   // Actの初期化時Projにアクセスする
        }
        #endregion
    }

    public interface IActivityDB : du.Cmp.IOrderedMap<IROContent> {
        //! 登録済みActivity一覧の生成
        void Initialize();
        /// <summary> ActivityをEnumerableで一括取得 </summary>
        IEnumerable<IROContent> Sorted(IProject parent);
    }

    /// <summary> 全てのプロジェクトは事前にDBに登録が必要 </summary>
    public class ActivityDB : du.Cmp.OrderedMap<IROContent>, IActivityDB {
        #region public
        //! 登録済みActivity一覧の生成
        public void Initialize() {
            // 色はプリセットからの選択式
            Load("System/Activities");
            // AddAct("pabulum"     , "安達としまむら");
            // AddAct("pabulum"     , "ユーフォ");
            // AddAct("MisLead"     , "面接練習");
            // // AddAct("Dev"         , "");
            // AddAct("Unity"       , "ActivityLog");
            // AddAct("Unproductive", "Twitter");
            // AddAct("TestSample"  , "Hoge");
        }

        /// <summary> 指定したProjectを直属の親に持つActivityのみを取得 </summary>
        public IEnumerable<IROContent> Sorted(IProject parent) {
            return Order
                .Where(key => At(key).Parent == parent)
                .Select(key => At(key));
        }
        #endregion

        #region private
        private void AddAct(IROContent content) { Add(content.Name, content); }
        private void AddAct(string parentName, string name                  ) { Add(name, new Content(ContentDB.Proj.At(parentName), name             )); }
        private void AddAct(string parentName, string name, bool isEffective) { Add(name, new Content(ContentDB.Proj.At(parentName), name, isEffective)); }

        private void Load(string csvFilePath) {
            using (var reader = new du.File.CSVReader<ActivityDesc>(du.App.AppManager.DataPath + csvFilePath, true)) {
                foreach (var desc in reader) {
                    AddAct(desc.Instantiate());
                }
            }
        }
        #endregion
    }

    public interface IProjectDB : du.Cmp.IOrderedMap<IProject> {
        //! 登録済みProject一覧の生成
        void Initialize();
        /// <summary> ProjectをEnumerableで一括取得 </summary>
        IEnumerable<IProject> Sorted(IProject parent);
    }

    /// <summary> 全てのプロジェクトは事前にDBに登録が必要 </summary>
    public class ProjectDB : du.Cmp.OrderedMap<IProject>, IProjectDB {
        #region public
        //! 登録済みProject一覧の生成
        public void Initialize() {
            // 色はプリセットからの選択式
            Load("System/Projects");
            // AddRootProj("pabulum"     , ThemeColors.Red  , true );
            // AddRootProj("MisLead"     , ThemeColors.Blue , true );
            // AddRootProj("Dev"         , ThemeColors.Green, true );
            // AddSubProj("Dev", "Unity");
            // AddRootProj("Unproductive", ThemeColors.Brown, false);
            // AddRootProj("TestSample"  , ThemeColors.Gray , false);
        }
        #endregion

        #region public
        /// <summary> 指定したProjectを直属の親に持つActivityのみを取得 </summary>
        public IEnumerable<IProject> Sorted(IProject parent) {
            return Order
                .Where(key => At(key).Parent == parent)
                .Select(key => At(key));
        }
        #endregion

        #region private
        private void AddProj(IProject proj) { Add(proj.Name, proj); }
        private void AddRootProj(string name, ThemeColor color, bool isEffectiveDefault) {
            AddProj(Project.Create(name, color, isEffectiveDefault));
        }
        private void AddSubProj(string parentName, string name) {
            AddProj(Project.Create(name, At(parentName)));
        }
        private void AddSubProj(string parentName, string name, ThemeColor color) {
            AddProj(Project.Create(name, At(parentName), color));
        }
        private void AddSubProj(string parentName, string name, bool isEffectiveDefault) {
            AddProj(Project.Create(name, At(parentName), isEffectiveDefault));
        }
        private void AddSubProj(string parentName, string name, ThemeColor color, bool isEffectiveDefault) {
            AddProj(new Project(name, At(parentName), color, isEffectiveDefault));
        }

        private void Load(string csvFilePath) {
            using (var reader = new du.File.CSVReader<ProjectDesc>(du.App.AppManager.DataPath + csvFilePath, true)) {
                foreach (var desc in reader) {
                    AddProj(desc.Instantiate());
                }
            }
        }

        // private void Save(string csvFilePath) {
        //     using (var writer = du.File.FWriter.OpenFile4Rewrite(du.App.AppManager.DataPath + csvFilePath + ".csv")) {
        //         writer.Write(Project.CSVLabels);
        //         foreach (var proj in Sorted()) {
        //             writer.Write(proj.ToCSV());
        //         }
        //     }
        // }
        #endregion
    }

}
