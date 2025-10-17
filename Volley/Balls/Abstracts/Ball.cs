using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * Ball Base Class
 * This class performs all the standard operations that any ball would need. Abstract classes are the 
 * platonic representation of an entity, with no implementation specific cruft.
 * 
 * There are elements that bloat the legibility of this class because of my contemporary obsession
 * with scripting the initialisation of every property. I'm not sure if this is entirely a discredit
 * in the case of R* as while you made me aware that you text script down to the content creation
 * designers, I'm not sure if there are also details panels and other nice editor functionalities
 * that would render what I'm doing here a bit overkill.
 * 
 * Further abstract implementations of this class can be found in this directory. Concrete 
 * implementations can be found in the parent directory.
 * 
 * The only properties of this class that are not protected encapsulation are those of type 
 * System.Action which I use for my observer pattern.
 */

public abstract class Ball : MonoBehaviour
{

    protected Rigidbody2D rb2d;
    protected CircleCollider2D circ;
    protected SpriteRenderer spriteRenderer;
    protected bool grounded;
    protected ParticleSystem trail;

    protected float setCount = 0;
    protected float despawner;
    protected float maxSets = 3;
    protected float weight = 1;

    protected float setForce = 15.0f;

    protected int pointValue;
    public Action<Ball, int> despawned;
    public Action<Ball, int> recalled;
    protected TMPEmission TMPE;

    public virtual void Init(Vector3 startPosition)
    {
        gameObject.layer = 6;
        rb2d = InitRigidbody();
        trail = InitTrailSystem();
        Pause();
        circ = gameObject.AddComponent<CircleCollider2D>();
        circ.radius = 0.4f;
        spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sortingLayerName = "Balls";
        grounded = false;
        despawner = 4;

        transform.position = startPosition;

        TMPE = new GameObject("TMPE").AddComponent<TMPEmission>();
        TMPE.Init();
    }

    // Used to pull a ball from the Pool
    public virtual void Pull(Vector3 startPosition)
    {

        transform.position = startPosition;

    }

    public virtual ParticleSystem InitTrailSystem()
    {
        AnimationCurve animationCurve = new();

        animationCurve.AddKey(0f, 1f);
        animationCurve.AddKey(0.1f, 0.7646f);
        animationCurve.AddKey(0.2f, 0.5812f);
        animationCurve.AddKey(0.3f, 0.4314f);
        animationCurve.AddKey(0.4f, 0.3118f);
        animationCurve.AddKey(0.5f, 0.2134f);
        animationCurve.AddKey(0.6f, 0.137f);
        animationCurve.AddKey(0.7f, 0.078f);
        animationCurve.AddKey(0.8f, 0.0362f);
        animationCurve.AddKey(0.9f, 0.01f);
        animationCurve.AddKey(1f, 0f);

        ParticleSystem part = gameObject.AddComponent<ParticleSystem>();

        ParticleSystem.MainModule myMain = part.main;
        ParticleSystem.ShapeModule myShape = part.shape;
        ParticleSystem.EmissionModule myEmit = part.emission;
        ParticleSystem.SizeOverLifetimeModule myScaler = part.sizeOverLifetime;
        ParticleSystemRenderer rend = gameObject.GetComponent<ParticleSystemRenderer>();

        myMain.startLifetime = 0.8f;
        myMain.startSpeed = 0;
        myMain.startSize = 0.25f;
        myMain.startColor = UnityEngine.Random.ColorHSV();
        myMain.simulationSpace = ParticleSystemSimulationSpace.World;

        myShape.shapeType = ParticleSystemShapeType.Circle;
        myShape.radius = 0.5f;
        myShape.radiusThickness = 0;

        myEmit.rateOverTime = 25;

        myScaler.size = new ParticleSystem.MinMaxCurve(1, animationCurve);
        myScaler.enabled = true;

        rend.material = Resources.Load<Material>("Materials/ParticleMaterials/WhiteSquare");
        rend.sortingLayerName = "Particles";
        part.Play();
        return part;


    }
    public virtual Rigidbody2D InitRigidbody()
    {
        Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.simulated = true;
        rb.mass = 1;
        rb.linearDamping = 0;
        rb.angularDamping = 0.05f;
        rb.gravityScale = 1.25f;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.constraints = RigidbodyConstraints2D.None;

        return rb;
    }

    public virtual void Set()
    {

        if (setCount <= maxSets)
        {
            rb2d.linearVelocity = (1 - (setCount / maxSets)) * setForce * Vector2.up;
        }

        setCount++;
    }
    public virtual void DizzySet()
    {
        Vector2 upVec = new Vector2(UnityEngine.Random.Range(-0.25f, 0.25f), 1).normalized;

        if (setCount <= maxSets)
        {
            rb2d.linearVelocity = (1 - (setCount / maxSets)) * setForce * upVec;
        }

        setCount++;
    }

    public virtual bool Spike(Vector2 dir, float launchPow)
    {
        rb2d.linearVelocity = dir * launchPow / weight;

        setCount++;

        return false;
    }

    /* Setting the grounded/airborne state for the balls. Each time a ball is set without
     * having hit the ground, the set is a little weaker. Tracking the state of the ball
     * provides extra utility for other entities.
     */

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.otherCollider.gameObject.layer == LayerMask.NameToLayer("Terrain") || collision.collider.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        {
            grounded = true;
            OnLanding();
            ResetSetCount();

        }

    }

    protected virtual void OnLanding()
    {
    }

    protected void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.otherCollider.gameObject.layer == LayerMask.NameToLayer("Terrain") || collision.collider.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        {
            grounded = false;

        }
    }


    public void ResetSetCount()
    {

        setCount = 0;

    }

    //Game rule: All balls have a despawn fuse attached to them, which "despawns" them.

    protected void Update()
    {

        if (grounded)
        {
            despawner -= Time.deltaTime;
        }

        if (despawner < 0)
        {
            Despawn();
        }
        if (trail)
        {
            ParticleSystem.EmissionModule emit = trail.emission;
            ParticleSystem.MainModule main = trail.main;

            main.startLifetime = Mathf.Lerp(.4f, 1f, rb2d.linearVelocity.magnitude / 20f);
            emit.rateOverTime = Mathf.Lerp(25, 100, rb2d.linearVelocity.magnitude / 20f);
        }

    }
    public void IsGrabbed()
    {
        rb2d.position = new Vector2(-200, -200);
    }
    public void Released(Vector2 from)
    {
        rb2d.position = from;
        rb2d.linearVelocity = Vector2.zero;
        Play();
    }

    //
    public virtual void Despawn()
    {
        TMPE.StartEmitter(rb2d.position);
        trail.Stop();
        despawner = 4;
        Pause();
    }

    public abstract void Recall();

    public void Pause()
    {
        rb2d.bodyType = RigidbodyType2D.Static;
    }

    public void Play()
    {
        rb2d.bodyType = RigidbodyType2D.Dynamic;

        trail.Play();
    }

    public int GetPoints()
    {
        return pointValue;
    }

    public void ZeroVelocity()
    {
        rb2d.linearVelocity = Vector2.zero;
    }

}
