using System.Collections.Generic;
using System;
using System.Linq;
using UniRx;
using du.Cmp;
using System.Text.RegularExpressions;

namespace Main.Act {

    /// <summary> ProjectとActivityを同一のインタフェースに </summary>
    public interface IContentAdapter : du.Cmp.IHashTreeDataType<IProject, string> {
        IProject Proj { get; }
        IActivity Act { get; }

        bool IsProj { get; }
        IContent Data { get; }
    }

    public abstract class ContentAdapter : IContentAdapter {
        public abstract IProject Proj { get; }
        public abstract IActivity Act { get; }
        public abstract bool IsProj { get; }

        public IProject Parent => Data.Parent;
        public string Key => Data.Key;

        public IContent Data => IsProj ? Proj as IContent : Act as IContent;
    }

    public class ProjectAdapter : ContentAdapter {
        public override IProject Proj { get; }
        public override IActivity Act => null;
        public override bool IsProj => true;

        /// <param name="proj"> nullだと例外を投げる </param>
        public ProjectAdapter(IProject proj) {
            if (proj is null) { throw new ArgumentNullException(); }
            Proj = proj;
        }
    }

    public class ActivityAdapter : ContentAdapter {
        public override IProject Proj => null;
        public override IActivity Act { get; }
        public override bool IsProj => false;

        /// <param name="act"> nullだと例外を投げる </param>
        public ActivityAdapter(IActivity act) {
            if (act is null) { throw new ArgumentNullException(); }
            Act = act;
        }
    }

}

