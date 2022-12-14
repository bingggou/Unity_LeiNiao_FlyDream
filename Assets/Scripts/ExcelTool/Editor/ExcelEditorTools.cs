using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using LitJson;
using System.Text;
using System;
using Excel;

public class Excel4Unity : Editor
{
    static string testFolderPath = Application.streamingAssetsPath + "/Test/";
    static string ReadWriteStr = "ReadWrite.xlsx";
    static string CreateCSStr = "CreateCS.xlsx";
    static string excelPath_ReadWrite = testFolderPath + ReadWriteStr;
    static string excelPath_CreateCS = testFolderPath + CreateCSStr;

    [MenuItem("Excel工具/测试/写!(入ReadWrite.xlsx添加一行时间)")]
    static void TestWrite()
    {

        TestWriteSth(System.DateTime.Now.ToLongDateString());
    }

    [MenuItem("Excel工具/测试/读!(取ReadWrite.xlsx并打印所有数据)")]
    static void TestRead()
    {
        //if (!Directory.Exists(testFolderPath))
        //{
        //    Directory.CreateDirectory(testFolderPath);
        //}
        if (!File.Exists(excelPath_ReadWrite))
        {
            TestWriteSth("新建文件写入了这段话");
        }
        ExcelTable table = ExcelTool.LoadTable(excelPath_ReadWrite);
        table.ShowLog();
    }


    [MenuItem("Excel工具/测试/生成脚本!(CreateCS.xlsx对应的脚本文件)")]
    static void TestMakeCs()
    {
        //if (!Directory.Exists(testFolderPath))
        //{
        //    Directory.CreateDirectory(testFolderPath);
        //}
        ExcelTable table;

        if (File.Exists(excelPath_CreateCS))
        {
            table = ExcelTool.LoadTable(excelPath_CreateCS);
        }
        else
        {
            table = ExcelTool.CreateExcel(ExcelTool.ExcelModelFilePath, testFolderPath, CreateCSStr);
            table[0, 0] = "测试字符串";
            table[1, 0] = "string";
            table[3, 0] = "我是一条字符串";
            table[0, 1] = "测试整形";
            table[1, 1] = "int";
            table[3, 1] = "888";
            table[0, 2] = "测试浮点数";
            table[1, 2] = "float";
            table[3, 2] = "1.88888";

            table.Save();


            ExcelTable2CS(table, "CreateCSStr", excelPath_CreateCS);
        }
    }


        static void TestWriteSth(string value)
        {
            if (!Directory.Exists(testFolderPath))
            {
                Directory.CreateDirectory(testFolderPath);
            }

            if (!File.Exists(excelPath_ReadWrite))
            {
                ExcelTable table = ExcelTool.CreateExcel(ExcelTool.ExcelModelFilePath, testFolderPath, ReadWriteStr);
                table[0, 0] = value;
                table.Save();
            }
            else
            {
                ExcelTable table = ExcelTool.LoadTable(excelPath_ReadWrite);
                int aimRowNum = table.GetRowsCount();
                table[aimRowNum, 0] = value;
                table.Save();
            }
        }



    [MenuItem("Assets/生成选中的excel第一个表的脚本和json数据")]
    static void TestExcel2JSON()
    {
        UnityEngine.Object[] objs = Selection.objects;
        for (int i = 0; i < objs.Length; i++)
        {
            string path = AssetDatabase.GetAssetPath(objs[i]);
            if (path.EndsWith(".xlsx"))
            {
                Debug.Log(objs[i].name);
                string theJson = GetClassAndJsonDataFromExcel(path, objs[i].name);
                Debug.Log(theJson);
            }
            else
            {
                EditorUtility.DisplayDialog("提示", "暂不支持的文件格式" + path, "ok");
                return;
            }
        }
        AssetDatabase.Refresh();
    }
    ///// <summary>
    ///// 得到一个excel的所有表类，及json数据文件，json名为文件名
    ///// </summary>
    ///// <param name="path"></param>
    ///// <param name="createCS"></param>
    ///// <returns></returns>
    //static string GetClassAndJsonDataFromExcelAll(string path, string aimTableName, bool createCS = true)
    //{
    //    //		UnityEngine.Debug.LogError ("path " + path);
    //    if (!path.EndsWith("xlsx"))
    //    {
    //        return null;
    //    }

