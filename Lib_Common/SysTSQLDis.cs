using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace NSysDB
{
    namespace NTSQL
    {
        /// <summary>
        /// T-SQL資料存取函式庫
        /// </summary>
        public partial class SQL1 : IDisposable
        {

            #region 實作IDisposable
            private bool disposed = false;

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            private void Dispose(bool disposing)
            {
                if (!this.disposed)
                {
                    if (disposing)
                    {
                        // TODO: 明確呼叫時釋放 Unmanaged 資源
                    }
                    // TODO: 釋放共用的 Unmanaged 資源
                    dbNTSQL.Dispose();
                }
                disposed = true;
            }

            ~SQL1()
            {
                Dispose(false);
            }
            #endregion

            private NSysDB.DB.Kind1 dbNTSQL;

            public SQL1()
            {
                dbNTSQL = new NSysDB.DB.Kind1();
            }


        }


    }


}
