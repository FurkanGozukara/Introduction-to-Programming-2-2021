using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace lecture_12_cmd
{
    class Program
    {
        //ctrl + m + o keys on keyboard : closes all regions
        //ctrl + m + l keys on keyboard : opens all regions
        //ctrl + k + d keys on keyboard : organize the code

        public class Base { }

        public class Derived : Base { }

        static void Main(string[] args)
        {

            object ab = 32.1;//double
            object ad = 1;//this is integer

            Console.WriteLine(ab is double);  // output: True
            Console.WriteLine(ab is int);  // output: False

            Console.WriteLine(ad is int);  // output: True
            Console.WriteLine(ab is double); // output: false

            Console.WriteLine("\n");

            object b = new Base();
            Console.WriteLine(b is Base);  // output: True
            Console.WriteLine(b is Derived);  // output: False

            object d = new Derived();
            Console.WriteLine(d is Base);  // output: True
            Console.WriteLine(d is Derived); // output: True

            Console.WriteLine("\n");

            int i = 27;
            Console.WriteLine(i is System.IFormattable);  // output: True

            object iBoxed = i;//when you turn something into an object class instance it is boxing
            Console.WriteLine(iBoxed is int);  // output: True
            Console.WriteLine(iBoxed is long);  // output: False

            Console.WriteLine("\n");

            i = 23;
            iBoxed = i;
            int? jNullable = 7;
            if (iBoxed is int newint1 && jNullable is int newint2)
            {
                Console.WriteLine(newint1 + newint2);  // output 30
            }

            if (iBoxed is double newint_1 && jNullable is int newint_2)
            {
                Console.WriteLine(newint_1 + newint_2);  // this wont print since this if statement will fail
            }

            Console.WriteLine("\n");

            IEnumerable<int> numbers = new[] { 10, 20, 30 };
            IList<int> indexable = numbers as IList<int>;
            if (indexable != null)
            {
                Console.WriteLine(indexable[0] + indexable[indexable.Count - 1]);  // output: 40
            }

            double x = 1234.7;
            int a = (int)x;
            Console.WriteLine(a);   // output: 1234,

            x = 1234324324234.7;
            a = (int)x;
            Console.WriteLine(a);   // output: -2147483648

            numbers = new int[] { 10, 20, 30 };
            IList<int> list = (IList<int>)numbers;
            Console.WriteLine(list.Count);  // output: 3
            Console.WriteLine(list[1]);  // output: 20

            Console.WriteLine("\n");

            void PrintType<T>() => Console.WriteLine(typeof(T));//this is inline method

            Console.WriteLine(typeof(List<string>));
            PrintType<int>();
            PrintType<System.Int32>();
            PrintType<Dictionary<int, char>>();

            b = new Giraffe();
            Console.WriteLine(b is Animal);  // output: True
            Console.WriteLine(b.GetType() == typeof(Animal));  // output: False

            Console.WriteLine(b is Giraffe);  // output: True
            Console.WriteLine(b.GetType() == typeof(Giraffe));  // output: True

            double d1 = 3215324324.123123;
            long l1 = 432543534534543543;
            List<string> list111 = new List<string> { "323", "1" };//this is managed type. 


            Console.WriteLine(sizeof(byte));  // output: 1
            Console.WriteLine(sizeof(double));  // output: 8


            List<customClassSizeTest> lstTest = new List<customClassSizeTest>();

            var mem1 = GC.GetTotalMemory(true);
       
            for (int mm = 0; mm < 400; mm++)
            {
                lstTest.Add(new customClassSizeTest(1, 3215315, 54654654654 , 32));
            }
         
            var mem2 = GC.GetTotalMemory(true);

            Console.WriteLine((mem2 - mem1) + $" bytes , avg {((mem2 - mem1)/400).ToString("N0")} is the size of customClassSizeTest");  // doesnt work




        }



        public class customClassSizeTest
        {
            public customClassSizeTest(byte tag, double x, double y, int ir)//constructor
            {
                Tag = tag;
                X = x;
                Y = y;
                iry1 = ir;
            }

            public byte Tag { get; }
            public double X { get; }
            public double Y { get; }

            public int iry1 { get; set; }
        }


        public class Animal { }

        public class Giraffe : Animal { }

        private static void lecturePart4()
        {



            //negation operator
            bool passed = false;
            Console.WriteLine(!passed);  // output: True
            Console.WriteLine(!true);    // output: False
            Console.WriteLine("\n");

            bool SecondOperand()
            {
                Console.WriteLine("Second operand is evaluated.");
                return true;
            }

            bool a = false & SecondOperand();
            Console.WriteLine(a);
            // Output:
            // Second operand is evaluated.
            // False

            bool b = true & SecondOperand();
            Console.WriteLine(b);
            // Output:
            // Second operand is evaluated.
            // True
            Console.WriteLine("\n");
            Console.WriteLine(true ^ true);    // output: False
            Console.WriteLine(true ^ false);   // output: True
            Console.WriteLine(false ^ true);   // output: True
            Console.WriteLine(false ^ false);  // output: False

            Console.WriteLine("\n");

            a = true | SecondOperand();
            Console.WriteLine(a);
            // Output:
            // Second operand is evaluated.
            // True

            b = false | SecondOperand();
            Console.WriteLine(b);
            // Output:
            // Second operand is evaluated.
            // True

            //The & operator evaluates both operands even if the left-hand operand evaluates to false
            //The conditional logical AND operator &&, also known as the "short-circuiting" logical AND operator, computes the logical AND of its operands. The result of x && y is true if both x and y evaluate to true. Otherwise, the result is false. If x evaluates to false, y is not evaluated.
            Console.Clear();
            a = false && SecondOperand();//here this SecondOperand method call is not executed since first operand is false
            Console.WriteLine(a);
            // Output:
            // False

            b = true && SecondOperand();
            Console.WriteLine(b);
            // Output:
            // Second operand is evaluated.
            // True

            //The conditional logical OR operator ||, also known as the "short-circuiting" logical OR operator, computes the logical OR of its operands. The result of x || y is true if either x or y evaluates to true. Otherwise, the result is false. If x evaluates to true, y is not evaluated.
            //The | operator evaluates both operands even if the left-hand operand evaluates to true, so that the operation result is true regardless of the value of the right-hand operand.

            a = true || SecondOperand();//this will not execute SecondOperand since it will be always true
            Console.WriteLine(a);
            // Output:
            // True

            b = false || SecondOperand();
            Console.WriteLine(b);
            // Output:
            // Second operand is evaluated.
            // True
        }

        private static void lecturePart3()
        {

            int[] xs = new[] { 0, 10, 20, 30, 40 };
            int last = xs[^1]; //this means lenght - 1 since array index start from 0 it is the last element of the array
            var test = xs[0..^0]; //this means lenght - 1 since array index start from 0 it is the last element of the array
            Console.WriteLine(last);  // output: 40

            var lines = new List<string> { "one", "two", "three", "four" };
            string prelast = lines[^2];
            Console.WriteLine(prelast);  // output: three

            string word = "Twenty";
            Index toFirst = ^word.Length;
            char first = word[toFirst];
            Console.WriteLine(first);  // output: T



            int[] numbers = Enumerable.Range(0, 100).ToArray();
            int x = 12;
            int y = 25;
            int z = 36;

            Console.WriteLine($"{numbers[^x]} is the same as {numbers[numbers.Length - x]}");
            Console.WriteLine($"{numbers[x..y].Length} is the same as {y - x}");

            Console.WriteLine("numbers[x..y] and numbers[y..z] are consecutive and disjoint:");
            Span<int> x_y = numbers[x..y];//x and u are inclusive
            Span<int> y_z = numbers[y..z];
            Console.WriteLine("0th element is: " + numbers[0]);
            y_z[0] = 100;//this will not affect the original array
            Console.WriteLine("0th element is: " + numbers[0]);
            List<int> x_yv2 = numbers[x..y].ToList();//x and u are inclusive
            Console.WriteLine($"\tnumbers[x..y] is {x_y[0]} through {x_y[^1]}, numbers[y..z] is {y_z[0]} through {y_z[^1]}");

            Console.WriteLine("numbers[x..^x] removes x elements at each end:");
            Span<int> x_x = numbers[x..^x];
            Console.WriteLine($"\tnumbers[x..^x] starts with {x_x[0]} and ends with {x_x[^1]}");

            Console.WriteLine("numbers[..x] means numbers[0..x] and numbers[x..] means numbers[x..^0]");
            Span<int> start_x = numbers[..x];
            Span<int> zero_x = numbers[0..x];
            Console.WriteLine($"\t{start_x[0]}..{start_x[^1]} is the same as {zero_x[0]}..{zero_x[^1]}");
            Span<int> z_end = numbers[z..];
            Span<int> z_zero = numbers[z..^0];
            Console.WriteLine($"\t{z_end[0]}..{z_end[^1]} is the same as {z_zero[0]}..{z_zero[^1]}");


            var jagged = new int[10][]
{
   new int[10] { 0,  1, 2, 3, 4, 5, 6, 7, 8, 9},
   new int[10] { 10,11,12,13,14,15,16,17,18,19},
   new int[10] { 20,21,22,23,24,25,26,27,28,29},
   new int[10] { 30,31,32,33,34,35,36,37,38,39},
   new int[10] { 40,41,42,43,44,45,46,47,48,49},
   new int[10] { 50,51,52,53,54,55,56,57,58,59},
   new int[10] { 60,61,62,63,64,65,66,67,68,69},
   new int[10] { 70,71,72,73,74,75,76,77,78,79},
   new int[10] { 80,81,82,83,84,85,86,87,88,89},
   new int[10] { 90,91,92,93,94,95,96,97,98,99},
};

            var selectedRows = jagged[3..^0];

            foreach (var row in selectedRows)
            {
                var selectedColumns = row[2..^2];
                foreach (var cell in selectedColumns)
                {
                    Console.Write($"{cell}, ");
                }
                Console.WriteLine();
            }

            //            a.. is equivalent to a..^0
            //..b is equivalent to 0..b
            //.. is equivalent to 0..^0


            int[] numbers2 = new[] { 0, 10, 20, 30, 40, 50 };
            int amountToDrop = numbers2.Length / 2;

            int[] rightHalf = numbers2[amountToDrop..];
            Display(rightHalf);  // output: 30 40 50

            int[] leftHalf = numbers2[..^amountToDrop];
            Display(leftHalf);  // output: 0 10 20

            int[] all = numbers2[..];
            Display(all);  // output: 0 10 20 30 40 50

            //this a local function
            void Display<T>(IEnumerable<T> xs) => Console.WriteLine(string.Join(" ", xs));
        }

        private static void lecturePart2()
        {
            string srName = null;

            //The conditional operator ?
            Console.WriteLine(((srName == null) ? "no name is set yet" : srName));

            srName = "Furkan";

            Console.WriteLine(srName == null ? "no name is set yet" : srName);

            List<int> numbers = null;
            int? a = null;

            (numbers ??= new List<int>()).Add(5);
            Console.WriteLine(string.Join(" ", numbers));  // output: 5

            numbers.Add(a ??= 0);
            Console.WriteLine(string.Join(" ", numbers));  // output: 5 0
            Console.WriteLine(a);  // output: 0

            (numbers ??= new List<int>()).Add(6);
            Console.WriteLine(string.Join(" ", numbers));  // output: 5

            numbers.Add(a ??= 1);
            Console.WriteLine(string.Join(" ", numbers));  // output: 5 0
            Console.WriteLine(a);  // output: 0

            a = 12;
            Console.WriteLine(a ?? 1);//here it will write 12
            a = null;
            Console.WriteLine(a ?? 1);//here it will write 1
            a = 5;
            Console.WriteLine(a ?? 3);//here it will write 5

            //* The null-coalescing operators are right-associative. That is, expressions of the form d ??= e ??= f : d ??= (e ??= f)



            var sum1 = SumNumbers(null, 0);
            Console.WriteLine(sum1);  // output: NaN

            var numberSets = new List<double[]>
{
    new[] { 1.0, 2.0, 3.0 },
    null,
       new[] { 11.0, 12.0, 13.0 },
};

            var sum2 = SumNumbers(numberSets, 0);
            Console.WriteLine(sum2);  // output: 6

            var sum3 = SumNumbers(numberSets, 1);
            Console.WriteLine(sum3);  // output: NaN

            var sum4 = SumNumbers(numberSets, 2);
            Console.WriteLine(sum4);  // output: 36

            var sum5 = SumNumbers(numberSets, 3);
            Console.WriteLine(sum5);  // output: NaN


            Console.WriteLine("\n\n");

            sum2 = SumNumbers2(numberSets, 0);
            Console.WriteLine(sum2);  // output: 6

            sum3 = SumNumbers2(numberSets, 1);
            Console.WriteLine(sum3);  // output: NaN

            sum4 = SumNumbers2(numberSets, 2);
            Console.WriteLine(sum4);  // output: 36

            sum5 = SumNumbers2(numberSets, 3);
            Console.WriteLine(sum5);  // output: NaN
        }

        private static double SumNumbers(List<double[]> setsOfNumbers, int indexOfSetToSum)
        {
            return setsOfNumbers?.ElementAtOrDefault(indexOfSetToSum)?.Sum() ?? double.NaN;
        }

        private static double SumNumbers2(List<double[]> setsOfNumbers, int indexOfSetToSum)
        {
            if (setsOfNumbers == null)
                return double.NaN;

            if (setsOfNumbers.Count <= indexOfSetToSum)
                return double.NaN;

            if (setsOfNumbers[indexOfSetToSum] == null)
                return double.NaN;

            return setsOfNumbers[indexOfSetToSum].Sum();
        }

        private static void lecturePart1()
        {
            string path = @"c:\users\public\test.txt";
            System.IO.StreamReader file = null;
            char[] buffer = new char[10];
            try
            {
                file = new System.IO.StreamReader(path);
                file.ReadBlock(buffer, 0, buffer.Length);
            }
            catch (System.IO.IOException e)
            {
                Console.WriteLine("Error reading from {0}. Message = {1}", path, e.Message);
            }
            finally
            {
                if (file != null)
                {
                    file.Close();
                }
            }


            file = null;
            buffer = new char[10];
            try
            {
                file = new System.IO.StreamReader("a.txt");
                file.ReadBlock(buffer, 0, buffer.Length);
            }
            catch (System.IO.IOException e)
            {
                Console.WriteLine("Error reading from {0}. Message = {1}", path, e.Message);
            }
            finally
            {
                if (file != null)
                {
                    file.Close();
                }
            }

            //alternatively you can use using block. even if an error happens inside using block, all elements will get disposed off

            using (file = new System.IO.StreamReader("a.txt"))
            {
                try
                {
                    throw (new Exception("test"));
                }
                catch (Exception)
                {

                }
            }

            tryToConvert("a");
            tryToConvert("3435436547654556356564");

            Console.Clear();

            initTestItems();

            while (true)
            {
                Console.WriteLine("enter 2 values to look whether exists in the database or not");
                var v1 = Console.ReadLine();
                var v2 = Console.ReadLine();
                csPerEvent csTempEvent = new csPerEvent
                {
                    dtDate = Convert.ToDateTime(v2.ToString()),
                    srPersonName = v1
                };
                //A tick represents one hundred nanoseconds or one ten-millionth of a second
                bool blExists = false;
                Stopwatch sw = new Stopwatch();
                sw.Start();
                if (dicEvents.ContainsKey(csTempEvent))
                {
                    blExists = true;
                }
                sw.Stop();
                Console.WriteLine("dictionary lookup took: " + sw.ElapsedTicks.ToString("N2") + " ticks");
                sw.Restart();
                if (listEvents.Where(pr => pr.Item1 == csTempEvent).Any() == true)
                {
                    blExists = true;
                }
                sw.Stop();
                Console.WriteLine("list lookup took: " + sw.ElapsedTicks.ToString("N2") + " tick");
            }
        }

        public static Dictionary<csPerEvent, csPerEventDetails> dicEvents = new Dictionary<csPerEvent, csPerEventDetails>();

        public static List<Tuple<csPerEvent, csPerEventDetails>> listEvents = new List<Tuple<csPerEvent, csPerEventDetails>>();

        private static void initTestItems()
        {
            Random myRand = new Random();
            List<string> lstPersonNames = File.ReadAllLines("names_list.txt").ToList();

            Stopwatch watchDictionary = new Stopwatch();
            Stopwatch watchList = new Stopwatch();

            for (int i = 0; i < 10000; i++)
            {
                int irYear = myRand.Next(1, 2100);
                int irMonth = myRand.Next(1, 13);
                int irDay = myRand.Next(1, 29);

                csPerEvent tempClass = new csPerEvent
                {
                    dtDate = new DateTime(irYear, irMonth, irDay),
                    srPersonName = lstPersonNames[myRand.Next(0, 2000)]
                };

                csPerEventDetails tempDetails = new csPerEventDetails
                {
                    irEventId = myRand.Next(),
                    srEventDescription = myRand.Next().ToString("N0")
                };

                watchDictionary.Start();

                if (dicEvents.ContainsKey(tempClass) == false)
                {
                    dicEvents.Add(tempClass, tempDetails);
                }

                watchDictionary.Stop();

                watchList.Start();

                if (listEvents.Where(pr => pr.Item1 == tempClass).Any() == false)
                {
                    listEvents.Add(new Tuple<csPerEvent, csPerEventDetails>(tempClass, tempDetails));
                }

                watchList.Stop();

                if (i % 100 == 0)//modulus operator , e.g. 5% 100 = 5, 250%100 = 50, 10321 % 1000 = 321
                    Console.WriteLine("init: " + i.ToString("N0"));
            }

            Console.WriteLine("dictionary init time: " + watchDictionary.ElapsedMilliseconds.ToString("N2") + " ms");
            Console.WriteLine("list init time: " + watchList.ElapsedMilliseconds.ToString("N2") + " ms");
        }

        public class csPerEvent
        {
            public string srPersonName { get; set; }
            public DateTime dtDate { get; set; }
        }

        public class csPerEventDetails
        {
            public int irEventId { get; set; }
            public string srEventDescription { get; set; }
        }


        private static void tryToConvert(string srVal)
        {
            int irVal = 0;
            try
            {
                irVal = Convert.ToInt32(srVal);
            }
            catch (FormatException)
            {
                Console.WriteLine("you have provided an invalid format to convert to integer");
            }
            catch (OverflowException)
            {
                Console.WriteLine("you have provided a numeric value too big for integer");
            }

            irVal += Int32.MaxValue;
            irVal += Int32.MaxValue;//this doesnt throw any error however your number becomes something negative
        }
    }
}
