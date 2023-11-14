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

struct VertexShaderInput
{
	float4 Position : POSITION0;
    float2 TextureCoordinate : TEXCOORD1;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
    float4 MyPosition : TEXCOORD1;
    float2 TextureCoordinate : TEXCOORD0;
};

texture ModelTexture;
sampler2D textureSampler = sampler_state
{
    Texture = (ModelTexture);
    MagFilter = Linear;
    MinFilter = Linear;
    AddressU = Clamp;
    AddressV = Clamp;
};

float Time = 0;

VertexShaderOutput MainVS(in VertexShaderInput input)
{
	VertexShaderOutput output = (VertexShaderOutput)0;

	// Animate position
    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
	
	// Project position
    output.Position = mul(viewPosition, Projection);
    output.MyPosition = mul(viewPosition, Projection);
	// Propagate texture coordinates
    output.TextureCoordinate = input.TextureCoordinate;

    return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
	// Get the texture texel textureSampler is the sampler, Texcoord is the interpolated coordinates
    float4 textureColor = tex2D(textureSampler, input.TextureCoordinate);
    
    // Calcular la posición de la esfera alrededor del objeto
    float4 worldPosition = mul(input.MyPosition, World);
    worldPosition = mul(worldPosition, View);
    worldPosition = mul(worldPosition, Projection);
    
    // Calcular la distancia desde el centro de la esfera
    float sphereDistance = distance(worldPosition.xy, input.MyPosition.xy);
    
    // Configurar los parámetros de la esfera translúcida
    float sphereRadius = 0.5; // Ajusta el radio de la esfera según tus necesidades
    float sphereTransparency = 0.5; // Ajusta la transparencia de la esfera
    
    // Calcular la contribución de la esfera a la mezcla de color
    float4 sphereContribution = float4(1, 1, 1, 1) * sphereTransparency; // Color blanco translúcido
    
    // Mezclar el color de la textura con la contribución de la esfera basada en la distancia
    float4 finalColor = lerp(textureColor, sphereContribution, smoothstep(sphereRadius, sphereRadius + 0.1, sphereDistance));
    
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
