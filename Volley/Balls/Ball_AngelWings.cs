using System.Collections;
using UnityEngine;

public class Ball_AngelWings : Ball_PowerUp
{
    public override void Init(Vector3 startPosition)
    {
        base.Init(startPosition);

        object[] sprites = Resources.LoadAll<Sprite>("Sprites/Game Items/Balls");

        spriteRenderer.sprite = sprites[4] as Sprite;

        TMPE.SetTMPText("Bonus Jump!");

        status = Status.StatusType.ExtraJump;
    }

    public override void Recall()
    {
        recalled?.Invoke(this, 8);
    }
    public override void Despawn()
    {
        despawned?.Invoke(this, 8);
        base.Despawn();
    }
}