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
using lecture_9.CustomClasses;

namespace lecture_9
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

        private void btnRefreshDrugs_Click(object sender, RoutedEventArgs e)
        {
            LoggedInOperations.refreshDrugsDataGrid();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            LoggedInOperations.saveDrugChanges();
        }

        private void btnDeleteSelectedDrug_Click(object sender, RoutedEventArgs e)
        {
            LoggedInOperations.deleteSelectedDrug();
        }

        private void btnLoadDrugData_Click(object sender, RoutedEventArgs e)
        {
            LoggedInOperations.loadReadyDrugData();
        }

        private void cmbSortingDrugs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoggedInOperations.refreshDrugsDataGrid();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            LoggedInOperations.refreshDrugsDataGrid();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (tabDrugs.IsSelected)
                {
                    LoggedInOperations.refreshDrugsDataGrid();
                }
                if (tabLogin.IsSelected)
                {
                    LoginOperations.loginTry(this);
                }
                if (tabRegister.IsSelected)
                {
                    RegisterOperations.completeRegister(this);
                }
            }
        }
    }
}
