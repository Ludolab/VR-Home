Shader "Custom/Fruit" {
     Properties {
         _MainTex ("Skin Texture (RGB)", 2D) = "white" {}
         _GuideTex ("Color Guide (RGB)", 2D) = "white" {}
         _Color1 ("Color 1", Color) = (.34, .85, .92, 1)
         _Color2 ("Color 2", Color) = (.82, .18, .61, 1)
     }
     SubShader {
         Tags { "RenderType"="Opaque" }
         LOD 200
         
         CGPROGRAM
         #pragma surface surf Lambert
 
         sampler2D _MainTex;
         sampler2D _GuideTex;
		 half4 _Color1;
		 half4 _Color2;
 
         struct Input {
             float2 uv_MainTex;
             float2 uv_GuideTex;
         };
 
         void surf (Input IN, inout SurfaceOutput o) {
             half4 c = tex2D (_MainTex, IN.uv_MainTex);
			 half4 g = tex2D (_GuideTex, IN.uv_GuideTex);
			 float t = g.a;
			 o.Albedo = (_Color1 * t + _Color2 * (1 - t)) * c;
             o.Alpha = c.a;
         }
         ENDCG
     } 
     FallBack "Diffuse"
 }