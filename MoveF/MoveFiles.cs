using NSysDB.NTSQL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

public class MoveFiles
{
    private string m_processName = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
    public string ProcessName { get { return m_processName; } set { m_processName = value; } }
    private static Dictionary<string, List<string>> m_errorEinvoiceNums = new Dictionary<string, List<string>>();
    public static Dictionary<string, List<string>> ErrorEinvoiceNums { get { return m_errorEinvoiceNums; } set { m_errorEinvoiceNums = value; } }

    public void Begin(string sKind, string sSource, string sToWhere, bool isTest = false)
    {
        string sPgSN = DateTime.Now.ToString("yyyyMMddHHmmssfff");

        if (sKind == "0")
        { sKind = "*.*"; }
        else
        { sKind = sKind.ToUpper() + "*.*"; }

        foreach (string sourceFile in Directory.GetFileSystemEntries(sSource, sKind))
        {
            var extension = Path.GetExtension(sourceFile);
            //ExXml為生成Xml的目錄，判斷來源非Xml目錄 再進行Test以及目錄判斷
            if (isTest)
            {
                if (sSource.IndexOf("ExXml") == -1)
                {
                    if (sourceFile.ToUpper().IndexOf("TEST") >= 0 || string.IsNullOrEmpty(extension))
                    {
                        if (sourceFile.ToUpper().IndexOf("TESTTEST") == -1)
                            continue;
                    }
                }
            }
            else
            {
                if (sSource.IndexOf("ExXml") == -1)
                {
                    if (sourceFile.ToUpper().IndexOf("TEST") >= 0 || string.IsNullOrEmpty(extension))
                        continue;
                }
            }

            string processName = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;

            string dirName = Path.GetDirectoryName(sToWhere).Replace(":", "_").Replace(@"\", "_");
            string fileFullName = Path.GetFileName(sourceFile);
            string errorFileIdentify = DateTime.Now.ToString("yyyyMMddHHmm") + "_" + new Random().Next(5000, 20000).ToString() + "_" + dirName + "_hasfile_" + fileFullName;
            string errorDir = @"D:\eInvoiceFile\ImErrorTXT\";
            string errorFileName = string.Concat(errorDir, errorFileIdentify);
            bool moveHasFile = false;

            string fileNewStage = sourceFile.Replace(sSource, sToWhere);
            using (NSysDB.NTSQL.SQL1 query = new NSysDB.NTSQL.SQL1())
            {
                try
                {
                    if (File.Exists(fileNewStage))
                    {
                        File.Move(sourceFile, errorFileName);
                        moveHasFile = true;
                        throw new Exception(sToWhere + "=>檔案已存在：" + fileFullName + "，錯誤檔案識別名稱：" + errorFileIdentify);
                    }
                    File.Move(sourceFile, fileNewStage);
                    query.GoLogsAll(sPgSN, processName, sourceFile, "", "", 99);
                }
                catch (Exception ex)
                {
                    if (!moveHasFile)
                        File.Move(sourceFile, errorFileName);
                    query.GoLogsAll(sPgSN, processName, sourceFile, ex.ToString(), "", 15);
                }
            }
        }
    }

    /// <summary>
    /// 移動D:\
    /// </summary>
    public string MoveToTempDatabase(List<string> getEinvoiceTypes, string dirID = null, bool isTest = false)
    {
        var today = DateTime.UtcNow.AddHours(8);
        string dateSn = string.Concat(
            today.Year, 
            today.Month.ToString().PadLeft(2, '0'),
            today.Day.ToString().PadLeft(2, '0'),".",
            today.Hour.ToString().PadLeft(2, '0'),
            today.Minute.ToString().PadLeft(2, '0'),
            ".");
        string tempID = dateSn + new Random().Next(22, 555);
        string txtSource = dirID == null || dirID == "ALL" ? @"D:\ImInputERP" : @"D:\ImInputERP\" + dirID;
        string tempDir = @"D:\eInvoiceFile\historyText\";
        string virtualTempDir = string.Concat(tempDir, tempID, "\\");
        //bool isEmpty = !Directory.EnumerateFiles(txtSource).Any();
        //if (isEmpty)
        //    return null;
        if (!Directory.Exists(virtualTempDir))
            Directory.CreateDirectory(virtualTempDir);
        if (!Directory.Exists(txtSource))
        {
            throw new Exception("[寫入暫存]無法取得目錄，是否尚未建立." + txtSource);
        }
        using (SQL1 query = new SQL1())
        {
            string catchFilePath = string.Empty;
            try
            {
                Console.WriteLine("開始搬檔...");
                Console.WriteLine(string.Format("從{0}移動至{1}", txtSource, virtualTempDir));

                //根據條件搬移指定的檔名
                foreach (var type in getEinvoiceTypes)
                {

                    int index = 1;
                    //move all files to temp directory
                    foreach (string file in Directory.GetFileSystemEntries(txtSource, type + "*.*"))
                    {
                        var extension = Path.GetExtension(file);
                        if (isTest)
                        {
                            if (file.IndexOf("ExXml") == -1)
                            {
                                if (file.ToUpper().IndexOf("TEST") >= 0 || string.IsNullOrEmpty(extension))
                                {
                                    if (file.ToUpper().IndexOf("TESTTEST") == -1)
                                        continue;
                                }
                            }
                        }
                        else
                        {
                            if (file.IndexOf("ExXml") == -1)
                            {
                                if (file.ToUpper().IndexOf("TEST") >= 0 || string.IsNullOrEmpty(extension))
                                    continue;
                            }
                        }

                        Console.WriteLine(string.Format("{0}第{1}筆 開始.", type, index));
                        catchFilePath = file;
                        //File.Copy(file, Path.Combine(virtualTempDir, Path.GetFileName(file)));
                        File.Move(file, Path.Combine(virtualTempDir, Path.GetFileName(file)));
                        Console.WriteLine(string.Format("{0}第{1}筆 完成.", type, index));
                        index++;
                    }
                }
            }
            catch (Exception ex)
            {
                query.GoLogsAll(dateSn, ProcessName, catchFilePath + "[寫入暫存]請檢視檔案是否完成移動至暫存目錄.", ex.Message, "0", 13);
            }
        }
        return virtualTempDir;
    }

    public void WriteDataHandler(string path, string fileType, string identityKey)
    {
        fileType = fileType.ToUpper();
        WriteDataToDatabase(path, fileType, identityKey);
    }

    /// <summary>
    /// 將TXT寫入暫存資料庫
    /// </summary>
    /// <param name="path">暫存目錄路徑</param>
    /// <param name="fileType">發票種類 ex:A0401H</param>
    private void WriteDataToDatabase(string path, string fileType, string identityKey)
    {
        using (SQL1 query = new SQL1())
        {
            foreach (string sourceFile in System.IO.Directory.GetFileSystemEntries(path, fileType + "*.*"))
            {
                int index = 1;
                string einvoiceNumber = string.Empty;
                using (StreamReader txtFile = new StreamReader(sourceFile, Encoding.Default))
                {
                    try
                    {
                        string line = "";
                        while ((line = txtFile.ReadLine()) != null)
                        {
                            Console.WriteLine(string.Format(fileType + "寫入暫存資料庫 第{0}筆 開始.", index));
                            if (line.Trim() != "")
                            {
                                string[] charA = line.Split(new string[] { ";" }, StringSplitOptions.None);
                                einvoiceNumber = charA[0];

                                #region 檢查Head是否寫入時有錯誤 若有，明細一併不寫入

                                string headString = fileType.Replace("H", "").Replace("D", "") + "H";
                                if (ErrorEinvoiceNums.ContainsKey(headString))
                                {
                                    var errEinvoice = ErrorEinvoiceNums[headString].Where(o => o == charA[0]).ToList();
                                    if (errEinvoice.Count > 0)
                                        continue;
                                }

                                #endregion 檢查Head是否寫入時有錯誤 若有，明細一併不寫入

                                Hashtable hashTable = new Hashtable();
                                hashTable["IDENT_KEY"] = identityKey;
                                hashTable["FILE_NM"] = sourceFile;
                                hashTable["FILE_CONTENT"] = line;
                                hashTable["REMARK"] = "";
                                hashTable["EINVOICE_TP"] = fileType;
                                hashTable["EINVOICE_NUM"] = charA[0].ToUpper();
                                hashTable["BUD_DATE"] = DateTime.UtcNow.AddHours(8);
                                string insertMsg = query.InsertDataNonKey("FILE_TEMP", hashTable);

                                #region 寫入有錯誤之處理

                                if (!string.IsNullOrEmpty(insertMsg))
                                {
                                    query.GoLogsAll(fileType, ProcessName, sourceFile, "[寫入暫存][TXT寫入暫存發生錯誤，若有明細將不寫入]" + insertMsg, index.ToString(), 51);
                                    if (!ErrorEinvoiceNums.ContainsKey(fileType))
                                        ErrorEinvoiceNums.Add(fileType, new List<string>());
                                    ErrorEinvoiceNums[fileType].Add(einvoiceNumber);
                                }

                                #endregion 寫入有錯誤之處理
                            }
                            Console.WriteLine(string.Format(fileType + "寫入暫存資料庫 第{0}筆 完成.", index));
                            index++;
                            einvoiceNumber = "";
                        }
                    }
                    catch (Exception ex)
                    {
                        query.GoLogsAll(fileType, ProcessName, sourceFile, "[寫入暫存" + ex.Message, index.ToString(), 13);
                    }
                }
            }
        }
    }

    public string GetIdentityKey()
    {
        var date = DateTime.UtcNow.AddHours(8).AddMinutes(10);
        string radmStr = string.Concat(date.Second, date.Year, date.Month);
        string guid = string.Concat(radmStr, Guid.NewGuid().ToString());
        SHA256 sha256 = new SHA256CryptoServiceProvider();//建立一個SHA256
        byte[] source = Encoding.Default.GetBytes(guid);//將字串轉為Byte[]
        byte[] crypto = sha256.ComputeHash(source);//進行SHA256加密
        string result = Convert.ToBase64String(crypto);//把加密後的字串從Byte[]轉為字串
        return result;
    }
}