using UnityEngine;
[CreateAssetMenu]
public class BucklerOverride : AIStateMachineOverride
{
    public override void Init()
    {
        overrides = new();
        overrides.Add(AIState.AGGRO, new AI_2DAggro("Aggro"));
        overrides.Add(AIState.ROAM, new AI_Flying("Flying"));
        overrides.Add(AIState.ATTACK, new AI_BucklerAttack("Attack"));
        overrides.Add(AIState.HURT, new AI_FlyingHurt("Hurt"));
        overrides.Add(AIState.IDLE, overrides[AIState.ROAM]);

    }
}

