using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

	public Transform pcCam, pcCharacter, pcCenterPoint;

	public float centerPointYPosition = 1f;

	private float mouseX, mouseY;//For access.
	public float camPivotXSpeed = 2f;//Speed of X at which camera pivots when used.
	public float camPivotYSpeed = 2f;//Speed of Y at which camera pivots when used.

	private float zoom;//Default zoom.
	public float zoomSpeed = 4f;//Speed of Z at which each time the zoom wheel is turned.
	public float zoomMin = -1f;//Minimum value that camera Z zooms out.
	public float zoomMax = -10f;//Maxiumum value that camera Z zooms out.

	public float rotationSpeed = 5f;//For SLERPING. ***Not implemented***

	// Use this for initialization
	void Start () {

		zoom = -4f;//starting Z point on transform.

	}
	
	// Update is called once per frame
	void Update () {

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

		mouseY = Mathf.Clamp (mouseY, -60f, 60f);//creates a min/max clamp for y.
		pcCam.LookAt (pcCenterPoint);//Main Camera is always looking at center point object.
		pcCenterPoint.localRotation = Quaternion.Euler (mouseY, mouseX, 0); //RIGHT HERE!!! Change the 0 to Z and you've got something special for the attacking.
		//QUICK NOTE: Quaternions are used for rotation instead of Vector3s.

		//ABOVE FROM HERE IS THE POINT TO USE FOR SETTING UP LIGHT/MED/HEAVY ATTACKS!!!

		pcCenterPoint.position = new Vector3 (pcCharacter.position.x, pcCharacter.position.y + centerPointYPosition, pcCharacter.position.z);//Makes Center Point follow the PC, effectively making the Camera follow the Center Point.

		/*if (Input.GetAxisRaw ("Vertical") > 0 | Input.GetAxis ("Vertical") < 0) 
		{
			Quaternion turnAngle = Quaternion.Euler (0, pcCenterPoint.eulerAngles.y, 0);

			pcCharacter.rotation = Quaternion.Slerp (pcCharacter.rotation, turnAngle, Time.deltaTime * rotationSpeed);
		}*/
		//SLERP = Slows down the rotation, could be used for slowing down rotation movement, and for attacks?
	}
}
