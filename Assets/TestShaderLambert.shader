Shader "Coucou bite!"{
	Properties{
		_Color ("Color", Color) = (1.0, 1.0, 1.0, 1.0)
	}
	
	SubShader{
		Pass{
			Tags { "LightMode" = "ForwardBase" }
			
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			
			//User defined variables
			uniform float4 _Color;
			
			//Unity defined variables
			uniform float4 _LightColor0;
			
			//base input structs
			struct vertexInput{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};
			
			struct vertexOutput{
				float4 pos : SV_POSITION;
				float4 col : COLOR;
			};
			
			//vertex function
			vertexOutput vert(vertexInput v){
				vertexOutput o;
				
				//Retrieve vertex data
				float3 normalDirection = normalize( mul( float4( v.normal, 0.0), _World2Object).xyz );
				
				//Retrieve light data
				float3 lightDirection;
				float atten = 1.0;
				lightDirection = normalize(_WorldSpaceLightPos0.xyz);
				
				//Diffuse sans ambient
				float3 diffuseReflection = atten * _LightColor0.xyz * max( 0.0, dot(normalDirection, lightDirection));
				
				//ajout de l'ambient 
				diffuseReflection += UNITY_LIGHTMODEL_AMBIENT.xyz;
				
				//modification de la couleur
				o.col = float4(diffuseReflection * _Color.rgb, 1.0);
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				return o;
			}
			
			//fragment function
			float4 frag(vertexOutput i) : COLOR{
				
				return i.col;
			}
			
			ENDCG
		}
	}
	// FallBack
	//FallBack "Diffuse"
}