using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using omSupportBackup;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Reflection;
using System.Drawing;

namespace Fixit
{
    class FindAndFixCases 
    {        
        private MainForm form1;
        string selectedCaseCatcher;
        string selectedCaseSolution;

        string _clientDatabaseName;
        string _clientServerName;
        string _clientInstanceName;
        string _clientDbFilePath;
        string _BakFileSuffix;
        bool _backupSucceeded;
        bool _ConnectionSucceeded;
        string _SharedDataPath; //CW 2/3/2014 used for logging the fixes into a file in the data folder

        Solutions sols = new Solutions(); //this class contains the sql for solutions and catchers

         public FindAndFixCases(string _selectedSolution)
        {
            selectedCaseCatcher = _selectedSolution;
            selectedCaseSolution = _selectedSolution;
            //PopulateBkUpScreen();
            GetOmate32iniFileValues();
           // string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            
             //Version version = Assembly.GetExecutingAssembly().GetName().Version;
             //string myText = " " + version.Major + "." + version.Minor + " (build " + version.Build + ")"; //change form title
             //MainForm.ActiveForm.Text = MainForm.ActiveForm.Text + " " + myText ;

            //MainForm.ActiveForm.Text = MainForm.ActiveForm.Text + " " + version;
        }

         public FindAndFixCases() 
             //no parameter when called from the form = FeeslipsWithUnknownIns
             //which just needs to use the ProcessUnknownInsurance method, since it gathers feeslip data into a feeslip object
         {//todo: Can look to remove this redundant call, since OMSQLDB class handles it.
             GetOmate32iniFileValues();
         }

