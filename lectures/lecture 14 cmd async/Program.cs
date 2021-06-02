using System;
using System.Threading.Tasks;

namespace lecture_14_cmd_async
{
    class Program
    {
        static void Main(string[] args)
        {
            //part1();
            callMethod();
            Console.ReadKey();
        }

        public static async void callMethod()
        {
            Console.WriteLine("starting task Method11");
            Task<int> task = Method11();
            Console.WriteLine("started task Method11");
            Console.WriteLine("starting synchronous Method21");
            Method21();
            Console.WriteLine("started synchronous Method21");
        
            Console.WriteLine("starting to await Method11");
            int count = await task;
            Console.WriteLine("awaiting Method11 finished");
            Console.WriteLine("starting synchronous Method31");
            Method31(count);
            Console.WriteLine("started synchronous Method31");
        }

        public static async Task<int> Method11()
        {
            int count = 0;
            await Task.Run(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    Console.WriteLine(" Method 11");
                    count += 1;
                }
            });
            return count;
        }

        public static void Method21()
        {
            for (int i = 0; i < 25; i++)
            {
                Console.WriteLine(" Method 21");
            }
        }

        public static void Method31(int count)
        {
            Console.WriteLine("Total count is " + count);
        }

        public static void part1()
        {
            Method1();
            Method2();
            Console.ReadKey();
        }

        public static async Task Method1()
        {
            await Task.Run(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    Console.WriteLine(" Method 1");
                    // Do something
                    Task.Delay(100).Wait();
                }
            });

            Console.WriteLine("method 1 completed");
        }


        public static void Method2()
        {
            for (int i = 0; i < 25; i++)
            {
                Console.WriteLine(" Method 2");
                // Do something
                Task.Delay(100).Wait();
            }

            Console.WriteLine("method 2 completed");
        }
    }
}
