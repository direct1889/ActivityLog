
using System.Collections.Generic;
using UnityEngine;
using UColor = UnityEngine.Color;
using System.Linq;

namespace Main.Act {

    /// <summary> 全てのプロジェクトは事前にDBに登録が必要 </summary>
    public static class ProjectDB {
        #region static field
        static IList<string> s_projOrder = null;
        static IDictionary<string, IProject> s_projects = null;
        #endregion

        #region static public
        //! 登録済みProject一覧の生成
        public static void Initialize() {
            if (s_projects == null) {
                s_projOrder = new List<string>();
                s_projects = new Dictionary<string, IProject>();

                // 色はプリセットからの選択式
                AddProj("pabulum"     , ThemeColors.Red  , true );
                AddProj("MisLead"     , ThemeColors.Blue , true );
                AddProj("Dev"         , ThemeColors.Green, true );
                AddProj("Unproductive", ThemeColors.Brown, false);
                AddProj("TestSample"  , ThemeColors.Gray , false);

                // AddProj(new SubProject(At("Dev"), "Unity"));

                du.Test.LLog.MBoot.Log("Initialized ProjectsDB.");
            }
            else { du.Test.LLog.MBoot.Log("ProjectsDB has already initialized."); }
        }

        /// <summary> Projectを名前から引く </summary>
        /// <returns> 見つからなければ null </returns>
        public static IProject At(string name) {
            if (s_projects.ContainsKey(name)) {
                return s_projects[name];
            }
            else { return null; }
        }

        /// <summary> ProjectをEnumerableで一括取得 </summary>
        public static IEnumerable<IProject> Projects() {
            return s_projOrder.Select(name => s_projects[name]);
        }
        #endregion

        #region private
        private static void AddProj(IProject proj) {
            s_projOrder.Add(proj.Name);
            s_projects.Add(proj.Name, proj);
        }
        private static void AddProj(string name, UColor color, bool isEffectiveDefault) {
            AddProj(new Project(name, color, isEffectiveDefault));
        }
        #endregion
    }

}
