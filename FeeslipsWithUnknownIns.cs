using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Fixit
{
    public partial class FeeslipsWithUnknownIns : Form
    {
        public FeeslipsWithUnknownIns()
        {
            InitializeComponent();
        }
        string _clientDatabaseName;
        string _clientServerName;
        string _clientInstanceName;
        string _clientDbFilePath;
        string _SharedDataPath;
       
        bool isGridDirty = false; //this is only used by the ModifyCheckAllButton method, to flag if it's the first chk/unchk on the grid
        //the method should only run the first time a box is checked, so the button text can be set.
        bool isAnyRowChecked = false; //used to flag if any checkboxes on a row are checked.

        private void btnFixIT_Click(object sender, EventArgs e)
        {
            GetOmate32iniFileValues();
            string selectedFeeslips = "";
            int chkbox;            
            bool isSqlClient = false;
            OMSQLDB oms = new OMSQLDB();
            isSqlClient = oms.SqlClientCheck();

            int _colPositionOf_Feeslip_no = 1;
            int _colPositionOf_SlipItem_no = 2;
            int _colPositionOf_PatientTotal = 3;
            int _colPositionOf_InsuranceTotal = 4;
            
            //display the selections from the grid in a message box
            foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                                     
                    //the mistake here is that while it solves the error of a possible null value,
                    //it results in forcing the box to be checked if it isn't null
                    //chkbox = (row.Cells[0].Value == null) ? 0 : 1;
                    //we changed the code where the grid is populated so that it is automatically checked

                    chkbox = (Convert.ToBoolean(row.Cells[0].Value) == false) ? 0 : 1;
                    //if (Convert.ToBoolean(row.Cells[0].Value) == false)
                    //if ((string)row.Cells[0].Value == "1")
                    //if ((bool)(row.Cells[0].Value) == true)
                    if (Convert.ToBoolean(chkbox) == true)
                    {
                        //MessageBox.Show(temp);
                        selectedFeeslips += selectedFeeslips;
                        FeeSlip fs = new FeeSlip();
                        
                        //the code cover caused issues with these names, so using position instead
                        //fs.Feeslip_no = row.Cells["Feeslip_no"].Value.ToString();
                        //fs.SlipItem_no = row.Cells["SlipItem_no"].Value.ToString();
                        //fs.InsuranceTotal = row.Cells["InsuranceTotal"].Value.ToString();
                        //string _patTotal = (row.Cells["PatientTotal"].Value.ToString() != null) ? row.Cells["PatientTotal"].Value.ToString() : "0"; //this will capture all the fees for the line item, to allow adjusting in the ledger

                        //see comments above re: using col pos instead
                        fs.Feeslip_no = row.Cells[_colPositionOf_Feeslip_no].Value.ToString();
                        fs.SlipItem_no = row.Cells[_colPositionOf_SlipItem_no].Value.ToString();
                        fs.InsuranceTotal = row.Cells[_colPositionOf_InsuranceTotal].Value.ToString();
                        string _patTotal = (row.Cells[_colPositionOf_PatientTotal].Value.ToString() != null) ? row.Cells[_colPositionOf_PatientTotal].Value.ToString() : "0"; //this will capture all the fees for the line item, to allow adjusting in the ledger                        
                                                
                        fs.PatientTotal = _patTotal;     
                        //Update the table, moving the fee to the patient
                        FindAndFixCases ffc = new FindAndFixCases();                       
                        //ffc.ProcessUnknownInsurances_OLE(fs);
                        
                        if (isSqlClient)
                        {
                            ffc.ProcessUnknownInsurances_SQL(fs);
                        }
                        else
                        {                                                                      
                            ffc.ProcessUnknownInsurances_OLE(fs);
                        }
                        //MessageBox.Show("Solution Completed", "Fix It");    
                    }                                                                     
                }
                MessageBox.Show("Solution Completed", "Fix It");
                FixitLog fixLog = new FixitLog();
                fixLog.CreateLog("UnknownInsurance", ref dataGridView1, _SharedDataPath);
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
            _SharedDataPath = ws.SharedDataPath;
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
            string formname = "FeeslipsWithUnknownIns";
            string searchValue = this.SearchValue.Text;
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


    }
}
