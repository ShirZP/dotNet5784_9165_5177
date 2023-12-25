using System;
namespace Stage0
{
    partial class Program
    {
        private static void Main(string[] args)
        {
            Welcome9165();
            Welcome5177();
            Console.ReadKey();
        }

        private static void Welcome9165()
        {
            Console.WriteLine("Enter your name: ");
            string userName = Console.ReadLine();
            Console.WriteLine("{0}, welcome to my first console application", userName);
        }
        static partial void Welcome5177();      
    }
}
