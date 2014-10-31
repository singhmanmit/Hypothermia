using UnityEngine;
using System.Collections;

public class barrel : MonoBehaviour {


	public GameObject Player;
	public Health playerHealth;



	// Use this for initialization
	void Start () {
	
		Player = GameObject.Find("Player");
		playerHealth = Player.GetComponent<Health>();

	}
	
	// Update is called once per frame
	void Update () {



	
	}



	void OnTriggerStay(Collider col)
	{
		if(col.gameObject.tag == "Player")
		{
			//print("HE HERE");
			playerHealth.loseHealth = false;
		}

	}


	void OnTriggerExit(Collider col)
	{

		if(col.gameObject.tag == "Player")
		{
			playerHealth.loseHealth = true;
		}
	}

}
