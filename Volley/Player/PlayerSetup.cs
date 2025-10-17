using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Script to initialise player characters and their components
public class PlayerSetup : MonoBehaviour
{
    public GameObject Init(int playerNum, Vector2 StartPosition, PlayerInput inputHandler)
    {

        Destroy(GetComponent<ControllerPickerIcon>());
        Destroy(GetComponent<LayoutElement>());
        Destroy(GetComponent<Image>());
        Destroy(GetComponent<CanvasRenderer>());

        SceneManager.MoveGameObjectToScene(this.gameObject, SceneManager.GetActiveScene());

        gameObject.layer = LayerMask.NameToLayer("Player");

        Rigidbody2D RB = InitRigidbody();

        CapsuleCollider2D cap = InitCollider();

        PlayerAppearance appearance = InitAppearance(playerNum);

        PlayerMovementControl moves = InitMovement(RB, cap);

        PlayerBallInteractions ballPlay = InitBallPlay(RB, playerNum);

        PlayerKOHandler KO = InitKOHandler();

        PlayerInputHandler input = InitInput(ballPlay, moves, KO, appearance, inputHandler);

        PlayerHurtBox hurtBox = new GameObject("Hurtbox").AddComponent<PlayerHurtBox>();

        PlayerStatusHandler status = InitStatus();

        moves.landed += appearance.OnLanded;

        ballPlay.onHitstunned += moves.ApplyHitStun;
        ballPlay.onHitstunned += appearance.ApplyHitStun;

        ballPlay.onHitrelease += moves.RemoveHitStun;
        ballPlay.onHitrelease += appearance.RemoveHitStun;

        status.Confused += moves.OnConfuse;
        status.SpeedBoost += moves.OnBoost;
        status.SpeedDrop += moves.OnSlowed;
        status.NoJump += moves.OnNoJump;
        status.NoJump += input.OnNoJump;
        status.ExtraJump += moves.TempJumpToggle;
        status.ExtraJump += appearance.TempJumpToggle;

        status.INSTAKO += KO.InstaKO;
        status.Featherfallen += moves.Featherfallen;

        status.Hasted += KO.HasteToggle;
        
        hurtBox.transform.SetParent(gameObject.transform);
        hurtBox.Init();

        transform.position = StartPosition;

        return gameObject;
    }

    private PlayerStatusHandler InitStatus()
    {
        PlayerStatusHandler status = gameObject.AddComponent<PlayerStatusHandler>();
        status.Init();
        return status;
    }

    private PlayerAppearance InitAppearance(int playerNum)
    {

        PlayerAppearance appearance = gameObject.AddComponent<PlayerAppearance>();
        appearance.Init(playerNum == 1 ? "Elephant" : "Tiger", playerNum == 1);

        return appearance;

    }

    private PlayerKOHandler InitKOHandler()
    {
        PlayerKOHandler KO = gameObject.AddComponent<PlayerKOHandler>();
        KO.Init();

        return KO;
    }

    private PlayerInputHandler InitInput(PlayerBallInteractions ballPlay, PlayerMovementControl movement, PlayerKOHandler ko, PlayerAppearance looks, PlayerInput inputHandler)
    {
        PlayerInputHandler input = gameObject.AddComponent<PlayerInputHandler>();
        input.Init(ballPlay, movement, ko, looks, inputHandler);

        return input;
    }

    private PlayerBallInteractions InitBallPlay(Rigidbody2D myRB, int myNum)
    {
        PlayerBallInteractions balls = gameObject.AddComponent<PlayerBallInteractions>();
        balls.Init(myRB, myNum);

        return balls;
    }

    private CapsuleCollider2D InitCollider()
    {
        CapsuleCollider2D capsule = gameObject.AddComponent<CapsuleCollider2D>();
        capsule.offset = new Vector2(0, -0.3125f);
        capsule.size = new Vector2(1, 1.375f);

        return capsule;
    }

    private PlayerMovementControl InitMovement(Rigidbody2D rb, CapsuleCollider2D capsule)
    {
        PlayerMovementControl move = gameObject.AddComponent<PlayerMovementControl>();
        move.Init(rb, capsule);

        return move;
    }

    private Rigidbody2D InitRigidbody()
    {
        Rigidbody2D rb2d = gameObject.AddComponent<Rigidbody2D>();

        rb2d.bodyType = RigidbodyType2D.Dynamic;

        rb2d.simulated = true;

        rb2d.mass = 1;
        rb2d.linearDamping = 0;
        rb2d.angularDamping = 0;
        rb2d.gravityScale = 0;

        rb2d.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;

        return rb2d;
    }

}
