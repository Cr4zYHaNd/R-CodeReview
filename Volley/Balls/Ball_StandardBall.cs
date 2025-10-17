using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball_StandardBall : Ball, IGrabbable
{
    public void OnGrab(PlayerBallInteractions player)
    {
        rb2d.position = new Vector2(-300f, player.GetLeftSide() ? 200f : -200f);

    }
    public override void Init(Vector3 startPosition)
    {
        base.Init(startPosition);

        object[] sprites = Resources.LoadAll<Sprite>("Sprites/Game Items/Balls");

        spriteRenderer.sprite = sprites[1] as Sprite;

        pointValue = 10;
        TMPE.SetTMPText("-10");

        despawner = 0.2f;

    }

    public override Rigidbody2D InitRigidbody()
    {
        Rigidbody2D rb = base.InitRigidbody();

        rb.sharedMaterial = Resources.Load<PhysicsMaterial2D>("Physics/Materials/PM_StandardBall");

        return rb;
    }

    public void ReleaseMe()
    {
        throw new System.NotImplementedException();
    }

    public override void Recall()
    {
        recalled?.Invoke(this, 0);
    }
    public override void Despawn()
    {
        despawned?.Invoke(this, 0);
        base.Despawn();
    }

}
