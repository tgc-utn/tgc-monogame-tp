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
matrix InverseTransposeWorld;
matrix LightViewProjection;
matrix InvertViewProjection;

float3 CameraPosition;
float3 LightDirection;
float3 Color;
float3 LightColor;
float AddToFilter;

float SpecularIntensity;
float SpecularPower;
float3 AmbientLightColor;
float AmbientLightIntensity;

float modulatedEpsilon = 0.000041200182749889791011810302734375;
float maxEpsilon = 0.000023200045689009130001068115234375;
//float modulatedEpsilon = 0.00001200182749889791011810302734375;
//float maxEpsilon = 0.000000200045689009130001068115234375;


texture Texture;
sampler2D textureSampler = sampler_state
{
    Texture = (Texture);
    MagFilter = Linear;
    MinFilter = Linear;
    AddressU = Clamp;
    AddressV = Clamp;
};
texture ModelNormal;
sampler2D normalSampler = sampler_state
{
    Texture = (ModelNormal);
    MagFilter = Linear;
    MinFilter = Linear;
    AddressU = Clamp;
    AddressV = Clamp;
};

texture ShadowMap;
float2 ShadowMapSize;
sampler shadowSampler = sampler_state
{
    Texture = (ShadowMap);
    AddressU = CLAMP;
    AddressV = CLAMP;
    MagFilter = POINT;
    MinFilter = POINT;
    Mipfilter = POINT;
};
technique TexturedDraw
{
    pass Pass0
    {
        VertexShader = compile VS_SHADERMODEL DrawVS();
        PixelShader = compile PS_SHADERMODEL TexturedDrawPS();
    }
};
technique BasicColorDraw
{
    pass Pass0
    {
        VertexShader = compile VS_SHADERMODEL DrawVS();
        PixelShader = compile PS_SHADERMODEL BasicColorPS();
    }
};
technique TrenchDraw
{
    pass Pass0
    {
        VertexShader = compile VS_SHADERMODEL DrawVS();
        PixelShader = compile PS_SHADERMODEL TrenchPS();
    }
};


struct VSIDraw
{
    float4 Position : POSITION0;
    float3 Normal : NORMAL0;
    float3 Binormal : BINORMAL0;
    float3 Tangent : TANGENT0;
    float2 TextureCoordinates : TEXCOORD0;
};

struct VSODraw
{
    float4 Position : SV_POSITION;
    float2 TextureCoordinates : TEXCOORD0;
    float4 Normal : TEXCOORD1;
    float3 DirToCamera : TEXCOORD2;
    float3x3 WorldToTangentSpace : TEXCOORD3;
    float3 OmniLightColor : TEXCOORD6;
    float4 WorldPosition : TEXCOORD7;
    float4 LightSpacePosition : TEXCOORD8;
};

struct PSOMRT
{
    float4 Color : COLOR0;
    float4 Normal : COLOR1;
    float4 DirToCam : COLOR2;
    float4 Bloom : COLOR3;
};



float ApplyLightEffect;
float3 OmniLightsPos[20];
float3 OmniLightsColor[20];
int OmniLightsCount;
float OmniLightsRadiusMin;
float OmniLightsRadiusMax;

VSODraw DrawVS(in VSIDraw input)
{
    VSODraw output = (VSODraw) 0;

    float4 pos = mul(input.Position, WorldViewProjection);
    output.Position = pos;
    output.TextureCoordinates = input.TextureCoordinates;
    
    float4 worldPos = mul(input.Position, World);
    output.DirToCamera = normalize(float4(CameraPosition, 1.0) - worldPos).xyz;
    
    output.WorldToTangentSpace[0] = mul(normalize(input.Tangent), (float3x3) World);
    output.WorldToTangentSpace[1] = mul(normalize(input.Binormal), (float3x3) World);
    output.WorldToTangentSpace[2] = mul(normalize(input.Normal), (float3x3) World);
    
    float4 normal = float4(normalize(input.Normal), 1.0);
    
    output.Normal = normal;
    output.WorldPosition = worldPos;
    output.OmniLightColor = float3(0, 0, 0);
    
    output.LightSpacePosition = mul(worldPos, LightViewProjection);
    
    return output;
}
PSOMRT BasicColorPS(VSODraw input)
{
    PSOMRT output = (PSOMRT) 0;
    
    output.Color = float4(Color,0);

    output.Normal = float4(0, 0, 0, 0);
    output.DirToCam = float4(0, 0, 0, 0);
    
    output.Bloom = float4(Color * AddToFilter, 0.0);
    
    return output;
}

