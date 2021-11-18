using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance; 

    private int scorePlayerPref, levelNumberPlayerpref;
    public int SCORE { get { scorePlayerPref = PlayerPrefs.GetInt("Score"); return scorePlayerPref; } set { scorePlayerPref = value; PlayerPrefs.SetInt("Score", value); } }
    public int LEVEL { get { levelNumberPlayerpref = PlayerPrefs.GetInt("level"); return levelNumberPlayerpref;  } set { levelNumberPlayerpref = value; PlayerPrefs.SetInt("level", value); } }


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }
        PlayerData.Instance.LEVEL = 1;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
