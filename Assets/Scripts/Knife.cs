using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Knife : MonoBehaviour
{
	public GameObject KnifeObject;
	public GameObject EffectSpawnPoint;
	public String collisionHitTag;
	public GameObject TrailEffectPoolParent;

	public enum KnifeState { Idle, Flip, Hit};
	public KnifeState knifeState;
	public KnifeState KNIFESTATE {  get { return knifeState; } set { knifeState = value; SwitchKnifeState(); } }

    private void SwitchKnifeState()
    {
        switch (knifeState)
        {
            case KnifeState.Idle:
				rb.isKinematic = true;
				break;
            case KnifeState.Flip:
				EffectController.instance.SetPositionAndPlay("Trail", EffectSpawnPoint.transform.position, true);
				
				StartCoroutine(KnifeFlipOnClick());
				
				break;
            case KnifeState.Hit:
				if (collisionHitTag == "Ground" && !isGrounded)
				{
					Debug.Log("Knife is hitting the ground");
					isGrounded = true;
					enableRotation = false;
					rb.isKinematic = true;
					gameObject.transform.GetChild(0).transform.GetComponent<MeshCollider>().enabled = false;

				}
				if (collisionHitTag == "Finish")
				{
					isTransitioning = true;
					GameManager.Instance.CURRENTGAMEPLAYSTATE = GameManager.GamePlayState.Win;

				}
				if (collisionHitTag == "GameOver")
				{

					isLost = true;
					isTransitioning = true;
					enableRotation = false;
					GameManager.Instance.CURRENTGAMEPLAYSTATE = GameManager.GamePlayState.Lose;
					//InitialLevelSetup();
				}

				break;
            default:
                break;
        }
    }

    public Rigidbody rb;

	public float upForce = 5f, sideForce = 10f;
	public float rotation;
	public float maxRotation=270;
	public float rotationSpeed = 100;
	public float rotationSmoothness = 2f;

	public int Score;

	public GameObject canvas;
	public GameObject GameOverScreen;

	public Quaternion currentRotation;

	private Vector3 intitalPosition;
	private Quaternion initialRotation;

	Quaternion correctRotation;

	

	public bool enableRotation = false;
	public bool isTransitioning = false;
	public bool isWon = false;
	public bool isLost = false;
	public bool isRestarting = false;
	public bool isGrounded = false;

	// Use this for initialization
	void Start()
	{
		GameManager.Instance.CURRENTGAMEPLAYSTATE = GameManager.GamePlayState.Game;
		
		KnifeObject = PlayerManager.Instance.playerObject;
		InputManager.Instance.onClick += KnifeFlip;
		intitalPosition = KnifeObject.transform.position;
		initialRotation = KnifeObject.transform.rotation;
        rb = KnifeObject.GetComponent<Rigidbody>();
		KNIFESTATE = KnifeState.Idle;
		StartCoroutine(KnifeMovementandRotation());
	}

	// Update is called once per frame
	void Update()
	{

		
		
    }

	public void KnifeFlip()
    {
		KNIFESTATE = KnifeState.Flip;
	}

	IEnumerator KnifeFlipOnClick()
    {
		
		if (!isTransitioning)
        {

			UIManager.Instance.GameStartCanvas.SetActive(false);
			
			
			rb.velocity = new Vector3(0, 13f, 4);
			rb.AddForce(rb.velocity, ForceMode.VelocityChange);
			yield return new WaitForSeconds(0.2f);
			
			enableRotation = true;
			EffectController.instance.SpawnEffect("Trail", EffectSpawnPoint.transform.position, transform);

		}

		
	}

	private void OnCollisionStay(Collision other)
	{
		
		
	}

    private void OnCollisionEnter(Collision collision)
    {
		collisionHitTag = collision.collider.tag;
		KNIFESTATE = KnifeState.Hit;
		
		

	}

	public void PlayerPrefsScore()
    {
		PlayerData.Instance.SCORE = Score;
    }




    public void DestroyPreviousLevelObjects()
    {
		GameObject[] destroySliceables = GameObject.FindGameObjectsWithTag("Sliceable");
		GameObject[] destroyCubeSliceable = GameObject.FindGameObjectsWithTag("SliceableCube");
		GameObject[] destroySphereSliceable = GameObject.FindGameObjectsWithTag("SliceableSphere");
		GameObject[] destroyGameOverObstacles = GameObject.FindGameObjectsWithTag("GameOver");
		
		foreach (GameObject sliceable in destroySliceables)
		{

			Destroy(sliceable);
		}

		foreach(GameObject GameoverObstacle in destroyGameOverObstacles)
        {
			GameoverObstacle.SetActive(false);
        }
		foreach(GameObject cubeSliceable in destroyCubeSliceable)
		{ 
			cubeSliceable.SetActive(false);
        }
		foreach(GameObject sphereSliceable in destroySphereSliceable)
		{ 
			sphereSliceable.SetActive(false);
        }
	}


	public void InitialLevelSetup()
    {
		Time.timeScale = 1;
		isTransitioning = false;
		enableRotation = false;
		rb.isKinematic = true;
		DestroyPreviousLevelObjects();
		LevelPrefabSpawnFromJSON.Instance.Invoke("Start", 0.1f);
		StartCoroutine(SetKnifeAndCameraToInitialPositions());
	}

	public void Restart()
    {
		GameManager.Instance.CURRENTGAMEPLAYSTATE = GameManager.GamePlayState.Restart;
	}

	

	IEnumerator SetKnifeAndCameraToInitialPositions()
    {
		yield return new WaitForSecondsRealtime(0.1f);
		KnifeObject.transform.position = intitalPosition;
		KnifeObject.transform.rotation = initialRotation;
		Camera.main.transform.position = CameraController.Instance.initialPosition;
		UIManager.Instance.GameStartCanvas.SetActive(true);
	}






	IEnumerator KnifeMovementandRotation()
    {
		while(true)
        {

			rotation = rotationSpeed * Time.deltaTime;
			if (enableRotation && !isGrounded)
			{
				if (maxRotation > rotation)
				{
					maxRotation -= rotation;
					KnifeObject.transform.Rotate(rotation, 0, 0);
					gameObject.transform.GetChild(0).transform.GetComponent<MeshCollider>().enabled = true;
				}
				else
				{
					enableRotation = false;
					maxRotation = 270 - KnifeObject.transform.eulerAngles.x;
					if(maxRotation <0)
                    {
						maxRotation += 360;
                    }

					
					rotation = maxRotation;
					rb.velocity = new Vector3(0, -10f, 2.75f);
					rb.AddForce(rb.velocity, ForceMode.VelocityChange);
				}
				
			}
			else
			{
				if (rb.isKinematic == false && enableRotation == false && isTransitioning == false && !isGrounded)
				{
					currentRotation = Quaternion.Euler(KnifeObject.transform.eulerAngles);
					correctRotation = Quaternion.Euler(Vector3.zero);
					currentRotation = Quaternion.Slerp(currentRotation, correctRotation, Time.deltaTime * rotationSmoothness);
					currentRotation.y = 0;
					currentRotation.z = 0;
					KnifeObject.transform.rotation = currentRotation;
					//rb.AddForce(rb.velocity, ForceMode.Impulse);
				}
			}
			yield return null;
		}
    }
    









}
