using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lecture_13_cmd
{
    public class Program
    {
        public class csTemp
        {
            public int MyProperty { get; set; }
            public double MyProperty2 { get; set; }

            ~csTemp()//this is a finalizer or as known as destructor
            {
                Console.WriteLine("this cs temp object is being garbage collected");
            }
        }

        static void Main(string[] args)
        {
            //part1();
            //part2();
            //part3();
            //part4();
            //part5();
            //part6();

            Parallel.For(1, 100, index =>
            {
                ProcessBusinessLogic bl = new ProcessBusinessLogic();
                bl.ProcessCompleted += Bl_ProcessCompleted;//we dont provide any parameters here because they are coming from          ProcessCompleted?.Invoke(irId);
                bl.StartProcess(index);

            });
        }

        private static void Bl_ProcessCompleted(int irId)
        {
            Console.WriteLine("Process Id: " + irId + "\t\t" + DateTime.Now + "\t\tthe ProcessBusinessLogic notified the subscribers : Process Completed!");
        }

        public delegate void Notify(int irId);  // delegate

        public class ProcessBusinessLogic
        {
            public Student myStudent;

            public int IdOfEvent { get; set; }
            public event Notify ProcessCompleted; // event

            public void StartProcess(int irId)
            {
                Random myRand = new Random();
                Console.WriteLine("Process Id: " + irId + "\t\tProcess Started!");
                System.Threading.Thread.Sleep(myRand.Next(1000, 100000));
                this.IdOfEvent = irId;
                OnProcessCompleted(irId);
            }

            protected virtual void OnProcessCompleted(int irId) //protected virtual method
            {
                //if ProcessCompleted is not null then call delegate
                ProcessCompleted?.Invoke(irId);
            }
        }



        static void part6()
        {

            parameterizedMethod(1);

            parameterizedMethod(1, 0.3);
            parameterizedMethod(32, -453.453, 56567687563735675);
            parameterizedMethod(32F, 453.453f, 56567687563735675L, 3423, 35, 332, 56, 657, 34, 42, 3212);

            TakeValue(10);
            TakeValue(10, 30f);
            TakeValue(10, 30f, 40.3);
            TakeValue(10, 30f, 40.3, "Yusuf");
            TakeValue(10, 30.435346f, 40.3, "Yusuf", 3M);
        }

        static void TakeValue(params object[] incomingValues)
        {
            foreach (object value in incomingValues)
            {
                Console.WriteLine("Value: {0} - Type: {1}", value, value.GetType().Name);
            }
            Console.WriteLine();
        }

        private static void parameterizedMethod(params object[] list)
        {
            double sum = 0;

            double dblTemp = 0;

            foreach (var vrItem in list)
            {
                double.TryParse(vrItem.ToString(), out dblTemp);
                sum += dblTemp;
            }

            Console.WriteLine(sum.ToString("N5"));

        }

        private static void part5()
        {

            DataStore<string> store = new DataStore<string>();
            store.Data = "such example";
            Console.WriteLine(store.Data);

            //store.Data = 43242; you cant convert type to other types once assigned. compile time error

            DataStore<string> strStore = new DataStore<string>();
            strStore.Data = "Hello World!";
            //strStore.Data = 123; // compile-time error

            DataStore<int> intStore = new DataStore<int>();
            intStore.Data = 100;
            //intStore.Data = "Hello World!"; // compile-time error

            KeyValuePair<int, string> kvp1 = new KeyValuePair<int, string>();
            kvp1.Key = 100;
            kvp1.Value = "Hundred";

            KeyValuePair<string, string> kvp2 = new KeyValuePair<string, string>();
            kvp2.Key = "IT";
            kvp2.Value = "Information Technology";

            DataStore<long> ldData = new DataStore<long>
            {
                Data = 2312,
                DataArray = new long[] { 3213, 12312, 532432 }
            };

            foreach (var item in ldData.DataArray)
            {
                Console.WriteLine(item);
            }

            ldData.AddOrUpdate(3, 321531532L);//this will call first AddOrUpdate

            ldData.AddOrUpdate(2321L, 12312L);//this will call second AddOrUpdate

            ldData.AddOrUpdate<double>(2321L, 3321.12);//this will call third AddOrUpdate

            var vrdata = ldData.GetData(3);

            Console.WriteLine(vrdata);


            Printer printer = new Printer();
            printer.Print<int>(100);
            printer.Print(200); // type infer from the specified value
            printer.Print<string>("Hello");
            printer.Print("World!"); // type infer from the specified value

            DataStore_v2<ConsoleColor> mycolor = new DataStore_v2<ConsoleColor>();
            mycolor.Data = ConsoleColor.Red;

            // DataStore_v2<string> mycolor2 = new DataStore_v2<string>(); this will throw compile time error

            DataStore_v3<string> dt3 = new DataStore_v3<string>();

            // DataStore_v3<int> dt4 = new DataStore_v3<int>(); this will throw compile time error

            DataStore_v3<int[]> dt5 = new DataStore_v3<int[]>();

            DataStore_v3<Dictionary<string, string>> dt6 = new DataStore_v3<Dictionary<string, string>>();

            TestStringEquality();
        }

        public static void OpEqualsTest<T>(T s, T t) where T : class
        {
            System.Console.WriteLine(s == t);
        }

        private static void TestStringEquality()
        {
            string s1 = "target";
            System.Text.StringBuilder sb = new System.Text.StringBuilder("target");
            string s2 = sb.ToString();
            OpEqualsTest<string>(s1, s2);
        }

        class DataStore_v2<T> where T : System.Enum
        {
            public T Data { get; set; }

        }

        class DataStore_v3<T> where T : IEnumerable
        {
            public T Data { get; set; }

        }


        class DataStore<T>
        {
            public T Data { get; set; }

            public T[] DataArray = new T[10];

            private T[] _data = new T[10];

            public void AddOrUpdate(int index, T item)
            {
                Console.WriteLine("first AddOrUpdate");
                if (index >= 0 && index < 10)
                    _data[index] = item;
            }

            public T GetData(int index)
            {
                if (index >= 0 && index < 10)
                    return _data[index];
                else
                    return default(T);
            }
            public void AddOrUpdate(T data1, T data2) { Console.WriteLine("second AddOrUpdate"); }
            public void AddOrUpdate<U>(T data1, U data2) { Console.WriteLine("third AddOrUpdate"); }
            public void AddOrUpdate(T data) { }

        }

        class Printer
        {
            public void Print<T>(T data)
            {
                Console.WriteLine(data);
            }
        }

        class KeyValuePair<TKey, TValue>
        {
            public TKey Key { get; set; }
            public TValue Value { get; set; }
        }

        static void part4()
        {
            dynamic MyDynamicVar = 4534534;

            Console.WriteLine(MyDynamicVar.GetType() + "\t" + MyDynamicVar);

            dynamic MyDynamicVar2 = 156476746766L;

            Console.WriteLine(MyDynamicVar2.GetType() + "\t" + MyDynamicVar2);

            MyDynamicVar = 100;
            Console.WriteLine("Value: {0}, Type: {1}", MyDynamicVar, MyDynamicVar.GetType());

            MyDynamicVar = "Hello World!!";
            Console.WriteLine("Value: {0}, Type: {1}", MyDynamicVar, MyDynamicVar.GetType());

            MyDynamicVar = true;
            Console.WriteLine("Value: {0}, Type: {1}", MyDynamicVar, MyDynamicVar.GetType());

            MyDynamicVar = DateTime.Now;
            Console.WriteLine("Value: {0}, Type: {1}", MyDynamicVar, MyDynamicVar.GetType());

            dynamic stud = new Studentx();

            //stud.DisplayStudentInfo(1, "Bill");// run-time error, no compile-time error
            //stud.DisplayStudentInfo("1");// run-time error, no compile-time error
            //stud.FakeMethod();// run-time error, no compile-time error
        }

        public class Studentx
        {
            public void DisplayStudentInfo(int id)
            {
            }
        }

        private static void part3()
        {




            var student = new { Id = 1.5, FirstName = "James", LastName = "Bond" };
            Console.WriteLine(student.Id); //output: 1
            Console.WriteLine(student.FirstName); //output: James
            Console.WriteLine(student.LastName); //output: Bond

            //Anonymous objects are ready only
            //student.Id = 2;//Error: cannot chage value
            //student.FirstName = "Steve";//Error: cannot chage value

            //nested Anonymous type object
            var student2 = new
            {
                Id = 1,
                FirstName = "James",
                LastName = "Bond",
                Address = new { Id = 1, City = "London", Country = "UK" }
            };

            Console.WriteLine(student2.Address.City);

            IList<Student> studentList = new List<Student>() {
            new Student() { StudentID = 1, StudentName = "John", age = 18 },
            new Student() { StudentID = 2, StudentName = "Steve",  age = 21 },
            new Student() { StudentID = 3, StudentName = "Bill",  age = 18 },
            new Student() { StudentID = 4, StudentName = "Ram" , age = 20  },
            new Student() { StudentID = 5, StudentName = "Ron" , age = 21 }
        };
            //Anonymous type objects are most commonly used to select a subset of a collection like below
            var students = from s in studentList
                           select new { Id = s.StudentID, Name = s.StudentName };

            foreach (var stud in students)
                Console.WriteLine(stud.Id + "-" + stud.Name);

            IList<Student> studentList2 = studentList;//this simply copy the pointer of studentList into the studentList2 object. so they point to the same values. whatever change we made on values, will be reflected to the both objects beacuse they point to the same values. they are just memory addreses 

            studentList2[2].age = 100;

            Console.WriteLine($"studentList index 2 age = {studentList[2].age} and studentList2 index 2 age = {studentList2[2].age}");

            stStudent st_student_1 = new stStudent
            {
                irAge = 21,
                srName = "Test Student"
            };

            stStudent st_student_2 = st_student_1;//this is copying values since it is struct type. on classes it just copies reference not the values

            st_student_2.irAge = 100;

            Console.WriteLine($"st_student_1 age = {st_student_1.irAge} and st_student_2 age = {st_student_2.irAge}");





            for (int i = 0; i < 999999; i++)
            {
                csTemp gg = new csTemp { MyProperty = 3213, MyProperty2 = 12312 };
            }
        }

        public struct stStudent
        {
            public int irAge { get; set; }
            public string srName { get; set; }

            public stStudent(int ir1, string sr2)
            {
                irAge = ir1;
                srName = sr2;
            }
        }



        public class Student
        {
            public int StudentID { get; set; }
            public string StudentName { get; set; }
            public int age { get; set; }
        }

        static void part2()
        {

            car car1 = new car { Price = 2, Weight = 3 };
            car car2 = new car { Price = 3, Weight = 4 };

            car car3 = car1 + car2;//Operator Overloading of + operator

            car3.WriteLine();

            var vrCar1 = globalMethods.MultiplyCar(car1, car2);
            var vrCar2 = globalMethods.MultiplyCar(new List<car> { vrCar1, vrCar1, vrCar1 });
            Console.WriteLine("");

            vrCar1.WriteLine("vrCar1: ");
            vrCar2.WriteLine("vrCar2: ");
            Console.WriteLine("\ncar class print");
            vrCar2.printCar();

            trucks t = new trucks(vrCar2);
            Console.WriteLine("\ntruck class print");
            t.printCar();
        }

        public class car
        {
            public long Weight { get; set; }
            public long Price { get; set; }

            public static car operator +(car b, car c)
            {
                car d = new car();
                d.Price = b.Price + c.Price;
                d.Weight = b.Weight + c.Weight;
                return d;
            }

            public virtual void printCar()
            {
                Console.WriteLine($"car price: {Price} \t\t car weight: {Weight}");
            }
        }

        public class trucks : car
        {
            public trucks(car d)
            {
                this.Price = d.Price;
                this.Weight = d.Weight;
            }

            public override void printCar()
            {
                stStudent gg = new stStudent();
                Console.WriteLine($"Car Price: {Price.ToString("N0")} \t\t Car Weight: {Weight.ToString("N0")}");
            }
        }


        static void part1()
        {
            Hashtable ht = new Hashtable();

            ht.Add("001", "Zara Ali");
            ht.Add("002", "Abida Rehman");
            ht.Add("003", "Joe Holzner");
            ht.Add("004", "Mausam Benazir Nur");
            ht.Add("005", "M. Amlan");
            ht.Add("006", "M. Arif");
            ht.Add("007", "Ritesh Saikia");

            if (ht.ContainsValue("Nuha Ali"))
            {
                Console.WriteLine("This student name is already in the list");
            }
            else
            {
                ht.Add("008", "Nuha Ali");
            }

            // Get a collection of the keys.
            ICollection key = ht.Keys;


            csTemp firstInstance = new csTemp { MyProperty = 232321, MyProperty2 = 43534534.43543 };

            ht.Add("anything", firstInstance);

            foreach (string k in key)
            {
                Console.WriteLine(k + ": " + (ht[k] is csTemp ? ((csTemp)(ht[k])).MyProperty2 : ht[k]));
            }

            Console.WriteLine(ht["fdgfd"] == null ? "this values does not exists" : ht["fdgfd"]);

            Dictionary<string, csTemp> dicTemp = new Dictionary<string, csTemp>();

            dicTemp.Add("anything", firstInstance);


            Queue<string> callerIds = new Queue<string>();
            callerIds.Enqueue("entry 1");
            callerIds.Enqueue("entry 2");
            callerIds.Enqueue("entry 3");
            callerIds.Enqueue("entry 4");

            foreach (var id in callerIds)
                Console.WriteLine(id); //prints 1234

            Queue<string> strQ = new Queue<string>();
            strQ.Enqueue("H");
            strQ.Enqueue("e");
            strQ.Enqueue("l");
            strQ.Enqueue("l");
            strQ.Enqueue("o");

            Console.WriteLine("Total elements: {0}", strQ.Count); //prints 5

            while (strQ.Count > 0)
                Console.WriteLine(strQ.Dequeue()); //prints Hello

            Console.WriteLine("Total elements: {0}", strQ.Count); //prints 0

            strQ = new Queue<string>();
            strQ.Enqueue("H");
            strQ.Enqueue("e");
            strQ.Enqueue("l");
            strQ.Enqueue("l");
            strQ.Enqueue("o");

            Console.WriteLine("Total elements: {0}", strQ.Count); //prints 5

            if (strQ.Count > 0)
            {
                Console.WriteLine(strQ.Peek()); //prints H
                Console.WriteLine(strQ.Peek()); //prints H
            }



            Console.WriteLine("Total elements: {0}", strQ.Count); //prints 0
        }


    }
}
