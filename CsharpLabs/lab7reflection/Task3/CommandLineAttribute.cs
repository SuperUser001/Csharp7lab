using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab7_03
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class CommandLineAttribute : Attribute
	{		public string CommandSwitch { get; set; }

		public CommandLineAttribute(string commandSwitch) {
			this.CommandSwitch = commandSwitch;
		}
	}

	class Foo
	{
		[CommandLineAttribute("str")]
		public string Str;
		[CommandLineAttribute("int")]
		public int Int;
		[CommandLineAttribute("bool")]
		public bool Bool;
	}
}
