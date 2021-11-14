using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameState {None, Active, Pause, Restart};
public delegate void GameStateHandler();
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public delegate void GameStates(GameState gameState);
    public event GameStates onGameStateChange;
    private GameState gameState;
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance.gameObject);
        }
    }
    // Start is called before the first frame update
    private void Start()
    {
        gameState = GameState.None;
        this.onGameStateChange += GameStateCheck;
    }

    private void GameStateCheck(GameState gameState)
    {
        switch(gameState)
        {
            case GameState.Active:
                Time.timeScale = 1;
                break;
            case GameState.Pause:
                Time.timeScale = 0;
                break;
            default:
                Time.timeScale = 1;
                break;
        }
    }

    private void GameActive()
    {
        onGameStateChange?.Invoke(GameState.Active);
    }

}
