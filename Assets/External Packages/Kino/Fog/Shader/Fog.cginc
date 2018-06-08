float _FogDistanceOffset;
float _FogDensity;
float _FogLinearGrad;
float _FogLinearOffs;

#include "Assets/Shaders/Vertical Gradient Skybox/VerticalGradientSkybox.cginc"

// Fog/skybox information
float4 _FogColor;

// Applies one of standard fog formulas, given fog coordinate (i.e. distance)
half ComputeFogFactor(float coord)
{
    coord = max(0.0, coord - _FogDistanceOffset);
    float fog = 0.0;
#if KINO_FOG_LINEAR
    // factor = (end-z)/(end-start) = z * (-1/(end-start)) + (end/(end-start))
    fog = coord * _FogLinearGrad + _FogLinearOffs;
#elif KINO_FOG_EXP
    // factor = exp(-density*z)
    fog = _FogDensity * coord;
    fog = exp2(-fog);
#else // KINO_FOG_EXP2
    // factor = exp(-(density*z)^2)
    fog = _FogDensity * coord;
    fog = exp2(-fog * fog);
#endif
    return saturate(fog);
}

// Distance-based fog
float ComputeDistance(float3 ray, float depth)
{
    float dist;
#if KINO_FOG_RADIAL_DIST
    dist = length(ray * depth);
#else
    dist = depth * _ProjectionParams.z;
#endif
    // Built-in fog starts at near plane, so match that by
    // subtracting the near value. Not a perfect approximation
    // if near plane is very large, but good enough.
    dist -= _ProjectionParams.y;
    return dist;
}
