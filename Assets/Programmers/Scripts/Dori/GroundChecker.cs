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
		if(other.gameObject == GameObject.FindWithTag ("Hut")) {
			this.gameObject.transform.parent.GetComponent<SmallMonsterAI>().canJump = false;
			this.gameObject.transform.parent.GetComponent<Animation>().animation.Stop();
		}
	}

	void OnTriggerExit(Collider other) 
	{
		if(other.gameObject == GameObject.FindWithTag("Hut")) {
			this.gameObject.transform.parent.GetComponent<SmallMonsterAI>().canJump = true;
		}
	}
}