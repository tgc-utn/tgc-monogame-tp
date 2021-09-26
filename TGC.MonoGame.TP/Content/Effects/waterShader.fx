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
float Time = 0;
//float3 DiffuseColor;



struct VertexShaderInput
{
	float4 Position : POSITION0;
	float2 TextureCoordinate : TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float2 TextureCoordinate : TEXCOORD0;
	float4 WorldPosition : TEXCOORD1;
};

texture2D TextureWater;
sampler2D textureSampler = sampler_state
{
	Texture = (TextureWater);
	MagFilter = Linear;
	MinFilter = Linear;
	AddressU = Mirror;
	AddressV = Mirror;
};


float3 crearWave(float4 position, float2 directionWave)
{
	float multiplicadorPosicion = 0.02 * 3.14;
	float multiplicadorTiempo = 0.07 * 3.14;
	float3 wave = float3(0, 0, 0);
	float amplitud = 20;
	float largo = 100;
	wave.x = amplitud * 4 * directionWave.x * cos(dot(position.xz, directionWave) * multiplicadorPosicion + Time * multiplicadorTiempo);
	wave.y = amplitud * pow(((sin(dot(position.xz, directionWave) * multiplicadorPosicion + Time * multiplicadorTiempo) + 1) / 2), 5);
	wave.z = amplitud * directionWave.y * cos(dot(position.xz, directionWave)) + Time * multiplicadorTiempo;
	return wave;
	
}
VertexShaderOutput MainVS(in VertexShaderInput input)
{
    // Clear the output
	VertexShaderOutput output = (VertexShaderOutput)0;
    // Model space to World space
	//input.Position.x *= sin(Time);
	//Precommit
	float4 worldPosition = mul(input.Position, World);
	float3 wave1 = crearWave(worldPosition, float2(0.3, 0.5));
	float3 wave2 = crearWave(worldPosition, float2(0.8, -0.4));
	float3 wave3 = crearWave(worldPosition, float2(-0.5, 0.3));
	float3 wave4 = crearWave(worldPosition, float2(-0.3, 0.5));
	float3 wave5 = crearWave(worldPosition, float2(0, -0.2));
	float3 wave6 = crearWave(worldPosition, float2(0.5, 0.5));
	float3 wave7 = crearWave(worldPosition, float2(-0.8, 0.6));
	
	worldPosition.xyz += (wave1 + wave2 + wave3 + wave4 + wave5 + wave6 + wave7) / 7;
	//output.WorldPosition = worldPosition;
	//
    //float4 worldPosition = mul(input.Position, World);
    // World space to View space
    float4 viewPosition = mul(worldPosition, View);	
	// View space to Projection space
    output.Position = mul(viewPosition, Projection);
	//output.TextureCoordinate = input.TextureCoordinate;
	output.TextureCoordinate.x += input.TextureCoordinate.x + abs(Time*0.005f);
	output.TextureCoordinate.y += input.TextureCoordinate.y + abs(Time*0.005f);
    return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
	float4 textureColor = tex2D(textureSampler, input.TextureCoordinate);
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
