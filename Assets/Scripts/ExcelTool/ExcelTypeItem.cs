using UnityEngine;
using LitJson;
using System.Collections.Generic;
using Excel;

public class ExcelTypeItem
{

	public ExcelTypeItem()
	{

	}

	public virtual void Setup(JsonData data)
	{

	}

	public virtual int Identity()
	{
		return 0;
	}

	public virtual int IndexIdentity()
	{
		return 0;
	}

	public virtual string StringIdentity()
	{
		return string.Empty;
	}


	public virtual string[] GetValueStrings()
	{
		return null;
	}

	//public void Write1Row2ExcelDate( ExcelTable table, int tableIndex, int rowNum,params string[] columnValues)
	//{
	//	//string excelPath = Application.dataPath + "/Test/Test.xlsx";
	//	//string outputPath = Application.dataPath + "/Test/Test2.xlsx";
	//	//Excel xls = ExcelHelper.LoadExcel(excelPath);

	//	for (int i = 0; i < columnValues.Length; i++)
	//	{
 //           table[rowNum, i]=columnValues[i];
	//	}
		
	//}
	//public static void SaveExcelData(ExcelTable table)
	//{
 //       table.Save();
 //   }



	public static List<string> GetAllRowJson(ExcelTable table)
	{
		List<string> value = new List<string>();
		//Debug.Log(table.NumberOfRows);
		if (table.rowNumDic.Count!=0)
		{
			for (int i = 3; i < table.GetCollumnsCount(); i++)
			{
				value.Add(Get1RowJson(i,table));
			}
		}
		else
		{
			
			Debug.LogError("表不足三行");
		}
		return value;

	}

	public static string Get1RowJson(int rowIndex, ExcelTable table)
	{
		//ExcelTable table = excel.Tables[0];
		System.Text.StringBuilder sb = new System.Text.StringBuilder();
		JsonWriter writer = new JsonWriter(sb);
		int i = rowIndex;
		string idStr = table[i, 1];
		writer.WriteObjectStart();
		
		for (int j = 1; j <= table.GetCollumnsCount(); j++)
		{
			int tableColumn = j;
			string propName = table[1,j];
			string propType = table[2, j];
			propName = propName.Replace("*", "");
			string currentPropName = propName;

			if (propName.StartsWith("#"))
			{
				continue;
			}
			if (string.IsNullOrEmpty(propName) || string.IsNullOrEmpty(propType))
			{
				continue;
			}
			writer.WritePropertyName(propName);
			string v = table[i, j];
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
		return sb.ToString();
	}
	public static string ExcelTable0ToJson(string path)
	{



		if (!path.EndsWith("xlsx"))
		{
			return null;
		}


		List<string> allClipStr = new List<string>();
		//string tableName = "";
		string currentPropName = "";
		int tableRow = 0;
		int tableColumn = 0;
		string v = "";
        ExcelTable aimTable=ExcelTool.LoadTable(path);

        try
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			JsonWriter writer = new JsonWriter(sb);
			writer.WriteObjectStart();			
			ExcelTable table = aimTable;

            //tableName = table.TableName;
            //bool language = tableName.ToLower().Contains("language");


            //writer.WritePropertyName(table.TableName + "s");
            writer.WriteArrayStart();
            for (int i = 3; i < table.GetRowsCount(); i++)
            {
                tableRow = i;
                string idStr = table[i, 1];
                if (idStr.Length <= 0)
                {
                    //						UnityEngine.Debug.LogError ("ID error:" + tableName + "  (第" + i + "行)");
                    break;
                }
                writer.WriteObjectStart();

				for (int j = 0; j < table.GetCollumnsCount(); j++)
				{
					tableColumn = j;
					string propName = table[1, j];
					string propType = table[3, j];
					//propName = propName.Replace("*", "");
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
			Debug.Log(sb.ToString());
			return sb.ToString();
		}
		catch (System.Exception e)
		{
			if (aimTable == null)
			{
				//                EditorUtility.DisplayDialog("ERROR!", "open excel failed!","ok"); 
				Debug.LogError("open excel failed!");
				Debug.LogError(e.StackTrace);
			}
			else
			{
				string msg = "解析错误！ \n表:" + path + " \n字段:" + currentPropName + "  \n第" + tableRow + "行,第" + tableColumn + "列 \nvalue = " + v;

				Debug.LogError(e);
				Debug.LogError(e.StackTrace);
				Debug.LogError(msg);
			}
			return null;
		}
	}


}


