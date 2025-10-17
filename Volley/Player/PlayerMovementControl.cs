using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementControl : MonoBehaviour
{
    private float mSpeed;
    private float mAirSpeed;
    private float gravScale;
    private float yVel;
    private float xVel;
    private float jumpForce;
    private bool grounded;

    private Rigidbody2D rb2d;

    public Action<float> landed;

    private CapsuleCollider2D capsule;
    private float walkInp;
    private bool boosted, slowed, jumpOn, lightened;
    private int maxJumps, jumpCount;
    private bool hitStun;

    public void Init(Rigidbody2D myRB2D, CapsuleCollider2D myCap)
    {
        hitStun = false;
        rb2d = myRB2D;
        capsule = myCap;

        mSpeed = 7.5f;
        mAirSpeed = 4.5f;
        gravScale = 40;
        jumpForce = 25;

        walkInp = 0;
        yVel = 0;
        xVel = 0;

        grounded = false;

        boosted = false;
        slowed = false;
        jumpOn = true;
        lightened = false;
        maxJumps = 1;
        jumpCount = 1;

    }

    public void OnConfuse()
    {

        mSpeed *= -1;
        mAirSpeed *= -1;

    }

    public void OnDizzy()
    {

        mSpeed *= 0.75f;
        mAirSpeed *= 0.5f;
        jumpForce *= 0.5f;

    }

    public void OnSnapOut()
    {

        mSpeed /= 0.75f;
        mAirSpeed *= 2;
        jumpForce *= 2;

    }

    public void TempJumpToggle()
    {
        if (maxJumps == 1)
        {
            maxJumps++;
            jumpCount++;
            jumpForce = 20;
        }
        else
        {
            maxJumps--;
            jumpCount = Mathf.Max(jumpCount - 1, 0);
            jumpForce = 25;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!grounded)
        {
            bool wasGrounded = grounded;

            if (collision.otherCollider.gameObject.layer == LayerMask.NameToLayer("Terrain") || collision.collider.gameObject.layer == LayerMask.NameToLayer("Terrain"))
            {
                if (yVel < 0)
                {
                    yVel = 0;
                    grounded = true;
                    landed?.Invoke(xVel);
                    jumpCount = maxJumps;
                }
            }
        }
    }

    public void Featherfallen()
    {
        lightened = !lightened;
        if (lightened)
        {
            jumpForce *= .5f;
            gravScale *= .25f;
        }
        else
        {
            jumpForce *= 2f;
            gravScale *= 4f;
        }

    }

    public void OnBoost()
    {
        boosted = !boosted;
        if (boosted)
        {
            mSpeed *= 2;
            mAirSpeed *= 2;
        }
        else
        {
            mSpeed *= 0.5f;
            mAirSpeed *= 0.5f;
        }
    }
    public void OnSlowed()
    {
        slowed = !slowed;
        if (!slowed)
        {
            mSpeed *= 2;
            mAirSpeed *= 2;
        }
        else
        {
            mSpeed *= 0.5f;
            mAirSpeed *= 0.5f;
        }
    }
    public void OnNoJump()
    {
        jumpOn = !jumpOn;
        if (!jumpOn && jumpCount > 0)
        {
            yVel = Mathf.Min(0, yVel);
        }
    }

    public void DoJump(int a)
    {
        jumpCount--;
        yVel = jumpForce;
        grounded = false;
        return;
    }

    public void ApplyHitStun()
    {
        hitStun = true;
    }

    public void RemoveHitStun()
    {
        hitStun = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (hitStun)
        {
            rb2d.linearVelocity = Vector2.zero;
            return;
        }
        if (!grounded)
        {

            xVel = mAirSpeed * walkInp;
            yVel -= gravScale * Time.fixedDeltaTime;

        }
        else
        {

            xVel = mSpeed * walkInp;

        }

        rb2d.linearVelocity = new Vector2(xVel, yVel);

    }

    public void SetWalkInput(float input)
    {
        walkInp = input;
    }

    public void OnReload()
    {

        grounded = false;
        walkInp = 0;
        xVel = 0;
        yVel = 0;

    }

    private void OnDisable()
    {
        xVel = 0;
        yVel = 0;
        rb2d.linearVelocity = Vector2.zero;
    }

}
