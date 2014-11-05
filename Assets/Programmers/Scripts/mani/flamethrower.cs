using UnityEngine;
using System.Collections;

public class flamethrower : MonoBehaviour {

	// Use this for initialization
	void Start () {
		letPlay = false;
	}
	

	protected bool letPlay;
	
	public void Update()
	{
		if(Input.GetMouseButtonDown(0))
		{
			letPlay = !letPlay;
		}

		if(Input.GetMouseButtonDown(1))
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
				gameObject.particleSystem.Clear();
			}
		}
	}
}
