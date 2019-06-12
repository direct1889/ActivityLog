
using System.Collections.Generic;
using UnityEngine;
using UColor = UnityEngine.Color;
using System.Linq;
using static Main.Act.DB.ExContentCSV;
using System;
using UniRx;

namespace Main.Act.DB {

    public static class ContentDB {
        #region field
        static IContentTree m_tree = new ContentTree();
        #endregion

        #region field
        public static IContentTree Tree => m_tree;
        #endregion

        #region public
        public static void Initialize() {
            du.Test.LLog.MBoot.Log("Initialized ContentDB.");
            Tree.Initialize();
        }
        #endregion
    }

    public interface IActivityDB
    // : du.Cmp.IRxOrderedMap<IROContent, string>
    {
        /// <summary> ActivityをEnumerableで一括取得 </summary>
        IEnumerable<IROContent> ActSorted(IProject parent);
        // TODO: OVERLAP
        /// <summary> Activityを新たに登録 </summary>
        void AddAct(IROContent content);
    }

    public interface IProjectDB
    // : du.Cmp.IRxOrderedMap<IProject, string>
    {
        /// <summary> ProjectをEnumerableで一括取得 </summary>
        IEnumerable<IProject> ProjSorted(IProject parent);
        // TODO: OVERLAP
        /// <summary> Projectを新たに登録 </summary>
        void AddProj(IProject project);
    }

}
