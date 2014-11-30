using UnityEngine;
using System.Collections;

public class LargeMonsterAI : MonoBehaviour 
{
	public State currentState;
	public float huntDistance;
	public float chaseDistance;
	public float attackDistance;
	public float speed;
	public float posTimer;
	public float pop;
	public float maxPop;
	public float minPop;
	public Vector3 lastKnownPos;
	public Transform target;
	public bool getPos;
	public bool isJump;
	public bool isAwake;
	public bool canJump;
	public bool playParticles;

	private GameObject player;
	private SphereCollider sphereCollider;
	private ParticleSystem particles;

	public RaycastHit ground;

	public enum State {
		Hunt,
		Chase
	};

	void Awake()
	{
		sphereCollider = this.gameObject.GetComponent<SphereCollider>();
	}

	void Start()
	{
		huntDistance = 50.0f;
		chaseDistance = 30.0f;
		attackDistance = 5.0f;
		speed = 3.0f;
		posTimer = 0.0f;
		getPos = false;
		isJump = false;
		isAwake = false;
		canJump = false;

		currentState = State.Hunt;

		gameObject.rigidbody.isKinematic = true;
		gameObject.collider.enabled = false;

		player = GameObject.FindGameObjectWithTag("Player");
		sphereCollider.radius = chaseDistance;
	}

	void Update()
	{
		// Popping out of the ground 
		if(isJump && canJump) {
			if(Physics.Raycast(transform.position, Vector3.up, out ground, 2.0f)) {
				if(ground.transform.tag == "Ground") {
					minPop = ground.transform.position.y - 2.0f;
					maxPop = ground.transform.position.y + 0.3f;
				}
			}
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

		if(currentState == State.Hunt) {
			speed = 3.0f;
			target = null;

			posTimer += 1.0f * Time.deltaTime;

			if(posTimer >= 10.0f) {
				getPos = true;
			}

			if(getPos) {
				lastKnownPos = player.transform.position;
				getPos = false;
				posTimer = 0.0f;
			}

			Vector3 direction = lastKnownPos - transform.position;
			transform.LookAt(lastKnownPos);

			if(direction.magnitude > chaseDistance) {
				Vector3 moveVector = direction.normalized * speed * Time.deltaTime;
				transform.position += moveVector;
			}
			if(direction.magnitude < chaseDistance) {
				currentState = State.Chase;
			}
		}

		if(currentState == State.Chase) {
			speed = 8.0f;

			target = player.gameObject.transform;
			Vector3 direction = target.transform.position - transform.position;
			transform.LookAt(target);

			if(direction.magnitude > attackDistance) {
				Vector3 moveVector = direction.normalized * speed * Time.deltaTime;
				transform.position += moveVector;
			}

			if(direction.magnitude < attackDistance) {
				speed = 2.0f;
				player.GetComponent<Health>().loseHealth = true;
			}
		}
	}
}