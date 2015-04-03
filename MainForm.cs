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
using System.Threading;
using Microsoft.SqlServer.Management.Smo;
using System.Data.OleDb;
using System.Data.Common;
using SendKeys;
using System.Diagnostics;
using System.Reflection;



namespace Fixit  //omSupportBackup
{
    public partial class MainForm : Form
    {

        string _clientDatabaseName;
        string _clientServerName;
        string _clientInstanceName;
        string _clientDbFilePath;
        string _BakFileSuffix;
        bool _backupSucceeded;
        bool _ConnectionSucceeded;
        Solutions sols = new Solutions(); //this class contains the sql for solutions and catchers

        delegate void SetTextCallback(string text);

        private readonly BackgroundWorker _bw = new BackgroundWorker();
        public MainForm()
        {
            InitializeComponent();
            PopulateBkUpScreen();

            //Extract the embedded ibal.exe and utilityprep.exefile to the client desktop
            //when this form closes, the files will be deleted.
            Workstation ws = new Workstation();
            ws.Extract_Utilityprep_To_Desktop();
            ws.Extract_iBal_To_Desktop();

            //launch utility prep for backing up the db
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            System.Diagnostics.Process.Start(path + @"\Utilityprep.exe");
           
            
            //Now called from the classe's constructor
            //sols.PopulateCaseCatcherDict();
            sols.PopulateComboBoxOfCaseCatcher(cboCaseCatchers);

            //sols.PopulateSolutionsDict();
            sols.PopulateComboBoxOfSolutions(cboInsuranceChoices);

            // set MarqueeAnimationSpeed
            progressBar.MarqueeAnimationSpeed = 60;

            // set Visible false before you start long running task
            //progressBar.Visible = false;
            _bw.WorkerReportsProgress = true;
            _bw.DoWork += CreateBackupFile;
            _bw.RunWorkerCompleted += BwRunWorkerCompleted;
            _bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);

            //hide the catch(grpbxCatchIt) & fix(grpbxFixIt) panel until the backup completes, then the backup panel is hidden.
          //  this.Size = new Size(this.Size.Width, this.Size.Height - 140);

            //using a separate app to backup so display the find and fix panel
            DisplayCatchFixOnly();

            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            string myText = " " + version.Major + "." + version.Minor + "." + version.Build; //change form title
            //MainForm.ActiveForm.Text = MainForm.ActiveForm.Text + " " + myText;
            this.Text = this.Text + " " + myText;        

        }//Show progress bar and wire start/stop background worker

        private void CreateBackupFile(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            // show the progress bar when the associated event fires (here, a button click)                
            OMSQLDB oms = new OMSQLDB();
            if (!oms.SqlClientCheck())
            {
                MessageBox.Show("SQLCl");
                CreateBakFile();
            }
            else
            {
                MessageBox.Show("OLECl");
                CreateBakFileOLE();
            }
        }

