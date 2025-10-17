using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class OnCourtUIManager : MonoBehaviour
{
    private Canvas canvas;
    private TextMeshProUGUI timerText, p1Score, p2Score;
    private Canvas EndGameScreen;
    private KOBar p1KO, p2KO;
    private float timerTime;
    private bool timeGo;
    public Action restarted, menued;

    public void Init()
    {
        EventSystem ES = new GameObject("EventSystem").AddComponent<EventSystem>();
        canvas = InitCanvas();

        InputSystemUIInputModule inputMod = ES.gameObject.AddComponent<InputSystemUIInputModule>();

        //Initialize input module properties
        inputMod.moveRepeatDelay = 0.5f;
        inputMod.moveRepeatRate = 0.1f;
        inputMod.deselectOnBackgroundClick = true;
        inputMod.pointerBehavior = UIPointerBehavior.SingleUnifiedPointer;

        //Bind input actions to module actions
        inputMod.actionsAsset = Resources.Load<InputActionAsset>("Input/PlayerInput/PlayerInputActions");

        inputMod.move = InputActionReference.Create(inputMod.actionsAsset.FindAction("Move"));
        inputMod.submit = InputActionReference.Create(inputMod.actionsAsset.FindAction("Accept"));
        inputMod.cancel = InputActionReference.Create(inputMod.actionsAsset.FindAction("Cancel"));

        //Initialize End game screen and hide it
        EndGameScreen = MakeEGS();

        //Initialise clock
        InitTimerPanel(canvas);

        //Initialise score boards
        p1Score = InitScorePanel(1);
        p2Score = InitScorePanel(2);

        //Initialize KO Meters
        p1KO = InitKOBar(1);
        p2KO = InitKOBar(2);

        timeGo = false;

        //Start the clock! - [/[/[/[move to game start in player manager eventually]\]\]\]
        StartClock(180f);

    }

    //End game screen init
    private Canvas MakeEGS()
    {

        Canvas EGS = new GameObject("EGS Canvas").AddComponent<Canvas>();
        //Init Canvas properties
        EGS.renderMode = RenderMode.ScreenSpaceOverlay;
        EGS.pixelPerfect = false;
        EGS.sortingOrder = 0;
        EGS.targetDisplay = 0;

        //Init Canvas Scaler
        CanvasScaler CS = EGS.gameObject.AddComponent<CanvasScaler>();

        CS.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        CS.referenceResolution = new Vector2(1920, 1080);
        CS.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        CS.matchWidthOrHeight = 0;
        CS.referencePixelsPerUnit = 100;

        //Init Graphicraycaster
        GraphicRaycaster GR = EGS.gameObject.AddComponent<GraphicRaycaster>();
        GR.ignoreReversedGraphics = true; ;
        GR.blockingObjects = GraphicRaycaster.BlockingObjects.None;
        GR.blockingMask = ~0;

        TextMeshProUGUI winnerText = new GameObject("Winner Text").AddComponent<TextMeshProUGUI>();

        winnerText.transform.SetParent(EGS.transform);
        winnerText.font = Resources.Load<TMP_FontAsset>("UI/Fonts/TMP Fonts/04B_30__ SDF");
        winnerText.text = "X WINS!";
        winnerText.enableAutoSizing = true;
        winnerText.fontSizeMax = 200;
        winnerText.alignment = TextAlignmentOptions.Center;
        winnerText.rectTransform.anchorMin = new Vector2(0, 1);
        winnerText.rectTransform.anchorMax = new Vector2(1, 1);
        winnerText.rectTransform.sizeDelta = new Vector2(0, 400);
        winnerText.rectTransform.pivot = new Vector2(0.5f, 1);
        winnerText.rectTransform.localPosition = new Vector3(0, 540, 0);

        Image RematchPanel = new GameObject("RematchPanel").AddComponent<Image>();
        RematchPanel.transform.SetParent(EGS.transform);
        RematchPanel.sprite = Resources.Load<Sprite>("UI/Images/InnerGlowPanelSmall");
        RematchPanel.color = new Color(1, 1, 1, 150.0f / 255.0f);

        RematchPanel.type = Image.Type.Sliced;

        RematchPanel.rectTransform.sizeDelta = new Vector2(900, 250);
        RematchPanel.rectTransform.pivot = new Vector2(0.5f, 0);

        RematchPanel.rectTransform.anchorMin = new Vector2(0.5f, 0);
        RematchPanel.rectTransform.anchorMax = new Vector2(0.5f, 0);

        RematchPanel.rectTransform.anchoredPosition = new Vector2(0, 375);

        TextMeshProUGUI rematchText = new GameObject("RematchText").AddComponent<TextMeshProUGUI>();
        rematchText.transform.SetParent(RematchPanel.transform);
        rematchText.font = Resources.Load<TMP_FontAsset>("UI/Fonts/TMP Fonts/PublicPixel SDF");
        rematchText.text = "Rematch";
        rematchText.enableAutoSizing = true;
        rematchText.fontSizeMax = 75;
        rematchText.alignment = TextAlignmentOptions.Center;
        rematchText.rectTransform.anchorMin = Vector2.zero;
        rematchText.rectTransform.anchorMax = Vector2.one;
        rematchText.rectTransform.offsetMin = Vector2.zero;
        rematchText.rectTransform.offsetMax = Vector2.zero;


        Image MenuPanel = new GameObject("MenuPanel").AddComponent<Image>();
        MenuPanel.transform.SetParent(EGS.transform);
        MenuPanel.sprite = Resources.Load<Sprite>("UI/Images/InnerGlowPanelSmall");
        MenuPanel.color = new Color(1, 1, 1, 150.0f / 255.0f);

        MenuPanel.type = Image.Type.Sliced;

        MenuPanel.rectTransform.sizeDelta = new Vector2(900, 250);
        MenuPanel.rectTransform.pivot = new Vector2(0.5f, 0);

        MenuPanel.rectTransform.anchorMin = new Vector2(0.5f, 0);
        MenuPanel.rectTransform.anchorMax = new Vector2(0.5f, 0);

        MenuPanel.rectTransform.anchoredPosition = new Vector2(0, 50);

        TextMeshProUGUI MenuText = new GameObject("RematchText").AddComponent<TextMeshProUGUI>();
        MenuText.transform.SetParent(MenuPanel.transform);
        MenuText.font = Resources.Load<TMP_FontAsset>("UI/Fonts/TMP Fonts/PublicPixel SDF");
        MenuText.text = "Back to Menu";
        MenuText.enableAutoSizing = true;
        MenuText.fontSizeMax = 75;
        MenuText.alignment = TextAlignmentOptions.Center;
        MenuText.rectTransform.anchorMin = Vector2.zero;
        MenuText.rectTransform.anchorMax = Vector2.one;
        MenuText.rectTransform.offsetMin = Vector2.zero;
        MenuText.rectTransform.offsetMax = Vector2.zero;
        EGS.enabled = false;

        UINavButton RNav = RematchPanel.gameObject.AddComponent<UINavButton>();
        UINavButton MNav = MenuPanel.gameObject.AddComponent<UINavButton>();

        RNav.Init(Color.yellow, Color.white, new UINavButton.NB_Neighbours { above = MNav, below = MNav });
        MNav.Init(Color.yellow, Color.white, new UINavButton.NB_Neighbours { above = RNav, below = RNav });

        RNav.pressed += RematchSubmitted;
        MNav.pressed += ReturnToMenu;

        return EGS;
    }

    private void RematchSubmitted()
    {
        restarted?.Invoke();
    }

    private void ReturnToMenu()
    {
        menued?.Invoke();
    }

    private Canvas InitCanvas()
    {

        Canvas canvas = new GameObject("Canvas").AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.pixelPerfect = false;
        canvas.sortingOrder = 0;
        canvas.targetDisplay = 0;

        CanvasScaler CS = canvas.gameObject.AddComponent<CanvasScaler>();
        CS.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        CS.referenceResolution = new Vector2(1920, 1080);
        CS.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        CS.matchWidthOrHeight = 0;
        CS.referencePixelsPerUnit = 100;

        GraphicRaycaster GR = canvas.gameObject.AddComponent<GraphicRaycaster>();
        GR.ignoreReversedGraphics = true; ;
        GR.blockingObjects = GraphicRaycaster.BlockingObjects.None;
        GR.blockingMask = ~0;

        return canvas;

    }

    public void ShowGameEndMenu(int winnerPlaynum)
    {

        EndGameScreen.GetComponentInChildren<TextMeshProUGUI>().text = "PLAYER " + winnerPlaynum + " WINS!!";
        EndGameScreen.enabled = true;

        EndGameScreen.transform.Find("RematchPanel").GetComponent<UINavButton>().Select();

    }

    internal void UpdateScores(int[] scores)
    {
        p1Score.text = scores[0].ToString();
        p2Score.text = scores[1].ToString();
    }

    private void InitTimerPanel(Canvas canvas)
    {

        Image TimerPanel = new GameObject("Timer").AddComponent<Image>();

        TimerPanel.sprite = Resources.Load<Sprite>("UI/Images/InnerGlowPanelSmall");
        TimerPanel.color = new Color(1, 1, 1, 150.0f / 255.0f);

        TimerPanel.type = Image.Type.Sliced;

        TimerPanel.rectTransform.SetParent(canvas.transform);

        TimerPanel.rectTransform.sizeDelta = new Vector2(250, 120);

        TimerPanel.rectTransform.anchorMin = new Vector2(0.5f, 1);
        TimerPanel.rectTransform.anchorMax = new Vector2(0.5f, 1);

        TimerPanel.rectTransform.anchoredPosition = new Vector2(0, -75);

        timerText = new GameObject("Text").AddComponent<TextMeshProUGUI>();
        timerText.transform.SetParent(TimerPanel.rectTransform);
        timerText.font = Resources.Load<TMP_FontAsset>("UI/Fonts/TMP Fonts/PublicPixel SDF");
        timerText.text = "3:00";
        timerText.enableAutoSizing = true;
        timerText.fontSizeMax = 300;
        timerText.alignment = TextAlignmentOptions.Center;
        timerText.rectTransform.anchorMin = new Vector2(0, 0);
        timerText.rectTransform.anchorMax = new Vector2(1, 1);
        timerText.rectTransform.offsetMax = new Vector2(0, 0);
        timerText.rectTransform.offsetMin = new Vector2(0, 0);

    }

    private TextMeshProUGUI InitScorePanel(int playerNum)
    {

        Image player = new GameObject("Player " + playerNum + " Score Panel").AddComponent<Image>();
        player.transform.SetParent(canvas.transform);

        player.sprite = Resources.Load<Sprite>("UI/Images/InnerGlowPanelSmall");
        player.color = new Color(1, 1, 1, 150.0f / 255.0f);

        player.type = Image.Type.Sliced;

        player.rectTransform.SetParent(canvas.transform);

        player.rectTransform.sizeDelta = new Vector2(250, 120);

        player.rectTransform.anchorMin = playerNum == 1 ? new Vector2(0, 0) : new Vector2(1, 0);
        player.rectTransform.anchorMax = playerNum == 1 ? new Vector2(0, 0) : new Vector2(1, 0);
        player.rectTransform.pivot = playerNum == 1 ? new Vector2(0, 0) : new Vector2(1, 0);

        player.rectTransform.anchoredPosition = playerNum == 1 ? new Vector2(25, 25) : new Vector2(-25, 25);

        TextMeshProUGUI scoreText = new GameObject("Text").AddComponent<TextMeshProUGUI>();

        scoreText.transform.SetParent(player.transform);

        scoreText.font = Resources.Load<TMP_FontAsset>("UI/Fonts/TMP Fonts/PublicPixel SDF");
        scoreText.text = "1000";
        scoreText.fontSizeMax = 300;
        scoreText.enableAutoSizing = true;
        scoreText.alignment = TextAlignmentOptions.Center;
        scoreText.rectTransform.anchorMin = new Vector2(0, 0);
        scoreText.rectTransform.anchorMax = new Vector2(1, 1);
        scoreText.rectTransform.offsetMax = new Vector2(0, 0);
        scoreText.rectTransform.offsetMin = new Vector2(0, 0);

        return scoreText;

    }

    private KOBar InitKOBar(int playerNum)
    {

        KOBarSetup setup = new GameObject("Player " + playerNum + " KO Bar").AddComponent<KOBarSetup>();

        setup.gameObject.AddComponent<RectTransform>();

        setup.transform.SetParent(canvas.transform);

        KOBar KOBar = setup.Init(playerNum);

        Destroy(setup);

        return KOBar;

    }

    public void SubscribeUI(GameObject[] players)
    {

        players[0].GetComponent<PlayerKOHandler>().knocked += p1KO.IncrementFill;
        players[0].GetComponent<PlayerKOHandler>().snapOut += p1KO.ResetBar;
        players[0].GetComponent<PlayerKOHandler>().woken += p1KO.ResetBar;

        players[1].GetComponent<PlayerKOHandler>().knocked += p2KO.IncrementFill;
        players[1].GetComponent<PlayerKOHandler>().snapOut += p2KO.ResetBar;
        players[1].GetComponent<PlayerKOHandler>().snapOut += p2KO.ResetBar;

    }

    private void Update()
    {
        if (TryGetComponent<InputSystemUIInputModule>(out InputSystemUIInputModule x))
        {
            Debug.Log(x.move.ToString());
        }
        if (timeGo)
        {
            timerTime -= Time.deltaTime;
        }

        UpdateClock();

    }

    public void CloseEGM()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EndGameScreen.enabled = false;

    }

    public  void StartClock(float startTime)
    {

        timeGo = true;
        timerTime = startTime;

    }

    private void PauseClock()
    {
        timeGo = false;
    }

    private void ResumeClock()
    {
        timeGo = true;
    }

    private void UpdateClock()
    {
        int timerMins = (int)timerTime / 60;
        int timerSecs = (int)timerTime % 60;
        string secsString = (timerSecs < 10 ? "0" : "") + timerSecs;
        timerText.text = "" + timerMins + ":" + secsString;

    }
}
