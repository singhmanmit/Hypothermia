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

	private GameObject player;
	private SphereCollider sphereCollider;

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

		currentState = State.Hunt;

		player = GameObject.FindGameObjectWithTag("Player");
		sphereCollider.radius = chaseDistance;
	}

	void Update()
	{
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