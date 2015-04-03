using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.Common;
using System.Reflection;
using System.Data.OleDb;

namespace Fixit //omSupportBackup
{
    class OMSQLDB
    {
        public SqlConnection _sqlConn { get; set; }
        public OleDbConnection _oleConn;
        public OMSQLDB()
        {
            GetServerAndInstanceNamesFromIniFile();
            //GetLocationFromFilePath();
        }        

        private string _clientDatabaseName = "";
        public string DatabaseName
            {
                get { return _clientDatabaseName; }
                set { _clientDatabaseName = value; }
            }

        private string _clientServerName = "";
        public string ServerName
        {
            get { return _clientServerName; }
            set { _clientServerName = value; }
        }

        private string _clientInstanceName = "";
        public string InstanceName
        {
            get { return _clientInstanceName; }
            set { _clientInstanceName = value; }
        }

        private string _clientDbFilePath = "";
        public string DbFilePath
        {
            get { return _clientDbFilePath; }
            set { _clientDbFilePath = value; }
        }


        
        private string _clientSharedDataPath = "";
        public string SharedDataPath
            {
                get { return _clientSharedDataPath; }
                set { _clientSharedDataPath = value; }
            }

        public string Omate32IniFile { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public bool _ConnectionSucceeded { get; set; }
        public string ConnectionString { get; set; }        


        private void GetServerAndInstanceNamesFromIniFile()
        {
            //todo: GetInstanceName
            int counter = 0;
            string line;


            // Read the file and display it line by line.
            System.IO.StreamReader file =
               new System.IO.StreamReader("c:\\windows\\omate32.ini");

            while ((line = file.ReadLine()) != null)
            {
                if (line.Contains("Databasename"))
                {
                    string[] words = line.Split('=');
                    foreach (string word in words)
                    {
                        this.DatabaseName = word;
                    }
                }

                if (line.Contains("DataSource"))
                {
                    string iniLine = line.Substring(11);
                    string[] words = iniLine.Split('\\');
                    this.ServerName = words[0];
                    // CW 2/4/2014 Some omate32.ini files don't have an instance resulting in an error
                    // when looking for the second part of the "datasource" line.                        
                    //if (words.Length > 1) this.InstanceName = words[1];
                    this.InstanceName = (words.Length > 1) ? words[1] : "";
                }

                counter++;
            }

        }

        //trying to add this method to replace the getshareddatapath method above.
        //this method is used throughout all the other classes, so easier to maintain.

        private void GetOmate32iniFileValues()
        {
            Workstation ws = new Workstation();
            _clientDatabaseName = ws.ClientDatabaseName;
            _clientServerName = ws.ClientServerName;
            _clientInstanceName = ws.ClientInstanceName;
            _clientDbFilePath = ws.ClientDbFilePath;
            _clientSharedDataPath = ws.SharedDataPath; //CW 2/3/2014 added to support logging the fix in a file in the data folder
        }

        public void BackupTheDatabase()
        {
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection();
            conn.ConnectionString =
                    "integrated security=true;Data Source=tcp:" +
                    this.ServerName +
                    "\\" +
                    this.InstanceName +
                    ";" +
                "user id=" +
                UserId +
                ";password=" +
                this.Password +
                ";" +
                "persist security info=False;database=" +
                this.DatabaseName;

            //WORK STRING

            //"integrated security=true;Data Source=tcp:WL93603V7\\OMSQL;" +
            //"user id=OM_USER;password=OMSQL@2004;" +
            //"persist security info=False;database=TESTDB";

            try
            {
                conn.Open();
                //SqlCommand command = new SqlCommand ();
                SqlCommand cmdGetDataFilePath = new SqlCommand();
                #region trying to get file path
                cmdGetDataFilePath.CommandText = "select physical_name from sys.database_files where type = 0";
                try
                {
                    string _DbFilePath = (string)cmdGetDataFilePath.ExecuteScalar();
                    Console.WriteLine(_DbFilePath);
                    string temppath = GetLocationFromFilePath(_DbFilePath);

                    //KEEP  command = new SqlCommand(@"backup database TestDB to disk ='C:\OFFICEMATE\DATA\OMSQLBkup\TestDB1.bak' with init,stats=10", conn);
                    SqlCommand command = new SqlCommand(@"backup database" + this.DatabaseName + @" to disk ='C:\Program Files\Microsoft SQL Server\MSSQL.1\MSSQL\DATA\OktoDeleteTrainingDB1.bak' with init,stats=10", conn);
                    command.ExecuteNonQuery();
                    conn.Close();


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    conn.Dispose();
                }
                #endregion


                //Not used check for existing bak files
                //var hasBak = Directory.GetFiles(@"C:\OFFICEMATE\DATA\OMSQLBkup\", "*.bak").Length > 0;

            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, ex.Number.ToString());
            }
        }

