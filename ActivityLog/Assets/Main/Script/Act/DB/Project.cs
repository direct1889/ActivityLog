
namespace Main.Act {

    /// <summary> プロジェクト:タスクグループ </summary>
    public interface IProject : IContent {}

    /// <summary> プロジェクト:タスクグループ </summary>
    public class Project : du.Cmp.EqualsComparable<Project>, IProject {
        #region field property
        public string Name { get; }
        public ThemeColor Color { get; }
        public bool IsEffective { get; }
        /// <value> 親を持たない場合null </value>
        public IProject Parent { get; }
        #endregion

        #region getter property
        public int ParentCount => (Parent is null ? 0 : Parent.ParentCount + 1);
        public string Key => Name;
        #endregion

        #region ctor/dtor
        public Project(string name, IProject parent, ThemeColor color, bool isEffective) {
            Name = name;
            Parent = parent;
            Color = color;
            IsEffective = isEffective;
        }
        #if false
        /// <param name="color"> 省略すると親から引き継ぐ </param>
        /// <param name="isEffective"> 省略すると親から引き継ぐ </param>
        public static IProject Create(string name, IProject parent, ThemeColor color)
            => parent is null ? null : new Project(name, parent, color, parent.IsEffective);
        public static IProject Create(string name, IProject parent, bool isEffective)
            => parent is null ? null : new Project(name, parent, parent.Color, isEffective);
        public static IProject Create(string name, IProject parent)
            => parent is null ? null : new Project(name, parent, parent.Color, parent.IsEffective);
        /// <summary> 親を持たないRootProjectの場合、color/isEffectiveの省略不可 </summary>
        public static IProject Create(string name, ThemeColor color, bool isEffective)
            => new Project(name, null, color, isEffective);
        #endif
        #endregion

        #region override
        public override int GetHashCode() => ToString().GetHashCode();
        public override string ToString() => Parent?.ToString() ?? "" + $"::{Name}";
        #endregion

        #region static
        /// <value> CSVファイルのラベル行 </value>
        public static string CSVLabels => "IsProject,ParentGenealogy,Name,IsEffective,Color";
        #endregion
    }

}
