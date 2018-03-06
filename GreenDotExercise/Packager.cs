using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GreenDotExercise
{
	public class Packager
	{
		IEnumerable<Package> _packages;

		/// <summary>
		/// Calculate install order for a list of packages
		/// </summary>
		/// <param name="packages"></param>
		/// <returns>List of package names ordered by dependencies</returns>
		public IEnumerable<string> InstallPackages(IEnumerable<Package> packages)
		{
			var sorted = new List<string>();
			var visited = new Dictionary<string, bool>();

			//Validate input since the algorithm assumes a valid input
			ValidatePackage(packages);

			_packages = packages;

			foreach (var package in _packages)
			{
				ProcessPackage(package.Name, package.Dependencies, sorted, visited);
			}

			return sorted;
		}

		/// <summary>
		/// Traverse through the package to create an ordered list with dependencies 
		/// appearing before the package itself  
		/// </summary>
		/// <param name="packageName"></param>
		/// <param name="dependencies"></param>
		/// <param name="sorted"></param>
		/// <param name="visited"></param>
		private void ProcessPackage(string packageName, ICollection<string> dependencies, List<string> sorted, Dictionary<string, bool> visited)
		{
			bool inProcess; //Is the package currently being processed
			var alreadyVisited = visited.TryGetValue(packageName.ToLower(), out inProcess);

			if (alreadyVisited)
			{
				if (inProcess)
				{
					throw new ArgumentException("Cyclic dependency found.");
				}
			}
			else
			{
				visited[packageName.ToLower()] = true;

				if (dependencies != null)
				{
					foreach (var dependency in dependencies)
					{
						ProcessPackage(dependency, GetDependencies(dependency), sorted, visited);
					}
				}

				visited[packageName.ToLower()] = false;
				sorted.Add(packageName);
			}
		}

		/// <summary>
		/// Get dependencies for a package
		/// </summary>
		/// <param name="packageName"></param>
		/// <returns>list of dependencies</returns>
		private ICollection<string> GetDependencies(string packageName)
		{
			List<string> dependencies = new List<string>();

			Package pkg = _packages.FirstOrDefault(p => p.Name.Equals(packageName, StringComparison.InvariantCultureIgnoreCase));
			if (pkg != null)
				dependencies = pkg.Dependencies as List<string>;

			return dependencies;
		}

		/// <summary>
		/// Validate packages
		/// </summary>
		/// <param name="packages"></param>
		private void ValidatePackage(IEnumerable<Package> packages)
		{
			if (packages == null || packages.Count() <= 0)
			{
				throw new ArgumentException("Package list is empty.");
			}

			foreach (var package in packages)
			{
				if (string.IsNullOrEmpty(package.Name) || package.Dependencies == null || package.Dependencies.Contains(null))
				{
					throw new ArgumentException("Invalid Package.");
				}
			}
		}
	}
}