        private string GetLocationFromFilePath(string path)
        {
            //given a complete file path, return the file location only
            //this tells us where to save the .bak file, assumes the location has most hd space
            string[] words = path.Split('=');
            foreach (string word in words)
            {
                this.DatabaseName = word;
            }
            return path;
        }

        private string GetSharedDataPath(string path)
        {
            //given a complete file path, return the file location only
            //this tells us where to save the .bak file, assumes the location has most hd space
            string[] words = path.Split('=');
            foreach (string word in words)
            {
                this.DatabaseName = word;
            }
            return path;
        }

        public bool SqlClientCheck()
        {
            bool _hassqlclient = false;
            // Retrieve the installed providers and factories.
            DataTable table = DbProviderFactories.GetFactoryClasses();
            foreach (DataRow row in table.Rows)
            {
                Console.WriteLine (row[0]);
                if (row[0].ToString().Contains("SqlClient"))
                {
                    _hassqlclient = true;
                    return _hassqlclient;
                }
            }
            return _hassqlclient;
        }

        private bool HasSqlClient
        {
            get
            {
                return SqlClientCheck();
            }

        }

        private void GetSQLConnection() //sets and opens a sql connection for this class var
        {
            using (SqlConnection conn = new SqlConnection())
            {// When copying/pasting...note this uses slightly different vars below than in form1 originally
                conn.ConnectionString =
                    "integrated security=false;Data Source=tcp:" + ServerName + "\\" + InstanceName + ";" +
                    "user id=OM_USER;password=OMSQL@2004;" +
                    "persist security info=False;database=" + DatabaseName;
                try
                {
                    conn.Open();
                    //todo: Make sure this conn assignment maintains the dispose feature of USING 
                    _sqlConn = conn;
                }

                catch (SqlException ex)
                {
                    _ConnectionSucceeded = false;
                    MessageBox.Show(ex.Message, ex.Number.ToString());
                    string message = ex.ToString();
                    string caption = "Error";
                    var result = MessageBox.Show(message, caption,
                                                 MessageBoxButtons.OK,
                                                 MessageBoxIcon.Exclamation);
                }
            }
        
        }

        public OleDbConnection  GetOLEConnection() //sets and opens a sql connection for this class var
        { 
            //CW 07/29/2014 Removed the 'using' statement, it was preventing the con from passing as called.
            //Also added code to accomodate default sqlserver installations with no instance name.
            //This will accomodate many, many more cases that can run fixIt, as default installs are becoming the norm.

            OleDbConnection conn = new OleDbConnection();

                string ConnectionString_Has_InstanceName = "Provider = SQLOLEDB;" +
                "Driver=SQLOLEDB;" +
                "Data Source=tcp:" + ServerName + "\\" + InstanceName + ";" +
                "Initial Catalog=" + DatabaseName + ";" +
                "User id=OM_USER;" + ";" +
                "Password=OMSQL@2004;";

                 string ConnectionString_Default_NoInstanceName = "Provider = SQLOLEDB;" +
                "Driver=SQLOLEDB;" +
                "Data Source=tcp:" + ServerName + "\\" + InstanceName + ";" +
                "Initial Catalog=" + DatabaseName + ";" +
                "User id=OM_USER;" + ";" +
                "Password=OMSQL@2004;";

                ConnectionString = (InstanceName == "") ? ConnectionString_Default_NoInstanceName : ConnectionString_Has_InstanceName;
                conn.ConnectionString = ConnectionString;

                try
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                        //todo: Make sure this conn assignment maintains the dispose feature of USING 
                        return conn;

                    else
                    {
                        return null;
                    }
                }

                catch (SqlException ex)
                {
                    return null;
                    _ConnectionSucceeded = false;
                    MessageBox.Show(ex.Message, ex.Number.ToString());
                    string message = ex.ToString();
                    string caption = "Error";
                    var result = MessageBox.Show(message, caption,
                                                 MessageBoxButtons.OK,
                                                 MessageBoxIcon.Exclamation);
                }
            
            }
        
        }
      
}
