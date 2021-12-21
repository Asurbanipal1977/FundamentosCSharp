using FluentValidation;
using FluentValidation.Results;
using Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Concurrencia
{
    internal class Program
    {
        public static event EventHandler<int> MyEvent;
        public static event Action MyAction;
        static void Main(string[] args)
        {
            List<Post> posts = new List<Post>
            {
                new Post{
                    Id = 1,
                    UserId = 1,
                    Title = "Es una prueba",
                    Body = "Es una prueba"
                },
                new Post{
                    Id = 2,
                    UserId = 1,
                    Title = "Es una prueba",
                    Body = "Es una prueba"
                },
                new Post{
                    Id = 3,
                    UserId = 2,
                    Title = "Es una prueba",
                    Body = "Es una prueba"
                }

            };

            Post post1 = new Post()
            {
                Id=1,
                UserId = 1,
                Title = "Es una prueba",
                Body = "Es una prueba"
            };

            PostValidation postValidation1 = new PostValidation(posts);
            ValidationResult result = postValidation1.Validate(post1);
            if (!result.IsValid)
            {
                foreach (var elem in result.Errors)
                {
                    Console.WriteLine($"Error en {elem.PropertyName} {elem.ErrorMessage}");
                }
            }
            else
            {
                Console.WriteLine("Correcto");
            }


            //Comparativa entre asincrono y sincrono
            var watch = Stopwatch.StartNew();
            for (int i = 0; i < 100; i++)
            {
                var client = new HttpClient();
                var response = client.GetAsync("http://jsonplaceholder.typicode.com/todos/").Result;
                var result2 = response.Content.ReadAsStringAsync().Result;
            }
            watch.Stop();
            Console.WriteLine($"El tiempo para síncrono es de: {watch.ElapsedMilliseconds}");

            watch = Stopwatch.StartNew();
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < 100; i++)
            {
                tasks.Add(Task.Factory.StartNew(async () =>
                {
                    var client = new HttpClient();
                    var response = await client.GetAsync("http://jsonplaceholder.typicode.com/todos/");
                    //response.EnsureSuccessStatusCode();
                    var result2 = await response.Content.ReadAsStringAsync();
                    //Console.WriteLine(result);
                }));
            }
            Task.WaitAll(tasks.ToArray());
            watch.Stop();
            Console.WriteLine($"El tiempo para asíncrono es de: {watch.ElapsedMilliseconds}");

            //PROGRAMACION REACTIVA
            var observable = Observable.FromEventPattern<int>(
                c => MyEvent += c,
                c => MyEvent -= c
                );

            var subscrito1 = observable.Subscribe(c => { Console.WriteLine($"Se ha detectado el cambio por subscrito1: {c.EventArgs}"); });
            var subscrito2= observable.Subscribe(c => { Console.WriteLine($"Se ha detectado el cambio por subscrito2: {c.EventArgs}"); });

            MyEvent(null, 1);
            MyEvent(null, 2);

            //EVENTOS
            Empleados empleados = new Empleados();

            MyAction += () => { Console.WriteLine("Apagar Luces"); };
            MyAction += () => { Console.WriteLine("Cerrar Puertas"); };

            empleados.ficharSalida();
            empleados.ficharSalida();
            empleados.ficharSalida();
            empleados.ficharSalida();
            empleados.ficharSalida();
            empleados.ficharSalida();


            //GROUP BY EN LINQ
            List<Product> productos = new List<Product>()
            {
                new Product { Id=2, Name="Producto2", Country="Fracia", Price=2400},
                new Product { Id=1, Name="Producto1", Country="España", Price=1150},
                new Product { Id=1, Name="Producto1", Country="España", Price=1150},
                new Product { Id=2, Name="Producto2", Country="España", Price=150},
                new Product { Id=2, Name="Producto3", Country="Francia", Price=50},
                new Product { Id=2, Name="Producto2", Country="Fracia", Price=2400}
            };

            var totalVentas = from p in productos
                              group p by new { p.Name, p.Country } into totals
                              select new
                              {
                                  Name = totals.Key.Name,
                                  Country = totals.Key.Country,
                                  Total = totals.Sum(t => t.Price)
                              };

            foreach (var totalventa in totalVentas)
            {
                Console.WriteLine($"El {totalventa.Name} del país {totalventa.Country} tiene un sumatorio de {totalventa.Total} ");
            }


            var watch2 = Stopwatch.StartNew();
            var listaNumeros = Enumerable.Range(0,1000);
            var filterNumeros = listaNumeros.AsParallel().WithDegreeOfParallelism(2).Where(e => IsNumberValid(e)).ToList();
            watch2.Stop();
            Console.WriteLine($"Tiempo con Linq Lista {watch2.ElapsedMilliseconds}");

            //Leer xml
            var filename = @"C:\Users\mame1\source\repos\FundamentosCSharp\FundamentosCSharp\Concurrencia\store.xml";
            XDocument store = XDocument.Load(filename);

            var empleadosXML = store.Root.Elements("Empleado").OrderBy(e=>e.Attribute("Nombre").Value).ToList();
            empleadosXML.ForEach(empleado => Console.WriteLine(empleado.Attribute("Nombre").Value));

            Console.ReadLine();
        }


        public static bool IsNumberValid(int num)
        {
            Thread.Sleep(10);
            return (num % 2 != 0 && num % 5 != 0);
        }
        

        
        public class Empleados
        {
            private int empleados = 5;

            public void ficharSalida()
            {
                if (empleados > 0)
                {
                    empleados--;
                    if (empleados > 0)
                    {
                        Console.WriteLine($"Quedan {empleados} empleados");
                    }
                    else
                    {
                        Console.WriteLine("No hay mas empleados");
                        if (MyAction != null)
                            MyAction();
                    }
                }
            }
        }   

        public class Product
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public decimal Price { get; set; }
            public string Country { get; set; }
        }


        public class PostValidation : AbstractValidator<Post>
        {
            private List<Post> posts = new List<Post>();
            public PostValidation(List<Post> posts)
            {
                RuleFor(r => r.Id).NotNull().NotEmpty().GreaterThan(0);
                RuleFor(r => r.UserId).NotNull().NotEmpty().GreaterThan(0);
                RuleFor(r => r.Title).NotNull().NotEmpty().MaximumLength(20).MinimumLength(1);
                RuleFor(r => r.Id).Must(NoExistPost).WithMessage("Ya existe un post igual");
                this.posts = posts;
            }

            public bool NoExistPost(int id)
            {
                return !posts.Any(e => e.Id == id);
            }
        }
    }
}
