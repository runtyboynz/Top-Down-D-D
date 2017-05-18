using UnityEngine;
using System.Collections;

[RequireComponent (typeof (PlayerController))] //Automatically gives the object a player controller
[RequireComponent (typeof (WeaponController))]
public class Player : MonoBehaviour {

	public float moveSpeed = 5;

	PlayerController controller;
	Camera viewCamera;
	WeaponController weaponController;

	void Start () 
	{
		controller = GetComponent<PlayerController> (); 
		weaponController = GetComponent<WeaponController> (); 
		viewCamera = Camera.main;
	}

	void Update () 
	{
		//Movement Input

		//Next 3 lines set movement of player
		Vector3 moveInput = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical")); //"Raw" takes away any default smoothing (Which is great!!!!)
		Vector3 moveVelocity = moveInput.normalized * moveSpeed; //"normalized" just gives the direction of the input
		controller.Move (moveVelocity);

		//Look Input

		//Raycast to make player look in direction. 
		Ray ray = viewCamera.ScreenPointToRay (Input.mousePosition); //Gives screen position (mouse position), return a ray from the camera to that position to infinity.
		Plane groundPlane = new Plane (Vector3.up,Vector3.zero);//Generates a plane for mouse to use
		float rayDistance;

		if (groundPlane.Raycast (ray, out rayDistance)); //Gives OUT raydistance then assigns it a variable. Will return true if the ray itersects with the ground plane and will also know length.
		{
			Vector3 point = ray.GetPoint (rayDistance); //returns the point of intersection.
			//Debug.DrawLine (ray.origin, point, Color.red);
			controller.LookAt(point);//Player looks at ray point.
		}

		//Weapon Right Hand Input

		if (Input.GetMouseButton(1))//If right mouse button is pushed down, shoot 
		{
			weaponController.ShootRH ();
		}

		//Weapon Left Hand Input

		if (Input.GetMouseButton(0))//If left mouse button is pushed down, shoot 
		{
			weaponController.ShootLH ();
		}

	}
}
