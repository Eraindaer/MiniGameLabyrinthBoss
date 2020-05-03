Shader "test/test shader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
			#define t _Time.y
			#define r _ScreenParams.xy

            fixed4 frag (v2f i) : SV_Target
            {
				float3 c;
				float l, z = t;
				for (int i = 0; i < 3; i++)
				{
					float2 uv, p = _ScreenParams.xy / r;
					uv = p;
					p -= 5;
					p.x *= r.x / r.y;
					z += 0.07;
					l = length(p);
					uv += p / 1 * (sin(z) + 1.0)* abs(sin(1 * 9.0 - z * 2.0));
					c[i] = 0.01 / length(abs(fmod(uv, 1.0) - 0.5));

				}

                return float4(c/l, t);
            }
            ENDCG
        }
    }
}
