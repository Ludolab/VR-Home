// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/RSShader" 
{
	Properties {
		_MainTex ("Base (RGB), Alpha (A)", 2D) = "white" {}
		_DepthTex ("Vertex Modify", 2D) = "white" {}
		_ModAmount ("Modulation Amount", float) = 1.0
		_BackgroundSub("Background Point", float) = 0.5
		_DepthScale("Depth Scale", float) = 100
		
	}
	SubShader {
		Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
		}
		
		ZWrite On
		Cull Off
        Blend SrcAlpha OneMinusSrcAlpha
		//Fog { Mode Off }

		CGPROGRAM
		#pragma vertex vert
		#pragma surface surf BlinnPhong// alpha:transparent
		#pragma target 3.0
		#pragma glsl

		sampler2D _MainTex;
		sampler2D _DepthTex;
		sampler2D _BackgroundTex;
		float _ModAmount;
		float _BackgroundSub;
		float _DepthScale;

		struct Input {
			float2 uv_MainTex;
		};
		
		void vert(inout appdata_full v) {
			float4 tex = tex2Dlod(_DepthTex, float4(v.texcoord.xy, 0, 0));

			//v.vertex.y -= smoothstep(0,1,tex.r * _ModAmount);
			float d = tex.r * _DepthScale;
			if (d == 0){
				
				v.vertex.y = -_DepthScale;
			}

			else{
				v.vertex.y -= (d) * _ModAmount;
			}
		}
		
		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			half4 b = tex2D (_DepthTex, IN.uv_MainTex);
			
			o.Albedo = c.rgb;

			float d = clamp((b.r) * _DepthScale, 0, 1.);
			float dd = b.r * _DepthScale;
			o.Alpha = 1;

			if(d > _BackgroundSub){
				discard;
				//o.Alpha = 0;
			}
			else if (dd == 0){
				//o.Alpha = 0;
				discard;
			
			}
			else{
				if (d> _BackgroundSub ){
					//discard;
					o.Alpha = (1-d)/_BackgroundSub;
				}
				else{
					o.Alpha = 1;
				}
				
			}
		}
		ENDCG
	} 
	FallBack "Diffuse"
}