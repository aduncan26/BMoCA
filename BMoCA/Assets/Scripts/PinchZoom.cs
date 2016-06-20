using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TrackpadTouch;

public class PinchZoom : MonoBehaviour {
	public GameObject prefab;
	Dictionary<int, GameObject> touchObjects = new Dictionary<int, GameObject>();

	public Camera cam;

	float zoomSpeed = 0f;

	const float ZOOM_ACCELERATION = 3f;
	const float ZOOM_DECELERATION = 0.8f;

	const float MAX_ZOOM_SPEED = 10f;

	const float MAX_SIZE = 100f;
	const float MIN_SIZE = 10f;

	const float MAX_DIST = 300f;

	float farthestDist = 0;

//	const float MIN_DIST = 0.0001f;

	float lastDist;
	float currentDist;

	void Update () {
//		Debug.Log (TrackpadInput.touches.Count);


		foreach (var touch in TrackpadInput.touches) {

			var screenPoint = new Vector3(touch.position.x, touch.position.y, 0);
			var worldPos = Camera.main.ScreenToWorldPoint(screenPoint);
			worldPos.z = 0;

			GameObject debugSphere;

			switch (touch.phase) {

			case TouchPhase.Began:
				if (touchObjects.TryGetValue (touch.fingerId, out debugSphere))
					Object.Destroy (debugSphere);
				debugSphere = touchObjects [touch.fingerId] = (GameObject)Object.Instantiate (prefab, worldPos, Quaternion.identity);	
				debugSphere.GetComponentInChildren<Text> ().text = touch.fingerId.ToString ();
				debugSphere.name = "Touch " + touch.fingerId;

				if (touch.fingerId == 1) {
					lastDist = Vector3.Distance (touchObjects [0].transform.position, touchObjects [1].transform.position);
				}
				break;

			case TouchPhase.Moved:
				if (touchObjects.TryGetValue(touch.fingerId, out debugSphere))
					debugSphere.transform.position = worldPos;
				break;

			case TouchPhase.Canceled:
			case TouchPhase.Ended:
				if (touchObjects.TryGetValue(touch.fingerId, out debugSphere))
					Object.Destroy(debugSphere);
				touchObjects.Remove(touch.fingerId);
				break;

				// case TouchPhase.Stationary:
				// break;

			default:
				break;
			}
		}

		ZoomCamera ();
	}

	void OnDisable() {
		foreach (var gameObject in touchObjects.Values)
			Object.Destroy(gameObject);
		touchObjects.Clear();
	}

	void ZoomCamera(){

//		if (touchObjects.Count > 1) {
//			currentDist = Vector3.Distance (touchObjects [0].transform.position, touchObjects [1].transform.position);
//
//		}
//
//
//
//		Debug.Log (currentDist);
//
//		cam.orthographicSize = Mathf.Lerp (MIN_SIZE, MAX_SIZE, ((MAX_DIST - Mathf.Abs(currentDist)) / MAX_DIST));
//	}

		if (touchObjects.Count > 1) {



			
			currentDist = Vector3.Distance (touchObjects [0].transform.position, touchObjects [1].transform.position);

			if (zoomSpeed < MAX_ZOOM_SPEED) {
				zoomSpeed += (lastDist - currentDist) * ZOOM_ACCELERATION * Time.deltaTime;
			} else {
				zoomSpeed = MAX_ZOOM_SPEED;
			}
		

			lastDist = currentDist;

		}

		if (Mathf.Abs(zoomSpeed) > 0) {
			zoomSpeed *= ZOOM_DECELERATION;
		}


		if (cam.orthographicSize <= MAX_SIZE && cam.orthographicSize >= MIN_SIZE) {
			cam.orthographicSize += zoomSpeed;
		} else if (cam.orthographicSize > MAX_SIZE) {
			cam.orthographicSize = MAX_SIZE;
			zoomSpeed = 0f;
		} else {
			cam.orthographicSize = MIN_SIZE;
			zoomSpeed = 0f;
		}



	}

}
