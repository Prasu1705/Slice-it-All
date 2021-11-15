using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Knife : MonoBehaviour
{

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
	// Use this for initialization
	void Start()
	{
		intitalPosition = transform.position;
		initialRotation = transform.rotation;
        rb = transform.GetComponent<Rigidbody>();
		rb.isKinematic = true;
		StartCoroutine("KnifeMovementandRotation");
    }

	// Update is called once per frame
	void Update()
	{
		

		
    }


	private void OnCollisionStay(Collision other)
	{
		if (other.collider.tag == "Ground")
		{
			Debug.Log("Knife is hitting the ground");
			rb.isKinematic = true;
		}
		if(other.collider.tag == "Finish")
        {
			LevelPrefabSpawnFromJSON.Instance.levelnumber +=1;
			rb.isKinematic = true;
			enableRotation = false;
			GameObject[] destroySliceables = GameObject.FindGameObjectsWithTag("Sliceable");
			foreach(GameObject sliceable in destroySliceables)
			{
				Destroy(sliceable);
			}
			LevelPrefabSpawnFromJSON.Instance.Invoke("Start", 0.1f);
			StartCoroutine(waitForOneSecond());
		}
		if(other.collider.tag == "GameOver")
        {
			GameOverScreen.SetActive(true);
			Time.timeScale = 0;
			
        }
	}


	IEnumerator waitForOneSecond()
    {
		yield return new WaitForSecondsRealtime(1);
		transform.position = intitalPosition;
		transform.rotation = initialRotation;
		Camera.main.transform.position = CameraController.Instance.initialPosition;
		canvas.SetActive(true);
	}






	IEnumerator KnifeMovementandRotation()
    {
		while(true)
        {
			if (Input.GetMouseButtonDown(0))
			{
				canvas.SetActive(false);
				rb.isKinematic = false;
				rb.velocity = new Vector3(0, 12f, 5);
				rb.AddForce(rb.velocity, ForceMode.VelocityChange);
				yield return new WaitForSeconds(0.2f);
				enableRotation = true;
			}
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
					rb.velocity = new Vector3(0, -10f, 0);
					rb.AddForce(rb.velocity, ForceMode.VelocityChange);
				}
				
			}
			else
			{
				if (rb.isKinematic == false && enableRotation == false)
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
