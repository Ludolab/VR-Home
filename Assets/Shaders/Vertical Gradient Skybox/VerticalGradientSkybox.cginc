float4 _FirstGradient1;
float4 _FirstGradient2;
float4 _FirstGradient3;
float4 _FirstGradient4;
float4 _FirstGradientKeys;
float4 _SecondGradient1;
float4 _SecondGradient2;
float4 _SecondGradient3;
float4 _SecondGradient4;
float4 _SecondGradientKeys;
float4 _ThirdGradient1;
float4 _ThirdGradient2;
float4 _ThirdGradient3;
float4 _ThirdGradient4;
float4 _ThirdGradientKeys;
float4 _FourthGradient1;
float4 _FourthGradient2;
float4 _FourthGradient3;
float4 _FourthGradient4;
float4 _FourthGradientKeys;

float _Progress;

half4 EvaluateFirstGradient(float value)
{
    return lerp(_FirstGradient1, 
        lerp(_FirstGradient2,
            lerp(_FirstGradient3,
                _FirstGradient4,
                clamp((value - _FirstGradientKeys.z)/(_FirstGradientKeys.w - _FirstGradientKeys.z), 0, 1)),
            clamp((value - _FirstGradientKeys.y)/(_FirstGradientKeys.z - _FirstGradientKeys.y), 0, 1)),
        clamp((value - _FirstGradientKeys.x)/(_FirstGradientKeys.y - _FirstGradientKeys.x), 0, 1));
}
half4 EvaluateSecondGradient(float value)
{
    return lerp(_SecondGradient1, 
        lerp(_SecondGradient2,
            lerp(_SecondGradient3,
                _SecondGradient4,
                clamp((value - _SecondGradientKeys.z)/(_SecondGradientKeys.w - _SecondGradientKeys.z), 0, 1)),
            clamp((value - _SecondGradientKeys.y)/(_SecondGradientKeys.z - _SecondGradientKeys.y), 0, 1)),
        clamp((value - _SecondGradientKeys.x)/(_SecondGradientKeys.y - _SecondGradientKeys.x), 0, 1));
}
half4 EvaluateThirdGradient(float value)
{
    return lerp(_ThirdGradient1, 
        lerp(_ThirdGradient2,
            lerp(_ThirdGradient3,
                _ThirdGradient4,
                clamp((value - _ThirdGradientKeys.z)/(_ThirdGradientKeys.w - _ThirdGradientKeys.z), 0, 1)),
            clamp((value - _ThirdGradientKeys.y)/(_ThirdGradientKeys.z - _ThirdGradientKeys.y), 0, 1)),
        clamp((value - _ThirdGradientKeys.x)/(_ThirdGradientKeys.y - _ThirdGradientKeys.x), 0, 1));
}
half4 EvaluateFourthGradient(float value)
{
    return lerp(_FourthGradient1, 
        lerp(_FourthGradient2,
            lerp(_FourthGradient3,
                _FourthGradient4,
                clamp((value - _FourthGradientKeys.z)/(_FourthGradientKeys.w - _FourthGradientKeys.z), 0, 1)),
            clamp((value - _FourthGradientKeys.y)/(_FourthGradientKeys.z - _FourthGradientKeys.y), 0, 1)),
        clamp((value - _FourthGradientKeys.x)/(_FourthGradientKeys.y - _FourthGradientKeys.x), 0, 1));
}

fixed4 SkyColor(float3 ray)
{
    float value = (ray.y - 1.0) * .5;
    value = -value;
    float3 t;
    t.x = clamp(_Progress * 3, 0, 1);
    t.y = clamp((_Progress - .333333333333) * 3, 0, 1);
    t.z = clamp((_Progress - .666666666667) * 3, 0, 1);
    fixed4 c = lerp(EvaluateFirstGradient(value),
        lerp(EvaluateSecondGradient(value),
            lerp(EvaluateThirdGradient(value), EvaluateFourthGradient(value),
                t.z),
            t.y),
        t.x);
    return c;
}
