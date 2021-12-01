using System;

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
	}
}
