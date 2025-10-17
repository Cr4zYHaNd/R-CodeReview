using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

//Initialiser for KOBar
public class KOBarSetup : MonoBehaviour
{

    private RectTransform rect;

    private Image[] barrel = new Image[3];
    private Image[] fill = new Image[3];

    // Use this for initialization
    public KOBar Init(int playerNum)
    {

        rect = GetComponent<RectTransform>();

        rect.sizeDelta = new Vector2(200, 100);

        rect.anchorMin = playerNum == 1 ? new Vector2(0, 0) : new Vector2(1, 0);
        rect.anchorMax = playerNum == 1 ? new Vector2(0, 0) : new Vector2(1, 0);
        rect.pivot = playerNum == 1 ? new Vector2(0, 0) : new Vector2(1, 0);

        rect.anchoredPosition = playerNum == 1 ? new Vector2(300, 25) : new Vector2(-300, 25);

        InitBarrel(playerNum);
        InitFill(playerNum);

        Image word = InitIcon(playerNum);

        KOBar kobar = gameObject.AddComponent<KOBar>();
        kobar.Init(barrel, fill, word);

        return kobar;

    }

    private Image InitIcon(int playerNum)
    {

        Image word = new GameObject("KO!").AddComponent<Image>();

        word.transform.SetParent(rect);

        word.sprite = Resources.Load<Sprite>("UI/Images/KO" + (playerNum == 1 ? "Left" : "Right") + "Word");

        word.rectTransform.anchorMin = new Vector2(0, 0);
        word.rectTransform.anchorMax = new Vector2(1, 1);
        word.rectTransform.offsetMax = new Vector2(0, 0);
        word.rectTransform.offsetMin = new Vector2(0, 0);

        word.enabled = true;

        return word;

    }

    private void InitFill(int playerNum)
    {
        for (int i = 0; i < fill.Length; i++)
        {

            fill[i] = new GameObject("KO Fill " + (i + 1)).AddComponent<Image>();
            fill[i].transform.SetParent(rect);

            fill[i].sprite = Resources.Load<Sprite>("UI/Images/KOLeft" + (i + 1) + "Fill");

            fill[i].rectTransform.anchorMin = new Vector2(0, 0);
            fill[i].rectTransform.anchorMax = new Vector2(1, 1);
            fill[i].rectTransform.offsetMax = new Vector2(0, 0);
            fill[i].rectTransform.offsetMin = new Vector2(0, 0);

            fill[i].enabled = false;

            fill[i].rectTransform.localScale = new Vector2(playerNum == 1 ? 1 : -1, 1);
        }

    }

    private void InitBarrel(int playerNum)
    {

        for (int i = 0; i < barrel.Length; i++)
        {

            barrel[i] = new GameObject("KO Barrel " + (i + 1)).AddComponent<Image>();
            barrel[i].transform.SetParent(rect);

            barrel[i].sprite = Resources.Load<Sprite>("UI/Images/KOLeft" + (i + 1) + "Barrel");

            barrel[i].rectTransform.anchorMin = new Vector2(0, 0);
            barrel[i].rectTransform.anchorMax = new Vector2(1, 1);
            barrel[i].rectTransform.offsetMax = new Vector2(0, 0);
            barrel[i].rectTransform.offsetMin = new Vector2(0, 0);

            barrel[i].color = Color.black;

            barrel[i].enabled = true;

            barrel[i].rectTransform.localScale = new Vector2(playerNum == 1 ? 1 : -1, 1);

        }
    }

}
