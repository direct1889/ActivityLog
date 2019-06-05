
using System.Collections.Generic;
using UnityEngine;
using UColor = UnityEngine.Color;
using System.Linq;

namespace Main.Act {

    /// <summary> プロジェクト:タスクグループ </summary>
    public interface IProject : IROContent {
        #region property
        /// <summary> 名称 </summary>
        // string Name { get; }
        /// <summary> テーマカラー </summary>
        UColor Color { get; }
        /// <summary> 所属タスクが原則エフェクティブか </summary>
        // bool IsEffective { get; }
        /// <value>
        /// 親プロジェクト
        /// 最上位プロジェクトの場合 null
        /// </value>
        // IProject Parent { get; }
        /// <value>
        /// 親の数
        /// 最上位プロジェクトの場合 0
        /// </value>
        int ParentCount { get; }
        #endregion
    }

    /// <summary> プロジェクト:タスクグループ </summary>
    public class Project : IProject {
        #region field-property
        public string Name { get; }
        public UColor Color { get; }
        public bool IsEffective { get; }
        public virtual IProject Parent { get { return null; } }
        public virtual int ParentCount { get { return 0; } }
        #endregion

        #region ctor/dtor
        public Project(string name, UColor color, bool isEffectiveDefault) {
            Name = name;
            Color = color;
            IsEffective = isEffectiveDefault;
        }
        #endregion

        #region getter
        public override string ToString() {
            return $"[{Name},{Color},{IsEffective}]";
        }
        #endregion
    }

    /// <summary> サブプロジェクト:親プロジェクトを持つ </summary>
    public class SubProject : Project {
        #region field-property
        public override IProject Parent { get; }
        public override int ParentCount { get { return Parent.ParentCount + 1; } }
        #endregion

        #region ctor/dtor
        /// <param name="color"> 省略すると親から引き継ぐ </param>
        /// <param name="isEffectiveDefault"> 省略すると親から引き継ぐ </param>
        public SubProject(IProject parent, string name, UColor color, bool isEffectiveDefault) : base(name, color, isEffectiveDefault) {
            Parent = parent;
        }
        public SubProject(IProject parent, string name, bool isEffectiveDefault) : base(name, parent.Color, isEffectiveDefault) {
            Parent = parent;
        }
        public SubProject(IProject parent, string name, UColor color) : base(name, color, parent.IsEffective) {
            Parent = parent;
        }
        public SubProject(IProject parent, string name) : base(name, parent.Color, parent.IsEffective) {
            Parent = parent;
        }
        #endregion
    }

}
