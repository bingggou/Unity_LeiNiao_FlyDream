using System.Collections.Generic;
using UnityEngine;
namespace Excel
{
    /// <summary>
    /// 所有表格数据
    /// </summary>
    public class ExcelTable : XmlReadExcel
    {
        private Sheet1Data data;
        public string tableName;
        public Dictionary<string, int> rowNumDic = new Dictionary<string, int>();


        public void ShowLog()
        {

            for (int i = 0; i < GetCollumnsCount(); i++)
            {
                for (int j = 0; j < GetRowsCount(); j++)
                {
                    Debug.Log(this[j,i]);
                }
            }

        }


        public ExcelTable(string excelPath)
        {
            Parse(excelPath);
            data = _sheetData;
            List<Sheet1RowData> allRows = data.GetAllData();
            if (allRows.Count > 3)
            {
                for (int i = 3; i < allRows.Count; i++)
                {
                    if (rowNumDic.ContainsKey(this[i, 0]))
                    {
                        Debug.LogError("id重复！！");
                    }
                    else
                    {
                        rowNumDic.Add(this[i, 0], i);
                    }
                }
            }

        }


        public string this[int i, int j]
        {
            get
            {
                var rowDatas = data.GetAllData();
                if (rowDatas == null || i >= rowDatas.Count)
                    return null;

                var rows = rowDatas[i];
                var columns = rows.GetCollumDatas();
                if (j >= columns.Count || columns[j] == null)
                    return null;
                return columns[j].ShareValue;
            }
            set
            {
                TryExpandSheet(i, j);

                var allDatas = data.GetAllData();
                var rowData = allDatas[i];
                rowData.AddValue(value, j);
            }
        }

        public int GetRowsCount()
        {
            var allData = data.GetAllData();
            return allData.Count;
        }

        public int GetCollumnsCount()
        {
            var allData = data.GetAllData();
            int lenth = 0;
            for (int i = 0; i < allData.Count; i++)
            {
                int count = allData[i].GetCollumnCount();
                lenth = count < lenth ? lenth : count;
            }
            return lenth;
        }

        //尝试扩展表格，如果row或者collumn比存放的索引更大，则需要扩充
        private void TryExpandSheet(int row, int collumn)
        {
            var rowDatas = data.GetAllData();
            if (row >= rowDatas.Count)
            {
                int deltaRow = row - rowDatas.Count + 1;
                for (int i = 0; i < deltaRow; ++i)
                {
                    rowDatas.Add(null);
                }
            }

            if (rowDatas[row] == null)
            {
                rowDatas[row] = new Sheet1RowData(_shareData);
                rowDatas[row].Row = row + 1;
                rowDatas[row].SpansMax = rowDatas.Count;
            }

            var curRowData = rowDatas[row];

            curRowData.TryExpandCollumn(collumn);
        }
    }
}
