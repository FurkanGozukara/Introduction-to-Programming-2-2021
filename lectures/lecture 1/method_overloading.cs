using System;
using System.Collections.Generic;
using System.Text;

namespace lecture_1
{
    public static class method_overloading
    {
        //https://www.geeksforgeeks.org/c-sharp-method-overloading/
        //what matter is with method overloading, input types and input types order
        public static double add(double x)//signature is double
        {
            return x + x;
        }

        public static double add(double x, int y)//signature is double + int
        {
            return x + y * 3;
        }

        public static int add(int x, int y)//signature is int + int
        {
            return x + y + y;
        }

        public static double add(int y, double x)//signature is int + double
        {
            return x * x + y;
        }

        internal static double add(double y, double x)//signature is int + double
        {
            return x * x + y * y;
        }
    }
}
