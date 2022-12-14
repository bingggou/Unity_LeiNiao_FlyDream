using System.Collections.Generic;
using System.Xml;

namespace Excel
{
    /// <summary>
    /// Excel文件中所有非int值的集合,sharedStrings.xml，值从0开始
    /// </summary>
    public class Sheet1SharedData
    {
        public string Value = "";

        private XmlDocument _document = null;
        private List<string> _valueArray = null;
        public void Parse(XmlDocument document)
        {
            _document = document;
            var nodeList = _document.GetElementsByTagName("si");
            _valueArray = new List<string>(nodeList.Count);
            for (int i = 0; i < nodeList.Count; ++i)
            {
                var node = nodeList[i] as XmlElement;
                var tNodes = node.GetElementsByTagName("t");
                string text = "";
                for (int j = 0; j < tNodes.Count; ++j)
                {
                    text += tNodes[j].InnerText;
                }

                _valueArray.Add(text);
            }
        }

        /// <summary>
        /// 获取第几行的值，从0开始
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string GetValue(int index)
        {
            if (index >= _valueArray.Count)
                return "";

            return _valueArray[index];
        }

        public int AddValue(string value)
        {
            bool find = false;
            int ret = 0;
            for (int i = 0; i < _valueArray.Count; ++i)
            {
                if (value.CompareTo(_valueArray[i]) == 0)
                {
                    ret = i;
                    find = true;
                    break;
                }
            }

            if (!find)
            {
                //把值加到list中
                _valueArray.Add(value);
                //在shareXml中添加新的节点
                AddNewNode(value);
                ret = _valueArray.Count - 1;
            }

            return ret;
        }

        /// <summary>
        /// 创建新的si节点
        /// <si>
		///     <t>value</t>
	    /// </si>
        /// </summary>
        /// <param name="value"></param>
        private void AddNewNode(string value)
        {
            XmlElement siNode = _document.CreateElement("si", _document.DocumentElement.NamespaceURI);
            XmlElement tNode = _document.CreateElement("t", _document.DocumentElement.NamespaceURI);
            tNode.InnerText = value;
            siNode.AppendChild(tNode);

            _document.DocumentElement.AppendChild(siNode);
        }
    }
}
