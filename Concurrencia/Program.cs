using FluentValidation;
using FluentValidation.Results;
using Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Concurrencia
{
    internal class Program
    {
        public static event EventHandler<int> MyEvent;
        static void Main(string[] args)
        {
            List<Post> posts = new List<Post>
            {
                new Post{
                    Id = 1,
                    UserId = 1,
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

            Console.ReadLine();
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
