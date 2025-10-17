using System.Collections;
using UnityEngine;

public class PlayerHurtBox : MonoBehaviour
{
    CapsuleCollider2D capsule;
    // Use this for initialization
    public void Init()
    {
        gameObject.layer = LayerMask.NameToLayer("Hurtbox");
        capsule = gameObject.AddComponent<CapsuleCollider2D>();
        transform.localPosition = Vector2.zero;
        capsule.offset = Vector2.zero;
        capsule.size = new Vector2(1, 2);
        capsule.isTrigger = true;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<IKOPotential>(out IKOPotential ball))
        {
            ball.ApplyKOP(GetComponentInParent<PlayerKOHandler>());
        } else if(collision.TryGetComponent<Ball_PowerUp>(out Ball_PowerUp powerUp))
        {
            GetComponentInParent<PlayerStatusHandler>().Consume(powerUp);
        }
    }


}
