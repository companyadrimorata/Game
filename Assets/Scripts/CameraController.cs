using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public Transform player;

	Rigidbody2D _rigidbody;

	void Start () 
	{
		_rigidbody = GetComponent<Rigidbody2D> ();
	}
	
	void Update () 
	{
		_rigidbody.transform.position = player.transform.position;
	}
}
