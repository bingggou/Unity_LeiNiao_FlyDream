using System.Xml;

namespace Excel
{
    /// <summary>
    /// 一列数据
    /// </summary>
    public class Sheet1CollumData
    {
        //A1 B1 C1 ...   <c r="A1"
        public string Position = "";
        // s="1"
        public string AttrS = "1";
        //是否有 t="s"这个属性
        public bool IsInt = false;
        //<v>1</v>，如果是int，该值是最终值，否则是索引
        public string value = "";

        public string ShareValue = "";

        public int Row = 0;

        //第几列
        private int _collum = 0;

        public void Parse(XmlElement node)
        {
            Position = node.GetAttribute("r");
            AttrS = node.GetAttribute("s");
            IsInt = !node.HasAttribute("t");
            if (node["v"] != null)
            {
                value = node["v"].InnerText;
            }

            string tempStr = Position;
            string numStr = "";
            //消除数字
            for (int i = 0; i < tempStr.Length; ++i)
            {
                if ("0123456789".IndexOf(tempStr[i]) >= 0)
                    numStr += tempStr[i];
            }
            tempStr = tempStr.Replace(numStr, "");

            char[] posChars = tempStr.ToCharArray();
            string charString = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            int column = 0;
            for (int i = 0; i < posChars.Length; ++i)
            {
                if (charString.IndexOf(posChars[i]) >= 0)
                {
                    if (i != posChars.Length - 1)
                        column += (posChars[i] - 'A' + 1) * (int)System.Math.Pow(('Z' - 'A' + 1), (posChars.Length - 1 - i));
                    else
                        column += (posChars[i] - 'A') * (int)System.Math.Pow(('Z' - 'A' + 1), (posChars.Length - 1 - i));
                }
            }

            Row = int.Parse(numStr);
            _collum = column;
        }


        public static XmlElement ToXmlElement(XmlDocument doc, Sheet1CollumData data)
        {
            XmlElement xmlNode = doc.CreateElement("c", doc.DocumentElement.NamespaceURI);
            xmlNode.SetAttribute("r", data.Position);
            xmlNode.SetAttribute("s", data.AttrS);
            if (!data.IsInt) xmlNode.SetAttribute("t", "s");
            XmlElement vNode = doc.CreateElement("v", doc.DocumentElement.NamespaceURI);
            vNode.InnerText = data.value;

            xmlNode.AppendChild(vNode);

            return xmlNode;
        }

        public void SetShareValue(Sheet1SharedData shareData)
        {
            if (shareData != null && !IsInt)
            {
                ShareValue = shareData.GetValue(int.Parse(value));
            }
            else
            {
                ShareValue = value;
            }
        }

        /// <summary>
        /// 获取当前在第几列，从0开始
        /// </summary>
        /// <returns></returns>
        public int GetCollumNum()
        {
            return _collum;
        }

        /// <summary>
        /// row从1开始，collum从0开始
        /// </summary>
        /// <param name="value"></param>
        /// <param name="shareValue"></param>
        /// <param name="row"></param>
        /// <param name="collum"></param>
        /// <param name="isInt"></param>
        /// <returns></returns>
        public static Sheet1CollumData CreateData(string value, string shareValue, int row, int collum, bool isInt)
        {
            Sheet1CollumData data = new Sheet1CollumData();
            data.value = value;
            data._collum = collum;
            data.Position = CreateCollumWord(collum) + row;
            data.IsInt = isInt;
            data.ShareValue = shareValue;

            return data;
        }

        private static string CreateCollumWord(int collum)
        {
            int normal = 'Z' - 'A' + 1;
            int num = collum / normal;
            int more = collum % normal;
            string value = "";
            value = "" + (char)('A' + more);
            while (num > 0)
            {
                more = num % normal;

                if (num <= normal)
                {
                    value = "" + (char)('A' + num - 1) + value;
                    break;
                }
                else
                    value = "" + (char)('A' + more - 1) + value;
                num = num / normal;
            }

            return value;
        }
    }
}
