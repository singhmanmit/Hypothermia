using UnityEngine;
using System.Collections;

public class SmallMonsterAI : MonoBehaviour 
{
	public enum State {
		Idle,
		Chase,
		Attack
	};

	public bool isJump;
	public bool isAwake;
	public float health;
	public float speed;
	public float pop;
	public float maxPop;

	public Vector3 ground;
	public GameObject cubes;

	void Start()
	{
		isAwake = false;
		health = 100.0f;
		speed = 8.0f;
		pop = 30.0f;
		maxPop = 0.1f;
		ground = new Vector3 (transform.position.x, 0.3f, transform.position.z);

		gameObject.rigidbody.isKinematic = true;
		gameObject.collider.enabled = false;
	}

	void Update()
	{
		if(isJump) {
			gameObject.rigidbody.isKinematic = false;
			gameObject.rigidbody.mass = 0.5f;
			gameObject.rigidbody.AddForce(Vector3.up * pop);

			if(!isAwake){
				Rigidbody clone = Instantiate(cubes, ground, Quaternion.identity) as Rigidbody;
				cubes.rigidbody.AddExplosionForce (Random.Range (-15.0f, 15.0f), Vector3.right, 15.0f);
			}

			if(gameObject.transform.position.y > maxPop) {
				gameObject.collider.enabled = true;
				gameObject.rigidbody.AddForce(Vector3.up * pop * 2);
				isJump = false;
				isAwake = true;
			}
			Debug.Log ("Pop!");
		}

		if(isAwake) {
			GameObject[] cubes = GameObject.FindGameObjectsWithTag("IceBlock");
			foreach(var obj in cubes) {
//				Destroy (obj);
			}
		}
	}

	void FixedUpdate()
	{
		Physics.IgnoreLayerCollision (0, 8, true);
	}
}