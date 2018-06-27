Shader "Custom/Transition" {
     Properties {
         _MainTex ("Default (RGB)", 2D) = "white" {}
         _DisplayTex ("Display (RGB)", 2D) = "white" {}
         _Guide ("Guide (RGB)", 2D) = "white" {}
         _Threshold("Threshold",Range(0,1))=0
         _ScaleWidth("Scale Width", Float)=0
         _ScaleHeight("Scale Height", Float)=0
     }
     SubShader {
         Tags { "RenderType"="Opaque" }
         LOD 200
         
         CGPROGRAM
         #pragma surface surf Lambert
 
         sampler2D _MainTex;
         sampler2D _DisplayTex;
         sampler2D _Guide;
         float _Threshold;
         float _ScaleWidth;
         float _ScaleHeight;
		 float4 _DisplayTex_TexelSize;
 
         struct Input {
             float2 uv_MainTex;
             float2 uv_DisplayTex;
             float2 uv_Guide;
         };
 
         void surf (Input IN, inout SurfaceOutput o) {
             half4 c = tex2D (_MainTex, IN.uv_MainTex);

			 float width = _ScaleWidth / _DisplayTex_TexelSize.z;// * _ScaleWidth;
			 float height = _ScaleHeight / _DisplayTex_TexelSize.w;// * _ScaleHeight;
			 float newU = IN.uv_DisplayTex.x;
			 float newV = IN.uv_DisplayTex.y;
			 float aspect;
			 if(width < height) {
			     //height is big- keep the height and make it wider
			     aspect = height / width;
				 newU = newU / aspect + 0.5f - 0.5f / aspect;
			 }
			 else {
                 //width is big- keep the width and make it taller
			     aspect = width / height;
				 newV = newV / aspect + 0.5f - 0.5f / aspect;
			 }
			 float2 newUV = float2(1 - newU, 1 - newV); //rotate the image 180 degrees for some reason

             half4 d = tex2D (_DisplayTex, newUV);
             half4 g = tex2D (_Guide, IN.uv_Guide); //TODO: make this adjusted UVs too
             if((g.r+g.g+g.b)*0.33333f<_Threshold)
                 o.Albedo = d.rgb;
             else
                 o.Albedo = c.rgb;
             o.Alpha = c.a;
         }
         ENDCG
     } 
     FallBack "Diffuse"
 }