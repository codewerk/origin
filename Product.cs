using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace Fixit
{
    public class Product //holds details of a product
    {
        public Product()
        {
            GetOmate32iniFileValues();
        }

        #region variables
        private string _prd_no;
        public string Prd_no
        {
            get { return _prd_no; }
            set { _prd_no = value; }
        }

        private string _prd_style_name;
        public string Prd_style_name  //used for unknown insurance
        {
            get { return _prd_style_name; }
            set { _prd_style_name = value; }
        }
      
        private string  _prd_new_since;
        public string  Prd_new_since
        {
            get { return _prd_new_since; }
            set { _prd_new_since = value; }
        }

    

        string _clientDatabaseName;
        string _clientServerName;
        string _clientInstanceName;
        string _clientDbFilePath;
        bool _ConnectionSucceeded;
        Solutions sols = new Solutions(); //this class contains the sql for solutions and catchers
        #endregion

        private void GetOmate32iniFileValues()
        {
            Workstation ws = new Workstation();
            _clientDatabaseName = ws.ClientDatabaseName;
            _clientServerName = ws.ClientServerName;
            _clientInstanceName = ws.ClientInstanceName;
            _clientDbFilePath = ws.ClientDbFilePath;
        }

        public void  ZeroOutOnOrderQuantity_OLE(Product p)//Sol#5979 
        {      //given a product object, update db field with a zero if the field is greater than zero      

            #region GatherProductRecords
            string queryString = "";
            string _productNumber = p.Prd_no;

            //todo: this sql can be put in the dictionary, since it only adds on a product number at the end
            queryString = @"UPDATE Product_Loc_Details SET prddtl_qty_on_order = 0 WHERE prd_no = " + _productNumber;

            OMSQLDB oms = new OMSQLDB();
            // we are now using the omsqldb class so we can remove all connection clode.
            //  using (OleDbConnection conn = new OleDbConnection())
            using (OleDbConnection conn = oms.GetOLEConnection())
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

        public void RemoveProduct_OLE(Product p)
        {
            IsProductInProdDetails_OLE(p);

            #region GatherProductRecords
            //string queryString = "";
            string _productNumber = p.Prd_no;

            /*
             *Given a passed in product number from ProductAlreadyExistsProcessing.cs -RemoveProductRecordFromDB()
             *It has already been confirmed that no record exists for it in the product_detail table.
             *So we will delete it from the main Product table, product_notes & product_fees tables.
             *Although it is unlikely to be in the latter 2 tables, it is possible as there are no constraints,
             *which leads to the cause of this error condition.
             *We are using separate delete statements at this time for clarity and unsure if cascading deletes
             *are possible and untested.                         
             */
            string queryString = @"DELETE from product          WHERE prd_no = " + _productNumber +
                                  "DELETE from product_notes    WHERE prd_no = " + _productNumber +
                                  "DELETE from product_fees     WHERE prd_no = " + _productNumber;
                                       
            OMSQLDB oms = new OMSQLDB();
            //we are now using the omsqldb class so we can remove all connection code.
            //using (OleDbConnection conn = new OleDbConnection())
            
            using (OleDbConnection conn = oms.GetOLEConnection ())
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

        public void RemoveProduct_SQL(Product p)
        {
            //Given a fee slip object, resolve the unknown insurance condition by
            //moving the insurance total to the patient total and updating the sql table
            //this is called from a loop in FixItOleClient
            #region GatherUnknownInsuranceFeeSlips
            string queryString = "";
            string _productNumber = p.Prd_no;

            //this is not correct for this solution
            queryString = @"UPDATE Product_Loc_Details
                            SET prddtl_qty_on_order = '0'
                            WHERE (prddtl_qty_on_order > 0) 
                            AND prd_no = " + _productNumber;
            

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

        public bool IsProductInProdDetails_OLE(Product p)
        {
          //Check to see if the product exists in the product_details table
            //escalate to T3 if it does.
            #region See if ProductRecords
            string queryString = "";
            string _productNumber = p.Prd_no;
            bool productDetailRecordExists = true;
            
            //todo: this sql can be put in the dictionary, since it only adds on a product number at the end
            queryString = @"Select * from Product_Details WHERE prd_no = " + _productNumber;
            
            OMSQLDB oms = new OMSQLDB();
            // we are now using the omsqldb class so we can remove all connection clode.
            //  using (OleDbConnection conn = new OleDbConnection())
            using (OleDbConnection conn = oms.GetOLEConnection())
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
                    using (OleDbCommand command = new OleDbCommand(queryString, conn))
                    {
                        try
                        {
                            conn.Open();
                            using (OleDbDataReader reader = command.ExecuteReader())
                            {
                                productDetailRecordExists = reader.HasRows;
                                command.Dispose();
                                conn.Close();

                                if (productDetailRecordExists)
                                    {
                                        //MessageBox.Show("There are still records in the Product_Detail Table, Contact Tier3 Support");
                                        //the message is handled in the calling code.
                                        return true;                                    
                                    }
                                else
                                    {
                                        return false ;
                                    }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Can not open connection ! " + ex.ToString());
                            return true;                                    
                        }
                    }

                }

                catch (OleDbException f)
                {
                    return true;                                    
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

    }
}
