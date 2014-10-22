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
		Chase,
		Attack
	};

	public Transform target;

	void Start()
	{
		// Setting the variables
		maxNumOfLargeMonsters = 1;
		spawnTimer = 0.0f;

		// Set the player as the target
		target = GameObject.FindWithTag ("Player").transform;
	}

	void Update()
	{
		// "Sight" raycast
		RaycastHit hit;
		if(Physics.Raycast(transform.position, Vector3.forward, out hit, sightDistance)) {
			if(hit.transform.gameObject == GameObject.FindWithTag ("Player")) {

			}
		}

		// "Hearing" raycasts - should be a Physics.SphereCast
	}
}