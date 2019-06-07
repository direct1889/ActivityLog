
using System.Collections.Generic;
using UnityEngine;
using UColor = UnityEngine.Color;
using System.Linq;

namespace Main.Act {

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

    /// <summary>
    /// 順序付き連想配列
    /// Dicと別にキーのListを持ってるだけ
    /// </summary>
    public interface IOrderedDB<T> where T : class {
        /// <returns> 見つからなければ null </returns>
        T At(int i);
        /// <returns> 見つからなければ null </returns>
        T At(string key);
        /// <summary> ActivityをEnumerableで一括取得 </summary>
        IEnumerable<T> Sorted();
    }

    public class OrderedDB<T> : IOrderedDB<T> where T : class {
        #region field
        IList<string> m_order = null;
        IDictionary<string, T> m_data = null;
        #endregion

        #region protected property
        protected IList<string> Order { get { return m_order; } }
        protected IDictionary<string, T> Data { get { return m_data; } }
        #endregion

        #region ctor/dtor
        public OrderedDB() {
            m_order = new List<string>();
            m_data = new Dictionary<string, T>();
        }
        #endregion

        #region public
        /// <returns> 見つからなければ null </returns>
        public T At(int i) {
            if (0 <= i && i < m_data.Count) { return m_data[m_order[i]]; }
            else { return null; }
        }
        /// <returns> 見つからなければ null </returns>
        public T At(string key) {
            if (m_data.ContainsKey(key)) { return m_data[key]; }
            else { return null; }
        }
        /// <summary> ActivityをEnumerableで一括取得 </summary>
        public IEnumerable<T> Sorted() {
            return m_order.Select(name => m_data[name]);
        }
        #endregion

        #region protected
        protected void Add(string key, T value) {
            m_order.Add(key);
            m_data.Add(key, value);
        }
        #endregion
    }

    public interface IActivityDB : IOrderedDB<IROContent> {
        //! 登録済みActivity一覧の生成
        void Initialize();
        /// <summary> ActivityをEnumerableで一括取得 </summary>
        IEnumerable<IROContent> Sorted(IProject parent);
    }

    /// <summary> 全てのプロジェクトは事前にDBに登録が必要 </summary>
    public class ActivityDB : OrderedDB<IROContent>, IActivityDB {
        #region public
        //! 登録済みActivity一覧の生成
        public void Initialize() {
            // 色はプリセットからの選択式
            AddAct("pabulum"     , "安達としまむら");
            AddAct("pabulum"     , "ユーフォ");
            AddAct("MisLead"     , "面接練習");
            // AddAct("Dev"         , "");
            AddAct("Unity"       , "ActivityLog");
            AddAct("Unproductive", "Twitter");
            AddAct("TestSample"  , "Hoge");
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
        #endregion
    }

    public interface IProjectDB : IOrderedDB<IProject> {
        //! 登録済みProject一覧の生成
        void Initialize();
        /// <summary> ProjectをEnumerableで一括取得 </summary>
        IEnumerable<IProject> Sorted(IProject parent);
    }

    /// <summary> 全てのプロジェクトは事前にDBに登録が必要 </summary>
    public class ProjectDB : OrderedDB<IProject>, IProjectDB {
        #region public
        //! 登録済みProject一覧の生成
        public void Initialize() {
            // 色はプリセットからの選択式
            AddRootProj("pabulum"     , ThemeColors.Red  , true );
            AddRootProj("MisLead"     , ThemeColors.Blue , true );
            AddRootProj("Dev"         , ThemeColors.Green, true );
            AddSubProj("Dev", "Unity");
            AddRootProj("Unproductive", ThemeColors.Brown, false);
            AddRootProj("TestSample"  , ThemeColors.Gray , false);
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
        private void AddRootProj(                  string name, UColor color, bool isEffectiveDefault) { AddProj(Project.Create(name,                 color, isEffectiveDefault)); }
        private void AddSubProj(string parentName, string name                                       ) { AddProj(Project.Create(name, At(parentName)                           )); }
        private void AddSubProj(string parentName, string name, UColor color                         ) { AddProj(Project.Create(name, At(parentName), color)); }
        private void AddSubProj(string parentName, string name,               bool isEffectiveDefault) { AddProj(Project.Create(name, At(parentName),        isEffectiveDefault)); }
        private void AddSubProj(string parentName, string name, UColor color, bool isEffectiveDefault) { AddProj(new Project(name, At(parentName), color, isEffectiveDefault)); }
        #endregion
    }

}
