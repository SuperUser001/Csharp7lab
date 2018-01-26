using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
/*
Используя задачи (объекты классов Task и Task<TResult>), реализовать
программу. Спроектировать задачу
так, чтобы, несмотря на количество созданных в процессе выполнения задач,
завершение выполнения всего расчета можно было отследить по одной,
объединяющей остальные, задаче.

Написать программу, параллельно вычисляющую значения числа
Пи методом Монте-Карло. Общее количество случайных точек для
всех параллельных потоков должно задаваться пользователем после
запуска программы. Результаты вычислений параллельных участков
после их завершения должны усредняться. Полученный результат
вывести на экран.

 */
namespace Lab10_01
{
	class Proga
	{
		static double getDouble(Random rand) {
			return rand.NextDouble();
		}
		static double GetPiVAlue(int radius, double dots_count) { 


			double dotsInCircleCount = 0;  // точка на окружности
			int radius2 = radius * radius; //квадрат радиуса

			Random rand = new Random(); // создвние объекта класса для получения рандомного числа
			for ( int i = 0; i < dots_count; i++ ) {   // цикл создания точек на окнужности
				double x = getDouble(rand) * radius;  // получение значения х точки
				double y = getDouble(rand) * radius; // получение значения у точки

				//если меньше, то точка находится в окружности
				if ( (y * y) + (x * x) <= radius2 )
					dotsInCircleCount++; //на точку больше становится соответственно
			}
			return 4.0 * dotsInCircleCount / dots_count; // возвращаем значение
		}


		static void Main(string[] args) {
			double pi = 0;  
			DateTime begin = DateTime.Now;   
			for ( int i = 0; i < 10; i++ ) { 
				pi += GetPiVAlue(1, 1000);
			}

			pi /= 10;  
			DateTime end = DateTime.Now; 
			Console.WriteLine(end.Subtract(begin));  // получаем разницу по дате и времени (сколько затратили)
			begin = DateTime.Now; // получаем текущую дату и время
			List<Task<double>> pi_tasks = new List<Task<double>>();    // создаем список задач
			for ( int i = 0; i < 10; i++ ) { 
				pi_tasks.Add(Task.Factory.StartNew<double>(() => { return GetPiVAlue(1, 1000); }));
			}

			Task finalTask = Task.Factory.ContinueWhenAll( 
				pi_tasks.ToArray(),  // приводим список а массиву
				pi_val => { Console.WriteLine($"{ (pi_val.Sum(val => val.Result)) / (double) pi_val.Count() }"); });
			// выводим значение в консоль // суммируем полученные значения от результатов вычисления делем сумму на количество результатов
			finalTask.Wait(); 
			Console.WriteLine("ccc"); 
			end = DateTime.Now;    // получаем текущую дату и время
			Console.WriteLine(end.Subtract(begin)); 
			Console.ReadKey(); 
		}
	}
}
