using UnityEngine;
using System.Collections;
using LitJson;

public class EnemyTable : ExcelTypeItem  {
	public string enemyName;
	public override string StringIdentity(){ return enemyName; }
	public string hp;
	public int defense;
	public float moveSpeed;
	public string changeDestinationCd;
	public float rotateSpeed;
	public float minDisFromPlayer;
	public string moveSoundName;
	public string attackCd;
	public string bulletName;
	public string shootPointEffectName;

	public static string excelPath="Assets/StreamingAssets/Excel/FSM/EnemyTable.xlsx";
    public override void Setup(JsonData data) {
		base.Setup(data);
		enemyName = data["enemyName"].ToString();
		hp = data["hp"].ToString();
		defense = int.Parse(data["defense"].ToString());
		moveSpeed = float.Parse(data["moveSpeed"].ToString());
		changeDestinationCd = data["changeDestinationCd"].ToString();
		rotateSpeed = float.Parse(data["rotateSpeed"].ToString());
		minDisFromPlayer = float.Parse(data["minDisFromPlayer"].ToString());
		moveSoundName = data["moveSoundName"].ToString();
		attackCd = data["attackCd"].ToString();
		bulletName = data["bulletName"].ToString();
		shootPointEffectName = data["shootPointEffectName"].ToString();

    }

	public EnemyTable () {
	
	}
	public override string[] GetValueStrings()
	{
		return new string[]{enemyName.ToString(),hp.ToString(),defense.ToString(),moveSpeed.ToString(),changeDestinationCd.ToString(),rotateSpeed.ToString(),minDisFromPlayer.ToString(),moveSoundName.ToString(),attackCd.ToString(),bulletName.ToString(),shootPointEffectName.ToString(),};
	}
}

