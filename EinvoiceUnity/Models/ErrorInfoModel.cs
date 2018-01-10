using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EinvoiceUnity.Models
{
    public class ErrorInfoModel
    {
        Dictionary<string, EinvoiceErrorMain> m_errorBuffer = new Dictionary<string, EinvoiceErrorMain>();
        public Dictionary<string, EinvoiceErrorMain> ErrorBuffer { get { return m_errorBuffer; } set { m_errorBuffer = value; } }
    }


    public class EinvoiceErrorMain
    {
        private List<EinvoiceErrorDetails> m_details = new List<EinvoiceErrorDetails>();
        public List<EinvoiceErrorDetails> Details { get { return m_details; } set { m_details = value; } }
    }
    public class EinvoiceErrorDetails
    {
        public int ErrorGroupKey { get; set; }
        public string EinvoiceNumber { get; set; }
        public string ErrorMessage { get; set; }
        public short ErrorLevel { get; set; }
        public string SourceFile { get; set; }
        public string OtherMessage { get; set; }
    }
}
