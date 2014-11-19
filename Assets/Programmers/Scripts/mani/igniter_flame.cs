using UnityEngine;
using System.Collections;

public class igniter_flame : MonoBehaviour {

	protected bool bothclicked;
	protected bool letPlay;

	// Use this for initialization
	void Start () {
		letPlay = false;
		bothclicked = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(1))
		{
			letPlay = true;
		}

		if(Input.GetMouseButtonUp(1))
		{
			letPlay = false;
		}

		if(letPlay)
		{
			if(!gameObject.particleSystem.isPlaying)
			{
				gameObject.particleSystem.Play();
			}
		}else{
			if(gameObject.particleSystem.isPlaying)
			{
				gameObject.particleSystem.Stop();
			}
		}
	}
}
