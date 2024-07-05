#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float4x4 World;         // Matriz de mundo
float4x4 View;          // Matriz de vista
float4x4 Projection;    // Matriz de proyección

float3 ambientColor;    // Color ambiental
float3 diffuseColor;    // Color difuso
float3 specularColor;   // Color especular
float KAmbient;         // Factor de ambiente
float KDiffuse;         // Factor difuso
float KSpecular;        // Factor especular
float shininess;        // Brillo especular

bool EnableTerrainDraw = false;
bool EnableGrass = false;
bool EnableTrees = false;
float onhit;

float3 lightPosition;
float3 eyePosition; 

float impacto;
float3 ImpactPosition;
float3 TankPosition;
float3 c_Esfera; 

float4 Plano_ST;

float TrackOffset;
bool IsTrack;

float alphaValue = 1;
float time = 0;

texture texDiffuseMap;
sampler2D diffuseMap = sampler_state
{
    Texture = (texDiffuseMap);
    ADDRESSU = WRAP;
    ADDRESSV = WRAP;
    MINFILTER = LINEAR;
    MAGFILTER = LINEAR;
    MIPFILTER = LINEAR;
};

texture texDiffuseMap2;
sampler2D diffuseMap2 = sampler_state
{
    Texture = (texDiffuseMap2);
    ADDRESSU = MIRROR;
    ADDRESSV = MIRROR;
    MINFILTER = LINEAR;
    MAGFILTER = LINEAR;
    MIPFILTER = LINEAR;
};

texture texColorMap;
sampler2D colorMap = sampler_state
{
    Texture = (texColorMap);
    ADDRESSU = WRAP;
    ADDRESSV = WRAP;
    MINFILTER = LINEAR;
    MAGFILTER = LINEAR;
    MIPFILTER = LINEAR;
};

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
    float4 Normal : NORMAL;
    float2 TextureCoordinate : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float2 TextureCoordinate : TEXCOORD0;
    float3 WorldPosition : TEXCOORD1;
    float3 Normal : TEXCOORD2;
};

struct VS_INPUT
{
    float4 Position : POSITION0;
    float2 Texcoord : TEXCOORD0;
    float3 Normal : NORMAL0;
};

struct VS_OUTPUT
{
    float4 Position : POSITION0;
    float2 Texcoord : TEXCOORD0;
    float3 WorldPos : TEXCOORD1;
    float3 WorldNormal : TEXCOORD2;
};

struct PS_INPUT
{
    float2 Texcoord : TEXCOORD0;
    float3 WorldPos : TEXCOORD1;
    float3 WorldNormal : TEXCOORD2;
};

VS_OUTPUT vs_RenderTerrain(VS_INPUT input)
{
    VS_OUTPUT output;

    //Proyectar posicion
    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);

    //Enviar Texcoord directamente
    output.Texcoord = input.Texcoord;

    //todo: que le pase el inv trasp. word
    float4x4 matInverseTransposeWorld = World;
    output.WorldPos = worldPosition.xyz;
    output.WorldNormal = mul(input.Normal, matInverseTransposeWorld).xyz;

    return output;
}

float4 ps_RenderTerrain(VS_OUTPUT input) : COLOR0
{
    if(EnableTerrainDraw == false)
        return float4(0, 0, 0, 0);
    
    float3 N = normalize(input.WorldNormal);
    float3 L = normalize(lightPosition - input.WorldPos);
    float kd = saturate(0.4 + 0.7 * saturate(dot(N, L)));

    float3 c = tex2D(colorMap, input.Texcoord).rgb;
    float3 tex1 = tex2D(diffuseMap, input.Texcoord * 31).rgb;
    float3 tex2 = tex2D(diffuseMap2, input.Texcoord * 27).rgb;
    float3 clr = lerp(lerp(tex1, tex2, c.r), c, -1.1);
    
    return float4(clr * kd, 1);
}

VertexShaderOutput ImpactVS(in VertexShaderInput input)
{
    VertexShaderOutput output;

    // Transformaciones de espacio
    float4 worldPosition = mul(input.Position, World);
    
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);

    // Pasar datos para el pixel shader
    output.WorldPosition = worldPosition.xyz;
    output.Normal = normalize(mul(input.Normal.xyz, (float3x3) World));

    // Coordenadas de textura
    output.TextureCoordinate = input.TextureCoordinate;

    return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
    if (IsTrack)
    {
        input.TextureCoordinate.y += TrackOffset;
    }
    
    float3 light = lightPosition;
      
    // Muestrear la textura
    float4 texelColor = tex2D(TextureSampler, input.TextureCoordinate.xy);
    // Si es un arbol invierte la direccion de la luz devido a que los arboles se dibujan alrevez
    if (EnableTrees)
        light = float3(lightPosition.x * -1, lightPosition.y, lightPosition.z * -1);
    // Dirección de la luz (asumiendo luz solar, puedes ajustar según necesites)
    float3 lightDirection = normalize(light - input.WorldPosition.xyz);
    // Vector de vista
    float3 viewDirection = normalize(eyePosition - input.WorldPosition.xyz);
    // Vector semibrillante
    float3 halfVector = normalize(lightDirection + viewDirection);
    // Cálculo del componente difuso
    float NdotL = saturate(dot(input.Normal, lightDirection));
    float3 diffuse = KDiffuse * diffuseColor * NdotL;
    // Cálculo del componente especular
    float NdotH = saturate(dot(input.Normal, halfVector));
    float3 specular = KSpecular * specularColor * pow(NdotH, shininess);
    // Cálculo del componente ambiental
    float3 ambient = KAmbient * ambientColor;
    // Color final
    float3 finalColor = ambient + diffuse + specular;
    
    float4 finalOutput;
    
    if(!EnableTerrainDraw)
        finalOutput = texelColor * float4(finalColor, 1.0);
    else
        finalOutput = float4(finalColor, 1.0);
    
    finalOutput.rgb = saturate(finalOutput.rgb);

    return finalOutput;
}

technique Impact

{
    pass P0
    {
        VertexShader = compile VS_SHADERMODEL ImpactVS();
        PixelShader = compile PS_SHADERMODEL ps_RenderTerrain();
    }

    pass P1
    {
        PixelShader = compile PS_SHADERMODEL MainPS();
    }

};