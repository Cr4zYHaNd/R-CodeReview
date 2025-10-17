using System.Collections;
using UnityEngine;


public class Ball_StunGrenade : Ball_ContactExplosive, IStatus
{
    private Status.StatusType statusType;
    public override void Init(Vector3 startPosition)
    {
        base.Init(startPosition);


        object[] sprites = Resources.LoadAll<Sprite>("Sprites/Game Items/Balls");
        circ.radius = 0.2f;
        spriteRenderer.sprite = sprites[10] as Sprite;
        statusType = Status.StatusType.Confused;
        explosionRadius = 2.5f;
        TMPE.SetTMPText("BOOM!");
        pointValue = 0;
        maxContacts = 1;

    }

    public void GiveStatus(PlayerStatusHandler player)
    {
        if (!player.CheckStatus((int)statusType))
        {
            player.ApplyStatus(statusType);
        }

    }

    public override void Recall()
    {
        recalled?.Invoke(this, 9);
        ResetContactCount();
    }
    public override void Despawn()
    {
        despawned?.Invoke(this, 9);
        ResetContactCount();
        base.Despawn();
    }

    protected override void ExplosionHit(GameObject GO)
    {
        if (GO.TryGetComponent<PlayerStatusHandler>(out PlayerStatusHandler player))
        {
            GiveStatus(player);
        }
    }
}

