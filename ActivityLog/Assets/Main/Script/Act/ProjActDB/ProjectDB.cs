
using System.Collections.Generic;
using UnityEngine;
using UColor = UnityEngine.Color;
using System.Linq;

namespace Main.Act {

    public static class ProjActDB {
        #region property
        public static IProjectDB Proj { get; } = new ProjectDB();
        // public static IActivityDB Act { get; }
        #endregion

        #region public
        public static void Initialize() {
            Proj.Initialize();
        }
        #endregion
    }

    public interface IProjectDB {
        //! 登録済みProject一覧の生成
        void Initialize();

        /// <summary> Projectを名前から引く </summary>
        /// <returns> 見つからなければ null </returns>
        IProject At(string name);

        /// <summary> ProjectをEnumerableで一括取得 </summary>
        IEnumerable<IProject> Projects();

        // #region private
        // void AddProj(IProject proj);
        // void AddProj(string name, UColor color, bool isEffectiveDefault);
        // void AddSubProj(string parentName, string name);
        // void AddSubProj(string parentName, string name, UColor color);
        // void AddSubProj(string parentName, string name, UColor color, bool isEffectiveDefault);
        // #endregion
    }

    /// <summary> 全てのプロジェクトは事前にDBに登録が必要 </summary>
    public class ProjectDB : IProjectDB {
        #region field
        IList<string> s_projOrder = null;
        IDictionary<string, IProject> s_projects = null;
        #endregion

        #region public
        //! 登録済みProject一覧の生成
        public void Initialize() {
            if (s_projects == null) {
                s_projOrder = new List<string>();
                s_projects = new Dictionary<string, IProject>();

                // 色はプリセットからの選択式
                AddProj("pabulum"     , ThemeColors.Red  , true );
                AddProj("MisLead"     , ThemeColors.Blue , true );
                AddProj("Dev"         , ThemeColors.Green, true );
                AddSubProj("Dev", "Unity");
                AddProj("Unproductive", ThemeColors.Brown, false);
                AddProj("TestSample"  , ThemeColors.Gray , false);

                du.Test.LLog.MBoot.Log("Initialized ProjectsDB.");
            }
            else { du.Test.LLog.MBoot.Log("ProjectsDB has already initialized."); }
        }

        /// <summary> Projectを名前から引く </summary>
        /// <returns> 見つからなければ null </returns>
        public IProject At(string name) {
            if (s_projects.ContainsKey(name)) {
                return s_projects[name];
            }
            else { return null; }
        }

        /// <summary> ProjectをEnumerableで一括取得 </summary>
        public IEnumerable<IProject> Projects() {
            return s_projOrder.Select(name => s_projects[name]);
        }
        #endregion

        #region private
        private void AddProj(IProject proj) {
            s_projOrder.Add(proj.Name);
            s_projects.Add(proj.Name, proj);
        }
        private void AddProj(string name, UColor color, bool isEffectiveDefault) {
            AddProj(new Project(name, color, isEffectiveDefault));
        }
        private void AddSubProj(string parentName, string name) {
            AddProj(new SubProject(At(parentName), name));
        }
        private void AddSubProj(string parentName, string name, UColor color) {
            AddProj(new SubProject(At(parentName), name, color));
        }
        private void AddSubProj(string parentName, string name, UColor color, bool isEffectiveDefault) {
            AddProj(new SubProject(At(parentName), name, color, isEffectiveDefault));
        }
        #endregion
    }

}
