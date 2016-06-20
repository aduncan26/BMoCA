using UnityEngine;
using System.Collections;
using TrackpadTouch;
using System.Collections.Generic;

public class CameraMovement : MonoBehaviour {

	Camera cam;
	const float MAX_FOV = 100;
	const float MIN_FOV = 30;

	const float ZOOM_ACCEL = 0.1f;
	const float MAX_ZOOM_SPEED = 5f;

	float zoomSpeed = 1f;

	Vector3 touchOneStartPos;
	Vector3 touchTwoStartPos;
	float lastDistance;
	float currentDistance;

	// Use this for initialization
	void Start () {
		cam = GetComponent<Camera> ();
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log (TrackpadInput.touchCount);
		ZoomCamera ();
	}
		

	void ZoomCamera(){
		

		if (TrackpadInput.touchCount > 1) {
//			float totalDist = 0;
//			for (int i = 0; i < TrackpadInput.touchCount - 1; i++) {
//				totalDist += Vector3.Distance (TrackpadInput.GetTouch (i).position, 
//					TrackpadInput.GetTouch (i + 1).position);
//			}
//			if (totalDist < 50)
//				return;
//			
//			if (TrackpadInput.touchCount > 2) {
//				TrackpadInput.touches.RemoveRange (2, TrackpadInput.touchCount - 2);
//			}
			
			Touch touchOne = TrackpadInput.GetTouch (0);
			Touch touchTwo = TrackpadInput.GetTouch (1);

			if (touchTwo.phase == TouchPhase.Began) {
				touchOneStartPos = (touchOne.position);
				touchTwoStartPos = (touchTwo.position);

				lastDistance = Vector3.Distance (touchOneStartPos, touchTwoStartPos);
				currentDistance = lastDistance;
			}

			for (int i = 0; i < 2; i++) {
				if (TrackpadInput.GetTouch(i).phase == TouchPhase.Moved) {
					currentDistance = Vector3.Distance ((touchOne.position),(touchTwo.position));
				}
			}

			if(cam.fieldOfView <= MAX_FOV && cam.fieldOfView >= MIN_FOV){
				cam.fieldOfView += (lastDistance - currentDistance) * zoomSpeed * Time.deltaTime;
			} else if(cam.fieldOfView > MAX_FOV){
				cam.fieldOfView = MAX_FOV;
			} else{
				cam.fieldOfView = MIN_FOV;
			}

			lastDistance = currentDistance;
			Debug.Log (lastDistance);
			currentDistance = 0;


			for(int i = 0; i < 2; i++){
				
				if (TrackpadInput.GetTouch (i).phase == TouchPhase.Ended) {
					lastDistance = 0;
					currentDistance = 0;
					touchOneStartPos = Vector3.zero;
					touchTwoStartPos = Vector3.zero;
				}
			}



		}
	}

	void AltZoom(){
		currentDistance = 0;
		lastDistance = 0;

		if (TrackpadInput.touches.Count > 1) {
//			float totalDist = 0;
//			for (int i = 0; i < TrackpadInput.touchCount - 1; i++) {
//				totalDist += Vector3.Distance (TrackpadInput.GetTouch (i).position, 
//					TrackpadInput.GetTouch (i + 1).position);
//			}
//			if (totalDist < 50)
//				return;
		
			for (int i = 1; i < TrackpadInput.touchCount; i++) {
				
				if (TrackpadInput.GetTouch (i).phase == TouchPhase.Began) {
					lastDistance += Vector3.Distance (TrackpadInput.GetTouch (i - 1).position, TrackpadInput.GetTouch (i).position);
					Debug.Log ("Hello?");
				}

				if (TrackpadInput.GetTouch (i).phase == TouchPhase.Moved) {
					currentDistance += Vector3.Distance (TrackpadInput.GetTouch (i - 1).position, TrackpadInput.GetTouch (i).position);
				}

			}
			if (cam.fieldOfView <= MAX_FOV && cam.fieldOfView >= MIN_FOV) {
				cam.fieldOfView += (lastDistance - currentDistance) * zoomSpeed * Time.deltaTime;
			} else if (cam.fieldOfView > MAX_FOV) {
				cam.fieldOfView = MAX_FOV;
			} else {
				cam.fieldOfView = MIN_FOV;
			}

			lastDistance = currentDistance;
		}
	}
}
