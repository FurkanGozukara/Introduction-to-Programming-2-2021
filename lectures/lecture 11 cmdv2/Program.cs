using System;
using System.Threading.Tasks;

namespace lecture_11_cmdv2
{
    class Program
    {
        static void Main(string[] args)
        {
            Random rand = new Random();

            Parallel.For(0, 1000000, (i, loop) =>
            {
                if (rand.Next() == 0) loop.Stop();
            });

            while (true)
            {
                Console.WriteLine(rand.Next());
                Console.ReadLine();
            }
        }
    }
}
