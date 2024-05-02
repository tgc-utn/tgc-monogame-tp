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

float3 AmbientLight;
float3 MaterialProperties; // Assume x = ambient coefficient, y = diffuse coefficient, z = specular coefficient

float4x4 LightSources[8]; // Each column: Color, Position, Direction, Cone Angle, Intensity

Texture2D Texture;

sampler2D textureSampler = sampler_state
{
    Texture = <Texture>;
    MinFilter = Linear;
    MagFilter = Linear;
    MipFilter = Linear;
    AddressU = Clamp;
    AddressV = Clamp;
};

struct VertexShaderInput
{
	float4 Position : POSITION;
	float3 Normal : NORMAL;
    float2 UV : TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
    float3 Normal : TEXCOORD0;
    float3 WorldPos : TEXCOORD1;
    float2 UV : TEXCOORD0;
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
	VertexShaderOutput output = (VertexShaderOutput)0;
    float4 worldPosition = mul(input.Position, World);
    output.WorldPos = worldPosition.xyz;
    output.Position = mul(worldPosition, View);
    output.Position = mul(output.Position, Projection);

    // Transform normals to world space
    output.Normal = mul(input.Normal, (float3x3)World);
    output.UV = input.UV;

    return output;
}

float4 MainPS(VertexShaderOutput input) : SV_Target
{
    float3 N = normalize(input.Normal);
    float3 V = normalize(-input.WorldPos); // View vector
    float4 finalColor = float4(0, 0, 0, 0);

    // Ambient lighting
    finalColor.rgb += AmbientLight * MaterialProperties.x;

    // Loop through each light source
    for (int i = 0; i < 8; i++)
    {
        float3 lightColor = LightSources[i][0].rgb;
        float3 lightPos = LightSources[i][1].xyz;
        float3 lightDir = normalize(LightSources[i][2].xyz);
        float coneAngle = LightSources[i][2].w;

        float3 L = normalize(lightPos - input.WorldPos);
        float cosTheta = dot(L, -lightDir);

        if (coneAngle >= acos(cosTheta))
        {
            float diff = max(dot(N, L), 0.0);
            finalColor.rgb += lightColor * diff * MaterialProperties.y;

            float3 H = normalize(L + V);
            float spec = pow(max(dot(N, H), 0.0), MaterialProperties.z);
            finalColor.rgb += lightColor * spec;
        }

    }

    finalColor *= tex2D(textureSampler, input.UV);

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

