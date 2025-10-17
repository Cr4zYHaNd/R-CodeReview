using System.Collections;
using UnityEngine;

// Explodes after hitting terrain enough times
public abstract class Ball_ContactExplosive : Ball_Explosive
{

    protected int maxContacts, contactsToBoom;

    public override void Set()
    {
        base.Set();
        ContactTick();
    }
    public override void DizzySet()
    {
        base.DizzySet();
        ContactTick();
    }

    public override bool Spike(Vector2 dir, float launchPow)
    {
        ContactTick();
        return base.Spike(dir, launchPow);

    }

    protected virtual void ContactTick()
    {
        contactsToBoom--;
        if (contactsToBoom == 0)
        {
            Explode();
        }
    }

    protected void ResetContactCount()
    {
        contactsToBoom = maxContacts;
    }

}
