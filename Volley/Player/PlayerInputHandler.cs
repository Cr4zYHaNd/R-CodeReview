using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{

    private InputAction IA_WalkMove;
    private InputActionAsset IAA_Player;
    private PlayerInput I_Player;
    private PlayerBallInteractions ballPlay;
    private PlayerMovementControl movement;
    private PlayerAppearance appearance;
    public void Init(PlayerBallInteractions myBallPlay, PlayerMovementControl myMovement, PlayerKOHandler ko, PlayerAppearance looks, PlayerInput myInputHandler)
    {
        I_Player = myInputHandler;
        IAA_Player = I_Player.actions;

        ballPlay = myBallPlay;
        movement = myMovement;
        appearance = looks;

        ko.fainted += OnFaint;
        ko.fainted += UnsubscribeToInput;

        ko.dizzied += SubscribeToInput;

        ko.snapOut += UnsubscribeToInput;

        ko.woken += FaintRecover;

        OnEnable();

    }
    private void OnEnable()
    {
        if (IAA_Player != null)
        {
            EnableGameplayActions();
        }
    }

    private void EnableGameplayActions()
    {
        IA_WalkMove = IAA_Player.FindAction("Walk");
        IA_WalkMove.started += BufferStartWalk;
        IA_WalkMove.canceled += BufferEndWalk;
        IA_WalkMove.Enable();

        IAA_Player.FindAction("Jump").performed += BufferJump;
        IAA_Player.FindAction("Jump").Enable();

        IAA_Player.FindAction("Grab").performed += BufferGrab;
        IAA_Player.FindAction("Grab").Enable();

        IAA_Player.FindAction("Spike").performed += BufferSpike;
        IAA_Player.FindAction("Spike").Enable();

        IAA_Player.FindAction("Set").performed += BufferSet;
        IAA_Player.FindAction("Set").Enable();

        IAA_Player.FindAction("Reset").performed += ResetGame;
        IAA_Player.FindAction("Reset").Enable();

    }

    private void ResetGame(InputAction.CallbackContext obj)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SC_ControllerAssignment", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    private void BufferStartWalk(InputAction.CallbackContext obj)
    {

        appearance.TryChangeState(1);

    }

    private void BufferEndWalk(InputAction.CallbackContext obj)
    {

        appearance.TryChangeState(0);

    }

    private void BufferJump(InputAction.CallbackContext inp)
    {

        if (appearance.TryChangeState(2))
        {
            appearance.ResetActiveFrameActions();
            appearance.ActiveFrame += movement.DoJump;
        }

    }
    private void BufferGrab(InputAction.CallbackContext inp)
    {
        if (appearance.TryChangeHandState(5))
        {
            appearance.ResetActiveFrameActions();
            appearance.ActiveFrame += ballPlay.DoGrab;
        }

    }
    private void BufferSet(InputAction.CallbackContext inp)
    {
        if (appearance.TryChangeHandState(6))
        {
            appearance.ResetActiveFrameActions();
            appearance.ActiveFrame += ballPlay.DoSet;
        }
    }
    private void BufferSpike(InputAction.CallbackContext inp)
    {

        if (appearance.TryChangeHandState(7))
        {
            appearance.ResetActiveFrameActions();
            appearance.ActiveFrame += ballPlay.DoSpike;
        }

    }

    private void OnDisable()
    {
        DisableGameplayActions();
    }

    private void DisableGameplayActions()
    {
        IA_WalkMove.Disable();
        IAA_Player.FindAction("Grab").Disable();
        IAA_Player.FindAction("Spike").Disable();
        IAA_Player.FindAction("Set").Disable();
        IAA_Player.FindAction("Jump").Disable();
    }

    private void OnFaint()
    {
        DisableGameplayActions();
        StartCoroutine(FaintTime());

    }

    private IEnumerator FaintTime()
    {
        yield return new WaitForSeconds(4);

        FaintRecover();
    }

    public void OnNoJump()
    {
        if (IAA_Player.FindAction("Jump").enabled)
        {
            IAA_Player.FindAction("Jump").Disable();
            return;
        }
        IAA_Player.FindAction("Jump").Enable();
    }

    private void StunMe()
    {
        DisableGameplayActions();
        StartCoroutine(StunTime(1.0f));

    }

    private IEnumerator StunTime(float timeToStun)
    {

        yield return new WaitForSeconds(timeToStun);

        SnapOutOfStun();

    }

    private void SnapOutOfStun()
    {

        EnableGameplayActions();

    }

    private void SubscribeToInput()
    {
        IA_WalkMove.performed += OnActed;
        IAA_Player.FindAction("Grab").performed += OnActed;
        IAA_Player.FindAction("Spike").performed += OnActed;
        IAA_Player.FindAction("Set").performed += OnActed;
        IAA_Player.FindAction("Jump").performed += OnActed;
    }
    private void UnsubscribeToInput()
    {
        IA_WalkMove.performed -= OnActed;
        IAA_Player.FindAction("Grab").performed -= OnActed;
        IAA_Player.FindAction("Spike").performed -= OnActed;
        IAA_Player.FindAction("Set").performed -= OnActed;
        IAA_Player.FindAction("Jump").performed -= OnActed;
    }

    private void OnActed(InputAction.CallbackContext obj)
    {

        gameObject.GetComponent<PlayerKOHandler>().ActEarly();

    }

    private void FaintRecover()
    {
        EnableGameplayActions();

    }

    // Update is called once per frame
    void Update()
    {

        movement.SetWalkInput(IA_WalkMove.ReadValue<float>());

    }
}
