using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GreenDotExercise.Test
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void PackagerSort_Multiple()
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

			var expected = new List<string>();
			expected.Add("Padme");
			expected.Add("Luke");
			expected.Add("Millenium Falcon");
			expected.Add("Han");
			expected.Add("Chewbacca");
			expected.Add("Leia");

			CollectionAssert.AreEqual(result, expected);
		}
		[TestMethod]
		public void PackagerSort_Single()
		{
			var packages = new[]
			{
				new Package { Name = "Luke", Dependencies = new List<string> { "Obi-Wan" }},
				new Package { Name = "Obi-Wan", Dependencies = new List<string> { }},
			};

			Packager p = new Packager();
			List<string> result = p.InstallPackages(packages) as List<string>;

			var expected = new List<string>();
			expected.Add("Obi-Wan");
			expected.Add("Luke");

			CollectionAssert.AreEqual(result, expected);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException), "Cyclic dependency found.")]
		public void PackagerSortCyclicException()
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
	}
}