    //    string tableName = "";
    //    string currentPropName = "";
    //    int tableRow = 0;
    //    int tableColumn = 0;
    //    string v = "";
    //    ExcelTable aimTable = null;
    //    aimTable = ExcelTool.LoadTable(path);
    //    try
    //    {
    //        StringBuilder sb = new StringBuilder();
    //        JsonWriter writer = new JsonWriter(sb);
    //        writer.WriteObjectStart();
    //        foreach (ExcelTable table in aimTable.Tables)
    //        {
    //            tableName = aimTableName;


    //            if (createCS)
    //            {

    //                ExcelTable2CS(table, aimTableName, path);
    //            }
    //            writer.WritePropertyName(aimTableName);
    //            writer.WriteArrayStart();
    //            int rowNum = table.GetRows();
    //            int columnNum = table.GetCollumns();
    //            for (int i = 3; i < rowNum; i++)
    //            {
    //                tableRow = i;
    //                string idStr = table[i, 1];
    //                Debug.Log(idStr);
    //                if (idStr.Length <= 0)
    //                {
    //                    break;
    //                }
    //                writer.WriteObjectStart();

    //                for (int j = 0; j < columnNum; j++)
    //                {
    //                    tableColumn = j;
    //                    string propName = table[0, j];
    //                    string propType = table[1, j];
    //                    propName = propName.Replace("*", "");
    //                    currentPropName = propName;

    //                    if (propName.StartsWith("#"))
    //                    {
    //                        continue;
    //                    }
    //                    if (string.IsNullOrEmpty(propName) || string.IsNullOrEmpty(propType))
    //                    {
    //                        continue;
    //                    }
    //                    writer.WritePropertyName(propName);
    //                    v = table[i, j];
    //                    if (propType.Equals("int"))
    //                    {
    //                        int value = v.Length > 0 ? int.Parse(v) : 0;
    //                        writer.Write(value);
    //                    }
    //                    else if (propType.Equals("bool"))
    //                    {
    //                        int value = v.Length > 0 ? int.Parse(v) : 0;
    //                        writer.Write(value);
    //                    }
    //                    else if (propType.Equals("float"))
    //                    {
    //                        float value = v.Length > 0 ? float.Parse(v) : 0;
    //                        writer.Write(value);
    //                    }
    //                    else
    //                    {
    //                        string ss = table[i, j];

    //                        writer.Write(ss);
    //                    }
    //                }
    //                writer.WriteObjectEnd();
    //            }
    //            writer.WriteArrayEnd();
    //        }
    //        writer.WriteObjectEnd();
    //        string outputDir = Application.dataPath + "/Resources/JsonAndClass/";
    //        string outputPath = outputDir + Path.GetFileNameWithoutExtension(path) + ".txt";
    //        if (!Directory.Exists(outputDir))
    //        {
    //            Directory.CreateDirectory(outputDir);
    //        }
    //        string str = string.Empty;
    //        if (File.Exists(path))
    //        {
    //            byte[] bytes = File.ReadAllBytes(path);
    //            UTF8Encoding encoding = new UTF8Encoding();
    //            str = encoding.GetString(bytes);
    //        }
    //        string content = sb.ToString();
    //        if (str != content)
    //        {
    //            File.WriteAllText(outputPath, content);
    //        }
    //        Debug.Log("convert success! path = " + path);

    //        return sb.ToString();
    //    }
    //    catch (System.Exception e)
    //    {
    //        if (aimTable == null)
    //        {
    //            Debug.LogError("open excel failed!");
    //            Debug.LogError(e.StackTrace);
    //        }
    //        else
    //        {
    //            string msg = "解析错误！ \n表:" + tableName + " \n字段:" + currentPropName + "  \n第" + tableRow + "行,第" + tableColumn + "列 \nvalue = " + v;
    //            EditorUtility.DisplayDialog("error!", msg, "ok");
    //            Debug.LogError(e);
    //            Debug.LogError(e.StackTrace);
    //            Debug.LogError(msg);
    //        }
    //        return null;
    //    }
    //}

