#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float4x4 World;
float4x4 View;
float4x4 Projection;
float3 CameraPosition;
float Time = 0;

struct VertexShaderInput
{
    float4 Position : POSITION0;
    float4 Color : COLOR0;
    float2 TextureCoordinate : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float2 TextureCoordinate : TEXCOORD1;
};

texture SkyDomeTexture;
sampler2D SkyDomeSampler = sampler_state
{
    Texture = <SkyDomeTexture>;
    MagFilter = Linear;
    MinFilter = Linear;
    AddressU = Mirror;
    AddressV = Mirror;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput)0;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);

    //float4 VertexPosition = mul(input.Position, World);
    output.TextureCoordinate = input.TextureCoordinate;

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    float4 textureColor = float4(tex2D(SkyDomeSampler, input.TextureCoordinate).rgb, 1);
    
    float fade = clamp(cos(Time * 0.3) + 0.5, 0.15, 1);
    textureColor = textureColor * float4(fade, fade, fade, 1);

    return textureColor;
}

technique SkyDome
{
    pass Pass1
    {
        VertexShader = compile VS_SHADERMODEL VertexShaderFunction();
        PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
    }
}
