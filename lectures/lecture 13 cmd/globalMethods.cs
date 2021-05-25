using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static lecture_13_cmd.Program;

namespace lecture_13_cmd
{
    public static class globalMethods
    {
        public static void WriteLine(this car c, string Addition = null)//this is method extension. it becomes extension with this keyword
        {
            Console.WriteLine(Addition + $"Car price: {c.Price.ToString("N0")} $ \t Car weight: {c.Weight.ToString("N0")} kg ");
        }

        public static car MultiplyCar(car c, car d)//the signature of first method is car and car
        {
            return new car { Price = c.Price * d.Price, Weight = c.Weight * d.Weight };
        }
        //this is method overloading
        public static car MultiplyCar(IEnumerable<car> list)//the signature of the second method is an ienumerable object
        {
            return new car { Price = list.Select(pr => pr.Price).Aggregate(1L,(x,y) => x*y),
                Weight = list.Select(pr => pr.Weight).Aggregate(1L, (x, y) => x * y)
            };
        }

    }
}
