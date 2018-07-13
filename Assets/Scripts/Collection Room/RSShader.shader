// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/RSShader" 
{
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_DepthTex ("Vertex Modify", 2D) = "white" {}
		_ModAmount ("Modulation Amount", float) = 1.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		
		CGPROGRAM
		#pragma surface surf Lambert vertex:vert
		#pragma target 3.0
		#pragma glsl

		sampler2D _MainTex;
		sampler2D _DepthTex;
		float _ModAmount;

		struct Input {
			float2 uv_MainTex;
		};
		
		void vert(inout appdata_full v) {
			float4 tex = tex2Dlod(_DepthTex, float4(v.texcoord.xy, 0, 0));
			v.vertex.y -= tex.x * _ModAmount;
		}
		
		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}