         public void FindItOleClient()//  WORKS called from form1 
        {
            string selectedSolution = selectedCaseCatcher;
            string queryString = sols.GetSQLFromCaseCatcher(selectedSolution);
            string selectedSolutionNumber = selectedCaseCatcher.Substring(0, 8);
            
             //Populate list which may later be used to populate datagrid have user select
            //specific feeslips to resolve unk insurance.                                     
            List<FeeSlip> listFeeSlipsForUserSelection = new List<FeeSlip>();
            FeeslipsWithUnknownIns fsForm = new FeeslipsWithUnknownIns();

            //Populate list which may later be used to populate datagrid have user select
            //specific patients to check the send statement checkbox of the patient demographic
            List<Patient> listPatientsForUserSelection = new List<Patient>();
            SendStatementCheckBox  patForm = new SendStatementCheckBox ();
            

            //Populate list which may later be used to populate datagrid have user select
            //specific insurance items to resolve missing group (AKA plan number)
            List<Insurance> listInsuranceForUserSelection = new List<Insurance>();
            //SendStatementCheckBox patForm = new SendStatementCheckBox();

            //Populate list which may later be used to populate datagrid for display/print            
            List<Exam > listPatientsWithBMIerrs = new List<Exam >();
            SendStatementCheckBox examForm = new SendStatementCheckBox();

            //Populate list which may later be used to populate datagrid have user select
            //specific feeslips to resolve long reasons.                                     
            List<FeeSlipItmTrans > listFeeSlipsForUserSelection_LongReason = new List<FeeSlipItmTrans>();
            FeeSlipItmTrans fsForm_LongReason = new FeeSlipItmTrans();

            //Populate list which may later be used as a list for user selection in a text box, to edit state
            List<FeeSlip> listFeeslipNumbersForEditState = new List<FeeSlip>();
            OMSQLDB o = new OMSQLDB();
             
                 using (OleDbConnection conn = o.GetOLEConnection ())
            {

                try
                {
                        OleDbCommand command = new OleDbCommand(queryString, conn);
                        using (OleDbDataReader reader = command.ExecuteReader())
                        {
                            string sqlResults = "";

                            while (reader.Read())
                            {
                                //Unknown insurance requires a different field returned and treatment
                               switch (selectedSolutionNumber)
                               {
                                   case "00000523": //Error 217887 when selecting insurance drop-down on insurance ledger tab
                                       sqlResults += reader[0] + "\n";
                                       FeeSlipItmTrans fsItmTrnReason = new FeeSlipItmTrans();
                                       fsItmTrnReason.Itmtrn_no = reader["itmtrn_no"].ToString();
                                       fsItmTrnReason.Feeslip_no = reader["feeslip_no"].ToString();
                                       fsItmTrnReason.SlipItem_no = reader["slipitm_no"].ToString();
                                       fsItmTrnReason.Itmtrn_reason = reader["Itmtrn_reason"].ToString();
                                       fsItmTrnReason.Itmtrn_reason_length = reader["Length"].ToString();

                                       //replace null values with zeroes, to prevent errors as these values
                                       //are passed to the grid, processed by other classes/methods                                       
                                       fsItmTrnReason.Itmtrn_no = (fsItmTrnReason.Itmtrn_no != null) ? fsItmTrnReason.Itmtrn_no : "0";
                                       fsItmTrnReason.Feeslip_no = (fsItmTrnReason.Feeslip_no != null) ? fsItmTrnReason.Feeslip_no : "0";
                                       fsItmTrnReason.SlipItem_no = (fsItmTrnReason.SlipItem_no != null) ? fsItmTrnReason.SlipItem_no : "0";
                                       fsItmTrnReason.Itmtrn_reason = (fsItmTrnReason.Itmtrn_reason != null) ? fsItmTrnReason.Itmtrn_reason : "0";
                                       fsItmTrnReason.Itmtrn_reason_length = (fsItmTrnReason.Itmtrn_reason_length != null) ? fsItmTrnReason.Itmtrn_reason_length : "0";
                                       listFeeSlipsForUserSelection_LongReason.Add(fsItmTrnReason);
                                       break;
                                   //-------------------------------------------------------------------------------------------
                                   case "00000726": //Third Party Processing - Error 217900- Incorrect syntax
                                       sqlResults += reader[0] + "\n";
                                       Patient pt = new Patient();
                                       pt.Patient_ID = reader["patient_no"].ToString();
                                       pt.Last_Name = reader["last_name"].ToString();
                                       pt.First_Name = reader["first_name"].ToString();
                                       pt.First_Name = reader["first_name"].ToString();
                                       pt.First_Name = reader["first_name"].ToString();

                                       //replace null values with zeroes, to prevent errors as these values
                                       //are passed to the grid, processed by other classes/methods
                                       pt.Patient_ID = (pt.Patient_ID != null) ? pt.Patient_ID : "";
                                       pt.Last_Name = (pt.Last_Name != null) ? pt.Last_Name : "";
                                       pt.First_Name = (pt.First_Name != null) ? pt.First_Name : "";

                                       listPatientsForUserSelection.Add(pt);
                                       break;

                                   //-------------------------------------------------------------------------------------------
                                   case "00001466": //Unknown Insurance
                                       sqlResults += reader[0] + "\n"; //The feeslip number field in the table
                                       FeeSlip fs = new FeeSlip();
                                       fs.Feeslip_no = reader["feeslip_no"].ToString();
                                       fs.SlipItem_no = reader["slipitm_no"].ToString();
                                       fs.InsuranceTotal = reader["slipitm_ins_total"].ToString();
                                       fs.PatientTotal = reader["slipitm_pat_total"].ToString();

                                       //replace null values with zeroes, to prevent errors as these values
                                       //are passed to the grid, processed by other classes/methods
                                       fs.Feeslip_no = (fs.Feeslip_no != null) ? fs.Feeslip_no : "0";                                       
                                       fs.SlipItem_no = (fs.SlipItem_no != null) ? fs.SlipItem_no : "0";
                                       fs.InsuranceTotal = (fs.InsuranceTotal != null) ? fs.InsuranceTotal : "0";
                                       fs.PatientTotal = (fs.PatientTotal != null) ? fs.PatientTotal : "0";

                                       listFeeSlipsForUserSelection.Add(fs);
                                        //Use the line below to create a new obj in one line
                                        //listFeeSlipsForUserSelection.Add(new FeeSlip { Feeslip_no = reader[0].ToString(), PatientTotal = reader[3].ToString(), InsuranceTotal = reader[2].ToString() });                                       
                                       break;
                                //-------------------------------------------------------------------------------------------

                                   case "00001579": //Third Party Processing - Error 217900- Incorrect syntax
                                       sqlResults += reader[0] + "\n";
                                       Insurance ins = new Insurance();
                                       //ins.plan_name = reader["plan_name"].ToString();


                                       //replace null values with zeroes, to prevent errors as these values
                                       //are passed to the grid, processed by other classes/methods
                                       //pt.Patient_ID = (pt.Patient_ID != null) ? pt.Patient_ID : "";
                                       //pt.Last_Name = (pt.Last_Name != null) ? pt.Last_Name : "";
                                       //pt.First_Name = (pt.First_Name != null) ? pt.First_Name : "";                                       
                                       listInsuranceForUserSelection.Add(ins);
                                       break;                                  
                               
                               //-------------------------------------------------------------------------------------------

                                   case "00001585": //Send statement box is not checked
                                       sqlResults += reader[0] + "\n";
                                       Patient pat = new Patient();
                                       pat.Patient_ID = reader["patient_no"].ToString();
                                       pat.Last_Name = reader["last_name"].ToString();
                                       pat.First_Name = reader["first_name"].ToString();

                                       //replace null values with zeroes, to prevent errors as these values
                                       //are passed to the grid, processed by other classes/methods
                                       pat.Patient_ID = (pat.Patient_ID != null) ? pat.Patient_ID : "";
                                       pat.Last_Name = (pat.Last_Name != null) ? pat.Last_Name : "";
                                       pat.First_Name = (pat.First_Name != null) ? pat.First_Name : "";

                                       listPatientsForUserSelection.Add(pat);
                                       break;

                                   //--------------------------------------------------------------------------------------
                                   case "00003085": //
                                       sqlResults += reader[0] + "\n";
                                       FeeSlip fe = new FeeSlip();
                                       fe.Feeslip_no  = reader["feeslip_no"].ToString();                                      
                                       listFeeslipNumbersForEditState.Add(fe);
                                       break;
                                   
                                   //--------------------------------------------------------------------------------------

                                   case "00007481": //Error 217833 Arithmetic overflow running NQF0421 POP1/POP2 this is from Scott A.
                                       sqlResults += reader[0] + "\n";
                                       //field list for this sql is:
                                       // recid, ExamRec, PatientNo, first_name, last_name,  RecType, Text2
                                       Exam exm = new Exam();
                                       exm.ExamRec = reader["ExamRec"].ToString();
                                       exm.PatientNo  = reader["PatientNo"].ToString();
                                       exm.first_name = reader["first_name"].ToString();
                                       exm.last_name = reader["last_name"].ToString();
                                       exm.RecType = reader["RecType"].ToString();
                                       exm.Text2 = reader["Text2"].ToString();
                                       //replace null values with zeroes, to prevent errors as these values
                                       //are passed to the grid, processed by other classes/methods
                                       exm.ExamRec = (exm.ExamRec != null) ? exm.ExamRec : "";
                                       exm.first_name = (exm.first_name != null) ? exm.first_name : "";
                                       exm.last_name = (exm.last_name != null) ? exm.last_name : "";
                                       exm.RecType = (exm.RecType != null) ? exm.RecType : "";
                                       exm.Text2 = (exm.Text2 != null) ? exm.Text2 : "";
                                       
                                       listPatientsWithBMIerrs.Add(exm);

                                       break;
                                   //--------------------------------------------------------------------------------------

                                   case "00007563": //EHR client reports not being able to send any demographic due to unencrypted CreditCard numbers
                                      
                                       sqlResults += reader[0] + "\n";
                                       Patient p = new Patient();
                                       p.Patient_ID = reader["patient_no"].ToString();
                                       p.Last_Name = reader["last_name"].ToString();
                                       p.First_Name = reader["first_name"].ToString();

                                       //replace null values with zeroes, to prevent errors as these values
                                       //are passed to the grid, processed by other classes/methods
                                       p.Patient_ID = (p.Patient_ID != null) ? p.Patient_ID : "";
                                       p.Last_Name = (p.Last_Name != null) ? p.Last_Name : "";
                                       p.First_Name = (p.First_Name != null) ? p.First_Name : "";

                                       listPatientsForUserSelection.Add(p);
                                       break;
                               //--------------------------------------------------------------------------------------
                                   default:
                                       sqlResults += reader[0] + "\n"; //The first field name in the sql table
                                       break;
                               }
                                
                            }

                            if (reader.HasRows)//the list<> is populated in the while loop above, now just build the grid with the list<> as the data source.
                            {
                                switch (selectedSolutionNumber)
                                {
                                    case "00000523": //Error 217887 When selecting an insurance in the patient ledger dropdown list
                                        //show a list of feeslips with reasons longer than 75.
                                        //READ-ONLY UI this should now only be run from FindIT, there is a read-only UI

                                        FSItemReasonLength FSlongReason = new FSItemReasonLength();
                                        FSlongReason.dataGridView1.DataSource = listFeeSlipsForUserSelection_LongReason;
                                        DataGridViewCheckBoxColumn CBColumnFS_reason = new DataGridViewCheckBoxColumn();
                                        CBColumnFS_reason.HeaderText = "Resolve";
                                        CBColumnFS_reason.Width = 55;
                                        CBColumnFS_reason.FalseValue = 0;
                                        CBColumnFS_reason.TrueValue = 1;
                                        FSlongReason.dataGridView1.Columns.Insert(0, CBColumnFS_reason);
                                        //note this is only hidden for this solution, since it is displaying a list only
                                        CBColumnFS_reason.Visible = true;

                                        //renaming columns with friendly header titles
                                        //FSlongReason.dataGridView1.Columns[1].HeaderText = "FeeSlip#";
                                        FSlongReason.dataGridView1.Columns[1].Width = 60;
                                        //FSlongReason.dataGridView1.Columns[2].Visible = false;
                                        FSlongReason.dataGridView1.Columns[3].Visible = false;
                                        FSlongReason.dataGridView1.Columns[4].HeaderText = "Length";
                                        FSlongReason.dataGridView1.Columns[4].Width = 60;
                                        FSlongReason.dataGridView1.Columns[5].HeaderText = "Reason";
                                        FSlongReason.dataGridView1.Columns[5].Width = 600;
                                        SharedUtilities su = new SharedUtilities();
                                        //su.ResizeTheGrid(FSlongReason.dataGridView1);
                                        //su.SetColumnToSortable(FSlongReason.dataGridView1,"Feeslip_no");
                                        //su.SetColumnToSortable(FSlongReason.dataGridView1, "Itmtrn_reason_length");

                                        FSlongReason.Show();

                                        //make all checked by default   
                                        //needed this to try and resolve the checkall fail in another module

                                        for (int i = 0; i < FSlongReason.dataGridView1.Rows.Count - 1; i++)
                                        {
                                            FSlongReason.dataGridView1.Rows[i].Cells[0].Value = true;
                                        }

                                        FSlongReason.dataGridView1.RefreshEdit();
                                        //FSlongReason.CheckAllBoxes();

                                        break;
                                    //------------------------------------------------------------------------------------------
                                    case "00000726": //Third Party Processing - Error 217900- Incorrect syntax
                                        //show a list of patients with illegal characters.
                                        //READ-ONLY UI this should now only be run from FindIT, there is a read-only UI

                                        PunctuationCharacter formPunctuation = new PunctuationCharacter();
                                        formPunctuation.dataGridView1.DataSource = listPatientsForUserSelection;
                                        DataGridViewCheckBoxColumn CBColumnPt = new DataGridViewCheckBoxColumn();
                                        CBColumnPt.HeaderText = "Resolve";
                                        CBColumnPt.FalseValue = 0;
                                        CBColumnPt.TrueValue = 1;
                                        formPunctuation.dataGridView1.Columns.Insert(0, CBColumnPt);
                                        //note this is only hidden for this solution, since it is displaying a list only
                                        CBColumnPt.Visible = false;

                                        formPunctuation.Show();

                                        //make all checked by default   
                                        //needed this to try and resolve the checkall fail in another module

                                        //for (int i = 0; i < formPunctuation.dataGridView1.Rows.Count - 1; i++)
                                        //{
                                        //    formPunctuation.dataGridView1.Rows[i].Cells[0].Value = true;
                                        //}

                                        //formPunctuation.dataGridView1.RefreshEdit();
                                        //formPunctuation.CheckAllBoxes();

                                        break;
                                    //------------------------------------------------------------------------------------------                           

                                    case "00001466": //Unknown Insurance
                                        FeeslipsWithUnknownIns formfeeslipswithUNK = new FeeslipsWithUnknownIns();
                                        formfeeslipswithUNK.dataGridView1.DataSource = listFeeSlipsForUserSelection;
                                        DataGridViewCheckBoxColumn CBColumn = new DataGridViewCheckBoxColumn();
                                        CBColumn.HeaderText = "Resolve";
                                        CBColumn.FalseValue = 0;
                                        CBColumn.TrueValue = 1;
                                        formfeeslipswithUNK.dataGridView1.Columns.Insert(0, CBColumn);
                                        formfeeslipswithUNK.Show();
                                        //make all checked by default   
                                        //needed this to try and resolve the checkall fail in another module
                                        for (int i = 0; i < formfeeslipswithUNK.dataGridView1.Rows.Count - 1; i++)
                                        {
                                            formfeeslipswithUNK.dataGridView1.Rows[i].Cells[0].Value = true;                                                                                 
                                        }
                                        formfeeslipswithUNK.dataGridView1.RefreshEdit();
                                        formfeeslipswithUNK.CheckAllBoxes();
                                        
                                    // formfeeslipswithUNK.dataGridView1.Invalidate();
                                        break;
                                    //------------------------------------------------------------------------------------------
                                    case "00001579": //Third Party Processing - Error 217900- Incorrect syntax
                                        //show a list of patients with illegal characters.
                                        //READ-ONLY UI this should now only be run from FindIT, there is a read-only UI

                                        Err94Null_InsPatDemog err94nullpatins = new Err94Null_InsPatDemog();
                                        err94nullpatins.dataGridView1.DataSource = listInsuranceForUserSelection;
                                        DataGridViewCheckBoxColumn CBColumnPt_insurance = new DataGridViewCheckBoxColumn();
                                        CBColumnPt_insurance.HeaderText = "Resolve";
                                        CBColumnPt_insurance.FalseValue = 0;
                                        CBColumnPt_insurance.TrueValue = 1;
                                        err94nullpatins.dataGridView1.Columns.Insert(0, CBColumnPt_insurance);
                                        //note this is only hidden for this solution, since it is displaying a list only
                                        CBColumnPt_insurance.Visible = true;

                                        err94nullpatins.Show();

                                        //make all checked by default   
                                        //needed this to try and resolve the checkall fail in another module

                                        for (int i = 0; i < err94nullpatins.dataGridView1.Rows.Count - 1; i++)
                                        {
                                            err94nullpatins.dataGridView1.Rows[i].Cells[0].Value = true;
                                        }

                                        err94nullpatins.dataGridView1.RefreshEdit();
                                        //err94nullpatins.CheckAllBoxes();

                                        break;

                                    //--------------------------------------------------------------------------------------
        
                                    case "00001585": //Send statement box is not checked
                                        //this should now only be run from FindIT, there is a UI
                                        
                                        SendStatementCheckBox  formpatientsSendStatement = new SendStatementCheckBox ();
                                        formpatientsSendStatement.dataGridView1.DataSource = listPatientsForUserSelection ;
                                        DataGridViewCheckBoxColumn CBColumnPat = new DataGridViewCheckBoxColumn();
                                        CBColumnPat.HeaderText = "Resolve";
                                        CBColumnPat.FalseValue = 0;
                                        CBColumnPat.TrueValue = 1;
                                        formpatientsSendStatement.dataGridView1.Columns.Insert(0, CBColumnPat);
                                        formpatientsSendStatement.Show();
                                        
                                        //make all checked by default   
                                        //needed this to try and resolve the checkall fail in another module
                                        
                                        for (int i = 0; i < formpatientsSendStatement.dataGridView1.Rows.Count - 1; i++)
                                        {
                                            formpatientsSendStatement.dataGridView1.Rows[i].Cells[0].Value = true;
                                        }

                                        formpatientsSendStatement.dataGridView1.RefreshEdit();
                                        // CW 1/14/2014 this was needed because the last row would not check
                                        // regardless of how many rows.
                                        formpatientsSendStatement.CheckAllBoxes();
                                      
                                        break;


                                    //--------------------------------------------------------------------------------------
                                    case "00003085": //Edit state of feeslip
                                        //show a form with an autocomplete text box list of feeslips                                        
                                        EditStateEntry eForm = new EditStateEntry(listFeeslipNumbersForEditState);                                                                               
                                        eForm.Show();
                                        break;

                                    //--------------------------------------------------------------------------------------
                                  
                                    case "00007481": // This is BMI calc in EW, not send statement, just using the objs.                                                                         

                                        SendStatementCheckBox formpatientsSendStatement2 = new SendStatementCheckBox();
                                        formpatientsSendStatement2.dataGridView1.DataSource = listPatientsWithBMIerrs; 
                                        DataGridViewCheckBoxColumn CBColumnPat2 = new DataGridViewCheckBoxColumn();
                                        CBColumnPat2.HeaderText = "Resolve";
                                        CBColumnPat2.FalseValue = 0;
                                        CBColumnPat2.TrueValue = 1;
                                        CBColumnPat2.Visible = false;
                                        formpatientsSendStatement2.dataGridView1.Columns.Insert(0, CBColumnPat2);

                                        formpatientsSendStatement2.dataGridView1.Columns[1].HeaderText = "Exam#";
                                        formpatientsSendStatement2.dataGridView1.Columns[2].HeaderText = "Patient#";
                                        formpatientsSendStatement2.dataGridView1.Columns[3].HeaderText = "First Name";
                                        formpatientsSendStatement2.dataGridView1.Columns[4].HeaderText = "Last Name";
                                        formpatientsSendStatement2.dataGridView1.Columns[5].HeaderText = "Vital Sign";
                                        formpatientsSendStatement2.dataGridView1.Columns[6].HeaderText = "Measurement";

                                        //resize some colums so all will display without scrolling
                                        int tempWidth = 0;
                                        for (int i = 1; i < 3; i++)
                                        {
                                            tempWidth = formpatientsSendStatement2.dataGridView1.Columns[i].Width;
                                            formpatientsSendStatement2.dataGridView1.Columns[i].Width = tempWidth - 50;
                                        }

                                        formpatientsSendStatement2.Show();

                                        //make all checked by default   
                                        //needed this to try and resolve the checkall fail in another module

                                        for (int i = 0; i < formpatientsSendStatement2.dataGridView1.Rows.Count - 1; i++)
                                        {
                                            formpatientsSendStatement2.dataGridView1.Rows[i].Cells[0].Value = true;
                                        }

                                        formpatientsSendStatement2.dataGridView1.RefreshEdit();
                                        // CW 1/14/2014 this was needed because the last row would not check
                                        // regardless of how many rows.
                                        //formpatientsSendStatement2.CheckAllBoxes();
                                        formpatientsSendStatement2.Text = "BMI Entries Needing Correction";
                                        formpatientsSendStatement2.lbInstructions.Text = "To Copy this list \r";
                                        formpatientsSendStatement2.lbInstructions.Text += "1) Click the Copy button to copy the list \r";
                                        formpatientsSendStatement2.lbInstructions.Text += "2) Use Ctl-v to paste this list into another document.";                                      
                                        break;
                                    //--------------------------------------------------------------------------------------

                                    case "00007563": // This is Unencrypted credit card, not send statement, just reusing the form
                                    
                                        SendStatementCheckBox formpatientsUnencryptedCreditCard = new SendStatementCheckBox();
                                        formpatientsUnencryptedCreditCard.dataGridView1.DataSource = listPatientsForUserSelection;
                                        DataGridViewCheckBoxColumn CBColumnPat1 = new DataGridViewCheckBoxColumn();
                                        CBColumnPat1.HeaderText = "Resolve";
                                        CBColumnPat1.FalseValue = 0;
                                        CBColumnPat1.TrueValue = 1;
                                        formpatientsUnencryptedCreditCard.dataGridView1.Columns.Insert(0, CBColumnPat1);
                                        formpatientsUnencryptedCreditCard.Show();

                                        //make all checked by default   
                                        //needed this to try and resolve the checkall fail in another module

                                        for (int i = 0; i < formpatientsUnencryptedCreditCard.dataGridView1.Rows.Count - 1; i++)
                                        {
                                            formpatientsUnencryptedCreditCard.dataGridView1.Rows[i].Cells[0].Value = true;
                                        }

                                        formpatientsUnencryptedCreditCard.dataGridView1.RefreshEdit();
                                        // CW 1/14/2014 this was needed because the last row would not check
                                        // regardless of how many rows.
                                        formpatientsUnencryptedCreditCard.CheckAllBoxes();

                                        //CW 07/29/2014 Adding customization to the sendstatement form
                                        //Hiding 'Fix' button since this is display only
                                        //Moving all buttons over to the left since the fix button is hidden
                                        //Changing the 3 step instructions in the label //Be careful adjusting this text. Check the UI after any adjustment.
                                        
                                        formpatientsUnencryptedCreditCard.Text = "Unencrypted Credit Card Number";
                                        formpatientsUnencryptedCreditCard.lbInstructions.Text = "The following patients have unencrypted credit card numbers that need to be removed and re-added. \r";
                                        formpatientsUnencryptedCreditCard.lbInstructions.Text += "Click the Copy button then then Ctl-v to paste the list in another application. \r";
                                        
                                        //Hide the Fix buttona and reposition the other buttons
                                        formpatientsUnencryptedCreditCard.btnCopyGrid.Left = formpatientsUnencryptedCreditCard.btnFixIT.Left;                                        
                                        formpatientsUnencryptedCreditCard.btnFixIT.Visible = false;
                                        
                                        //change the title of the form to match this purpose
                                        formpatientsUnencryptedCreditCard.Text = "Unencrypted Credit Card Number";
                                        break;


                                    //--------------------------------------------------------------------------------------
                                   

                                    default:                                      
                                        MessageBox.Show("Results from the Case Catcher: \n" +
                                        "There is a solution to fix records with the following values in the " + reader.GetName(0) + " field: \n" +
                                         sqlResults, "Case Catcher");
                                        break;
                                }
                            }
                            else
                            {
                                MessageBox.Show("There are no records that meet the solution condition", "Case Catcher");
                            }
                        }
                }


                catch (OleDbException f)
                {
                    _ConnectionSucceeded = false;
                    MessageBox.Show(f.Message, f.ErrorCode.ToString());
                    //Uncomment the section below when more error detail is needed.
                    //string message = f.ToString();
                    //string caption = "Error";
                    //var result = MessageBox.Show(message, caption,
                    //                             MessageBoxButtons.OK,
                    //                             MessageBoxIcon.Exclamation);
                }
            }
        }

