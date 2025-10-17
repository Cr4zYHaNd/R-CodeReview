using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ControllerPickerIcon : MonoBehaviour
{
    private PlayerInput playerInp;
    private Image icon;
    private Text nameText;
    private int playerNum = 0;
    [SerializeField] private ControllerPickerIcon copy;
    public event Action<int, ControllerPickerIcon> moved;
    public event Action<int> readied;
    private bool spliced = false;
    private bool isReady = false;

    // Start is called before the first frame update
    public void Init()
    {
        playerInp = GetComponent<PlayerInput>();
        nameText = GetComponentInChildren<Text>();
        nameText.text = nameText.text == "<Controller Name>" ? playerInp.devices[0].displayName : nameText.text;
        InitSprite();
        gameObject.name = "User" + playerInp.user.index;
    }

    public void InitSprite()
    {
        icon = GetComponentInChildren<Image>();
        icon.sprite = playerInp.currentControlScheme switch
        {
            "Gamepad" => Resources.Load<Sprite>("UI/Images/Icons/GamePad"),
            "KeyboardLeft" => Resources.Load<Sprite>("UI/Images/Icons/WASD"),
            "KeyboardRight" => Resources.Load<Sprite>("UI/Images/Icons/ArrKeys"),
            _ => Resources.Load<Sprite>("UI/Images/Icons/Gamepad"),
        };
    }
    public void OnXInp(InputValue value)
    {
        if (!isReady)
        {
            int wasN = playerNum;
            if (value.Get<float>() != 0)
            {
                playerNum += value.Get<float>() > 0 ? 1 : -1;


                playerNum = playerNum < -1 ? -1 : playerNum > 1 ? 1 : playerNum;
                if (playerNum != wasN)
                {
                    moved?.Invoke(playerNum, this);
                }
            }
        }
    }

    public void OnReady()
    {
        if (playerNum != 0)
        {
            isReady = !isReady;
            readied?.Invoke(isReady ? 1 : -1);
        }

        icon.color = isReady ? Color.green : Color.white;
    }

    public PlayerInput Splice()
    {

        ControllerPickerIcon secondHalf = Instantiate(copy, transform.parent).GetComponent<ControllerPickerIcon>();

        secondHalf.SpliceTwo();

        GetComponentInChildren<Text>().text = "Left Keys";
        if (playerInp == null) { }
        {
            playerInp = GetComponent<PlayerInput>();
        }
        playerInp.SwitchCurrentControlScheme("KeyboardLeft", playerInp.devices[0]);
        icon.sprite = Resources.Load<Sprite>("UI/Images/Icons/WASD");


        spliced = true;

        return secondHalf.GetComponent<PlayerInput>();

    }

    public void SpliceTwo()
    {

        GetComponentInChildren<Text>().text = "Right Keys";

        playerInp.SwitchCurrentControlScheme("KeyboardRight", playerInp.devices[0]);
        icon.sprite = Resources.Load<Sprite>("UI/Images/Icons/ArrKeys");

        spliced = true;

    }

    public bool GetSpliced()
    {
        return spliced;
    }

    public void MoveMeTo(int dest)
    {
        playerNum = dest;
    }

    public bool GetIsReady()
    {
        return isReady;
    }

    public void Middled()
    {
        playerNum = 0;
    }
}
