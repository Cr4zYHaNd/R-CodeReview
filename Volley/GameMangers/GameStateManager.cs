using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameStateManager : MonoBehaviour
{

    private OnCourtUIManager displayBoard;
    private BallManager ballBoy;
    private TMPEManager TMPEs;
    private OnCourtPlayerManager play;
    public Action reloaded;
    private bool playActive = false;


    public void Init(PlayerInput p1, PlayerInput p2, Vector2[] vector2s)
    {
        play = new GameObject("PlayerManager").AddComponent<OnCourtPlayerManager>();
        GameObject[] players = play.Init(p1, p2, vector2s);

        reloaded += p1.GetComponent<PlayerMovementControl>().OnReload;
        reloaded += p2.GetComponent<PlayerMovementControl>().OnReload;

        ballBoy = new GameObject("BallBoy").AddComponent<BallManager>();
        ballBoy.Init(play);

        displayBoard = new GameObject("UIManager").AddComponent<OnCourtUIManager>();
        displayBoard.Init();

        displayBoard.SubscribeUI(players);

        displayBoard.menued += GoToMenu;
        displayBoard.restarted += Reload;

        ballBoy.StartSpawning();

        playActive = true;

        TMPEs = new GameObject("TextMeshPool").AddComponent<TMPEManager>();
        TMPEs.Init();

        play.scored += OnScore;

    }

    private void Reload()
    {
        displayBoard.CloseEGM();

        reloaded?.Invoke();

        play.ResetPlayers();

        displayBoard.StartClock(30f);
        displayBoard.UpdateScores(play.GetScores());

        ballBoy.ToggleSpawning();
        playActive = true;

    }
    private void OnScore()
    {
        displayBoard.UpdateScores(play.GetScores());
        for (int i = 0; i < 2; i++)
        {
            if (play.GetScores()[i] == 0)
            {
                EndGame(2 - i);
                return;
            }
        }
    }

    private void EndGame(int winnerPlaynum)
    {
        if (!playActive)
        {
            return;
        }
        playActive = false;
        play.DisablePlayers();
        ballBoy.DespawnAll();
        ballBoy.ToggleSpawning();
        displayBoard.ShowGameEndMenu(winnerPlaynum);
    }

    private void GoToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }


}