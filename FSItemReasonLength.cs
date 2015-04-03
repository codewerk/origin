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
    public partial class FSItemReasonLength : Form
    {
        public FSItemReasonLength()
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

        private void btnFixIT_Click(object sender, EventArgs e)//shorten the itmtrn_reason field in table fee_slip_items_trans
        {
            GetOmate32iniFileValues();
            string selectedFeeslips = "";
            int chkbox;
            bool isSqlClient = false;
            OMSQLDB oms = new OMSQLDB();
            isSqlClient = oms.SqlClientCheck();
            
            int _colPositionOf_slipitmtrn_no = 1;
            
            string SlipItemTrn_no = "";
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
                    SlipItemTrn_no = row.Cells[_colPositionOf_slipitmtrn_no].Value.ToString();                    
                    FindAndFixCases ffc = new FindAndFixCases();                       
                    if (isSqlClient)
                    {
                        
                    }
                    else
                    {
                        ffc.ShortenFSItmReason_OLE(SlipItemTrn_no);
                    }
                    //MessageBox.Show("Solution Completed", "Fix It");    
                }
            }
            MessageBox.Show("Solution Completed", "Fix It");
            FixitLog fixLog = new FixitLog();
            fixLog.CreateLog("Solution 523 Err 217887", ref dataGridView1, _SharedDataPath);

            #region makecolumnssort
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {

                dataGridView1.Columns[column.Name].SortMode = DataGridViewColumnSortMode.Automatic;
            }

              foreach (DataGridViewColumn column in dataGridView1.Columns)
            column.SortMode = DataGridViewColumnSortMode.Automatic;
    

            #endregion
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {

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
            
            SharedUtilities su = new SharedUtilities();
            su.SearchGrid("FSItemReasonLength", dataGridView1, this.SearchValue.Text, 2);
        }

        private void SearchValue_Enter(object sender, EventArgs e)
        {
            SearchValue.Text = "";

        }

        private void SearchValue_TextChanged(object sender, EventArgs e)
        {
            SharedUtilities su = new SharedUtilities();
            su.SearchGrid("FSItemReasonLength", dataGridView1, this.SearchValue.Text, 2);

        }//end


    }
}
