using System.Collections;
using UnityEngine;

// Explodes after a set duration
public class Ball_FuseBomb : Ball_TimedExplosive, IKOPotential
{
    public override void Init(Vector3 startPosition)
    {
        base.Init(startPosition);

        object[] sprites = Resources.LoadAll<Sprite>("Sprites/Game Items/Balls");

        spriteRenderer.sprite = sprites[12] as Sprite;

        TMPE.SetTMPText("BOOOM!");
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

    protected override void ExplosionHit(GameObject GO)
    {
        if (GO.TryGetComponent<PlayerKOHandler>(out PlayerKOHandler player))
        {
            ApplyKOP(player);
            Rigidbody2D prb2d = player.GetComponent<Rigidbody2D>();
        }


    }

    public void ApplyKOP(PlayerKOHandler player)
    {
        player.AddKOP(2);
    }

    public void StartIgnoring(PlayerKOHandler playerToIgnore)
    {
        return;
    }

    public IEnumerator IgnorePlayer(PlayerKOHandler playerToIgnore)
    {
        yield return null;
    }
}
