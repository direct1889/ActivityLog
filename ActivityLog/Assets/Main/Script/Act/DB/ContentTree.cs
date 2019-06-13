using System.Collections.Generic;
using System;
using System.Linq;
using UniRx;
using du.Cmp;
using System.Text.RegularExpressions;

namespace Main.Act.DB {


    public class ContentTree
    : du.Cmp.RxHashTree<IContentAdapter, IProject, string>, IContentDB
    {
        #region IProjectDB
        public IProject AtProj(IProject proj) => AtProj(proj.Key, proj.Parent);
        public IProject AtProj(string key, IProject parent) {
            return At(parent)?.Children.At(key).Value.Proj;
        }
        public IProject AtProjByGenealogy(string genealogy) {
            return FromGenealogy(genealogy).Proj;
        }

        public bool ProjHasExist(string key, IProject parent) {
            return At(parent)?.Children.At(key)?.Value.IsProj ?? false;
        }
        public bool ProjHasExist(IProject proj) {
            // 見つからなければどこかでnullが出る
            return At(proj)?.Value.IsProj ?? false;
        }

        public IEnumerable<IProject> OrderedProjects(IProject parent) {
            return At(parent).Children            // parentを親に持つ
                .OrderedValues()                  // 子供を指定した順番で
                .Where(node => node.Value.IsProj) // Activityのみを
                .Select(node => node.Value.Proj); // IROContentで取得
        }

        public void Add(IProject proj) => Add(new ProjectAdapter(proj));
        #endregion

        #region IActivityDB
        public IActivity AtAct(IActivity act) => AtAct(act.Key, act.Parent);
        public IActivity AtAct(string key, IProject parent) {
            return At(parent)?.Children.At(key).Value.Act;
        }
        public IActivity AtActByGenealogy(string genealogy) {
            return FromGenealogy(genealogy).Act;
        }

        public bool ActHasExist(string key, IProject parent) {
            return !(At(parent)?.Children.At(key)?.Value.IsProj) ?? false;
        }
        public bool ActHasExist(IActivity act) {
            return !(At(act.Parent)?.Children.At(act.Key)?.Value.IsProj) ?? false;
        }

        public IEnumerable<IActivity> OrderedActivities(IProject parent) {
            return At(parent).Children             // parentを親に持つ
                .OrderedValues()                          // 子供を指定した順番で
                .Where(node => !node.Value.IsProj) // Activityのみを
                .Select(node => node.Value.Act);   // IROContentで取得
        }

        public void Add(IActivity act) => Add(new ActivityAdapter(act));
        #endregion

        #region IContentDB
        public IEnumerable<IContentAdapter> OrderedValues(IProject parent) {
            return At(parent).Children       // parentを親に持つ
                .OrderedValues()             // 子供を指定した順番で
                .Select(node => node.Value); // IContentAdapterで取得
        }

        public void Initialize() { Load(); }
        #endregion

        #region private
        private void Load() {
            du.Test.LLog.MBoot.Log("Tree load.");
            using (var reader = new du.File.CSVReader<DB.ContentDesc>(du.App.AppManager.DataPath + "System/Contents", true)) {
                foreach (var desc in reader) {
                    Add(desc.Instantiate());
                }
            }
        }

        static Regex Genealogy { get; } = new Regex("(::([^:]+))+");
        /// <returns> 見つからない場合、見つかったがProjectじゃない場合は null </returns>
        private IContentAdapter FromGenealogy(string genealogy) {
            var matched = Genealogy.Match(genealogy);
            var it = Root;
            for (int i = 0; i < matched.Groups[2].Captures.Count && !(it is null); ++i) {
                it = it.Children.At(matched.Groups[2].Captures[i].Value);
            }
            return it?.Value;
        }
        #endregion

    }

}

