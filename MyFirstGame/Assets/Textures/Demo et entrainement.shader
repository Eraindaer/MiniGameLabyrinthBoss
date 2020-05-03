// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Demo et entrainement"
{
    Properties
    {
	    _MainTex("Main Texture", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
		_SecondTex("Second Texture", 2D) = "white" {}
		_SecondColor("Second Color", Color) = (1,1,1,1)
		_Tween("Tween", Range(0, 1)) = 0
		_Saturation("Blue-ness", Range(0,1)) = 0.0
		_WaveStrength("Strength", Range(0,10)) = 1.0
	}
    SubShader
    {
		Tags{
			"Queue" = "Transparent"
		}

		Pass
		{
	Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			float _WaveStrength;

			v2f vert(appdata v) {
				v2f o;
				float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
				o.vertex = UnityObjectToClipPos(v.vertex);
				float4 displacement = (sin(worldPos.z *_Time.y* 0.025)*.5 + .5);
				const float4 posX = worldPos.x;
				worldPos.x = posX + displacement * _WaveStrength;
				o.vertex = mul(UNITY_MATRIX_VP, worldPos);
				o.uv = v.uv;
				return o;
			}

			sampler2D _MainTex;
			sampler2D _SecondTex;
			fixed4 _Color;
			fixed4 _SecondColor;
			float _Saturation;
			float _Tween;

			float4 frag(v2f i) : SV_Target{
				float4 mainColor = tex2D(_MainTex, i.uv);
				float4 secondColor = tex2D(_SecondTex, i.uv);
				return mainColor * _Color * float4(i.uv.x, _Saturation, i.uv.y, 1);
			}

		ENDCG
		}
    }
}
