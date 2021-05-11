using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace lecture_11_cmd
{
    class Program
    {
        static void Main(string[] args)
        {
            Debug.WriteLine("main thread id " + Thread.CurrentThread.ManagedThreadId);

            List<int> lstNumbersToIterate = new List<int>();
            for (int i = 0; i < 1000000; i++)
            {
                lstNumbersToIterate.Add(i);
            }

            ParallelOptions paralelOp = new ParallelOptions();
            paralelOp.MaxDegreeOfParallelism = 10;
            //since we have 1m items and maximum 8 threads, the .net has split our list into 8 parts starting from 0, 125k, 250k, 375k, 500k, ..
            //Parallel.ForEach(lstNumbersToIterate, paralelOp, number => {

            //    //Debug.WriteLine("each iteration inside parallel foreach scope thread id " + Thread.CurrentThread.ManagedThreadId); each one runs in a task not in the main thread

            //    Console.WriteLine(number);

            //});


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

            //int lastNumber = 1, count = 0;
            //Parallel.For(0, 1000000, (i, loop) =>
            //{
            //    // Get the next number
            //    int curNumber = new Random().Next();

            //    // If it’s the same as the previous number, 
            //    // note the increased count
            //    if (curNumber == lastNumber) count++;

            //    // Otherwise, we have a different number than
            //    // the previous. Write out the previous number
            //    // and its count, then start
            //    // over with the new number.
            //    else
            //    {
            //        if (count > 0)
            //            Console.WriteLine(
            //                count + ": " + lastNumber);
            //        lastNumber = curNumber;

            //    }
            //});

            Stopwatch sw;
            const int NUM = 100000;

            StreamWriter swWrite = new StreamWriter("classic non locking random generator.txt");
            swWrite.AutoFlush = true;

            paralelOp.MaxDegreeOfParallelism = 100;

            List<int> lstGeneratedNumbers = new List<int>();

            while (true)
            {
                sw = Stopwatch.StartNew();
                Parallel.For(0, NUM, paralelOp, i =>
                {
                    lstGeneratedNumbers.Add(RandomGen0.Next());
                });
                Console.WriteLine(sw.Elapsed);


                File.WriteAllLines("0 static non locking random generator.txt", lstGeneratedNumbers.Select(p => p.ToString()));
                lstGeneratedNumbers = new List<int>();

                sw = Stopwatch.StartNew();
                var vrResult = Parallel.For(0, NUM, paralelOp, i =>
                  {
                      lstGeneratedNumbers.Add(RandomGen1.Next());
                  });
                Console.WriteLine(sw.Elapsed);

                

                File.WriteAllLines("1 thread static random generator.txt", lstGeneratedNumbers.Select(p => p.ToString()));
                lstGeneratedNumbers = new List<int>();

                sw = Stopwatch.StartNew();
                Parallel.For(0, NUM, paralelOp, i =>
                {
                    lstGeneratedNumbers.Add(RandomGen2.Next());
                });
                Console.WriteLine(sw.Elapsed);


                File.WriteAllLines("2 thread static and RNGCryptoServiceProvider random generator.txt", lstGeneratedNumbers.Select(p => p.ToString()));
                lstGeneratedNumbers = new List<int>();

                sw = Stopwatch.StartNew();
                Parallel.For(0, NUM, paralelOp, i =>
                {
                    lstGeneratedNumbers.Add(RandomGen3.Next());
                });
                Console.WriteLine(sw.Elapsed);


                File.WriteAllLines("3 thread local and RNGCryptoServiceProvider random generator.txt", lstGeneratedNumbers.Select(p => p.ToString()));
                lstGeneratedNumbers = new List<int>();

                sw = Stopwatch.StartNew();
                Parallel.For(0, NUM, paralelOp, i =>
                {
                    lstGeneratedNumbers.Add(RandomGen4.Next());
                });
                Console.WriteLine(sw.Elapsed);

            
                lstGeneratedNumbers = new List<int>();

                sw = Stopwatch.StartNew();
                Parallel.For(0, NUM, paralelOp, i =>
                {
                    lstGeneratedNumbers.Add(threadSafeRandomWithCrypto(0,Int32.MaxValue));
                });
                Console.WriteLine(sw.Elapsed);

                File.WriteAllLines("5 thread safe random generator.txt", lstGeneratedNumbers.Select(p => p.ToString()));

                swWrite.Flush();

                Console.ReadLine();
            }
        }



        public static class RandomGen3
        {
            private static RNGCryptoServiceProvider _global =
                new RNGCryptoServiceProvider();
            [ThreadStatic]
            private static Random _local;

            public static int Next()
            {
                Random inst = _local;
                if (inst == null)
                {
                    byte[] buffer = new byte[4];
                    _global.GetBytes(buffer);
                    _local = inst = new Random(
                        BitConverter.ToInt32(buffer, 0));
                }
                return inst.Next();
            }
        }

        public static class RandomGen4
        {
            private static RNGCryptoServiceProvider _global =
                new RNGCryptoServiceProvider();

            private static ThreadLocal<Random> _local = new ThreadLocal<Random>(() =>
            {
                byte[] buffer = new byte[4];
                _global.GetBytes(buffer);
                return new Random(
                     BitConverter.ToInt32(buffer, 0));
            });

            public static int Next()
            {
                Random inst = _local.Value;
                return inst.Next();
            }
        }

        public static class RandomGen0
        {
            private static Random _inst = new Random();

            public static int Next()
            {
                return _inst.Next();
            }
        }

        public static class RandomGen1
        {
            private static Random _inst = new Random();

            public static int Next()
            {
                lock (_inst) return _inst.Next();
            }
        }

        public static class RandomGen2
        {
            private static Random _global = new Random();
            [ThreadStatic]
            private static Random _local;

            public static int Next()
            {
                Random inst = _local;
                if (inst == null)
                {
                    int seed;
                    lock (_global) seed = _global.Next();
                    _local = inst = new Random(seed);
                }
                return inst.Next();
            }
        }

        private static RNGCryptoServiceProvider _rng  =
             new RNGCryptoServiceProvider();

        private static int threadSafeRandomWithCrypto(Int32 minValue, Int32 maxValue)
        {
            if (minValue > maxValue)
                throw new ArgumentOutOfRangeException("minValue");
            if (minValue == maxValue) return minValue;
            Int64 diff = maxValue - minValue;
            byte[] _uint32Buffer = new byte[4];
            while (true)
            {
                _rng.GetBytes(_uint32Buffer);
                UInt32 rand = BitConverter.ToUInt32(_uint32Buffer, 0);

                Int64 max = (1 + (Int64)UInt32.MaxValue);
                Int64 remainder = max % diff;
                if (rand < max - remainder)
                {
                    return (Int32)(minValue + (rand % diff));
                }
            }
        }
    }
}
