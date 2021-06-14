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


VertexShaderOutput MainVS(in VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput) 0;

    output.Position = mul(input.Position, WorldViewProjection);
    output.TextureCoordinates = input.TextureCoordinates;
	
    return output;
}

float4 BloomModelPS(VertexShaderOutput input) : COLOR
{
    return float4(Color.rgb * ApplyBloom, 1);
}

float4 BloomPS(VertexShaderOutput input) : COLOR
{
    float4 color = tex2D(textureSampler, input.TextureCoordinates);
    float errorDelta = 0.2;
    float distanceToTargetColor = distance(color.rgb, enginesColor1);
    float distanceToTargetColor2 = distance(color.rgb, enginesColor2);
    float distanceToTargetColor3 = distance(color.rgb, laserColor1);
    float distanceToTargetColor4 = distance(color.rgb, laserColor2);
    float distanceToTargetColor5 = distance(color.rgb, laserColor3);
    
    float filter =  step(distanceToTargetColor, errorDelta) ||
                    step(distanceToTargetColor2, errorDelta) ||
                    step(distanceToTargetColor3, errorDelta) ||
                    step(distanceToTargetColor4, errorDelta) ||
                    step(distanceToTargetColor5, errorDelta);
    //float filter = step(distanceToTargetColor4, errorDelta);
    return float4(color.rgb * filter, 1);
}


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

technique BloomModelPass
{
    pass Pass0
    {
        VertexShader = compile VS_SHADERMODEL MainVS();
        PixelShader = compile PS_SHADERMODEL BloomModelPS();
    }
};

technique BloomPixelPass
{
    pass Pass0
    {
        VertexShader = compile VS_SHADERMODEL MainVS();
        PixelShader = compile PS_SHADERMODEL BloomPS();
    }
};



technique Integrate
{
    pass Pass0
    {
        VertexShader = compile VS_SHADERMODEL PostProcessVS();
        PixelShader = compile PS_SHADERMODEL BloomIntegratePS();
    }
};