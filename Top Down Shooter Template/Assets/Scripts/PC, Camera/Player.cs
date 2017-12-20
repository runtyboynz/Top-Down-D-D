using UnityEngine;
using System.Collections;

[RequireComponent (typeof (PlayerController))] //Automatically gives the object a player controller
[RequireComponent (typeof (WeaponController))]
public class Player : MonoBehaviour 
{
	Animator anim;

	public Transform motionPoint;//To get the character to move with the camera center point, aka camera.

	public Transform farPointAimerOriginRH, farPointAimerOriginLH, farPointAimerReachRH, farPointAimerReachLH;//For Object to follow ray point.

	public float farPointDefaultAimerReachY = 1.0f;//Sets default Aimer Reach Sphere Y axis.

	public Transform closePointAim;//To register close attacks.
	public Transform combatAimer;//The 1st/3rd Person Sphere that pc aims/reaches at/to when attacking.
	public Transform combatAimerHeadLook;//The 1st/3rd Person Sphere that pc aims/reaches at/to when attacking.
	public Transform combatAimerHeadLookParent;//The Parent of the Head Look child to move to.
	public Transform combatAimerEndAttackPoint;//Where the arms/legs will move to in order to finish its attack.

	public RaycastHit rayHitPcLook; //Ray that collides with in-game colliders for pc to look towards and interact with.
	string layer8 = "IgnoreCollidersPc1";
	public RaycastHit rayHitPcAttack; //Ray that collides with Quad to work out the attack direction.

	public float moveSpeed = 5;

	PlayerController controller;
	Camera viewCamera;
	WeaponController weaponController;

	Transform cam;
	Vector3 camForward;
	Vector3 move;
	Vector3 moveInput;

	float forwardAmount;
	float turnAmount;

	void Start () 
	{
		controller = GetComponent<PlayerController> (); 
		weaponController = GetComponent<WeaponController> (); 
		viewCamera = Camera.main;
		SetupAnimator (); //Further down the script, sets up the animations for the pc.
		cam = Camera.main.transform;
	}

