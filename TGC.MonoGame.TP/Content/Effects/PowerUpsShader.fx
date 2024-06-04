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
    float4 Color : COLOR0;
    float2 TextureCoordinate : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
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

float Time;

VertexShaderOutput MainVS(in VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput) 0;

// Animate position
    float atenuacion = 2.8;
    float rotationSpeed = 30.0; // Velocidad de rotaci�n en grados por segundo
    float verticalSpeed = 1.0; // Velocidad de movimiento vertical
    float3 position = input.Position;

// Calcula la rotaci�n en el eje y
    float rotationAngle = Time * rotationSpeed * 0.0174533; // Convertir grados a radianes
    float rotationX = sin(rotationAngle);
    float rotationZ = cos(rotationAngle);

// Aplica la rotaci�n alrededor del eje y
    float rotatedX = position.x * rotationZ - position.z * rotationX;
    float rotatedZ = position.x * rotationX + position.z * rotationZ;

// Calcula el desplazamiento vertical
    float displacementY = sin(Time * atenuacion) * verticalSpeed + 1;

// Aplica los desplazamientos a las coordenadas
    input.Position.x = rotatedX;
    input.Position.y = position.y + displacementY;
    input.Position.z = rotatedZ;

    float4 worldPosition = mul(input.Position, World);
    
    
    float4 viewPosition = mul(worldPosition, View);
	
	// Project position
    output.Position = mul(viewPosition, Projection);

	// Propagate texture coordinates
    output.TextureCoordinate = input.TextureCoordinate;

	// Animate color
    input.Color.r = abs(sin(Time * atenuacion));
    input.Color.g = abs(cos(Time * atenuacion));

	// Propagate color by vertex
    output.Color = input.Color;

    return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
	// Get the texture texel textureSampler is the sampler, Texcoord is the interpolated coordinates
    float4 textureColor = tex2D(textureSampler, input.TextureCoordinate);
    textureColor.a = 1;
	// Color and texture are combined in this example, 80% the color of the texture and 20% that of the vertex
    return 0.8 * textureColor + 0.2 * input.Color;
}

technique BasicColorDrawing
{
    pass P0
    {
        VertexShader = compile VS_SHADERMODEL MainVS();
        PixelShader = compile PS_SHADERMODEL  MainPS();
    }
};
