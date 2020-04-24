Shader "Cogobyte/CogobyteVertexShader" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
	    _Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
	}
		SubShader{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		LOD 200
		Pass{
			ZWrite On
			ColorMask 0
		}

		CGPROGRAM
#pragma surface surf Standard vertex:vert alpha:fade fullforwardshadows
#pragma target 3.0
		struct Input {
		float2 uv_MainTex;
		float4 vertexColor; 
	};

	struct v2f {
		float4 pos : SV_POSITION;
		float4 color : COLOR;
	};

	void vert(inout appdata_full v, out Input o)
	{
		UNITY_INITIALIZE_OUTPUT(Input,o);
		o.vertexColor = v.color; 
	}

	sampler2D _MainTex;

	half _Glossiness;
	half _Metallic;
	fixed4 _Color;

	void surf(Input IN, inout SurfaceOutputStandard o)
	{
		// Albedo comes from a texture tinted by color
		fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
		o.Albedo = c.rgb * IN.vertexColor.rgb; // Combine normal color with the vertex color
		o.Metallic = _Metallic;
		o.Smoothness = _Glossiness;
		o.Alpha = IN.vertexColor.a;
	}
	ENDCG
	}
		FallBack "Diffuse"
}
