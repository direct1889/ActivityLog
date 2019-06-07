using UColor = UnityEngine.Color;
using static du.Ex.ExString;

namespace Main.Act.DB {

    // CSVからの読み込みに対応
    public class ProjectDesc {
        [du.File.CSVColAttr(0,null)] public string parentName;
        [du.File.CSVColAttr(1,null)] public string name;
        [du.File.CSVColAttr(2,null)] public string color;
        [du.File.CSVColAttr(3,null)] public string isEffective;
        public override string ToString() {
            return $"{parentName}::{name}({color},{isEffective})";
        }

        public IProject Instantiate() {
            if (parentName.IsEmpty()) {
                return new Project(name, null, ThemeColors.Get(color), bool.Parse(isEffective));
            }
            else {
                IProject parent = ContentDB.Proj.At(parentName);
                return new Project(name, parent,
                    color.IsEmpty() ? parent.Color : ThemeColors.Get(color),
                    isEffective.IsEmpty() ? parent.IsEffective : bool.Parse(isEffective));
            }
        }
    }

    // CSVからの読み込みに対応
    public class ActivityDesc {
        [du.File.CSVColAttr(0,null)] public string parentName;
        [du.File.CSVColAttr(1,null)] public string name;
        [du.File.CSVColAttr(2,null)] public string isEffective;
        public override string ToString() {
            return $"{parentName}::{name}({isEffective})";
        }

        public IROContent Instantiate() {
            IProject parent = ContentDB.Proj.At(parentName);
            return new Content(parent, name, isEffective.IsEmpty() ? parent.IsEffective : bool.Parse(isEffective));
        }
    }

    public static class ExProject {
        public static string ToCSV(this IProject proj) {
            return proj.Parent is null ? "" : proj.Parent.Name + $",{proj.Name},{proj.Color},{proj.IsEffective}";
        }
        public static string ToCSV(this IROContent content) {
            return $"{content.Parent.Name},{content.Name},{content.IsEffective}";
        }
    }

}
