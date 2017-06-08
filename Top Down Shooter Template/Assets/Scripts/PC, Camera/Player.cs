using UnityEngine;
using System.Collections;

[RequireComponent (typeof (PlayerController))] //Automatically gives the object a player controller
[RequireComponent (typeof (WeaponController))]
public class Player : MonoBehaviour 
{

	public Transform motionPoint;//To get the character to move with the camera center point, aka camera.
	public Transform farPointAimerOrigin, farPointAimerReach;//For Object to follow ray point.
	public Transform closePointAim;//To register close attacks.

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
		moveInput = motionPoint.rotation * moveInput; //Connects with Motion point object for setting up movement with camera correctly.
		Vector3 moveVelocity = moveInput.normalized * moveSpeed; //"normalized" just gives the direction of the input.
		controller.Move (moveVelocity);

		//Look Input

		//Raycast to make player look in direction. 
		Ray ray = viewCamera.ScreenPointToRay (Input.mousePosition); //Gives screen position (mouse position), return a ray from the camera to that position to infinity.
		Plane groundPlane = new Plane (Vector3.up,Vector3.zero);//Generates a plane for mouse to use
		float rayDistance;

		if (groundPlane.Raycast (ray, out rayDistance)); //Gives OUT raydistance then assigns it a variable. Will return true if the ray itersects with the ground plane and will also know length.
		{
			Vector3 point = ray.GetPoint (rayDistance); //returns the point of intersection.
			Debug.DrawLine (ray.origin, point, Color.red);
			controller.LookAt(point);//Player looks at ray point.

			//AIMING IN CLOSE COMBAT: Here we use two game objects to measure the x,y,z distance for how large/small your attack will be.
			farPointAimerOrigin.LookAt (motionPoint);//looks at player/motion/center point

			farPointAimerReach.position = ray.GetPoint (rayDistance);//Makes Reach Object follow mouse/camera ray.
			farPointAimerReach.LookAt (motionPoint);//looks at player/motion/center point

			//SECONDARY RAY: For Finishing up Combat mechanics + Enabling 1st Person.
			closePointAim.LookAt (farPointAimerReach.position);
		}

		//Weapon Right Hand Input

		if (Input.GetMouseButtonDown(1))//If right mouse button is clicked (For Aiming Origin and Reach Gameobjects to register)
		{
			farPointAimerOrigin.transform.position = new Vector3 (farPointAimerReach.transform.position.x, farPointAimerReach.transform.position.y, farPointAimerReach.transform.position.z);
			farPointAimerOrigin.localRotation = Quaternion.Euler (0,farPointAimerOrigin.localRotation.y,0);//stops object from tilting when closer to player.
			farPointAimerReach.localRotation = Quaternion.Euler (0,farPointAimerReach.localRotation.y,0);//stops object from tilting when closer to player.
			//^^Sends Origin Gameobject to Reach Object position to measure the distance so Reach Object can measure how far away it moves from the origin point^^.
		}

		if (Input.GetMouseButtonUp(1))//If right mouse button is unclicked (For Aiming Origin to go back to default)
		{
			farPointAimerOrigin.transform.position = transform.TransformPoint (Vector3.zero);//Snaps Origin point back to Default position
		}

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
