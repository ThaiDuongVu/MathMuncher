using System;
using UnityEngine;

public class Timer
{
    public readonly float MaxProgress;
    public float Progress;

    private readonly bool _isInfTimer;

    public Timer(float max)
    {
        MaxProgress = max;

        if (float.IsPositiveInfinity(MaxProgress)) _isInfTimer = true;
        else Progress = 0f;
    }

    public void Reset()
    {
        Progress = 0f;
    }

    public bool IsReachedUnscaled()
    {
        if (_isInfTimer) return false;

        Progress += Time.unscaledDeltaTime;
        return Progress >= MaxProgress;
    }

    public bool IsReached()
    {
        if (_isInfTimer) return false;

        Progress += Time.deltaTime;
        return Progress >= MaxProgress;
    }

    public override string ToString()
    {
        var total = Convert.ToInt32(Progress);
        var minute = total / 60;
        var second = total % 60;
        return $"{minute}:{(second < 10 ? "0" : "")}{second}";
    }
}