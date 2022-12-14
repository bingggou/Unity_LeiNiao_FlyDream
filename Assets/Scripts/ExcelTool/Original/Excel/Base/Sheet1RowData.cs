using System.Collections.Generic;
using System.Xml;

namespace Excel
{
    /// <summary>
    /// 一行数据，包含若干列。行是从1开始，列从0开始
    /// </summary>
    public class Sheet1RowData
    {
        public int Row = 0;
        public int SpansMin = 1;
        public int SpansMax = 0;

        private List<Sheet1CollumData> _collums = null;

        private Sheet1SharedData _shareData = null;

        public Sheet1RowData(Sheet1SharedData shareData)
        {
            _shareData = shareData;
        }

        public void ParseRowNode(XmlElement node)
        {
            Row = int.Parse(node.GetAttribute("r"));
            var span = node.GetAttribute("spans");
            if (!string.IsNullOrEmpty(span))
            {
                string[] spans = span.Split(':');
                SpansMin = int.Parse(spans[0]);
                SpansMax = int.Parse(spans[1]);
            }

            _collums = new List<Sheet1CollumData>();

            for (int i = 0; i < node.ChildNodes.Count; ++i)
            {
                try
                {
                    Sheet1CollumData data = new Sheet1CollumData();
                    data.Parse(node.ChildNodes.Item(i) as XmlElement);
                    data.SetShareValue(_shareData);
                    var posColumn = data.GetCollumNum();
                    if (posColumn >= _collums.Count)
                    {
                        var delta = posColumn + 1 - _collums.Count;
                        for (int d = 0; d < delta; ++d)
                        {
                            _collums.Add(null);
                        }
                    }
                    _collums[data.GetCollumNum()] = data;
                }
                catch (System.Exception)
                {
                }
            }
        }

        //尝试扩展列
        public void TryExpandCollumn(int collumns)
        {
            if (_collums == null)
                _collums = new List<Sheet1CollumData>();

            var curCollumn = _collums.Count;
            if (collumns >= curCollumn)
            {
                var delta = collumns - curCollumn + 1;
                for (int i = 0; i < delta; ++i)
                {
                    _collums.Add(null);
                }
            }
        }

        public List<Sheet1CollumData> GetCollumDatas()
        {
            return _collums;
        }

        public int GetCollumnCount()
        {
            if (_collums == null)
                return 0;

            return _collums.Count;
        }

        public string[] ToArray()
        {
            if (_collums == null) return null;

            string[] dataArray = new string[_collums.Count];
            for (int i = 0; i < _collums.Count; ++i)
            {
                if (_collums[i] != null)
                {
                    dataArray[i] = _collums[i].ShareValue;
                }
            }

            return dataArray;
        }

        public static XmlElement ToXmlElement(XmlDocument doc, Sheet1RowData data)
        {
            XmlElement rowNode = doc.CreateElement("row", doc.DocumentElement.NamespaceURI);
            rowNode.SetAttribute("r", "" + data.Row);
            if (data.SpansMax > 0)
                rowNode.SetAttribute("spans", string.Format("{0}:{1}", data.SpansMin, data.SpansMax));

            for (int i = 0; i < data._collums.Count; ++i)
            {
                if (data._collums[i] != null)
                {
                    XmlElement collumNode = Sheet1CollumData.ToXmlElement(doc, data._collums[i]);
                    rowNode.AppendChild(collumNode);
                }
            }

            return rowNode;
        }

        /// <summary>
        /// 在第几列插入数据， collum是从0开始的
        /// </summary>
        /// <param name="value"></param>
        /// <param name="collum"></param>
        public void AddValue(string value, int collum)
        {
            if (collum >= _collums.Count) return;

            int iValue = -1;
            bool isInt = int.TryParse(value, out iValue);
            if (_collums[collum] == null)
            {
                string shareValue = value;
                if (!isInt)
                {
                    //value转换成索引，指向shareXml中的第几个值
                    value = "" + _shareData.AddValue(shareValue);
                }
                Sheet1CollumData data = Sheet1CollumData.CreateData(value, shareValue, Row, collum, isInt);
                _collums[collum] = data;
            }
            else
            {
                if (isInt)
                {
                    _collums[collum].value = value;
                    _collums[collum].ShareValue = value;
                }
                else
                {
                    string shareValue = value;
                    if (!isInt)
                    {
                        //value转换成索引，指向shareXml中的第几个值
                        value = "" + _shareData.AddValue(shareValue);
                    }
                    _collums[collum].value = value;
                    _collums[collum].ShareValue = shareValue;
                }
            }

            _collums[collum].IsInt = isInt;
        }

        public static Sheet1RowData CreateData(int row, int maxCollum, Sheet1SharedData shareData, string[] dataArray)
        {
            Sheet1RowData data = new Sheet1RowData(shareData);
            data.Row = row;
            data.SpansMax = maxCollum;
            data._collums = new List<Sheet1CollumData>(maxCollum);
            for (int i = 0; i < dataArray.Length; ++i)
            {
                if (string.IsNullOrEmpty(dataArray[i])) continue;
                data.AddValue(dataArray[i], i);
            }

            return data;
        }
    }
}
