using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fixit
{
    public class FeeSlip
    {
        #region Variables
        private string _feeslip_no;
        public string Feeslip_no
        {
            get { return _feeslip_no; }
            set { _feeslip_no = value; }
        }

        private string _slipItem_no;
        public string SlipItem_no  //used for unknown insurance
        {
            get { return _slipItem_no; }
            set { _slipItem_no = value; }
        }
      
        private string  _patientTotal;
        public string  PatientTotal
        {
            get { return _patientTotal; }
            set { _patientTotal = value; }
        }

        private string  _insuranceTotal;
        public string  InsuranceTotal
        {
            get { return _insuranceTotal; }
            set { _insuranceTotal = value; }
        }


        #endregion


    }
}
