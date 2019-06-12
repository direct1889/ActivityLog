using System.Collections.Generic;
using System;
using System.Linq;
using UniRx;
using du.Cmp;

namespace Main.Act {

    /// <summary> ProjectとActivityを同一のインタフェースに </summary>
    public interface IContent : du.Cmp.IHashTreeDataType<IProject, string> {
        IProject Proj { get; }
        IROContent Act { get; }
        bool IsProj { get; }
    }

    public abstract class ContentProxy : IContent {
        public abstract IProject Proj { get; }
        public abstract IROContent Act { get; }
        public abstract bool IsProj { get; }

        public IProject Parent => Data.Parent;
        public string Key => Data.Key;

        protected IROContent Data { get => IsProj ? Proj : Act; }
    }

    public class ProjectProxy : ContentProxy {
        public override IProject Proj { get; }
        public override IROContent Act => null;
        public override bool IsProj => true;

        public ProjectProxy(IProject proj) => Proj = proj;
    }

    public class ActivityProxy : ContentProxy {
        public override IProject Proj => null;
        public override IROContent Act { get; }
        public override bool IsProj => false;

        public ActivityProxy(IROContent act) => Act = act;
    }

    public interface IContentTree : DB.IActivityDB, DB.IProjectDB {
        /// <summary> 存在するか </summary>
        bool IsExistProj(string key, IProject parent);
        bool IsExist(IProject proj);
        bool IsExistAct(string key, IProject parent);
        bool IsExist(IROContent act);

        /// <returns> 見つからない場合、見つかったがProjectじゃない場合は null </returns>
        IProject AtProj(IProject proj);
        IProject AtProj(string key, IProject parent);
        /// <returns> 見つからない場合、見つかったがActivityじゃない場合は null </returns>
        IROContent AtAct(string key, IProject parent);

        /// <summary> 追加 </summary>
        void Add(IProject proj);
        void Add(IROContent act);
    }

    public class ContentTree
    : du.Cmp.RxHashTree<IContent, IProject, string>, IContentTree
    {
        public bool IsExistProj(string key, IProject parent) {
            return At(parent)?.Children.At(key)?.Value.IsProj ?? false;
        }
        public bool IsExist(IProject proj) {
            // 見つからなければどこかでnullが出る
            return At(proj)?.Value.IsProj ?? false;
        }
        public bool IsExistAct(string key, IProject parent) {
            return !(At(parent)?.Children.At(key)?.Value.IsProj) ?? false;
        }
        public bool IsExist(IROContent act) {
            return !(At(act.Parent)?.Children.At(act.Key)?.Value.IsProj) ?? false;
        }
        /// <returns> 見つからない場合、見つかったがProjectじゃない場合は null </returns>
        public IProject AtProj(IProject proj) {
            return AtProj(proj.Key, proj.Parent);
        }
        public IProject AtProj(string key, IProject parent) {
            return At(parent)?.Children.At(key).Value.Proj;
        }
        /// <returns> 見つからない場合、見つかったがActivityじゃない場合は null </returns>
        public IROContent AtAct(string key, IProject parent) {
            return At(parent)?.Children.At(key).Value.Act;
        }
        public void Add(IProject proj) {
            Add(new ProjectProxy(proj));
        }
        public void Add(IROContent act) {
            Add(new ActivityProxy(act));
        }


        // IObservable<IROContent> IRxOrderedMap<IROContent, string>.RxAdded => throw new NotImplementedException();

        // IObservable<IProject> IRxOrderedMap<IProject, string>.RxAdded => throw new NotImplementedException();


        public void ActInitialize() { Load(); }

        public IEnumerable<IROContent> ActSorted(IProject parent) {
            return At(parent).Children             // parentを親に持つ
                .Sorted()                          // 子供を指定した順番で
                .Where(node => !node.Value.IsProj) // Activityのみを
                .Select(node => node.Value.Act);   // IROContentで取得
        }

        public bool ActHasExistOverlapped(string name, IProject parent) {
            return IsExistAct(name, parent);
        }

        public void AddAct(IROContent content) { Add(content); }

        public void ProjInitialize() {
            // Load("System/Projects");
        }

        public IEnumerable<IProject> ProjSorted(IProject parent) {
            return At(parent).Children            // parentを親に持つ
                .Sorted()                         // 子供を指定した順番で
                .Where(node => node.Value.IsProj) // Activityのみを
                .Select(node => node.Value.Proj); // IROContentで取得
        }

        public bool ProjHasExistOverlapped(string name, IProject parent) {
            return IsExistProj(name, parent);
        }

        public void AddProj(IProject project) { Add(project); }


        private void Load() {
            using (var reader = new du.File.CSVReader<DB.ProjectDesc>(du.App.AppManager.DataPath + "System/Projects", true)) {
                foreach (var desc in reader) {
                    AddProj(desc.Instantiate());
                }
            }
            using (var reader = new du.File.CSVReader<DB.ActivityDesc>(du.App.AppManager.DataPath + "System/Activities", true)) {
                foreach (var desc in reader) {
                    AddAct(desc.Instantiate());
                }
            }
        }
    }

}

