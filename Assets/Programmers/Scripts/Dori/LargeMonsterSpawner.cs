using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LargeMonsterSpawner : MonoBehaviour 
{
	public List<GameObject> largeMonsters = new List<GameObject>();
	public GameObject largeMonster;
	public Vector3 spawn;

	private GameObject[] spawnLoc;

	void Start()
	{
		spawnLoc = GameObject.FindGameObjectsWithTag("LargeMonsterSpawn");

		StartCoroutine("Spawn");
	}

	IEnumerator Spawn()
	{
		int index = Random.Range(0, 3);
		spawn = spawnLoc[index].transform.position;
		spawn.y = -50.0f;

		yield return new WaitForSeconds(5);

		if(largeMonsters.Count < 1) {
			GameObject clone = Instantiate(largeMonster, spawn, transform.rotation) as GameObject;
			clone.name = "Large Monster " + largeMonsters.Count;
			clone.tag = "LargeMonster";
			largeMonsters.Add (clone);
		}

		StopCoroutine("Spawn");
	}
}