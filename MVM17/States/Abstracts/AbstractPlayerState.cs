using System;
using UnityEngine;

public abstract class AbstractPlayerState
{
    protected AnimationClip clip;
    protected CustomAnimationController anim;
    protected CharacterMovement movement;
    protected CharacterStateMachine stateMachine;
    protected CharacterSelect selector;
    protected PlayerHurtBehaviour hurtBehaviour;
    public Action<State> OnExit;
    protected string stateName;

    public AbstractPlayerState(string name)
    {
        stateName = name;
    }

    //Initialiser references relevant monobehaviour components
    public virtual void Init(CustomAnimationController _a, CharacterMovement _m, CharacterStateMachine _s, CharacterSelect _c, PlayerHurtBehaviour _h)
    {
        movement = _m;
        stateMachine = _s;
        selector = _c;
        anim = _a;
        hurtBehaviour = _h;

        BindStateAnimation(selector.CurrentCharacter.ToString());
    }

    //Behaviour to run when the state starts
    public abstract void OnStateEnter(PIA actions);

    //Behaviour to run when the state stops running
    public abstract void OnStateExit(PIA actions);

    public virtual void BindStateAnimation(string currentChar)
    {
        clip = Resources.Load<AnimationClip>("AnimationClips/Player/General/" + currentChar + "/" + stateName);
    }

}
