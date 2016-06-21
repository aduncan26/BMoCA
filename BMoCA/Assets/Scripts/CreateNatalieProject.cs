using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreateNatalieProject : MonoBehaviour {

	private static CreateNatalieProject instance;
	public static CreateNatalieProject GetInstance(){
		return instance;
	}

	public GameObject butterflyBridgeTDPrefab;
	public GameObject butterflyBridgeBVPrefab;


	public GameObject farmacyTDPrefab;
	public GameObject farmacyBVPrefab;


	public Vector3[] butterflyBridgePositions ;

	public Vector3[] farmacyPositions ;

	public List<GameObject> inactiveProjectObjects = new List<GameObject>();

	// Use this for initialization
	void Start () {
		instance = this;

		CreateAllProjects (butterflyBridgePositions, butterflyBridgeBVPrefab);
		CreateAllProjects (farmacyPositions, farmacyBVPrefab);

		ShuffleList ();

		EnvironmentHealthManager.maxProjects = butterflyBridgePositions.Length + farmacyPositions.Length;
	}
	
	void Update(){
		if (Input.GetKeyDown (KeyCode.Space)) {
//			CreateNewProject ();
		}
	}

	void ShuffleList(){
		List<GameObject> temp = new List<GameObject> ();
		for (int i = inactiveProjectObjects.Count - 1; i >= 0; i--) {
			int whichObjectToPull = Random.Range (0, i);

			temp.Add (inactiveProjectObjects [whichObjectToPull]);
			inactiveProjectObjects.RemoveAt (whichObjectToPull);
		}

		inactiveProjectObjects = temp;
	}

	void CreateAllProjects(Vector3[] listOfPositions, GameObject objectPrefab){
		for (int i = 0; i < listOfPositions.Length; i++) {
			GameObject newObject = Instantiate (objectPrefab, listOfPositions [i], Quaternion.identity) as GameObject;

			inactiveProjectObjects.Add (newObject);

			newObject.SetActive (false);
		}
	}



}
