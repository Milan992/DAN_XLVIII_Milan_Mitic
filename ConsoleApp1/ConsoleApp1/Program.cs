using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static CountdownEvent countdown = new CountdownEvent(6);
        static void Main(string[] args)
        {
            new Thread(SaySomething).Start("i am one");
            new Thread(SaySomething).Start("i am dva");
            new Thread(SaySomething).Start("i am tri");
            new Thread(SaySomething).Start("i am cet");
            new Thread(SaySomething).Start("i am Pet");
            new Thread(SaySomething).Start("i am sest");

            countdown.Wait();
            Console.WriteLine("gotovo");
            Console.ReadKey();
        }

        private static void SaySomething(object thing)
        {
            Console.WriteLine("thing");
           countdown.Signal();
        }
    }
}
