using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace lecture_10_cmd
{
    class Program
    {
        private static double dblCount = 0;
        private static long irLong = 0;

        static void Main(string[] args)
        {
            int[] a = new int[10];
            a = myMethod(2, 10);
            foreach (var item in a)
            {
                Console.WriteLine(item);
            }

            foreach (var item in methodYield(2, 10))
            {
                Console.WriteLine(item);
            }

            Console.Clear();

            increaseNumber();
            increaseNumber();

            Console.WriteLine("expected value is 20000, the value we got is: " + dblCount.ToString("N0"));

            List<Task> lstStartedTasks = new List<Task>();


            for (int i = 0; i < 20; i++)
            {
                //Thread race condition C#
                //A race condition occurs when two or more threads are able to access shared data and they try to change it at the same time.
                var vrTask = Task.Factory.StartNew(() => { increaseNumber(); });
                lstStartedTasks.Add(vrTask);
                //so lets say Thread 1 access the variable, read it as 2000, increase by 1 and save
                //so at the same time Thread 2 access the variable, read it as 2000, increase 1 and save
                //thread 1 saves first and it becomes 2001 but at the same time, thread 2 saves and it becomes 2001 again instead of becoming 2002
                //so how can we prevent that? we need to use lock
                //with lock only a single thread can access to that element at a time
            }

            //when we read the data here, the tasks are still being executed, therefore we are not getting the final value
            //so we need to be sure that tasks are completed
            Task.WaitAll(lstStartedTasks.ToArray());//now the application will wait all tasks to get finished
            //however this is still not preventing data race
            Console.WriteLine("expected value is 220000, the value we got is: " + dblCount.ToString("N0"));
            Console.WriteLine("expected value of long with Interlocked is 220000, the value we got is: " + irLong.ToString("N0"));
            dblCount = 0;

            for (int i = 0; i < 20; i++)
            {
                //this time we are calling Synchronization having method with using lock 
                var vrTask = Task.Factory.StartNew(() => { increaseNumberWithSynchronization(); });
                lstStartedTasks.Add(vrTask);
            }
            Task.WaitAll(lstStartedTasks.ToArray());//now the application will wait all tasks to get 
            Console.WriteLine("expected value is 200000, the value we got is: " + dblCount.ToString("N0"));
        }

        private static void increaseNumber()
        {
            for (int i = 0; i < 10000; i++)
            {
                dblCount++;
                Interlocked.Add(ref irLong, 1);//this is always thread-safe
            }
        }

        private static object _lockDouble = new object();

        private static void increaseNumberWithSynchronization()
        {
            for (int i = 0; i < 10000; i++)
            {
                lock (_lockDouble)
                {
                    dblCount++;
                }
            }
        }

        public static int[] myMethod(int irStart, int irNumber)
        {
            int[] _irArray = new int[irNumber];
            for (int i = 0; i < irNumber; i++)
            {
                _irArray[i] = irStart + 2 * i;
            }
            return _irArray;
        }

        public static IEnumerable methodYield(int irStart, int irNumber)
        {
            for (int i = 0; i < irNumber; i++)
            {
                yield return irStart + 2 * i;
            }
        }
    }
}
