using Excel;
using System.IO;
using UnityEngine;

public class ExcelTool
{

    public static string ExcelModelFilePath
    {
        get
        {
            return ExcelPath + "/Model.xlsx";
        }
    }

    public static string ExcelPath_FSM
    {
        get
        {
            return ExcelPath + "/FSM/";
        }
    }

    public static string ExcelPath
    {
        get
        {
            return Application.streamingAssetsPath + "/Excel";
        }
    }


    public static ExcelTable LoadTable(string excelPath)
    {
        if (File.Exists(excelPath))
        {

            return new ExcelTable(excelPath);
        }
        else
        {
            Debug.Log("excel不存在");
            return null;
        }
    }

    public static ExcelTable CreateExcel(string originalModelFile, string aimPath, string aimName)
    {
        if (!Directory.Exists(aimPath))
        {
            Directory.CreateDirectory(aimPath);
        }
        File.Copy(originalModelFile, aimPath + aimName, true);
        return LoadTable(aimPath + aimName);
    }

    public static void Set1ColumnBaseInfo(ExcelTable table, int columnNum, string propName, string typeName, string intro)
    {
        table[0, columnNum] = propName;
        table[1, columnNum] = typeName;
        table[2, columnNum] = intro;
    }

    public static void Set1RowInfo(ExcelTable table, int rowNum, params string[] strs)
    {
        for (int i = 0; i < strs.Length; i++)
        {
            table[rowNum, i] = strs[i];
        }
    }
    public static void Set1RowInfoById(string id, ExcelTable table, params string[] strs)
    {
        if (table.rowNumDic.ContainsKey(id))
        {
            Set1RowInfo(table, table.rowNumDic[id], strs);
        }
        else
        {
            int aimRowIndex;
            if (table.rowNumDic.Count ==0)
            {
                aimRowIndex = 3;
            }
            else
            {
                aimRowIndex = table.GetRowsCount();
            }
            Set1RowInfo(table, aimRowIndex, strs);
        }


    }
 
}
