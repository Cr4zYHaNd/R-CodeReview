using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OnCourtPlayerManager : MonoBehaviour
{
    private GameObject player1, player2;
    private int startingPoints = 500;
    private int[] scores = new int[2];
    public Action scored;
    // Start is called before the first frame update
    public GameObject[] Init(PlayerInput p1, PlayerInput p2, Vector2[] StartPositions)
    {

        player1 = p1.gameObject.AddComponent<PlayerSetup>().Init(1, StartPositions[0], p1);

        Destroy(player1.GetComponent<PlayerSetup>());

        player2 = p2.gameObject.AddComponent<PlayerSetup>().Init(2, StartPositions[1], p2);

        Destroy(player2.GetComponent<PlayerSetup>());

        ResetScores();

        return new GameObject[] { player1, player2 };

    }

    public void PointScore(float xVal, int pointVal)
    {

        scores[xVal < 0 ? 0 : 1] -= pointVal;
        scores[xVal < 0 ? 0 : 1] = scores[xVal < 0 ? 0 : 1] < 0 ? 0 : scores[xVal < 0 ? 0 : 1];

        scored?.Invoke();
    }
    public void EnablePlayers()
    {
        player1.GetComponent<PlayerInputHandler>().enabled = true;
        player1.GetComponent<PlayerMovementControl>().enabled = true;
        player2.GetComponent<PlayerInputHandler>().enabled = true;
        player2.GetComponent<PlayerMovementControl>().enabled = true;
    }

    public void ResetPlayerPositions()
    {
        PlayerStart temp = null;

        int n = 0;

        while (temp == null)
        {

            temp = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects()[n].GetComponent<PlayerStart>();
            n++;

        }
        player1.transform.position = temp.GetStartPositions()[0];
        player2.transform.position = temp.GetStartPositions()[1];
    }

    public void DisablePlayers()
    {
        player1.GetComponent<PlayerInputHandler>().enabled = false;
        player1.GetComponent<PlayerMovementControl>().enabled = false;
        player2.GetComponent<PlayerInputHandler>().enabled = false;
        player2.GetComponent<PlayerMovementControl>().enabled = false;
        player1.GetComponent<PlayerInput>().SwitchCurrentActionMap("Menu");
    }

    internal void ResetScores()
    {
        scores = new int[] { startingPoints, startingPoints };
    }

    public int[] GetScores()
    {
        return scores;
    }
    public GameObject[] GetPlayers()
    {
        return new GameObject[] { player1, player2 };


    }

    internal void ResetPlayers()
    {
        ResetPlayerPositions();
        ResetScores();
        EnablePlayers();
    }
}
