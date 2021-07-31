using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    GameManager GM;

    private void Awake()
    {
        GM = GameManager.Instance;
        GM.OnStateChange += HandleOnStateChange;

        Debug.Log("Current game state when Awake: " + GM.gameState);
    }

    void Start()
    {
        Debug.Log("Current game state when Start: " + GM.gameState);
        GM.SetGameState(GameState.MAIN_MENU);
    }

    public void HandleOnStateChange()
    {
        Debug.Log("Handling state change to: " + GM.gameState);
        Invoke("LoadLevel", 3f);
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Additive);
    }
}
