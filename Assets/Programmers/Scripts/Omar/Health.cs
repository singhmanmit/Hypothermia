using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {




	private float health = 100;

	private float DOT = 0.2f;

	public bool loseHealth = false;

	public bool storm = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {


		if(storm == true)
		{
			DOT = 2.0f;

		}

		if(storm == false)
		{
			DOT = 0.2f;
			
		}



		if(loseHealth == true)
		{
			health -= Time.deltaTime * DOT;
		}


		if(health < 50)
		{

			health= 100;
		}

		// print(health);

	}

	void OnDamage(float _damage)
	{
		health -= _damage;
	}

}
