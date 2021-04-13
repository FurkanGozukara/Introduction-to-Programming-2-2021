using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using lecture_6.CustomClasses;

namespace lecture_6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public MainWindow()
        {
            InitializeComponent();
            GlobalMethods.main = this;
            AppInit.initApp(this);
            GlobalMethods.startInfoOperations();
        }     

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            RegisterOperations.completeRegister(this);
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            LoginOperations.loginTry(this);
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            LoginOperations.tryLogout();
        }
    }
}
