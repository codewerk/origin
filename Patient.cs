using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fixit
{
    class Patient
    {        //used for the send statement checkbox processing

         #region Variables

            private string _patient_ID;
            public string Patient_ID
            {
                get { return _patient_ID; }
                set { _patient_ID = value; }
            }

            private string _last_Name;
            public string Last_Name  
            {
                get { return _last_Name; }
                set { _last_Name = value; }
            }
          
            private string _first_Name;
            public string First_Name  
            {
                get { return _first_Name; }
                set { _first_Name = value; }
            }
       
          #endregion
    }
}
