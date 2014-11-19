using UnityEngine;
using System.Collections;

public class animations_script : MonoBehaviour {

	void Start () {
		animation.wrapMode =WrapMode.Loop;
		
		//animation["idle"].wrapMode=WrapMode.Clamp;
	}
	
	void Update () {
		animation["walk_idle"].wrapMode=WrapMode.PingPong;
		animation.CrossFade("walk_idle");

		animation ["fire"].wrapMode = WrapMode.ClampForever;
		animation ["igniter"].wrapMode = WrapMode.ClampForever;
		animation ["reload"].wrapMode = WrapMode.Once;

		animation["igniter"].speed = 2.0f;
		animation["fire"].speed = 2.5f;

		if (Input.GetMouseButton(1))
			animation.CrossFade ("igniter");

		if (Input.GetMouseButton(0))
			animation.CrossFade ("fire");

		if (Input.GetKey(KeyCode.R))
			StartCoroutine(reload());

	}

	IEnumerator reload()
	{
		//animation.CrossFade("reload");
		animation.Play("reload");
		yield return new WaitForSeconds(animation["reload"].length);
		Debug.Log("This happens 2 seconds later. Tada.");
	}
}