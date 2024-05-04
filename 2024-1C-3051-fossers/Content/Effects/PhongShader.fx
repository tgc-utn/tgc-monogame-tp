#if OPENGL
    #define SV_POSITION POSITION
    #define VS_SHADERMODEL vs_3_0
    #define PS_SHADERMODEL ps_3_0
#else
    #define VS_SHADERMODEL vs_5_0
    #define PS_SHADERMODEL ps_5_0
#endif



float4x4 World;
float4x4 View;
float4x4 Projection;

float3 LightSourcePositions[2]; 
float3 LightSourceColors[2];

float3 AmbientLight;
float AmbientCoefficient;
float DiffuseCoefficient;




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
    float4 Position : POSITION0;
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

    float3 DiffuseColor = float3(0,0,0);
   
    for (int i = 0; i < 2; i++){
        
        float3 lightPosition = LightSourcePositions[i].xyz;
        float3 lightColor = LightSourceColors[i].rgb;

        float3 L = normalize(lightPosition - input.WorldPos.xyz);
        float3 N = normalize(input.Normal);

        DiffuseColor += saturate(dot(L,N)) * lightColor * DiffuseCoefficient;
    }        


    float4 texelColor = tex2D(textureSampler,input.UV);
    float3 finalColor = saturate((AmbientLight * AmbientCoefficient + DiffuseColor)) * texelColor.rgb;

    return float4(finalColor,texelColor.a);
}

technique BasicColorDrawing
{
    pass P0
    {
        VertexShader = compile VS_SHADERMODEL MainVS();
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
};
