using UnityEngine;
using System.Collections;

//JUST ADD THIS SCRIPT AS A COMPONENT TO THE CAMERA TO GET THE SHADER TO WORK!
[ExecuteInEditMode]
public class ColorDrainScript : MonoBehaviour {

	//variables, with range values, to set the shader properties
	//I put such big ranges to allow strange visual effects
	//Going to -1 takes out all of that color in the scene
	//Going above 1 supersaturates the scene with that color (may effect Bloom shaders)
	[Range(-1,3)]
	public float redSaturation = 0;
	[Range(-1,3)]
	public float greenSaturation = 0;
	[Range(-1,3)]
	public float blueSaturation = 0;
	[Range(-1,1)]
	public float brightness = 0;

	//This gets the correct shader for the camera to use
	private Material renderMaterial;


	void Awake () {
		renderMaterial = new Material (Shader.Find ("Hidden/ColorDrain"));
	}

	void OnRenderImage(RenderTexture source, RenderTexture destination){
		//Sets the properties on the shader based on the public variable values above
		renderMaterial.SetFloat ("_RedSaturation", redSaturation);
		renderMaterial.SetFloat ("_GreenSaturation", greenSaturation);
		renderMaterial.SetFloat ("_BlueSaturation", blueSaturation);
		renderMaterial.SetFloat ("_Brightness", brightness);

		//Applies the shader material to the screen render image
		Graphics.Blit (source, destination, renderMaterial);

	}
}
