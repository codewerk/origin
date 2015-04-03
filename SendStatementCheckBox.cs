using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Fixit
{
    public partial class SendStatementCheckBox : Form
    {
        public SendStatementCheckBox()
        {
            InitializeComponent();
        }
        string _clientDatabaseName;
        string _clientServerName;
        string _clientInstanceName;
        string _clientDbFilePath;
        string _SharedDataPath; //CW 2/3/2014 used for logging the fixes into a file in the data folder

        bool isGridDirty = false; //this is only used by the ModifyCheckAllButton method, to flag if it's the first chk/unchk on the grid
        //the method should only run the first time a box is checked, so the button text can be set.
        bool isAnyRowChecked = false; //used to flag if any checkboxes on a row are checked.

        private void btnFixIT_Click(object sender, EventArgs e)
        {
            GetOmate32iniFileValues();            
            int chkbox;
            bool isSqlClient = false;
            OMSQLDB oms = new OMSQLDB();
            isSqlClient = oms.SqlClientCheck();

            int _colPositionOf_Patient_ID = 1;
            int _colPositionOf_Last_Name = 2;
            int _colPositionOf_First_Name = 3;

            //display the selections from the grid in a message box
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                chkbox = (Convert.ToBoolean(row.Cells[0].Value) == false) ? 0 : 1;
                if (Convert.ToBoolean(chkbox) == true)
                {
                    Patient  pat = new Patient ();
                    //the code cover made these names unreadable and errored out
                    //pat.Patient_ID = row.Cells["Patient_ID"].Value.ToString();
                    //pat.Last_Name = row.Cells["Last_Name"].Value.ToString();
                    //pat.First_Name  = row.Cells["First_Name"].Value.ToString();
                    
                    //so due to the above issue using position variables instead
                    pat.Patient_ID = row.Cells[_colPositionOf_Patient_ID].Value.ToString();
                    pat.Last_Name = row.Cells[_colPositionOf_Last_Name].Value.ToString();
                    pat.First_Name = row.Cells[_colPositionOf_First_Name].Value.ToString();


                    //Update the patient table, setting the send_statement field = 1, which is checking the box for sendstatement
                    FindAndFixCases ffc = new FindAndFixCases();                    

                    if (isSqlClient)
                    {
                        ffc.ProcessSendStatementCheckBox_SQL(pat);
                    }
                    else
                    {
                        ffc.ProcessSendStatementCheckBox_OLE (pat);
                    }

                }                    
            }
            MessageBox.Show("Solution Completed", "Fix It");
            FixitLog fixLog = new FixitLog();
            fixLog.CreateLog("SendStatement", ref dataGridView1, _SharedDataPath);

        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                //this fails is one of the rows is selected  if (Convert.ToBoolean(row.Cells[0].Value) == false)
                if (btnSelectAll.Text == "Unselect All")
                {
                    row.Cells[0].Value = true;
                    btnSelectAll.Text = "Unselect All";
                }
                else
                {
                    row.Cells[0].Value = false;
                    btnSelectAll.Text = "Select All";
                }
            }
        }

        private void GetOmate32iniFileValues()
        {
            Workstation ws = new Workstation();
            _clientDatabaseName = ws.ClientDatabaseName;
            _clientServerName = ws.ClientServerName;
            _clientInstanceName = ws.ClientInstanceName;
            _clientDbFilePath = ws.ClientDbFilePath;
            _SharedDataPath = ws.SharedDataPath; //CW 2/3/2014 added to support logging the fix in a file in the data folder
        }

        private void ModifyCheckAllButton(object sender, DataGridViewCellEventArgs e)
        {
            int colName_CheckBox = 0;

            if (!isGridDirty)
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (Convert.ToBoolean(row.Cells[colName_CheckBox].Value) == false)
                    {
                        isAnyRowChecked = true;
                        isGridDirty = true;
                        btnSelectAll.Text = "Uncheck All";

                    }
                }
            }

        }

        private void CheckAllBoxes(object sender, EventArgs e)
        {
            int colName_CheckBox = 0;
            isAnyRowChecked = false;

            //First check to see if any rows are checked
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {

                if (!DBNull.Value.Equals(row.Cells[colName_CheckBox].Value))
                {

                }
                else
                {
                    if (Convert.ToBoolean(row.Cells[colName_CheckBox].Value) == true)
                    {
                        isAnyRowChecked = true;
                        break;
                    }
                }


            }

            //Finally, use the row check flag to enter the loop to check or uncheck all the rows
            if (isAnyRowChecked || btnSelectAll.Text == "Uncheck All")
            {
                //Uncheck all boxes
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    row.Cells[colName_CheckBox].Value = false;
                }
                btnSelectAll.Text = "Check All";
            }
            else
            {
                //Check all boxes
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    row.Cells[colName_CheckBox].Value = true;
                }
                btnSelectAll.Text = "Uncheck All";
            }
        }

        public void CheckAllBoxes()
        {

            int colName_CheckBox = 0;
            isAnyRowChecked = false;

            //First check to see if any rows are checked
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {

                if (!DBNull.Value.Equals(row.Cells[colName_CheckBox].Value))
                {

                }
                else
                {
                    if (Convert.ToBoolean(row.Cells[colName_CheckBox].Value) == true)
                    {
                        isAnyRowChecked = true;
                        break;
                    }
                }


            }

            //Finally, use the row check flag to enter the loop to check or uncheck all the rows
            if (isAnyRowChecked || btnSelectAll.Text == "Uncheck All")
            {
                //Uncheck all boxes
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    row.Cells[colName_CheckBox].Value = false;
                }
                btnSelectAll.Text = "Check All";
            }
            else
            {
                //Check all boxes
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    row.Cells[colName_CheckBox].Value = true;
                }
                btnSelectAll.Text = "Uncheck All";
            }
        


        }

        private void btnCopyGrid_Click(object sender, EventArgs e)
        {
            SharedUtilities su = new SharedUtilities();
            su.CopyGridContents(dataGridView1);        
        }

        private void btnSearchForPatient_Click(object sender, EventArgs e)
        {
            SharedUtilities su = new SharedUtilities();
            su.SearchGrid("SendStatementCheckBox", dataGridView1, this.SearchValue.Text,1);
        }

        private void btnSearchForFeeSlip_Click(object sender, EventArgs e)
        {

            string formname = "FeeslipsWithUnknownIns";
            string searchValue = "";
            int searchColumn = 1;



            SharedUtilities su = new SharedUtilities();
            //su.SearchGrid("FeeslipsWithUnknownIns", dataGridView1, this.SearchValue.Text, 1);

            FeeslipsWithUnknownIns fc = (FeeslipsWithUnknownIns)Application.OpenForms[formname];
            DataGridView dgv = fc.dataGridView1;

            int i = 0;
            fc.lblFeeslipsFound.Text = "";
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            try
            {
                su.ClearGridSelections(dgv);
                foreach (DataGridViewRow row in dgv.Rows)
                {

                    if (row.Cells[searchColumn].Value.ToString().Equals(searchValue))
                    {
                        row.Selected = true;
                        dgv.FirstDisplayedScrollingRowIndex = dgv.SelectedRows[0].Index;
                        if (fc != null)
                        {
                            fc.lblFeeslipsFound.Text = "Found: " + ++i;
                        }
                    }

                }

            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void SearchValue_Enter(object sender, EventArgs e)
        {
            SearchValue.Text = "";
        }

        private void SearchValue_TextChanged(object sender, EventArgs e)
        {
            SearchGrid();
        }

        private void SearchGrid()
        {
            string formname = "SendStatementCheckBox"; 
            string searchValue = this.SearchValue.Text;
            int searchColumn = 1;

            SharedUtilities su = new SharedUtilities();
            //su.SearchGrid("FeeslipsWithUnknownIns", dataGridView1, this.SearchValue.Text, 1);

            SendStatementCheckBox ss = (SendStatementCheckBox)Application.OpenForms[formname];
            DataGridView dgv = ss.dataGridView1;

            int i = 0;
            ss.lblFeeslipsFound.Text = "";
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            try
            {
                su.ClearGridSelections(dgv);
                foreach (DataGridViewRow row in dgv.Rows)
                {

                    if (row.Cells[searchColumn].Value.ToString().Equals(searchValue))
                    {
                        row.Selected = true;
                        dgv.FirstDisplayedScrollingRowIndex = dgv.SelectedRows[0].Index;
                        if (ss != null)
                        {
                            ss.lblFeeslipsFound.Text = "Found: " + ++i;
                        }
                    }
                }

            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

    
       


    }
}
