Shader "Tutorials/Waves"
{
	Properties
	{
		_MainTexture("Main Texture", 2D) = "white" {}
		_Color("Color", Color) = (0,0,0,1)
		_Strength("Strength", Range(0,20)) = 1.0
		_Speed("Speed", Range(-200, 200)) = 100

	}

		SubShader
		{
			Tags
			{
				"RenderType" = "transparent"
			}
			Pass
			{
			Cull Off

			CGPROGRAM

			#pragma vertex vertexFunc
			#pragma fragment fragFunc

		sampler2D _MainTexture;
		float4 _Color;
		float _Strength;
		float _Speed;

		struct vertexInput {
			float4 vertex : POSITION;
		};

		struct vertexOutput {
			float4 pos : SV_POSITION;
			float4 uv : TEXCOORD0;
		};

		vertexOutput vertexFunc(vertexInput IN) {
			vertexOutput o;

			float4 worldPos = mul(unity_ObjectToWorld, IN.vertex);
			float displacement = sin((cos(worldPos.x * _Time * _Speed) + sin(worldPos.z * _Time * _Speed) + sin(worldPos.x * _Time * _Speed) + cos (worldPos.z * _Time * _Speed)) + cos(cos(worldPos.x * _Time * _Speed) + sin(worldPos.z * _Time * _Speed) + sin(worldPos.x * _Time * _Speed) + cos(worldPos.z * _Time * _Speed))) *0.25 -1.25;
			worldPos.y = worldPos.y + (displacement * _Strength);

			o.pos = mul(UNITY_MATRIX_VP, worldPos);

			return o;
		}

		float4 fragFunc(vertexOutput IN) : COLOR{

			fixed4 pixelColor = tex2D(_MainTexture, IN.uv);

			return _Color * pixelColor;
		}

		ENDCG
		}
	}
}