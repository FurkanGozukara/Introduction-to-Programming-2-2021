using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace lecture_1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //What Are Access Modifiers In C# : https://www.c-sharpcorner.com/uploadfile/puranindia/what-are-access-modifiers-in-C-Sharp/

        //private means that it can be only acccesed by the methods under same parent class
        private static void method_1(int param1, double param2)
        {

        }

        //public - can be access by anyone anywhere.
        //private - can only be accessed from with in the class it is a part of.
        //protected - can only be accessed from with in the class or any object that inherits off of the class.
        protected static void method_2(int param1, double param2)
        {

        }

        public static void method_3(int param1, double param2)
        {

        }

        //to use non static methods, you have to have instance of the class that has that method
        public void method_4(int param1, double param2)//signature of this method is int, double
        {

        }

        class Car
        {

        }

        private Car method_4(double param2, int param1)//signature of this method is double, int
        {
            Car myCar = new Car();
            return myCar;
        }
        //if you define methods with same name but different signatures, it is called as method overloading

        //public int, double, string myAwesomeMethod()
        //{
        //this kind of return definition of methods are not possible
        //}

        //this is the first way of returning multiple different type of parameters from a method
        //https://www.tutorialsteacher.com/csharp/csharp-tuple
        public Tuple<int, double, string> myAwesomeMethod()
        {
            return new Tuple<int, double, string>(32, 120.2, "awesome");
        }

        public Tuple<string, int, double, string, int, int, List<string>, Tuple<string, string>> myAwesomeMethod2()
        {
            return new Tuple<string, int, double, string, int, int, List<string>, Tuple<string, string>>(

             "string1", 101, 321.543, "string2", 6, 65, new List<string> { "list1", "list2", "list3" }, new Tuple<string, string>("tuple X1", "tuple X2"));
        }

        public class ourGreatClass
        {
            private string _srCustomProperty; // field

            public string srCustomProperty   // property
            {
                get //basically we are overriding the get method
                {// get method
                    return DateTime.Now + " " + _srCustomProperty;
                }
                set //we are overriding the set method
                {
                    _srCustomProperty = value.ToUpper();
                }  // set method
            }

            public string srFirstString { get; set; }

            public string srSecondString;//by default null

            public int firstInteger;//by default 0

            public double firstDouble;//by default 0

            private int _secondInteger;
            public int secondInteger//https://stackoverflow.com/questions/295104/what-is-the-difference-between-a-field-and-a-property
            {
                get
                {// get method
                    return _secondInteger % 10;// remainder, or modulus, operator
                }
                set
                {
                    _secondInteger = value + 5;
                }  // set method }
            }

            public int thirdInteger { get; set; }

            public List<string> firstList;

            public Tuple<string, string> firstTuple;


            public List<int> secondList;


            private int _irVolume;
            public int irVolume//we will have input validation
            {
                get
                {
                    if (_irVolume > 100)
                        return 100;
                    if (_irVolume < 0)
                        return 0;
                    return _irVolume;
                }
                set
                {
                    _irVolume = value;
                }  // set method }
            }
        }

        private void tplExample_Click(object sender, RoutedEventArgs e)
        {
            var myAwesomeTupleMetodReturn = myAwesomeMethod();
            listBox1.Items.Add($"item 1: {myAwesomeTupleMetodReturn.Item1}, item 2: {myAwesomeTupleMetodReturn.Item2}, item 3: {myAwesomeTupleMetodReturn.Item3}");

            var myAwesomeTupleMetodReturn2 = myAwesomeMethod2();
            listBox1.Items.Add($"item 1: {myAwesomeTupleMetodReturn2.Item1}, item 2: {myAwesomeTupleMetodReturn2.Item2}, item 3: {myAwesomeTupleMetodReturn2.Item3}, item 4: {myAwesomeTupleMetodReturn2.Item4}, list element [2]: {myAwesomeTupleMetodReturn2.Item7[2]} , TRest[1]: {myAwesomeTupleMetodReturn2.Rest.Item2}");

            var myAwesomeTupleMetodReturn3 = myAwesomeMethod3();
            listBox1.Items.Add($"item 1: {(string)myAwesomeTupleMetodReturn3[0]}, item 2: {(int)myAwesomeTupleMetodReturn3[1]}, item 3: {(double)myAwesomeTupleMetodReturn3[2]}, item 4: {(string)myAwesomeTupleMetodReturn3[0]}, list element [2]: {((List<string>)myAwesomeTupleMetodReturn3[6])[2]} , TRest[1]: {((Tuple<string, string>)myAwesomeTupleMetodReturn3[7]).Item2}");

            var myGreatClass = returnGreatClass();

            listBox1.Items.Add($"second integer: {myGreatClass.secondInteger}, second object of first list: {myGreatClass.firstList.ElementAtOrDefault(1)}, eleventh object of first list: {myGreatClass.firstList.ElementAtOrDefault(10)},  second object of second list: {myGreatClass.secondList.ElementAtOrDefault(1)}, eleventh object of second list: {myGreatClass.secondList.ElementAtOrDefault(10)}");

            listBox1.Items.Add($"custom property string (set value is programming): {myGreatClass.srCustomProperty}");

            listBox1.Items.Add($"custom property int second integer (set value is 1000): {myGreatClass.secondInteger}");

            myGreatClass.irVolume = -321;//private value -321
            listBox1.Items.Add("volume: " + myGreatClass.irVolume);
            myGreatClass.irVolume = 55;//private value 55
            listBox1.Items.Add("volume: " + myGreatClass.irVolume);
            myGreatClass.irVolume = 1554;//private value 1554
            listBox1.Items.Add("volume: " + myGreatClass.irVolume);
            myGreatClass.irVolume += -1254;//private value -1154
            listBox1.Items.Add("volume: " + myGreatClass.irVolume);

            var myGreatClass2 = returnGreatClass();

            myGreatClass2.irVolume = 24;

            listBox1.Items.Add("total volume: " + myGreatClass.irVolume + myGreatClass2.irVolume);//output is 024

            listBox1.Items.Add("total volume: " + (myGreatClass.irVolume + myGreatClass2.irVolume));//output is 24

            //first way is equal to below way

            listBox1.Items.Add("total volume: " + myGreatClass.irVolume.ToString() + myGreatClass2.irVolume.ToString());//output is 024
        }

        private ourGreatClass returnGreatClass()
        {
            return new ourGreatClass { firstDouble = 321.32, firstInteger = 99, firstList = new List<string> { "great list 1", "great list 2", "etc." }, firstTuple = new Tuple<string, string>("tuple 1", "tuple 2"), secondInteger = 1000, srCustomProperty = "programming", srFirstString = "school", srSecondString = "second", thirdInteger = 432, secondList = new List<int> { 657, 12356 } };
        }

        public List<object> myAwesomeMethod3()
        {
            return new List<object> {
             "string1", 101, 321.543, "string2", 6, 65, new List<string> { "list1", "list2", "list3" }, new Tuple<string, string>("tuple X1", "tuple X2")};
        }
    }
}
