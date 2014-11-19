using UnityEngine;
using System.Collections;

public class TravelPoints : MonoBehaviour {

	public Transform start;	
	public Transform end;
//	public float dist = Vector3.Distance(start.position, transform.position);
	public Transform ground;



	// Use this for initialization
	void Start () {

		ground = GameObject.FindGameObjectWithTag ("Ground").transform;
	
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;
			if (Physics.Raycast(transform.position,	Vector3.forward, out hit ))
			{
				float distance = hit.distance;

				if(GameObject.FindGameObjectWithTag("Ground") == this.ground)
				{
					print("you have clicked your destination");
				}
			}

		}
	
	}
}
