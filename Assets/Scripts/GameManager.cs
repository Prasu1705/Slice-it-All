using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Knife knife;
    public enum GamePlayState { Game, Lose, Win, Restart }

    public enum MenuState { MainMenu, GamePlay, GameOver }

    private GamePlayState currentGamePlayState;

    private MenuState currentMenuState;


    public bool isStart, isLevelStart, isGameOver, isRevive;
    public GamePlayState CURRENTGAMEPLAYSTATE { get { return currentGamePlayState; } set { currentGamePlayState = value; SwitchState(); } }
    //public MenuState CURRENTMENUSTATE { get { return currentMenuState; } set { currentMenuState = value; } }

    //public GameObject NormalStage, FaceOffStage;


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        AssignInstance();
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "SampleScene")
        {
            CURRENTGAMEPLAYSTATE = GamePlayState.Game;
            currentMenuState = MenuState.GamePlay;
        }
    }

    public void SwitchState()
    {
        switch (currentGamePlayState)
        {
            case GamePlayState.Game:
                {
                    Time.timeScale = 1;
                }
                break;
            case GamePlayState.Restart:
                {
                    knife.isLost = false;
                    knife.isRestarting = true;
                    UIManager.Instance.GameOverCanvas.SetActive(false);
                    knife.InitialLevelSetup();
                }
                break;
            case GamePlayState.Lose:
                {
                    StartCoroutine(GameFailed());
                }
                break;
            case GamePlayState.Win:
                {

                    StartCoroutine(GameCompleted());

                }
                break;
        }
    }

    IEnumerator GameCompleted()
    {
        knife.rb.isKinematic = true;
        LevelManager.Instance.LEVEL += 1;
        knife.InitialLevelSetup();
        yield return null;

    }

    IEnumerator GameFailed()
    {
        knife.isTransitioning = true;
        UIManager.Instance.GameOverCanvas.SetActive(true);
        Time.timeScale = 0;
        yield return null;
    }

    void AssignInstance()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            if (Instance != this)
            {
                Destroy(Instance.gameObject);
                Instance = this;
            }
        }
    }
}