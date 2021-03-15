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
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using static lecture_3.csDbConnection;

namespace lecture_3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            string srBaseQuery = "select top 1 * from tblStudents";
            DataSet dSet = selectDataSet(srBaseQuery);
            cmbColumnNames.Items.Add("All");
            cmbColumnNames.SelectedIndex = 0;
            foreach (DataTable dt in dSet.Tables)
            {
                foreach (var vrColumn in dt.Columns)
                {
                    cmbColumnNames.Items.Add(vrColumn.ToString());
                }
            }
        }

        private void btnDeleteAllStudents_Click(object sender, RoutedEventArgs e)
        {
            string srCommand = "delete from tblStudents";

            SqlConnection connection = new SqlConnection(srConnectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(srCommand, connection);
            int irRowsDeleted = command.ExecuteNonQuery();//ExecuteNonQuery because we will not get any results , used for update delete and insert
            MessageBox.Show($"deleted number of students from the table {irRowsDeleted.ToString("N0")}");
        }

        private void testConnectionLeaveOpen()
        {
            string srCommand = "delete from tblStudents";
            SqlConnection connection = new SqlConnection(srConnectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(srCommand, connection);
            int irRowsDeleted = command.ExecuteNonQuery();//ExecuteNonQuery because we will not get any results , used for update delete and insert       
        }

        private void testConnectionOpenClose()//this is the proper way with using encapsulation
        {
            string srCommand = "delete from tblStudents";
            //whatever you define inside the scope of using statment will be immediately disposed of when using statment ends
            using (SqlConnection connection = new SqlConnection(srConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(srCommand, connection))
                {
                    int irRowsDeleted = command.ExecuteNonQuery();//ExecuteNonQuery because we will not get any results , used for update delete and insert
                }
            }
        }

        SqlConnection connectionPermanent = new SqlConnection(srConnectionString);

        private void testSingleConnection()
        {
            if (connectionPermanent.State != ConnectionState.Open)
            {
                connectionPermanent = new SqlConnection(srConnectionString);
                connectionPermanent.Open();
            }

            string srCommand = "delete from tblStudents";
            SqlCommand command = new SqlCommand(srCommand, connectionPermanent);
            int irRowsDeleted = command.ExecuteNonQuery();//ExecuteNonQuery because we will not get any results , used for update delete and insert
        }

        private void btnTestNotClose_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch watchTimerOpenClose = new Stopwatch();
            Stopwatch watchTimerPermanentConnection = new Stopwatch();

            for (int cc = 0; cc < 5; cc++)
            {
                watchTimerOpenClose.Start();
                for (int i = 0; i < 10000; i++)
                {
                    testConnectionOpenClose();
                }
                watchTimerOpenClose.Stop();

                watchTimerPermanentConnection.Start();
                for (int i = 0; i < 10000; i++)
                {
                    testSingleConnection();
                }
                watchTimerPermanentConnection.Stop();
            }

            MessageBox.Show($"open close approach total time: {watchTimerOpenClose.ElapsedMilliseconds.ToString("N0")} ms \n\n permanent connection approach total time: {watchTimerPermanentConnection.ElapsedMilliseconds.ToString("N0")}");
        }

        private void btnInsertRandomStudents_Click(object sender, RoutedEventArgs e)
        {
            int irRowsInserted = 0;
            Random myRand = new Random();
            for (int i = 0; i < 50000; i++)
            {
                string srBaseQuery = @$"insert into tblStudents   ([StudentId]
           ,[StudentName]
           ,[StudentEmail]
           ,[IsMale]
           ,[NickName]) values ({myRand.Next(1,10000)},N'{RandomString(myRand.Next(3, 5))}',N'{RandomString(myRand.Next(3, 15))}@gmail.com',{myRand.Next(0, 2)},N'{RandomString(myRand.Next(8, 8))}');";

                using (SqlConnection connection = new SqlConnection(srConnectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(srBaseQuery, connection))
                    {
                        irRowsInserted += command.ExecuteNonQuery();//ExecuteNonQuery because we will not get any results , used for update delete and insert
                    }
                }
            }
            MessageBox.Show($"{irRowsInserted.ToString("N0")} student records added to the database ");
        }


        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789ŞÜĞÇ";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void btnListStudents_Click(object sender, RoutedEventArgs e)
        {
            string srBaseQuery = "select top 100 * from tblStudents; select top 10 * from tblStudents";
            DataSet dSet = new DataSet();
            using (SqlConnection connection = new SqlConnection(srConnectionString))
            {
                connection.Open();
                using (SqlDataAdapter DA = new SqlDataAdapter(srBaseQuery, connection))
                {
                    DA.Fill(dSet);
                }
            }

            foreach (DataTable myTable in dSet.Tables)
            {
                for (int i = 0; i < myTable.Rows.Count; i++)
                {
                    lstBox1.Items.Add($"student id: {myTable.Rows[i]["StudentId"].ToString()} , student name: {myTable.Rows[1]["StudentId"].ToString()}");
                }
            }

            for (int i = 0; i < dSet.Tables.Count; i++)
            {
                for (int mm = 0; mm < dSet.Tables[i].Rows.Count; mm++)
                {
                    lstBox1.Items.Add($"email : {dSet.Tables[i].Rows[mm]["StudentEmail"].ToString()}");
                }
            }

            foreach (DataTable myTable in dSet.Tables)
            {
                foreach (DataRow drw in myTable.Rows)
                {
                    lstBox1.Items.Add($"nick name : {drw["NickName"].ToString()}");
                }
            }

            foreach (DataRow drw in dSet.Tables[1].Rows)
            {
                lstBox1.Items.Add($"is male : {drw["IsMale"].ToString()}");
            }

        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (cmbColumnNames.SelectedIndex < 1)
                return;

            string srBaseQuery = $"update tblStudents set {cmbColumnNames.SelectedItem.ToString()} = N'{txtNewValue.Text.Replace("'","''")}'";

            if (txtPredicate.Text != "predicate")
                srBaseQuery += " " + txtPredicate.Text;

            MessageBox.Show("affected number of rows: " + updateDeleteInsert(srBaseQuery));
        }

        private void listAllRecords_Click(object sender, RoutedEventArgs e)
        {
            listBox2.Items.Clear();
            string srBaseQuery = "select top 500 * from tblStudents";

            if (cmbColumnNames.SelectedIndex > 0)
                srBaseQuery = $"select  top 500 {cmbColumnNames.SelectedItem.ToString()} from tblStudents";

            if (txtPredicate.Text != "predicate")
                srBaseQuery += " " + txtPredicate.Text;

            if (txtOrder.Text != "order command")
                srBaseQuery += " " + txtOrder.Text;

            DataSet dtRecords = selectDataSet(srBaseQuery);
            System.IO.File.AppendAllText("sql_queries.txt",DateTime.Now + "\r\n" + srBaseQuery + Environment.NewLine);

            if (dtRecords.Tables.Count > 0)
                foreach (DataRow drw in dtRecords.Tables[0].Rows)
                {
                    StringBuilder srBuild = new StringBuilder();
                    foreach (var vrColumn in dtRecords.Tables[0].Columns)
                    {
                        srBuild.Append(vrColumn.ToString() + " = " + drw[vrColumn.ToString()].ToString() + "\t");
                    }
                    listBox2.Items.Add(srBuild.ToString());
                }
        }
    }
}