float getShadow(VSODraw input, float3 normal)
{
    float3 lightSpacePosition = input.LightSpacePosition.xyz / input.LightSpacePosition.w;
    float2 shadowMapTextureCoordinates = 0.5 * lightSpacePosition.xy + float2(0.5, 0.5);
    shadowMapTextureCoordinates.y = 1.0f - shadowMapTextureCoordinates.y;
	
    float inclinationBias = max(modulatedEpsilon * (1.0 - dot(normal, LightDirection)), maxEpsilon);
	
    float shadowMapDepth = 1 - tex2D(shadowSampler, shadowMapTextureCoordinates).r + inclinationBias;
	
	// Compare the shadowmap with the REAL depth of this fragment
	// in light space
    
    
    return 0.25 * step(shadowMapDepth, lightSpacePosition.z);
}

PSOMRT TexturedDrawPS(VSODraw input)
{
    PSOMRT output = (PSOMRT) 0;
    
    float4 texColor = tex2D(textureSampler, input.TextureCoordinates);
    
    //sample normal from texture and convert 
    float3 fromNormalMap = (2.0 * tex2D(normalSampler, input.TextureCoordinates) - 1.0).xyz;
    float3 normal = normalize(mul(fromNormalMap, input.WorldToTangentSpace));
    output.Normal = float4(normal, 1);
 
    output.Color = texColor - getShadow(input, normal);
    
    output.DirToCam = float4(0.5 * (input.DirToCamera + 1), 1); //
    
    output.Bloom = float4(0,0,0, 1);
    
    output.Color.a = input.OmniLightColor.r;
    output.Normal.a = input.OmniLightColor.g;
    output.DirToCam.a = input.OmniLightColor.b;
    
    return output;
}

PSOMRT TrenchPS(VSODraw input)
{
    PSOMRT output = (PSOMRT) 0;
    
    float3 worldNormal = mul(input.Normal, InverseTransposeWorld).xyz;
    
    output.Normal = float4(0.5 * (worldNormal + 1.0), 1);
   
    float nDotLeft = dot(worldNormal, float3(-1.0, 0, 0.0));
    
    if (nDotLeft >= 0 && nDotLeft <= 0.10)
        output.Color = float4(Color * 0.8, 1);
    else
        output.Color = float4(Color, 1);
    
    output.Color -= getShadow(input, worldNormal);
    
    output.Bloom = float4(0,0,0, 1);
    
    output.DirToCam = float4(0.5 * (input.DirToCamera + 1), 1);
    
    output.Color.a = input.OmniLightColor.r;
    output.Normal.a = input.OmniLightColor.g;
    output.DirToCam.a = input.OmniLightColor.b;
    
    return output;
}

/* SKYBOX */
texture SkyBoxTexture;
samplerCUBE SkyBoxSampler = sampler_state
{
    texture = (SkyBoxTexture);
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
    float4 Position : SV_POSITION;
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
    
    output.Color = float4(texCUBE(SkyBoxSampler, normalize(input.TextureCoordinates)).rgb, 1);
    output.Bloom = float4(0, 0, 0, 0.0);
    output.DirToCam = float4(1, 0, 1, 0);
    output.Normal= float4(0, 0, 1, 0);
    
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
    
    float4 worldPosition = mul(input.Position, World);
    output.Position = mul(worldPosition, LightViewProjection);
    output.ScreenPos = mul(worldPosition, LightViewProjection);

    return output;
}
float4 DepthPS(VSOdepth input) : COLOR
{
    float depth = 1 - (input.ScreenPos.z / input.ScreenPos.w);
    return float4(depth, depth, depth, 1.0);
}

