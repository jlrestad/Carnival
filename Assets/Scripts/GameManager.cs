using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// GAME STATES
public enum GameState { MAIN_MENU, MAP_LEVEL, LEVEL_ONE}

public delegate void OnStateChangeHandler();

public class GameManager : MonoBehaviour
{
    public GameState gameState {get; private set;}

}
