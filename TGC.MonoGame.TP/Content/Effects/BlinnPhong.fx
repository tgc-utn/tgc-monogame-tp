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

float3 ambientColor; // Light's Ambient Color
float3 diffuseColor; // Light's Diffuse Color
float3 specularColor; // Light's Specular Color
float KAmbient; 
float KDiffuse; 
float KSpecular;
float shininess; 
float3 lightPosition;
float3 eyePosition; // Camera position

texture baseTexture;
sampler2D textureSampler = sampler_state
{
    Texture = (baseTexture);
    MagFilter = Linear;
    MinFilter = Linear;
    AddressU = Clamp;
    AddressV = Clamp;
};

struct VertexShaderInput
{
	float4 Position : POSITION0;
    float4 Normal : NORMAL;
    float2 TextureCoordinates : TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
    float2 TextureCoordinates : TEXCOORD0;
    float4 WorldPosition : TEXCOORD1;
    float4 Normal : TEXCOORD2;    
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
	VertexShaderOutput output = (VertexShaderOutput)0;

    output.Position = mul(input.Position, WorldViewProjection);
    output.WorldPosition = mul(input.Position, World);
    output.Normal = mul(input.Normal, InverseTransposeWorld);
    output.TextureCoordinates = input.TextureCoordinates;
	
	return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
    // Base vectors
    float3 lightDirection = normalize(lightPosition - input.WorldPosition.xyz);
    float3 viewDirection = normalize(eyePosition - input.WorldPosition.xyz);
    float3 halfVector = normalize(lightDirection + viewDirection);

	// Get the texture texel
    float4 texelColor = tex2D(textureSampler, input.TextureCoordinates);
    // Calculate the diffuse light
    float NdotL = saturate(dot(input.Normal.xyz, lightDirection));
    float3 diffuseLight = KDiffuse * diffuseColor * NdotL;

	// Calculate the specular light
    float NdotH = dot(input.Normal.xyz, halfVector);
    float3 specularLight = sign(NdotL) * KSpecular * specularColor * pow(saturate(NdotH), shininess);
    
    // Final calculation
    float4 finalColor = float4(saturate(ambientColor * KAmbient + diffuseLight) * texelColor.rgb + specularLight, texelColor.a);
    return finalColor;

}

struct TrenchVertexShaderInput
{
    float4 Position : POSITION0;
    float3 Normal : NORMAL0;
};

struct TrenchVertexShaderOutput
{
    float4 Position : SV_POSITION;
    float4 WorldPosition : TEXCOORD0;
    float3 Normal : TEXCOORD1;
};

TrenchVertexShaderOutput TrenchVS(in TrenchVertexShaderInput input)
{
    TrenchVertexShaderOutput output;
    output.Position = mul(input.Position, WorldViewProjection);

    // Propagate the World Position with the Tiling applied (scaling them)
    output.WorldPosition = mul(input.Position, World);

    // Propagate the Normal to choose from which side the coordinates will be used
    output.Normal = input.Normal;

    return output;
}

float4 TrenchPS(TrenchVertexShaderOutput input) : COLOR
{
    //normal angle
    // Get how parallel the normal of this point is to the X plane
    float xAlignment = abs(dot(input.Normal, float3(1, 0, 0)));
    // Same for the Y plane
    float yAlignment = abs(dot(input.Normal, float3(0, 1, 0)));

    // Use the world position as texture coordinates 
    // Choose which coordinates we will use based on our normal
    float2 yPlane = lerp(input.WorldPosition.xy, input.WorldPosition.xz, yAlignment);
    float2 resultPlane = lerp(yPlane, input.WorldPosition.yz, xAlignment);

    float2 tiling = float2(10, 10);
    float2 textureCoordinates = resultPlane * tiling;

    //LIGHT
    // Base vectors
    float3 lightDirection = normalize(lightPosition - input.WorldPosition.xyz);
    float3 viewDirection = normalize(eyePosition - input.WorldPosition.xyz);
    float3 halfVector = normalize(lightDirection + viewDirection);

	// Get the texture texel
    float4 texelColor = tex2D(textureSampler, textureCoordinates);
    // Calculate the diffuse light
    float NdotL = saturate(dot(input.Normal.xyz, lightDirection));
    float3 diffuseLight = KDiffuse * diffuseColor * NdotL;

	// Calculate the specular light
    float NdotH = dot(input.Normal.xyz, halfVector);
    float3 specularLight = sign(NdotL) * KSpecular * specularColor * pow(saturate(NdotH), shininess);
    
    // Final calculation
    float4 finalColor = float4(saturate(ambientColor * KAmbient + diffuseLight) * texelColor.rgb + specularLight, texelColor.a);
    return finalColor;
    
}

technique BasicLight
{
	pass Pass0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};
technique 
{
    pass NormalAngleLight
    {
        VertexShader = compile VS_SHADERMODEL TrenchVS();
        PixelShader = compile PS_SHADERMODEL TrenchPS();
    }
};