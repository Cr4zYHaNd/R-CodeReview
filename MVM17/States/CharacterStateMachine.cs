using System;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    Idle,
    Running,
    Turning,
    JumpSquat,
    Jumping,
    Falling,
    WallSlide,
    WallJump,
    WallCling,
    WallBound,
    Attacking,
    Hurt,
    Aerial,
    AirDash,
    Sliding
}

public class CharacterStateMachine : MonoBehaviour
{
    public Action onFixedUpdate, onUpdate;

    Dictionary<State, AbstractPlayerState> states = new();
    AbstractPlayerState currentState;
    PIA actions;

    public void Init(CustomAnimationController anim, CharacterMovement moves, CharacterSelect character, PlayerHurtBehaviour hurtBehaviour, PIA pia)
    {
        actions = pia;

        states.Add(State.Idle, new PS_Idle("Idle"));
        states.Add(State.Running, new PS_Running("Running"));
        states.Add(State.Turning, new PS_Turning("Turning"));
        states.Add(State.JumpSquat, new PS_Jumpsquat("Jumpsquat"));
        states.Add(State.Jumping, new PS_Jump("Jumping"));
        states.Add(State.Falling, new PS_Falling("Falling"));
        states.Add(State.WallSlide, new PS_WallSlide("WallSlide"));
        states.Add(State.WallJump, new PS_WallJump("WallJump"));
        states.Add(State.WallCling, new PS_WallCling("WallCling"));
        states.Add(State.WallBound, new PS_WallBound("WallBound"));
        states.Add(State.Attacking, new PS_Attacking("Attack"));
        states.Add(State.Hurt, new PS_Hurt("Hurt"));
        states.Add(State.Aerial, new PS_AirAttack("Aerial"));
        states.Add(State.AirDash, new PS_AirDash("AirDash"));
        states.Add(State.Sliding, new PS_SlideDodge("Slide"));

        foreach (KeyValuePair<State, AbstractPlayerState> pair in states)
        {
            pair.Value.Init(anim, moves, this, character, hurtBehaviour);
            pair.Value.OnExit += StateTransition;
        }

        StateTransition(State.Falling);

    }

    public void RebindStateAnimations(string name)
    {
        foreach (KeyValuePair<State, AbstractPlayerState> pair in states)
        {
            pair.Value.BindStateAnimation(name);

        }
    }

    private void StateTransition(State obj)
    {
        AbstractPlayerState targetState = states[obj];

        if (targetState == currentState) { return; }

        if (currentState != null)
        {
            currentState.OnStateExit(actions);
        }
        currentState = targetState;
        currentState.OnStateEnter(actions);

    }

    public void Respawn()
    {
        StateTransition(State.Falling);
    }

    void Update()
    {
        onUpdate?.Invoke();
    }

    private void FixedUpdate()
    {
        onFixedUpdate?.Invoke();

    }
}
