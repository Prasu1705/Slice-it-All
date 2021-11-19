using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    private int levelNumberPlayerpref;
    public int LEVEL { get { levelNumberPlayerpref = PlayerPrefs.GetInt("level"); return levelNumberPlayerpref; } set { levelNumberPlayerpref = value; PlayerPrefs.SetInt("level", value); } }

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
