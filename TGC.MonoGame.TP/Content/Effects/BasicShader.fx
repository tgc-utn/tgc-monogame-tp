#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

// Custom Effects - https://docs.monogame.net/articles/content/custom_effects.html
// High-level shader language (HLSL) - https://docs.microsoft.com/en-us/windows/win32/direct3dhlsl/dx-graphics-hlsl
// Programming guide for HLSL - https://docs.microsoft.com/en-us/windows/win32/direct3dhlsl/dx-graphics-hlsl-pguide
// Reference for HLSL - https://docs.microsoft.com/en-us/windows/win32/direct3dhlsl/dx-graphics-hlsl-reference
// HLSL Semantics - https://docs.microsoft.com/en-us/windows/win32/direct3dhlsl/dx-graphics-hlsl-semantics

float4x4 World;
float4x4 View;
float4x4 Projection;
float4x4 InverseTransposeWorld;

float3 DiffuseColor;

float3 ambientColor; // Light's Ambient Color
float3 diffuseColor; // Light's Diffuse Color
float3 specularColor; // Light's Specular Color
float KAmbient;
float KDiffuse;
float KSpecular;
float shininess;
float3 lightPosition;
float3 eyePosition; // Camera position

float Time = 0;

texture baseTexture;
sampler2D textureSampler = sampler_state
{
    Texture = (baseTexture);
    MagFilter = Linear;
    MinFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
};

struct VertexShaderInput
{
	float4 Position : POSITION0;
    float4 Normal : NORMAL;
    float2 TextureCoordinates : TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 Position : POSITION;
    float2 TextureCoordinates : TEXCOORD0;
    float4 WorldPosition : TEXCOORD1;
    float4 Normal : TEXCOORD2;
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
    // Clear the output
	VertexShaderOutput output = (VertexShaderOutput)0;
    // Model space to World space
    float4 worldPosition = mul(input.Position, World);
    // World space to View space
    float4 viewPosition = mul(worldPosition, View);	
	// View space to Projection space
    output.Position = mul(viewPosition, Projection);
    
    output.Normal = mul(input.Normal, InverseTransposeWorld);

	output.WorldPosition = worldPosition;
    output.TextureCoordinates = input.TextureCoordinates;

    return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
    // Base vectors
    float3 lightDirection = normalize(lightPosition - input.WorldPosition.xyz);
    float3 viewDirection = normalize(eyePosition - input.WorldPosition.xyz);
    float3 halfVector = normalize(lightDirection + viewDirection);

    //Ambient Light
    float3 ambientLight = KAmbient * ambientColor;

    // Get the texture texel
    float4 texelColor = tex2D(textureSampler, input.TextureCoordinates);

    // Diffuse Light
    float distanceAttenuation = length(lightPosition - input.WorldPosition.xyz);
    float singleDistanceAttenuation = distanceAttenuation;
    distanceAttenuation = distanceAttenuation * distanceAttenuation;
    
	// Calculate the diffuse light
    //Intensity of the diffuse light. Saturate to keep within the 0-1 range.
    lightDirection = normalize(distanceAttenuation);
    float3 normal = normalize(input.Normal.xyz);
    float NdotL = dot(normal, lightDirection);
    //float intensity = saturate(NdotL);
    float3 diffuseLight = KDiffuse * diffuseColor * max(0.0, NdotL) * 10 ;

    // Calculate the specular light
    float NdotH = dot(normal, halfVector);
    float3 specularLight =  KSpecular * specularColor * pow(saturate(NdotH), shininess);

    float linearLight = lerp(0, 250, 1 / singleDistanceAttenuation);
    float3 finalLinearDiffuseLight = diffuseColor * KDiffuse * (linearLight, linearLight, linearLight);

    return float4(saturate(ambientLight.rbg + finalLinearDiffuseLight) * (texelColor.r * 0.7 + texelColor.g * 0.15 + texelColor.b * 0.15) + specularLight, 1.0);
}

technique BasicColorDrawing
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};
