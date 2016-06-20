Shader "Custom/Silhouette" {
	Properties{
		_NormalMap ("Normal map", 2D) = "bump" {}	// Grey
		_Color("Color", Color) = (1, 1, 1, 0.1)

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

			uniform float4 _Color; // define shader property for shaders
			uniform float _Intensity;
			sampler2D _NormalMap;
			uniform float _MaxDist;
			uniform float _Exponent;
			uniform float _GeometryFactor;
			uniform fixed _MinSat;

			struct vertexInput {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};
			struct vertexOutput {
				float4 pos : SV_POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				float4 worldPos : TEXCOORD1;
				float3 normal : TEXCOORD2;
				float3 viewDir : TEXCOORD3;
			};

			vertexOutput vert(vertexInput input)
			{
				vertexOutput output;

				float4x4 modelMatrix = _Object2World;
				float4x4 modelMatrixInverse = _World2Object;

				output.normal = normalize(
					mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);
				output.viewDir = normalize(_WorldSpaceCameraPos
					- mul(modelMatrix, input.vertex).xyz);

				output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
				output.worldPos = mul(_Object2World, input.vertex);

				output.color = input.color;
				output.texcoord = input.texcoord;
//				output.texcoord = TRANSFORM_TEX(input.texcoord, _NormalMap);
				return output;
			}

			float4 frag(vertexOutput input) : COLOR
			{

//				***** DEALING WITH NORMALS *****

				//Get the normal from the bump map
				half4 bump = tex2D(_NormalMap, input.texcoord);
				half3 distortion = normalize(UnpackNormal(bump).rgb);

				//Get the normal from the geometry
				float3 normalDirection = -normalize(input.normal);

				//Find the view direction of the camera
				float3 viewDirection = normalize(input.worldPos - _WorldSpaceCameraPos);//input.viewDir);

				float3 normalWeights = lerp(distortion, normalDirection, _GeometryFactor);

//				if((dot(viewDirection, normalWeights)) < -0.5){
//				return fixed4(0,0,0,1);
//				}
//				return fixed4(1,1,1,1);

				float standardSil = 1 - abs(dot(viewDirection, normalWeights));

				float newOpacity = min(1.0, _Color.a
					/ pow(standardSil, _Intensity));

				newOpacity = clamp(newOpacity, _MinSat, 1);

//				***** DEALING WITH DISTANCE *****

				//Find the distance of the pixel from the camera
				float distFromCamera = distance(input.worldPos, _WorldSpaceCameraPos);

				//Get the distance at which we start the fade
				float fadeDistance = 1/_MaxDist;

				//Get the fade value from the distance to the camera and the fade distance
				//Clamp it between 0 and 1
				//The exponent value allows for non-linear falloff values
				float fadeAmount = clamp(pow((distFromCamera * fadeDistance), _Exponent), 0, 1);

				newOpacity = lerp(newOpacity, 0, fadeAmount);


				return float4(_Color.rgb, newOpacity);
			}

			ENDCG
		}
	}
}
