Shader "Custom/Essais Shader Surface"
{
    Properties
    {
		_myColor("Main Color", Color) = (1,1,1,1)
		_myEmissionColor("Emissive Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        CGPROGRAM
       
		#pragma surface surf Lambert

		struct Input {
			float2 uvMainTex;
		};

		fixed4 _myColor;
		fixed4 _myEmissionColor;

		void surf(Input IN, inout SurfaceOutput o) {
			o.Albedo = _myColor.rgb;
			o.Emission = _myEmissionColor.rgb;
		}

        ENDCG
    }
    FallBack "Diffuse"
}
