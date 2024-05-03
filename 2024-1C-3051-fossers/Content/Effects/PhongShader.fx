#if OPENGL
    #define SV_POSITION POSITION
    #define VS_SHADERMODEL vs_3_0
    #define PS_SHADERMODEL ps_3_0
#else
    #define VS_SHADERMODEL vs_5_0
    #define PS_SHADERMODEL ps_5_0
#endif


float4 AmbientLight;
float3 MaterialProperties; // Assume x = ambient coefficient, y = diffuse coefficient, z = specular coefficient
float4x4 LightSources[8]; // Each column: Color, Position, Direction, Cone Angle
int LightSourceCount; // Changed from float to int6

float4x4 World;
float4x4 View;
float4x4 Projection;
Texture2D Texture;
sampler2D textureSampler = sampler_state
{
    Texture = (Texture);
    MinFilter = Linear;
    MagFilter = Linear;
    MipFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
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
    float2 UV : TEXCOORD0;
    float3 Normal : TEXCOORD1;
    float3 WorldPos : TEXCOORD2;
  
};


VertexShaderOutput MainVS(in VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput)0;
    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);	
    output.WorldPos = worldPosition.xyz;
    output.Position = mul(viewPosition, Projection);
    output.UV = input.UV;
    output.Normal = input.Normal;
    return output;
}

float4 MainPS(VertexShaderOutput input) : SV_Target
{
    float3 N = normalize(input.Normal);
    float3 V = normalize(-input.WorldPos); // View vector

    // Ambient lighting
    float4 finalColor = tex2D(textureSampler, input.UV);
    finalColor += AmbientLight * MaterialProperties.x;
    // Loop through each light source
    for (int i = 0; i < LightSourceCount - 1; i++)
    {
        
        float4 lightColor = LightSources[i][0].xyzw;
        float3 lightPos = LightSources[i][1].xyz;
        float3 lightDir = normalize(LightSources[i][2].xyz);
        float coneAngle = LightSources[i][3].x;

        float3 L = normalize(lightPos - input.WorldPos);
        float cosTheta = dot(L, -lightDir);

        if (acos(cosTheta)<coneAngle)
        {
            float diff = max(dot(N, L), 0.0);
            finalColor += lightColor * diff * MaterialProperties.y;

            float3 H = normalize(L + V);
            float spec = pow(max(dot(N, H), 0.0), MaterialProperties.z);
            finalColor += lightColor * spec;
        }
    }


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
