using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace lecture_2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            listBoxMain.ItemsSource = ocStudents;
            listBoxMain.DisplayMemberPath = "displayMember";
        }

        private ObservableCollection<student> ocStudents = new ObservableCollection<student>();

        private void temp_Click(object sender, RoutedEventArgs e)
        {
            Random myRand = new Random();
            List<string> lstMaleNames = File.ReadAllLines("male_names.txt").ToList();
            List<string> lstFemaleNames = File.ReadAllLines("female_names.txt").ToList();
            for (int i = 0; i < 10; i++)
            {
                student randomStudent = new student();
                randomStudent.irStudentNumber = myRand.Next(10000000, 99999999);
                randomStudent.blMale = (myRand.Next(0, 2) == 0 ? true : false);
                //if (myRand.Next(0, 2) == 0)
                //    randomStudent.blMale = true;
                //else
                //    randomStudent.blMale = false;

                string srFirstName, srSurname;
                switch (randomStudent.blMale)
                {
                    case true:
                        srFirstName = lstMaleNames[myRand.Next(0, lstMaleNames.Count)];
                        srSurname = lstMaleNames[myRand.Next(0, lstMaleNames.Count)];
                        break;
                    case false:
                        srFirstName = lstFemaleNames[myRand.Next(0, lstFemaleNames.Count)];
                        srSurname = lstFemaleNames[myRand.Next(0, lstFemaleNames.Count)];
                        break;
                }
                randomStudent.srStudentName = srFirstName + " " + srSurname;
                randomStudent.srStudentEmail = Regex.Replace(randomStudent.srStudentName.Replace(" ", ""),
                    @"[^\u0020-\u007E]", string.Empty) + "@gmail.com";
                // randomStudent.displayMember = "gg"; read only

                ocStudents.Add(randomStudent);
            }
        }

        public class student
        {
            public int irStudentNumber { get; set; }
            public string srStudentName { get; set; }
            public string srStudentEmail { get; set; }
            public bool blMale { get; set; }

            public string displayMember//this is read only
            {
                get
                {
                    return $"Id: {this.irStudentNumber.ToString("N0")} \t\t Gender: {(this.blMale == true ? "Male" : "Female")} \t\t Name: {this.srStudentName} \t\t\t email: {this.srStudentEmail}";
                }
            }
        }

        private void listBoxMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MessageBox.Show(((student)listBoxMain.SelectedItem).irStudentNumber.ToString("N0"));
        }
    }
}
