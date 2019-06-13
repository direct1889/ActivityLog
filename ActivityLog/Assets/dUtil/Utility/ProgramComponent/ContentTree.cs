using System.Collections.Generic;
using System;
using System.Linq;
using UniRx;
using du.Cmp;
using System.Text.RegularExpressions;

namespace Main.Act {

    /// <summary> ProjectとActivityを同一のインタフェースに </summary>
    public interface IContentProxy : du.Cmp.IHashTreeDataType<IProject, string> {
        IProject Proj { get; }
        IActivity Act { get; }

        bool IsProj { get; }
        IContent Data { get; }
    }

    public abstract class ContentProxy : IContentProxy {
        public abstract IProject Proj { get; }
        public abstract IActivity Act { get; }
        public abstract bool IsProj { get; }

        public IProject Parent => Data.Parent;
        public string Key => Data.Key;

        public IContent Data { get
            {
                if (IsProj) { return Proj; }
                else { return Act; }
            }
        }
    }

    public class ProjectProxy : ContentProxy {
        public override IProject Proj { get; }
        public override IActivity Act => null;
        public override bool IsProj => true;

        public ProjectProxy(IProject proj) {
            if (proj is null) { throw new ArgumentNullException(); }
            Proj = proj;
        }
    }

    public class RootProjectProxy : ContentProxy {
        public override IProject   Proj   => null;
        public override IActivity Act    => null;
        public override bool       IsProj => true;

        private RootProjectProxy() {}
        static RootProjectProxy m_instance = new RootProjectProxy();
        public static IContentProxy Instance => m_instance;
    }

    public class ActivityProxy : ContentProxy {
        public override IProject Proj => null;
        public override IActivity Act { get; }
        public override bool IsProj => false;

        public ActivityProxy(IActivity act) {
            if (act is null) { throw new ArgumentNullException(); }
            Act = act;
        }
    }

    public interface IContentTree
    : du.Cmp.IRxHashTree<IContentProxy, IProject, string>, DB.IActivityDB, DB.IProjectDB
    {
        #region getter
        /// <summary> 既存プロジェクトと重複するか </summary>
        bool ProjHasExist(string key, IProject parent);
        bool ProjHasExist(IProject proj);
        /// <summary> 既存アクティビティと重複するか </summary>
        bool ActHasExist(string key, IProject parent);
        bool ActHasExist(IActivity act);
        #endregion

        #region getter
        IProject AtByKey(string key);
        /// <returns> 見つからない場合、見つかったがProjectじゃない場合は null </returns>
        IProject AtProj(IProject proj);
        IProject AtProj(string key, IProject parent);
        /// <returns> 見つからない場合、見つかったがActivityじゃない場合は null </returns>
        IActivity AtAct(string key, IProject parent);

        IEnumerable<IContentProxy> Sorted(IProject parent);
        #endregion

        #region public
        /// <summary> 登録済みActivity一覧の生成 </summary>
        void Initialize();
        /// <summary> 追加 </summary>
        void Add(IProject proj);
        void Add(IActivity act);
        #endregion
    }

    public class ContentTree
    : du.Cmp.RxHashTree<IContentProxy, IProject, string>, IContentTree
    {
        public bool ProjHasExist(string key, IProject parent) {
            return At(parent)?.Children.At(key)?.Value.IsProj ?? false;
        }
        public bool ProjHasExist(IProject proj) {
            // 見つからなければどこかでnullが出る
            return At(proj)?.Value.IsProj ?? false;
        }
        public bool ActHasExist(string key, IProject parent) {
            return !(At(parent)?.Children.At(key)?.Value.IsProj) ?? false;
        }
        public bool ActHasExist(IActivity act) {
            return !(At(act.Parent)?.Children.At(act.Key)?.Value.IsProj) ?? false;
        }

        /// <returns> 見つからない場合、見つかったがProjectじゃない場合は null </returns>
        public IProject AtByKey(string key) {
            return FromGenealogy(key);
        }
        /// <returns> 見つからない場合、見つかったがProjectじゃない場合は null </returns>
        public IProject AtProj(IProject proj) {
            return AtProj(proj.Key, proj.Parent);
        }
        public IProject AtProj(string key, IProject parent) {
            return At(parent)?.Children.At(key).Value.Proj;
        }
        /// <returns> 見つからない場合、見つかったがActivityじゃない場合は null </returns>
        public IActivity AtAct(string key, IProject parent) {
            return At(parent)?.Children.At(key).Value.Act;
        }
        public void Add(IProject proj) {
            Add(new ProjectProxy(proj));
        }
        public void Add(IActivity act) {
            Add(new ActivityProxy(act));
        }


        public IEnumerable<IContentProxy> Sorted(IProject parent) {
            return At(parent).Children            // parentを親に持つ
                .OrderedValues()                         // 子供を指定した順番で
                .Select(node => node.Value); // IROContentで取得
        }


        // IObservable<IROContent> IRxOrderedMap<IROContent, string>.RxAdded => throw new NotImplementedException();

        // IObservable<IProject> IRxOrderedMap<IProject, string>.RxAdded => throw new NotImplementedException();

        public void Initialize() { Load(); }


        public IEnumerable<IActivity> ActSorted(IProject parent) {
            return At(parent).Children             // parentを親に持つ
                .OrderedValues()                          // 子供を指定した順番で
                .Where(node => !node.Value.IsProj) // Activityのみを
                .Select(node => node.Value.Act);   // IROContentで取得
        }

        public void AddAct(IActivity content) { Add(content); }

        public IEnumerable<IProject> ProjSorted(IProject parent) {
            return At(parent).Children            // parentを親に持つ
                .OrderedValues()                         // 子供を指定した順番で
                .Where(node => node.Value.IsProj) // Activityのみを
                .Select(node => node.Value.Proj); // IROContentで取得
        }


        public void AddProj(IProject project) { Add(project); }


        private void Load() {
            using (var reader = new du.File.CSVReader<DB.ContentDesc>(du.App.AppManager.DataPath + "System/Contents", true)) {
                foreach (var desc in reader) {
                    Add(desc.Instantiate());
                    // AddProj(desc.Instantiate());
                }
            }
            // using (var reader = new du.File.CSVReader<DB.ProjectDesc>(du.App.AppManager.DataPath + "System/Projects", true)) {
            //     foreach (var desc in reader) {
            //         AddProj(desc.Instantiate());
            //     }
            // }
            // using (var reader = new du.File.CSVReader<DB.ActivityDesc>(du.App.AppManager.DataPath + "System/Activities", true)) {
            //     foreach (var desc in reader) {
            //         AddAct(desc.Instantiate());
            //     }
            // }
        }

        static Regex Genealogy { get; } = new Regex("(::([^:]+))+");
        /// <returns> 見つからない場合、見つかったがProjectじゃない場合は null </returns>
        private IProject FromGenealogy(string genealogy) {
            var matched = Genealogy.Match(genealogy);
            var it = Root;
            for (int i = 0; i < matched.Groups[2].Captures.Count && !(it is null); ++i) {
                it = it.Children.At(matched.Groups[2].Captures[i].Value);
            }
            return it?.Value?.Proj;
        }

    }

}

