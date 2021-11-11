using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameState {Active, Pause, Lose, Win};
public delegate void onStateChangeHandler();
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState gameState;
    public event onStateChangeHandler onStateChange;
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
   public void SetGameState(GameState state)
   {
        this.gameState = state;

   }
}
