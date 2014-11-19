using UnityEngine;
using System.Collections;

public class flamethrower : MonoBehaviour {
	
	protected bool bothclicked;
	protected bool letPlay;

	// Use this for initialization
	void Start () {
		letPlay = false;
		bothclicked = false;
	}
	
	public void Update()
	{
		if(Input.GetMouseButton(1))
		{
			bothclicked = true;
		}

		if(Input.GetMouseButtonDown(0))
		{
			if(bothclicked)
			{
				letPlay = true;
			}
		}

		if (Input.GetMouseButton(0) &&Input.GetMouseButton(1)) 
		{
			letPlay = true;
		}

		if(Input.GetMouseButtonUp(1))
		{
			bothclicked = false;
		}

		if(Input.GetMouseButtonUp(0))
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
