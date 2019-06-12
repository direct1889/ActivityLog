using UColor = UnityEngine.Color;
using static du.Ex.ExString;

namespace Main.Act.DB {

    public class ContentDesc {
        [du.File.CSVColAttr(0,0)] public int isProject; // 1:Proj / 0:Act
        [du.File.CSVColAttr(1,null)] public string parentGenealogy;
        [du.File.CSVColAttr(2,null)] public string name;
        [du.File.CSVColAttr(3,null)] public string isEffective;
        [du.File.CSVColAttr(4,null)] public string color;
        public override string ToString() {
            return $"{parentGenealogy}::{name}({color},{isEffective})";
        }

        public IContent Instantiate() {
            // Root直下
            if (parentGenealogy.IsEmpty()) {
                return new ProjectProxy(new Project(name, null, ThemeColors.Get(color), bool.Parse(isEffective)));
            }
            else {
                var parent = DB.ContentDB.Tree.AtByKey(parentGenealogy);
                if (isProject != 0) { // Project
                    return new ProjectProxy(new Project(name, parent,
                    color.IsEmpty() ? parent.Color : ThemeColors.Get(color),
                    isEffective.IsEmpty() ? parent.IsEffective : bool.Parse(isEffective)));
                }
                else { // Activity
                    return new ActivityProxy(new Content(parent, name, isEffective.IsEmpty() ? parent.IsEffective : bool.Parse(isEffective)));
                }
            }
        }
    }

    public static class ExContentCSV {
        public static string ToCSV(this IProject proj) {
            return proj.Parent is null ? "" : proj.Parent.Name + $",{proj.Name},{proj.Color},{proj.IsEffective}";
        }
        public static string ToCSV(this IROContent content) {
            return $"{content.Parent.Name},{content.Name},{content.IsEffective}";
        }
    }

}
