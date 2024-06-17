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

float3 DiffuseColor;

bool onhit;

float3 ImpactPosition;
float3 TankPosition;

float impacto; //tamaño del impacto
float velocidad; //profundidad del impacto

texture ModelTexture;
sampler2D TextureSampler = sampler_state
{
    Texture = (ModelTexture);
    MagFilter = Linear;
    MinFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
};

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

float3 VersorDireccion(float3 A, float3 B)
{
    float3 Vector = B - A;
    float moduloVector = length(Vector);

    return Vector / moduloVector;
}

float3 desplazarPorRadio(float3 Posicion, float radio, float3 centro)
{
    float3 direccion = VersorDireccion(centro, Posicion);
    float distancia = radio - distance(centro, Posicion);
    return Posicion + (direccion * distancia);
}

VertexShaderOutput MainVS(in VertexShaderInput input)
{
    // Clear the output
    VertexShaderOutput output = (VertexShaderOutput) 0;
    // Model space to World space
    float4 worldPosition = mul(input.Position, World);
    // World space to View space
    float4 viewPosition = mul(worldPosition, View);
    // View space to Projection space
    output.Position = mul(viewPosition, Projection);

    output.TextureCoordinate = input.TextureCoordinate;

    if (onhit)
    {
        float3 direccion = VersorDireccion(ImpactPosition, TankPosition);
        float3 c_Esfera = ImpactPosition + (direccion * velocidad);
        float r_Esfera = impacto;
        if (distance(c_Esfera, output.Position.xyz) <= r_Esfera)
        {
            output.Position.xyz = desplazarPorRadio(output.Position.xyz, r_Esfera, c_Esfera);
        }
    }

    return output;


}

float4 MainPS(VertexShaderOutput input) : COLOR
{
    
    float4 color = tex2D(TextureSampler, input.TextureCoordinate.xy);
    return color;

}

technique BasicColorDrawing
{
    pass P0
    {
        VertexShader = compile VS_SHADERMODEL MainVS();
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
};