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
	public float power;
	public float radius;
	public float yValue;
	public float yOffset;

	public float deadTimer;
	public float waypointTimer;

	public GameObject groundChecker;
	public GameObject cubes;
	public Transform target;
	public Vector3 waypoint;

	public RaycastHit ground;

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
	private MeshRenderer[] mesh;
	private SkinnedMeshRenderer[] skinned;

	// Set to public, but still not showing up in the inspector - what's up with that
	public enum State {
		Idle,
		Roam,
		Chase,
		Attack
	};

	void Awake()
	{
		mesh = GetComponentsInChildren<MeshRenderer> ();
		skinned = GetComponentsInChildren<SkinnedMeshRenderer> ();
		sphereCollider = GetComponent<SphereCollider> ();
		particles = this.gameObject.GetComponent<ParticleSystem>();
		spawner = GameObject.FindWithTag ("Spawner").GetComponent<SmallMonsterSpawner> ();
		player = GameObject.FindWithTag ("Player").GetComponent<Health> ();
		groundChecker = this.gameObject.transform.FindChild ("GroundChecker").gameObject;
	}

	void Start()
	{
		// Set up variables and references
		isAwake = false;
		isJump = false;
		playParticles = false;
		health = 100.0f;
		speed = 3.0f;
		power = 3.0f;
		radius = 5.0f;
		chaseDistance = 40.0f;
		attackDistance = 20.0f;
		destroyDistance = 45.0f;
		popDownDistance = 25.0f;
		yOffset = 20.0f;

		deadTimer = 0.0f;
		waypointTimer = 0.0f;

		gameObject.rigidbody.isKinematic = true;
		gameObject.collider.enabled = false;
		sphereCollider.enabled = true;

		foreach (var m in mesh) {
			m.enabled = false;
		}
		
		foreach (var s in skinned) {
			s.enabled = false;
		}

		Physics.IgnoreLayerCollision (0, 8, true);

		sphereCollider.radius = chaseDistance;
		yValue = player.transform.position.y - yOffset;
		transform.position = new Vector3 (transform.position.x, yValue, transform.position.z);
	}

	void Update()
	{
		// Popping out of the ground 
		if(isJump && canJump && !isDead && !spawner.inBuilding) {
			sphereCollider.enabled = false;

			foreach(var m in mesh) {
				m.enabled = true;
			}
			foreach(var s in skinned) {
				s.enabled = true;
			}

			animation.CrossFadeQueued("Appear");

			this.gameObject.renderer.enabled = true;
			transform.position = new Vector3 (transform.position.x, 
			                                  player.transform.position.y,
			                                  transform.position.z);
			if(!isAwake){
				playParticles = true;
				if(playParticles) {
					particles.Play();
				}
				transform.LookAt(target);
			}

			if(gameObject.transform.position.y > player.transform.position.y) {
				isJump = false;
				isAwake = true;
			}
		}

		if(currentState == State.Roam && !isDead) {
			speed = 3.0f;
			target = GameObject.FindGameObjectWithTag("Player").transform;

			Vector3 direction = target.transform.position - transform.position;
			direction.y = 0.0f;

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
			foreach (var m in mesh) {
				m.enabled = false;
			}
			
			foreach (var s in skinned) {
				s.enabled = false;
			}

			sphereCollider.enabled = true;
			isAwake = false;

			transform.position = new Vector3 (transform.position.x, player.transform.position.y - yOffset, transform.position.z);

			if(gameObject.transform.position.y < player.transform.position.y - yOffset) {
				gameObject.rigidbody.isKinematic = true;

				if(!isDead) {
					currentState = State.Chase;
				}
			}
		}
	
		// Chase State (if the player are within range)
		if(currentState == State.Chase && !isDead) {
			foreach (var m in mesh) {
				m.enabled = false;
			}
			
			foreach (var s in skinned) {
				s.enabled = false;
			}
			speed = 4.5f;
			target = GameObject.FindGameObjectWithTag ("Player").transform;

			// Get the direction to travel in by subtracting the target's position with the monster's position. Set y to a negative value
			Vector3 direction = target.transform.position - transform.position;
			direction.y = 0.0f;

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
				
				if(gameObject.transform.position.y < (player.transform.position.y - yOffset)) {
					gameObject.rigidbody.isKinematic = true;
					
					if(!isDead) {
						Vector3 moveDirection = direction.normalized * speed * Time.deltaTime;
						transform.position += moveDirection;
					}
				}
			}
		}

		if(currentState == State.Attack) {
			animation.CrossFade("Attack");
			// Shoot a raycast forward, and if it's a hit - blow ice onto the player
			RaycastHit hit;
			if(Physics.Raycast(transform.position, Vector3.forward, out hit, attackDistance)) {
				if(hit.transform.gameObject == GameObject.FindWithTag("Player")) {
					player.GetComponent<Health>().loseHealth = true;
				}
			}

			Vector3 distance = target.transform.position - transform.position;

			if(distance.magnitude > attackDistance + 5.0f) {
//				smallWormAnimator.SetTrigger("Burrow");
				currentState = State.Idle;
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