using System;

namespace AuthTesting
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }

        public static string Something()
        {
            return "Algo";
        }

        public static bool Login (string user, string password) =>
            user=="miguel" && password=="12345" ? true : false;
    }
}
