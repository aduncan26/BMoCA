using UnityEngine;
using System.Collections;

public class CursorMovement : MonoBehaviour {

	public Camera cam;

	GameObject cursorObject;

	float maxX;
	float maxY;

	float moveSpeed = 20f;

	float cursorZ;

	// Use this for initialization
	void Start () {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		maxX = cam.pixelWidth;
		maxY = cam.pixelHeight;

		Debug.Log (maxX);

		cursorObject = this.gameObject;

	}
	
	// Update is called once per frame
	void Update () {
		MoveCursor ();
	}

	void MoveCursor(){
		cursorZ = cam.transform.position.y - 0.4f;

		Debug.Log (cursorZ);


		Vector3 cursorPosition = cam.WorldToScreenPoint (cursorObject.transform.position);



		if (cursorPosition.x <= maxX && cursorPosition.z <= maxY) {
			cursorPosition += new Vector3 (Input.GetAxis ("Mouse X"), 0, Input.GetAxis ("Mouse Y")) * moveSpeed * Time.deltaTime;

		} else {
			if (cursorPosition.x > maxX) {
				cursorPosition = new Vector3 (maxX, cursorPosition.y, cursorPosition.z);
			}
			if (cursorPosition.z > maxY) {
				cursorPosition = new Vector3 (cursorPosition.x, cursorPosition.y, maxY);
			}
		}

		cursorPosition = cam.ScreenToWorldPoint (cursorPosition);


		cursorObject.transform.position = new Vector3 (cursorPosition.x, cursorZ, cursorPosition.z);
	}
}
