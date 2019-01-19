
using System.Collections.Generic;
using UnityEngine;
using UColor = UnityEngine.Color;
using System.Linq;

namespace Main {


	public interface IProject {

		//! ----- property -----

		string Name { get; }
		UColor Color { get; }
		bool IsEffectiveDefault { get; }

	}


	public class Project : IProject {

		//! ----- property -----

		public string Name { get; }
		public UColor Color { get; }
		public bool IsEffectiveDefault { get; }


		//! ----- itor/dtor -----

		public Project(string name, UColor color, bool is_effective_default) {
			Name = name;
			Color = color;
			IsEffectiveDefault = is_effective_default;
		}

	}


	public static class ProjectsDB {

		static IList<IProject> s_projects;

		//! ----- static -----

		//! 登録済みProject一覧の生成
		public static void Initialized() {
			if (s_projects == null) {
				s_projects = new List<IProject>{
					new Project("pabulum", new UColor(1f, 0.9f, 0.7f), true),
					new Project("MisLead", new UColor(0.7f, 0.7f, 1f), true),
					new Project("Dev::Unity", new UColor(0.7f, 1f, 0.8f), true),
					new Project("Unproductive", new UColor(0.5f, 0.2f, 0.2f), false),
				};
				Debug.Log("Initialized ProjectsDB.");
			}
			else { Debug.Log("ProjectsDB has already initialized."); }
		}

		//! Projectを名前から引く
		//! 見つからなければ null
		public static IProject At(string name) {
			var proj = s_projects.Where(p => p.Name == name);
			if (proj.Count() == 1) {
				return proj.First();
			}
			else { return null; }
		}


	}

}
