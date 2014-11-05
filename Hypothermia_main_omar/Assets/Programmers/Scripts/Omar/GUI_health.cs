using UnityEngine;
using System.Collections;
//using UnityEngine.UI;

public class GUI_health : MonoBehaviour {

	//public Image health;

	public GameObject Player;
	public Health playerHealth;


	private float moveX = 500.0f;
	// Use this for initialization
	void Start () {
		Player = GameObject.Find("Player");
		playerHealth = Player.GetComponent<Health>();
	}
	
	// Update is called once per frame
	void Update () {

		this.transform.localScale = new Vector3 ((playerHealth.health/100.0f), 0.2f, 0.0f);

		//health.transform.position += new Vector3(-(playerHealth.health/100.0f)/moveX,0.0f,0.0f);


	}
}