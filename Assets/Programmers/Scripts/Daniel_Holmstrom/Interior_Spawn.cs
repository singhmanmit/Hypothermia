using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Interior_Spawn : MonoBehaviour {
	public Tent tentobjects;
	public Cabin CabinObjects;
	public List<Building_Node> Buildings;

	// Use this for initialization
	void Start () {
		
		Random.seed = (int)System.DateTime.Now.Ticks;

		for(int i=0; i<Buildings.Count; i++){
			for(int j=0; j<Buildings[i].Nodes.Count; j++){
				if(Buildings[i].tag=="Cabin"){
					if(Buildings[i].Nodes[j].IsWallObject==false){
						if(Buildings[i].Nodes[j].IsStandingObject==false){
							GameObject.Instantiate(CabinObjects.NormalObjects[Random.Range(0,CabinObjects.NormalObjects.Count)],
							                       Buildings[i].Nodes[j].transform.position, Buildings[i].Nodes[j].transform.rotation);
							Destroy(Buildings[i].Nodes[j].gameObject);
						}
						else{
							GameObject.Instantiate(CabinObjects.StandingObjects[Random.Range(0,CabinObjects.StandingObjects.Count)],
							                       Buildings[i].Nodes[j].transform.position, Buildings[i].Nodes[j].transform.rotation);
							//Destroy(Buildings[i].Nodes[j].gameObject);
						}
					}
					else{
						GameObject.Instantiate(CabinObjects.WallObjects[Random.Range(0,CabinObjects.WallObjects.Count)],
						                       Buildings[i].Nodes[j].transform.position, Buildings[i].Nodes[j].transform.rotation);
						//Destroy(Buildings[i].Nodes[j].gameObject);
					}
				}
				else{
					GameObject.Instantiate(tentobjects.TentObjects[Random.Range(0,tentobjects.TentObjects.Count)],
					                       Buildings[i].Nodes[j].transform.position, Buildings[i].Nodes[j].transform.rotation);
					//Destroy(Buildings[i].Nodes[j].gameObject);
				}
			}
		}
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
[System.Serializable]
public class Cabin{
	public List<GameObject> NormalObjects;
	public List<GameObject> StandingObjects;
	public List<GameObject> WallObjects;
}

[System.Serializable]
public class Tent{
	public List<GameObject> TentObjects;
}