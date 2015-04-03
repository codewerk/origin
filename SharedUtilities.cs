using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Fixit
{
    class SharedUtilities
    {

        public void ResizeTheGrid(DataGridView dgv)
        {                        
            dgv.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            int height = 0;
            foreach (DataGridViewRow row in dgv.Rows)
            {
                height += row.Height;
            }
            height += dgv.ColumnHeadersHeight;


            int width = 0;
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                if (col.Visible) //CW 2/18/2014 Added this because on some grids, we want the rec no, but don't want to display it.
                width += col.Width;
            }
            width += dgv.RowHeadersWidth;

            dgv.ClientSize = new System.Drawing.Size(width + 2, height + 2);

        }

        public void CopyGridContents(DataGridView dgv)
        {
            var newline = System.Environment.NewLine;
            var theDelimiter = "~";
            var clipboard_string = "";

            foreach (DataGridViewRow row in dgv.Rows)
            {
                for (int i = 0; i < row.Cells.Count; i++)
                {
                    if (i == (row.Cells.Count - 1))
                        clipboard_string += row.Cells[i].Value + newline;
                    else
                        clipboard_string += row.Cells[i].Value + theDelimiter;
                }
            }

            Clipboard.SetText(clipboard_string);
            MessageBox.Show("You can now paste into another application"
                           , "Data Copied to Clipboard"
                           , MessageBoxButtons.OK
                           , MessageBoxIcon.Information);
        }

        public void SetColumnToSortable(DataGridView dgv)
        {
            foreach (DataGridViewColumn column in dgv.Columns)
            {

                dgv.Columns[column.Name].SortMode = DataGridViewColumnSortMode.Automatic;
                Console.WriteLine(column.Name);

            }
        }

        public void SetColumnToSortable(DataGridView dgv,string _columnName)
        {
            dgv.Columns[_columnName].SortMode = DataGridViewColumnSortMode.Automatic;            
        }

        public void SearchGrid(string formname, DataGridView dgv,string searchValue,int searchColumn)
        {
            try
            {
            FSItemReasonLength fc = (FSItemReasonLength)Application.OpenForms[formname];  
                      
            int i = 0;
            fc.lblFeeslipsFound.Text = "";
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            
                ClearGridSelections(dgv);
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
                MessageBox.Show("Error: " + exc.Message);
            }
        }

        public void ClearGridSelections(DataGridView dgv)
        {//used by the grid search, which doesn't unselect rows

            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            try
            {
                foreach (DataGridViewRow row in dgv.Rows)
                {
                        row.Selected = false;                   
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        public void ClearTextBox(TextBox tbx) //given a textbox, remove the current value
        {
            tbx.Text = "";            
        }
    }
}
