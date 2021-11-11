using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LitJson;

public class LevelPrefabSpawnFromJSON : MonoBehaviour
{
    private string jsonString;
    private JsonData prefabData;

    private Vector3 spawnPosition;
    private Quaternion spawnRotation;
    private Vector3 spawnScale;

    public GameObject cubePrefab;
    //public GameObject spherePrefab;
    // Start is called before the first frame update
    void Start()
    {
        jsonString = File.ReadAllText(Application.dataPath + "/Resources/PrefabProperties.json");
        prefabData = JsonMapper.ToObject(jsonString);
        
        //level-no(key value),constvalue=0,levelprefab(key value),cube-prop(dictionary in a array),property (key value)
        Debug.Log(prefabData[0][0]["Cube"].Count);
        SpawnPrefab("Cube", cubePrefab);
    }


    public void SpawnPrefab(string prefabType, GameObject objectPrefab)
    {
        for (int i = 0; i < prefabData[0][0][prefabType].Count; i++)
        {
            spawnPosition.x = (int)prefabData[0][0][prefabType][i]["XPos"];
            spawnPosition.y = (int)prefabData[0][0][prefabType][i]["YPos"];
            spawnPosition.z = (int)prefabData[0][0][prefabType][i]["ZPos"];
            spawnRotation.x = (int)prefabData[0][0][prefabType][i]["XRot"];
            spawnRotation.y = (int)prefabData[0][0][prefabType][i]["YRot"];
            spawnRotation.z = (int)prefabData[0][0][prefabType][i]["ZRot"];
            spawnScale.x = (int)prefabData[0][0][prefabType][i]["XScale"];
            spawnScale.y = (int)prefabData[0][0][prefabType][i]["YScale"];
            spawnScale.z = (int)prefabData[0][0][prefabType][i]["ZScale"];
            GameObject prefab = Instantiate(objectPrefab, spawnPosition, spawnRotation);
            prefab.transform.localScale += spawnScale;


        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
