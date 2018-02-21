using EinvoiceUnity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EinvoiceUnity.repositories
{
    public class EinvoiceRepository
    {
        public static void AddEinvoiceToErrorBuffer(
            string sKind,
            string einvNum,
            string errorMsg,
            int errorGroupKey,
            short errorLevel,
            ref ErrorInfoModel errorInfo,
            string processName,
            string sourceFile = null,
            string otherMsg = null
            )
        {
            if (!errorInfo.ErrorBuffer.ContainsKey(sKind))
                errorInfo.ErrorBuffer.Add(sKind, new EinvoiceErrorMain());
            errorInfo.ErrorBuffer[sKind].Details.Add(new EinvoiceErrorDetails()
            {
                ErrorGroupKey = errorGroupKey,
                EinvoiceNumber = einvNum,
                ErrorMessage = errorMsg,
                ErrorLevel = errorLevel,
                SourceFile = sourceFile,
                OtherMessage = otherMsg,
                ProcessName = processName
            });
        }
        public static bool CheckHeadHasError(string sKind0, string einvoiceNum, ErrorInfoModel errorInfo)
        {
            bool isFail = false;
            string kind = sKind0.Replace("D", "") + "H";
            var data = errorInfo.ErrorBuffer.Where(o => o.Key == kind).ToList();
            if (data.Count > 0)
                isFail = (data.Where(o => o.Value.Details.Any(w => w.EinvoiceNumber == einvoiceNum)).ToList().Count > 0);
            return isFail;
        }
    }
}
