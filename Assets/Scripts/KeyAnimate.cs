using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyAnimate : MonoBehaviour {

	public float rotationSpeed = 45;

	void Start () 
	{
		
	}
	
	void Update () 
	{
		transform.Rotate (0, 0, rotationSpeed * Time.deltaTime);

	}
}
