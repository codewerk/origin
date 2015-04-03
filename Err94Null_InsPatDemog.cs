using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace Fixit
{
    public partial class Err94Null_InsPatDemog : Form
    {
        public Err94Null_InsPatDemog()
        {
            InitializeComponent();
        }
        string _clientDatabaseName;
        string _clientServerName;
        string _clientInstanceName;
        string _clientDbFilePath;
        string _SharedDataPath; //CW 2/3/2014 used for logging the fixes into a file in the data folder

        //private void btnFixIT_Click(object sender, EventArgs e)
        //{
        //    GetOmate32iniFileValues();
        //    RemoveProductRecordFromDB();
        //}
        
      
        private void GetOmate32iniFileValues()
        {
            Workstation ws = new Workstation();
            _clientDatabaseName = ws.ClientDatabaseName;
            _clientServerName = ws.ClientServerName;
            _clientInstanceName = ws.ClientInstanceName;
            _clientDbFilePath = ws.ClientDbFilePath;
            _SharedDataPath = ws.SharedDataPath; //CW 2/3/2014 added to support logging the fix in a file in the data folder
        }

        private void btnLookupInsuranceWithPatID_Click(object sender, EventArgs e)
        {                        
            GatherAndDisplayInsuranceCarrierNames();                        
        }

        private void GatherAndDisplayInsuranceCarrierNames(object sender, EventArgs e)
        {

        }        
        
        //not used below
        
        private void GatherAndDisplayInsuranceCarrierNames()
        {
            //Obtain the patient ID from the user
            //Lookup the insurance names in the patient_insurances table and present so the user can select one carrier.
            //Using the insurance_no from the selection lookup the plans from the insurance_plans table where ins_group_no = 0, and present a list of plan names so the user can select one plan
            //Update the patient_insurances table ins_group_no with the plan_no selected in the step above.

            GetOmate32iniFileValues();


            string queryString = "";
            string InsuranceWithNoGroup = "0";
            //similar grid if more than one product exists
            List<Insurance> listInsuranceCarriersForUserSelection = new List<Insurance>();
            List<InsurancePlan> listInsurancePlansForUserSelection = new List<InsurancePlan>();

            //Get the product name from the user
            string value = "enter patient ID number here";
            string insuranceName = "";
            string patientID = txtbxPatientID.Text;

            queryString = @"SELECT pins.insurance_no, ins.insurance_name 
                            FROM patient_insurances pins
                            INNER JOIN insurance ins
                            ON
                            pins.insurance_no = ins.insurance_no
                            WHERE (pins.ins_group_no = 0 OR pins.ins_group_no IS NULL) AND 
                            patient_no = '" + patientID + "'";
                            
             OMSQLDB o = new OMSQLDB();
             
             using (OleDbConnection conn = o.GetOLEConnection ())
                {
                OleDbCommand command = new OleDbCommand(queryString, conn);
                using (OleDbDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        int r = 0;
                        /*Since we will only work with one potential carrier(missing a plan) at a time
                        we will comment out the loop for now. Otherwise need to code user's carrier selection */

                        /*while (reader.Read())
                        {
                            Insurance ins = new Insurance();
                            ins.insurance_no = reader["insurance_no"].ToString();
                            ins.insurance_name  = reader["insurance_name"].ToString();
                            InsuranceWithNoGroup = ins.insurance_no;
                            //listInsuranceCarriersForUserSelection.Add(ins);
                            listInsuranceCarriersForUserSelection.Add(ins);
                        } */

                        if (reader.Read())
                        {
                            Insurance ins = new Insurance();
                            ins.insurance_no = reader["insurance_no"].ToString();
                            ins.insurance_name = reader["insurance_name"].ToString();
                            InsuranceWithNoGroup = ins.insurance_no;
                            //listInsuranceCarriersForUserSelection.Add(ins);
                            listInsuranceCarriersForUserSelection.Add(ins);
                            lblCarrierFound.Text = ins.insurance_name;
                            lblInsuranceCarrierFound_No.Text = ins.insurance_no;
                            btnAssignPlanToCarrier.Text = "Assign Plan to \r " + ins.insurance_name;
                            //lblCarrierFoun.Text = ins.insurance_name;

                            dataGridView1.DataSource = listInsuranceCarriersForUserSelection;
                            dataGridView1.Columns[0].Visible = false;
                            dataGridView1.Columns[1].HeaderText = "Insurance"; 
                        }
                        


                        //Now fill the grid for user selection and processing
                       

                        //Err94Null_InsPatDemog err94SelectionScreen = new Err94Null_InsPatDemog();
                        
                        
                        dataGridView1.DataSource = listInsuranceCarriersForUserSelection;
                        dataGridView1.Columns[0].Visible = false;
                        dataGridView1.Columns[1].HeaderText = "Insurance";                                              
                    
                        
                        //SharedUtilities su = new SharedUtilities();
                        //su.ResizeTheGrid(dataGridView1);
                        
                        //err94SelectionScreen.dataGridView1.DataSource = listInsuranceCarriersForUserSelection;
                        //err94SelectionScreen.Show();
                    }
                    
                }

                //Now get second grid data using plan if a carrier without a group was found above

                if (InsuranceWithNoGroup != "0")
                queryString = @"SELECT plan_no, plan_name from insurance_plans WHERE insurance_no  = " + InsuranceWithNoGroup;
                
                OleDbCommand command2 = new OleDbCommand(queryString, conn);
                using (OleDbDataReader reader = command2.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            InsurancePlan insplan = new InsurancePlan();
                            insplan.plan_no  = reader["plan_no"].ToString();
                            insplan.plan_name  = reader["plan_name"].ToString();
                            listInsurancePlansForUserSelection.Add(insplan);
                        }                        
                        dataGridView2.DataSource = listInsurancePlansForUserSelection;
                        dataGridView2.ClearSelection(); //To avoid confusing the user from thinking the 1st cell selection means that carrier/row is selected
                        dataGridView2.Columns[0].Visible = false;
                        dataGridView2.Columns[1].HeaderText = "Plan";

                        SharedUtilities su = new SharedUtilities();
                        su.ResizeTheGrid(dataGridView2);
                        btnAssignPlanToCarrier.Enabled = true;
                        this.dataGridView2.Visible = true ;
                        
                    }
                    else // 3/5/2014 CW Added to clear the data from previous tasks
                    {
                        lblCarrierFound.Text = "";
                        lblInsuranceCarrierFound_No.Text = "";
                        btnAssignPlanToCarrier.Text =string.Format("Plan not missing for patient #{0}", patientID);
                        btnAssignPlanToCarrier.Enabled = false;
                        this.dataGridView2.DataSource = null;
                        this.dataGridView2.Rows.Clear();
                        this.dataGridView2.Visible = false;
                        
                        

                    }

                }


            }
        }

        private void btnAssignPlanToCarrier_Click(object sender, EventArgs e)
        {
                                    
            //Update the patient_insurances table ins_group_no 
            //with the plan_no selected in GatherAndDisplayInsuranceCarrierNames().
            
            GetOmate32iniFileValues();
            bool ConnectionSucceeded;
            string queryString2 = "";
            string InsuranceWithNoGroup = "0";
            //similar grid if more than one product exists
            List<Insurance> listInsuranceCarriersForUserSelection = new List<Insurance>();
            List<InsurancePlan> listInsurancePlansForUserSelection = new List<InsurancePlan>();

            //Get the product name from the user
            string value = "enter patient ID number here";
            string insuranceName = "";
            string patientID = txtbxPatientID.Text;
            queryString2 = "l";

            string plan_name = "";
            int _colPositionOf_plan_no = 0;
            int _colPositionOf_plan_name = 1;
            InsurancePlan patplan = new InsurancePlan(); //used in the Update query          

            
            OMSQLDB oms = new OMSQLDB();


                #region Get The Plan Selected By The User
                //display the selections from the grid in a message box
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    //if (row.Selected == true)
                    //At this time, we are only presenting one row, so go through this code always
                    
                        patplan.plan_no = row.Cells[_colPositionOf_plan_no].Value.ToString();
                        patplan.plan_name  = row.Cells[_colPositionOf_plan_name].Value.ToString();
                        #region Set The Plan in the Insurance Record
                        //using (OleDbConnection conn = new OleDbConnection())
                        
                        using (OleDbConnection conn = oms.GetOLEConnection())
                        {
                            queryString2 = @"Update patient_insurances SET ins_group_no = " + patplan.plan_no +
                                            " WHERE (insurance_no = " + lblInsuranceCarrierFound_No.Text  + ") AND patient_no = " + patientID;

                            conn.ConnectionString =
                            "Provider = SQLOLEDB;" +
                            "Driver=SQLOLEDB;" +
                            "Data Source=tcp:" + _clientServerName + "\\" + _clientInstanceName + ";" +
                            "Initial Catalog=" + _clientDatabaseName + ";" +
                            "User id=OM_USER;" + ";" +
                            "Password=OMSQL@2004;";

                            try
                            {
                                using (OleDbCommand command = new OleDbCommand(queryString2, conn))
                                {
                                    try
                                    {
                                        conn.Open();
                                        command.ExecuteNonQuery();
                                        command.Dispose();
                                        conn.Close();
                                        MessageBox.Show("Solution Completed", "Fix It");
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show("Can not open connection ! " + ex.ToString());
                                    }
                                }

                            }


                            catch (OleDbException f)
                            {
                                //_ConnectionSucceeded = false;
                                MessageBox.Show(f.Message, f.ErrorCode.ToString());
                                //Uncomment the section below when more error detail is needed.
                                //string message = f.ToString();
                                //string caption = "Error";
                                //var result = MessageBox.Show(message, caption,
                                //                             MessageBoxButtons.OK,
                                //                             MessageBoxIcon.Exclamation);
                            }
                        }
                        #endregion

                        FixitLog fixLog = new FixitLog();                        
                        //fixLog.CreateLog("Err94Null_InsPatDemog", "insuranceplan_no, insuranceplan_name", patplan.plan_no + "~" + patplan.plan_name, _sharedDataPath);
                    
                }                
                #endregion  
 
                
        }
        
        private void SearchValue_Enter(object sender, EventArgs e) //blank the current field value when entered
        {
            ClearFieldValue(sender);
        }

        private void SearchValue_TextChanged(object sender, EventArgs e)//search the grid
        {
            SearchGrid();
            ClearFieldValue(sender);                      
        }

        public void ClearFieldValue(object sender)//3/5/2014 CW Added to centralize clearing the search field value
        {            
            SharedUtilities su = new SharedUtilities();
            su.ClearTextBox((TextBox)sender);
            this.dataGridView1.DataSource = null;
            this.dataGridView1.Rows.Clear();         
        } 

        private void SearchGrid()
        {
            string formname = "Err94Null_InsPatDemog";
            string searchValue = this.txtbxPatientID.Text;
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
