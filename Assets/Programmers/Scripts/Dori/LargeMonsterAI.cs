/* This script is responsible for controlling the large monster.
 * Will utilize raycasts to detect the player through "sight" and "sound".
 * Will also continually hunt the player throughout the map.
*/

using UnityEngine;
using System.Collections;

public class LargeMonsterAI : MonoBehaviour 
{
	// Declaring variables and set in Start() to allow for flexibility 
	public int maxNumOfLargeMonsters;
	public float spawnTimer;
	public float audioDistance;
	public float sightDistance;
	public State currentState;

	public bool inSight;
	public bool inAudio;

	public enum State {
		Idle,
		Roam,
		Chase,
		Attack
	};

	public Transform target;

	void Start()
	{
		// Setting the variables
		maxNumOfLargeMonsters = 1;
		spawnTimer = 0.0f;
		audioDistance = 15.0f;

		target = null;
		currentState = State.Roam;
	}

	void Update()
	{
		// "Sight" raycast
		RaycastHit hit;
		if(Physics.Raycast(transform.position, Vector3.forward, out hit, sightDistance)) {
			if(hit.transform.gameObject == GameObject.FindWithTag ("Player")) {
				currentState = State.Chase;
				target = hit.transform.gameObject.transform;
			}
			else {
				currentState = State.Roam;
			}
		}

		// "Hearing" raycasts - should be a Physics.SphereCast
		if(Physics.SphereCast(transform.position, audioDistance, transform.forward, out hit, 10)) {
			if(hit.transform.gameObject == GameObject.FindWithTag ("Player")) {
				currentState = State.Chase;
				target = hit.transform.gameObject.transform;
			}
		}

		// Chase player when player enters key areas

	}
}