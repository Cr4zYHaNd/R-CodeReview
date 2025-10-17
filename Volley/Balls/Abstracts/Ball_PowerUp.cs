using System.Collections;
using UnityEngine;

public abstract partial class Ball_PowerUp : Ball, IStatus
{
    protected override void OnLanding()
    {
        Despawn();
    }

    public override Rigidbody2D InitRigidbody()
    {
        Rigidbody2D rb = base.InitRigidbody();

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.gravityScale = .65f;

        return rb;
    }

    protected Status.StatusType status;
    public override void Set()
    {
    }

    public override void DizzySet()
    {

    }
    public override bool Spike(Vector2 dir, float launchPow)
    {
        return false;
    }

    public virtual void ConsumeMe(PlayerStatusHandler player)
    {
        if (!player.CheckStatus((int)status))
        {
            GiveStatus(player);
            TMPE.StartEmitter(player.transform.position);
            Despawn();
        }

    }

    public void GiveStatus(PlayerStatusHandler player)
    {
        player.ApplyStatus(status);
    }

    public override void Despawn()
    {
        trail.Stop();
        despawner = 4;
        Pause();
    }
}