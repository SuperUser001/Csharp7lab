using System;
using System.Reflection;

namespace Lab7_01
{
	internal class MyClass : IInterface1, IInterface2
	{

		private int field1;
		protected int field2;
		internal static double field3;
		public string field4;

		public MyClass(int field1, int field2, string field4) { this.field1 = field1; this.field2 = field2; this.field4 = field4; }
		public MyClass() { field1 = 0; field2 = 0; field4 = ""; }

		public int Method1(int p1, int p2) { return p1 + p2; }

		public void Method3() {
			Console.WriteLine($"Строка: {field4} ");
		}
	
		public string Method4(string p4) {
			field4 = p4;
			Console.WriteLine($"4 метод выполнен, значение field4 = {field4}");
			return field4;
		}

		public string Method5(string p4) {
			field4 = p4;
			return field4;
		}
	}
}

