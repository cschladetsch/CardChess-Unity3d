Shader "Cogobyte/CogobyteSelfLitShader" {
Properties{
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
		float3 viewDir;
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

	half _Glossiness;
	half _Metallic;

	void surf(Input IN, inout SurfaceOutputStandard o)
	{
		half rim = 1 - saturate(dot(float3(0,1,0),o.Normal));
		rim = rim<0.8?0.8:rim;
		o.Emission =  rim * IN.vertexColor.rgb; 
		o.Alpha = IN.vertexColor.a;
	}
	ENDCG
	}
	FallBack "Diffuse"
}
