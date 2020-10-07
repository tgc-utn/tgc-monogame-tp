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
    
    float4 worldPosition = mul(input.Position, World);
    float4 zero = mul(float4(0,0,0,1), World);
        
    float waveFrequency = 2;
    float waveAmplitude = 25;
     float waveAmplitude2 = 25;
    //worldPosition.y += zero.y;
	worldPosition.y = zero.y + (sin(worldPosition.x*waveFrequency+ shaderTime) + sin(worldPosition.z*waveFrequency + shaderTime))*waveAmplitude + sin(worldPosition.x + worldPosition.z + shaderTime)*waveAmplitude2;
    
    float3 tangent1 = normalize(float3(1, 
    (cos(input.Position.x*waveFrequency + shaderTime)*waveFrequency*waveAmplitude + cos(worldPosition.x + worldPosition.z + shaderTime)*waveAmplitude2)
    ,0));
    float3 tangent2 = normalize(float3(0, 
    (cos(input.Position.z*waveFrequency + shaderTime)*waveFrequency*waveAmplitude + cos(worldPosition.x + worldPosition.z + shaderTime)*waveAmplitude2)
    ,1));
    
	input.Normal.xyz = normalize(cross(tangent1, tangent2));

//	float4 worldPosition = mul(input.Position, World);
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
    float3 lightDirection = normalize(float3(1000,700, 0) + input.WorldPosition.xyz - input.WorldPosition.xyz);
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