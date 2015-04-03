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
    public partial class ProductAlreadyExistsProcessing : Form
    {
        public ProductAlreadyExistsProcessing()
        {
            InitializeComponent();
        }
        string _clientDatabaseName;
        string _clientServerName;
        string _clientInstanceName;
        string _clientDbFilePath;
        string _SharedDataPath; //CW 2/3/2014 used for logging the fixes into a file in the data folder

        private void btnFixIT_Click(object sender, EventArgs e)
        {
            GetOmate32iniFileValues();
            RemoveProductRecordFromDB();
            
        }

        private void RemoveProductRecordFromDB()
        {            
            // string selectedFeeslips = "";
            int chkbox;
            bool isSqlClient = false;
            OMSQLDB oms = new OMSQLDB();
            isSqlClient = oms.SqlClientCheck();

            //display the selections from the grid in a message box
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                chkbox = (row.Cells[0].Value == null) ? 0 : 1;
                //if (Convert.ToBoolean(row.Cells[0].Value) == false)
                //if ((string)row.Cells[0].Value == "1")
                //if ((bool)(row.Cells[0].Value) == true)
                if (Convert.ToBoolean(chkbox) == true)
                {
                    //MessageBox.Show(temp);
                    //selectedFeeslips += selectedFeeslips;
                    Product p = new Product();
                    p.Prd_no = row.Cells["Prd_no"].Value.ToString();
                    p.Prd_style_name = row.Cells["Prd_style_name"].Value.ToString();
                    p.Prd_new_since = row.Cells["Prd_new_since"].Value.ToString();

                    //Remove the product from the table

                    if (!isSqlClient)
                    {
                        p.RemoveProduct_SQL(p);
                        
                    }
                    else
                    {                        
                        if (p.IsProductInProdDetails_OLE(p))//Escalate to Tier 3 support, if the prod is in the prod details table
                        {
                            string message = "A product detail record was found; Escalate to Tier 3 support";
                            string caption = "Product Detail record found";
                            var result = MessageBox.Show(message, caption,
                                                         MessageBoxButtons.OK,
                                                         MessageBoxIcon.Stop);
                        }
                        else
                        {
                            p.RemoveProduct_OLE(p);
                            
                            
                        }
                    }

                }
                else
                {
                   
                }

            }
            FixitLog fixLog = new FixitLog();
            fixLog.CreateLog("ProductAlreadyExists", ref dataGridView1, _SharedDataPath);

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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void lbInstructions_Click(object sender, EventArgs e)
        {

        }

    }
}
