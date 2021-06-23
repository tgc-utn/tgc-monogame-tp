#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

matrix World;
matrix WorldViewProjection;
matrix LightViewProjection;

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
    float4 Normal : NORMAL0;
    float2 TextureCoordinates : TEXCOORD0;
};

struct VSODraw
{
    float4 Position : SV_POSITION;
    float4 Normal : TEXCOORD0;
    float2 TextureCoordinates : TEXCOORD1;
};

struct PSOMRT
{
    float4 Color : COLOR0;
    float4 Normal : COLOR1;
    float4 Bloom : COLOR2;
};

VSODraw MRTVS(in VSIDraw input)
{
    VSODraw output = (VSODraw) 0;

    output.Position = mul(input.Position, WorldViewProjection);
    output.TextureCoordinates = input.TextureCoordinates;
    
    //output.Normal = World[3,3] * 0;
    //output.Normal += World[2, 3] * 0;
    //output.Normal += World[3, 2] * 0;
    
    output.Normal = input.Normal;
    //output.Normal = float3(0,0,0);
    
    return output;
}

PSOMRT BasicColorPS(VSODraw input) 
{
    PSOMRT output = (PSOMRT) 0;
    

    float3 worldNormal = mul(input.Normal, World);
    output.Color = float4(Color, 1);
    
    output.Normal = float4(0.5f * (normalize(worldNormal) + 1.0f), 1);
    //output.Normal = float4(0, 0, 0, 1);
    output.Bloom = float4(Color * AddToFilter, 1);
    
    return output;
}
PSOMRT TexturedDrawPS(VSODraw input)
{
    PSOMRT output = (PSOMRT) 0;
    
    output.Color = tex2D(textureSampler, input.TextureCoordinates);

    float3 worldNormal = mul(input.Normal, World);
    output.Normal = float4(0.5f * (normalize(worldNormal) + 1.0f), 1);
    
    //output.Normal = float4(0, 0, 0, 1);
    output.Bloom = float4(Color * AddToFilter, 1);
    
    return output;
}


/* SKYBOX */
float3 CameraPosition;

texture SkyBoxTexture;
samplerCUBE SkyBoxSampler = sampler_state
{
    texture = <SkyBoxTexture>;
    magfilter = LINEAR;
    minfilter = LINEAR;
    mipfilter = LINEAR;
    AddressU = Mirror;
    AddressV = Mirror;
};

struct VSIsky
{
    float4 Position : POSITION0;
};
struct VSOsky
{
    float4 Position : POSITION0;
    float3 TextureCoordinates : TEXCOORD0;
};


technique Skybox
{
    pass Pass1
    {
        VertexShader = compile VS_SHADERMODEL SkyboxVS();
        PixelShader = compile PS_SHADERMODEL SkyboxPS();
    }
}
VSOsky SkyboxVS(VSIsky input)
{
    VSOsky output = (VSOsky) 0;

    output.Position = mul(input.Position, WorldViewProjection);
    float4 VertexPosition = mul(input.Position, World);
    output.TextureCoordinates = VertexPosition.xyz - CameraPosition;

    return output;
}

PSOMRT SkyboxPS(VSOsky input)
{
    PSOMRT output = (PSOMRT) 0;
    
    output.Color = float4(texCUBE(SkyBoxSampler, normalize(input.TextureCoordinates)).rgb, 1);;
    output.Bloom = float4(0, 0, 0, 1);
    output.Normal = float4(0, 0, 0, 1);
    //output.Depth = float4(0, 0, 0, 1);
    
    return output;
}


struct VSIdepth
{
    float4 Position : POSITION0;
};
struct VSOdepth
{
    float4 Position : SV_POSITION;
    float4 ScreenPos : TEXCOORD0;
};

technique DepthPass
{
    pass Pass1
    {
        VertexShader = compile VS_SHADERMODEL DepthVS();
        PixelShader = compile PS_SHADERMODEL DepthPS();
    }
}

VSOdepth DepthVS(VSIdepth input)
{
    VSOdepth output = (VSOdepth) 0;
    
    output.Position = mul(input.Position, LightViewProjection);
    output.ScreenPos = mul(input.Position, LightViewProjection);

    return output;
}

float4 DepthPS(VSOdepth input) : COLOR
{
    float depth = input.ScreenPos.z / input.ScreenPos.w;
    return float4(depth, depth, depth, 1.0);
}