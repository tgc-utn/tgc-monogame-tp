#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float4x4 WorldViewProjection;
float4x4 World;
float4x4 InverseTransposeWorld;
float4x4 LightViewProjection;

float3 ambientColor; // Light's Ambient Color
float3 diffuseColor; // Light's Diffuse Color
float3 specularColor; // Light's Specular Color
float KAmbient; 
float KDiffuse; 
float KSpecular;
float shininess; 
float3 lightPosition;
float3 eyePosition; // Camera position
float2 Tiling;

float2 shadowMapSize;

static const float modulatedEpsilon = 0.000041200182749889791011810302734375;
static const float maxEpsilon = 0.000023200045689009130001068115234375;


    

texture ModelTexture;
sampler2D textureSampler = sampler_state
{
    Texture = (ModelTexture);
    MagFilter = Linear;
    MinFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
    MIPFILTER = LINEAR;
};



//Textura para Normals
texture NormalTexture;
sampler2D normalSampler = sampler_state
{
    Texture = (NormalTexture);
    ADDRESSU = WRAP;
    ADDRESSV = WRAP;
    MINFILTER = LINEAR;
    MAGFILTER = LINEAR;
    MIPFILTER = LINEAR;
};


texture shadowMap;
sampler2D shadowMapSampler =
sampler_state
{
	Texture = <shadowMap>;
	MinFilter = Point;
	MagFilter = Point;
	MipFilter = Point;
	AddressU = Clamp;
	AddressV = Clamp;
};


struct DepthPassVertexShaderInput
{
	float4 Position : POSITION0;
};

struct DepthPassVertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 ScreenSpacePosition : TEXCOORD1;
};

struct ShadowedVertexShaderInput
{
	float4 Position : POSITION0;
	float3 Normal : NORMAL;
	float2 TextureCoordinates : TEXCOORD0;
};

struct ShadowedVertexShaderOutput
{
	float4 Position : SV_POSITION;
	float2 TextureCoordinates : TEXCOORD0;
	float4 WorldSpacePosition : TEXCOORD1;
	float4 LightSpacePosition : TEXCOORD2;
    float4 Normal : TEXCOORD3;
};


struct MainVertexShaderInput
{
	float4 Position : POSITION0;
    float4 Normal : NORMAL;
    float2 TextureCoordinates : TEXCOORD0;
};


struct MainVertexShaderOutput
{
	float4 Position : SV_POSITION;
	float2 TextureCoordinates : TEXCOORD0;
	float4 WorldSpacePosition : TEXCOORD1;
	float4 LightSpacePosition : TEXCOORD2;
    float4 Normal : TEXCOORD3;  
};


float3 getNormalFromMap(float2 textureCoordinates, float3 WorldSpacePosition, float3 worldNormal)
{
    float3 tangentNormal = tex2D(normalSampler, textureCoordinates).xyz * 2.0 - 1.0;

    float3 Q1 = ddx(WorldSpacePosition);
    float3 Q2 = ddy(WorldSpacePosition);
    float2 st1 = ddx(textureCoordinates);
    float2 st2 = ddy(textureCoordinates);

    worldNormal = normalize(worldNormal.xyz);
    float3 T = normalize(Q1 * st2.y - Q2 * st1.y);
    float3 B = -normalize(cross(worldNormal, T));
    float3x3 TBN = float3x3(T, B, worldNormal);

    return normalize(mul(tangentNormal, TBN));
}


// ========================================
// ============  SHADERS  =================
// ========================================


    // Depth Shaders

DepthPassVertexShaderOutput DepthVS(in DepthPassVertexShaderInput input)
{
	DepthPassVertexShaderOutput output;
	output.Position = mul(input.Position, WorldViewProjection);
	output.ScreenSpacePosition = mul(input.Position, WorldViewProjection);
	return output;
}

float4 DepthPS(in DepthPassVertexShaderOutput input) : COLOR
{
    float depth = input.ScreenSpacePosition.z / input.ScreenSpacePosition.w;
    return float4(depth, depth, depth, 1.0);
}
// _______________________________________


    // Main Shaders  --> Normal Mapping, Shadow Map

MainVertexShaderOutput MainVS(in MainVertexShaderInput input)
{
    
    MainVertexShaderOutput output = (MainVertexShaderOutput) 0;

    output.Position = mul(input.Position, WorldViewProjection);
    output.WorldSpacePosition = mul(input.Position, World);
    output.Normal = mul(input.Normal, InverseTransposeWorld);
	output.LightSpacePosition = mul(output.WorldSpacePosition, LightViewProjection);
    output.TextureCoordinates = input.TextureCoordinates * Tiling;
	
    return output;
    
    
    return output;
}


