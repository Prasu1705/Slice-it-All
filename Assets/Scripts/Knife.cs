using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Knife : MonoBehaviour
{
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
                break;
            case KnifeState.Hit:
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
		InputManager.Instance.onClick += KnifeFlip;
		intitalPosition = transform.position;
		initialRotation = transform.rotation;
        rb = transform.GetComponent<Rigidbody>();
		rb.isKinematic = true;
		StartCoroutine(KnifeMovementandRotation());
	}

	// Update is called once per frame
	void Update()
	{
		

		
    }

	public void KnifeFlip()
    {
		if (isGrounded == false && gameObject.transform.GetChild(0).GetComponent<MeshCollider>().enabled == false)
        {
			gameObject.transform.GetChild(0).GetComponent<MeshCollider>().enabled = true;
		}
		StartCoroutine(KnifeFlipOnClick());
	}

	IEnumerator KnifeFlipOnClick()
    {
		
		if (!isTransitioning)
        {
			
			canvas.SetActive(false);
			
			//isGrounded = false;
			rb.velocity = new Vector3(0, 13f, 4);
			rb.AddForce(rb.velocity, ForceMode.VelocityChange);
			yield return new WaitForSeconds(0.2f);
			
			enableRotation = true;

			
		}

		
	}

	private void OnCollisionStay(Collision other)
	{
		
		
		if(other.collider.tag == "GameOver")
        {
			isLost = true;
			isTransitioning = true;
			enableRotation = false;
			GameManager.Instance.CURRENTGAMEPLAYSTATE = GameManager.GamePlayState.Lose;
			//InitialLevelSetup();
		}
	}



    private void OnCollisionEnter(Collision collision)
    {
		if (collision.collider.tag == "Ground" && !isGrounded)
		{
			Debug.Log("Knife is hitting the ground");
			gameObject.transform.GetChild(0).GetComponent<MeshCollider>().enabled = false;
			isGrounded = true;
			rb.isKinematic = true;
			
		}
		if (collision.collider.tag == "Finish")
		{
			isTransitioning = true;
			GameManager.Instance.CURRENTGAMEPLAYSTATE = GameManager.GamePlayState.Win;

		}
	}



    public void DestroyPreviousLevelObjects()
    {
		GameObject[] destroySliceables = GameObject.FindGameObjectsWithTag("Sliceable");
		foreach (GameObject sliceable in destroySliceables)
		{
			Destroy(sliceable);
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
		StartCoroutine(waitForOneSecond());
	}

	public void Restart()
    {
		GameManager.Instance.CURRENTGAMEPLAYSTATE = GameManager.GamePlayState.Restart;
	}

	

	IEnumerator waitForOneSecond()
    {
		yield return new WaitForSecondsRealtime(0.1f);
		transform.position = intitalPosition;
		transform.rotation = initialRotation;
		Camera.main.transform.position = CameraController.Instance.initialPosition;
		canvas.SetActive(true);
	}






	IEnumerator KnifeMovementandRotation()
    {
		while(true)
        {

			rotation = rotationSpeed * Time.deltaTime;
			if (enableRotation)
			{
				if (maxRotation > rotation)
				{
					maxRotation -= rotation;
					transform.Rotate(rotation, 0, 0);
					
				}
				else
				{
					enableRotation = false;
					maxRotation = 270 - transform.eulerAngles.x;
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
				if (rb.isKinematic == false && enableRotation == false && isTransitioning == false)
				{
					currentRotation = Quaternion.Euler(transform.eulerAngles);
					correctRotation = Quaternion.Euler(Vector3.zero);
					currentRotation = Quaternion.Slerp(currentRotation, correctRotation, Time.deltaTime * rotationSmoothness);
					currentRotation.y = 0;
					currentRotation.z = 0;
					transform.rotation = currentRotation;
					//rb.AddForce(rb.velocity, ForceMode.Impulse);
				}
			}
			yield return null;
		}
    }
    









}
