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

float3 ambientColor = float3(0.2, 0.2, 0.2); // Color ambiental
float3 diffuseColor = float3(1.0, 1.0, 1.0); // Color difuso
float3 specularColor = float3(1.0, 1.0, 1.0); // Color especular
float KAmbient = 1.0; // Factor de ambiente
float KDiffuse = 1.0; // Factor difuso
float KSpecular = 1.0; // Factor especular
float shininess = 32.0; // Brillo especular

float3 lightPosition;
float3 eyePosition; // Camera position


bool onhit;

float3 ImpactPosition;
float3 TankPosition;

float impacto; // tamaño del impacto
float velocidad; // profundidad del impacto

/*
float angulo; //angulo de giro de la esfera cuando el tanque gira

float3x3 rotationMatrixY(float alpha)
{
    float cosA = cos(alpha);
    float sinA = sin(alpha);
    
    return float3x3(
        cosA, 0.0, -sinA,
        0.0, 1.0, 0.0,
        sinA, 0.0, cosA
    );
}
*/

float TrackOffset;
bool IsTrack;

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

/*
float3 rotacion(float3 direccion)
{
    //derivado de los apuntes de 2D
    float3x3 M_Rot = rotationMatrixY(angulo);
    return mul(direccion, M_Rot);
}
*/

VertexShaderOutput MainVS(in VertexShaderInput input)
{
    VertexShaderOutput output;

    // Transformaciones de espacio
    float4 worldPosition = mul(input.Position, World);

    // Lógica adicional existente
    if (onhit)
    {
        float3 direccion = VersorDireccion(ImpactPosition, TankPosition);
        //direccion = rotacion(direccion);
        float3 c_Esfera = TankPosition + (direccion * velocidad);
        float r_Esfera = impacto;
        if (distance(c_Esfera, worldPosition.xyz) <= r_Esfera)
        {
            worldPosition.xyz = desplazarPorRadio(worldPosition.xyz, r_Esfera, c_Esfera);
        }
    }
    
    

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
    // Muestrear la textura
    float4 texelColor = tex2D(TextureSampler, input.TextureCoordinate.xy);

    // Dirección de la luz (asumiendo luz solar, puedes ajustar según necesites)
    float3 lightDirection = normalize(lightPosition - input.WorldPosition.xyz);

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

    // Aplicar el color final a la textura
    float4 finalOutput = texelColor * float4(finalColor, 1.0);
    // Desplazar la coordenada de textura de las orugas
    return finalOutput;
}

technique BasicColorDrawing
{
    pass P0
    {
        VertexShader = compile VS_SHADERMODEL MainVS();
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
};
