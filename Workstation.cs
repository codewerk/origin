using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Reflection;

namespace Fixit
{
    class Workstation
    {
        public Workstation()
        {
            GetOmate32iniFileValues(); //Load the values when the object is created
        }

        #region Member Variables

        private string _clientDatabaseName;
        public string ClientDatabaseName
        {
            get { return  _clientDatabaseName; }
            set { _clientDatabaseName = value; }
        }

        private string _clientServerName;
        public string ClientServerName  //used for unknown insurance
        {
            get { return _clientServerName; }
            set { _clientServerName = value; }
        }

        private string _clientInstanceName;
        public string ClientInstanceName
        {
            get { return _clientInstanceName; }
            set { _clientInstanceName = value; }
        }

        private string _clientDbFilePath;
        public string ClientDbFilePath
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

        #endregion

        public void Extract_Utilityprep_To_Desktop()
        {
            Assembly A = Assembly.GetExecutingAssembly();
            string[] names = A.GetManifestResourceNames();
            string _savedFileName = ""; //this will be the name used to save to the desktop
            foreach (string filename in names)
            {                
                if (filename.Contains("Utilityprep.exe"))
                {                    
                    _savedFileName = @"\Utilityprep.exe";
                    Stream S = A.GetManifestResourceStream(filename);

                    byte[] rawFile = new byte[S.Length];
                    //Read the data from the assembly

                    S.Read(rawFile, 0, (int)S.Length);
                    //Save the data to the desktop hard drive

                    string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    using (FileStream fs = new FileStream(path + _savedFileName, FileMode.Create))
                    {
                        fs.Write(rawFile, 0, (int)S.Length);
                    }
                }
            }
        }

        public void Extract_iBal_To_Desktop()
        {
            Assembly A = Assembly.GetExecutingAssembly();
            string[] names = A.GetManifestResourceNames();
            string _savedFileName = ""; //this will be the name used to save to the desktop
            foreach (string filename in names)
            {             
                if (filename.Contains("iBal.exe"))
                {
                    _savedFileName = @"\iBal.exe";
                    Stream S = A.GetManifestResourceStream(filename);

                    byte[] rawFile = new byte[S.Length];
                    //Read the data from the assembly

                    S.Read(rawFile, 0, (int)S.Length);
                    //Save the data to the desktop hard drive

                    string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    using (FileStream fs = new FileStream(path + _savedFileName, FileMode.Create))
                    {
                        fs.Write(rawFile, 0, (int)S.Length);
                    }
                }
            }
        }


        string Omate32iniFileLocation = "c:\\windows\\omate32.ini";

        public void GetOmate32iniFileValues()
        {
            int counter = 0;
            string line;


            // Read the omate32.ini file line by line.
            // Capture the relevant ADO section line items
            try
            {
                System.IO.StreamReader file =
                   new System.IO.StreamReader(Omate32iniFileLocation);

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

                    //CW 1/30/2014 added to get the shared data directory to support
                    //the new log class
                    if (line.Contains("DataDir"))
                    {
                        string iniLine = line;
                        string[] words = iniLine.Split('=');
                        //this.SharedDataPath = words[1] + "\\";
                        _clientSharedDataPath = words[1] + "\\";
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

    }
}
