using System;
using System.Collections.Generic;

namespace Closure
{
	class Program
	{
		static void Main(string[] args)
		{
			Action fn = EjemploClosure(2);
			HacerAlgo(fn);
			HacerAlgo(fn);
			HacerAlgo(fn);

			//Ejemplo de yield
			var i = 0;
			foreach (int arg in bucleSinYield())
            {
				Console.WriteLine($"Console externo {++i}");
            }

			i = 0;
			foreach (int arg in bucleConYield())
			{
				Console.WriteLine($"Console externo {++i}");
			}
		}

		static Action EjemploClosure(int maximo)
		{
			int i = 0;
			Console.WriteLine("Se inicializa EjemploClosure");

			return () =>
			{
				if (i < 2)
				{
					Console.WriteLine("Se ejecuta Ejemplo de Closure");
				}
				else
				{
					Console.WriteLine("Ya no se ejecuta Ejemplo de Closure");
				}
				i++;
			};
		}

		static void HacerAlgo(Action fn)
		{
			Console.WriteLine("Hace Algo Inicio");
			fn();
			Console.WriteLine("Hace Algo Fin");
		}


		public static IEnumerable<int> bucleSinYield()
        {
			List<int> auxiliar = new List<int>();
			for (int i=0;i<10;i++)
            {
				Console.WriteLine($"Console interno sin Yield {i+1}");
				auxiliar.Add(i);
            }

			return auxiliar;
        }

		public static IEnumerable<int> bucleConYield()
		{
			for (int i = 0; i < 10; i++)
			{
				Console.WriteLine($"Console interno con Yield {i + 1}");
				yield return i;
			}
		}
	}
}
