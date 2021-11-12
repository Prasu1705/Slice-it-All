using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Knife : MonoBehaviour
{

	public Rigidbody rb;

	public float upForce = 5f, sideForce = 10f;
	public float torque = 20f;

	public int clickCount = 0;
	public bool isMouseclicked = false;
	public float rotation;
	public float maxRotation=270;
	public float rotationSpeed = 100;

	public float rotationSmoothness = 2f;

	public Quaternion currentRotation;

	private Vector3 intitalPosition;
	private Quaternion initialRotation;

	Quaternion correctRotation;

	private float timeWhenWeStartedFlying;

	public bool enableRotation = false;
	// Use this for initialization
	void Start()
	{
		intitalPosition = transform.position;
		initialRotation = transform.rotation;
        rb = transform.GetComponent<Rigidbody>();
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
		else if(other.collider.tag == "Finish")
        {
			rb.isKinematic = true;
			enableRotation = false;
            LevelPrefabSpawnFromJSON.Instance.levelnumber = 1;
			LevelPrefabSpawnFromJSON.Instance.Invoke("Start", 0.1f);
			StartCoroutine(waitForOneSecond());
			
			
		}
	}


	IEnumerator waitForOneSecond()
    {
		yield return new WaitForSecondsRealtime(1);
		rb.isKinematic = false;
		transform.position = intitalPosition;
		transform.rotation = initialRotation;
		Camera.main.transform.position = CameraController.Instance.initialPosition;
	}






	IEnumerator KnifeMovementandRotation()
    {
		while(true)
        {
			if (Input.GetMouseButtonDown(0))
			{
				rb.isKinematic = false;
				rb.velocity = new Vector3(0, 10f, 5);
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
					rb.velocity = new Vector3(0, -6f, 0);
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
