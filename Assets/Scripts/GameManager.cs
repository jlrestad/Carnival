using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// GAME STATES (https://hub.packtpub.com/creating-simple-gamemanager-using-unity3d/)
public enum GameState { INTRO, MAIN_MENU, MAP_LEVEL, LEVEL_ONE}

public delegate void OnStateChangeHandler();

public class GameManager : ScriptableObject
{
    protected GameManager() {}
    private static GameManager instance = null;
    public event OnStateChangeHandler OnStateChange;
    public GameState GameState {get; private set;}

    public static GameManager Instance
    {
        get
        {
            if (GameManager.instance == null)
            {
                GameManager.instance = (GameManager)ScriptableObject.CreateInstance("GameManager");
                DontDestroyOnLoad(GameManager.instance);
            }
            return GameManager.instance;
        }
    }

    public void SetGameState(GameState state)
    {
        this.GameState = state;
        OnStateChange();
    }
}
