using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fixit
{
    class FeeSlipItmTrans
    {
        #region Variables
        private string _itmtrn_no;
        public string Itmtrn_no
        {
            get { return _itmtrn_no; }
            set { _itmtrn_no = value; }
        }
        
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



        //CW 2/24/2014 Solution 523 - Err 217887 when selecting carrier on tab of insurance ledger
        //testing new for item_trn_reason > 75
        private string _itmtrn_reason_length;
        public string Itmtrn_reason_length
        {
            get { return _itmtrn_reason_length; }
            set { _itmtrn_reason_length = value; }
        }

        private string _itmtrn_reason;
        public string Itmtrn_reason
        {
            get { return _itmtrn_reason; }
            set { _itmtrn_reason = value; }
        }



        #endregion
    }
}
