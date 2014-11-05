using UnityEngine;
using System.Collections;

public class GroundChecker : MonoBehaviour 
{
	void Start()
	{
		this.gameObject.transform.parent.GetComponent<SmallMonsterAI>().canJump = true;
	}

	void OnTriggerStay(Collider other)
	{
		if(other.gameObject == GameObject.FindWithTag ("Obstacle")) {
			this.gameObject.transform.parent.GetComponent<SmallMonsterAI>().canJump = false;
		}
	}

	void OnTriggerExit(Collider other) 
	{
		if(other.gameObject == GameObject.FindWithTag("Obstacle")) {
			this.gameObject.transform.parent.GetComponent<SmallMonsterAI>().canJump = true;
		}
	}
}