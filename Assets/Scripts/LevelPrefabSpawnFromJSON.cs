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
    public GameObject spherePrefab;

    public int PrefabId;
    //public GameObject spherePrefab;
    // Start is called before the first frame update
    void Start()
    {
        jsonString = File.ReadAllText(Application.dataPath + "/Resources/PrefabProperties.json");
        prefabData = JsonMapper.ToObject(jsonString);
        
        //level-no(key value),constvalue=0,levelprefab(key value),cube-prop(dictionary in a array),property (key value)
        Debug.Log(prefabData[0][1]["Sphere"].Count);
        SpawnPrefab("Cube", cubePrefab);
        SpawnPrefab("Sphere", spherePrefab);
    }


    public void SpawnPrefab(string prefabType, GameObject objectPrefab)
    {
        if(prefabType == "Cube")
        {
            PrefabId = 0;
        }
        else if(prefabType == "Sphere")
        {
            PrefabId = 1;
        }

        for (int i = 0; i < prefabData[0][PrefabId][prefabType].Count; i++)
        {
            spawnPosition.x = (int)prefabData[0][PrefabId][prefabType][i]["XPos"];
            spawnPosition.y = (int)prefabData[0][PrefabId][prefabType][i]["YPos"];
            spawnPosition.z = (int)prefabData[0][PrefabId][prefabType][i]["ZPos"];
            spawnRotation.x = (int)prefabData[0][PrefabId][prefabType][i]["XRot"];
            spawnRotation.y = (int)prefabData[0][PrefabId][prefabType][i]["YRot"];
            spawnRotation.z = (int)prefabData[0][PrefabId][prefabType][i]["ZRot"];
            spawnScale.x = (int)prefabData[0][PrefabId][prefabType][i]["XScale"];
            spawnScale.y = (int)prefabData[0][PrefabId][prefabType][i]["YScale"];
            spawnScale.z = (int)prefabData[0][PrefabId][prefabType][i]["ZScale"];
            GameObject prefab = Instantiate(objectPrefab, spawnPosition, spawnRotation);
            prefab.transform.localScale += spawnScale;


        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