         public void FixItOleClient()//  called from FixIt_FindItHandler
         {
             string selectedSolution = selectedCaseCatcher;
             string selectedSolutionNumber = selectedCaseCatcher.Substring(0, 8);                        
             string queryString = sols.GetSQLFromSolution(selectedSolution);
             bool isIBal = false;

             //test populating list which will later be used to populate datagrid have user select
             //specific feeslips to resolve unk insurance.                                     
             List<FeeSlip> listFeeSlipsForUserSelection = new List<FeeSlip>();
             FeeslipsWithUnknownIns fsForm = new FeeslipsWithUnknownIns();

             List<FeeSlip> listFeeSlipsForEditState = new List<FeeSlip>();

             List<String> listSolutionsWithUI = new List<String>(); //List of solution that require a special UI/form. Used in switch below
                listSolutionsWithUI.Add("00001492");
                listSolutionsWithUI.Add("00000238"); //need to launch iBal.exe that has been copied to the desktop
                listSolutionsWithUI.Add("00004668");  //product already exists needs to check table before removing a record              
                listSolutionsWithUI.Add("00001585"); //Send statement check box is not checked
                listSolutionsWithUI.Add("00001579"); //Err 94 Invalid Null Insurance tab of patient demographic
                listSolutionsWithUI.Add("00000523"); //Error 217887 when selecting patient's insurance ledger
                //listSolutionsWithUI.Add("00003085"); //Fee slip stuck on hold unable to record or delete
             
             //CW 7/29/2014 Using the instantiation of the OMSQLDB.GetOLEConnection() call to remove the redundant wiring here
             OMSQLDB o = new OMSQLDB();
             using (OleDbConnection conn = o.GetOLEConnection())
             {
                 try
                 {
                     using (OleDbCommand command = new OleDbCommand(queryString, conn))
                     {
                        //If this is simple, single-query update type solution run this otherwise use the datareader 
                        //but if it's a special solution that requires a UI/form go to the next section for processing.
                         bool solutionNeedsUIForm = listSolutionsWithUI.Any(selectedSolutionNumber.Contains);
                         bool donotCallAgain = false; //used for the UI based solutions, but don't need to loop reader.
                         if (!solutionNeedsUIForm)
                            {
                            #region Single Item Update Query
                            
                                try
                                {                                
                                    command.ExecuteNonQuery();
                                    command.Dispose();
                                    conn.Close();
                                    MessageBox.Show("Solution completed");
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Can not open connection ! " + ex.ToString());
                                }
                            #endregion
                            }
                         else //use the datareader for looping action
                            {
                             #region DataReader looping items and User Interface/form used
                             using (OleDbDataReader reader = command.ExecuteReader())
                             {
                                 string sqlResults = "";
                                 while (reader.Read())
                                 {
                                     //Unknown insurance requires a different field returned and treatment
                                     switch (selectedSolutionNumber)
                                     {
                                            case "00000238": //iBal selected for many reasons, so launch iBal with no programming. All user controlled
                                             //Extract the embedded ibal.exe and utilityprep.exefile to the client desktop
                                             Workstation ws = new Workstation();
                                             ws.Extract_iBal_To_Desktop();

                                             //working existing code when the file is already on the destop
                                             string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                                             System.Diagnostics.Process.Start(path + @"\iBal.exe");
                                             isIBal = true;//so exit the reader loop to avoid relaunching iBal                                     
                                             break;

                                            case "00000523": //Error 217887 when selecting patient's insurance ledger
                                             sqlResults += reader[1] + "\n";
                                             FeeSlipItmTrans fsItmTrnReason = new FeeSlipItmTrans();                
                                             fsItmTrnReason.Itmtrn_no = reader["itmtrn_no"].ToString();
                                             fsItmTrnReason.Feeslip_no = reader["feeslip_no"].ToString();
                                             fsItmTrnReason.SlipItem_no = reader["slipitm_no"].ToString();
                                             fsItmTrnReason.Itmtrn_reason = reader["Itmtrn_reason"].ToString();
                                             fsItmTrnReason.Itmtrn_reason_length = reader["Length"].ToString();
                                             //Update the table, truncating the reason field to within 75 characters
                                             ShortenFSItmReason_OLE(fsItmTrnReason.Itmtrn_reason);
                                             break;

                                   
                                         case "00001466": //Unknown Insurance
                                             sqlResults += reader[1] + "\n"; //The 2nd field is the feeslip number in the table
                                             FeeSlip fs = new FeeSlip();
                                             fs.Feeslip_no = reader["feeslip_no"].ToString();
                                             fs.SlipItem_no = reader["slipitm_no"].ToString();
                                             fs.InsuranceTotal = reader["slipitm_ins_total"].ToString();
                                             fs.PatientTotal = reader["slipitm_pat_total"].ToString();

                                             //Update the table, moving the fee to the patient
                                             ProcessUnknownInsurances_OLE(fs);
                                             break;                                  
     

                                        case "00001579": //err94 invalid use of null insurance tab of patient demographic
       
                                             if (donotCallAgain)//need this because a loop is not needed for this solution, and would open multiple forms
                                             {
                                                 break;
                                             }
                                             else
                                             {
                                                 //ResolveErr94NullPatientDemogInsurancetab_OLE();
                                                 Err94Null_InsPatDemog err94SelectionScreen = new Err94Null_InsPatDemog();                                                 
                                                 err94SelectionScreen.Show();
                                                 donotCallAgain = true;
                                             }
                                             break;


                                         case "00001585": //send statement checkbox is not checked
                                             sqlResults += reader[1] + "\n"; 
                                             Patient  pat = new Patient();
                                             pat.Patient_ID  = reader["patient_no"].ToString();
                                             pat.Last_Name = reader["last_name"].ToString();
                                             pat.First_Name  = reader["first_name"].ToString();

                                             //Update the table, checking the sendstatement box
                                            ProcessSendStatementCheckBox_OLE (pat);
                                             break;


                                         case "00004668": //Get the product name from the user, lookup the table, remove the rec as appropriate.
                                             ResolveProductNameExists_OLE();
                                             break;

                                         default:
                                             sqlResults += reader[0] + "\n"; //The first field name in the sql table
                                             MessageBox.Show("Solution ran successfully", "Fix It");
                                             break;
                                     }
                                 }

                             }
                            #endregion
                            }
                     }
     //                MessageBox.Show("Solution ran successfully", "Fix It");
                 }
                 catch (OleDbException f)
                 {
                     _ConnectionSucceeded = false;
                     MessageBox.Show(f.Message, f.ErrorCode.ToString());
                     //Uncomment the section below when more error detail is needed.
                     string message = f.ToString();
                     string caption = "Error";
                     var result = MessageBox.Show(message, caption,
                                                  MessageBoxButtons.OK,
                                                  MessageBoxIcon.Exclamation);
                 }
             }

         }

