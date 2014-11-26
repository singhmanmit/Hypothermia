/* This script is responsible for controlling the small monster.
 * Will "pop" out of the ground when the player is near, will give chase
 * up to a certain distance, but will eventually fall back down into a
 * hole when the player is out of range. Will also fall back down into a 
 * hole when it's killed, and the game object will destroy itself after
 * a certain period of time.
 */ 

using UnityEngine;
using System.Collections;

public class SmallMonsterAI : MonoBehaviour 
{
	// Declare variables here, and set up values in Start()
	public bool isJump;
	public bool isAwake;
	public bool isDead;
	public bool canJump;
	public bool playParticles;
	public float health;
	public float speed;
	public float pop;
	public float maxPop;
	public float minPop;
	public float power;
	public float radius;
	public float yValue;

	public float deadTimer;
	public float waypointTimer;

	public GameObject groundChecker;
	public GameObject cubes;
	public Transform target;
	public Vector3 waypoint;

	private State currentState;
	private float chaseDistance;
	private float attackDistance;
	private float destroyDistance;
	private float popDownDistance;

	// Private references
	private SphereCollider sphereCollider;
	private SmallMonsterSpawner spawner;
	private Health player;
	private ParticleSystem particles;

	// Set to public, but still not showing up in the inspector - what's up with that
	public enum State {
		Idle,
		Roam,
		Chase,
		Attack
	};

	void Awake()
	{
		spawner = GameObject.FindWithTag ("Spawner").GetComponent<SmallMonsterSpawner> ();
		player = GameObject.FindWithTag ("Player").GetComponent<Health> ();
		sphereCollider = GetComponent<SphereCollider> ();
		groundChecker = this.gameObject.transform.FindChild ("GroundChecker").gameObject;
		particles = this.gameObject.GetComponent<ParticleSystem>();
	}

	void Start()
	{
		// Set up variables and references
		isAwake = false;
		isJump = false;
		playParticles = false;
		health = 100.0f;
		speed = 3.0f;
		pop = 25.0f;
		power = 3.0f;
		radius = 5.0f;
		chaseDistance = 50.0f;
		attackDistance = 12.0f;
		destroyDistance = 60.0f;
		popDownDistance = 20.0f;

		deadTimer = 0.0f;
		waypointTimer = 0.0f;

		gameObject.rigidbody.isKinematic = true;
		gameObject.collider.enabled = false;

		sphereCollider.radius = chaseDistance;
		yValue = player.transform.position.y - 3.0f;
		transform.position = new Vector3 (transform.position.x, yValue, transform.position.z);
	}

	void Update()
	{
		maxPop = player.transform.position.y + 0.2f;
		minPop = player.transform.position.y;

		// Popping out of the ground 
		if(isJump && canJump && !isDead && !spawner.inBuilding) {
			// Set rigidbody.isKinematic to false, so that gravity can be used. Previously turned false to keep below surface.
			gameObject.rigidbody.isKinematic = false;
			gameObject.rigidbody.AddForce(Vector3.up * pop);

			if(!isAwake){
				playParticles = true;
				if(playParticles) {
					particles.Play();
				}
			}

			if(gameObject.transform.position.y > maxPop) {
				gameObject.collider.enabled = true;
				gameObject.rigidbody.AddForce(-Vector3.up * pop);
				isJump = false;
				isAwake = true;
			}
		}

		// If the monster is not dead, and the player is inside of a house
		if(currentState == State.Roam && !isDead) {
			speed = 3.0f;
			target = GameObject.FindGameObjectWithTag("Player").transform;

			Vector3 direction = target.transform.position - transform.position;
			transform.LookAt(target);

			waypointTimer += 1.0f * Time.deltaTime;

			if(waypointTimer >= 5.0f) {
				GetNewWaypoint();
				waypointTimer = 0.0f;
			}

			if(direction.magnitude > destroyDistance) {
				spawner.smallMonsters.Remove(this.gameObject);
				Destroy(this.gameObject);
			}

			Vector3 moveVector = waypoint.normalized * speed * Time.deltaTime;
			transform.position += moveVector;
		}

		// Fall back into the ground if the player is out of range or the monster is dead
		if(currentState == State.Idle || isDead) {
			gameObject.collider.enabled = false;
			isAwake = false;

			if(gameObject.transform.position.y < minPop) {
				gameObject.rigidbody.isKinematic = true;

				if(!isDead) {
					currentState = State.Roam;
				}
			}
		}
	
		// Chase State (if the player are within range)
		if(currentState == State.Chase && !isDead) {
			speed = 6.0f;
			target = GameObject.FindGameObjectWithTag ("Player").transform;

			// Get the direction to travel in by subtracting the target's position with the monster's position. Set y to a negative value
			Vector3 direction = target.transform.position - transform.position;

			// Look in the direction of the player
			transform.LookAt(target);

			// Keep moving if the player is out of attack range
			if(direction.magnitude > attackDistance) {
				Vector3 moveDirection = direction.normalized * speed * Time.deltaTime;
				transform.position += moveDirection;
			}

			// Stop moving and switch to attack mode
			if(direction.magnitude <= attackDistance) {
				isJump = true;
				currentState = State.Attack;
				direction.x = 0.0f;
			}

			// Pop back under the ground and continue to chase the player
			if(direction.magnitude >= popDownDistance) {
				gameObject.collider.enabled = false;
				isAwake = false;
				
				if(gameObject.transform.position.y < minPop) {
					gameObject.rigidbody.isKinematic = true;
					
					if(!isDead) {
						Vector3 moveDirection = direction.normalized * speed * Time.deltaTime;
						transform.position += moveDirection;
					}
				}
			}
		}

		if(currentState == State.Attack) {
			// Look at the player
			transform.LookAt(target);

			// Shoot a raycast forward, and if it's a hit - blow ice onto the player
			RaycastHit hit;
			if(Physics.Raycast(transform.position, Vector3.forward, out hit, attackDistance)) {
				if(hit.transform.gameObject == GameObject.FindWithTag("Player")) {
					player.SendMessage("OnDamage", 10.0f);
				}
			}
		}

		// If the monster is dead, set isDead to true - this will keep the monster from popping back up
		if(health <= 0.0f) {
			isDead = true;
			Debug.Log (gameObject.name + " is dead"); 
			spawner.Remove(gameObject.name);
		}

		if(isDead) {
			deadTimer += 1.0f * Time.deltaTime;

			if (deadTimer > 3.0f) {
				deadTimer = 0;
				Destroy (gameObject); // Destroy the dead monster and free up space for a new monster
			}
		}
	}

	Vector3 GetNewWaypoint()
	{
//		Debug.Log ("New Waypoint..." + waypoint);
		waypoint = new Vector3 (Random.Range (player.transform.position.x / 2, player.transform.position.x / 2), 
		                        0.0f, 
		                        Random.Range (player.transform.position.z / 2, player.transform.position.z / 2));
		return waypoint;
	}

	void FixedUpdate()
	{
		// Ignore collisions with the ice blocks on layer 8
		Physics.IgnoreLayerCollision (0, 8, true);
	}

	void OnTriggerStay (Collider other) 
	{
		if (other.gameObject == GameObject.FindWithTag ("Player")) {
			currentState = State.Chase;
		}
	}

	void OnTriggerExit (Collider other) 
	{
		if (other.gameObject == GameObject.FindWithTag ("Player")) {
			currentState = State.Idle;
		}
	}
}