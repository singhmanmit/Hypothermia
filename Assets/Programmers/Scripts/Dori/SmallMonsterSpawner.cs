/* This script is responsible for spawning the little monsters in the 
 * vicinity of the player. It randomly repositions itself at the start
 * of its lifetime and after each monster has been spawned. 
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SmallMonsterSpawner : MonoBehaviour 
{
	private GameObject player;

	public bool inBuilding;
	public Vector3 newPosition;
	public Transform spawner;
	public bool canSpawn;
	public float spawnTimer;
	public GameObject monster; // Prefab
	public int maxSmallMonsters; // Max number of small monsters allowed

	// Two lists for storing the small monsters in the game
	public List<GameObject> smallMonsters = new List<GameObject>();

	void Awake()
	{
		// Set up references
		player = GameObject.FindWithTag ("Player");
	}

	void Start()
	{
		// Set up variables
		maxSmallMonsters = 2;
		spawnTimer = 3.0f;
		inBuilding = false;

		InvokeRepeating("Spawn", spawnTimer, spawnTimer);
	}

	void Spawn()
	{
		if(canSpawn) {
			// Spawn a new enemy every five seconds and only if the current count is below the max number allowed
			if(smallMonsters.Count <= maxSmallMonsters) {
				GetNewPosition();
				GameObject clone = Instantiate (monster, newPosition, transform.rotation) as GameObject;
				clone.name = "Small Monster " + smallMonsters.Count; // Rename the monster for removal later
				smallMonsters.Add(clone); // Add the clone to the list of monsters
			} 

			// Set canSpawn to false if there are the max number of monsters on the field
			if(smallMonsters.Count >= maxSmallMonsters) {
				canSpawn = false;
			}
		}

		if(smallMonsters.Count < 5) {
			canSpawn = true;
		}
	}

	public void Remove(string _monster)
	{
		// Remove the monster from circulation if dead
		smallMonsters.Remove (GameObject.Find (_monster));
	}

	Vector3 GetNewPosition()
	{
		newPosition = new Vector3 (player.transform.localPosition.x + Random.Range (-50.0f, 50.0f), 
		                           player.transform.position.y - 50.0f, 
		                           player.transform.localPosition.z + Random.Range (-50.0f, 50.0f));
		return newPosition;
	}

	void OnTriggerStay(Collider other)
	{
		if (other.gameObject == GameObject.FindWithTag ("Hut")) {
			canSpawn = false;
			inBuilding = true;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject == GameObject.FindWithTag("Hut")) {
			canSpawn = true;
			inBuilding = false;
		}
	}
}