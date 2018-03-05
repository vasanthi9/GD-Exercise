using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GreenDotExercise
{
	public class Packager
	{
		Dictionary<string, List<string>> _source = new Dictionary<string, List<string>>();

		public Packager()
		{

		}

		public IList<string> InstallPackages(IEnumerable<Package> source)
		{
			var sorted = new List<string>();
			var visited = new Dictionary<string, bool>();

			foreach (var package in source)
			{
				_source.Add(package.Name, package.Dependencies.ToList());
			}

			foreach (var item in _source)
			{
				Visit(item.Key, item.Value, sorted, visited);
			}

			return sorted;
		}

		private void Visit(string itemName, ICollection<string> dependencies, List<string> sorted, Dictionary<string, bool> visited)
		{
			bool inProcess;
			var alreadyVisited = visited.TryGetValue(itemName, out inProcess);

			if (alreadyVisited)
			{
				if (inProcess)
				{
					throw new ArgumentException("Cyclic dependency found.");
				}
			}
			else
			{
				visited[itemName] = true;

				if (dependencies != null)
				{
					foreach (var dependency in dependencies)
					{
							Visit(dependency, GetDependencies(dependency), sorted, visited);
					}
				}

				visited[itemName] = false;
				sorted.Add(itemName);
			}
		}

		private ICollection<string> GetDependencies(string itemName)
		{
			List<string> dependencies = new List<string>();

			if (_source.ContainsKey(itemName))
				dependencies = _source[itemName];

			return dependencies;
		}
	}
}
