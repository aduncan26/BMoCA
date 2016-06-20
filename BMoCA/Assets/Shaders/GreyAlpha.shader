Shader "Custom/GreyAlpha" {
	Properties{
		_MainTex ("Texture", 2D) = "white" {}	// Grey

		_Greyscale("Greyscale", Range(0,1)) = 0

		_Brightness("Brightness", Range(0,1)) = 0

		_MinSat("Minimum saturation", Range(0,1)) = 0.5

		_GeometryFactor("Geometry weight", Range(0,1)) = 0.5

		_Intensity("Silhouette intensity", Range(0, 5)) = 2

		_MaxDist("Sight distance", float) = 50
		_Exponent("Distance exponent", Range(0,20)) = 2
	}
	SubShader{
		Tags{ "Queue" = "Transparent" "RenderType"="transparent"}
		// draw after all opaque geometry has been drawn

		Pass{
//			Cull Off
			ZWrite Off // don't occlude other objects
			Blend SrcAlpha OneMinusSrcAlpha // standard alpha blending

			CGPROGRAM

			#pragma vertex vert  
			#pragma fragment frag 

			#include "UnityCG.cginc"

			uniform float _Intensity;
			sampler2D _MainTex;
			uniform float _MaxDist;
			uniform float _Exponent;
			uniform float _GeometryFactor;
			uniform fixed _MinSat;
			uniform fixed _Greyscale;
			uniform fixed _Brightness;

			struct vertexInput {
				float4 vertex : POSITION;
				float4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};
			struct vertexOutput {
				float4 pos : SV_POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				float4 worldPos : TEXCOORD1;
				float3 normal : TEXCOORD2;
			};

			vertexOutput vert(vertexInput input)
			{
				vertexOutput output;

				float4x4 modelMatrix = _Object2World;
				float4x4 modelMatrixInverse = _World2Object;

				output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
				output.worldPos = mul(_Object2World, input.vertex);

				output.color = input.color;
				output.texcoord = input.texcoord;
//				output.texcoord = TRANSFORM_TEX(input.texcoord, _NormalMap);
				return output;
			}

			float4 frag(vertexOutput input) : COLOR
			{
//				***** DEALING WITH DISTANCE *****
				fixed4 pixelColor = tex2D(_MainTex, input.texcoord);
				fixed3 bw = (pixelColor.r*0.3 + _Brightness) + (pixelColor.g*0.59+ _Brightness) + (pixelColor.b*0.11+ _Brightness);

				fixed3 newColor = lerp(pixelColor.rgb, bw, _Greyscale);

				fixed newOpacity = pixelColor.a;

				//Find the distance of the pixel from the camera
				float distFromCamera = distance(input.worldPos, _WorldSpaceCameraPos);

				//Get the distance at which we start the fade
				float fadeDistance = 1/_MaxDist;

				//Get the fade value from the distance to the camera and the fade distance
				//Clamp it between 0 and 1
				//The exponent value allows for non-linear falloff values
				fixed fadeAmount = clamp(pow((distFromCamera * fadeDistance), _Exponent), 0, 1);

				newOpacity = lerp(newOpacity, 0, fadeAmount);


				return float4(newColor, newOpacity);
			}

			ENDCG
		}
	}
}
