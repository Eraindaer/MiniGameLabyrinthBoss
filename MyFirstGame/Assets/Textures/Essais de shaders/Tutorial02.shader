Shader "Tutorials/Tutorial02"
{
	Properties
	{
		_MainTexture("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1.0,1.0,1.0,1.0)
		_AnimationSpeed("AnimationSpeed", Range(0,3)) = 0
		_OffsetSize("OffsetSize", Range(0,10)) = 0
	}

	SubShader
	{
		Pass
		{
			CGPROGRAM

			#pragma vertex vertexFunc
			#pragma fragment fragmentFunc

			#include "UnityCG.cginc"

			struct appdata {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};
			
			struct v2f {
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			fixed4 _Color;
			sampler2D _MainTexture;
			float _AnimationSpeed;
			float _OffsetSize;

			v2f vertexFunc(appdata IN) 
			{
				v2f OUT;
				IN.vertex.y += sin(_Time.y * _AnimationSpeed + IN.vertex.x * _OffsetSize + IN.vertex.z * _OffsetSize);
				IN.vertex.y -= 1;
				
				OUT.position = UnityObjectToClipPos(IN.vertex);
				OUT.uv = IN.uv;

				return OUT;
			}

			fixed4 fragmentFunc(v2f IN) : SV_Target
			{
				fixed4 pixelColor = tex2D(_MainTexture, IN.uv);

				return pixelColor * _Color;
			}

			ENDCG
		}

	}
}