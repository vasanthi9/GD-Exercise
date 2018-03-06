using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GreenDotExercise.Test
{
	[TestClass]
	public class UnitTest
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentException), "Package list is empty.")]
		public void InstallPackages_EmptyPackageListException()
		{
			Packager p = new Packager();
			List<string> result = p.InstallPackages(null) as List<string>;
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException), "Package list is empty.")]
		public void InstallPackages_EmptyPackageException()
		{
			var packages = new[] { new Package { } };

			Packager p = new Packager();
			List<string> result = p.InstallPackages(packages) as List<string>;
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException), "Cyclic dependency found.")]
		public void InstallPackagesCyclicException()
		{
			var packages = new[]
			{
				new Package { Name = "R2D2", Dependencies = new List<string> { "C3PO" }},
				new Package { Name = "C3PO", Dependencies = new List<string> { "BB8" }},
				new Package { Name = "BB8", Dependencies = new List<string> { "R2D2" }},
			};

			Packager p = new Packager();
			List<string> result = p.InstallPackages(packages) as List<string>;
		}

		[TestMethod]
		public void InstallPackages_Single()
		{
			var packages = new[]
			{
				new Package { Name = "Luke", Dependencies = new List<string> { "Obi-Wan" }},
				new Package { Name = "Obi-Wan", Dependencies = new List<string> { }},
			};

			Packager p = new Packager();
			List<string> result = p.InstallPackages(packages) as List<string>;

			var expected = new List<string> { "Obi-Wan", "Luke" };

			CollectionAssert.AreEqual(result, expected);
		}

		[TestMethod]
		public void InstallPackages_Multiple()
		{
			var packages = new[]
			{
				new Package { Name = "Luke", Dependencies = new List<string> { "Padme" }},
				new Package { Name = "Chewbacca", Dependencies = new List<string> { "Han" }},
				new Package { Name = "Leia", Dependencies = new List<string> { "Padme" }},
				new Package { Name = "Han", Dependencies = new List<string> { "Millenium Falcon" }},
			};

			Packager p = new Packager();

			List<string> result = p.InstallPackages(packages) as List<string>;

			var expected = new List<string> { "Padme", "Luke", "Millenium Falcon", "Han", "Chewbacca", "Leia" };

			CollectionAssert.AreEqual(result, expected);
		}

		[TestMethod]
		public void InstallPackages_EnsureCaseAgnostic()
		{
			var packages = new[]
			{
				new Package { Name = "January", Dependencies = new List<string> { "February" }},
				new Package { Name = "february", Dependencies = new List<string> { "March" }}
			};

			Packager p = new Packager();

			List<string> result = p.InstallPackages(packages) as List<string>;

			var expected = new List<string> { "March", "February", "January" };

			CollectionAssert.AreEqual(result, expected);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException), "Invalid Package.")]
		public void InstallPackages_InvalidPackageNameException()
		{
			var packages = new[]
			{
				new Package { Name = "January", Dependencies = new List<string> { "February" }},
				new Package { Name = string.Empty, Dependencies = new List<string> { "March" }}
			};

			Packager p = new Packager();

			List<string> result = p.InstallPackages(packages) as List<string>;

			var expected = new List<string> { "March", "February", "January" };

			CollectionAssert.AreEqual(result, expected);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException), "Invalid Package.")]
		public void InstallPackages_InvalidPackageDependencyException()
		{
			var packages = new[]
			{
				new Package { Name = "January", Dependencies = new List<string> { "February" }},
				new Package { Name = "February", Dependencies = new List<string> { null }}
			};

			Packager p = new Packager();

			List<string> result = p.InstallPackages(packages) as List<string>;

			var expected = new List<string> { "February", "January" };

			CollectionAssert.AreEqual(result, expected);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException), "Invalid Package.")]
		public void InstallPackages_NullPackageDependencyException()
		{
			var packages = new[]
			{
				new Package { Name = "January", Dependencies = new List<string> { "February" }},
				new Package { Name = "February", Dependencies = null}
			};

			Packager p = new Packager();

			List<string> result = p.InstallPackages(packages) as List<string>;

			var expected = new List<string> { "February", "January" };

			CollectionAssert.AreEqual(result, expected);
		}

		[TestMethod]
		public void InstallPackages_MultipleDependencies()
		{
			var packages = new[]
			{
				new Package { Name = "April", Dependencies = new List<string> { "January", "March" }},
				new Package { Name = "January", Dependencies = new List<string> { } },
				new Package { Name = "March", Dependencies = new List<string> { "February" } },
			};

			Packager p = new Packager();

			List<string> result = p.InstallPackages(packages) as List<string>;

			var expected = new List<string> { "January", "February", "March", "April"  };

			CollectionAssert.AreEqual(result, expected);
		}
	}
}
