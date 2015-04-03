using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fixit
{
    class Exam
    {                
        public string ExamRec  { get; set; }
        public string PatientNo  { get; set; }
        public string first_name  { get; set; }
        public string last_name { get; set; }
        public string RecType { get; set; } //supports the BMI solution. This identifies the measurement as being HT or WT
        public string Text2 { get; set; } //supports the BMI solution. This would be the measurement for HT or WT

    }
}