        //CW 3/5/2014 Combining FindItOleClient() and FixItOleClient(), so there is one list for the user
         public void FindItANDFixItOleClient()//  New code to combine find and fix code 
         {
             string selectedSolution = selectedCaseCatcher;
             string queryString = sols.GetSQLFromCaseCatcher(selectedSolution);
             string selectedSolutionNumber = selectedCaseCatcher.Substring(0, 8);

             //Populate list which may later be used to populate datagrid have user select
             //specific feeslips to resolve unk insurance.                                     
             List<FeeSlip> listFeeSlipsForUserSelection = new List<FeeSlip>();
             FeeslipsWithUnknownIns fsForm = new FeeslipsWithUnknownIns();

             //Populate list which may later be used to populate datagrid have user select
             //specific patients to check the send statement checkbox of the patient demographic
             List<Patient> listPatientsForUserSelection = new List<Patient>();
             SendStatementCheckBox patForm = new SendStatementCheckBox();


             //Populate list which may later be used to populate datagrid have user select
             //specific insurance items to resolve missing group (AKA plan number)
             List<Insurance> listInsuranceForUserSelection = new List<Insurance>();
             //SendStatementCheckBox patForm = new SendStatementCheckBox();

             //Populate list which may later be used to populate datagrid for display/print            
             List<Exam> listPatientsWithBMIerrs = new List<Exam>();
             SendStatementCheckBox examForm = new SendStatementCheckBox();

             //Populate list which may later be used to populate datagrid have user select
             //specific feeslips to resolve long reasons.                                     
             List<FeeSlipItmTrans> listFeeSlipsForUserSelection_LongReason = new List<FeeSlipItmTrans>();
             FeeSlipItmTrans fsForm_LongReason = new FeeSlipItmTrans();


             OMSQLDB o = new OMSQLDB();
             using (OleDbConnection conn = o.GetOLEConnection ())
             {                 
                 try
                 {
              
                     OleDbCommand command = new OleDbCommand(queryString, conn);
                     using (OleDbDataReader reader = command.ExecuteReader())
                     {
                         string sqlResults = "";

                         while (reader.Read())
                         {
                             //Unknown insurance requires a different field returned and treatment
                             switch (selectedSolutionNumber)
                             {
                                 case "00001466": //Unknown Insurance
                                     sqlResults += reader[0] + "\n"; //The feeslip number field in the table
                                     FeeSlip fs = new FeeSlip();
                                     fs.Feeslip_no = reader["feeslip_no"].ToString();
                                     fs.SlipItem_no = reader["slipitm_no"].ToString();
                                     fs.InsuranceTotal = reader["slipitm_ins_total"].ToString();
                                     fs.PatientTotal = reader["slipitm_pat_total"].ToString();

                                     //replace null values with zeroes, to prevent errors as these values
                                     //are passed to the grid, processed by other classes/methods
                                     fs.Feeslip_no = (fs.Feeslip_no != null) ? fs.Feeslip_no : "0";
                                     fs.SlipItem_no = (fs.SlipItem_no != null) ? fs.SlipItem_no : "0";
                                     fs.InsuranceTotal = (fs.InsuranceTotal != null) ? fs.InsuranceTotal : "0";
                                     fs.PatientTotal = (fs.PatientTotal != null) ? fs.PatientTotal : "0";

                                     listFeeSlipsForUserSelection.Add(fs);
                                     //Use the line below to create a new obj in one line
                                     //listFeeSlipsForUserSelection.Add(new FeeSlip { Feeslip_no = reader[0].ToString(), PatientTotal = reader[3].ToString(), InsuranceTotal = reader[2].ToString() });                                       
                                     break;
                                 //-------------------------------------------------------------------------------------------
                                 case "00001585": //Send statement box is not checked
                                     sqlResults += reader[0] + "\n";
                                     Patient pat = new Patient();
                                     pat.Patient_ID = reader["patient_no"].ToString();
                                     pat.Last_Name = reader["last_name"].ToString();
                                     pat.First_Name = reader["first_name"].ToString();

                                     //replace null values with zeroes, to prevent errors as these values
                                     //are passed to the grid, processed by other classes/methods
                                     pat.Patient_ID = (pat.Patient_ID != null) ? pat.Patient_ID : "";
                                     pat.Last_Name = (pat.Last_Name != null) ? pat.Last_Name : "";
                                     pat.First_Name = (pat.First_Name != null) ? pat.First_Name : "";

                                     listPatientsForUserSelection.Add(pat);
                                     break;

                                 //-------------------------------------------------------------------------------------------
                                 case "00000726": //Third Party Processing - Error 217900- Incorrect syntax
                                     sqlResults += reader[0] + "\n";
                                     Patient pt = new Patient();
                                     pt.Patient_ID = reader["patient_no"].ToString();
                                     pt.Last_Name = reader["last_name"].ToString();
                                     pt.First_Name = reader["first_name"].ToString();
                                     pt.First_Name = reader["first_name"].ToString();
                                     pt.First_Name = reader["first_name"].ToString();

                                     //replace null values with zeroes, to prevent errors as these values
                                     //are passed to the grid, processed by other classes/methods
                                     pt.Patient_ID = (pt.Patient_ID != null) ? pt.Patient_ID : "";
                                     pt.Last_Name = (pt.Last_Name != null) ? pt.Last_Name : "";
                                     pt.First_Name = (pt.First_Name != null) ? pt.First_Name : "";

                                     listPatientsForUserSelection.Add(pt);
                                     break;

                                 //-------------------------------------------------------------------------------------------
                                 case "00001579": //Third Party Processing - Error 217900- Incorrect syntax
                                     sqlResults += reader[0] + "\n";
                                     Insurance ins = new Insurance();
                                     //ins.plan_name = reader["plan_name"].ToString();


                                     //replace null values with zeroes, to prevent errors as these values
                                     //are passed to the grid, processed by other classes/methods
                                     //pt.Patient_ID = (pt.Patient_ID != null) ? pt.Patient_ID : "";
                                     //pt.Last_Name = (pt.Last_Name != null) ? pt.Last_Name : "";
                                     //pt.First_Name = (pt.First_Name != null) ? pt.First_Name : "";                                       
                                     listInsuranceForUserSelection.Add(ins);
                                     break;

                                 //-------------------------------------------------------------------------------------------
                                 case "00000523": //Error 217887 when selecting insurance drop-down on insurance ledger tab
                                     sqlResults += reader[0] + "\n";
                                     FeeSlipItmTrans fsItmTrnReason = new FeeSlipItmTrans();
                                     fsItmTrnReason.Itmtrn_no = reader["itmtrn_no"].ToString();
                                     fsItmTrnReason.Feeslip_no = reader["feeslip_no"].ToString();
                                     fsItmTrnReason.SlipItem_no = reader["slipitm_no"].ToString();
                                     fsItmTrnReason.Itmtrn_reason = reader["Itmtrn_reason"].ToString();
                                     fsItmTrnReason.Itmtrn_reason_length = reader["Length"].ToString();

                                     //replace null values with zeroes, to prevent errors as these values
                                     //are passed to the grid, processed by other classes/methods                                       
                                     fsItmTrnReason.Itmtrn_no = (fsItmTrnReason.Itmtrn_no != null) ? fsItmTrnReason.Itmtrn_no : "0";
                                     fsItmTrnReason.Feeslip_no = (fsItmTrnReason.Feeslip_no != null) ? fsItmTrnReason.Feeslip_no : "0";
                                     fsItmTrnReason.SlipItem_no = (fsItmTrnReason.SlipItem_no != null) ? fsItmTrnReason.SlipItem_no : "0";
                                     fsItmTrnReason.Itmtrn_reason = (fsItmTrnReason.Itmtrn_reason != null) ? fsItmTrnReason.Itmtrn_reason : "0";
                                     fsItmTrnReason.Itmtrn_reason_length = (fsItmTrnReason.Itmtrn_reason_length != null) ? fsItmTrnReason.Itmtrn_reason_length : "0";
                                     listFeeSlipsForUserSelection_LongReason.Add(fsItmTrnReason);
                                     break;

                                 //--------------------------------------------------------------------------------------


                                 //--------------------------------------------------------------------------------------

                                 case "00007481": //Error 217833 Arithmetic overflow running NQF0421 POP1/POP2 this is from Scott A.
                                     sqlResults += reader[0] + "\n";
                                     //field list for this sql is:
                                     // recid, ExamRec, PatientNo, first_name, last_name,  RecType, Text2
                                     Exam exm = new Exam();
                                     exm.ExamRec = reader["ExamRec"].ToString();
                                     exm.PatientNo = reader["PatientNo"].ToString();
                                     exm.first_name = reader["first_name"].ToString();
                                     exm.last_name = reader["last_name"].ToString();
                                     exm.RecType = reader["RecType"].ToString();
                                     exm.Text2 = reader["Text2"].ToString();
                                     //replace null values with zeroes, to prevent errors as these values
                                     //are passed to the grid, processed by other classes/methods
                                     exm.ExamRec = (exm.ExamRec != null) ? exm.ExamRec : "";
                                     exm.first_name = (exm.first_name != null) ? exm.first_name : "";
                                     exm.last_name = (exm.last_name != null) ? exm.last_name : "";
                                     exm.RecType = (exm.RecType != null) ? exm.RecType : "";
                                     exm.Text2 = (exm.Text2 != null) ? exm.Text2 : "";

                                     listPatientsWithBMIerrs.Add(exm);


                                     break;
                                 //--------------------------------------------------------------------------------------
                                 default:
                                     sqlResults += reader[0] + "\n"; //The first field name in the sql table
                                     break;



                             }

                         }

                         if (reader.HasRows)//the list<> is populated in the while loop above, now just build the grid with the list<> as the data source.
                         {
                             switch (selectedSolutionNumber)
                             {
                                 case "00001466": //Unknown Insurance
                                     FeeslipsWithUnknownIns formfeeslipswithUNK = new FeeslipsWithUnknownIns();
                                     formfeeslipswithUNK.dataGridView1.DataSource = listFeeSlipsForUserSelection;
                                     DataGridViewCheckBoxColumn CBColumn = new DataGridViewCheckBoxColumn();
                                     CBColumn.HeaderText = "Resolve";
                                     CBColumn.FalseValue = 0;
                                     CBColumn.TrueValue = 1;
                                     formfeeslipswithUNK.dataGridView1.Columns.Insert(0, CBColumn);
                                     formfeeslipswithUNK.Show();
                                     //make all checked by default   
                                     //needed this to try and resolve the checkall fail in another module
                                     for (int i = 0; i < formfeeslipswithUNK.dataGridView1.Rows.Count - 1; i++)
                                     {
                                         formfeeslipswithUNK.dataGridView1.Rows[i].Cells[0].Value = true;
                                     }
                                     formfeeslipswithUNK.dataGridView1.RefreshEdit();
                                     formfeeslipswithUNK.CheckAllBoxes();

                                     // formfeeslipswithUNK.dataGridView1.Invalidate();
                                     break;

                                 case "00001585": //Send statement box is not checked
                                     //this should now only be run from FindIT, there is a UI

                                     SendStatementCheckBox formpatientsSendStatement = new SendStatementCheckBox();
                                     formpatientsSendStatement.dataGridView1.DataSource = listPatientsForUserSelection;
                                     DataGridViewCheckBoxColumn CBColumnPat = new DataGridViewCheckBoxColumn();
                                     CBColumnPat.HeaderText = "Resolve";
                                     CBColumnPat.FalseValue = 0;
                                     CBColumnPat.TrueValue = 1;
                                     formpatientsSendStatement.dataGridView1.Columns.Insert(0, CBColumnPat);
                                     formpatientsSendStatement.Show();

                                     //make all checked by default   
                                     //needed this to try and resolve the checkall fail in another module

                                     for (int i = 0; i < formpatientsSendStatement.dataGridView1.Rows.Count - 1; i++)
                                     {
                                         formpatientsSendStatement.dataGridView1.Rows[i].Cells[0].Value = true;
                                     }

                                     formpatientsSendStatement.dataGridView1.RefreshEdit();
                                     // CW 1/14/2014 this was needed because the last row would not check
                                     // regardless of how many rows.
                                     formpatientsSendStatement.CheckAllBoxes();

                                     break;

                                 case "00000726": //Third Party Processing - Error 217900- Incorrect syntax
                                     //show a list of patients with illegal characters.
                                     //READ-ONLY UI this should now only be run from FindIT, there is a read-only UI

                                     PunctuationCharacter formPunctuation = new PunctuationCharacter();
                                     formPunctuation.dataGridView1.DataSource = listPatientsForUserSelection;
                                     DataGridViewCheckBoxColumn CBColumnPt = new DataGridViewCheckBoxColumn();
                                     CBColumnPt.HeaderText = "Resolve";
                                     CBColumnPt.FalseValue = 0;
                                     CBColumnPt.TrueValue = 1;
                                     formPunctuation.dataGridView1.Columns.Insert(0, CBColumnPt);
                                     //note this is only hidden for this solution, since it is displaying a list only
                                     CBColumnPt.Visible = false;

                                     formPunctuation.Show();

                                     //make all checked by default   
                                     //needed this to try and resolve the checkall fail in another module

                                     //for (int i = 0; i < formPunctuation.dataGridView1.Rows.Count - 1; i++)
                                     //{
                                     //    formPunctuation.dataGridView1.Rows[i].Cells[0].Value = true;
                                     //}

                                     //formPunctuation.dataGridView1.RefreshEdit();
                                     //formPunctuation.CheckAllBoxes();

                                     break;

                                 case "00001579": //Third Party Processing - Error 217900- Incorrect syntax
                                     //show a list of patients with illegal characters.
                                     //READ-ONLY UI this should now only be run from FindIT, there is a read-only UI

                                     Err94Null_InsPatDemog err94nullpatins = new Err94Null_InsPatDemog();
                                     err94nullpatins.dataGridView1.DataSource = listInsuranceForUserSelection;
                                     DataGridViewCheckBoxColumn CBColumnPt_insurance = new DataGridViewCheckBoxColumn();
                                     CBColumnPt_insurance.HeaderText = "Resolve";
                                     CBColumnPt_insurance.FalseValue = 0;
                                     CBColumnPt_insurance.TrueValue = 1;
                                     err94nullpatins.dataGridView1.Columns.Insert(0, CBColumnPt_insurance);
                                     //note this is only hidden for this solution, since it is displaying a list only
                                     CBColumnPt_insurance.Visible = true;

                                     err94nullpatins.Show();

                                     //make all checked by default   
                                     //needed this to try and resolve the checkall fail in another module

                                     for (int i = 0; i < err94nullpatins.dataGridView1.Rows.Count - 1; i++)
                                     {
                                         err94nullpatins.dataGridView1.Rows[i].Cells[0].Value = true;
                                     }

                                     err94nullpatins.dataGridView1.RefreshEdit();
                                     //err94nullpatins.CheckAllBoxes();

                                     break;

                                 //--------------------------------------------------------------------------------------
                                 case "00000523": //Error 217887 When selecting an insurance in the patient ledger dropdown list
                                     //show a list of feeslips with reasons longer than 75.
                                     //READ-ONLY UI this should now only be run from FindIT, there is a read-only UI

                                     FSItemReasonLength FSlongReason = new FSItemReasonLength();
                                     FSlongReason.dataGridView1.DataSource = listFeeSlipsForUserSelection_LongReason;
                                     DataGridViewCheckBoxColumn CBColumnFS_reason = new DataGridViewCheckBoxColumn();
                                     CBColumnFS_reason.HeaderText = "Resolve";
                                     CBColumnFS_reason.Width = 55;
                                     CBColumnFS_reason.FalseValue = 0;
                                     CBColumnFS_reason.TrueValue = 1;
                                     FSlongReason.dataGridView1.Columns.Insert(0, CBColumnFS_reason);
                                     //note this is only hidden for this solution, since it is displaying a list only
                                     CBColumnFS_reason.Visible = true;

                                     //renaming columns with friendly header titles
                                     //FSlongReason.dataGridView1.Columns[1].HeaderText = "FeeSlip#";
                                     FSlongReason.dataGridView1.Columns[1].Width = 60;
                                     //FSlongReason.dataGridView1.Columns[2].Visible = false;
                                     FSlongReason.dataGridView1.Columns[3].Visible = false;
                                     FSlongReason.dataGridView1.Columns[4].HeaderText = "Length";
                                     FSlongReason.dataGridView1.Columns[4].Width = 60;
                                     FSlongReason.dataGridView1.Columns[5].HeaderText = "Reason";
                                     FSlongReason.dataGridView1.Columns[5].Width = 600;
                                     SharedUtilities su = new SharedUtilities();
                                     //su.ResizeTheGrid(FSlongReason.dataGridView1);
                                     //su.SetColumnToSortable(FSlongReason.dataGridView1,"Feeslip_no");
                                     //su.SetColumnToSortable(FSlongReason.dataGridView1, "Itmtrn_reason_length");

                                     FSlongReason.Show();

                                     //make all checked by default   
                                     //needed this to try and resolve the checkall fail in another module

                                     for (int i = 0; i < FSlongReason.dataGridView1.Rows.Count - 1; i++)
                                     {
                                         FSlongReason.dataGridView1.Rows[i].Cells[0].Value = true;
                                     }

                                     FSlongReason.dataGridView1.RefreshEdit();
                                     //FSlongReason.CheckAllBoxes();

                                     break;

                                 //--------------------------------------------------------------------------------------




                                 case "00007481": // This is BMI calc in EW, not send statement, just using the objs.                                                                         

                                     SendStatementCheckBox formpatientsSendStatement2 = new SendStatementCheckBox();
                                     formpatientsSendStatement2.dataGridView1.DataSource = listPatientsWithBMIerrs;
                                     DataGridViewCheckBoxColumn CBColumnPat2 = new DataGridViewCheckBoxColumn();
                                     CBColumnPat2.HeaderText = "Resolve";
                                     CBColumnPat2.FalseValue = 0;
                                     CBColumnPat2.TrueValue = 1;
                                     CBColumnPat2.Visible = false;
                                     formpatientsSendStatement2.dataGridView1.Columns.Insert(0, CBColumnPat2);

                                     formpatientsSendStatement2.dataGridView1.Columns[1].HeaderText = "Exam#";
                                     formpatientsSendStatement2.dataGridView1.Columns[2].HeaderText = "Patient#";
                                     formpatientsSendStatement2.dataGridView1.Columns[3].HeaderText = "First Name";
                                     formpatientsSendStatement2.dataGridView1.Columns[4].HeaderText = "Last Name";
                                     formpatientsSendStatement2.dataGridView1.Columns[5].HeaderText = "Vital Sign";
                                     formpatientsSendStatement2.dataGridView1.Columns[6].HeaderText = "Measurement";

                                     //resize some colums so all will display without scrolling
                                     int tempWidth = 0;
                                     for (int i = 1; i < 3; i++)
                                     {
                                         tempWidth = formpatientsSendStatement2.dataGridView1.Columns[i].Width;
                                         formpatientsSendStatement2.dataGridView1.Columns[i].Width = tempWidth - 50;
                                     }

                                     formpatientsSendStatement2.Show();

                                     //make all checked by default   
                                     //needed this to try and resolve the checkall fail in another module

                                     for (int i = 0; i < formpatientsSendStatement2.dataGridView1.Rows.Count - 1; i++)
                                     {
                                         formpatientsSendStatement2.dataGridView1.Rows[i].Cells[0].Value = true;
                                     }

                                     formpatientsSendStatement2.dataGridView1.RefreshEdit();
                                     // CW 1/14/2014 this was needed because the last row would not check
                                     // regardless of how many rows.
                                     //formpatientsSendStatement2.CheckAllBoxes();
                                     formpatientsSendStatement2.Text = "BMI Entries Needing Correction";
                                     formpatientsSendStatement2.lbInstructions.Text = "To Copy this list \r";
                                     formpatientsSendStatement2.lbInstructions.Text += "1) Click the Copy button to copy the list \r";
                                     formpatientsSendStatement2.lbInstructions.Text += "2) Use Ctl-v to paste this list into another document.";


                                     break;

                                 default:
                                     MessageBox.Show("Results from the Case Catcher: \n" +
                                     "There is a solution to fix records with the following values in the " + reader.GetName(0) + " field: \n" +
                                      sqlResults, "Case Catcher");
                                     break;
                             }
                         }
                         else
                         {
                             MessageBox.Show("There are no records that meet the solution condition", "Case Catcher");
                         }
                     }
                 }


                 catch (OleDbException f)
                 {
                     _ConnectionSucceeded = false;
                     MessageBox.Show(f.Message, f.ErrorCode.ToString());
                     //Uncomment the section below when more error detail is needed.
                     //string message = f.ToString();
                     //string caption = "Error";
                     //var result = MessageBox.Show(message, caption,
                     //                             MessageBoxButtons.OK,
                     //                             MessageBoxIcon.Exclamation);
                 }
             }
         }
        
