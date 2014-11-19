using UnityEngine;
using System.Collections;

public class Foundation : MonoBehaviour {
	public Collider FloorCheck;
	public GameObject FoundationBlock;

	// Use this for initialization
	void Start(){
		if(FloorCheck==null){
			Debug.Log("One of the buildings is missing the FloorCheck collider");
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other){
		if(other.gameObject.tag=="Floor"){
			GameObject.Instantiate(FoundationBlock, transform.position, transform.rotation);
			gameObject.transform.position += new Vector3(0,2,0);
		}
	}

}
