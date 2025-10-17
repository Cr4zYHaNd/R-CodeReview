using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ball_HeavyBall : Ball, IKOPotential
{

    private List<PlayerKOHandler> ignores;
    public override void Init(Vector3 startPosition) { 
        
        ignores = new List<PlayerKOHandler>();
        base.Init(startPosition);
    }

    public virtual void ApplyKOP(PlayerKOHandler player)
    {
        if (!ignores.Contains(player))
        {
            if (rb2d.linearVelocity.y < 0 && !grounded)
            {
                player.AddKOP();
            }
        }
    }

    protected override void OnLanding()
    {
        base.OnLanding();

        rb2d.linearVelocity = Vector3.zero;

    }

    public void StartIgnoring(PlayerKOHandler playerToIgnore)
    {

        StartCoroutine(IgnorePlayer(playerToIgnore));

    }

    public IEnumerator IgnorePlayer(PlayerKOHandler playerToIgnore)
    {
        ignores.Add(playerToIgnore);
        yield return new WaitForSeconds(1);
        ignores.Remove(playerToIgnore);
    }

}