         public void FindItSqlClient()// WORKS  called from form1 
         {
 
             string selectedSolution = selectedCaseCatcher;
             string queryString = sols.GetSQLFromCaseCatcher(selectedSolution);
             string selectedSolutionNumber = selectedCaseCatcher.Substring(0, 8);

             //Populate list which may later be used to populate datagrid have user select
             //specific feeslips to resolve unk insurance.                                     
             List<FeeSlip> listFeeSlipsForUserSelection = new List<FeeSlip>();
             FeeslipsWithUnknownIns fsForm = new FeeslipsWithUnknownIns();


             {
                 using (SqlConnection conn = new SqlConnection())
                 {
                     conn.ConnectionString =
                         "integrated security=false;Data Source=tcp:" + _clientServerName + "\\" + _clientInstanceName + ";" +
                         "user id=OM_USER;password=OMSQL@2004;" +
                         "persist security info=False;database=" + _clientDatabaseName;

                     try
                     {
                         conn.Open();

                         using (SqlCommand command = new SqlCommand(queryString, conn))
                         {
                             using (SqlDataReader reader = command.ExecuteReader())
                             {
                                 string sqlResults = "";

                                 while (reader.Read())
                                 {
                                     sqlResults += reader[0] + "\n"; //This captures the field name in the sql table
                                     switch (selectedSolutionNumber)
                                     {
                                         case "00001466": //Unknown Insurance
                                             sqlResults += reader[0] + "\n"; //The feeslip number field in the table
                                             FeeSlip fs = new FeeSlip();
                                             fs.Feeslip_no = reader["feeslip_no"].ToString();
                                             fs.SlipItem_no = reader["slipitm_no"].ToString();
                                             fs.InsuranceTotal = reader["slipitm_ins_total"].ToString();

                                             //replace null values with zeroes, to prevent errors as these values
                                             //are passed to the grid, processed by other classes/methods
                                             fs.Feeslip_no = (fs.Feeslip_no != null) ? fs.Feeslip_no : "0";
                                             fs.SlipItem_no = (fs.SlipItem_no != null) ? fs.SlipItem_no : "0";
                                             fs.InsuranceTotal = (fs.InsuranceTotal != null) ? fs.InsuranceTotal : "0";

                                             listFeeSlipsForUserSelection.Add(fs);
                                             //Use the line below to create a new obj in one line
                                             //listFeeSlipsForUserSelection.Add(new FeeSlip { Feeslip_no = reader[0].ToString(), PatientTotal = reader[3].ToString(), InsuranceTotal = reader[2].ToString() });                                       
                                             break;

                                         default:
                                             sqlResults += reader[0] + "\n"; //The first field name in the sql table
                                             break;

                                     }
                                 }

                                 if (reader.HasRows)
                                 {
                                     switch (selectedSolutionNumber)
                                     {
                                         case "00001466": //Unknown Insurance
                                             //new display grid with fee slips with unk insurance
                                             FeeslipsWithUnknownIns formfeeslipswithUNK = new FeeslipsWithUnknownIns();
                                             formfeeslipswithUNK.dataGridView1.DataSource = listFeeSlipsForUserSelection;
                                             DataGridViewCheckBoxColumn CBColumn = new DataGridViewCheckBoxColumn();
                                             CBColumn.HeaderText = "Resolve";
                                             CBColumn.FalseValue = "0";
                                             //CBColumn.TrueValue = "1";
                                             formfeeslipswithUNK.dataGridView1.Columns.Insert(0, CBColumn);
                                             formfeeslipswithUNK.Show();
                                             break;
                                             
                                         default:
                                             MessageBox.Show("Results from the Case Catcher: \n" +
                                             "There is a solution to fix records with the following values in the " + reader.GetName(0) + " field: \n" +
                                              sqlResults, "Case Catcher");
                                             break;
                                     }
                                 }
                                 else
                                 {
                                     MessageBox.Show("There are no records that meet the solution condition", "Case Catcher");
                                 }
                             }
                         }


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
         }

         /// <Summary>
         /// Author :   CW
         /// Added  :   12/23/2013
         /// What   :   Added this code similar to fixtOLEclient, needs testing on sql client
         /// </Summary>
         public  void FixItSqlClient()// called from FixIt_FindItHandler
         {

             string selectedSolution = selectedCaseCatcher;
             string selectedSolutionNumber = selectedCaseCatcher.Substring(0, 8);                        
             string queryString = sols.GetSQLFromSolution(selectedSolution);

             using (SqlConnection conn = new SqlConnection())
             {
                 conn.ConnectionString =
                 "integrated security=false;Data Source=tcp:" + _clientServerName + "\\" + _clientInstanceName + ";" +
                         "user id=OM_USER;password=OMSQL@2004;" +
                         "persist security info=False;database=" + _clientDatabaseName;

                 try
                 {
                     conn.Open();
                     using (SqlCommand command = new SqlCommand(queryString, conn))
                     {
                         using (SqlDataReader reader = command.ExecuteReader())
                         {
                             string sqlResults = "";

                             while (reader.Read())
                             {
                                 //Unknown insurance requires a different field returned and treatment
                                 switch (selectedSolutionNumber)
                                 {
                                     case "00001466": //Unknown Insurance
                                         sqlResults += reader[1] + "\n"; //The 2nd field is the feeslip number in the table
                                         FeeSlip fs = new FeeSlip();
                                         fs.Feeslip_no = reader["feeslip_no"].ToString();
                                         fs.SlipItem_no = reader["slipitm_no"].ToString();
                                         fs.InsuranceTotal = reader["slipitm_ins_total"].ToString();
                                         fs.PatientTotal = reader["slipitm_pat_total"].ToString();

                                         //Update the table, moving the fee to the patient
                                         ProcessUnknownInsurances_SQL(fs);                                         
                                         break;

                                     case "00000238": //iBal selected for many reasons, so launch iBal with no programming. All user controlled
                                         string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                                         System.Diagnostics.Process.Start(path + @"\iBal.exe");
                                         break;

                                     default:
                                         sqlResults += reader[0] + "\n"; //The first field name in the sql table
                                         break;
                                 }
                             }
                             MessageBox.Show("Solution ran successfully", "Fix It");

                         }
                     }

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

         private void PopulateBkUpScreen()
         {   //Try to get dbname and server from omate32.ini;         
             GetOmate32iniFileValues();
             //Try to get the file path on the server from SQL Server
             if (_clientDatabaseName != null || _clientServerName != null) { GetDatabaseProperties(); }
             //Populate the fields on the user screen
             
             //todo: need to still respond if the path is null
             //if (_clientDbFilePath != null) { PopulateScreenFields(); }
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
         public  void ProcessUnknownInsurances_OLE(FeeSlip fs)
         {
             //Given a fee slip object, resolve the unknown insurance condition by
             //moving the insurance total to the patient total and updating the sql table
             //this is called from a loop in FixItOleClient
        #region GatherUnknownInsuranceFeeSlips             
             string queryString = "";
             string _fsItemNumber = fs.SlipItem_no;
             
             string _insTotal = (fs.InsuranceTotal != null) ? fs.InsuranceTotal : "0";
             string _patTotal = (fs.PatientTotal != null) ? fs.PatientTotal : "0"; //this will capture all the fees for the line item, to allow adjusting in the ledger
             decimal _newPatientTotal = 0; //this will be the new patient total = insTotal + patTotal;

             _newPatientTotal = decimal.Parse(_insTotal) + decimal.Parse(_patTotal);

             //Get the Sum total of fields slipitm_pat_total + slipitm_ins_total for just those unk fee slip line items
             //Update the field called slipitm_pat_total in the fee_slip_items table with the sum total of patient and insurance.
             //This allows the client to make adjustments in the patient side of the ledger.

//             queryString = @"UPDATE Fee_Slip_Items
//                            SET slipitm_ins_total = 0, slipitm_pat_total = " + _newPatientTotal +
//                            @" WHERE slipitm_no NOT IN (SELECT slipitm_no FROM Fee_Slip_HCFA_CPT_Items) 
//                            AND slipitm_ins_total <> 0";

             queryString = @"UPDATE Fee_Slip_Items
                            SET slipitm_ins_total = 0, slipitm_pat_total = " + _newPatientTotal +
                            @" WHERE slipitm_no = " + _fsItemNumber;
             OMSQLDB o = new OMSQLDB();
             using (OleDbConnection conn = o.GetOLEConnection ())
             {                 
                 try
                 {                  
                     using (OleDbCommand command = new OleDbCommand(queryString, conn))
                     {                                                                                  
                            try
                            {                                                                                                
                                command.ExecuteNonQuery();
                                command.Dispose();
                                conn.Close();
                                                                
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Can not open connection ! " + ex.ToString());
                            }                         
                     }

                 }


                 catch (OleDbException f)
                 {
                     _ConnectionSucceeded = false;
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
         }
         public void ProcessUnknownInsurances_SQL(FeeSlip fs)
         {
             //Given a fee slip object, resolve the unknown insurance condition by
             //moving the insurance total to the patient total and updating the sql table
             //this is called from a loop in FixItSQLClient
             #region GatherUnknownInsuranceFeeSlips
             string queryString = "";
             string _fsItemNumber = fs.SlipItem_no;

             string _insTotal = (fs.InsuranceTotal != null) ? fs.InsuranceTotal : "0";
             string _patTotal = (fs.PatientTotal != null) ? fs.PatientTotal : "0"; //this will capture all the fees for the line item, to allow adjusting in the ledger
             decimal _newPatientTotal = 0; //this will be the new patient total = insTotal + patTotal;

             _newPatientTotal = decimal.Parse(_insTotal) + decimal.Parse(_patTotal);

             //Get the Sum total of fields slipitm_pat_total + slipitm_ins_total for just those unk fee slip line items
             //Update the field called slipitm_pat_total in the fee_slip_items table with the sum total of patient and insurance.
             //This allows the client to make adjustments in the patient side of the ledger.

//             queryString = @"UPDATE Fee_Slip_Items
//                            SET slipitm_ins_total = 0, slipitm_pat_total = " + _newPatientTotal +
//                            @" WHERE slipitm_no NOT IN (SELECT slipitm_no FROM Fee_Slip_HCFA_CPT_Items) 
//                            AND slipitm_ins_total <> 0";

             queryString = @"UPDATE Fee_Slip_Items
                            SET slipitm_ins_total = 0, slipitm_pat_total = " + _newPatientTotal +
               @" WHERE slipitm_no = " + _fsItemNumber;

             using (SqlConnection conn = new SqlConnection())
             {
                 conn.ConnectionString =
                     "integrated security=false;Data Source=tcp:" + _clientServerName + "\\" + _clientInstanceName + ";" +
                     "user id=OM_USER;password=OMSQL@2004;" +
                     "persist security info=False;database=" + _clientDatabaseName;

                 try
                 {
                     using (SqlCommand command = new SqlCommand(queryString, conn))
                     {
                         try
                         {
                             conn.Open();
                             command.ExecuteNonQuery();
                             command.Dispose();
                             conn.Close();
                         }
                         catch (Exception ex)
                         {
                             MessageBox.Show("Can not open connection ! " + ex.ToString());
                         }
                     }

                 }


                 catch (SqlException f)
                 {
                     _ConnectionSucceeded = false;
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
         }

         public void ResolveProductNameExists_OLE()
         {
             //Obtain the product name from the user
             //Lookup the name in the Product table and delete the record if it exists, or escalate to Tier 3.
             //Using the prd_no from the initial lookup remove possible orphaned record
             //in the following tables: product_details,Prod_Addl_Attr, and product_notes             
             string queryString = "";
             //similar grid if more than one product exists
             List<Product> listProductsForUserSelection = new List<Product>();
             FeeslipsWithUnknownIns fsForm = new FeeslipsWithUnknownIns();

             //Get the product name from the user
             string value = "enter product name here";
             string productName = "";
             if (UserInput.InputBox("Product Name", "Enter the product name:", ref value) == DialogResult.OK)
             {
                 productName = value;
             }

             //Get the product number from the database table
                queryString = @"select prd_no,prd_style_name, prd_new_since  from product " +
                                @"where prd_style_name  = '" + productName + "'";
             OMSQLDB o = new OMSQLDB();
                     using (OleDbConnection conn = o.GetOLEConnection ())
                     {
                         
                     OleDbCommand command = new OleDbCommand(queryString, conn);
                     using (OleDbDataReader reader = command.ExecuteReader())
                     {
                         if (reader.HasRows )
                             {
                                 int r = 0;
                                 while (reader.Read())//keep loop in case, this is expanded to show many products in a grid for 
                                                        //user selection, then processing of selected items.
                                     {                                                    
                                                 Product p = new Product();
                                                 p.Prd_no = reader["prd_no"].ToString();
                                                 p.Prd_style_name  = reader["prd_style_name"].ToString();
                                                 p.Prd_new_since  = reader["prd_new_since"].ToString();
                                                 listProductsForUserSelection.Add(p);
                                     }

                             //Now fill the grid for user selection and processing
                             //The user needs to confirm the date the product was created is close
                                 ProductAlreadyExistsProcessing productSelectionScreen = new ProductAlreadyExistsProcessing();

                                 productSelectionScreen.dataGridView1.DataSource = listProductsForUserSelection;
                                 DataGridViewCheckBoxColumn CBColumn = new DataGridViewCheckBoxColumn();
                                 CBColumn.Name = "colName_CheckBox";
                                 CBColumn.HeaderText = "Resolve";
                                 CBColumn.FalseValue = false;
                                 CBColumn.TrueValue = true ;                                 
                                 productSelectionScreen.dataGridView1.Columns.Insert(0, CBColumn);
                                 SharedUtilities su = new SharedUtilities();
                                 su.ResizeTheGrid(productSelectionScreen.dataGridView1);
                                 productSelectionScreen.Show();
                             }
                         else
                                 {                             
                                     string message = "A product named '" + productName + "' was not found";
                                     string caption =  "Product Name not found";
                                     var result = MessageBox.Show(message, caption,
                                                         MessageBoxButtons.RetryCancel,
                                                         MessageBoxIcon.Exclamation);
                                     if (result.ToString() == "Retry")
                                     {
                                         //allow user to re-enter the prod name
                                         ResolveProductNameExists_OLE();
                                     }

                                     else
                                     {

                                     }
                                 }                         
                     }
                 }             
            }

         public void EditStateFeeslip_OLE()
         {
             //CW: Added 7/28/2014 Copied from ResolveProductNameExists_OLE()
             //TODO: Combine similar solutions that only require one input from the user.
             //Purpose: Obtain the feeslip number from the user
             //Lookup the number in the fee_slip table and change the editstate field to zero
             
             string queryString = "";

             //Get the product name from the user
             string value = "enter feeslip number here";
             string feeslipNumber = "";
             if (UserInput.InputBox("Feeslip Number", "Enter the feeslip number:", ref value) == DialogResult.OK)
             {
                 feeslipNumber = value;
             }

             if (UserInput.InputBox("Feeslip Number", "Enter the feeslip number:", ref value) == DialogResult.Cancel )
             {                 
                 //this.StopTheApplication();
             }

            int num1;
            bool res = int.TryParse(feeslipNumber, out num1);
            if (res == false)
            {
                //MessageBox.Show("Please reselect the solution and enter a feeslip number");
                //this.StopTheApplication();
            }
            else
            {

                //Get the product number from the database table
                queryString = @"Update Fee_slip Set edit_state = 0 where feeslip_no = '***' " +
                                @"where feeslip_no = '" + feeslipNumber + "'";

                try
                {
                    OMSQLDB o = new OMSQLDB();
                    using (OleDbConnection conn = o.GetOLEConnection())
                    {                        
                        using (OleDbCommand command = new OleDbCommand(queryString, conn))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }


         }

         public void ResolveErr94NullPatientDemogInsurancetab_OLE()
         {
             //Obtain the patient ID from the user
             //Lookup the insurance names in the patient_insurances table and present so the user can select one carrier.
             //Using the insurance_no from the selection lookup the plans from the insurance_plans table where ins_group_no = 0, and present a list of plan names so the user can select one plan
             //Update the patient_insurances table ins_group_no with the plan_no selected in the step above.
             
             string queryString = "";
             //similar grid if more than one product exists
             List<Insurance> listInsuranceCarriersForUserSelection = new List<Insurance>();             

             //Get the product name from the user
             string value = "enter patient ID number here";
             string insuranceName = "";
             string patientID = "";

             
             //if (UserInput.InputBox("Patient ID", "Enter the Patient ID:", ref value) == DialogResult.OK)
             //{
             //    patientID = value;
             //}

             

             queryString = @"SELECT pins.insurance_no, ins.insurance_name 
                            FROM patient_insurances pins
                            INNER JOIN insurance ins
                            ON
                            pins.insurance_no = ins.insurance_no
                            WHERE patient_no = '" + patientID + "'";

             OMSQLDB o = new OMSQLDB();
             using (OleDbConnection conn = o.GetOLEConnection ())
             {
                 OleDbCommand command = new OleDbCommand(queryString, conn);
                 using (OleDbDataReader reader = command.ExecuteReader())
                 {
                     if (reader.HasRows)
                     {
                         int r = 0;
                         while (reader.Read())
                         {
                             Insurance ins = new Insurance ();
                             ins.insurance_name  = reader["insurance_name"].ToString();                             
                             //listInsuranceCarriersForUserSelection.Add(ins);
                             listInsuranceCarriersForUserSelection.Add(ins);
                         }

                         //Now fill the grid for user selection and processing
                         //The user needs to confirm the date the product was created is close
                         
                         Err94Null_InsPatDemog err94SelectionScreen = new Err94Null_InsPatDemog();
                         err94SelectionScreen.dataGridView1.DataSource = listInsuranceCarriersForUserSelection;                         
                         err94SelectionScreen.Show();
                     }
                     //else
                     //{
                     //    string message = "A patient/ins named '" + insuranceName + "' was not found";
                     //    string caption = "Pat/ins Name not found";
                     //    var result = MessageBox.Show(message, caption,
                     //                        MessageBoxButtons.RetryCancel,
                     //                        MessageBoxIcon.Exclamation);
                     //    if (result.ToString() == "Retry")
                     //    {
                     //        //allow user to re-enter the prod name
                     //        //ResolveErr94NullPatientDemogInsurancetab_OLE();
                     //    }

                     //    else
                     //    {

                     //    }
                     //}
                 }
             }
         }

         public void ProcessSendStatementCheckBox_OLE(Patient pat)
         {
             //Given a patient object, check the send statement check box             
             //this is called from a loop in FixItOleClient
             #region checksendstatementbox
             string queryString = "";
             string _patientIDNumber = pat.Patient_ID;
             
//             queryString = @"UPDATE Fee_Slip_Items
//                            SET slipitm_ins_total = 0, slipitm_pat_total = " + _newPatientTotal +
//                            @" WHERE slipitm_no = " + _fsItemNumber;

             //queryString = @"UPDATE patient SET [send_statement] = 1 WHERE send_statement = 0 AND send_to_collection = 0 AND patient_type = 0 GO  " ;
             //CW 7/29/2014 Adding check boxes for Apply late charge and Apply Finance Charge per Melody Counts Request
             //queryString = @"UPDATE patient SET [send_statement] = 1 WHERE patient_no = " + pat.Patient_ID;
                queryString = @"UPDATE patient SET 
                                [send_statement] = 1 
                                [late_payment_only] = 1
                                [financial_charge] = 1
                              WHERE patient_no = " + pat.Patient_ID;

             OMSQLDB o = new OMSQLDB();
             using (OleDbConnection conn = o.GetOLEConnection ())
             {
                 try
                 {
                     using (OleDbCommand command = new OleDbCommand(queryString, conn))
                     {
                         try
                         {
                          
                             command.ExecuteNonQuery();
                             command.Dispose();
                             conn.Close();
                         }
                         catch (Exception ex)
                         {
                             MessageBox.Show("Can not open connection ! " + ex.ToString());
                         }
                     }

                 }


                 catch (OleDbException f)
                 {
                     _ConnectionSucceeded = false;
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
         } //may not use this, replaced with UI from findIT
         public void ProcessSendStatementCheckBox_SQL(Patient pat)
         {
             //Given a patient object, check the send statement check box             
             //this is called from a loop in FixItOleClient
             #region checksendstatementbox
             string queryString = "";
             string _patientIDNumber = pat.Patient_ID;

            //CW 7/29/2014 Adding check boxes for Apply late charge and Apply Finance Charge per Melody Counts Request
            //queryString = @"UPDATE patient SET [send_statement] = 1 WHERE patient_no = " + pat.Patient_ID;
            queryString = @"UPDATE patient SET 
                                [send_statement] = 1, 
                                [late_payment_only] = 1,
                                [financial_charge] = 1
                              WHERE patient_no = " + pat.Patient_ID;


             using (SqlConnection conn = new SqlConnection())
             {
                 conn.ConnectionString =
                     "integrated security=false;Data Source=tcp:" + _clientServerName + "\\" + _clientInstanceName + ";" +
                     "user id=OM_USER;password=OMSQL@2004;" +
                     "persist security info=False;database=" + _clientDatabaseName;


                 try
                 {
                     using (SqlCommand command = new SqlCommand(queryString, conn))
                     {
                         try
                         {
                             conn.Open();
                             command.ExecuteNonQuery();
                             command.Dispose();
                             conn.Close();
                         }
                         catch (Exception ex)
                         {
                             MessageBox.Show("Can not open connection ! " + ex.ToString());
                         }
                     }

                 }


                 catch (SqlException f)
                 {
                     _ConnectionSucceeded = false;
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
         }

         public void ShortenFSItmReason_OLE(string feeslip_item_tran_number)
         {
             //Given a fee slip item transaction number, shorten the reason field to <=75             
             //this is called from a loop in FixItOleClient
             #region Shorten the reason code
             string queryString = "";             

             queryString = @"UPDATE Fee_slip_items_trans 
                            SET itmtrn_reason=LTRIM(LEFT(itmtrn_reason, 75))
                            WHERE itmtrn_no = " + feeslip_item_tran_number;
             OMSQLDB o = new OMSQLDB();
             using (OleDbConnection conn = o.GetOLEConnection())
             {
                 try
                 {
                     using (OleDbCommand command = new OleDbCommand(queryString, conn))
                     {
                         try
                         {
                             conn.Open();
                             command.ExecuteNonQuery();
                             command.Dispose();
                             conn.Close();
                         }
                         catch (Exception ex)
                         {
                             MessageBox.Show("Can not open connection ! " + ex.ToString());
                         }
                     }

                 }


                 catch (OleDbException f)
                 {
                     _ConnectionSucceeded = false;
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
         }

         private void StopTheApplication()//this is used in all the catches
         {
             //error compiling the close & dispose, but works in the MainForm
             //this.Close();
             //this.Dispose();
         }
         private void StopTheApplication(string _exceptionType)//this is used in all the catches that seem to error when trying to close the form
         {
             //Todo: here we might suggest solutions based on the exception
             //for now it just continues to display the form with default values.

         }

    } //end of main class


}
