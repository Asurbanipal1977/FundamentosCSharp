using FundamentosCSharp.Models;
using FundamentosCSharp.BD;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http;
using FundamentosCSharp.Service;
using System.Linq;
using System.Threading;

namespace FundamentosCSharp
{
    public class Program
    {
        public delegate string Mostrar(string cadena);
        //static void Main(string[] args)
        //{
            //IBebidaAlcoholica bebida = new Vino(330);
            //bebida.CantidadMax();

            //AccesoBD acceso = new AccesoBD();
            //acceso.Edit(new Cerveza(330)
            //{
            //    Id = 1004,
            //    Nombre = "Calsberg",
            //    Marca = "Heineken",
            //    Alcohol = 6
            //});
            //acceso.Delete(new Cerveza(330)
            //{
            //    Id = 1004
            //});
            //List<Cerveza> lista = acceso.ListarCervezas();

            //foreach (Cerveza c in lista)
            //{
            //    Console.WriteLine(c);
            //    File.AppendAllText("prueba.txt", c.ToString());
            //    File.AppendAllText("prueba.txt", "\r\n");
            //}
            //Deserializar();
            //Task task = Task.Run(() => Console.WriteLine("Es una prueba de tarea"));
            //task.Wait();

            //Console.WriteLine("Terminé el programa");
        //}

        //public static void Deserializar()
        //{
        //    foreach (string line in File.ReadLines("prueba.txt"))
        //    {
        //        Cerveza cerveza = JsonSerializer.Deserialize<Cerveza>(line);
        //        Console.WriteLine("Objeto impreso:" + cerveza);
        //    }

        //}



        public static async Task Main(string[] args)
        {
            SendRequest<Post> service = new SendRequest<Post>();

            //Listado
            List<Post> posts = await service.Listar();

            //Modificación
            Post modificado = await service.ModifyTypiCode(100, new Post
            {
                UserId = 100,
                Title = "Prueba",
                Body = "Prueba"
            });
            int index = posts.FindIndex(s => s.Id == 100);
            if (index != -1)
                posts[index] = modificado;

            //Alta
            posts.Add(await service.AddTypicode(new Post
            {
                UserId = 101,
                Title = "Uno cualquiera",
                Body = "Este es el cuerpo"
            }));

            //Borrado
            if (await service.DeleteTypiCode(99))
            {
                posts.RemoveAt(posts.FindIndex(s => s.Id == 99));
            }

            Console.WriteLine(JsonSerializer.Serialize(posts.Where(p => p.Id > 99).OrderByDescending(p => p.Id).ToList()));

            AccesoBD acceso = new AccesoBD();
            List<Cerveza> listaCervezas = acceso.ListarCervezas();
            List<Bar> listaBar = new List<Bar>
            {
                new Bar("Bar Reynols"){ CervezaList=listaCervezas},
                new Bar("Bar Malo"){ CervezaList=listaCervezas},
                new Bar("Bar Vacío"){ CervezaList=null}
            };

            //Se usa CervezaList? por si está igual a null
            var bar = (from b in listaBar
                       where b.CervezaList?.Where(c => c.Marca.ToUpper() == "PAULANER").Count() > 0
                       select new { Nombre = b.Name }).ToList();

            Console.WriteLine(JsonSerializer.Serialize(bar));

            //Ahora no funcionaría porque la clase SendRequest solo admite clases que implementen la interfaz ISendRequest
            //SendRequest<Cerveza> service2 = new SendRequest<Cerveza>();
            //var cerveza = await service2.AddTypicode(new Cerveza { Alcohol = 8, Cantidad = 330, Marca = "Una", Nombre = "Otra" });
            //Console.WriteLine(JsonSerializer.Serialize(cerveza));

            //Se asigna al delegado una función de con la misma firma
            //Mostrar mostrar = Show;

            //Función anónima
            Func<string, string> mostrar = (a) => a.ToUpper();
            HacerAlgo(mostrar);

            Action<string, string> mostrar2 = (a, b) => Console.WriteLine($"{a} {b}");
            HacerAlgo2(mostrar2);

            var numbers = new List<int> { 1, 56, 3, 7, 9, 13, 23, 36, 8 };
            var predicado = new Predicate<int>(IsDivider2);
            var dividers = numbers.FindAll(predicado);

            dividers.ForEach(d => Console.WriteLine(d));

            //Funciones en tipos anónimos
            Action<int> FunctionPrueba = (a) => Console.WriteLine($"Es una prueba: {a}");

            var pruebaAnonimo = new
            {
                Name = "prueba",
                Function = FunctionPrueba
            };
            pruebaAnonimo.Function(6);

            foreach (var elem in pruebaAnonimo.GetType().GetProperties())
            {
                Console.WriteLine($"{elem.Name} {elem.GetValue(pruebaAnonimo)} {pruebaAnonimo.GetType().GetProperty(elem.Name)}");
            }

            //recorrer lista con Parallel.Foreach: se recorre la lista de forma paralela
            List<int> listaNumeros = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
            Parallel.ForEach(listaNumeros, c => Console.WriteLine(c));

            ////Lista de tareas
            var cancel = new CancellationTokenSource();
            var token = cancel.Token;

            Task[] tasks = new Task[3];
            tasks[0] = Task.Run(async delegate
            {
                await Task.Delay(2000, token);
                Console.WriteLine("Tarea 1");
            });
            tasks[1] = Task.Run(async delegate
            {
                await Task.Delay(2000);
                Console.WriteLine("Tarea 2");
            });
            tasks[2] = Task.Run(async delegate
            {
                await Task.Delay(2000, token);
                Console.WriteLine("Tarea 4");
            });

            await Task.Delay(100);
            cancel.Cancel();

            try { Task.WaitAll(tasks); } catch { }

            foreach (var item in tasks)
            {
                Console.WriteLine($"{item.Id} {item.Status}");
            }

            Console.WriteLine("Se termina el programa");

        }

        public static void HacerAlgo(Func<string, string> funcionFinal)
        {
            Console.WriteLine("Hace algo");
            Console.WriteLine(funcionFinal("Se envió desde otra función"));
        }

        public static void HacerAlgo2(Action<string, string> funcionFinal)
        {
            Console.WriteLine("Hace algo");
            funcionFinal("Se envió desde la otra función", "La segunda");
        }

        public async static void Tarea1(CancellationToken c)
        {
            await Task.Delay(2000, c);
            Console.WriteLine("Tarea 1");
        }

        public async static void Tarea2(CancellationToken c)
        {
            await Task.Delay(2000,c);
            Console.WriteLine("Tarea 2");
        }

        public async static void Tarea3(CancellationToken c)
        {
            await Task.Delay(2000,c);
            Console.WriteLine("Tarea 3");
        }

        public static bool IsDivider2(int x) => x % 2 == 0;

    }
}