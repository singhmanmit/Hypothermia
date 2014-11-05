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
		maxSmallMonsters = 5;
		spawnTimer = 0.0f;

		// Grab a new position and translate the Spawner there at the start of the script
		GetNewPosition ();
		transform.Translate (newPosition);
	}

	void Update()
	{
		if(canSpawn) {
			// Start the spawn timer
			spawnTimer += 1.0f * Time.deltaTime;

			// Spawn a new enemy every five seconds and only if the current count is below the max number allowed
			if(spawnTimer >= 5.0f && smallMonsters.Count <= maxSmallMonsters) {
				GameObject clone = Instantiate (monster, transform.position, transform.rotation) as GameObject;
				clone.name = "Small Monster " + smallMonsters.Count; // Rename the monster for removal later
				smallMonsters.Add(clone); // Add the clone to the list of monsters
				GetNewPosition();
				transform.Translate (newPosition);
				spawnTimer = 0; // Reset the timer
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

	public Vector3 GetNewPosition()
	{
		transform.Translate(player.transform.position); // Reset the spawn position to the position of the player

		// Set a new random position using the player's coordinates
		newPosition = new Vector3 (Random.Range (player.transform.position.x / 2, player.transform.position.x / 2),
		                           -2.0f, 
		                           Random.Range (-player.transform.position.z / 2, -player.transform.position.z / 2));
		return newPosition;
	}

	void OnTriggerStay(Collider other)
	{
		if (other.gameObject == GameObject.FindWithTag ("Obstacle")) {
			canSpawn = false;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject == GameObject.FindWithTag("Obstacle")) {
			canSpawn = true;
		}
	}
}