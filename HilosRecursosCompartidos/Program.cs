using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using StackExchange;
using StackExchange.Redis;

namespace HilosRecursosCompartidos
{
    public static class Program
    {
        static void Main(string[] args)
        {
            //Todos van al mismo fichero
            string path = "result.txt";
            var fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
            Parallel.For(1, 101, new ParallelOptions { MaxDegreeOfParallelism = 10 },
                (i) => Write(i.ToString(), fs));


            //Redis
            var redisDb = RedisDB.Connection.GetDatabase();
            redisDb.StringSet("1", "prueba");

            var valor = redisDb.StringGet("1");
            Console.WriteLine(valor);

            //Memoization
            var watch = Stopwatch.StartNew();
            Func<long, long> factorial = null;
            factorial = n => n > 1 ? n * factorial(n - 1) : 1;

            for (int i = 0; i < 20000000; i++)
                factorial(9);
            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds);


            var watch2 = Stopwatch.StartNew();
            var factorial2 = factorial.Memoize();

            for (int i = 0; i < 20000000; i++)
                factorial2(9);
            watch2.Stop();
            Console.WriteLine(watch2.ElapsedMilliseconds);

        }

        public static Func<T, TResult> Memoize<T, TResult>(this Func<T, TResult> func)
        {
            var cache = new ConcurrentDictionary<T, TResult>();
            //Se crea un diccionario de funciones
            return (a) => cache.GetOrAdd(a, func);
        }

        static void Write (string content, FileStream fs)
        {
            lock (fs)
            {
                byte[] bContent = new UTF8Encoding(true).GetBytes(content + Environment.NewLine);
                fs.Write(bContent, 0, bContent.Length);
                fs.Flush();
            }
        }
    }

    public class RedisDB
    {
        //Para objetos grandes
        private static Lazy<ConnectionMultiplexer> _lazy;

        public static ConnectionMultiplexer Connection
        {
            get { return _lazy.Value; }
        }
        static RedisDB()
        {
            _lazy = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect("localhost"));
        }
    }
}
