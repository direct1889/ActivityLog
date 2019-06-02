
using System.Collections.Generic;
using UnityEngine;
using UColor = UnityEngine.Color;
using System.Linq;

namespace Main.Act {

    /// <summary> プロジェクト:タスクグループ </summary>
    public interface IProject {
        #region property
        /// <summary> 名称 </summary>
        string Name { get; }
        /// <summary> テーマカラー </summary>
        UColor Color { get; }
        /// <summary> 所属タスクが原則エフェクティブか </summary>
        bool IsEffectiveDefault { get; }
        #endregion
    }

    /// <summary> プロジェクト:タスクグループ </summary>
    public class Project : IProject {

        #region field-property
        public string Name { get; }
        public UColor Color { get; }
        public bool IsEffectiveDefault { get; }
        #endregion

        #region ctor/dtor
        public Project(string name, UColor color, bool isEffectiveDefault) {
            Name = name;
            Color = color;
            IsEffectiveDefault = isEffectiveDefault;
        }
        #endregion

    }

    /// <summary> サブプロジェクト:親プロジェクトを持つ </summary>
    public class SubProject : Project {

        #region field-property
        public IProject Parent { get; }
        #endregion

        #region ctor/dtor
        /// <param name="color"> 省略すると親から引き継ぐ </param>
        /// <param name="isEffectiveDefault"> 省略すると親から引き継ぐ </param>
        public SubProject(IProject parent, string name, UColor color, bool isEffectiveDefault) : base(name, color, isEffectiveDefault) {
            Parent = parent;
        }
        public SubProject(IProject parent, string name, bool isEffectiveDefault) : base(name, parent.Color, isEffectiveDefault) {
            Parent = parent;
        }
        public SubProject(IProject parent, string name, UColor color) : base(name, color, parent.IsEffectiveDefault) {
            Parent = parent;
        }
        public SubProject(IProject parent, string name) : base(name, parent.Color, parent.IsEffectiveDefault) {
            Parent = parent;
        }
        #endregion

    }

    /// <summary> 全てのプロジェクトは事前にDBに登録が必要 </summary>
    public static class ProjectDB {

        static IDictionary<string, IProject> s_projects;

        #region static public
        //! 登録済みProject一覧の生成
        public static void Initialize() {
            if (s_projects == null) {
                s_projects = new Dictionary<string, IProject>();

                AddProj("pabulum", new UColor(1f, 0.9f, 0.7f), true);
                AddProj("MisLead", new UColor(0.7f, 0.7f, 1f), true);
                AddProj("Dev", new UColor(0.7f, 1f, 0.8f), true);
                AddProj("Unproductive", new UColor(0.5f, 0.2f, 0.2f), false);
                AddProj("TestSample", new UColor(0.8f, 0.8f, 0.8f), false);

                AddProj(new SubProject(At("Dev"), "Unity"));

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
        #endregion

        #region private
        private static void AddProj(IProject proj) {
            s_projects.Add(proj.Name, proj);
        }
        private static void AddProj(string name, UColor color, bool isEffectiveDefault) {
            AddProj(new Project(name, color, isEffectiveDefault));
        }
        #endregion

    }

}
