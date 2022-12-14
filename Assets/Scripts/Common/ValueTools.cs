
using UnityEngine;
[System.Serializable]
public class BottleValue<ValueType>
{
    public ValueType nowValue;
    public ValueType fullValue;
    public BottleValue(ValueType _nowValue, ValueType _fullValue)
    {
        nowValue = _nowValue;
        fullValue = _fullValue;
    }

    public void Reset()
    {
        nowValue = fullValue;
    }

    public virtual float GetPercent()
    {
        return -1;
    }

    public string GetExcelValueStr()
    {
        return nowValue.ToString() +"/"+ fullValue.ToString();
    }

}

[System.Serializable]
public class IntBottle : BottleValue<int>
{
    public IntBottle(int n,int f) : base(n,f) {  }
    public override float GetPercent()
    {
        return (float)nowValue / (float)fullValue;
    }
    public void OffectValue(int value)
    {
     
        int tempValue = nowValue + value;
    
      nowValue=  Mathf.Clamp(tempValue, 0,fullValue);
    }

}


[System.Serializable]
public class IntRange
{
    public int nowNum = 2;
    public int minNum = 1;
    public int maxNum = 5;
    public IntRange(int _min, int _max)
    {
        minNum = _min;
        maxNum = _max;
        SetNewNum();
    }

    public void Ini()
    {
        if (nowNum == 0)
        {
            nowNum = 6;
        }
    }

    public void SetNewNum()
    {
        nowNum = UnityEngine.Random.Range(minNum, maxNum+1);
    }

}