        // This event handler updates the progress. 
        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            label4.Text = "Progress: " + e.ProgressPercentage + "%";
        }

        private void btnCreateBakFile_Click(object sender, EventArgs e)
        {
            try
            {
                //it will be enabled again when either the backup completes or 'cancel' button is clicked.
                btnCreateBakFile.Enabled = false;
                GetOmate32iniFileValues();
                
                //todo: need to handle this problem in the latest file which is at work
                //the line below errors when running this remotely
                //it trys to find the db locally only
                //var hasBak = Directory.GetFiles(_clientDbFilePath + @"\", "*.bak").Length > 0;                
            }

            catch (DirectoryNotFoundException dirEx)
            {
                //MessageBox.Show(dirEx.Message, dirEx.ToString());
                label4.Text = "Progress:";
                string message = dirEx.ToString();
                string caption = "Error";
                var result = MessageBox.Show(message, caption,
                                             MessageBoxButtons.OK,
                                             MessageBoxIcon.Asterisk);
            }

            PopulateBkUpScreen();
            // show the progress bar when the associated event fires (here, a button click)
            progressBar.Show();

            // start the long running task async
            _bw.RunWorkerAsync();

        }

        private void BwRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // hide the progress bar when the long running process finishes
            progressBar.Hide();
            btnCreateBakFile.Enabled = true;
            if (_backupSucceeded)
            {
                //DisplayBkUpSuccessMessage(e);
                //Hide the Backup Screen and show Fix and Catch button and dropdown listboxes.
                DisplayCatchFixOnly();
            }
        }

        public void CreateBakFile()
        {

            using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection())
            {
                conn.FireInfoMessageEventOnUserErrors = true;
                conn.InfoMessage += new SqlInfoMessageEventHandler(OnInfoMessage);

                conn.ConnectionString =
                    "integrated security=false;Data Source=tcp:" + _clientServerName + "\\" + _clientInstanceName + ";" +
                    "user id=OM_USER;password=OMSQL@2004;" +
                    "persist security info=False;database=" + _clientDatabaseName;

                try
                {
                    conn.Open();
                    string database = conn.DataSource.ToString();
                    #region just lists all db on the server-not used
                    //DataTable tblDatabases = conn.GetSchema("Databases");
                    //foreach (DataRow row in tblDatabases.Rows)
                    //{
                    //    Console.WriteLine("Database: " + row["database_name"]);
                    //}
                    #endregion
                    
                    string _tempCommandString = "select physical_name from sys.database_files where type = 0";
                    SqlCommand commandCreateBakFile;
                    SqlCommand commandGetDBFilePath = new SqlCommand(_tempCommandString, conn);//had to use this avoid the 'connection property has not been initialized error'

                    //Get the omsqldb path111
                    commandGetDBFilePath.CommandText = "select physical_name from sys.database_files where type = 0";
                    _clientDbFilePath = (string)commandGetDBFilePath.ExecuteScalar();
                    //now need to strip the database name away from the path, so we can use the path in the createBakFile command
                    int _filePathLen = _clientDbFilePath.Length;
                    int _dbLen = _clientDatabaseName.Length;
                    _clientDbFilePath = _clientDbFilePath.Substring(0, ((_filePathLen - 4) - _dbLen));

                    //create the .bak file
                    //keep this hardcoded example for reference:  commandCreateBakFile = new SqlCommand(@"backup database TestDB to disk ='C:\OFFICEMATE\DATA\OMSQLBkup\TestDB1.bak' with init,stats=10", conn);
                    _BakFileSuffix = "_FullBackup_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".bak";

                    commandCreateBakFile = new SqlCommand(@"backup database " + _clientDatabaseName + " to disk ='" + _clientDbFilePath + "\\" + _clientDatabaseName + _BakFileSuffix + "' with init,stats=10", conn);
                    commandCreateBakFile.CommandTimeout = 200;
                    commandCreateBakFile.ExecuteNonQuery();
                    _backupSucceeded = true;
                    conn.Close();

                }
                catch (SqlException ex)
                {
                    _backupSucceeded = false;
                    MessageBox.Show(ex.Message, ex.Number.ToString());
                    string message = ex.ToString();
                    string caption = "Error";
                    var result = MessageBox.Show(message, caption,
                                                 MessageBoxButtons.OK,
                                                 MessageBoxIcon.Exclamation);
                }

            }
        }

        private void CreateBakFileOLE()
        {
            OMSQLDB o = new OMSQLDB();

            using (OleDbConnection conn = o.GetOLEConnection())
            {
                //conn.FireInfoMessageEventOnUserErrors = true;               
                conn.InfoMessage += new OleDbInfoMessageEventHandler(OnInfoMessage);
               
                try
                {
               
                    string database = conn.DataSource.ToString();
                    string _tempCommandString = "select physical_name from sys.database_files where type = 0";

                    
                    OleDbCommand commandCreateBakFile;
                   // 12/14/2013 CW: adding using statement to help ensure disposed
                    //OleDbCommand commandGetDBFilePath = new OleDbCommand(_tempCommandString, conn);//had to use this avoid the 'connection property has not been initialized error'

                    using (OleDbCommand commandGetDBFilePath = new OleDbCommand(_tempCommandString, conn))//had to use this avoid the 'connection property has not been initialized error'
                    {
                        //Get the omsqldb path
                        commandGetDBFilePath.CommandText = "select physical_name from sys.database_files where type = 0";
                        _clientDbFilePath = (string)commandGetDBFilePath.ExecuteScalar();
                        //now need to strip the database name away from the path, so we can use the path in the createBakFile command
                        int _filePathLen = _clientDbFilePath.Length;
                        int _dbLen = _clientDatabaseName.Length;
                        _clientDbFilePath = _clientDbFilePath.Substring(0, ((_filePathLen - 4) - _dbLen));

                        //create the .bak file
                        //keep this hardcoded example for reference:  commandCreateBakFile = new SqlCommand(@"backup database TestDB to disk ='C:\OFFICEMATE\DATA\OMSQLBkup\TestDB1.bak' with init,stats=10", conn);
                        _BakFileSuffix = "_FullBackup_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".bak";
                    }

                    // 12/14/2013 CW: adding using statement to help ensure disposed
                    //commandCreateBakFile = new OleDbCommand(@"backup database " + _clientDatabaseName + " to disk ='" + _clientDbFilePath + "\\" + _clientDatabaseName + _BakFileSuffix + "' with init,stats=10", conn);
                    
                    using (commandCreateBakFile = new OleDbCommand(@"backup database " + _clientDatabaseName + " to disk ='" + _clientDbFilePath + "\\" + _clientDatabaseName + _BakFileSuffix + "' with init,stats=10", conn))
                    {
                    commandCreateBakFile.CommandTimeout = 200;
                    commandCreateBakFile.ExecuteNonQuery();
                    _backupSucceeded = true;
                    conn.Close();
                    }
                }
                catch (SqlException ex)
                {
                    _backupSucceeded = false;
                    MessageBox.Show(ex.Message, ex.Number.ToString());
                    string message = ex.ToString();
                    string caption = "Error";
                    var result = MessageBox.Show(message, caption,
                                                 MessageBoxButtons.OK,
                                                 MessageBoxIcon.Exclamation);
                }
            }
        }

        private void GetOmate32iniFileValues()
        {
            int counter = 0;
            string line;


            // Read the omate32.ini file line by line.
            // Capture the relevant ADO section line items
            try
            {
                System.IO.StreamReader file =
                   new System.IO.StreamReader("c:\\windows\\omate32.ini");

                while ((line = file.ReadLine()) != null)
                {
                    if (line.Contains("Databasename"))
                    {
                        string[] words = line.Split('=');
                        foreach (string word in words)
                        {
                            _clientDatabaseName = words[1];
                        }
                    }

                    if (line.Contains("DataSource"))
                    {
                        string iniLine = line.Substring(11);
                        string[] words = iniLine.Split('\\');
                        _clientServerName = words[0];
                        // CW 2/4/2014 Some omate32.ini files don't have an instance resulting in an error
                        // when looking for the second part of the "datasource" line.                        
                        //if (words.Length > 1) _clientInstanceName = words[1];
                        _clientInstanceName = (words.Length > 1) ? words[1] : "";
                    }
                    counter++;
                }

            }
            catch (FileNotFoundException ex)
            {
                string message = "Could not find or open the omate32.ini file";
                string caption = "Error";

                var result = MessageBox.Show(message, caption,
                                             MessageBoxButtons.OK,
                                             MessageBoxIcon.Error);
            }
        }
        
        private void OnInfoMessage(object sender, SqlInfoMessageEventArgs args)
        {//this does work but need to get it to a form label
            Console.WriteLine(args.Message.ToString());
            if (args.Message.Contains("percent processed."))
            {
                string temp = args.Message.Substring(0, 3);
                int mypercent = Convert.ToInt32(temp);
                _bw.ReportProgress(mypercent);
            }
            if (args.Message.Contains("successfully processed"))
            {
                //string temp = args.Message.Substring(0, 3);
                //_bw.ReportProgress(temp);
                //_bw.RunWorkerCompleted
                DisplayBkUpSuccessMessage(args);
            }

            #region not used, but handles all args
            /*
            foreach (SqlError err in args.Errors)
            {
                Console.WriteLine(
              "The {0} has received a severity {1}, state {2} error number {3}\n" +
              "on line {4} of procedure {5} on server {6}:\n{7}",
               err.Source, err.Class, err.State, err.Number, err.LineNumber,
               err.Procedure, err.Server, err.Message);
                //todo: update file at work with this progressbar change
                //label4.Text = "Progress:{0} " + err.Message;
                if (err.Message.EndsWith("percent processed."))
                {
                string temp = err.Message.Substring(0, 3);
                int mypercent = Convert.ToInt32(temp);                
                _bw.ReportProgress(mypercent);
                }
              }
 * */
            #endregion
        }

        private void OnInfoMessage(object sender, OleDbInfoMessageEventArgs args)
        {
            Console.WriteLine(args.Message.ToString());
            if (args.Message.Contains("percent processed."))
            {
                string temp = args.Message.Substring(0, 3);
                int mypercent = Convert.ToInt32(temp);
                _bw.ReportProgress(mypercent);
            }
            if (args.Message.Contains("successfully processed"))
            {
                //string temp = args.Message.Substring(0, 3);
                //_bw.ReportProgress(temp);
                //_bw.RunWorkerCompleted
                DisplayBkUpSuccessMessage(args);
            }
        }

        private void DisplayBkUpSuccessMessage(SqlInfoMessageEventArgs e)
        {
            string bksuccessmsg = e.ToString();
            //Label lblBackupSuccess = new Label();
            //lblBackupSuccess.Location = new Point(0, 160);
            //lblBackupSuccess.Width = 300;
            //lblBackupSuccess.Height = 50;
            //lblBackupSuccess.AutoSize = true;
            //this.Controls.Add(lblBackupSuccess);
            //lblBackupSuccess.Text = bksuccessmsg;
            MessageBox.Show(bksuccessmsg);
        } //overloaded to provide optional parameter the .net 3.5 way

        private void DisplayBkUpSuccessMessage(string e)
        {
            string bksuccessmsg = e.ToString();
            Label lblBackupSuccess = new Label();
            lblBackupSuccess.Location = new Point(0, 160);
            lblBackupSuccess.Width = 300;
            lblBackupSuccess.Height = 50;
            lblBackupSuccess.AutoSize = true;
            this.Controls.Add(lblBackupSuccess);
            lblBackupSuccess.Text = bksuccessmsg;
            MessageBox.Show(bksuccessmsg);
            DisplayBkUpSuccessMessage("Successfully created Backup file: " + _clientDbFilePath + _clientDatabaseName + _BakFileSuffix);
        } //overloaded to provide optional parameter the .net 3.5 way

        private void DisplayBkUpSuccessMessage(OleDbInfoMessageEventArgs e)
        {
            string bksuccessmsg = e.ToString();
            //Label lblBackupSuccess = new Label();
            //lblBackupSuccess.Location = new Point(0, 160);
            //lblBackupSuccess.Width = 300;
            //lblBackupSuccess.Height = 50;
            //lblBackupSuccess.AutoSize = true;
            //this.Controls.Add(lblBackupSuccess);
            //lblBackupSuccess.Text = bksuccessmsg;
            MessageBox.Show(bksuccessmsg);
        } //overloaded to provide optional parameter the .net 3.5 way

        private void GetDatabaseProperties()//get database properties from sql server
        {
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection();
            conn.ConnectionString =
                "integrated security=false;Data Source=tcp:" + _clientServerName + "\\" + _clientInstanceName + ";" +
                "user id=OM_USER;password=OMSQL@2004;" +
                "persist security info=False;database=" + _clientDatabaseName;
            try
            {
                conn.Open();
                string database = conn.DataSource.ToString();
                string _tempCommandString = "select physical_name from sys.database_files where type = 0";
                //            SqlCommand commandCreateBakFile;
                SqlCommand commandGetDBFilePath = new SqlCommand(_tempCommandString, conn);//had to use this avoid the 'connection property has not been initialized error'
                //Get the omsqldb path
                commandGetDBFilePath.CommandText = "select physical_name from sys.database_files where type = 0";
                _clientDbFilePath = (string)commandGetDBFilePath.ExecuteScalar();
                //now need to strip the database name away from the path, so we can use the path in the createBakFile command
                int _filePathLen = _clientDbFilePath.Length;
                int _dbLen = _clientDatabaseName.Length;
                _clientDbFilePath = _clientDbFilePath.Substring(0, ((_filePathLen - 4) - _dbLen));
                conn.Close();
            }

            catch (SqlException ex)
            {

                StopTheApplication("SqlException");
                string message = "The server was not found or was not accessible. Verify that the instance name is correct and that SQL Server is configured to allow remote connections";
                string caption = "Error";
                var result = MessageBox.Show(message, caption,
                                             MessageBoxButtons.OK,
                                             MessageBoxIcon.Error);


            }
        }

        private void PopulateBkUpScreen()
        {   //Try to get dbname and server from omate32.ini;         
            GetOmate32iniFileValues();
            //Try to get the file path on the server from SQL Server
            if (_clientDatabaseName != null || _clientServerName != null) { GetDatabaseProperties(); }
            //Populate the fields on the user screen
            if (_clientDbFilePath != null) { PopulateScreenFields(); }
        }

        private void PopulateScreenFields()
        {
            cboDatabaseName.Items.Add(new ComboBoxItem(_clientDatabaseName, _clientDatabaseName));
            cboDatabaseName.SelectedIndex = cboDatabaseName.Items.Count - 1;
            cboBackupType.Items.Add(new ComboBoxItem("Full", "Full"));
            txtBxBackupSetName.Text = _clientDatabaseName + "-Full Database Backup";
            txtbxDestination.Text = _clientDbFilePath + _clientDatabaseName + "_FullBackup_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".bak";
        }

        public class ComboBoxItem //used in PopulateScreenFields
        {
            public string Value;
            public string Text;
            public ComboBoxItem(string val, string text)
            {
                Value = val;
                Text = text;
            } //needed to populate comboboxes
            public override string ToString()
            {
                return Text;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            StopTheApplication();
            btnCreateBakFile.Enabled = true;
        }

        private void StopTheApplication()//this is used in all the catches
        {
            this.Close();
            this.Dispose();
        }

        private void StopTheApplication(string _exceptionType)//this is used in all the catches that seem to error when trying to close the form
        {
            //Todo: here we might suggest solutions based on the exception
            //for now it just continues to display the form with default values.

        }

        private void btnTestConnection_Click(object sender, EventArgs e)
        {

            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection();
            conn.InfoMessage += new SqlInfoMessageEventHandler(OnInfoMessage);

            #region hardcoded connection string for reference


            //conn.ConnectionString =
            //    "integrated security=false;Data Source=tcp:WL10407FP\\OMSQL;" +
            //    "Initial Catalog=OMSQLDB" +
            //    "user id=OM_USER;password=OMSQL@2004;" +
            //    "persist security info=False;database=OMSQLDB";

            #endregion
            //not used

            //conn.ConnectionString = "Data Source=WL10407FP\\OMSQL;Initial Catalog=OMSQLDB;Integrated Security=false;";

            conn.ConnectionString =
                "integrated security=false;Data Source=tcp:" + _clientServerName + "\\" + _clientInstanceName + ";" +
                "user id=OM_USER;password=OMSQL@2004;" +
                "persist security info=False;database=" + _clientDatabaseName;

            try
            {
                conn.Open();
                string database = conn.DataSource.ToString();
                MessageBox.Show("Connection Succeeded");

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

        private void btnTestConnectionOLEDB_Click(object sender, EventArgs e)
        {
            OleDbConnection conn = new OleDbConnection();
            conn.ConnectionString =
            "Provider = SQLOLEDB;" +
            "Driver=SQLOLEDB;" +
            "Data Source=tcp:" + _clientServerName + "\\" + _clientInstanceName + ";" +
            "Initial Catalog=" + _clientDatabaseName + ";" +
            "User id=OM_USER;" + ";" +
            "Password=OMSQL@2004;";

            try
            {
                conn.Open();
                string database = conn.DataSource.ToString();
                MessageBox.Show("Connection Succeeded");
                conn.Close();
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

        private void ChooseSolutionProvider()
        {

        }

        private void ExecuteSolutionOleDb(int _solutionnumber)
        {//given a solution number, run its sql script

            using (OleDbConnection conn = new OleDbConnection())
            {
                conn.ConnectionString =
                "Provider = SQLOLEDB;" +
                "Driver=SQLOLEDB;" +
                "Data Source=tcp:" + _clientServerName + "\\" + _clientInstanceName + ";" +
                "Initial Catalog=" + _clientDatabaseName + ";" +
                "User id=OM_USER;" + ";" +
                "Password=OMSQL@2004;";

                try
                {
                    conn.Open();
                    //string database = conn.DataSource.ToString();
                    //MessageBox.Show("Connection Succeeded");

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

                string queryString = "Update patient_referrals set ref_by_no=0 where ref_by_no is null";
                {
                    OleDbCommand command = new OleDbCommand(queryString, conn);
                    command.ExecuteNonQuery();
                    // always call Close when done reading.

                }
            }
        }

        //TODO: Remove as this appears to not be used after migrating to the NewSmallFixIt_FindItHandler
        //this handler has been replaced with NewSmallFixIt_FindItHandler, but may need the code handling
        private void FixIt_FindItHandler(object sender, EventArgs e)
        {
//            string queryString = "";
//            //            Solutions sos = new Solutions();            
//            bool isFixIt = false;
//            const string  iBal_solutionNumber = "s00000238 iBal";
//            const string activateEditandVoidButtons_solutionNumber = "s00005656";
//            const string UnknownInsurance_solutionNumber = "s00001466";
//            try
//            {
                
//                if (sender == btnFixIt)
//#region FixIT
//                {
//                    isFixIt = true;
//                    string selectedSolution = ((string)cboInsuranceChoices.SelectedItem).ToString();
//                    MessageBox.Show (selectedSolution);
//                    if (selectedSolution == iBal_solutionNumber || selectedSolution == activateEditandVoidButtons_solutionNumber)
//                    {
//                        switch (selectedSolution)
//                        {
//                            case iBal_solutionNumber:
//                                InitiateiBal();
//                                return;

//                            case UnknownInsurance_solutionNumber:
//                                InitiateiBal();
//                                return;                                

//                            case activateEditandVoidButtons_solutionNumber:
//                                MessageBox.Show("OK");
//                                ChangeFeeslipEditState("0"); //for now, we are only putting the feeslip in Recorded state, so it can be voided.
//                                return;
                            
//                            default:
//                                break;
//                        }

//                    }
//                    else//run generic, ready-to-run sql statement extracted from the solution
//                    {
//                        queryString = sols.GetSQLFromSolution(selectedSolution);
//                    }
//                }
//#endregion
//                else//This is a case catcher
//                {
//                    string selectedSolution = ((string)cboCaseCatchers.SelectedItem).ToString();
//                    queryString = sols.GetSQLFromCaseCatcher(selectedSolution);
//                    //todo: add code to run generic, ready-to-run sql statements from solutions
//                }
//            }

//            catch
//            {
//                MessageBox.Show("Please select a solution");
//                return;
//            }

//            queryString = queryString.Replace("\t", " ").Replace("\n", " ").Replace("\r", " ");
//            OMSQLDB oms = new OMSQLDB();
//            if (!oms.SqlClientCheck())
//            {
//                #region run script using Sql Client
//                using (SqlConnection conn = new SqlConnection())
//                {
//                    conn.ConnectionString =
//                        "integrated security=false;Data Source=tcp:" + _clientServerName + "\\" + _clientInstanceName + ";" +
//                        "user id=OM_USER;password=OMSQL@2004;" +
//                        "persist security info=False;database=" + _clientDatabaseName;

//                    try
//                    {
//                        conn.Open();
//                        SqlCommand command = new SqlCommand(queryString, conn);
//                        command.ExecuteNonQuery();
//                        // always call Close when done reading.

//                    }
//                    catch (SqlException ex)
//                    {
//                        _ConnectionSucceeded = false;
//                        MessageBox.Show(ex.Message, ex.Number.ToString());
//                        string message = ex.ToString();
//                        string caption = "Error";
//                        var result = MessageBox.Show(message, caption,
//                                                     MessageBoxButtons.OK,
//                                                     MessageBoxIcon.Exclamation);
//                    }
//                }

//                #endregion
//            }
//            else
//            {
//                #region run script using OleDb
//                using (OleDbConnection conn = new OleDbConnection())
//                {
//                    conn.ConnectionString =
//                    "Provider = SQLOLEDB;" +
//                    "Driver=SQLOLEDB;" +
//                    "Data Source=tcp:" + _clientServerName + "\\" + _clientInstanceName + ";" +
//                    "Initial Catalog=" + _clientDatabaseName + ";" +
//                    "User id=OM_USER;" + ";" +
//                    "Password=OMSQL@2004;";

//                    try
//                    {
//                        conn.Open();
//                        OleDbCommand command = new OleDbCommand(queryString, conn);
//                        using (OleDbDataReader reader = command.ExecuteReader())
//                        {
//                            string sqlResults = "";                            
                            
//                            while (reader.Read())
//                            {
//                                sqlResults += reader[0] + "\n";
//                            }
//                            if (isFixIt)
//                            {
//                                MessageBox.Show("Solution run successfully", "Fix It");
//                            }
//                            else //this is a catchIt request
//                            {                               
//                                if (reader.HasRows)
//                                {
//                                    MessageBox.Show("Results from the Case Catcher: \n" +
//                                    "There is a solution to fix records with the following values in the " + reader.GetName(0) + " field: \n" +
//                                     sqlResults, "Case Catcher");
//                                }
//                                else
//                                {
//                                    MessageBox.Show("There are no records that meet the solution condition", "Case Catcher");
//                                }
//                            }
//                        }

//                    }
//                    catch (SqlException ex)
//                    {
//                        _ConnectionSucceeded = false;
//                        MessageBox.Show(ex.Message, ex.Number.ToString());
//                        string message = ex.ToString();
//                        string caption = "Error";
//                        var result = MessageBox.Show(message, caption,
//                                                     MessageBoxButtons.OK,
//                                                     MessageBoxIcon.Exclamation);
//                    }
//                }

//                #endregion
//            }
        } //Does everything, so breaking out in to manageable pieces


        //TODO: Remove as this appears to not be used after migrating to the NewSmallFixIt_FindItHandler
        #region Fix it Setup
        //TODO: Remove as this appears to not be used after migrating to the NewSmallFixIt_FindItHandler
        private void PopulateComboBoxOfSolutions()
        {
            //Solutions solutionList = new Solutions();
            //foreach (System.Reflection.MemberInfo mi in solutionList.GetType().GetFields())
            //{
            //    cboInsuranceChoices.Items.Add(mi.Name.ToString());
            //}
        }

        Dictionary<string, string> dictSolutions = new Dictionary<string, string>();

        //TODO: Remove as this appears to not be used after migrating to the NewSmallFixIt_FindItHandler
        private void PopulateSolutionsDict()
        {
//            dictSolutions.Add("s00003810", @"UPDATE
//                                    soft_contact_lens_rx
//                                    SET
//                                    BASE_CURVE_MM = ISNULL(BASE_CURVE_MM , 0),
//                                    ADD_DIOPTER = ISNULL(ADD_DIOPTER, 0)");

//            dictSolutions.Add("s00004347", @"update Attribute set attribute_dev_cd = 0 
//                                    WHERE attribute_dev_cd is null and attribute_category_id = 10");
        }

        //TODO: Remove as this appears to not be used after migrating to the central Solution class
        private string GetSQLFromSolution(string _solutionNumber)
        {

            string _sqlScript = "";

            //foreach (var pair in dictSolutions)
            //{
            //    if (_solutionNumber == pair.Key)
            //    {
            //        _sqlScript = pair.Value;
            //        return pair.Value;
            //    }
            //}
            return _sqlScript;

        }
        #endregion//this may be unneeded 

        private void NewSmallFixIt_FindItHandler(object sender, EventArgs e)//This is in production
        {
            //determine client
            bool isSqlClient = false;
            OMSQLDB oms = new OMSQLDB();
            isSqlClient = oms.SqlClientCheck();
            //TODO: We hardcoded isSqlClient = false to force oledbclient usage.Maybe this is where server issue is resolved.
            //TODO: If isSqlClient is reactivated, then we need to 
            isSqlClient = false;
            string selectedSolution;
            try
            {
                switch (((Button)sender).Text)
                {

                    case "Find It":
                        //check to see if anything was selected
                        if (cboCaseCatchers.SelectedIndex > -1)
                        {
                            selectedSolution = ((string)cboCaseCatchers.SelectedItem).ToString();
                            if (isSqlClient)
                            {
                                //FindItSqlClient(); //this was moved to its own class FindAndFixCases
                                //testing the ext class                       
                                FindAndFixCases ffc = new FindAndFixCases(selectedSolution);
                                ffc.FindItSqlClient();
                            }
                            else
                            {
                                //this was moved to its own class FindAndFixCases
                                //FindItOleClient();

                                FindAndFixCases ffc = new FindAndFixCases(selectedSolution);
                                ffc.FindItOleClient();
                            }
                        }
                        else
                        {

                            var result = MessageBox.Show("Please select a solution", "Missing Solution",
                             MessageBoxButtons.OK,
                             MessageBoxIcon.Question);
                        }
                        break;

                    case "Fix It":
                        if (cboInsuranceChoices.SelectedIndex > -1)
                        {
                            selectedSolution = ((string)cboInsuranceChoices.SelectedItem).ToString();
                            if (isSqlClient)
                            {//todo: move FixItSQlClient call to the findandfixcases class
                                FixItSqlClient();
                                //FindAndFixCases ffc = new FindAndFixCases(selectedSolution);
                                //ffc.FixItSqlClient();
                            }
                            else
                            {
                                //FixItOleClient();
                                FindAndFixCases ffc = new FindAndFixCases(selectedSolution);
                                ffc.FixItOleClient();

                            }
                        }
                        else
                        {
                            var result = MessageBox.Show("Please select a solution", "Missing Solution",
                             MessageBoxButtons.OK,
                             MessageBoxIcon.Question);
                        }
                        break;

                    default:
                        break;

                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
   
        } //Determine ole or sql client then call fix or find
       //TODO: Refactor the individual fixit findit code below.
        //TODO: Remove as this appears to not be used after migrating to the FindAndFixCases.cs
        private void FindItOleClient()// WORKS called from FixIt_FindItHandler
        {
            //string selectedSolution = ((string)cboCaseCatchers.SelectedItem).ToString();            
            //string queryString = sols.GetSQLFromCaseCatcher(selectedSolution);

            //    using (OleDbConnection conn = new OleDbConnection())
            //    {
            //        conn.ConnectionString =
            //        "Provider = SQLOLEDB;" +
            //        "Driver=SQLOLEDB;" +
            //        "Data Source=tcp:" + _clientServerName + "\\" + _clientInstanceName + ";" +
            //        "Initial Catalog=" + _clientDatabaseName + ";" +
            //        "User id=OM_USER;" + ";" +
            //        "Password=OMSQL@2004;";

            //        try
            //        {
            //            conn.Open();
            //            OleDbCommand command = new OleDbCommand(queryString, conn);
            //            using (OleDbDataReader reader = command.ExecuteReader())
            //            {
            //                string sqlResults = "";                            
                            
            //                while (reader.Read())
            //                {
            //                    sqlResults += reader[0] + "\n"; //This captures the field name in the sql table
            //                }			
                                                           
            //                    if (reader.HasRows)
            //                    {
            //                        MessageBox.Show("Results from the Case Catcher: \n" +
            //                        "There is a solution to fix records with the following values in the " + reader.GetName(0) + " field: \n" +
            //                         sqlResults, "Case Catcher");
            //                    }
            //                    else
            //                    {
            //                        MessageBox.Show("There are no records that meet the solution condition", "Case Catcher");
            //                    }
            //                }
            //            }

                    
            //        catch (SqlException ex)
            //        {
            //            _ConnectionSucceeded = false;
            //            MessageBox.Show(ex.Message, ex.Number.ToString());
            //            string message = ex.ToString();
            //            string caption = "Error";
            //            var result = MessageBox.Show(message, caption,
            //                                         MessageBoxButtons.OK,
            //                                         MessageBoxIcon.Exclamation);
            //        }
            //    }
        }

        //TODO: Remove as this appears to not be used after migrating to the FindAndFixCases.cs
        private void FixItOleClient()// WORKS called from FixIt_FindItHandler
        {            
            //string selectedSolution = ((string)cboInsuranceChoices.SelectedItem).ToString();
            //string queryString = sols.GetSQLFromSolution(selectedSolution);
            
            //using (OleDbConnection conn = new OleDbConnection())
            //{
            //    conn.ConnectionString =
            //    "Provider = SQLOLEDB;" +
            //    "Driver=SQLOLEDB;" +
            //    "Data Source=tcp:" + _clientServerName + "\\" + _clientInstanceName + ";" +
            //    "Initial Catalog=" + _clientDatabaseName + ";" +
            //    "User id=OM_USER;" + ";" +
            //    "Password=OMSQL@2004;";

            //    try
            //    {
            //        conn.Open();
            //        OleDbCommand command = new OleDbCommand(queryString, conn);
            //        using (OleDbDataReader reader = command.ExecuteReader())
            //        {
            //            string sqlResults = "";

            //            while (reader.Read())
            //            {
            //                sqlResults += reader[0] + "\n";
            //            }                        
            //                MessageBox.Show("Solution ran successfully", "Fix It");

            //        }

            //    }
            //    catch (SqlException ex)
            //    {
            //        _ConnectionSucceeded = false;
            //        MessageBox.Show(ex.Message, ex.Number.ToString());
            //        string message = ex.ToString();
            //        string caption = "Error";
            //        var result = MessageBox.Show(message, caption,
            //                                     MessageBoxButtons.OK,
            //                                     MessageBoxIcon.Exclamation);
            //    }
            //}
           
        }

        //TODO: Remove as this appears to not be used after migrating to the FindAndFixCases.cs
        private void FindItSqlClient()// WORKS called from FixIt_FindItHandler
        {
            //string selectedSolution = ((string)cboCaseCatchers.SelectedItem).ToString();
            //string queryString = sols.GetSQLFromCaseCatcher(selectedSolution);

            //{            
            //    using (SqlConnection conn = new SqlConnection())
            //    {
            //        conn.ConnectionString =
            //            "integrated security=false;Data Source=tcp:" + _clientServerName + "\\" + _clientInstanceName + ";" +
            //            "user id=OM_USER;password=OMSQL@2004;" +
            //            "persist security info=False;database=" + _clientDatabaseName;

            //        try
            //        {
            //            conn.Open();

                        	    
            //        using (SqlCommand command = new SqlCommand(queryString, conn))
            //        {                           
            //            using (SqlDataReader  reader = command.ExecuteReader())
            //            {
            //                string sqlResults = "";

            //                while (reader.Read())
            //                {
            //                    sqlResults += reader[0] + "\n"; //This captures the field name in the sql table
            //                }

            //                if (reader.HasRows)
            //                {
            //                    MessageBox.Show("Results from the Case Catcher: \n" +
            //                    "There is a solution to fix records with the following values in the " + reader.GetName(0) + " field: \n" +
            //                     sqlResults, "Case Catcher");
            //                }
            //                else
            //                {
            //                    MessageBox.Show("There are no records that meet the solution condition", "Case Catcher");
            //                }
            //            }
            //        }
                      

            //        }
            //        catch (SqlException ex)
            //        {
            //            _ConnectionSucceeded = false;
            //            MessageBox.Show(ex.Message, ex.Number.ToString());
            //            string message = ex.ToString();
            //            string caption = "Error";
            //            var result = MessageBox.Show(message, caption,
            //                                         MessageBoxButtons.OK,
            //                                         MessageBoxIcon.Exclamation);
            //        }
            //    }

             
            //}
        }

        //TODO: Remove as this appears to not be used after migrating to the FindAndFixCases.cs
        private void FixItSqlClient()// WORKS called from FixIt_FindItHandler
        {

            //string selectedSolution = ((string)cboInsuranceChoices.SelectedItem).ToString();
            //string queryString = sols.GetSQLFromSolution(selectedSolution);

            //using (SqlConnection conn = new SqlConnection())            
            //{
            //    conn.ConnectionString =
            //    "integrated security=false;Data Source=tcp:" + _clientServerName + "\\" + _clientInstanceName + ";" +
            //            "user id=OM_USER;password=OMSQL@2004;" +
            //            "persist security info=False;database=" + _clientDatabaseName;

            //    try
            //    {
            //        conn.Open();
            //        using (SqlCommand command = new SqlCommand(queryString, conn))
            //        {
            //            using (SqlDataReader reader = command.ExecuteReader())
            //            {
            //                string sqlResults = "";

            //                while (reader.Read())
            //                {
            //                    sqlResults += reader[0] + "\n";
            //                }
            //                MessageBox.Show("Solution ran successfully", "Fix It");

            //            }
            //        }

            //    }
            //    catch (SqlException ex)
            //    {
            //        _ConnectionSucceeded = false;
            //        MessageBox.Show(ex.Message, ex.Number.ToString());
            //        string message = ex.ToString();
            //        string caption = "Error";
            //        var result = MessageBox.Show(message, caption,
            //                                     MessageBoxButtons.OK,
            //                                     MessageBoxIcon.Exclamation);
            //    }
            //}
        }

        //TODO: Remove as this appears to not be used after migrating to the FindAndFixCases.cs
        private void PopulateComboBoxOfCaseCatcher()
        {


            //Solutions solutionList = new Solutions();
            //foreach (System.Reflection.MemberInfo mi in solutionList.GetType().GetFields())
            //{
            //    cboCaseCatchers.Items.Add(mi.Name.ToString());
            //}
        }

        private void DisplayCatchFixOnly()
        {
            grpbxCatchIt.Location = new Point(grpbxCatchIt.Location.X, 61);
            grpbxFixIt.Location = new Point(grpbxFixIt.Location.X, 150);
            grpbxBackupDestination.Visible = false;
            grpboxBackupSet.Visible = false;
            grpBoxSource.Visible = false;
            grpbxBackup.Visible = false;
            grpbxCatchIt.Visible = true;
            grpbxFixIt.Visible = true;

            //this.Size = new Size(this.Size.Width, this.Size.Height + 25);
         
            
               this.Size = new Size(this.Size.Width, 300);
            txtbxExplanation.Text = @"Click the 'FindIt' button to assess the system for unsettled database conditions. 
            Click the 'FixIt' button to resolve those conditions using the selected solution";
            this.Text = "FindIt and FixIt";

        }

        //TODO: Remove as this appears to not be used after migrating to the NewSmallFixIt_FindItHandler
        private void ChangeFeeslipEditState(string feeslipEditState)
        {
            //switch (feeslipEditState)
            //{
            //    case "Null": //All edit/hold buttons active
            //        break;
                
            //    case "0": // put the fee slip in a recorded state, feeslip can be edited or voided
            //        break;                
                
            //    default:
            //        break;
            //}

        }

        private void btnIbal_Click(object sender, EventArgs e)//keep for demo only
        {
            //DO NOT Try to call this ***ReadAR_Report();*** 
            //here due to timing issues, will error/not get read onto the screen
            
            InitiateiBal();

        }//don't delete
        
        private void InitiateiBal()
        {//calls launchiBal1 and launchiBal2
           //This works well here, doesn't run into timing issues.
            if (!ReadAR_Report())
            {
                return;
            }


            var signals = new List<ManualResetEvent>();

            var signal = new ManualResetEvent(false);
            var thread = new Thread(() => { LaunchiBal1(); signal.Set(); });
            thread.Start();

            var completionTask = new Thread(() =>
            {
                WaitHandle.WaitAll(signals.ToArray());
                LaunchiBal2();
            });
        }

        private void GetTaskWindows()
        {
            // Get the desktopwindow handle
            int nDeshWndHandle = NativeWin32.GetDesktopWindow();
            // Get the first child window
            int nChildHandle = NativeWin32.GetWindow(nDeshWndHandle,
                                NativeWin32.GW_CHILD);

            while (nChildHandle != 0)
            {
                //If the child window is this (SendKeys) application then ignore it.
                if (nChildHandle == this.Handle.ToInt32())
                {
                    nChildHandle = NativeWin32.GetWindow
                        (nChildHandle, NativeWin32.GW_HWNDNEXT);
                }

                // Get only visible windows
                if (NativeWin32.IsWindowVisible(nChildHandle) != 0)
                {
                    StringBuilder sbTitle = new StringBuilder(1024);
                    // Read the Title bar text on the windows to put in combobox
                    NativeWin32.GetWindowText(nChildHandle, sbTitle, sbTitle.Capacity);
                    String sWinTitle = sbTitle.ToString();
                    {
                        if (sWinTitle.Length > 0)
                        {
                            Console.WriteLine(sWinTitle);
                        }
                    }
                }
                // Look for the next child.
                nChildHandle = NativeWin32.GetWindow(nChildHandle,
                               NativeWin32.GW_HWNDNEXT);
            }
        }

        private void LaunchiBal1()
        {
            //todo: Will any of this run on terminal services or other cloud-based environments
            //todo:Where to assume iBal is located when running LaunchiBal1
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            System.Diagnostics.Process.Start(path + @"\iBal.exe");
            Thread.Sleep(1500);
            System.Windows.Forms.SendKeys.SendWait("{ENTER}");
            Thread.Sleep(1000);
            LaunchiBal2();
        }

        //todo: Remove LaunchiBal1 and 2 methods that were migrated to the findandfixcases class
        //this can be removed safely, it has been moved
        //out of the form and into the findandfixcases class
        private void LaunchiBal2()
        {

            #region Code to send keys
            
            //need this to get onto the name field
            for (int i = 1; i < 15; i++)
            {
                System.Windows.Forms.SendKeys.SendWait("{TAB}");//going to magnifying glass to initiate fee slip search/gather
            }
            
            System.Windows.Forms.SendKeys.SendWait("Admin_iBal");
            System.Windows.Forms.SendKeys.SendWait("{TAB}"); //going to fee slip number field
            //DO NOT TRY to put this ****ReadAR_Report()*** here, it will run through the screen multiple times
            System.Windows.Forms.SendKeys.SendWait(FeeSlipsWithUnknownInsurance);
            for (int i = 1; i < 19; i++)
            {
                System.Windows.Forms.SendKeys.SendWait("{TAB}");//going to magnifying glass to initiate fee slip search/gather
            }
            Application.DoEvents() ;            System.Windows.Forms.SendKeys.SendWait("{ENTER}"); //Initiate feeslip search/populate fs 'Available' listbox

            for (int i = 1; i < 19; i++)
            {
                System.Windows.Forms.SendKeys.SendWait("{TAB}");//going to '>>' button to move 'Available' slips to selected listbox
            }

            System.Windows.Forms.SendKeys.SendWait("{ENTER}"); //Initiate feeslip search/populate fs 'Available' listbox

            for (int i = 1; i < 10; i++)
            {
                System.Windows.Forms.SendKeys.SendWait("{TAB}");//going to 'Update' button to run iBal on the selected fee slips
            }
            ResumeLayout();
            #endregion

        }

        private string FeeSlipsWithUnknownInsurance;// will be comma delimited populated from ReadAR_Report

        //TODO: Remove as this appears to not be used as we don't use the AR report, we look for conditions in the database
        private bool ReadAR_Report()
        {//Read the exported .ttx Accounts Receivable report
            //capture the fee slip numbers that have Unknown Insurances
            //then the numbers will be put on the iBal utility

            int counter = 0;
            string line;
            string[] arrLine;
            string searchForWord = "";
            string foundFeeslipNumber = "";
            FeeSlipsWithUnknownInsurance = "";
            //todo: need error checking here and really need
            // Read the file and display it line by line.

            if (OpenAcctsRecFile() == null)
            {
                MessageBox.Show("No file selected, cancelling operation", "Cancel", 0, MessageBoxIcon.Information);
                return false;
            }
    

            System.IO.StreamReader file =          
            new System.IO.StreamReader(OpenAcctsRecFile());

            while ((line = file.ReadLine()) != null)
            {
                arrLine = line.Split(',');
                searchForWord = arrLine[21];
                foundFeeslipNumber = arrLine[32];
                if (searchForWord.Contains(" Unknown Insurance Company"))
                {
                   
                    // innner loop to capture the next line(s) as a customer line
                    // then check for the word "Total" indicating the total line
                    // the Insurance Carrier name is in position 22,but we have a base 0 array
                    // the fee slip number is in position 32           
                    if (FeeSlipsWithUnknownInsurance == "")
                    {
                        FeeSlipsWithUnknownInsurance = foundFeeslipNumber.Trim('"');
                    }
                    else
                    {
                        FeeSlipsWithUnknownInsurance +=  "," + foundFeeslipNumber.Trim('"');
                    }
                    
                }
                counter++;
            }

            file.Close();
            return true;
            // Suspend the screen.
            //Console.ReadLine();

        }

        //TODO: Remove as this appears to not be used after migrating to the NewSmallFixIt_FindItHandler
        private string OpenAcctsRecFile()
            {
                // Create an instance of the open file dialog box.
                OpenFileDialog openFileDialog1 = new OpenFileDialog();

                // Set filter options and filter index.
                //openFileDialog1.Filter = "Text Files (.txt)|*.txt|All Files (*.*)|*.*";
                openFileDialog1.Filter = "Comma Separated Files (.csv)|*.csv";
                openFileDialog1.FilterIndex = 1;

                //// Call the ShowDialog method to show the dialog box.
                //bool? userClickedOK = openFileDialog1.ShowDialog();
                // == DialogResult.OK
              
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    Console.WriteLine("ok");
                    string _fileName = openFileDialog1.FileName;
                    return _fileName;
                }

                else
                {
                    Console.WriteLine("no");
                    return null;
                } 
            }

        private void button1_Click(object sender, EventArgs e)
        {
            string feeslipNumber;

            //iBalBackup4.GetFeeSlipNumberFromUser testDialog = new iBalBackup4.GetFeeSlipNumberFromUser();

            //// Show testDialog as a modal dialog and determine if DialogResult = OK. 
            //if (testDialog.ShowDialog(this) == DialogResult.OK)
            //{
            //    // Read the contents of testDialog's TextBox. 
            //    MessageBox.Show(testDialog.txtBxFeeSlipNumber.Text);
            //}
            //else
            //{
            //    MessageBox.Show ("Cancelled");
            ////}
            ////testDialog.Dispose();





                           //Allow input of search text
           Fixit.GetFeeSlipNumberFromUser frm = new Fixit.GetFeeSlipNumberFromUser();
           frm.ShowDialog(this);               
           feeslipNumber = frm.txtBxFeeSlipNumber.Text;
               







        }

        private void DisplayCatchFixPanelsWithoutBackup(object sender, MouseEventArgs e)
        {
            //this can be called to skip the backup, by double-clicking the progress bar
            grpbxCatchIt.Visible = true;
            grpbxFixIt.Visible = true;
        }

        private void DisplayCatchFixPanelsWithoutBackup()
        {

        }

        public string CaseCatcherSelection
        {
            get { return ((string)cboCaseCatchers.SelectedItem).ToString(); }
        }
        //used by the FindAndFixCases class to determine the solution in play

        public string CaseSolutionSelection
        {
            get { return ((string)cboInsuranceChoices.SelectedItem).ToString(); }
        }
        //used by the FindAndFixCases class to determine the solution in play

        public static string _SelectionID = string.Empty;

        private void button2_Click(object sender, EventArgs e)
        {
            string selectedSolution = ((string)cboCaseCatchers.SelectedItem).ToString();
            FindAndFixCases ffc = new FindAndFixCases(selectedSolution);
            ffc.FindItOleClient();
            


        }


        //Remove files from the user's desktop
        private void RemoveDesktopFiles(object sender, FormClosedEventArgs e)
        {
            Delete_iBal_From_Desktop();
            Delete_Utilityprep_From_Desktop();
        }

        //remove the ibal.exe file when closing the form
        private void Delete_iBal_From_Desktop()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);                                 
            string file = path + @"\iBal.exe";
            if (Directory.Exists(Path.GetDirectoryName(file)))
            {
                File.Delete(file);
            }
        }

        //remove the utilityprep.exe file when closing the form
        private void Delete_Utilityprep_From_Desktop()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);           
            string file = path + @"\Utilityprep.exe";                       
            if (Directory.Exists(Path.GetDirectoryName(file)))
            {
                File.Delete(file);
            }
        }


        private void RemoveDesktopFiles(object sender, FormClosingEventArgs e)
        {
            Delete_iBal_From_Desktop();
            Delete_Utilityprep_From_Desktop();
        }


    }
}
