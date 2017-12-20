using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonPlayerCamera : MonoBehaviour {

	private Player playerScript;

	public Transform pcCam, pcCharacter, pcCenterPoint, pcMotionPoint;

	public float centerPointYPosition = 1.75f;

	private float mouseX, mouseY;//For access.
	private float keyQ, keyE;//For access.
	public float camPivotXSpeed = 2f;//Speed of X at which camera pivots when used.
	public float camPivotYSpeed = 2f;//Speed of Y at which camera pivots when used.

	private float zoom;//Default zoom.
	public float zoomSpeed = 4f;//Speed of Z at which each time the zoom wheel is turned.
	public float zoomMin = -0.6f;//Minimum value that camera Z zooms in.
	public float zoomMax = -10f;//Maxiumum value that camera Z zooms out.

	//public float rotationSpeed = 5f;//For SLERPING (Although needs to be put on PC + PC SCRIPT). ***Not implemented***

	// Use this for initialization
	void Start () {

		playerScript = FindObjectOfType<Player> ();
		zoom = -4f;//starting Z point on transform.

	}
	
	// Update is called once per frame
	void Update () 
	{

		zoom += Input.GetAxis ("Mouse ScrollWheel") * zoomSpeed;//Assigns scrollwheel to zoom + with zoomspeed.

		if (zoom > zoomMin)
			zoom = zoomMin;//Doesn't go any lower than minimum.
		
		if (zoom < zoomMax)
			zoom = zoomMax;//Doesn't go any lower than maximum.

		pcCam.transform.localPosition = new Vector3 (0, 0, zoom);//Sets up main camera Z axis to use zoom.

		//DOWN FROM HERE IS THE POINT TO USE FOR SETTING UP LIGHT/MED/HEAVY ATTACKS!!!

		if (Input.GetMouseButton (2))//Middle Mouse button push.
		{
			mouseX += Input.GetAxis ("Mouse X") * camPivotXSpeed;//Axis for x.
			mouseY -= Input.GetAxis ("Mouse Y") * camPivotYSpeed;//Axis for y.
		}
			
		mouseY = Mathf.Clamp (mouseY, -60f, 75f);//creates a min/max clamp for y.

		//1st Person

		if (Input.GetButton ("Cursor")) //When holding down Alt, you can play fps! *****NEEDS WORK*****
		{
			//viewCamera (which is in the player script) needs to be accessed here, to become 1st person camera, so all the rays go from it. It might just work.
			pcCam.LookAt (playerScript.rayHitPcLook.point);//Camera looks
			mouseX += Input.GetAxis ("Mouse X");//Axis for x.
			mouseY -= Input.GetAxis ("Mouse Y");//Axis for y.
		}
		else
		{
			pcCam.LookAt (pcCenterPoint);//For normal camera movement.
		}	
			
		pcCenterPoint.localRotation = Quaternion.Euler (mouseY, mouseX, 0); //RIGHT HERE!!! Change the 0 to Z and you've got something special for the attacking.

		//For Pivoting with Q and E:
		if (Input.GetKey ("e"))// Q button push.
		{
			pcCenterPoint.localRotation = Quaternion.Euler (mouseY, mouseX += 0.75f * camPivotXSpeed, 0);
		}
		if (Input.GetKey ("q"))// E button push.
		{
			pcCenterPoint.localRotation = Quaternion.Euler (mouseY, mouseX -= 0.75f * camPivotXSpeed, 0);
		}

		//QUICK NOTE: Quaternions are used for rotation instead of Vector3s.

		//ABOVE FROM HERE IS THE POINT TO USE FOR SETTING UP LIGHT/MED/HEAVY ATTACKS!!!

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
