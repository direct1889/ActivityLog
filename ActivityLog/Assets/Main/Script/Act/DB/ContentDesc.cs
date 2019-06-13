using UColor = UnityEngine.Color;
using static du.Ex.ExString;

namespace Main.Act.DB {

    /// <summary> CSVを読み込みContent(Proj/Act)に変換 </summary>
    public class ContentDesc {
        #region field
        /// <value> 1:Project / 0:Activity </value>
        [du.File.CSVColAttr(0,0)] public int isProject;
        /// <value>
        /// e.g. ::Dev::Unity::AcTrack
        /// - 空欄ならRoot直下 (Projectのみ許可)
        /// </value>
        [du.File.CSVColAttr(1,null)] public string parentGenealogy;
        /// <value> 名称 </value>
        [du.File.CSVColAttr(2,null)] public string name;
        /// <value> 空欄なら親と同じ </value>
        [du.File.CSVColAttr(3,null)] public string isEffective;
        /// <value> 空欄なら親と同じ </value>
        [du.File.CSVColAttr(4,null)] public string color;
        #endregion

        #region getter
        public override string ToString() {
            return $"{parentGenealogy}::{name}({color},{isEffective})";
        }

        /// <summary> CSVを読み込みContent(Proj/Act)に変換 </summary>
        public IContentAdapter Instantiate() {
            // Root直下
            if (parentGenealogy.IsEmpty()) {
                return new ProjectAdapter(new Project(name, null, ThemeColors.Get(color), bool.Parse(isEffective)));
            }
            else {
                var parent = CDB.Proj.AtProjByGenealogy(parentGenealogy);
                if (isProject != 0) { // Project
                    return new ProjectAdapter(new Project(name, parent,
                    color.IsEmpty() ? parent.Color : ThemeColors.Get(color),
                    isEffective.IsEmpty() ? parent.IsEffective : bool.Parse(isEffective)));
                }
                else { // Activity
                    return new ActivityAdapter(new Activity(parent, name, isEffective.IsEmpty() ? parent.IsEffective : bool.Parse(isEffective)));
                }
            }
        }
        #endregion
    }

    public static class ExContentCSV {
        /// <summary> CSV出力形式に変換 </summary>
        public static string ToCSV(this IContentAdapter content) {
            return content.IsProj ? content.Proj.ToCSV() : content.Act.ToCSV();
        }
        private static string ToCSV(this IProject proj) {
            return "1" + (proj.Parent is null ? "" : proj.Parent.Key) + $",{proj.Name},{proj.IsEffective},{proj.Color}";
        }
        private static string ToCSV(this IActivity content) {
            return $"0,{content.Parent.Key},{content.Name},{content.IsEffective},";
        }
    }

}
