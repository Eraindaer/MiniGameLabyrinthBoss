Shader "Custom/Test Surface Shader"
{
	Properties{
		_Color("Tint", Color) = (0, 0, 0, 1)
		_MainTex("Texture", 2D) = "white" {}
		
	}
	SubShader{
		Tags{ "RenderType" = "Opaque" }

		CGPROGRAM

		#pragma surface surf Standard fullforwardshadows vertex:vert

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float multiplyValue;
		};
		
		fixed4 _Color;

		void vert(inout appdata_full v, out Input o) {
			
			float multiplyValue = abs(sin(_Time * 30 + v.vertex.y));

			v.vertex.x *= multiplyValue * v.normal.x;
			v.vertex.z *= multiplyValue * v.normal.y;

			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.multiplyValue = multiplyValue;
		}


		void surf(Input i, inout SurfaceOutputStandard o) {
			fixed4 col = tex2D(_MainTex, i.uv_MainTex);
			col *= _Color;
			o.Albedo = lerp(col.rgb, float3(.3,.3,1), i.multiplyValue);
			o.Alpha = col.a;
		}

		ENDCG
	}
	FallBack "Diffuse"
}
