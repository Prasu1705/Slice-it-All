using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance; 

    public GameObject playerObject;
    private Vector3 offset;
    public Vector3 initialPosition;

    private void Awake()
    {
        if(Instance ==  null)
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
        initialPosition = transform.position;
        offset = transform.position - playerObject.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = playerObject.transform.position + offset;

    }
}
