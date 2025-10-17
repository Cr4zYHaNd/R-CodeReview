using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControllerAssignment : MonoBehaviour
{

    [SerializeField] private Transform mid, P1Panel, P2Panel;
    private PlayerInputManager input;
    List<string> names = new List<string>();
    List<InputControl> splicedDevices = new List<InputControl>();
    int readyPlayers = 0;
    PlayerInput P1, P2;

    // Start is called before the first frame update
    void Start()
    {

        input = GetComponent<PlayerInputManager>();
        input.onPlayerJoined += InitNewPlayer;
    }

    private void InitNewPlayer(PlayerInput input)
    {
        input.GetComponent<ControllerPickerIcon>().Init();
        TrySpliceKeyboard(input);
        FirstAssignPlayerToMid(input);
    }

    private void FirstAssignPlayerToMid(PlayerInput inp)
    {

        inp.transform.SetParent(mid);

        inp.GetComponent<ControllerPickerIcon>().moved += OnPlayerMove;
        inp.GetComponent<ControllerPickerIcon>().readied += ReadyTick;
    }

    private void TrySpliceKeyboard(PlayerInput inp)
    {
        if (inp.user.pairedDevices[0].device.description.deviceClass == "Keyboard")
        {

            if (!splicedDevices.Contains(inp.devices[0]))
            {
                splicedDevices.Add(inp.devices[0]);
                PlayerInput secondHalf = inp.GetComponent<ControllerPickerIcon>().Splice();
            }
        }

    }

    private void OnPlayerMove(int playerNum, ControllerPickerIcon control)
    {

        Transform destination = playerNum == 0 ? mid : playerNum == 1 ? P2Panel : P1Panel;


        if (destination != mid)
        {
            ControllerPickerIcon temp = destination.GetComponentInChildren<ControllerPickerIcon>();
            if (temp != null)
            {
                if (!temp.GetIsReady())
                {
                    temp.transform.SetParent(mid);
                    temp.GetComponent<ControllerPickerIcon>().Middled();
                    control.transform.SetParent(destination);
                }
                else
                {
                    control.MoveMeTo(0);


                }

            }
            else
            {


                control.transform.SetParent(destination);

            }

        }
        else
        {
            control.transform.SetParent(destination);
        }

    }
    private void ReadyTick(int a)
    {
        readyPlayers += a;
        if (readyPlayers == 2)
        {
            OnAllPlayersReadied();
        }
    }
    private void OnAllPlayersReadied()
    {
        P1 = P1Panel.GetComponentInChildren<PlayerInput>();
        P2 = P2Panel.GetComponentInChildren<PlayerInput>();

        P1Panel.transform.DetachChildren();
        P2Panel.transform.DetachChildren();

        DontDestroyOnLoad(P1.gameObject);
        DontDestroyOnLoad(P2.gameObject);

        this.GetComponent<PlayerInputManager>().DisableJoining();

        DontDestroyOnLoad(this);

        SceneManager.LoadScene("OnCourt", LoadSceneMode.Single);

        SceneManager.sceneLoaded += StartGame;

    }

    private void StartGame(Scene scene, LoadSceneMode LSM)
    {
        SceneManager.sceneLoaded -= StartGame;

        PlayerStart temp = null;

        int n = 0;

        while (temp == null)
        {

            temp = scene.GetRootGameObjects()[n].GetComponent<PlayerStart>();
            n++;

        }

        GameStateManager onCourt = new GameObject("Court").AddComponent<GameStateManager>();


        onCourt.Init(P1, P2, temp.GetStartPositions());

        SceneManager.MoveGameObjectToScene(this.gameObject, SceneManager.GetActiveScene());

        Destroy(this);

    }
}
