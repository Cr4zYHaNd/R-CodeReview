using System.Collections;
using UnityEngine;


public class Ball_RedBoots : Ball_PowerUp
{

    public override void Init(Vector3 startPosition)
    {
        base.Init(startPosition);

        object[] sprites = Resources.LoadAll<Sprite>("Sprites/Game Items/Balls");

        spriteRenderer.sprite = sprites[7] as Sprite;

        TMPE.SetTMPText("Fast!");

        status = Status.StatusType.SpeedBoost;
    }

    public override void Recall()
    {
        recalled?.Invoke(this, 6);
    }
    public override void Despawn()
    {
        despawned?.Invoke(this, 6);
        base.Despawn();
    }

}
