using UnityEngine;
using System.Collections;

public class ButterflyMovement : MonoBehaviour {

	Transform trans;
	Transform rotTrans;
	Transform camTrans;

	public Vector3[] routePositions;

	int routeIndex = 0;

	//Speeds
	const float BODY_MOVE_SPEED = 3f;
	const float TURN_SPEED = 0.5f;

	//Lerp cutoffs
	const float POINT_SWITCH_DIST = 0.5f;

	// Use this for initialization
	void Start () {
		trans = GetComponent<Transform> ();
		rotTrans = trans.GetChild (0);
		camTrans = rotTrans.GetChild (0);

//		ShuffleArray.Shuffle (routePositions);
	}
	
	// Update is called once per frame
	void Update () {
		MoveToPoint ();
		TurnToFace ();
	}

	void MoveToPoint(){
		trans.position = Vector3.MoveTowards (trans.position, routePositions [routeIndex], BODY_MOVE_SPEED * Time.deltaTime);

		if (Vector3.Distance (trans.position, routePositions [routeIndex]) < POINT_SWITCH_DIST) {
			routeIndex++;
			if (routeIndex >= routePositions.Length) {
				routeIndex = 0;
//				ShuffleArray.Shuffle (routePositions);
			}
		}
	}

	void TurnToFace(){
		Vector3 targetPlayer = routePositions[routeIndex] - rotTrans.position;

		Quaternion targetQuat = Quaternion.LookRotation(targetPlayer);

		rotTrans.rotation = Quaternion.Slerp(rotTrans.rotation, targetQuat, TURN_SPEED * Time.deltaTime);

//		Quaternion lookRot = Quaternion.FromToRotation (rotTrans.forward, 
//			(routePositions [routeIndex] - rotTrans.position).normalized);
//
//		rotTrans.rotation = Quaternion.Lerp (rotTrans.rotation, rotTrans.rotation * lookRot, TURN_SPEED * Time.deltaTime);
	}
}
