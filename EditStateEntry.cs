using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace Fixit
{
    public partial class EditStateEntry : Form
    {
        private List<FeeSlip> ListOfFeeslips;

        public EditStateEntry(List<FeeSlip> _listOfFeeslips)
        {
            InitializeComponent();
            ListOfFeeslips = _listOfFeeslips;
              PopulateTextBox(_listOfFeeslips);
        }
 
        private void PopulateTextBox(List<FeeSlip> _listOfFeeslips)
        {
 // declare custom source.
            var source = new AutoCompleteStringCollection();
            // fill list of database feeslip numbers to source
            foreach (FeeSlip fs in _listOfFeeslips) // Loop through List with foreach
            {
                source.Add(fs.Feeslip_no);
            }

          

            // Create and initialize the text box.
            TextBox txtbxFeeSlipNumber = new TextBox();
            txtbxFeeSlipNumber.Name = "myTextBox";                 
            txtbxFeeSlipNumber.AutoCompleteCustomSource = source;
            txtbxFeeSlipNumber.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtbxFeeSlipNumber.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtbxFeeSlipNumber.Location = new Point(160, 10);
            txtbxFeeSlipNumber.Width = 60;
            txtbxFeeSlipNumber.Visible = true;
            this.Controls.Add(txtbxFeeSlipNumber);      
        }


        private void btnRun_Click(object sender, EventArgs e)
        {
            string _feeslipNumber = String.Empty ;
            //test get dynamic textbox value
            foreach (TextBox txt in this.Controls.OfType<TextBox>())
            {
                if (txt.Name == "myTextBox")
                {
                    _feeslipNumber = txt.Text;
                }                
            }
            bool found = false;
            //check to validate the entered feeslip is in the db (using the list safely)
             foreach (FeeSlip fs in ListOfFeeslips) // Loop through List with foreach
            {
                if (fs.Feeslip_no == _feeslipNumber)
                {
                    //change the edit state field value to zero to allow editing the feeslip            
                    OMSQLDB o = new OMSQLDB();
                    using (OleDbConnection conn = o.GetOLEConnection())                    
                    {
                        Solutions s = new Solutions();
                        string query = "Update Fee_slip Set edit_state = 0 where feeslip_no = '" + _feeslipNumber + "'";
                        found = true;
                        using (OleDbCommand com = new OleDbCommand(query, conn))
                        {
                            try
                            {
                                com.ExecuteNonQuery();
                                com.Dispose();
                                conn.Close();                                
                                MessageBox.Show("The feeslip can now be edited", "Feeslip - Edit Mode", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Can not open connection ! " + ex.ToString());
                            }
                        }
                    }


                }                                
            }
            if (!found)                             
            MessageBox.Show("Feeslip number is not valid, please enter another number", "Invalid Number", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                     

        }
    
    }


}
