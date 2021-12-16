using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Closure
{
	class Program
	{
		static void Main(string[] args)
		{
			Empleado empleado = new Empleado()
            {
				Id = 1,
				Name = "Juan",
				Position = "CEO"
            };

			//Ejemplo de uso de deconstructor
			var (id, nombre, _) = empleado;
			Console.WriteLine($"{id} {nombre}");

			//ejemplo de tupla
			var tupla = (a:5, b:4);
			Console.WriteLine(tupla.a);

			//Otro closure
			var fncAcumulador = Acumulador(LlamadaHttp);
			Parallel.For(1, 11, (i) => fncAcumulador());		


			//Ejemplo validación general con expresiones lambda
			Post post1 = new Post()
			{
				Id=1,
				UserId = 1,
				Title="Es una prueba",
				Body = "Es una prueba"
			};
			Console.WriteLine(Validator.Validate(post1, Validator.predicates));

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

			Console.ReadLine();

			//Ejemplo de validación de clase Post
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

		static Func<int> Acumulador(Action<int> fncLlamadaHttp)
        {
			int counter = 0;

			return () => {
				fncLlamadaHttp(++counter);
				return counter; 
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

		public static async void LlamadaHttp(int i)
        {
			HttpClient httpClient = new HttpClient();
			var response = await httpClient.GetAsync("https://www.20minutos.es/");
			Console.WriteLine($"Solicitud: {i} {response.StatusCode}");
        }
	}

	public class GeneralValidator<T>
    {
		public static readonly Predicate<T> NotNull = o => o != null && (o.GetType()== typeof(int)? Convert.ToInt32(o)>0:true);
		public static readonly Func<string, int, bool> SizeMax = (o, num) => o != null && o.Length > 0 && o.Length <= num;
	}

	public class Validator
    {
		public static readonly Predicate<Post>[] predicates =
		{
			c => GeneralValidator<int>.NotNull(c.Id),
			c => GeneralValidator<int>.NotNull(c.UserId),
			c => GeneralValidator<string>.NotNull(c.Title),
			c => GeneralValidator<Post>.SizeMax(c.Title,20)
		};

		public static bool Validate(Post p, params Predicate<Post>[] predicates) =>
			predicates.ToList().Where(elemP =>
            {
                return !elemP(p);
			}
        ).Count() == 0;

    }

	public class Empleado
    {
		public int Id { get; set; }
		public string Name { get; set; }
		public string Position { get; set; }

		public void Deconstruct (out int Id, out string Name, out string Position) => (Id, Name, Position) = (this.Id, this.Name, this.Position);
        
    }
}
