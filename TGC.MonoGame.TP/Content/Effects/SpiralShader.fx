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

float Time = 0.0;

float4x4 World;
float4x4 View;
float4x4 Projection;

struct VertexShaderInput
{
	float4 Position : POSITION0;
    float4 TextureCoordinate : TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
    float4 TextureCoordinate : TEXCOORD0;
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
	VertexShaderOutput output = (VertexShaderOutput)0;
    
    output.Position = input.Position;
    output.TextureCoordinate = input.TextureCoordinate;
	
    return output;
}

//Rota un punto respecto al origen
float2 rotate(float2 Point, float Rotation) 
{ 
    return float2(
        Point.x * cos(Rotation) - Point.y * sin(Rotation),
        Point.x * sin(Rotation) + Point.y * cos(Rotation)
    ); 
}

//Devuelve 1 si pertenece a la funcion, 0 si no pertenece
float spiralFunction(float2 Point) 
{
    float distanceToCenter = length(Point);
    float radius = atan(Point.y/Point.x);
    float thickness = 0.1;

    return step(distanceToCenter, radius+thickness) - step(distanceToCenter, radius-thickness);
}


float4 MainPS(VertexShaderOutput input) : COLOR
{
    float2 coordinates = input.TextureCoordinate * 2.0 - 1.0;
    float3 color = float3(input.TextureCoordinate.x, input.TextureCoordinate.y, 1.0);

    float pi = 3.14;

    float2 domain1 = rotate(coordinates, Time);
    float2 domain2 = rotate(coordinates, Time + pi/3.0);
    float2 domain3 = rotate(coordinates, Time + pi*2.0/3.0);

    float function1 = spiralFunction(domain1);
    float function2 = spiralFunction(domain2);
    float function3 = spiralFunction(domain3);

    float function = (function1 + function2 + function3);
    
    return float4(color * function, 1.0);
}

technique BasicColorDrawing
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};
