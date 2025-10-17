using UnityEngine;
using System;
public abstract class AbstractAIState
{
    protected AIHurtBehaviour pain;
    protected AIDetector detector;
    protected AIStateMachine stateMachine;
    protected AINavigator nav;
    protected CustomAnimationController anim;

    protected AnimationClip clip;
    public Action<AIState> onExit;

    protected string stateName;

    public AbstractAIState(string name)
    {
        stateName = name;
    }

    public virtual void Init(AINavigator _n, AIStateMachine _s, AIDetector _d, AIHurtBehaviour _h, CustomAnimationController _a)
    {
        pain = _h;
        detector = _d;
        stateMachine = _s;
        nav = _n;
        anim = _a;
    }
    public abstract void OnStateEnter();

    public abstract void OnStateExit();

    public virtual void BindAnimation(string animationName)
    {

        clip = Resources.Load<AnimationClip>("AnimationClips/Enemy/" + animationName + "/" + stateName);

    }
}
