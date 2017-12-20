using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonPlayerCamera : MonoBehaviour {

	private Player playerScript;

	public Transform pcCam, pcCharacter, pcCenterPoint, pcMotionPoint;

	public float centerPointYPosition = 1.675f;

	private float mouseX, mouseY;//For access.

	//public float rotationSpeed = 5f;//For SLERPING (Although needs to be put on PC + PC SCRIPT). ***Not implemented***

	// Use this for initialization
	void Start () {

		playerScript = FindObjectOfType<Player> ();

	}

	// Update is called once per frame
	void Update () 
	{

		//1st Person

			pcCam.LookAt (playerScript.rayHitPcLook.point);//Camera looks

			mouseX += Input.GetAxisRaw ("Mouse X");//Axis for x.
			mouseY -= Input.GetAxisRaw ("Mouse Y");//Axis for y.

		pcCenterPoint.localRotation = Quaternion.Euler (mouseY, mouseX, 0); //RIGHT HERE!!! Change the 0 to Z and you've got something special for the attacking.

		pcCenterPoint.position = new Vector3 (pcCharacter.position.x, pcCharacter.position.y + centerPointYPosition, pcCharacter.position.z);//Makes Center Point follow the PC, effectively making the Camera follow the Center Point.

		//Motion Point, into player movement direction.
		pcMotionPoint.position = new Vector3 (pcCharacter.position.x, pcCharacter.position.y, pcCharacter.position.z);//Makes Motion Point follow the PC.
		pcMotionPoint.localRotation = Quaternion.Euler (0, mouseX, 0);//Sets up just y rotational directional movement of Motion Point for Movement direction.

		/*if (Input.GetAxisRaw ("Vertical") > 0 | Input.GetAxis ("Vertical") < 0) 
		{
			Quaternion turnAngle = Quaternion.Euler (0, pcCenterPoint.eulerAngles.y, 0);

			pcCharacter.rotation = Quaternion.Slerp (pcCharacter.rotation, turnAngle, Time.deltaTime * rotationSpeed);
		}*/
		//SLERP = Slows down the rotation, could be used for slowing down rotation movement, and for attacks?
	}

	void fixedUpdate ()
	{

	}
}
