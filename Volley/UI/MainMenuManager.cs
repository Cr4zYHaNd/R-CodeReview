using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


public class MainMenuManager : MonoBehaviour
{
    [SerializeField] BallManager ballManager;
    [SerializeField] BallKillZone ballKillZone;
    [SerializeField] Image[] shutters;
    [SerializeField] TextMeshProUGUI startText;
    [SerializeField] TextMeshProUGUI[] MenuLayer1;

    private void Awake()
    {
        ballManager.Init();

        ballManager.StartSpawning();

        GetComponent<PlayerInputManager>().onPlayerJoined += OnStartPress;

        for (int i = 0; i < MenuLayer1.Length; i++)
        {

            MenuLayer1[i].canvasRenderer.SetAlpha(0);

        }
    }

    public void ChooseOption(int option)
    {
        switch (option)
        {
            case 0:

                StartGame();

                break;
            case 1:
                break;
            case 2:

#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;

#else           
                

                Application.Quit(); 


#endif
                break;
            default:
                break;
        }


    }

    private void StartGame()
    {
        StartCoroutine(ShutterClose());
    }

    private IEnumerator FadeToMenu()
    {

        startText.CrossFadeAlpha(0f, 0.4f, true);
        yield return new WaitForSeconds(0.4f);
        foreach (TextMeshProUGUI menuText in MenuLayer1)
        {
            menuText.CrossFadeAlpha(1f, 0.4f, true);
        }
        yield return new WaitForSeconds(0.4f);
        MenuLayer1[0].GetComponent<Selectable>().Select();

    }

    private void OnStartPress(PlayerInput obj)
    {

        StartCoroutine(FadeToMenu());

    }

    private IEnumerator ShutterClose()
    {

        float progress = 0;

        while (progress < 1)
        {
            yield return null;

            progress += Time.deltaTime * 1.25f;

            foreach (Image shutter in shutters)
            {
                shutter.rectTransform.sizeDelta = new Vector2(progress * 960f, 1080);
            }
        }

        SceneManager.LoadScene("SC_ControllerAssignment", LoadSceneMode.Single);

        /*yield return new WaitForSeconds(0.5f);

        while (progress > 0)
        {
            yield return null;

            progress -= Time.deltaTime * 1.25f;

            foreach (Image shutter in shutters)
            {
                shutter.rectTransform.sizeDelta = new Vector2(progress * 960f, 1080);
            }
        }

        SceneManager.MoveGameObjectToScene(this.gameObject, SceneManager.GetActiveScene());

        Destroy(this.gameObject);
        Destroy(shutters[0].gameObject);
        Destroy(shutters[1].gameObject); */

    }

}
