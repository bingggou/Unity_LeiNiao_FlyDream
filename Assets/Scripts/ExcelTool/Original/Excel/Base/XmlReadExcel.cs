using System.IO;
using System.Xml;
using Excel.Package.Zip.Zip;
using UnityEngine;

namespace Excel
{
    /// <summary>
    /// 通过xml的方式来读取Excel
    /// </summary>
    public abstract class XmlReadExcel
    {
        protected const string ENTRY_SHEET = "xl/worksheets/sheet1.xml";
        protected const string ENTRY_DATA = "xl/sharedStrings.xml";
        //对应Excel里面的sheet表
        protected XmlDocument _sheetXml = null;
        //对应Excel里面的各项值
        protected XmlDocument _sharedStringsXml = null;

        protected string _excelPath = "";

        protected Sheet1SharedData _shareData = null;
        protected Sheet1Data _sheetData = null;

        ~XmlReadExcel()
        {

        }

        public virtual void Parse() { }
        public virtual void Save()
        {
            Save(_excelPath);
        }


        /// <summary>
        /// 将excel解析成xml
        /// </summary>
        /// <param name="path"></param>
        public virtual void Parse(string path)
        {
            try
            {
                _sheetXml = readExcel(path, ENTRY_SHEET);
                _sharedStringsXml = readExcel(path, ENTRY_DATA);

                _shareData = new Sheet1SharedData();
                _shareData.Parse(_sharedStringsXml);

                _sheetData = new Sheet1Data(_shareData);
                _sheetData.Parse(_sheetXml);
            }
            catch (System.Exception)
            {
            }
        }


        public virtual void Save(string path)
        {
            var tempXmlDoc = _sheetData.ToXmlDocument();
            //_sharedStringsXml.Save("../SubResource/UIConfig" + "/share.xml");
            //_sheetXml.Save("../SubResource/UIConfig" + "/sheet.xml");
            string tempFile = path + ".temp";
            FileStream fs = new FileStream(tempFile, FileMode.OpenOrCreate);
            ZipOutputStream zOut = new ZipOutputStream(fs);

            FileStream sr = File.OpenRead(_excelPath);
            ZipInputStream zInput = new ZipInputStream(sr);
            ZipEntry zEntry = null;
            while ((zEntry = zInput.GetNextEntry()) != null)
            {
                string fullPath = zEntry.Name;
                string fileName = Path.GetFileName(fullPath);
                if (fullPath == ENTRY_SHEET)
                {
                    ZipEntry sheetEntry = new ZipEntry(ENTRY_SHEET);

                }

                ZipEntry clone = new ZipEntry(zEntry.Name);
                zOut.PutNextEntry(clone);

                if (fullPath == ENTRY_SHEET)
                {
                    write(tempXmlDoc, zOut);
                }
                else if (fullPath == ENTRY_DATA)
                {
                    write(_sharedStringsXml, zOut);
                }
                else
                {
                    write(zOut, zInput);
                }
            }

            zInput.Close();
            zOut.Finish();
            zOut.Close();
            fs.Close();

            File.Delete(path);
            File.Copy(tempFile, path, true);
            File.Delete(tempFile);

            _sheetXml = tempXmlDoc;
        }

        protected void write(XmlDocument doc, ZipOutputStream zOut)
        {
            doc.Save(XmlWriter.Create(zOut));
        }

        protected void write(ZipOutputStream zOut, Stream stream, int bufferSize)
        {
            byte[] buffer = new byte[bufferSize];
            stream.Read(buffer, 0, bufferSize);
            zOut.Write(buffer, 0, bufferSize);
        }

        protected void write(ZipOutputStream zOut, ZipInputStream zInput)
        {
            int bufferSize = 1024;
            byte[] buffer = new byte[bufferSize];
            int len = 0;
            while ((len = zInput.Read(buffer, 0, bufferSize)) > 0)
            {
                zOut.Write(buffer, 0, len);
            }
        }


        /// <summary>
        /// 读取excel文件中对应的xml数据
        /// </summary>
        /// <param name="path"></param>
        /// <param name="entryFilePath"></param>
        /// <returns></returns>
        protected XmlDocument readExcel(string path, string entryFilePath)
        {
            _excelPath = path;
            FileStream sr = File.OpenRead(path);

            ZipInputStream s = new ZipInputStream(sr);
            //Debug.Log("s:" + s);

            ZipEntry zEntry = null;
            bool find = false;
            while ((zEntry = s.GetNextEntry()) != null)
            {
                string fullPath = zEntry.Name;
                string fileName = Path.GetFileName(fullPath);
                if (fullPath == entryFilePath)
                {
                    find = true;
                    break;
                }
            }

            try
            {
                if (find)
                {
                    XmlDocument xml = new XmlDocument();
                    xml.Load(s);
                    return xml;
                }
            }
            finally
            {
                s.Close();
                sr.Close();
            }
            return null;
        }

        /// <summary>
        /// 获取shared表中的值
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected string GetSharedValue(int index)
        {
            return _shareData.GetValue(index);
        }

        /// <summary>
        /// int型的字符串自加1
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected string strSumb1(string value)
        {
            int iValue = int.Parse(value);
            iValue++;

            return "" + iValue;
        }
    }
}