float4 MainPS(in MainVertexShaderOutput input) : COLOR
{

	

    // BlinnPhong 
    
        // Base vectors
        float3 lightDirection = normalize(lightPosition - input.WorldSpacePosition.xyz);
        float3 viewDirection = normalize(eyePosition - input.WorldSpacePosition.xyz);
        float3 halfVector = normalize(lightDirection + viewDirection);
        float3 normal =  getNormalFromMap(input.TextureCoordinates, input.WorldSpacePosition.xyz, normalize(input.Normal.xyz));      

        // Get the texture texel
        float4 texelColor = tex2D(textureSampler, input.TextureCoordinates);  
		

        // Calculate the diffuse light
        float NdotL = saturate(dot(normal, lightDirection));
        float3 diffuseLight = KDiffuse * diffuseColor * NdotL;

        // Calculate the specular light
        float NdotH = dot(normal, halfVector);
        float3 specularLight = KSpecular * specularColor * pow(NdotH, shininess);
        
    
    // Shadow Map 
        float3 lightSpacePosition = input.LightSpacePosition.xyz / input.LightSpacePosition.w;
        float2 shadowMapTextureCoordinates = 0.5 * lightSpacePosition.xy + float2(0.5, 0.5);
        shadowMapTextureCoordinates.y = 1.0f - shadowMapTextureCoordinates.y;
        

        float inclinationBias = max(modulatedEpsilon * (1.0 - dot(normal, lightDirection)), maxEpsilon);
        
        // Sample and smooth the shadowmap
        // Also perform the comparison inside the loop and average the result
        float notInShadow = 0.0;
        float2 texelSize = 1.0 / shadowMapSize;
        for (int x = -1; x <= 1; x++)
            for (int y = -1; y <= 1; y++)
            {
                float pcfDepth = tex2D(shadowMapSampler, shadowMapTextureCoordinates + float2(x, y) * texelSize).r + inclinationBias;
                notInShadow += step(lightSpacePosition.z, pcfDepth) / 9.0;
            }
	
	
	
    // Result 
    
    float4 finalColor = float4(saturate(ambientColor * KAmbient + diffuseLight) * texelColor.rgb + specularLight, texelColor.a) ;
    
    finalColor.rgb *= 0.5 + 0.5 * notInShadow;
             
	return finalColor ;
	

}


// _______________________________________


    // ShadowedPCFPS Shaders  


float4 ShadowedPCFPS(in ShadowedVertexShaderOutput input) : COLOR
{
    float3 lightSpacePosition = input.LightSpacePosition.xyz / input.LightSpacePosition.w;
    float2 shadowMapTextureCoordinates = 0.5 * lightSpacePosition.xy + float2(0.5, 0.5);
    shadowMapTextureCoordinates.y = 1.0f - shadowMapTextureCoordinates.y;
	
    float3 normal = normalize(input.Normal.rgb);
    float3 lightDirection = normalize(lightPosition - input.WorldSpacePosition.xyz);
    float inclinationBias = max(modulatedEpsilon * (1.0 - dot(normal, lightDirection)), maxEpsilon);
	
	// Sample and smooth the shadowmap
	// Also perform the comparison inside the loop and average the result
    float notInShadow = 0.0;
    float2 texelSize = 1.0 / shadowMapSize;
    for (int x = -1; x <= 1; x++)
        for (int y = -1; y <= 1; y++)
        {
            float pcfDepth = tex2D(shadowMapSampler, shadowMapTextureCoordinates + float2(x, y) * texelSize).r + inclinationBias;
            notInShadow += step(lightSpacePosition.z, pcfDepth) / 9.0;
        }
	
    float4 baseColor = tex2D(textureSampler, input.TextureCoordinates);
    baseColor.rgb *= 0.5 + 0.5 * notInShadow;
    return baseColor;
}



// _______________________________________


    // NormalMap Shaders  

MainVertexShaderOutput NormalMapVS(in MainVertexShaderInput input)
{
    MainVertexShaderOutput output = (MainVertexShaderOutput) 0;

    output.Position = mul(input.Position, WorldViewProjection);
    output.WorldSpacePosition = mul(input.Position, World);
    output.Normal = mul(input.Normal, InverseTransposeWorld);
    output.TextureCoordinates = input.TextureCoordinates * Tiling;
	
    return output;
}


float4 NormalMapPS(MainVertexShaderOutput input) : COLOR
{
    // Base vectors
    float3 lightDirection = normalize(lightPosition - input.WorldSpacePosition.xyz);
    float3 viewDirection = normalize(eyePosition - input.WorldSpacePosition.xyz);
    float3 halfVector = normalize(lightDirection + viewDirection);
    float3 normal =  getNormalFromMap(input.TextureCoordinates, input.WorldSpacePosition.xyz, normalize(input.Normal.xyz));

	// Get the texture texel
    float4 texelColor = tex2D(textureSampler, input.TextureCoordinates);
    
	// Calculate the diffuse light
    float NdotL = saturate(dot(normal, lightDirection));
    float3 diffuseLight = KDiffuse * diffuseColor * NdotL;

	// Calculate the specular light
    float NdotH = dot(normal, halfVector);
    float3 specularLight = KSpecular * specularColor * pow(NdotH, shininess);
    
    // Final calculation
    float4 finalColor = float4(saturate(ambientColor * KAmbient + diffuseLight) * texelColor.rgb + specularLight, texelColor.a);
    return finalColor;

}



// ========================================
// ===========  TECHNIQUES  ===============
// ========================================

technique DepthPass
{
	pass Pass0
	{
		VertexShader = compile VS_SHADERMODEL DepthVS();
		PixelShader = compile PS_SHADERMODEL DepthPS();
	}
};


technique Draw_NM_SM
{
	pass Pass0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};



technique DrawShadowedPCF
{
    pass Pass0
    {
        VertexShader = compile VS_SHADERMODEL MainVS();
        PixelShader = compile PS_SHADERMODEL ShadowedPCFPS();
    }
};



technique NormalMapping
{
    pass Pass0
    {
        VertexShader = compile VS_SHADERMODEL NormalMapVS();
        PixelShader = compile PS_SHADERMODEL NormalMapPS();
    }
};