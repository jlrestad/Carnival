using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    GameManager GM;

    private void Awake()
    {
        GM = GameManager.Instance;
        GM.OnStateChange += HandleOnStateChange;
    }

    public void HandleOnStateChange()
    {
        Debug.Log("OnStateChange!");
    }

    //public void OnGUI()
    //{
    //    // Menu layout
    //    GUI.BeginGroup(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 50, 100, 800));
    //    GUI.Box(new Rect(0, 0, 100, 200),"Menu");

    //    if (GUI.Button(new Rect(10, 40, 80, 30), "Start"))
    //    {
    //        StartGame();
    //    }
    //    if (GUI.Button(new Rect(10, 160, 80, 30), "Quit"))
    //    {
    //        Quit();
    //    }
    //    GUI.EndGroup();
    //}

    public void StartGame()
    {
        // Start game scene
        GM.SetGameState(GameState.LEVEL_ONE);
        SceneManager.LoadScene("SandBox", LoadSceneMode.Single);
        Debug.Log(GM.gameState);
    }

    public void Quit()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}
