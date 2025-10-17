using System.Collections;
using UnityEngine;
public class Ball_Piano : Ball_HeavyBall
{

    // Use this for initialization
    public override void Init(Vector3 startPosition)
    {
        base.Init(startPosition);
        
        object[] sprites = Resources.LoadAll<Sprite>("Sprites/Game Items/Balls");

        spriteRenderer.sprite = sprites[0] as Sprite;

        circ.radius = 1.2f;

        pointValue = 25;
        TMPE.SetTMPText("-25");

        ParticleSystem.ShapeModule shape = trail.shape;
        shape.radius = 0.45f;

    }

    public override Rigidbody2D InitRigidbody()
    {
        Rigidbody2D rb = base.InitRigidbody();

        rb.sharedMaterial = Resources.Load<PhysicsMaterial2D>("Physics/Materials/PM_HeavyNonBounce");

        return rb;
    }

    public override void Recall()
    {
        recalled?.Invoke(this, 2);
    }
    public override void Despawn()
    {
        despawned?.Invoke(this, 2);
        base.Despawn();
    }
}