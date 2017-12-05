using System;
using System.Data;
using System.Data.SqlClient;

namespace NSysDB
{
    public partial class DB
    {
        /// <summary>
        /// 資料庫函式庫
        /// </summary>
        public class Kind1 : IDisposable
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
                    cnnDB.Close();
                    cnnDB.Dispose();
                }
                disposed = true;
            }

            ~Kind1()
            {
                Dispose(false);
            }
            #endregion

            private SqlConnection cnnDB;

            public Kind1()
            {
                cnnDB = NSysDB.DB.GetKind1Connection();
                cnnDB.Open();
            }

            #region Scalar
            public object ExecuteScalar(SqlCommand cmd)
            {
                object objResult;

                cmd.Connection = cnnDB;
                objResult = cmd.ExecuteScalar();

                return objResult;
            }

            public object ExecuteScalar(string SQL)
            {
                object objResult;

                using (SqlCommand cmd = new SqlCommand(SQL))
                {
                    objResult = this.ExecuteScalar(cmd);
                }

                return objResult;
            }
            #endregion

            #region Query
            public bool ExecuteQuery(SqlCommand cmd, DataSet ds)
            {
                bool blnResult = false;

                cmd.Connection = cnnDB;
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    if (da.Fill(ds) > 0)
                    {
                        blnResult = true;
                    }
                }

                return blnResult;
            }

            public bool ExecuteQuery(string SQL, DataSet ds)
            {
                bool blnResult = false;

                using (SqlCommand cmd = new SqlCommand(SQL))
                {
                    blnResult = this.ExecuteQuery(cmd, ds);
                }

                return blnResult;
            }

            public bool ExecuteQuery(SqlCommand cmd, DataSet ds, int RecordStartIndex, int RecordCount)
            {
                bool blnResult = false;

                cmd.Connection = cnnDB;
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    if (da.Fill(ds, RecordStartIndex, RecordCount, "TableReward") > 0)
                    {
                        blnResult = true;
                    }
                }

                return blnResult;
            }

            public bool ExecuteQuery(string SQL, DataSet ds, int RecordStartIndex, int RecordCount)
            {
                bool blnResult = false;

                using (SqlCommand cmd = new SqlCommand(SQL))
                {
                    blnResult = this.ExecuteQuery(cmd, ds, RecordStartIndex, RecordCount);
                }

                return blnResult;
            }

            public bool ExecuteQuery(SqlCommand cmd, out SqlDataReader dr)
            {
                cmd.Connection = cnnDB;
                dr = cmd.ExecuteReader();

                return dr.HasRows;
            }

            public bool ExecuteQuery(string SQL, out SqlDataReader dr)
            {
                bool blnResult = false;

                using (SqlCommand cmd = new SqlCommand(SQL))
                {
                    blnResult = this.ExecuteQuery(cmd, out dr);
                }

                return blnResult;
            }
            #endregion


            #region NonQuery
            public int ExecuteNonQuery(SqlCommand cmd)
            {
                int intResult;
                SqlTransaction trsReward;

                trsReward = cnnDB.BeginTransaction();
                cmd.Connection = cnnDB;
                cmd.Transaction = trsReward;
                try
                {
                    intResult = cmd.ExecuteNonQuery();
                    trsReward.Commit();
                }
                catch (Exception ex)
                {
                    trsReward.Rollback();
                    //throw new ApplicationException("發生未指定的錯誤");
                    throw (ex);
                }

                return intResult;
            }

            public int ExecuteNonQuery(string SQL)
            {
                int intResult;

                using (SqlCommand cmd = new SqlCommand(SQL))
                {
                    intResult = this.ExecuteNonQuery(cmd);
                }

                return intResult;
            }
            #endregion

        }

    }
}
