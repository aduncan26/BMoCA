using UnityEngine;
using System;
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


	public List<Vector3> butterflyBridgePositions = new List<Vector3> ();

	public List<Vector3> farmacyPositions = new List<Vector3> ();

	enum NatalieProjects{
		ButterflyBridge,
		Farmacy
	}
	// Use this for initialization
	void Start () {
		instance = this;

		EnvironmentHealthManager.maxProjects = butterflyBridgePositions.Count + farmacyPositions.Count;
	}
	
	void Update(){
		if (Input.GetKeyDown (KeyCode.Space)) {
			CreateNewProject ();
		}
	}

	void CreateNewProject(){
		int numProjects = Enum.GetNames (typeof(NatalieProjects)).Length;

		NatalieProjects newProject =  (NatalieProjects)UnityEngine.Random.Range(0, numProjects);


		switch (newProject) {
		case NatalieProjects.ButterflyBridge:
			if (butterflyBridgePositions.Count > 0) {
				int whichPosition = UnityEngine.Random.Range (0, butterflyBridgePositions.Count);

				Instantiate (butterflyBridgeBVPrefab, butterflyBridgePositions [whichPosition], Quaternion.identity);
				Instantiate (butterflyBridgeTDPrefab, butterflyBridgePositions [whichPosition], Quaternion.identity);

				butterflyBridgePositions.RemoveAt (whichPosition);

				EnvironmentHealthManager.GetInstance ().NewProject ();
			} else {
				newProject = NatalieProjects.Farmacy;
			}
			break;
		
		case NatalieProjects.Farmacy:
			if (farmacyPositions.Count > 0) {
				int whichPosition = UnityEngine.Random.Range (0, farmacyPositions.Count);

				Instantiate (farmacyBVPrefab, farmacyPositions [whichPosition], Quaternion.identity);
				Instantiate (farmacyTDPrefab, farmacyPositions [whichPosition], Quaternion.identity);

				farmacyPositions.RemoveAt (whichPosition);

				EnvironmentHealthManager.GetInstance ().NewProject ();
			} else {
				newProject = NatalieProjects.ButterflyBridge;
			}
			break;
		}
	}

}
