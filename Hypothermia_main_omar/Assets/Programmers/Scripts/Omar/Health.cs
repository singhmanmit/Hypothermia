using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {




	public float health = 100;

	public float DOT = 0.2f;

	public bool loseHealth = false;

	public bool storm = false;

	public controller p_movement;

	// Use this for initialization
	void Start () {
	
		p_movement = this.GetComponent<controller>();
	}
	
	// Update is called once per frame
	void Update () {


		if(storm == true)
		{
			DOT = 2.0f;

			p_movement.speed = 2;
		}

		if(storm == false)
		{
			DOT = 0.2f;

			p_movement.speed = 10;
			
		}



		if(loseHealth == true)
		{
			health -= Time.deltaTime * DOT;
		}


		if(health < 10)
		{

			health= 100;
		}

		print(health);

	}


}
