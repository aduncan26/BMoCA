using UnityEngine;
using System.Collections;

public class TurnOffLight : MonoBehaviour {

	public Light light;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnPreCull(){
		light.enabled = false;
	}

	void OnPreRender(){
		light.enabled = false;
	}

	void OnPostRender(){
		light.enabled = true;
	}
}
