using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolTime
{
    public float cooltime;

    float coolCnt = 0;

    private void Start()
    {
        coolCnt = Time.time;
    }

    public float Timer(float t)
    {
        cooltime += Time.deltaTime;

        if(coolCnt + t <= Time.time)
        {
            coolCnt = Time.time;
            cooltime = 0;
        }
        return cooltime;
    }
}
