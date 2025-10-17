using System.Collections;
using UnityEngine;


public class Ball_BlueBoots : Ball_PowerUp
{
    public override void Init(Vector3 startPosition)
    {
        base.Init(startPosition);

        object[] sprites = Resources.LoadAll<Sprite>("Sprites/Game Items/Balls");

        spriteRenderer.sprite = sprites[6] as Sprite;

        TMPE.SetTMPText("Slowed!");

        status = Status.StatusType.SpeedDrop;
    }

    public override void Recall()
    {
        recalled?.Invoke(this, 7);
    }
    public override void Despawn()
    {
        despawned?.Invoke(this, 7);
        base.Despawn();
    }
}
