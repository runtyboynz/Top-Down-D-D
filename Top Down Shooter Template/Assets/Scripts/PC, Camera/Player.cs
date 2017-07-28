using UnityEngine;
using System.Collections;

[RequireComponent (typeof (PlayerController))] //Automatically gives the object a player controller
[RequireComponent (typeof (WeaponController))]
public class Player : MonoBehaviour 
{

	public Transform motionPoint;//To get the character to move with the camera center point, aka camera.

	public Transform farPointAimerOriginRH, farPointAimerOriginLH, farPointAimerReachRH, farPointAimerReachLH;//For Object to follow ray point.

	public float farPointDefaultAimerReachY = 1.0f;//Sets default Aimer Reach Sphere Y axis.

	public Transform closePointAim;//To register close attacks.
	public Transform combatAimer;//The 1st/3rd Person Sphere that pc aims/reaches at/to when attacking.

	//public Plane thirdPersonPlane;
	//public Plane firstPersonPlane;
	public RaycastHit rayHitPcLook; //Ray that collides with in-game colliders for pc to look towards and interact with.
	//public RaycastHit rayHitPcAttack; //Ray that collides with Quad to work out the attack direction.

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

		if (Physics.Raycast (ray, out rayHitPcLook)) //Gives OUT raydistance then assigns it a variable. Will return true if the ray itersects with a collider and will also know length.
		{
			
			Debug.DrawLine (ray.origin, rayHitPcLook.point, Color.red);
			controller.LookAt(rayHitPcLook.point);//Player looks at ray point.

			//AIMING IN CLOSE COMBAT: Here we use two game objects to measure the x,y,z distance for how large/small your attack will be.
			farPointAimerOriginRH.LookAt (motionPoint);//looks at player/motion/center point RH
			farPointAimerOriginLH.LookAt (motionPoint);//looks at player/motion/center point LH

			farPointAimerReachRH.position = rayHitPcLook.point;//Makes Reach Object follow mouse/camera ray RH.
			farPointAimerReachLH.position = rayHitPcLook.point;//Makes Reach Object follow mouse/camera ray LH.

			farPointAimerReachRH.transform.position = new Vector3 (farPointAimerReachRH.transform.position.x, farPointAimerReachRH.transform.position.y + farPointDefaultAimerReachY, farPointAimerReachRH.transform.position.z);//Sets Mouse Aim Reach at higher Y, so he looks upward while freelooking RH.
			farPointAimerReachLH.transform.position = new Vector3 (farPointAimerReachLH.transform.position.x, farPointAimerReachLH.transform.position.y + farPointDefaultAimerReachY, farPointAimerReachLH.transform.position.z);//Sets Mouse Aim Reach at higher Y, so he looks upward while freelooking LH.

			farPointAimerReachRH.LookAt (motionPoint);//looks at player/motion/center point RH
			farPointAimerReachLH.LookAt (motionPoint);//looks at player/motion/center point LH

			//SECONDARY RAY: For Finishing up Combat mechanics + Enabling 1st Person.
			closePointAim.LookAt (farPointAimerReachRH);//Makes Hands/attacks aim at the combat aimer reach which it will reach/aim at/to.
		}


		//Weapon Right Hand Input

		if (Input.GetMouseButton(1))//If right mouse button is pushed down, shoot 
		{
			farPointAimerReachRH.transform.position = new Vector3 (farPointAimerReachRH.transform.position.x, farPointAimerReachRH.transform.position.y - farPointDefaultAimerReachY, farPointAimerReachRH.transform.position.z);//Resets Mouse Aim Sphere to default Y position RH.
			farPointAimerReachRH.localRotation = Quaternion.Euler (0,farPointAimerReachRH.localRotation.y,0);//stops object from tilting when closer to player.
			weaponController.ShootRH ();
		}

		if (Input.GetMouseButtonDown(1))//If right mouse button is clicked (For Aiming Origin and Reach Gameobjects to register)
		{
			farPointAimerOriginRH.transform.position = new Vector3 (farPointAimerReachRH.transform.position.x, farPointAimerReachRH.transform.position.y, farPointAimerReachRH.transform.position.z);//Sets Origin Aimer at the exact point Reach aim is when mouse is pushed down.
			farPointAimerOriginRH.localRotation = Quaternion.Euler (0,farPointAimerOriginRH.localRotation.y,0);//stops object from tilting when closer to player.
			//^^Sends Origin Gameobject to Reach Object position to measure the distance so Reach Object can measure how far away it moves from the origin point^^.
		}

		if (Input.GetMouseButtonUp(1))//If right mouse button is unclicked (For Aiming Origin to go back to default)
		{
			farPointAimerOriginRH.transform.position = transform.TransformPoint (Vector3.zero);//Snaps Origin point back to Default position
		}


		//Weapon Left Hand Input

		if (Input.GetMouseButton(0))//If left mouse button is pushed down, shoot 
		{
			farPointAimerReachLH.transform.position = new Vector3 (farPointAimerReachLH.transform.position.x, farPointAimerReachLH.transform.position.y - farPointDefaultAimerReachY, farPointAimerReachLH.transform.position.z);//Resets Mouse Aim Sphere to default Y position LH.
			farPointAimerReachLH.localRotation = Quaternion.Euler (0,farPointAimerReachLH.localRotation.y,0);//stops object from tilting when closer to player.
			weaponController.ShootLH ();
		}

		if (Input.GetMouseButtonDown(0))//If left mouse button is clicked (For Aiming Origin and Reach Gameobjects to register)
		{
			farPointAimerOriginLH.transform.position = new Vector3 (farPointAimerReachLH.transform.position.x, farPointAimerReachLH.transform.position.y, farPointAimerReachLH.transform.position.z);//Sets Origin Aimer at the exact point Reach aim is when mouse is pushed down.
			farPointAimerOriginLH.localRotation = Quaternion.Euler (0,farPointAimerOriginLH.localRotation.y,0);//stops object from tilting when closer to player.
			//^^Sends Origin Gameobject to Reach Object position to measure the distance so Reach Object can measure how far away it moves from the origin point^^.
		}

		if (Input.GetMouseButtonUp(0))//If left mouse button is unclicked (For Aiming Origin to go back to default)
		{
			farPointAimerOriginLH.transform.position = transform.TransformPoint (Vector3.zero);//Snaps Origin point back to Default position
		}
	}
}

//private void SendRayToPlayer () {
	/*
  RaycastHit[] hits;
  Vector3 rayDirection;

    hits = Physics.RaycastAll (transform.position, rayDirection);

    for (int i = 0; i < hits.Length; i++) {

     RaycastHit hit = hits [i];

     if (hit.transform.tag == "ignorePc1Collider") {
      panelScript = hit.transform.GetComponent<PanelPieceScript> ();
      panelScript.panelGoTransparent = true;
     }
    }
   }
  }
 */
