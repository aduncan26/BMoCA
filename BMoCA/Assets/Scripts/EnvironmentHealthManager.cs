using UnityEngine;
using System.Collections;

public class EnvironmentHealthManager : MonoBehaviour{

	private static EnvironmentHealthManager instance;
	public static EnvironmentHealthManager GetInstance(){
		return instance;
	}

	[Range (0,1)]
	float leafAreaIndex = 0;

	int currentNatalieProjects;

	public static int maxProjects;

	ColorDrainScript colorDrain;

	void Awake(){
		instance = this;

		colorDrain = GameObject.Find ("ButterflyCam").GetComponent<ColorDrainScript> ();
	}

	public void NewProject(){
		currentNatalieProjects++;
		leafAreaIndex = (float)currentNatalieProjects / (float)maxProjects;

		AddSaturationFromLeafAreaIndex ();
	}

	void AddSaturationFromLeafAreaIndex(){
		colorDrain.redSaturation = leafAreaIndex;
		colorDrain.greenSaturation = leafAreaIndex;
		colorDrain.blueSaturation = leafAreaIndex;

	}
}
