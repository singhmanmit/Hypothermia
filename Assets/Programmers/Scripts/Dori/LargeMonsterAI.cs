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
	public Vector3 lastKnownPos;
	public Transform target;
	public bool getPos;
	public bool isAttack;
	public bool isMiss;
	public bool playParticles;

	private GameObject player;
	private SphereCollider sphereCollider;
	private float yValue;
	private MeshRenderer[] mesh;
	private SkinnedMeshRenderer[] skinned;

	private float xPos;
	private float zPos;

	public RaycastHit ground;

	public enum State {
		Hunt,
		Chase
	};

	void Awake()
	{
		sphereCollider = this.gameObject.GetComponent<SphereCollider>();
		mesh = GetComponentsInChildren<MeshRenderer> ();
		skinned = GetComponentsInChildren<SkinnedMeshRenderer> ();
	}

	void Start()
	{
		huntDistance = 50.0f;
		chaseDistance = 45.0f;
		attackDistance = 15.0f;
		speed = 3.0f;
		posTimer = 0.0f;
		getPos = false;
		isAttack = false;
		isMiss = false;

		currentState = State.Hunt;

		gameObject.rigidbody.isKinematic = true;
		gameObject.collider.enabled = false;
		sphereCollider.enabled = true;

		foreach (var m in mesh) {
			m.enabled = false;
		}

		foreach (var s in skinned) {
			s.enabled = false;
		}

		player = GameObject.FindGameObjectWithTag("Player");
		sphereCollider.radius = chaseDistance;

		yValue = player.transform.position.y - 30.0f;
		transform.position = new Vector3 (transform.position.x, yValue, transform.position.z);
	}

	void Update()
	{
		if(isMiss) {
			animation.Stop ("Attack");
			animation.Play ("Miss And Burrow");
			currentState = State.Hunt;

			if(!animation.isPlaying) {
				animation.Stop ("Miss and Burrow");
			}

			foreach (var m in mesh) {
				m.enabled = false;
			}
			
			foreach (var s in skinned) {
				s.enabled = false;
			}
		}

		if(isAttack){
			foreach(var m in mesh) {
				m.enabled = true;
			}
			foreach(var s in skinned) {
				s.enabled = true;
			}

			transform.LookAt(player.transform);
			transform.position = new Vector3 (xPos, 
			                                  player.transform.position.y,
			                                  zPos);

			animation.Play ("Attack");
		}

		if(currentState == State.Hunt) {
			speed = 3.0f;

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
			direction.y = 0.0f;

			if(direction.magnitude > chaseDistance) {
				Vector3 moveVector = direction.normalized * speed * Time.deltaTime;
				transform.position += moveVector;
			}
			if(direction.magnitude < chaseDistance) {
				currentState = State.Chase;
			}
		}

		if(currentState == State.Chase) {
			speed = 10.0f;

			target = player.gameObject.transform;
			Vector3 direction = target.transform.position - transform.position;

			if(direction.magnitude > attackDistance) {
				Vector3 moveVector = direction.normalized * speed * Time.deltaTime;
				transform.position += moveVector;
			}

			if(direction.magnitude < attackDistance) {
				xPos = player.transform.position.x + 50.0f;
				zPos = player.transform.position.z + 50.0f;
				isAttack = true;
				speed = 0.0f;
				player.GetComponent<Health>().loseHealth = true;
			}
		}
	}
}