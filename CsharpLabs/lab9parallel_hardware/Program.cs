using System;
using System.IO;
using System.Collections.Concurrent;
using System.Threading;
using System.Collections.Generic;

/*
Предполагая, что в некотором каталоге на диске сохранено большое количество файлов с логами (журналами работы) прокси-сервера, 
написать программу вычисляющую статистику потребления трафика сети Интернет. 
На выходе программа должна создавать три текстовых документа: статистика по пользователям, статистика по доменам, статистика по датам. 
В качестве статистики использовать общий объем потребленного трафика, соответственно пользователем за все дни, при обращении к домену, в указанный день.
При разработке программы считать, что каждый файл должен обрабатываться отдельно параллельно выполняющимся участком кода. 
После обработки всех файлов, полученные результаты для каждого из них должны суммироваться в общую сводку.
 */

namespace Lab09_01
{
		class MyThreadLab
		{
			string[] dirs = Directory.GetFiles("C:/Users/Admin/Desktop/7семестрc#/ВУЗ/Lab09_01/Lab09_01/bin/Debug", "*.log");
			ConcurrentDictionary<string, int> users = new ConcurrentDictionary<string, int>(); //пользователи
			// Представляет потокобезопасную коллекцию пар "ключ-значение", 
			// доступ к которой могут одновременно получать несколько потоков.

			ConcurrentDictionary<string, int> date = new ConcurrentDictionary<string, int>(); //даты
			ConcurrentDictionary<string, int> domain = new ConcurrentDictionary<string, int>(); //домены
			
			public void printDir() {
				foreach ( var d in dirs ) {
					Console.WriteLine("Dirs: " + d);
				}
			}
			
			//читаем файл
			public void readFile() {
				List<Thread> lst = new List<Thread>();
				foreach ( var d in dirs ) {
					Thread th = new Thread(() => {
						using ( var st = new StreamReader(d) ) {
							while ( !st.EndOfStream ) {
								var str = st.ReadLine().Split(' ');
								var traffic = System.Convert.ToInt32(str[3]);
								users.AddOrUpdate(str[0], traffic, (key, oldValue) => oldValue + traffic);

								date.AddOrUpdate(str[1], traffic, (key, oldValue) => oldValue + traffic);

								domain.AddOrUpdate(str[2], traffic, (key, oldValue) => oldValue + traffic);
							}
						}
					});
					th.Start();
					lst.Add(th);
				}
				for ( int i = 0; i < lst.Count; i++ ) {
					lst[i].Join();
				//Блокирует вызывающий поток до момента пока поток,
				//связанный с методом, не будет завершен(ожидание
				//завершения потока).
				}
			}

		//статистика по пользователям
			public void writeFileUser() {
				Thread.Sleep(4000);
				FileStream fs = new FileStream("StatUser.txt", FileMode.Create);
				using ( var st = new StreamWriter(fs) ) {
					var keys = users.Keys;
					foreach ( var k in keys ) {
						st.WriteLine(k + ": " + users[k]);
					}
				}

			}

		//статистика по датам
			public void writeFileDate() {
				FileStream fs = new FileStream("StatDate.txt", FileMode.Create);
				using ( var st = new StreamWriter(fs) ) {
					var keys = date.Keys;
					foreach ( var k in keys ) {
						st.WriteLine(k + ": " + date[k]);
					}
				}
			}

		//статистика по доменам
			public void writeFileDomain() {
				FileStream fs = new FileStream("StatDomain.txt", FileMode.Create);
				using ( var st = new StreamWriter(fs) ) {
					var keys = domain.Keys;
					foreach ( var k in keys ) {
						st.WriteLine(k + ": " + domain[k]);
					}
				}
			}
		}

		class MainClass
		{
			public static void Main(string[] args) {
				MyThreadLab mtl = new MyThreadLab();
				mtl.printDir();
				mtl.readFile();
				Thread th = new Thread(mtl.writeFileUser);
				Thread th2 = new Thread(mtl.writeFileDate);
				Thread th3 = new Thread(mtl.writeFileDomain);
				th.Start();
				th2.Start();
				th3.Start();
				//th.Join();
				//th2.Join();
				//th3.Join();
				
				Console.ReadKey();
			}
		}

}