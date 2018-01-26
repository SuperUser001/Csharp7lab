using System;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

/*
ЗАДАНИЕ 3.
С использованием механизма рефлексии и пользовательских атрибутов выполнить один из следующих вариантов:
 Реализовать атрибут CommandLineAttribute с параметром CommandSwitch указывающим имя параметра командной строки программы. 
Атрибут должен применяться к полям и свойствам класса.
 Написать алгоритм разбора командной строки вида «-<имя-параметра1>[=<значение1>] …» 
 присваивающий соответствующим полям и свойствам объекта значения параметра из командной строки.
  Должны поддерживаться поля и свойства логического, целочисленного и строкового типов.
 */
namespace Lab7_03
{
	class Programm
	{


		static void Main(string[] args) {
			//парсим ключи командной строки
			var dict = new Dictionary<string, string>();
			foreach ( var arg in args ) {
				var regex = new Regex(@"^-(?<key>\w+)=(?<value>.*)$");
				var m = regex.Match(arg);
				if ( m.Success )
					dict[m.Groups["key"].Value] = m.Groups["value"].Value;
			}

			//создаем объект
			var foo = new Foo();
			//перебираем поля
			foreach ( var fi in foo.GetType().GetFields() ) {
				//получаем атрибуты
				var attrs = fi.GetCustomAttributes(typeof(CommandLineAttribute), false);
				//перебираем атрибуты
				foreach ( var attr in attrs.Cast<CommandLineAttribute>() )
					if ( dict.ContainsKey(attr.CommandSwitch) )//командная строка содержит CommandSwitch ?
					{
						//парсим
						var val = Convert.ChangeType(dict[attr.CommandSwitch], fi.FieldType);
						//заносим в объект
						fi.SetValue(foo, val);
					}
			}

			//выводим поля объекта
			Console.WriteLine("Str: {0}", foo.Str);
			Console.WriteLine("Int: {0}", foo.Int);
			Console.WriteLine("Bool: {0}", foo.Bool);

			Console.ReadKey();
		}
	}
}