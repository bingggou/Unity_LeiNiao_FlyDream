using UnityEngine;
using System.Collections;
using LitJson;

public class table1 : ExcelTypeItem  {
	public string 测试字符串;
	public override string StringIdentity(){ return 测试字符串; }
	public int 测试整形;
	public float 测试浮点数;

	public static string excelPath="Assets/StreamingAssets/Test/CreateCS.xlsx";
    public override void Setup(JsonData data) {
		base.Setup(data);
		测试字符串 = data["测试字符串"].ToString();
		测试整形 = int.Parse(data["测试整形"].ToString());
		测试浮点数 = float.Parse(data["测试浮点数"].ToString());

    }

	public table1 () {
	
	}
	public override string[] GetValueStrings()
	{
		return new string[]{测试字符串.ToString(),测试整形.ToString(),测试浮点数.ToString()};
	}
}

