using System.Collections;
using UnityEngine;

public class Ball_Metronome : Ball_PowerUp
{

    public override void Init(Vector3 startPosition)
    {
        base.Init(startPosition);

        object[] sprites = Resources.LoadAll<Sprite>("Sprites/Game Items/Balls");

        spriteRenderer.sprite = sprites[3] as Sprite;

        TMPE.SetTMPText("Impetuous!");

        status = Status.StatusType.Hasted;
    }

    public override void Recall()
    {
        recalled?.Invoke(this, 3);
    }
    public override void Despawn()
    {
        despawned?.Invoke(this, 3);
        base.Despawn();
    }
}
