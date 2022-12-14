using UnityEngine;

[System.Serializable]
public class ColdTimeMachine
{

    public float cdTime = 10;
    private float passTime = 0;


    public bool PassingAuto()
    {
        passTime += Time.deltaTime;
        if (passTime >= cdTime)
        {
            Reset();
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool PassingManual()
    {
        passTime += Time.deltaTime;
        if (passTime >= cdTime)
        {

            return true;
        }
        else
        {
            return false;
        }
    }


    public ColdTimeMachine(float _cdTime)
    {
        cdTime = _cdTime;
    }

    public void ChangeCdTime(float _cdTime)
    {
        cdTime = _cdTime;
    }

    public void Reset()
    {
        passTime = 0;

    }



}
[System.Serializable]
public class TimeRange
{
    public float nowTime = 2;
    public float minTime = 1;
    public float maxTime = 5;

    public string GetExcelValueStr()
    {
        return nowTime.ToString() + "*" + minTime.ToString() + "*" + maxTime.ToString();
    }

    public TimeRange(float _min, float _max)
    {
        minTime = _min;
        maxTime = _max;
        SetNewTime();
    }

    public void Ini()
    {
        if (nowTime == 0)
        {
            nowTime = 6;
        }
    }

    public void SetNewTime()
    {
        nowTime = UnityEngine.Random.Range(minTime, maxTime);
    }

}


