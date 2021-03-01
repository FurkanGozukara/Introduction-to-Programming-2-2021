using System;
using System.Collections.Generic;
using System.Text;

namespace lecture_1
{
    class instance_example
    {

        private int method_11()
        {
            MainWindow.method_3(0, 0);

            MainWindow myAnotherWindow = new MainWindow();
            //now i have an instance of mainwindow class
            //you have to have instances of non-static classes and methods to be able to use them
            myAnotherWindow.method_4(0, 0);
           

            return 0;
        }

    }
}
