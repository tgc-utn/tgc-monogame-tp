#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float4x4 World;
float4x4 View;
float4x4 Projection;
float4x4 InverseTransposeWorld;
float3 CameraPosition;

float3 AmbientColor; 
float KAmbient;

float3 DiffuseColor;
float KDiffuse;

float3 SpecularColor; 
float KSpecular;
float Shininess;

 
float Time = 0;

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float4 Normal : NORMAL;
	float4 Color : COLOR0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float4 WorldPosition : TEXCOORD1;
    float4 Normal : TEXCOORD2;    
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
	VertexShaderOutput output = (VertexShaderOutput)0;
    float shaderTime = Time;
	input.Position.y = (sin(input.Position.x + shaderTime) + sin(input.Position.z + shaderTime))*15 + sin(input.Position.x + input.Position.z + shaderTime)*15 + 50;
    
    float3 tangent1 = normalize(float3(1, 
    (cos(input.Position.x+shaderTime)*15+   cos(input.Position.x + input.Position.z + shaderTime)*15)
    ,0));
    float3 tangent2 = normalize(float3(0, 
    (cos(input.Position.z+shaderTime)*15+   cos(input.Position.x + input.Position.z + shaderTime)*15)
    ,1));

    
	input.Normal.xyz = normalize(cross(tangent1, tangent2));

	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);

    output.Normal = mul(input.Normal, InverseTransposeWorld);
	output.Position = mul(viewPosition, Projection);
	output.Color = input.Color;
	output.WorldPosition = worldPosition;

	return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
    //Todo: Set light position from game script
    float3 lightDirection = normalize(float3(1000,700, 0) - input.WorldPosition.xyz);
    float3 viewDirection = normalize(CameraPosition - input.WorldPosition.xyz);
    float3 halfVector = normalize(lightDirection + viewDirection);
    
    float3 ambientLight = KAmbient * AmbientColor;
    
    float NdotL = saturate(dot(input.Normal.xyz, lightDirection));
    float3 diffuseLight = KDiffuse * DiffuseColor * NdotL;

    float NdotH = dot(input.Normal.xyz, halfVector);
    float3 specularLight = sign(NdotL) * KSpecular * SpecularColor * pow(saturate(NdotH), Shininess);
    
   // float4 finalColor = float4(diffuseLight + ambientLight, 1);
    float4 finalColor = float4(saturate(ambientLight + diffuseLight) + specularLight, 0);

	return finalColor;
}

technique BasicColorDrawing
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};