
using System.Collections.Generic;
using UnityEngine;
using UColor = UnityEngine.Color;
using System.Linq;

namespace Main.Act {

    /// <summary> プロジェクト:タスクグループ </summary>
    public interface IProject : IROContent {}

    /// <summary> プロジェクト:タスクグループ </summary>
    public class Project : du.Cmp.EqualsComparable<Project>, IProject {
        #region field property
        public string Name { get; }
        public ThemeColor Color { get; }
        public bool IsEffective { get; }
        /// <value> 親を持たない場合null </value>
        public virtual IProject Parent { get; }
        #endregion

        #region getter property
        public int ParentCount { get { return (Parent is null ? 0 : Parent.ParentCount + 1); } }
        #endregion

        #region ctor/dtor
        public Project(string name, IProject parent, ThemeColor color, bool isEffective) {
            Name = name;
            Parent = parent;
            Color = color;
            IsEffective = isEffective;
        }
        /// <param name="color"> 省略すると親から引き継ぐ </param>
        /// <param name="isEffective"> 省略すると親から引き継ぐ </param>
        public static IProject Create(string name, IProject parent, ThemeColor color) {
            return parent is null ? null : new Project(name, parent, color, parent.IsEffective);
        }
        public static IProject Create(string name, IProject parent, bool isEffective) {
            return parent is null ? null : new Project(name, parent, parent.Color, isEffective);
        }
        public static IProject Create(string name, IProject parent) {
            return parent is null ? null : new Project(name, parent, parent.Color, parent.IsEffective);
        }
        /// <summary> 親を持たないRootProjectの場合、color/isEffectiveの省略不可 </summary>
        public static IProject Create(string name, ThemeColor color, bool isEffective) {
            return new Project(name, null, color, isEffective);
        }
        #endregion

        #region override
        // public override bool Equals(object obj) {
            // if (obj == null || GetType() != obj.GetType()) { return false; }
            // return GetHashCode() == ((Project)obj).GetHashCode();
        // }
        public override int GetHashCode() { return ToString().GetHashCode(); }
        public override string ToString() { return Parent?.ToString() ?? "" + $"::{Name}"; }
        #endregion

        // #region operator
        // public static bool operator== (Project m, Project n) { return m.Equals(n); }
        // public static bool operator!= (Project m, Project n) { return m.Equals(n); }
        // #endregion

        #region static
        public static string CSVLabels { get { return "ParentName,Name,Color,IsEffective"; } }
        #endregion
    }

}
