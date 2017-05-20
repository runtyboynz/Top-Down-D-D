using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]
public class PlayerController : MonoBehaviour {

	Vector3 velocity;
	Rigidbody myRigidBody;

	void Start () 
	{
		myRigidBody = GetComponent<Rigidbody> ();
	
	}

	public void Move (Vector3 _velocity)
	{
		velocity = _velocity;
	}

	public void LookAt(Vector3 lookPoint)
	{
		Vector3 adjustPointHeight = new Vector3 (lookPoint.x, transform.position.y, lookPoint.z);//Adjusts ray point which is on the ground to player eye height.
		transform.LookAt (adjustPointHeight);//makes the player look at the camera/mouse ray point.
	}

	public void FixedUpdate()
	{
		myRigidBody.MovePosition (myRigidBody.position + velocity * Time.fixedDeltaTime);
	}
}
