using System.Collections;
using UnityEngine;

public class Ball_GraceFeather : Ball_PowerUp
{
    public override void Init(Vector3 startPosition)
    {
        base.Init(startPosition);

        object[] sprites = Resources.LoadAll<Sprite>("Sprites/Game Items/Balls");

        spriteRenderer.sprite = sprites[5] as Sprite;

        status = Status.StatusType.FeatherFall;

        TMPE.SetTMPText("Light!");
        rb2d.gravityScale = 0.45f;

    }

    public override void Recall()
    {
        recalled?.Invoke(this, 4);
    }

    public override void Despawn()
    {
        despawned?.Invoke(this, 4);
        base.Despawn();
    }
}
