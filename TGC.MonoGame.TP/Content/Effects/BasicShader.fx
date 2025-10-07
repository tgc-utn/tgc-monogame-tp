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
//
float4x4 World;
float4x4 View;
float4x4 Projection;

float3 DiffuseColor;

float Time = 0;

struct VertexShaderInput
{
	float4 LocalPosition : POSITION0;
	float4 Normal : NORMAL0;
	float2 TextureCoordinates : TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 ProjectionPosition : SV_POSITION;
	float3 WorldPosition : TEXCOORD0;
	float4 Normal : TEXCOORD1;
	float2 TextureCoordinates : TEXCOORD2;
};

uniform texture ModelTexture;

sampler2D TextureSampler = sampler_state
{
	Texture = (ModelTexture);
	MagFilter = Linear;
	MinFilter = Linear;
	AddressU = Wrap;
	AddressV = Wrap;
};


//
VertexShaderOutput MainVS(in VertexShaderInput input)
{
    // Clear the output
	VertexShaderOutput output = (VertexShaderOutput)0;
	
    // Model space to World space
    float4 worldPosition = mul(input.LocalPosition, World);
    // World space to View space
    float4 viewPosition = mul(worldPosition, View);
	// View space to Projection space
    output.ProjectionPosition = mul(viewPosition, Projection);

	// Propagar
	output.WorldPosition = worldPosition;
	output.Normal = input.Normal;
	output.TextureCoordinates = input.TextureCoordinates; 

    return output;
}
//
float4 MainPS(VertexShaderOutput input) : COLOR
{
	// Aplicando una textura
	float2 textureCoordinates = input.TextureCoordinates.xy;
	float4 textureColor = tex2D(TextureSampler, textureCoordinates);
	// if (textureColor.g < frac(Time * 0.25))
	if (textureColor.g < frac(Time * 0.25))
		discard; // <- Este comando es para descartar
	return textureColor;
}

technique BasicColorDrawing
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};
