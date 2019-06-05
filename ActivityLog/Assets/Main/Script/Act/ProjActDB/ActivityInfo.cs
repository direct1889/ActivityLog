
using System.Collections.Generic;
using UnityEngine;
using UColor = UnityEngine.Color;
using System.Linq;

namespace Main.Act {

    /// <summary> アクティビティのプリセット </summary>
    public interface IActivityInfo : IROContent {}

    public class ContentAsActInfo : IProject {

        IROContent m_content;

        public string Name { get { return m_content.Name; } }
        public UColor Color { get { return m_content.Parent.Color; } }
        public bool IsEffective { get { return m_content.IsEffective; } }
        public IProject Parent { get { return m_content.Parent; } }
        public int ParentCount { get { return m_content.Parent.ParentCount + 1; } }

        public ContentAsActInfo(IROContent cnt) { m_content = cnt; }
    }

}
