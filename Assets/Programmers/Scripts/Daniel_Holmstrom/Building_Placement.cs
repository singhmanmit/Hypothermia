using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Building_Placement : MonoBehaviour {
	public List<GameObject> Cabins;
	public List<GameObject> Tents;
	//Randomization variables
	List<GameObject> CB;
	Vector3 Distance;
	public int LowestNumberOfBuildings;
	public int HighestNumberOfBuildings;
	public float MinimumDistance;
	public float MaximumDistance;
	public int ChanceOutOfHundredForTent = 40;
	//Number of Buildings to be made
	int nob;

	//GameObject with the script to make interiors of buildings
	public GameObject InteriorSpawnObject;

	// Use this for initialization
	void Start () {
		//avoids null values
		Distance = new Vector3();
		CB = new List<GameObject>();

		//Assigns a random seed for randomization
		Random.seed = (int)System.DateTime.Now.Ticks;
		//determines the number of buildings to be made
		nob=Random.Range(LowestNumberOfBuildings,HighestNumberOfBuildings);

		//checks if the buildings should be a tent
		if(Random.Range(1,100)<=ChanceOutOfHundredForTent){
			CreateDistance(MinimumDistance,MaximumDistance);
			CB.Add((GameObject)(GameObject.Instantiate(Tents[Random.Range(0,Tents.Count-1)],transform.position + Distance, transform.rotation)));
		}
		//makes the buliding a Cabin if check failed 
		else{
			CreateDistance(MinimumDistance,MaximumDistance);
			CB.Add((GameObject)(GameObject.Instantiate(Cabins[Random.Range(0,Tents.Count-1)],transform.position + Distance, transform.rotation)));
		}

		//Checks if the number of buildings has reached the limit 
		for(int i=0; i<nob-1;i++){
			//creates a temporary index that selects the branching point
			int tempInd = Random.Range(0,CB.Count-1);
			if(Random.Range(1,100)<=ChanceOutOfHundredForTent){
				CreateDistance(MinimumDistance,MaximumDistance);
				CB.Add((GameObject)(GameObject.Instantiate(Tents[Random.Range(0,Tents.Count-1)],CB[tempInd].transform.position + Distance, transform.rotation)));
			}
			//makes the buliding a Cabin if check failed 
			else{
				CreateDistance(MinimumDistance,MaximumDistance);
				CB.Add((GameObject)(GameObject.Instantiate(Cabins[Random.Range(0,Tents.Count-1)],CB[tempInd].transform.position + Distance, transform.rotation)));
			}
		}
		InteriorSpawnObject =(GameObject)GameObject.Instantiate(InteriorSpawnObject);
		Interior_Spawn tempS = InteriorSpawnObject.GetComponent<Interior_Spawn>();
		for(int i=0; i<CB.Count; i++){
			tempS.Buildings.Add(CB[i].GetComponent<Building_Node>());
		}
	}
	
	// Update is called once per frame
	void Update () {
	}

	void CreateDistance(float x, float z){
		//temp stores the total distance value as a temporary value
		float temp;
		//determines the total distance from the branch point
		temp=Random.Range(x,z);
		//assigns a portion of the distance to the X axis
		Distance.x = Random.Range (0,temp);
		//assigns the rest of the distance to the Z axis
		Distance.z = temp - Distance.x;
		//decides if the X distance should be negitive
		if(Random.Range(0,100)>=50){
			Distance.x = Distance.x * (-1);
		}
		//decides if the Z distance should be negitive, slightly different conditions for hopefully more randomness
		if(Random.Range(0,100)<=50){
			Distance.z = Distance.z * (-1);
		}
	}
}
