using System;
using System.Collections;
using UnityEngine;

public class PlayerAppearance : MonoBehaviour
{

    private SpriteRenderer headRenderer, bHandRenderer, fHandRenderer, bodyRenderer;
    private Sprite[][] bodySheet, headSheet, bHandSheet, fHandSheet;
    public Action<int> ActiveFrame;
    private bool stateLock, handStateLock, lockFreeJump, hitStun;

    private enum AnimationState
    {

        Idle,
        Walking,
        Jumping,
        FallStart,
        FallLoop

    }

    private enum HandsState
    {
        Idle,
        Walking,
        Jumping,
        FallStart,
        FallLoop,
        Grab,
        Set,
        Spike
    }

    private AnimationState state;
    private HandsState handsState;

    private Coroutine animationLoop;

    public void Init(string animal, bool leftSide)
    {
        stateLock = false;
        handStateLock = false;
        lockFreeJump = false;
        hitStun = false;

        Sprite[] bodySprites = Resources.LoadAll<Sprite>("Sprites/Player/Body");
        Sprite[] headSprites = Resources.LoadAll<Sprite>("Sprites/Player/Head/" + animal);
        Sprite[] bHandSprites = Resources.LoadAll<Sprite>("Sprites/Player/Hands/Back");
        Sprite[] fHandSprites = Resources.LoadAll<Sprite>("Sprites/Player/Hands/Front");

        InitSpriteSheets(bodySprites, headSprites, bHandSprites, fHandSprites);

        headRenderer = InitHead();
        bHandRenderer = InitBackHand();
        fHandRenderer = InitForeHand();
        bodyRenderer = InitBody();

        ApplyColour(animal);

        headRenderer.flipX = !leftSide;
        bodyRenderer.flipX = !leftSide;
        bHandRenderer.flipX = !leftSide;
        fHandRenderer.flipX = !leftSide;

        state = AnimationState.Idle;
        handsState = HandsState.Idle;

        animationLoop = StartCoroutine(PlayAnimation());

    }

    public void OnLanded(float velocity)
    {
        stateLock = false;

        if (velocity == 0)
        {
            TryChangeState(0);
        }
        else
        {
            TryChangeState(1);
        }
    }

    private void ApplyColour(string animal)
    {

        Color myColor = animal switch
        {
            "Tiger" => new Color(1, 0.75f, 0.1f),
            "Elephant" => Color.grey,
            _ => Color.white,
        };

        bHandRenderer.color = myColor;
        fHandRenderer.color = myColor;
        bodyRenderer.color = myColor;

    }

    public void ApplyHitStun()
    {
        hitStun = true;
    }
    public void RemoveHitStun()
    {
        hitStun = false;
    }

    private IEnumerator PlayAnimation()
    {
        bool broken = false;
        int pointer = 0;
        int handPointer = 0;
        AnimationState was;
        HandsState wasHands;
        while (!broken)
        {
            if (hitStun)
            {
                yield return new WaitForSeconds(1f / 20f);
                continue;
            }

            was = state;
            wasHands = handsState;

            headRenderer.sprite = headSheet[((int)state)][pointer];
            bodyRenderer.sprite = bodySheet[((int)state)][pointer];
            bHandRenderer.sprite = bHandSheet[((int)handsState)][handPointer];
            fHandRenderer.sprite = fHandSheet[((int)handsState)][handPointer];

            HandleFrameLogic(pointer, handPointer );

            yield return new WaitForSeconds(1f / 20f);

            if (state != was)
            {
                pointer = 0;
            }
            else if (state != AnimationState.FallStart)
            {
                pointer = (pointer + 1) % headSheet[(int)state].Length;
            }
            else if (pointer + 1 == headSheet[(int)state].Length)
            {
                pointer = 0;
                state = AnimationState.FallLoop;

            }
            else
            {
                pointer++;
            }

            if (handsState != wasHands)
            {
                handPointer = 0;
            }
            else
            {
                handPointer++;
                if (handPointer == bHandSheet[(int)handsState].Length)
                {
                    handsState = (HandsState)state;
                    handPointer = pointer;
                }
            }
        }

    }

