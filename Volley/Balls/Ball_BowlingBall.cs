using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball_BowlingBall : Ball_HeavyBall
{

    public override void Init(Vector3 startPosition)
    {
        base.Init(startPosition);

        ParticleSystem.MainModule main = trail.main;
        ParticleSystem.ShapeModule shape = trail.shape;
        object[] sprites = Resources.LoadAll<Sprite>("Sprites/Game Items/Balls");

        spriteRenderer.sprite = sprites[2] as Sprite;

        spriteRenderer.color = Random.ColorHSV();
        main.startColor = spriteRenderer.color;
        shape.radius = 0.2f;
        pointValue = 15;
        TMPE.SetTMPText("-15");
    }

    public override Rigidbody2D InitRigidbody()
    {
        Rigidbody2D rb = base.InitRigidbody();

        rb.sharedMaterial = Resources.Load<PhysicsMaterial2D>("Physics/Materials/PM_HeavyNonBounce");

        return rb;
    }

    public override void Recall()
    {
        recalled?.Invoke(this, 1);
    }

    public override void Despawn()
    {
        despawned?.Invoke(this, 1);
        base.Despawn();
    }

}
