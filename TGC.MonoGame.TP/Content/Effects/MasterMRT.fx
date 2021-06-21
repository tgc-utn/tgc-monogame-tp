#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float4x4 World;
float4x4 WorldViewProjection;
float3 Color;
float AddToFilter;
texture Texture;


sampler2D textureSampler = sampler_state
{
    Texture = (Texture);
    MagFilter = Linear;
    MinFilter = Linear;
    AddressU = Clamp;
    AddressV = Clamp;
};


    


technique TexturedDraw
{
    pass Pass0
    {
        VertexShader = compile VS_SHADERMODEL MRTVS();
        PixelShader = compile PS_SHADERMODEL TexturedDrawPS();
    }
};
technique BasicColorDraw
{
    pass Pass0
    {
        VertexShader = compile VS_SHADERMODEL MRTVS();
        PixelShader = compile PS_SHADERMODEL BasicColorPS();
    }
};


struct VSIDraw
{
    float4 Position : POSITION0;
    float3 Normal : NORMAL0;
    float2 TextureCoordinates : TEXCOORD0;
};

struct VSODraw
{
    float4 Position : SV_POSITION;
    float3 Normal : TEXCOORD0;
    float4 ScreenPos : TEXCOORD1;
    float2 TextureCoordinates : TEXCOORD2;
};

struct PSOMRT
{
    float4 Color : COLOR0;
    float4 Normal : COLOR1;
    float4 Depth : COLOR2;
    float4 Bloom : COLOR3;
};

VSODraw MRTVS(in VSIDraw input)
{
    VSODraw output = (VSODraw) 0;

    output.Position = mul(input.Position, WorldViewProjection);
    output.TextureCoordinates = input.TextureCoordinates;
    
    //output.Normal = World[3,3] * 0;
    //output.Normal += World[2, 3] * 0;
    //output.Normal += World[3, 2] * 0;
    
    //output.Normal = mul(input.Normal, (float3x3) World);
    output.Normal = float3(0,0,0);
    output.ScreenPos = output.Position;
    
    return output;
}

PSOMRT BasicColorPS(VSODraw input) 
{
    PSOMRT output = (PSOMRT) 0;
    
    output.Color = float4(Color, 1);
    //output.Normal.xyz = input.Normal / 2.0 + 0.5;
    output.Normal = float4(0, 0, 0, 1);
    float depth = input.ScreenPos.z / input.ScreenPos.w;
    output.Depth = float4(depth, depth, depth, 1);
    output.Bloom = float4(Color * AddToFilter, 1);
    
    return output;
}
PSOMRT TexturedDrawPS(VSODraw input)
{
    PSOMRT output = (PSOMRT) 0;
    
    output.Color = tex2D(textureSampler, input.TextureCoordinates);
    //output.Normal.xyz = input.Normal / 2.0 + 0.5;
    output.Normal = float4(0, 0, 0, 1);
    output.Depth = input.ScreenPos.z / input.ScreenPos.w;
    output.Bloom = float4(Color * AddToFilter, 1);
    
    return output;
}

technique Integrate
{
    pass Pass0
    {
        VertexShader = compile VS_SHADERMODEL PostProcessVS();
        PixelShader = compile PS_SHADERMODEL BloomIntegratePS();
    }
};

texture bloomTexture;
sampler2D bloomTextureSampler = sampler_state
{
    Texture = (bloomTexture);
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
    float4 bloomColor = tex2D(bloomTextureSampler, input.TextureCoordinates);
    float4 sceneColor = tex2D(textureSampler, input.TextureCoordinates);
    
    return sceneColor * 0.8 + bloomColor * 2;
}



