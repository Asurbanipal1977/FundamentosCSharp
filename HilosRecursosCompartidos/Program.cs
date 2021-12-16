using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace HilosRecursosCompartidos
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Todos van al mismo fichero
            string path = "result.txt";
            var fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
            Parallel.For(1, 101, new ParallelOptions { MaxDegreeOfParallelism = 10 },
                (i) => Write(i.ToString(), fs));
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
}
