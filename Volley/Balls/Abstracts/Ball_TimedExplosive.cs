using System.Collections;
using UnityEngine;

public abstract class Ball_TimedExplosive : Ball_Explosive
{

    protected float maxTimer, timer;

    protected void ResetTimer()
    {
        timer = maxTimer;
    }

    protected void TickTimer(out float alpha)
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            Explode();
        }
        alpha = 1 - (timer / maxTimer);
    }
}