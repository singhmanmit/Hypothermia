using UnityEngine;
using System.Collections;

public class TravelPoints : MonoBehaviour {

	public Transform start;	
	public Transform end;
//	public float dist = Vector3.Distance(start.position, transform.position);
	public Transform ground;

	public Camera mainCam;
	public Camera mapCam;


	bool isMapCameraON;



	// Use this for initialization
	void Start () {

		ground = GameObject.FindGameObjectWithTag ("Ground").transform;

		// the variables will find the camera components for the main and Map camera
		mapCam = GameObject.FindGameObjectWithTag ("MainCamera").camera;
		mainCam = GameObject.FindGameObjectWithTag ("PlayerCamera").camera;

		mainCam.enabled = true;
		mapCam.enabled = false;

		isMapCameraON = false;

	
	}
	
	// Update is called once per frame
	void Update () {

		// The I and the P key will transition to and From the Camera Views
			if (Input.GetKey (KeyCode.I)) {

				StartCoroutine(mapCameraWait(3.0f));
				Debug.Log ("this is the MapCamera");

				}
			if (Input.GetKey (KeyCode.P)) {

				StartCoroutine(revMapCameraWait(3.0f));
				Debug.Log ("this is the MainCamera");

			}

		
		if(isMapCameraON){
//			if(Input.GetMouseButtonDown(0))
//			{
//				Debug.Log("Map Camera is On");
//				RaycastHit hit;
//				if (Physics.Raycast(transform.position,	Vector3.forward, out hit ))
//				{
//					float distance = hit.distance;
//
//					if(GameObject.FindGameObjectWithTag("Ground") == this.ground)
//					{
//						print("you have clicked your destination ------------>");
//					}
//				}
//
//			}


			if (Input.GetButtonDown("Fire1"))
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

				if(Physics.Raycast(ray))
				{
					Debug.Log("you have clicked on a new coordiante " + Input.mousePosition);
				}
			}
		
		}
		
	}


	// These slow down the switching process. We need to time this with the animations so it will properly transition
	// To Map Camera View 
	IEnumerator mapCameraWait(float waitTime)
	{
		yield return new WaitForSeconds (waitTime);
		mainCam.enabled = false;
		mapCam.enabled = true;
		isMapCameraON = true;
		//print ("Map camera is on");

	}

	// To Main Camera View
	IEnumerator revMapCameraWait(float revWaitTime)
	{
		yield return new WaitForSeconds(revWaitTime);
		
		mainCam.enabled = true;
		mapCam.enabled = false;



	}















}