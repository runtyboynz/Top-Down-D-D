using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCollider : MonoBehaviour 
{

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	void OnCollisionEnter (Collision collisionInfo)
	{
		Debug.Log (collisionInfo.collider.name);
		/*if (other.tag == "PC1BodyColliderTag") 
		{
			Debug.Log ("Touch");
		}*/
	}
}
