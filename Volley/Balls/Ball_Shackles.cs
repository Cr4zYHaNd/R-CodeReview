using System.Collections;
using UnityEngine;


public class Ball_Shackles : Ball_PowerUp
{
    public override void Init(Vector3 startPosition)
    {
        base.Init(startPosition);

        object[] sprites = Resources.LoadAll<Sprite>("Sprites/Game Items/Balls");

        spriteRenderer.sprite = sprites[11] as Sprite;

        TMPE.SetTMPText("No Jump!");

        status = Status.StatusType.NoJump;
    }

    public override void Recall()
    {
        recalled?.Invoke(this, 5);
    }
    public override void Despawn()
    {
        despawned?.Invoke(this, 5);
        base.Despawn();
    }
}