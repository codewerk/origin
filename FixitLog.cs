using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Fixit
{
    class FixitLog
    {
        public void CreateLog(string fileTitle, ref DataGridView passedDataGridView,string sharedDataPath)
        //Given a datagrid, this will create a log file and save it to the OfficeMate shared data directory
        //the name of the file will be SolutionName_DateTimeStamp
        {
            //int rowCount = passedDataGridView.Rows.Count;            
            int rowCount = passedDataGridView.RowCount;            
            if (rowCount == 1)
            {
                rowCount = 2; //this is needed because we subtract 1 in the loop below, 
                             //so this would never run otherwise
            }

            int cellCount = 0;
            string lineOfData = "";
            //string defaultFilename = @"C:\\" + fileTitle + DateTime.Now.ToString("yyyyMMddHHmm") + ".txt";
            string defaultFilename = sharedDataPath + fileTitle + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".txt";
            System.IO.StreamWriter objWriter;
            objWriter = new System.IO.StreamWriter(defaultFilename);


            //write header row to the file
            //log the column names

            {
                cellCount = passedDataGridView.Rows[0].Cells.Count;
                for (int cell = 0; cell < cellCount; cell++)
                {
                    if (passedDataGridView.Rows[0].Cells[cell].Value == null)
                    {
                    }
                    else
                    {
                        if (lineOfData == "")
                        {
                            lineOfData = passedDataGridView.Columns[cell].HeaderText;
                        }
                        else
                        {
                            lineOfData = lineOfData + "~" + passedDataGridView.Columns[cell].HeaderText;
                        }
                    }
                }
                objWriter.WriteLine(lineOfData);
                lineOfData = "";
            }



            //log the information
            try
            {
                for (int row = 0; row < rowCount; row++)
                {

                    for (int cell = 0; cell < cellCount; cell++)
                    {
                        if (passedDataGridView.Rows[row].Cells[cell].Value == null)
                        {

                        }
                        else
                        {
                            if (lineOfData == "")
                            {
                                lineOfData = passedDataGridView.Rows[row].Cells[cell].Value.ToString();
                            }

                            else
                            {
                                lineOfData = lineOfData + "~" + passedDataGridView.Rows[row].Cells[cell].Value.ToString();
                            }
                        }
                    }
                    objWriter.WriteLine(lineOfData);
                    lineOfData = "";
                }
                objWriter.Close();
            }

            catch (ArgumentOutOfRangeException Ex)
            {
                Console.WriteLine(Ex.ToString());
                objWriter.Close(); // CW 2/3/2014 we'll still close the file and maintain what was written,
                                    // since it's just an error with the number of grid rows being read.
                                    // This should be fixed above, by looking to see if there are any rows left.
            }
        }

        public void CreateLog(string fileTitle, string listColumnTitle, List<string> listLogValues, string sharedDataPath)
        //Given a collection of data to log, this will create a log file and save it to the OfficeMate shared data directory
        //the name of the file will be SolutionName_DateTimeStamp
        {

            //string defaultFilename = @"C:\\" + fileTitle + DateTime.Now.ToString("yyyyMMddHHmm") + ".txt";
            string defaultFilename = sharedDataPath + "\\" + fileTitle + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".txt";
            System.IO.StreamWriter objWriter;
            objWriter = new System.IO.StreamWriter(defaultFilename);

            //write header row to the file
            //log the column names
            objWriter.WriteLine(listColumnTitle);//write the column header


            //log the information
            try
            {
                foreach (string row in listLogValues)
                {
                    objWriter.WriteLine(row);
                }
                objWriter.Close();
            }

            catch (ArgumentOutOfRangeException Ex)
            {
                Console.WriteLine(Ex.ToString());
                objWriter.Close(); // CW 2/3/2014 we'll still close the file and maintain what was written,
                // since it's just an error with the number of grid rows being read.
                // This should be fixed above, by looking to see if there are any rows left.
            }
        }

    }
}
