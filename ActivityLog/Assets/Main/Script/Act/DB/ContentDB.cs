
using System.Collections.Generic;
using UnityEngine;
using UColor = UnityEngine.Color;
using System.Linq;
using static Main.Act.DB.ExContentCSV;
using System;
using UniRx;

namespace Main.Act {

    public static class CDB {
        #region field
        static DB.IContentDB m_content = new DB.ContentTree();
        #endregion

        #region field
        public static DB.IActivityDB Act => m_content;
        public static DB.IProjectDB Proj => m_content;
        public static DB.IContentDB Content => m_content;
        #endregion

        #region public
        public static void Initialize() {
            du.Test.LLog.MBoot.Log("Initialized ContentDB.");
            m_content.Initialize();
        }
        #endregion
    }

}

namespace Main.Act.DB {

    public interface IProjectDB
    // : du.Cmp.IRxOrderedMap<IProject, string>
    {
        #region getter
        /// <returns> 見つからない場合、見つかったがProjectじゃない場合は null </returns>
        IProject AtProj(IProject proj);
        /// <returns> 見つからない場合、見つかったがProjectじゃない場合は null </returns>
        IProject AtProj(string key, IProject parent);
        /// <returns> 見つからない場合、見つかったがProjectじゃない場合は null </returns>
        IProject AtProjByGenealogy(string genealogy);

        /// <summary> 既存プロジェクトと重複するか </summary>
        bool ProjHasExist(string key, IProject parent);
        /// <summary> 既存プロジェクトと重複するか </summary>
        bool ProjHasExist(IProject proj);

        /// <summary> ProjectをEnumerableで一括取得 </summary>
        IEnumerable<IProject> OrderedProjects(IProject parent);
        #endregion

        /// <summary> Projectを新たに登録 </summary>
        void Add(IProject proj);
    }

    public interface IActivityDB
    // : du.Cmp.IRxOrderedMap<IROContent, string>
    {
        #region getter
        /// <returns> 見つからない場合、見つかったがProjectじゃない場合は null </returns>
        IActivity AtAct(IActivity act);
        /// <returns> 見つからない場合、見つかったがActivityじゃない場合は null </returns>
        IActivity AtAct(string key, IProject parent);
        /// <returns> 見つからない場合、見つかったがActivityじゃない場合は null </returns>
        IActivity AtActByGenealogy(string genealogy);

        /// <summary> 既存アクティビティと重複するか </summary>
        bool ActHasExist(string key, IProject parent);
        /// <summary> 既存アクティビティと重複するか </summary>
        bool ActHasExist(IActivity act);
        #endregion

        /// <summary> ActivityをEnumerableで一括取得 </summary>
        IEnumerable<IActivity> OrderedActivities(IProject parent);
        // TODO: OVERLAP
        /// <summary> Activityを新たに登録 </summary>
        void Add(IActivity act);
    }


    public interface IContentDB
    : du.Cmp.IRxHashTree<IContentAdapter, IProject, string>, IActivityDB, IProjectDB
    {
        #region getter
        IEnumerable<IContentAdapter> OrderedValues(IProject parent);
        #endregion

        #region public
        /// <summary> 登録済みActivity一覧の生成 </summary>
        void Initialize();
        #endregion
    }

}