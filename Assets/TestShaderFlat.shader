Shader "Coucou" {

	Properties {
		_Color ("Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_RimPower ("Rim Power", Range(1.0, 3.0)) = 10.0
		_FillEffect ("Fill Effect", Range(1.0, 2.5)) = 1.0
	
	}
	
	SubShader {
	
		Pass {
		
			CGPROGRAM
			
			//pragmas
			#pragma vertex vert
			#pragma fragment frag
			
			//user defined variables
			uniform float4 _Color;
			uniform float _RimPower;
			uniform float _FillEffect;
			
			//base input structs
			struct vertexInput{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			
			}; 
			
			struct vertexOutput{
				float4 pos : SV_POSITION;
				float4 posWorld : TEXCOORD0;
				float3 normalDir : TEXCOORD1;
			};
			
			//vertex function
			vertexOutput vert(vertexInput v){
				vertexOutput o;
				o.posWorld = mul (_Object2World, v.vertex);
				o.normalDir = normalize( mul( float4 ( v.normal, 0.0 ), _World2Object ).xyz );
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				
				return o;
			}
			
			//fragment function
			float4 frag(vertexOutput i) : COLOR{
			
				float3 normalDirection = i.normalDir;
				float3 viewDirection = normalize( _WorldSpaceCameraPos.xyz - i.posWorld.xyz );
			
				float rim =  1 - saturate(dot ( viewDirection, normalDirection)) / _FillEffect;
				
				float3 rimLighting = pow(rim, _RimPower) * _Color;
		
				return float4(rimLighting, 1.0);
			}
			
			
			ENDCG
		
		}
	
	}
	
	//fallback commented out when under development
	Fallback "Diffuse"

}