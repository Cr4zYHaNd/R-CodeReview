using System.Collections;
using UnityEngine;

public class Ball_HolyBall : Ball_PowerUp
{

    public override void Init(Vector3 startPosition)
    {
        base.Init(startPosition);

        object[] sprites = Resources.LoadAll<Sprite>("Sprites/Game Items/Balls");

        spriteRenderer.sprite = sprites[9] as Sprite;
        circ.radius = 0.25f;
        TMPE.SetTMPText("BLESSED!");

        status = Status.StatusType.Blessed;
    }

    public override void Recall()
    {
        recalled?.Invoke(this, 11);
    }
    public override void Despawn()
    {
        despawned?.Invoke(this, 11);
        base.Despawn();
    }



}
