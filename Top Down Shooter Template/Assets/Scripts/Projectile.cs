using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
	float speed = 10;

	public void SetSpeed(float newSpeed) //Able to change speed at will
	{
		speed = newSpeed;
	}

	void Update () 
	{
		transform.Translate (Vector3.forward * Time.deltaTime * speed); //Makes Projectile move forward
	}
}
