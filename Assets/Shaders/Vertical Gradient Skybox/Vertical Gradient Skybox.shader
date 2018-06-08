Shader "Custom/Vertical Gradient Skybox"
{
    Properties
    {
        [HDR] _FirstGradient1   ("First Gradient 1", Color) = (0, 0, 0, 1)
        [HDR] _FirstGradient2   ("First Gradient 2", Color) = (1, 0, 0, 1)
        [HDR] _FirstGradient3   ("First Gradient 3", Color) = (0, 1, 0, 1)
        [HDR] _FirstGradient4   ("First Gradient 4", Color) = (0, 0, 1, 1)
        _FirstGradientKeys      ("First Gradient Keys", Vector) = (0, .3333, .6667, 1)
        [HDR] _SecondGradient1   ("Second Gradient 1", Color) = (.5, 0, 1, 1)
        [HDR] _SecondGradient2   ("Second Gradient 2", Color) = (1, 0, .5, 1)
        [HDR] _SecondGradient3   ("Second Gradient 3", Color) = (0, .5, 1, 1)
        [HDR] _SecondGradient4   ("Second Gradient 4", Color) = (1, .5, 0, 1)
        _SecondGradientKeys      ("Second Gradient Keys", Vector) = (0, .3333, .6667, 1)
        [HDR] _ThirdGradient1   ("Third Gradient 1", Color) = (.5, 1, .5, 1)
        [HDR] _ThirdGradient2   ("Third Gradient 2", Color) = (1, .5, .5, 1)
        [HDR] _ThirdGradient3   ("Third Gradient 3", Color) = (.5, .5, 1, 1)
        [HDR] _ThirdGradient4   ("Third Gradient 4", Color) = (.5, .5, .5, 1)
        _ThirdGradientKeys      ("Third Gradient Keys", Vector) = (0, .3333, .6667, 1)
        [HDR] _FourthGradient1   ("Fourth Gradient 1", Color) = (1, 1, 1, 1)
        [HDR] _FourthGradient2   ("Fourth Gradient 2", Color) = (1, 1, 0, 1)
        [HDR] _FourthGradient3   ("Fourth Gradient 3", Color) = (0, 1, 1, 1)
        [HDR] _FourthGradient4   ("Fourth Gradient 4", Color) = (1, 0, 1, 1)
        _FourthGradientKeys      ("Fourth Gradient Keys", Vector) = (0, .3333, .6667, 1)
        _Progress      ("Progress", Range(0, 1)) = 0
    }

    CGINCLUDE

    #include "UnityCG.cginc"
    #include "VerticalGradientSkybox.cginc"

    struct appdata_t {
        float4 vertex : POSITION;
        UNITY_VERTEX_INPUT_INSTANCE_ID
    };

    struct v2f {
        float4 vertex : SV_POSITION;
        float3 texcoord : TEXCOORD0;
        UNITY_VERTEX_OUTPUT_STEREO
    };

    v2f vert (appdata_t v)
    {
        v2f o;
        UNITY_SETUP_INSTANCE_ID(v);
        UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
        o.vertex = UnityObjectToClipPos(v.vertex);
        o.texcoord = v.vertex.xyz;

        return o;
    }

    fixed4 frag (v2f i) : SV_Target
    {
        fixed4 c = SkyColor(i.texcoord.xyz);
        return c;
    }

    ENDCG

    SubShader
    {
        Tags { "Queue"="Background" "RenderType"="Background" "PreviewType"="Skybox" }
        Cull Off ZWrite Off
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            ENDCG
        }
    }
}
