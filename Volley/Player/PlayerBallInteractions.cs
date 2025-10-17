
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ball interactions component, handles grabbing, setting and spiking of balls
public class PlayerBallInteractions : MonoBehaviour
{
    private const float SPIKE_STUN_TIME = .1f;
    private Rigidbody2D RB2D;
    private float interactRange;
    private bool leftSide;
    public Action onHitstunned, onHitrelease;
    private List<Ball> stunnedBalls = new();
    private float power = 25;
    private bool grabbing;
    private bool dizzy;
    public Action<Vector2> grabRelease;

    public bool GetLeftSide()
    {
        return leftSide;
    }
    public void Init(Rigidbody2D myRB, int playerNum)
    {

        RB2D = myRB;
        interactRange = 2.5f;
        leftSide = playerNum == 1;
        grabbing = false;
    }

    public void OnDizzy()
    {
        dizzy = true;
    }

    public void OnSnapOut()
    {
        dizzy = false;
    }

    public void OnFaint()
    {
        if (grabbing)
        {
            DoGrab(0);
        }
    }

    public void DoSpike(int a)
    {
        if (!dizzy)
        {
            float mod = leftSide ? 1 : -1;

            RaycastHit2D[] hits = a == 0 ?
                Physics2D.CircleCastAll(transform.position, interactRange * 1.5f, Vector2.zero, 0) :
                Physics2D.CircleCastAll(transform.position + Vector3.left * mod * 0.25f, interactRange, Vector2.zero, 0);

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.transform.TryGetComponent<Ball>(out Ball ball))
                {
                    if (ball.TryGetComponent<IKOPotential>(out IKOPotential ko))
                    {
                        ko.StartIgnoring(this.GetComponent<PlayerKOHandler>());
                    }
                    ball.Pause();
                    stunnedBalls.Add(ball);
                }
            }
            if (hits.Length > 0)
            {
                StartCoroutine(ApplyHitStun(SPIKE_STUN_TIME));
                onHitrelease += PostSpikeHitstun;
            }
        }
    }

    private void PostSpikeHitstun()
    {
        Vector2 displacement;
        float sideModifier = leftSide ? 1 : -1;
        Vector2 spikeDirection;
        float powerModifier = 1;

        foreach (Ball ball in stunnedBalls)
        {
            ball.Play();
            displacement = RB2D.position - ball.GetComponent<Rigidbody2D>().position;

            float verticality = displacement.normalized.y * sideModifier;

            if (verticality > 0.35f)
            {
                spikeDirection = Vector2.Lerp(
                    new(sideModifier * 0.75f, 0.65f), //direction at .35 verticality
                    new(sideModifier * 0.35f, 0.95f), //direction at 1 verticality
                    (verticality - 0.35f) / 0.65f //how far along from .35 to 1
                    );
                powerModifier = 1;
            }
            else if (verticality > -0.85f)
            {
                spikeDirection = Vector2.Lerp(
                    new(sideModifier,0),
                    new(sideModifier * 0.75f, 0.65f),
                    (verticality + 0.85f) / 1.2f
                    );
                powerModifier = 1.25f;
            }
            else
            {
                spikeDirection = Vector2.up;
                powerModifier = 0.1f;
            }

            ball.Spike(spikeDirection, power * powerModifier);

        }
        stunnedBalls.Clear();
        onHitrelease -= PostSpikeHitstun;
    }

    public void DoGrab(int a)
    {
        if (grabbing)
        {

            grabRelease?.Invoke(RB2D.position);
            grabRelease = null;
            grabbing = false;

        }
        else
        {
            RaycastHit2D hit = Physics2D.CircleCast(RB2D.position, interactRange * 0.75f, Vector2.zero, 0);
            if (hit.transform.TryGetComponent<IGrabbable>(out IGrabbable grab))
            {
                Ball ball = hit.transform.GetComponent<Ball>();
                grabbing = true;

                grabRelease += ball.Released;

                grab.OnGrab(this);

            }
        }
    }

    public IEnumerator ApplyHitStun(float time)
    {
        onHitstunned?.Invoke();
        yield return new WaitForSeconds(time);
        onHitrelease?.Invoke();
    }

    public void DoSet(int a)
    {

        RaycastHit2D[] hits = Physics2D.CircleCastAll(RB2D.position, interactRange * 0.75f, Vector2.zero, 0);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform.TryGetComponent<Ball>(out Ball ball))
            {
                if (dizzy)
                {
                    ball.DizzySet();
                }
                else
                {
                    ball.Set();
                }

                if (ball.TryGetComponent<IKOPotential>(out IKOPotential ko))
                {
                    ko.StartIgnoring(this.GetComponent<PlayerKOHandler>());
                }
            }
        }

    }


}
