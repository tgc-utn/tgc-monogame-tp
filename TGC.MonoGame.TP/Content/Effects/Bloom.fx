#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float4x4 WorldViewProjection;
float3 Color;
float ApplyBloom;
texture baseTexture;
float3 enginesColor1;
float3 enginesColor2;
float3 laserColor1;
float3 laserColor2;
float3 laserColor3;

sampler2D textureSampler = sampler_state
{
    Texture = (baseTexture);
    MagFilter = Linear;
    MinFilter = Linear;
    AddressU = Clamp;
    AddressV = Clamp;
};

texture blurHTexture;
sampler2D blurHSampler = sampler_state
{
    Texture = (blurHTexture);
    MagFilter = Linear;
    MinFilter = Linear;
    AddressU = Clamp;
    AddressV = Clamp;
};
texture blurVTexture;
sampler2D blurVSampler = sampler_state
{
    Texture = (blurVTexture);
    MagFilter = Linear;
    MinFilter = Linear;
    AddressU = Clamp;
    AddressV = Clamp;
};

struct VertexShaderInput
{
    float4 Position : POSITION0;
    float2 TextureCoordinates : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float2 TextureCoordinates : TEXCOORD0;
};

VertexShaderOutput PostProcessVS(in VertexShaderInput input)
{
    VertexShaderOutput output;
    output.Position = input.Position;
    output.TextureCoordinates = input.TextureCoordinates;
    return output;
}

float4 BloomIntegratePS(in VertexShaderOutput input) : COLOR
{    
    float4 blurHColor = tex2D(blurHSampler, input.TextureCoordinates);
    float4 blurVColor = tex2D(blurVSampler, input.TextureCoordinates);
    
    float4 sceneColor = float4(tex2D(textureSampler, input.TextureCoordinates).xyz, 1);
    
    return sceneColor  + blurHColor * 1 + blurVColor * 1;
}

technique Integrate
{
    pass Pass0
    {
        VertexShader = compile VS_SHADERMODEL PostProcessVS();
        PixelShader = compile PS_SHADERMODEL BloomIntegratePS();
    }
};