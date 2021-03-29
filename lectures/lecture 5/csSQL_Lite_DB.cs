using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SQLite;
using System.IO;

namespace lecture_3
{
    public static class csSQL_Lite_DB
    {
        private static readonly string _dbFileName="lecture5.sqlite";
        static csSQL_Lite_DB()
        {
            if(!File.Exists(_dbFileName))
            {
                SQLiteConnection.CreateFile(_dbFileName);
            }
        }

        public static readonly string srConnectionString = $"Data Source={_dbFileName};Version=3;";
        public static DataSet selectDataSet(string srQuery)
        {
            DataSet dSet = new DataSet();
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(srConnectionString))
                {
                    connection.Open();
                    using (SQLiteDataAdapter DA = new SQLiteDataAdapter(srQuery, connection))
                    {
                        DA.Fill(dSet);
                    }
                }
            }
            catch (Exception E)
            {
                //log exception E
            }

            return dSet;
        }

        public static int db_updateDeleteInsert(string srCommand)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(srConnectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(srCommand, connection))
                    {
                        return command.ExecuteNonQuery();//ExecuteNonQuery because we will not get any results , used for update delete and insert
                    }
                }
            }
            catch (Exception E)
            {
                //log exception E
            }
            return 0;
        }

        public static DataTable db_Select_DataTable(string strQuery, bool blsetCommit = false)
        {
            //System.IO.File.AppendAllText(@"C:\temp\dbcon.txt", strQuery + "\r\n\r\n");

            DataTable dt = new DataTable();
            if (strQuery.Length < 5)
                return dt;

            if (blsetCommit == true)
            {
                strQuery = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED " + Environment.NewLine + " " + strQuery;
            }

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(srConnectionString))
                {
                    connection.Open();
                    using (SQLiteDataAdapter DA = new SQLiteDataAdapter(strQuery, connection))
                    {
                        DA.Fill(dt);
                    }
                }
                return dt;
            }
            catch (Exception E)
            {
                //insertIntoTblSqlErrors(strQuery + " " + E.Message.ToString());
                return dt;
            }
        }

        public static DataRow db_Select_DataRow(string strQuery, bool blsetCommit = false)
        {
            //System.IO.File.AppendAllText(@"C:\temp\dbcon.txt", strQuery + "\r\n\r\n");

            DataRow drw = null;
            if (strQuery.Length < 5)
                return drw;

            if (blsetCommit == true)
            {
                strQuery = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED " + Environment.NewLine + " " + strQuery;
            }

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(srConnectionString))
                {
                    connection.Open();
                    using (SQLiteDataAdapter DA = new SQLiteDataAdapter(strQuery, connection))
                    {
                        using (DataTable drTemp = new DataTable())
                        {
                            DA.Fill(0, 1, drTemp);
                            if (drTemp.Rows.Count > 0)
                                drw = drTemp.Rows[0];
                        }
                    }
                }
                return drw;
            }
            catch (Exception E)
            {
                // insertIntoTblSqlErrors(strQuery + " " + E.Message.ToString());
                return drw;
            }
        }

        public class parameterizedQueryObj
        {
            public string srParameterName { get; set; }
            public DbType typeofParameter { get; set; }
            public object valueofParameter { get; set; }

        }

        public class parameterizedQuery
        {
            public List<parameterizedQueryObj> listofParameters = new List<parameterizedQueryObj>();
            public string srQuery { get; set; }
        }

        public static DataTable db_Parameterized_Select_DataTable(parameterizedQuery queryObject, bool blsetCommit = false)
        {
            DataTable dtResult = new DataTable();
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(srConnectionString))
                {
                    connection.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand())
                    {
                        cmd.CommandText = queryObject.srQuery;
                        cmd.Connection = connection;
                        foreach (var vrObject in queryObject.listofParameters)
                        {
                            SQLiteParameter myParam = new SQLiteParameter(vrObject.srParameterName, vrObject.valueofParameter);
                            cmd.Parameters.Add(myParam);
                        }

                        using (SQLiteDataAdapter sqlAdapt = new SQLiteDataAdapter(cmd))
                        {
                            sqlAdapt.Fill(dtResult);
                        }
                    }
                }
            }
            catch (Exception E)
            {
                // insertIntoTblSqlErrors(strQuery + " " + E.Message.ToString());             
            }

            return dtResult;
        }

        public static int db_Parameterized_Update_Delete_Insert(parameterizedQuery queryObject, bool blsetCommit = false)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(srConnectionString))
                {
                    connection.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand())
                    {
                        cmd.CommandText = queryObject.srQuery;
                        cmd.Connection = connection;
                        foreach (var vrObject in queryObject.listofParameters)
                        {
                            SQLiteParameter myParam = new SQLiteParameter(vrObject.srParameterName, vrObject.valueofParameter);
                            cmd.Parameters.Add(myParam);
                        }
                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception E)
            {
                // insertIntoTblSqlErrors(strQuery + " " + E.Message.ToString());    
                return -1;
            }
        }
    }
}

