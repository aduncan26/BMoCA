Shader "Hidden/ColorDrain"
{
	Properties
	{
		//This is the "Texture" that is actually just the image rendered onscreen
		//before doing the postprocessing effect
		_MainTex ("Texture", 2D) = "white" {}

		//The saturation levels of each color, which will be set externally on ColorDrainScript
		_RedSaturation("_RedSaturation", Range (-1, 3)) = 0
		_GreenSaturation("_GreenSaturation", Range (-1, 3)) = 0
		_BlueSaturation("_BlueSaturation", Range (-1, 3)) = 0

		//A variable for brightness of scene
		_Brightness("_Brightness", Range(-1, 1)) = 0
	}
	SubShader
	{

		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
 
			#include "UnityCG.cginc"

			//Declare the variables/properties
			uniform float _RedSaturation;
			uniform float _GreenSaturation;
			uniform float _BlueSaturation;
			uniform float _Brightness;
			sampler2D _MainTex;

			fixed4 frag (v2f_img i) : SV_Target
			{
				//Sample/store the screen (MainTex) pixel color
				//We'll need this later when we re-saturate the scene
				fixed4 col = tex2D(_MainTex, i.uv);

				//Set the desaturation value based on r, g, and b
				//These colors are better than just averaging
				//see http://www.alanzucconi.com/2015/07/08/screen-shaders-and-postprocessing-effects-in-unity3d/
				float saturation = col.r * 0.33 + col.g * 0.59 * col.b * 0.11;

				//Set the values of each color, which will depend on the saturation variables
				//set above, which will be determined by an outside script
				//Also, add brightness at end
				float redValue = saturation * clamp((1 - _RedSaturation), 0, 1) + col.r * (_RedSaturation) + _Brightness;
				float greenValue = saturation * clamp((1 - _GreenSaturation), 0, 1) + col.g * (_GreenSaturation) + _Brightness;
				float blueValue = saturation * clamp((1 - _BlueSaturation), 0, 1) + col.b * (_BlueSaturation) + _Brightness;

				//Use these values to make a new color, and return it
				return fixed4(redValue, greenValue, blueValue, col.a);
			}
			ENDCG
		}
	}
}
