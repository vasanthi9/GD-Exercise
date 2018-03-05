using System;
using System.Collections.Generic;
using System.Text;

namespace GreenDotExercise
{
    public class Package
    {
		public string Name { get; set; }
		public ICollection<string> Dependencies { get; set; }		

	}
}
