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
using System.Threading;
using System.Data.Entity;

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

            GlobalFunctions.logThread("MainWindow");
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
            Task.Factory.StartNew(() =>
            {
                addRecords();
            });
        }

        private void addRecords()
        {
            GlobalFunctions.logThread("btnInsertRandomStudents_Click");

            int irRowsInserted = 0;
            Random myRand = new Random();
            int irMax = 50000;
            for (int i = 0; i < irMax; i++)
            {
                string srBaseQuery = @$"insert into tblStudents   ([StudentId]
           ,[StudentName]
           ,[StudentEmail]
           ,[IsMale]
           ,[NickName]) values ({myRand.Next(1, 10000)},N'{RandomString(myRand.Next(3, 5))}',N'{RandomString(myRand.Next(3, 15))}@gmail.com',{myRand.Next(0, 2)},N'{RandomString(myRand.Next(8, 8))}');";

                using (SqlConnection connection = new SqlConnection(srConnectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(srBaseQuery, connection))
                    {
                        irRowsInserted += command.ExecuteNonQuery();//ExecuteNonQuery because we will not get any results , used for update delete and insert
                    }
                }

                if (i % 100 == 0)
                {
                    Dispatcher.BeginInvoke((Action)(() =>
                    {
                        updateLabel($"records added to database: {i} / {irMax}");
                    }));
                }

                csSQL_Lite_DB.parameterizedQuery newQuery = new csSQL_Lite_DB.parameterizedQuery();


                csSQL_Lite_DB.parameterizedQueryObj myobj = new csSQL_Lite_DB.parameterizedQueryObj
                {
                    srParameterName = "@StudentId",
                    typeofParameter = DbType.Int32,
                    valueofParameter = myRand.Next(1, 10000)
                };

                newQuery.listofParameters.Add(myobj);

                myobj = new csSQL_Lite_DB.parameterizedQueryObj
                {
                    srParameterName = "@StudentName",
                    typeofParameter = DbType.String,
                    valueofParameter = RandomString(myRand.Next(3, 5))
                };

                newQuery.listofParameters.Add(myobj);

                myobj = new csSQL_Lite_DB.parameterizedQueryObj
                {
                    srParameterName = "@StudentEmail",
                    typeofParameter = DbType.String,
                    valueofParameter = RandomString(myRand.Next(3, 15))
                };

                newQuery.listofParameters.Add(myobj);

                myobj = new csSQL_Lite_DB.parameterizedQueryObj
                {
                    srParameterName = "@IsMale",
                    typeofParameter = DbType.Binary,
                    valueofParameter = myRand.Next(0, 2)
                };

                newQuery.listofParameters.Add(myobj);

                myobj = new csSQL_Lite_DB.parameterizedQueryObj
                {
                    srParameterName = "@NickName",
                    typeofParameter = DbType.String,
                    valueofParameter = RandomString(myRand.Next(8, 8))
                };

                newQuery.listofParameters.Add(myobj);

                newQuery.srQuery = "insert into tblStudents values (@StudentId,@StudentName,@StudentEmail,@IsMale,@NickName)";

                csSQL_Lite_DB.db_Parameterized_Update_Delete_Insert(newQuery);

            }
            updateLabel($"records added to database: {irMax} / {irMax}");
            MessageBox.Show($"{irRowsInserted.ToString("N0")} student records added to the database ");
        }

        private void updateLabel(string srMessage)
        {
            Dispatcher.BeginInvoke((Action)(() =>
            {
                lblCounter.Content = srMessage;
            }));
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

            string srBaseQuery = $"update tblStudents set {cmbColumnNames.SelectedItem.ToString()} = N'{txtNewValue.Text.Replace("'", "''")}'";

            if (txtPredicate.Text != "predicate")
                srBaseQuery += " " + txtPredicate.Text;

            MessageBox.Show("affected number of rows: " + db_updateDeleteInsert(srBaseQuery));
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
            System.IO.File.AppendAllText("sql_queries.txt", DateTime.Now + "\r\n" + srBaseQuery + Environment.NewLine);

            if (dtRecords.Tables.Count > 0)
                foreach (DataRow drw in dtRecords.Tables[0].Rows)
                {
                    addRecordToListBox(drw, listBox2, ds: dtRecords);//we are providing references (address of the values) of these objects to the new method call addRecordToListBox
                }
        }

        private void addRecordToListBox(DataRow drw, ListBox lstBox, DataSet ds = null, DataTable dt = null)
        {
            DataColumnCollection varColumns;

            if (ds == null)
                varColumns = dt.Columns;
            else
                varColumns = ds.Tables[0].Columns;

            StringBuilder srBuild = new StringBuilder();
            foreach (var vrColumn in varColumns)
            {
                srBuild.Append(vrColumn.ToString() + " = " + drw[vrColumn.ToString()].ToString() + "\t");
            }
            lstBox.Items.Add(srBuild.ToString());
        }

        //the prevention of SQL injection is made with parameterized queries in C#

        private void btnListStudentsByName_Click(object sender, RoutedEventArgs e)
        {
            listBox3.Items.Clear();
            parameterizedSecureListStudents_v2();
        }

        private void SQLInjectionVulnerableListStudents()
        {

            var vrQuery = $"select * from tblStudents where StudentName like N'%{txtStudentName.Text}%'";
            DataTable dtResult = csDbConnection.db_Select_DataTable(vrQuery);

            if (dtResult != null)
                foreach (DataRow drw in dtResult.Rows)
                {
                    addRecordToListBox(drw, listBox3, dt: dtResult);
                }
        }

        private void parameterizedSecureListStudents()
        {
            DataTable dtResult = new DataTable();

            using (SqlConnection connection = new SqlConnection(srConnectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = $"select * from tblStudents where StudentName like @StudentName";
                    cmd.Connection = connection;
                    //cmd.Parameters.AddWithValue("@StudentName", txtStudentName.Text); you can also define values like this but interpretation is left to SQL server
                    cmd.Parameters.Add("@StudentName", SqlDbType.VarChar).Value = "%" + txtStudentName.Text + "%";
                    using (SqlDataAdapter sqlAdapt = new SqlDataAdapter(cmd))
                    {
                        sqlAdapt.Fill(dtResult);
                    }
                }
            }

            if (dtResult != null)
                foreach (DataRow drw in dtResult.Rows)
                {
                    addRecordToListBox(drw, listBox3, dt: dtResult);
                }
        }

        private void parameterizedSecureListStudents_v2()
        {
            parameterizedQuery paramQuery = new parameterizedQuery();
            paramQuery.srQuery = $"select * from tblStudents where StudentName like @StudentName";
            parameterizedQueryObj vrObject = new parameterizedQueryObj
            {
                srParameterName = "@StudentName",
                typeofParameter = SqlDbType.VarChar,
                valueofParameter = "%" + txtStudentName.Text + "%"
            };
            paramQuery.listofParameters.Add(vrObject);

            DataTable dtResult = db_Parameterized_Select_DataTable(paramQuery);

            if (dtResult != null)
                foreach (DataRow drw in dtResult.Rows)
                {
                    addRecordToListBox(drw, listBox3, dt: dtResult);
                }
        }

        private void btnListStudentsByNameSafe_Copy_Click(object sender, RoutedEventArgs e)
        {
            listBox3.Items.Clear();
            SQLInjectionVulnerableListStudents();
        }

        private void btnSecureUpdate_Click(object sender, RoutedEventArgs e)
        {
            parameterizedQuery paramQuery = new parameterizedQuery();
            paramQuery.srQuery = $"update tblStudents set StudentName=StudentName+@addition";
            parameterizedQueryObj vrObject = new parameterizedQueryObj
            {
                srParameterName = "@addition",
                typeofParameter = SqlDbType.VarChar,
                valueofParameter = txtStudentName.Text
            };
            paramQuery.listofParameters.Add(vrObject);
            var vrRowsCount = db_Parameterized_Update_Delete_Insert(paramQuery);
            MessageBox.Show("number of rows updated: " + vrRowsCount);
        }

        private void btnDeleteUnsecureById_Click(object sender, RoutedEventArgs e)
        {
            var vrRowsCount = db_updateDeleteInsert($"  delete from tblStudents where StudentId={txtStudentName.Text}");
            MessageBox.Show("number of rows delete: " + vrRowsCount);
        }

        private void btnComposeTable_Click(object sender, RoutedEventArgs e)
        {
            var vrREsult = csSQL_Lite_DB.db_updateDeleteInsert(System.IO.File.ReadAllText("compose_sqlite_table.sql"));
            MessageBox.Show(vrREsult.ToString());
        }

        SchoolContext myContext = new SchoolContext();

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            // dtGridStudents.ItemsSource = selectDataSet("  select top 100 * from tblStudents order by StudentId asc").Tables[0].AsDataView();

            myContext = new SchoolContext();
            myContext.TblStudents.OrderBy(pr => pr.UniqueId).Take(100).Load();
            dtGridStudents.ItemsSource = myContext.TblStudents.Local.ToBindingList();
            dtGridStudents.Items.Refresh();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            myContext.SaveChanges();
        }
    }
}

