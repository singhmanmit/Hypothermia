using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

	public Texture2D text1;
	public Texture2D text2;
	public Texture2D text3;
	public Texture2D text4;


	public float health = 100;
	private float maxHealth = 100;

	private float DOT = 0.2f;

	public bool loseHealth = false;

	public bool storm = false;

	public bool enterCollider = false;

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

		// ========================================================================================================

		if(loseHealth == true)
		{
			health -= Time.deltaTime * DOT;
		}



		if(health > 0 && enterCollider == true)
		{
			health -= Time.deltaTime * 2;
		}

		if(health >= 0 && enterCollider == false)
		{
			health += Time.deltaTime * 2;
		}

		if(health > maxHealth)
		{
			health = maxHealth;
		}


		/*
		if(health < 50)
		{

			health= 100;
		}
		*/
		
		// ========================================================================================================


		// print(health);

	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Player")
		{
			enterCollider = true;
			Debug.Log("The player is in the collider");
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			enterCollider = false;
			Debug.Log("The player has left the collider");
		}
					
	}

	void OnGUI ()
	{
		if (health <= 100)
		{
			GUI.DrawTexture(new Rect(0,0, Screen.width, Screen.height), text1);
		}
		
		if (health <= 95)
		{
			GUI.DrawTexture(new Rect(0,0, Screen.width, Screen.height), text2);
		}
		
		if (health <= 90)
		{
			GUI.DrawTexture(new Rect(0,0, Screen.width, Screen.height), text3);
		}
		
		if (health <= 85)
		{
			GUI.DrawTexture(new Rect(0,0, Screen.width, Screen.height), text4);
		}
	}


	void OnDamage(float _damage)
	{
		health -= _damage;
	}

}
