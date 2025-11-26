// ===== CoolTime.cs =====
using System.Collections;
using UnityEngine;

public class CoolTime
{
    private float startTime;
    private float duration;

    public void StartTimer(float t)
    {
        startTime = Time.time;
        duration = t;
    }

    public float GetTimer()
    {
        return Time.time - startTime;
    }

    public bool IsFinished()
    {
        return (Time.time - startTime) >= duration;
    }

    public float GetProgress()
    {
        float elapsed = Time.time - startTime;
        return Mathf.Clamp01(elapsed / duration);
    }
}