	void Update () 
	{

		float horizontal = Input.GetAxisRaw ("Horizontal");
		float vertical = Input.GetAxisRaw ("Vertical");

		//Animation in relation to camera point to the player
		if (cam != null) 
		{
			camForward = Vector3.Scale (cam.up, new Vector3 (1, 0, 1)).normalized;//This will take care of the scale for the camera (WHAT DOES NORMALIZED MEAN?)
			move = vertical * camForward + horizontal * cam.right;
		} 
		else 
		{
			move = vertical * Vector3.forward + horizontal * Vector3.right;
		}

		if (move.magnitude > 1)
		{
			move.Normalize();//if magnitude movement is higher than one, normalize it
		}

		Move (move);


		//Movement Input

		//Next 3 lines set movement of player
		Vector3 movement = new Vector3 (horizontal, 0, vertical); //"Raw" takes away any default smoothing (Which is great!!!!)
		movement = motionPoint.rotation * movement; //Connects with Motion point object for setting up movement with camera correctly.
		Vector3 moveVelocity = movement.normalized * moveSpeed; //"normalized" just gives the direction of the input.
		controller.Move (moveVelocity);

		//Animation set up
		//anim.SetFloat ("Forward", vertical);
		//anim.SetFloat ("Turn", horizontal);



		//Look Input


			//RAY 1: Looking ray/interacting ray.

			/*/ Dylans stuff//////////
			RaycastHit[] hits;
			Vector3 rayDirection;
			hits = Physics.RaycastAll (rayLook);
			for (int i = 0; i < hits.Length; i++) 
			{
				RaycastHit hit = hits [i];
				// Add tags here that you want the rayCast to ignore (must have 'return')
				if (hit.transform.tag != "IgnorePc1CollidersTag") 
					// Dylans stuff/////*/
		
			Ray rayLook = viewCamera.ScreenPointToRay (Input.mousePosition); //Gives screen position (mouse position), return a ray from the camera to that position to infinity.
			LayerMask layerMaskIgnoreCollider = ~(1 << LayerMask.NameToLayer(layer8)); {//Sets layer 8 ("IgnoreColliderPc1") to be used for the ray to ignore.

					//Raycast to make player look in direction. 
					if (Physics.Raycast (rayLook, out rayHitPcLook, Mathf.Infinity, layerMaskIgnoreCollider)) { //Gives OUT raydistance then assigns it a variable. Will return true if the ray itersects with a collider and will also know length.

					Debug.DrawLine (rayLook.origin, rayHitPcLook.point, Color.red);
					controller.LookAt (rayHitPcLook.point);//Player looks at ray point.

		//Attack Input


			//RAY 2: Attack Direction ray.

			Ray rayAttack = viewCamera.ScreenPointToRay (Input.mousePosition); //Gives screen position (mouse position), return a ray from the camera to that position to infinity.

			//Raycast to make player attack from Origin to Reach point. 
			if (Physics.Raycast (rayAttack, out rayHitPcAttack)) { //Gives OUT raydistance then assigns it a variable. Will return true if the ray itersects with a collider and will also know length.

			Debug.DrawLine (rayAttack.origin, rayHitPcAttack.point, Color.blue);
				}

					//AIMING IN CLOSE COMBAT: Here we use two game objects to measure the x,y,z distance for how large/small your attack will be.
					farPointAimerOriginRH.LookAt (motionPoint);//looks at player/motion/center point RH
					farPointAimerOriginLH.LookAt (motionPoint);//looks at player/motion/center point LH

					farPointAimerReachRH.position = rayHitPcAttack.point;//Makes Reach Object follow mouse/camera ray RH.
					farPointAimerReachLH.position = rayHitPcAttack.point;//Makes Reach Object follow mouse/camera ray LH.

					farPointAimerReachRH.transform.position = new Vector3 (farPointAimerReachRH.transform.position.x, farPointAimerReachRH.transform.position.y + farPointDefaultAimerReachY, farPointAimerReachRH.transform.position.z);//Sets Mouse Aim Reach at higher Y, so he looks upward while freelooking RH.
					farPointAimerReachLH.transform.position = new Vector3 (farPointAimerReachLH.transform.position.x, farPointAimerReachLH.transform.position.y + farPointDefaultAimerReachY, farPointAimerReachLH.transform.position.z);//Sets Mouse Aim Reach at higher Y, so he looks upward while freelooking LH.

					farPointAimerReachRH.LookAt (motionPoint);//looks at player/motion/center point RH
					farPointAimerReachLH.LookAt (motionPoint);//looks at player/motion/center point LH

					//1ST PERSON: For Finishing up Combat mechanics + Enabling 1st Person.
				closePointAim.LookAt (rayHitPcLook.point);//Makes Hands aim at the combat aimer reach which it will reach/aim at/to.
				combatAimer.transform.position = rayHitPcLook.point;//Combat aimer is an object to interact with things at the end of the ray.
				}

				//Weapon Right Hand Input

				if (Input.GetMouseButton (1)) {//If right mouse button is pushed down, shoot 
					farPointAimerReachRH.transform.position = new Vector3 (farPointAimerReachRH.transform.position.x, farPointAimerReachRH.transform.position.y - farPointDefaultAimerReachY, farPointAimerReachRH.transform.position.z);//Resets Mouse Aim Sphere to default Y position RH.
					farPointAimerReachRH.localRotation = Quaternion.Euler (0, farPointAimerReachRH.localRotation.y, 0);//stops object from tilting when closer to player.
					weaponController.ShootRH ();
				}

				if (Input.GetMouseButtonDown (1)) {//If right mouse button is clicked (For Aiming Origin and Reach Gameobjects to register)
					farPointAimerOriginRH.transform.position = new Vector3 (farPointAimerReachRH.transform.position.x, farPointAimerReachRH.transform.position.y, farPointAimerReachRH.transform.position.z);//Sets Origin Aimer at the exact point Reach aim is when mouse is pushed down.
					farPointAimerOriginRH.localRotation = Quaternion.Euler (0, farPointAimerOriginRH.localRotation.y, 0);//stops object from tilting when closer to player.
					//^^Sends Origin Gameobject to Reach Object position to measure the distance so Reach Object can measure how far away it moves from the origin point^^.

					combatAimerHeadLook.transform.parent = combatAimerHeadLookParent.transform;//Moves combatAimerHeadLook to a parent outside of the pc, so none of the objects interfere with it's movement.
					combatAimerHeadLook.transform.position = new Vector3 (combatAimer.transform.position.x, combatAimer.transform.position.y, combatAimer.transform.position.z);//Makes Players Head look at the point where you first clicked the mouse.
				}

				if (Input.GetMouseButtonUp (1)) {//If right mouse button is unclicked (For Aiming Origin to go back to default)
					farPointAimerOriginRH.transform.position = transform.TransformPoint (Vector3.zero);//Snaps Origin point back to Default position

					combatAimerHeadLook.transform.parent = combatAimerEndAttackPoint.transform;//Puts combatAimerHeadLook back within PC/Motion Point/Close Point Aim/Combat End Attack Point.
					combatAimerHeadLook.transform.position = combatAimerEndAttackPoint.transform.position;//Makes Players Head look back at his fists.
				}


				//Weapon Left Hand Input

				if (Input.GetMouseButton (0)) {//If left mouse button is pushed down, shoot 
					farPointAimerReachLH.transform.position = new Vector3 (farPointAimerReachLH.transform.position.x, farPointAimerReachLH.transform.position.y - farPointDefaultAimerReachY, farPointAimerReachLH.transform.position.z);//Resets Mouse Aim Sphere to default Y position LH.
					farPointAimerReachLH.localRotation = Quaternion.Euler (0, farPointAimerReachLH.localRotation.y, 0);//stops object from tilting when closer to player.
					weaponController.ShootLH ();
				}

				if (Input.GetMouseButtonDown (0)) {//If left mouse button is clicked (For Aiming Origin and Reach Gameobjects to register)
					farPointAimerOriginLH.transform.position = new Vector3 (farPointAimerReachLH.transform.position.x, farPointAimerReachLH.transform.position.y, farPointAimerReachLH.transform.position.z);//Sets Origin Aimer at the exact point Reach aim is when mouse is pushed down.
					farPointAimerOriginLH.localRotation = Quaternion.Euler (0, farPointAimerOriginLH.localRotation.y, 0);//stops object from tilting when closer to player.
					//^^Sends Origin Gameobject to Reach Object position to measure the distance so Reach Object can measure how far away it moves from the origin point^^.

					combatAimerHeadLook.transform.parent = combatAimerHeadLookParent.transform;//Moves combatAimerHeadLook to a parent outside of the pc, so none of the objects interfere with it's movement.
					combatAimerHeadLook.transform.position = new Vector3 (combatAimer.transform.position.x, combatAimer.transform.position.y, combatAimer.transform.position.z);//Makes Players Head look at the point where you first clicked the mouse.
				}

				if (Input.GetMouseButtonUp (0)) {//If left mouse button is unclicked (For Aiming Origin to go back to default)
					farPointAimerOriginLH.transform.position = transform.TransformPoint (Vector3.zero);//Snaps Origin point back to Default position

					combatAimerHeadLook.transform.parent = combatAimerEndAttackPoint.transform;//Puts combatAimerHeadLook back within PC/Motion Point/Close Point Aim/Combat End Attack Point.
					combatAimerHeadLook.transform.position = combatAimerEndAttackPoint.transform.position;//Makes Players Head look back at his fists.
				}
			}
		}


	void Move (Vector3 move)
	{
		if (move.magnitude > 1)
			{
				move.Normalize();//if magnitude movement is higher than one, normalize it
			}

		this.moveInput = move;//moveInput will be the move we are passing.

		ConvertMoveInput();//need to change the moveInput because it is going to be used in other functions
		UpdateAnimator();
	}

	void ConvertMoveInput();
	{
		Vector3 localMove = Transform.InverseTransformDirection(controller);//Inverse direction takes one direction and converts it form world position to local position.
		turnAmount = localMove.x;

		forwardAmount = localMove.z;
	}


	void UpdateAnimator();
	{
		anim.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
		anim.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);
	}

	void SetupAnimator()//Don't need to say much about this here, :P
	{
		// this is a ref to the animator component on the root.
		anim = GetComponent<Animator> ();

		//we use avatar from a child animator component if present
		//this is to enable easy swapping of the character model as a child node
		foreach (var childAnimator in GetComponentsInChildren<Animator>()) 
		{
			if (childAnimator != anim) 
			{
				anim.avatar = childAnimator.avatar;
				Destroy (childAnimator);
				break; //if you find the first animator, stop searching
			}
		}
	}
}

