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
technique TexturedDraw
{
    pass Pass0
    {
        VertexShader = compile VS_SHADERMODEL TexturedDrawVS();
        PixelShader = compile PS_SHADERMODEL TexturedDrawPS();
    }
};
technique BasicColorDraw
{
    pass Pass0
    {
        VertexShader = compile VS_SHADERMODEL BasicColorVS();
        PixelShader = compile PS_SHADERMODEL BasicColorPS();
    }
};
technique TrenchDraw
{
    pass Pass0
    {
        VertexShader = compile VS_SHADERMODEL BasicColorVS();
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
    float4 ScreenPos : TEXCOORD2;
    float3x3 WorldToTangentSpace : TEXCOORD3;
    float3 DirToCamera : TEXCOORD6; //unable to use 4 or 5?
};

struct PSOMRT
{
    float4 Color : COLOR0;
    float4 Normal : COLOR1;
    float4 Depth : COLOR2;
    float4 Bloom : COLOR3;
};


VSODraw BasicColorVS(in VSIDraw input)
{
    VSODraw output = (VSODraw) 0;

    float4 pos = mul(input.Position, WorldViewProjection);
    output.Position = pos;
    output.TextureCoordinates = input.TextureCoordinates;
    
    //output.Normal = World[3,3] * 0;
    //output.Normal += World[2, 3] * 0;
    //output.Normal += World[3, 2] * 0;
    float4 normal = float4(normalize(input.Normal), 1.0);
    //output.Normal = mul(normal, World).rgb;
    //output.Normal = float3(0,0,0);
    output.Normal = normal;
    output.ScreenPos = pos;
    
    return output;
}

float ApplyLightEffect;

PSOMRT BasicColorPS(VSODraw input)
{
    PSOMRT output = (PSOMRT) 0;
    
    output.Color = float4(Color, 1);
    output.Color.a = SpecularIntensity;
    
    //float3 worldNormal = mul(input.Normal, World);
    //float4 normal = float4(normalize(input.Normal.rgb), 0);
    
    //float3 worldNormal = normalize(mul(normal, World).rgb);
    if (ApplyLightEffect)
    {
        float3 worldNormal = mul(input.Normal, InverseTransposeWorld).xyz;
        output.Normal = float4(0.5 * (worldNormal + 1.0), 1);
        output.Normal.a = SpecularPower;
    }
    else
        output.Normal = float4(0, 0, 0, 0);
    
    output.Bloom = float4(Color * AddToFilter, 1);
    
    
    float depth = 1 - input.ScreenPos.z  / input.ScreenPos.w;
    
    output.Depth = float4(depth, depth, depth, 1);
    
    return output;
}

VSODraw TexturedDrawVS(in VSIDraw input)
{
    VSODraw output = (VSODraw) 0;

    float4 worldPos = mul(input.Position, World);
    float4 pos = mul(input.Position, WorldViewProjection);
    output.Position = pos;
    output.TextureCoordinates = input.TextureCoordinates;
    
    output.ScreenPos = pos;
    
    output.WorldToTangentSpace[0] = mul(normalize(input.Tangent), (float3x3) World);
    output.WorldToTangentSpace[1] = mul(normalize(input.Binormal), (float3x3) World);
    output.WorldToTangentSpace[2] = mul(normalize(input.Normal), (float3x3) World);
    
    output.DirToCamera = normalize(float4(CameraPosition, 1.0) - worldPos).xyz;
    //output.OmniAtt = ... ?
    return output;
}
PSOMRT TexturedDrawPS(VSODraw input)
{
    PSOMRT output = (PSOMRT) 0;
    
    output.Color = tex2D(textureSampler, input.TextureCoordinates);
    output.Color.a = SpecularIntensity;
    
    //sample normal from texture and convert 
    float3 fromNormalMap = (2.0 * tex2D(normalSampler, input.TextureCoordinates) - 1.0).xyz;
    float3 normal = normalize(mul(fromNormalMap, input.WorldToTangentSpace));
    output.Normal = float4(normal, SpecularPower);
 
    //float3 worldNormal = mul(normal, InverseTransposeWorld);
    //output.Normal = float4(0.5 * (worldNormal + 1.0), 1);
    //output.Normal.a = SpecularPower;
    
    //Add RT DirCam - OmniAtt?
    //output.CamDir = float4(input.DirToCamera, 1); // replace 1 for OmniAtt
    
    //output.Normal = float4(0, 0, 0, 1);
    output.Bloom = float4(Color * AddToFilter, 1);
    
    float depth = 1 - input.ScreenPos.z / input.ScreenPos.w;
    output.Depth = float4(depth, depth, depth, 1);
    
    return output;
}

PSOMRT TrenchPS(VSODraw input)
{
    PSOMRT output = (PSOMRT) 0;
    
    float3 worldNormal = mul(input.Normal, InverseTransposeWorld).xyz;
    
    output.Normal = float4(0.5 * (worldNormal + 1.0), 1);
    output.Normal.a = SpecularPower;

    float nDotLeft = dot(worldNormal, float3(-1.0, 0, 0.0));
    
    if (nDotLeft >= 0 && nDotLeft <= 0.10)
        output.Color = float4(Color * 0.8, 1);
    else
        output.Color = float4(Color, 1);
    
    output.Color.a = SpecularIntensity;
    
    output.Bloom = float4(Color * AddToFilter, 1);
    
    
    float depth = input.ScreenPos.z * 0.5 / input.ScreenPos.w;
    
    output.Depth = float4(depth, depth, depth, 1);
    
    return output;
}
/* SKYBOX */


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
    
    output.Color = float4(texCUBE(SkyBoxSampler, normalize(input.TextureCoordinates)).rgb, 1);
    output.Bloom = float4(0, 0, 0, 1);
    output.Normal = float4(0, 0, 0, 0);
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
texture DepthMap;
sampler depthSampler = sampler_state
{
    Texture = (DepthMap);
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
float3 OmniLightsPos[15];
float3 OmniLightsColor[15];
int OmniLightsCount;
float OmniLightsRadiusMin;
float OmniLightsRadiusMax;

float3 CalculateOmniLights(float3 vexPos, float3 normal)
{
    float3 color = float3(0,0,0);
    float3 lightVector;
    float NdL;
    float3 directedColor;
    
    //int i = 0;
    for (int i = 0; i < OmniLightsCount; i++)
    {
        lightVector = vexPos -  OmniLightsPos[i];

        NdL = max(0, dot(normal, normalize(lightVector)));
        directedColor = NdL * float3(1, 0, 1);
        
        float attenuation = 1 - smoothstep(OmniLightsRadiusMin, OmniLightsRadiusMax, length(lightVector));
        
        color += directedColor * attenuation;
    }
    
    return color;
}
float4 DLightPS(DLightVSO input) : COLOR0
{
    //get original pixel color
    float4 colorMap = tex2D(colorSampler, input.TexCoord);
    
    float3 texColor = colorMap.rgb;
        
    //get normal data from the normalMap
    float4 normalData = tex2D(normalMapSampler, input.TexCoord);
    
    if(normalData.w == 0)
        return float4(1,1,1, 0);
    //tranform normal back into [-1,1] range
    float3 normal = 2.0f * normalData.xyz - 1.0;
    //get specular power, and get it into [0,255] range]
    float specularPower = normalData.a * 255;
    //get specular intensity from the colorMap
    float specularIntensity = colorMap.a;
    
    //read depth
    float depthVal = 1 - tex2D(depthSampler, input.TexCoord).r;
        
    //compute screen-space position
    float4 position;
    position.x = input.TexCoord.x * 2.0 - 1.0;
    position.y = -(input.TexCoord.y * 2.0 - 1.0);
    position.z = depthVal;
    position.w = 1.0f;
    //transform to world space
    position = mul(position, InvertViewProjection);
    position /= position.w;
    
    //surface-to-light vector
    float3 lightVector = -normalize(LightDirection);

    //compute diffuse light (directional)
    float NdL = max(0, dot(normal, lightVector));
    float3 directionalLight = NdL * LightColor;
    
    float3 omniLights = CalculateOmniLights(position.xyz, normal);

    float3 diffuseLight = directionalLight + omniLights;
    
    //reflexion vector
    float3 reflectionVector = normalize(reflect(-lightVector, normal));
    //camera-to-surface vector
    float3 directionToCamera = normalize(CameraPosition - position.xyz);
    //compute specular light
    float specularLight = specularIntensity * pow(saturate(dot(reflectionVector, directionToCamera)), specularPower);

    
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