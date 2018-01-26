using System;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

/*ЗАДАНИЕ 1.
1) Описать класс MyClass, который будет содержать:
 поля различных типов и различным уровнем доступа;
 методы, с различным набором аргументов и различным типом возвращаемого значения.
2) Объявить класс MyTestClass, который будет содержать методы выполняющие следующие действия:
 выводить по имени класса имена методов, которые содержат строковые параметры (имя класса передается в качестве аргумента);
 вызывать некоторый метод класса, при этом значения для его параметров необходимо прочитать из текстового файла (имя класса и имя метода передаются в качестве аргументов).
  ЗАДАНИЕ 2.
  1) Расположить класс MyClass в отдельном .cs-файле и дополнить его следующими членами:
 перегрузить конструктор: один конструктор без параметров, другой с параметрами;
 объявить два интерфейса (IInterface1 и IInterface2) как минимум с двумя методами каждый и реализовать их
 одно из полей объявить как static
2) В классе MyTestClass реализовать метод (принимающий в качестве параметра имя класса), который выводит всё содержимое класса в текстовый файл;
3) Реализовать метод (принимающий в качестве параметра имя класса), который записывает все члены класса в файл *.cs, который должен правильно компилироваться в среде .NET.
 */
namespace Lab7_01
{
	class MyTestClass
	{
		public void NumberOne(Type type, MyClass myclass) {
			//вывод по имени класса имя методов, содержащих строковый параметр
			Console.WriteLine("Методы, содержащий строковый параметр: ");
			var method = type.GetMethods();
			foreach (MethodInfo methodInfo in method) {
				ParameterInfo [] parametrs = methodInfo.GetParameters();		
					foreach(ParameterInfo parametrinfo in parametrs)
						if ( parametrs[0].ParameterType == typeof(string) ) {
							Console.WriteLine(methodInfo.Name);
				}
			}
		}

		public void NumberTwo(MyClass myclass, Type type) {
			//вызываем метод класса, значения переменных считываем с файла
			// имя класса и имя метода - аргументы
			Console.WriteLine("Вторая функция.");
			FileStream file1 = new FileStream("file.txt", FileMode.Open);
			StreamReader reader = new StreamReader(file1);
			string a = reader.ReadLine();
			var methodInfo = type.GetMethod(a, BindingFlags.Public | BindingFlags.Instance);
			string b = reader.ReadLine();
			methodInfo.Invoke(myclass, new object[] {b});
			reader.Close();
		}

		public void NumberThree(MyClass myclass) {
			Console.WriteLine("Содержимое класса, 3 функция: ");
			Type type = myclass.GetType();
			using ( StreamWriter sw = new StreamWriter("file3.txt", false) ) {
				var method = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);
				sw.WriteLine("–––––––––––––––––––––\n");
				sw.WriteLine("Информация о классе");
				sw.WriteLine("Базовый?:  {0}", type.BaseType);
				sw.WriteLine("Абстрактный:  {0}", type.IsAbstract);
				sw.WriteLine("Защищённый клас:  {0}", type.IsSealed);
				sw.WriteLine("Простой класс:  {0}", type.IsClass);
				sw.WriteLine("–––––––––––––––––––––––––––\n");

				sw.WriteLine("-----------------------");
				sw.WriteLine("Информация о методах");
				foreach (MethodInfo mi in method) {
				sw.WriteLine(mi.ToString());
		//		Console.WriteLine(mi.ToString());
			}
				sw.WriteLine("-----------------------");
				sw.WriteLine();
				sw.WriteLine("-----------------------");
				sw.WriteLine("Информация о интерфейсах");
				var interfaceValue = type.GetInterfaces();
				foreach (Type typeInterface in interfaceValue) {
					sw.WriteLine(typeInterface.ToString());
			//		Console.WriteLine(typeInterface);
			}
				sw.WriteLine("-----------------------");
				sw.WriteLine();
				sw.WriteLine("-----------------------");
				sw.WriteLine("Информация о полях (в т.ч. защищенных)");
				var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
			foreach ( FieldInfo fieldInfo in fields) {
					sw.WriteLine(fieldInfo.ToString());
			//		Console.WriteLine(fieldInfo);
			}
	      	}
		}

		public void NumberFour(MyClass myclass) {
			{
				Console.WriteLine("Выполнена функция 4.");
				Type type = myclass.GetType();
				using (StreamWriter sw = new StreamWriter("file4.cs", false) ) {
					sw.WriteLine("using System;");
					sw.WriteLine("using System.Reflection;");
					sw.WriteLine("namespace Lab7_01  {");
					sw.WriteLine("internal class MyClassClone  { ");
					FieldInfo [] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static);
					foreach ( FieldInfo fieldInfo in fields ) {
						Console.WriteLine(fieldInfo.Name);
						if ( fieldInfo.IsPrivate ) sw.Write("private"); 
						if ( fieldInfo.IsAssembly ) sw.Write("internal ");
						if ( fieldInfo.IsPublic ) sw.Write("public");
						if ( fieldInfo.IsFamily ) sw.Write("protected");
						if (fieldInfo.IsStatic) sw.Write("static");
						sw.Write(" " + fieldInfo.FieldType.Name.ToString() + " " + fieldInfo.Name.ToString() + ";");
						sw.WriteLine();
					}
					sw.WriteLine();
					var constructors = type.GetConstructors();
					foreach ( ConstructorInfo constrInfo in constructors ) {
						Console.WriteLine(constrInfo.DeclaringType.Name);
						if ( constrInfo.IsPrivate ) sw.Write("private ");
						if ( constrInfo.IsPublic ) sw.Write("public ");
						sw.Write(constrInfo.DeclaringType.Name.ToString() + "Clone");
						var parametrs = constrInfo.GetParameters(); 
						sw.Write(" ("); int i = 0;
							foreach (ParameterInfo paramInfo in parametrs) {
							sw.Write(paramInfo.ParameterType.Name.ToString() + " " + paramInfo.Name.ToString());
							if ( i < 2 ) sw.Write(","); else sw.WriteLine(") { throw new System.NotImplementedException(); }");
							i++;
						}
					}
					sw.Write(" ) { throw new System.NotImplementedException(); }");
					sw.WriteLine();


					sw.WriteLine("	}");
					sw.WriteLine("}");
				}
	}
		}
	}
	 
	class Programm{


		static void Main(string[] args) {

			MyClass myclass = new MyClass();
			Type type = myclass.GetType();
			//возвращает метаописание типа объекта, у которого он вызван (GetType())

			MyTestClass mytestclass = new MyTestClass();
			//			mytestclass.NumberOne(type, myclass);
			//		mytestclass.NumberTwo(myclass, type);
			//	mytestclass.NumberThree(myclass);
			mytestclass.NumberFour(myclass);
			Console.ReadLine();
		}
	}
}
