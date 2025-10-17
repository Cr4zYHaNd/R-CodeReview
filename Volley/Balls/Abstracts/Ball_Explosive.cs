using System.Collections;
using UnityEngine;

public abstract class Ball_Explosive : Ball
{
    protected float explosionRadius;

    protected virtual void Explode()
    {

        RaycastHit2D[] hits = Physics2D.CircleCastAll(rb2d.position, explosionRadius, Vector2.zero);

        foreach(RaycastHit2D hit in hits)
        {
            ExplosionHit(hit.transform.gameObject);
        }

        Despawn();
    }

    protected abstract void ExplosionHit(GameObject GO);

}
