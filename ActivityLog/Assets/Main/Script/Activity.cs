

namespace Main.Graph {

	//! 時間の単位はすべて[minute]
	public interface IActivity {

		IProject Project { get; }
		string Name { get; }
		bool IsEffective { get; }
		float BeginTime { get; }
		float? EndTime { get; }

		float Duration { get; }

	}

	public class Activity : IActivity {

		#region property
		public IProject	Project		{ get; private set; }
		public string	Name		{ get; private set; }
		public bool		IsEffective	{ get; private set; }
		public float	BeginTime	{ get; private set; }
		public float?	EndTime		{ get; private set; }
		public float	Duration	{ get { return (float)EndTime - BeginTime; } }
		#endregion

		#region ctor/dtor
		public Activity(string proj, string name) {
			Project = ProjectsDB.At(proj);
			Name = name;
			IsEffective = Project.IsEffectiveDefault;
		}
		public Activity(string proj, string name, float begin) : this(proj, name, begin, null) {}

		public Activity(string proj, string name, float begin, float? end) {
			Project = ProjectsDB.At(proj);
			Name = name;
			IsEffective = Project.IsEffectiveDefault;
			BeginTime = begin;
			EndTime = end;
		}
		#endregion

	}

}