/* DIRECTIONAL LIGHT CALC AND INT */
texture ColorMap;
sampler colorSampler = sampler_state
{
    Texture = (ColorMap);
    AddressU = CLAMP;
    AddressV = CLAMP;
    MagFilter = LINEAR;
    MinFilter = LINEAR;
    Mipfilter = LINEAR;
};
texture DirToCamMap;
sampler dirToCamSampler = sampler_state
{
    Texture = (DirToCamMap);
    AddressU = CLAMP;
    AddressV = CLAMP;
    MagFilter = POINT;
    MinFilter = POINT;
    Mipfilter = POINT;
};

texture NormalMap;
sampler normalMapSampler = sampler_state
{
    Texture = (NormalMap);
    AddressU = CLAMP;
    AddressV = CLAMP;
    MagFilter = POINT;
    MinFilter = POINT;
    Mipfilter = POINT;
};
texture BloomFilter;
sampler bloomFilterSampler = sampler_state
{
    Texture = (BloomFilter);
    AddressU = CLAMP;
    AddressV = CLAMP;
    MagFilter = POINT;
    MinFilter = POINT;
    Mipfilter = POINT;
};

struct DLightVSI
{
    float3 Position : POSITION0;
    float2 TexCoord : TEXCOORD0;
};

struct DLightVSO
{
    float4 Position : POSITION0;
    float2 TexCoord : TEXCOORD0;
};

DLightVSO DLightVS(DLightVSI input)
{
    DLightVSO output;
    output.Position = float4(input.Position, 1);
    output.TexCoord = input.TexCoord;
    return output;
}


float4 DLightPS(DLightVSO input) : COLOR0
{
    float applyLighting = tex2D(bloomFilterSampler, input.TexCoord).w;
    
    if (applyLighting == 0.0)
        return float4(1, 1, 1, 0);
    
    //get original pixel color
    float4 colorMap = tex2D(colorSampler, input.TexCoord);
    
    float3 texColor = colorMap.rgb;
        
    //get normal data from the normalMap
    float4 normalData = tex2D(normalMapSampler, input.TexCoord);
    //tranform normal back into [-1,1] range
    float3 normal = 2.0f * normalData.xyz - 1.0;
    
    //get dir to cam map
    float4 dirToCamMap = tex2D(dirToCamSampler, input.TexCoord);
    
    float3 OmniLight = float3(colorMap.a, normalData.a, dirToCamMap.a);
    
    //surface-to-light vector
    float3 lightVector = -normalize(LightDirection);

    //compute diffuse light (directional)
    float NdL = max(0, dot(normal, lightVector));
    float3 directionalLight = NdL * LightColor;
    
 
    //float3 diffuseLight = directionalLight + omniLights;
    float3 diffuseLight = directionalLight + OmniLight;
    
    //reflexion vector
    float3 reflectionVector = normalize(reflect(-lightVector, normal));
    //convert back to [-1, 1]
    float3 directionToCamera = 2.0 * dirToCamMap.xyz - 1.0;
    
    //compute specular light
    float specularLight = SpecularIntensity * pow(saturate(dot(reflectionVector, directionToCamera)), SpecularPower);

    return float4(AmbientLightColor * AmbientLightIntensity + diffuseLight, specularLight);
}

texture LightMap;
sampler lightSampler = sampler_state
{
    Texture = (LightMap);
    AddressU = CLAMP;
    AddressV = CLAMP;
    MagFilter = LINEAR;
    MinFilter = LINEAR;
    Mipfilter = LINEAR;
};

float4 IntLightPS(DLightVSO input) : COLOR
{
    float3 diffuseColor = tex2D(colorSampler, input.TexCoord).rgb;
    float4 light = tex2D(lightSampler, input.TexCoord);
    float3 diffuseLight = light.rgb;
    float specularLight = light.a;
    return float4((diffuseColor * diffuseLight + specularLight), 1);
}

technique DirectionalLight
{
    pass Pass0
    {
        VertexShader = compile VS_SHADERMODEL DLightVS();
        PixelShader = compile PS_SHADERMODEL DLightPS();
    }
}

technique IntegrateLight
{
    pass Pass0
    {
        VertexShader = compile VS_SHADERMODEL DLightVS();
        PixelShader = compile PS_SHADERMODEL IntLightPS();
    }
}