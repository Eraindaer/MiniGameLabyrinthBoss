Shader "Custom/Test Displacement Surface"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Smoothness("Smoothness", Range(0,1)) = 0.0
		_Metallic("Metalness", Range(0,1)) = 0.0
        
		_Amount ("Amount", Range (0,1)) = 0.0
		_DisplacementTexture("Displacement Texture", 2D) = "white" {}
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }

		CGPROGRAM

		#pragma surface surf Standard fullforwardshadows vertex:vert

		sampler2D _MainTex;
		
		struct Input {
			float2 uv_MainTex;
			float displacementValue;
		};

		fixed4 _Color;
		half _Smoothness;
		half _Metallic;
		float _Amount;
		sampler2D _DisplacementTexture;
		
		

		void vert(inout appdata_full v, out Input o) {
			float value = tex2Dlod(_DisplacementTexture, v.texcoord * 7).x * _Amount;

			v.vertex.xyz += v.normal.xyz * value * .3;

			UNITY_INITIALIZE_OUTPUT(Input, o);

			o.displacementValue = value;

			v.vertex.z += sin(_Time.y *0.5) * 0.05;
			v.vertex.y += cos(_Time.y * 10) * 0.1;
			v.vertex.x += sin(_Time.y * 0.3) * 0.05;
		}

		void surf(Input i, inout SurfaceOutputStandard o) {
			fixed4 col = tex2D(_MainTex, i.uv_MainTex) * _Color;

			o.Albedo = lerp(col.rgb * col.a, float3(0, 0, 0), i.displacementValue);

			o.Alpha = col.a;
			o.Smoothness = _Smoothness;
			o.Metallic = _Metallic;
		}

        ENDCG
    }
    FallBack "Diffuse"
}