    private void HandleFrameLogic(int mainPointer, int handPointer)
    {

        switch (state)
        {
            case AnimationState.Idle:
            case AnimationState.Walking:
                break;
            case AnimationState.Jumping:
                switch (mainPointer)
                {
                    case 0:
                        stateLock = true;
                        ActiveFrame?.Invoke(0);
                        break;
                    case 4:
                        stateLock = false;
                        TryChangeState(3);
                        break;
                    default:
                        break;

                }
                break;
            case AnimationState.FallStart:
                switch (mainPointer)
                {
                    case 0:
                        stateLock = true;
                        break;
                    case 1:
                        stateLock = false;
                        TryChangeState(4);
                        stateLock = true;
                        break;
                    default:
                        break;

                }
                break;
            case AnimationState.FallLoop:
                break;

        }

        switch (handsState)
        {
            case HandsState.Idle:
            case HandsState.Walking:
            case HandsState.Jumping:
            case HandsState.FallStart:
            case HandsState.FallLoop:
                break;
            case HandsState.Spike:
                switch (handPointer)
                {
                    case 0:
                        handStateLock = true;
                        break;
                    case 1:
                        ActiveFrame?.Invoke(0);
                        break;
                    case 3:
                        ActiveFrame?.Invoke(1);
                        break;
                    case 8:
                        handStateLock = false;
                        break;
                    default:
                        break;
                }
                break;
            case HandsState.Set:

                switch (handPointer)
                {
                    case 0:
                        handStateLock = true;
                        break;
                    case 4:
                        ActiveFrame?.Invoke(0);
                        break;
                    case 6:
                        handStateLock = false;
                        break;
                    default:
                        break;
                }
                break;

            case HandsState.Grab:

                switch (handPointer)
                {
                    case 0:
                        handStateLock = true;
                        break;
                    case 3:
                        ActiveFrame?.Invoke(0);
                        break;
                    case 4:
                        handStateLock = false;
                        break;
                    default:
                        break;
                }
                break;
        }


    }

    public void ResetActiveFrameActions()
    {
        ActiveFrame = null;
    }

    private SpriteRenderer InitBody()
    {

        SpriteRenderer body = new GameObject("Body").AddComponent<SpriteRenderer>();
        body.sortingLayerID = 0;
        body.sprite = bodySheet[0][0];
        body.sortingOrder = 1;
        body.transform.SetParent(transform);
        body.transform.localPosition = Vector3.zero;

        return body;

    }

    private SpriteRenderer InitForeHand()
    {
        SpriteRenderer hand = new GameObject("Forehand").AddComponent<SpriteRenderer>();

        hand.sprite = fHandSheet[0][0];
        hand.sortingOrder = 3;
        hand.sortingLayerID = 0;
        hand.transform.SetParent(transform);
        hand.transform.localPosition = Vector3.zero;

        return hand;
    }
    private SpriteRenderer InitBackHand()
    {
        SpriteRenderer hand = new GameObject("Backhand").AddComponent<SpriteRenderer>();

        hand.sprite = bHandSheet[0][0];

        hand.sortingOrder = 0;

        hand.transform.SetParent(transform);
        hand.transform.localPosition = Vector3.zero;
        hand.sortingLayerID = 0;

        return hand;
    }

    private void InitSpriteSheets(Sprite[] body, Sprite[] head, Sprite[] bHand, Sprite[] fHand)
    {
        // Change to make - investigate sprite sheets/unused sprites in body and head sheets for hand dedicated clips - see if the arrays can be relieved of these unused sprites
        bodySheet = new Sprite[8][];
        bHandSheet = new Sprite[8][];
        fHandSheet = new Sprite[8][];
        headSheet = new Sprite[8][];

        int n = 0;
        int[] maxframes = { 6, 8, 5, 2, 2, 5, 7, 9 }; // Frame count of each animation, to add an animation, sheet array lengths ++, add frame count of animation to the end of this array

        for (int i = 0; i < 8; i++)
        {

            bodySheet[i] = new Sprite[maxframes[i]];
            headSheet[i] = new Sprite[maxframes[i]];
            bHandSheet[i] = new Sprite[maxframes[i]];
            fHandSheet[i] = new Sprite[maxframes[i]];

            for (int j = 0; j < bodySheet[i].Length; j++)
            {

                bodySheet[i][j] = body[n + j];
                bHandSheet[i][j] = bHand[n + j];
                fHandSheet[i][j] = fHand[n + j];
                headSheet[i][j] = head[n + j];

            }

            n += maxframes[i];

        }

    }

    private SpriteRenderer InitHead()
    {

        SpriteRenderer head = new GameObject("Head").AddComponent<SpriteRenderer>();
        head.sprite = headSheet[0][0];
        head.sortingOrder = 2;
        head.transform.SetParent(transform);
        head.transform.localPosition = Vector3.zero;
        head.sortingLayerID = 0;

        return head;

    }

    public bool TryChangeState(int stateNumber)
    {
        if (stateLock)
        {
            if(!((int)state > 1 && lockFreeJump))
            {
                return false;
            }
        }

        state = (AnimationState)stateNumber;
        if (!handStateLock)
        {
            handsState = (HandsState)state;
        }
        return true;

    }

    public bool TryChangeHandState(int stateNumber)
    {
        if (handStateLock)
        {
            return false;
        }
        else
        {
            handsState = (HandsState)stateNumber;
            return true;
        }

    }

    public void TempJumpToggle()
    {
        lockFreeJump = !lockFreeJump;
    }



}