    /// <summary>
    /// 得到一个excel首个表类，及json数据文件，json名为表名
    /// </summary>
    /// <param name="path"></param>
    /// <param name="createCS"></param>
    /// <returns></returns>
    static string GetClassAndJsonDataFromExcel(string path, string aimTableName, bool createCS = true)
    {
        //		UnityEngine.Debug.LogError ("path " + path);
        if (!path.EndsWith("xlsx"))
        {
            return null;
        }

        string tableName = "";
        string currentPropName = "";
        int tableRow = 0;
        int tableColumn = 0;
        string v = "";
        ExcelTable aimTable = null;
        aimTable = ExcelTool.LoadTable(path);
        try
        {
            StringBuilder sb = new StringBuilder();
            JsonWriter writer = new JsonWriter(sb);
            writer.WriteObjectStart();
            if (aimTable == null)
            {
                return "";
            }
            ExcelTable table = aimTable;
            tableName = aimTableName;


            if (createCS)
            {
                ExcelTable2CS(table, aimTableName, path);
            }
            writer.WritePropertyName(aimTableName);
            writer.WriteArrayStart();
            int rowNum = table.GetRowsCount();
            int columnNum = table.GetCollumnsCount();
            for (int i = 3; i < rowNum; i++)
            {
                tableRow = i;
                string idStr = table[i, 1];
                Debug.Log(idStr);
                if (idStr.Length <= 0)
                {
                    break;
                }
                writer.WriteObjectStart();

                for (int j = 0; j < columnNum; j++)
                {
                    tableColumn = j;
                    string propName = table[0, j];
                    string propType = table[1, j];
                    // propName = propName.Replace("*", "");
                    currentPropName = propName;

                    if (propName.StartsWith("#"))
                    {
                        continue;
                    }
                    if (string.IsNullOrEmpty(propName) || string.IsNullOrEmpty(propType))
                    {
                        continue;
                    }
                    writer.WritePropertyName(propName);
                    v = table[i, j];
                    if (propType.Equals("int"))
                    {
                        int value = v.Length > 0 ? int.Parse(v) : 0;
                        writer.Write(value);
                    }
                    else if (propType.Equals("bool"))
                    {
                        int value = v.Length > 0 ? int.Parse(v) : 0;
                        writer.Write(value);
                    }
                    else if (propType.Equals("float"))
                    {
                        float value = v.Length > 0 ? float.Parse(v) : 0;
                        writer.Write(value);
                    }
                    else
                    {
                        string ss = table[i, j];

                        writer.Write(ss);
                    }
                }
                writer.WriteObjectEnd();
            }
            writer.WriteArrayEnd();

            writer.WriteObjectEnd();
            string outputDir = Application.dataPath + "/Resources/JsonAndClass/";
            string outputPath = outputDir + tableName + ".txt";
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }
            string str = string.Empty;
            if (File.Exists(path))
            {
                byte[] bytes = File.ReadAllBytes(path);
                UTF8Encoding encoding = new UTF8Encoding();
                str = encoding.GetString(bytes);
            }
            string content = sb.ToString();
            if (str != content)
            {
                File.WriteAllText(outputPath, content);
            }
            Debug.Log("convert success! path = " + path);

            return sb.ToString();
        }
        catch (System.Exception e)
        {
            if (aimTable == null)
            {
                Debug.LogError("open excel failed!");
                Debug.LogError(e.StackTrace);
            }
            else
            {
                string msg = "解析错误！ \n表:" + tableName + " \n字段:" + currentPropName + "  \n第" + tableRow + "行,第" + tableColumn + "列 \nvalue = " + v;
                EditorUtility.DisplayDialog("error!", msg, "ok");
                Debug.LogError(e);
                Debug.LogError(e.StackTrace);
                Debug.LogError(msg);
            }
            return null;
        }
    }

    static bool ExcelTable2CS(ExcelTable table, string tableName, string excelPath, string ignoreSymbol = "")
    {
        string modelPath = Application.dataPath + "/Scripts/ExcelTool/ExcelTypeItemModel.txt";
        string moudle = File.ReadAllText(modelPath);

        string properties = "";
        string parse = "";
        int tableColumn = 0;
        string getStrsfunctionStrs = "";
        try
        {
            int columnCount = table.GetCollumnsCount();
            for (int j = 0; j < columnCount; j++)
            {
                tableColumn = j;
                string propName = table[0, j].ToString();
                string propType = table[1, j].ToString().ToLower();
                if (propName != "")
                {
                    if (j == columnCount)
                    {
                        getStrsfunctionStrs += propName + ".ToString()";
                    }
                    else
                    {
                        getStrsfunctionStrs += propName + ".ToString(),";
                    }
                }
                if (!string.IsNullOrEmpty(ignoreSymbol) && propName.StartsWith(ignoreSymbol))
                {
                    continue;
                }
                if (string.IsNullOrEmpty(propName) || string.IsNullOrEmpty(propType))
                {
                    continue;
                }
                if (properties.Length == 0)
                {
                    properties += string.Format("\tpublic {0} {1};\n", propType, propName);
                    if (propType.Equals("string"))
                    {
                        properties += "\tpublic override string StringIdentity(){ return " + propName + "; }\n";
                    }
                    else
                    {
                        properties += "\tpublic override int Identity(){ return " + propName + "; }\n";
                    }
                }
                else
                {
                    properties += string.Format("\tpublic {0} {1};\n", propType, propName);
                }

                if (propType == "string")
                {
                    parse += string.Format("\t\t{0} = data[\"{1}\"].ToString();\n", propName, propName);
                }
                else if (propType == "bool")
                {
                    parse += string.Format("\t\t{0} = data[\"{1}\"].ToString() != \"0\";\n", propName, propName);
                }
                else if (propType == "int" || propType == "float" || propType == "double")
                {
                    parse += string.Format("\t\t{0} = {1}.Parse(data[\"{2}\"].ToString());\n", propName, propType, propName);
                }
                else if (propType == "string[]")
                {
                    string subType = propType.Replace("[]", "");
                    parse += string.Format("\t\tstring {0}_str = data[\"{1}\"].ToString();\n", propName, propName);
                    parse += "\t\tif(" + propName + "_str.Length > 0) { \n";
                    parse += string.Format("\t\t {0} = data[\"{1}\"].ToString().Split (';');\n", propName, propName);
                    string elseStr = string.Format("{0} = new {1}[0];", propName, subType);
                    parse += "\t\t} else {" + elseStr + "}\n";
                }
                else if (propType == "int[]" || propType == "float[]" || propType == "double[]")
                {
                    string subType = propType.Replace("[]", "");
                    parse += string.Format("\t\tstring {0}_str = data[\"{1}\"].ToString();\n", propName, propName);
                    parse += "\t\tif(" + propName + "_str.Length > 0) { \n";
                    parse += string.Format("\t\tstring[] {0}_data = data[\"{1}\"].ToString().Split (';');\n", propName, propName);
                    parse += string.Format("\t\t{0} = new {1}[{2}_data.Length];\n", propName, subType, propName);
                    parse += "\t\tfor (int i = 0; i < " + propName + "_data.Length; i++) { " + propName + "[i] = " + subType + ".Parse (" + propName + "_data [i]);}\n";
                    string elseStr = string.Format("{0} = new {1}[0];", propName, subType);
                    parse += "\t\t} else {" + elseStr + "}\n";
                }
                else
                {
                    Debug.LogError("generate .cs failed! " + propType + " not a valid type" + " " + "table:" + table);
                    return false;
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
            Debug.LogError("generate .cs failed: " + table + "!" + " " + "table:" + excelPath);
            return false;
        }




        moudle = moudle.Replace("{0}", tableName);
        moudle = moudle.Replace("{1}", properties);
        moudle = moudle.Replace("{2}", tableName);
        moudle = moudle.Replace("{3}", parse);
        moudle = moudle.Replace("{4}", getStrsfunctionStrs);
        moudle = moudle.Replace("{5}", excelPath);



        string path = Application.dataPath + "/Resources/JsonAndClass/" + tableName + ".cs";
        string str = string.Empty;
        if (File.Exists(path))
        {
            str = File.ReadAllText(path);
        }
        string directory = Path.GetDirectoryName(path);

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        if (str != moudle)
        {
            Debug.LogError("change " + tableName + ".cs");
            File.WriteAllText(path, moudle);
        }
        else
        {
            //			Debug.LogError ("no change " + table.TableName + ".cs");
        }
        AssetDatabase.Refresh();
        return true;
    }




}
