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


float4x4 View;
float4x4 Projection;
float4x4 World;
float4x4 InverseTransposeWorld;
float3 cameraPosition;
float3 sunPosition;


float KAmbient;
float3 ambientColor;

float KDiffuse;
float3 diffuseColor;

float KSpecular;
float3 specularColor;
float shininess;

float KReflection;
float KFoam;

float Time = 0;


struct VertexShaderInput
{
    float4 Position : POSITION0;
    float4 Color : COLOR0;
    float4 Normal : NORMAL;
    float2 TextureCoordinates : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float2 TextureCoordinates : TEXCOORD0;
    float4 WorldPosition : TEXCOORD1;
    float4 Normal : TEXCOORD2;
};

texture baseTexture;
sampler2D textureSampler = sampler_state
{
    Texture = (baseTexture);
    MagFilter = Linear;
    MinFilter = Linear;
    AddressU = Mirror;
    AddressV = Mirror;
};

texture foamTexture;
sampler2D foamSampler = sampler_state
{
    Texture = (foamTexture);
    MagFilter = Linear;
    MinFilter = Linear;
    AddressU = Mirror;
    AddressV = Mirror;
};

texture normalTexture;
sampler2D normalSampler = sampler_state
{
    Texture = (normalTexture);
    ADDRESSU = WRAP;
    ADDRESSV = WRAP;
    MINFILTER = LINEAR;
    MAGFILTER = LINEAR;
    MIPFILTER = LINEAR;
};

texture environmentMap;
samplerCUBE environmentMapSampler = sampler_state
{
    Texture = (environmentMap);
    MagFilter = Linear;
    MinFilter = Linear;
    AddressU = Clamp;
    AddressV = Clamp;
};


// 2D Random
float random(in float2 st) {
    return frac(sin(dot(st.xy, float2(12.9898, 78.233))) * 43758.5453123);
}

float noise(in float2 st) {
    float2 i = floor(st);
    float2 f = frac(st);

    // Four corners in 2D of a tile
    float a = random(i);
    float b = random(i + float2(1.0, 0.0));
    float c = random(i + float2(0.0, 1.0));
    float d = random(i + float2(1.0, 1.0));

    float2 u = f * f * (3.0 - 2.0 * f);

    return lerp(a, b, u.x) + (c - a) * u.y * (1.0 - u.x) + (d - b) * u.x * u.y;
}

float3 createWave(float steepness, float numWaves, float2 waveDir, float waveAmplitude, float waveLength, float peak, float speed, float4 position) {
    float3 wave = float3(0, 0, 0);

    float spaceMult = 2 * 3.14159265359 / waveLength;
    float timeMult = speed * 2 * 3.14159265359 / waveLength;

    wave.x = waveAmplitude * steepness * waveDir.x * cos(dot(position.xz, waveDir) * spaceMult + Time * timeMult);
    wave.y = 2 * waveAmplitude * pow(((sin(dot(position.xz, waveDir) * spaceMult + Time * timeMult) + 1) / 2), peak);
    wave.z = waveAmplitude * steepness * waveDir.y * cos(dot(position.xz, waveDir) * spaceMult + Time * timeMult);
    return wave;
}

float3 getNormalFromMap(float2 textureCoordinates, float3 worldPosition, float3 worldNormal)
{
    float3 tangentNormal = tex2D(normalSampler, textureCoordinates).xyz * 2.0 - 1.0;

    float3 Q1 = ddx(worldPosition);
    float3 Q2 = ddy(worldPosition);
    float2 st1 = ddx(textureCoordinates);
    float2 st2 = ddy(textureCoordinates);

    worldNormal = normalize(worldNormal.xyz);
    float3 T = normalize(Q1 * st2.y - Q2 * st1.y);
    float3 B = -normalize(cross(worldNormal, T));
    float3x3 TBN = float3x3(T, B, worldNormal);

    return normalize(mul(tangentNormal, TBN));
}




VertexShaderOutput MainVS(in VertexShaderInput input)
{
    // Clear the output
    VertexShaderOutput output = (VertexShaderOutput)0;

    // Model space to World space
    float4 worldPosition = mul(input.Position, World);

    //createWave(float steepness, float numWaves, float2 waveDir, float waveAmplitude, float waveLength, float peak, float speed, float4 position) {

    float3 wave1 = createWave(4, 5, float2(0.5, 0.3), 40, 160, 3, 10, worldPosition);
    float3 wave2 = createWave(8, 5, float2(0.8, -0.4), 12, 120, 1.2, 20, worldPosition);
    float3 wave3 = createWave(4, 5, float2(0.3, 0.2), 2, 90, 5, 25, worldPosition);
    float3 wave4 = createWave(2, 5, float2(0.4, 0.25), 2, 60, 15, 15, worldPosition);
    float3 wave5 = createWave(6, 5, float2(0.1, 0.8), 20, 250, 2, 40, worldPosition);

    float3 wave6 = createWave(4, 5, float2(-0.5, -0.3), 0.5, 8, 0.2, 4, worldPosition);
    float3 wave7 = createWave(8, 5, float2(-0.8, 0.4), 0.3, 5, 0.3, 6, worldPosition);

    // NORMALS //
    float EPSILON = 0.001;
    float3 dxWave1 = createWave(4, 5, float2(0.5, 0.3), 40, 160, 3, 10, float4(worldPosition.x + EPSILON, worldPosition.yz, 1));
    float3 dzWave1 = createWave(4, 5, float2(0.5, 0.3), 40, 160, 3, 10, float4(worldPosition.xy, worldPosition.z + EPSILON, 1));
    float3 dxWave2 = createWave(8, 5, float2(0.8, -0.4), 12, 120, 1.2, 20, float4(worldPosition.x + EPSILON, worldPosition.yz, 1));
    float3 dzWave2 = createWave(8, 5, float2(0.8, -0.4), 12, 120, 1.2, 20, float4(worldPosition.xy, worldPosition.z + EPSILON, 1));
    float3 dxWave3 = createWave(4, 5, float2(0.3, 0.2), 2, 90, 5, 25, float4(worldPosition.x + EPSILON, worldPosition.yz, 1));
    float3 dzWave3 = createWave(4, 5, float2(0.3, 0.2), 2, 90, 5, 25, float4(worldPosition.xy, worldPosition.z + EPSILON, 1));
    float3 dxWave4 = createWave(2, 5, float2(0.4, 0.25), 2, 60, 15, 15, float4(worldPosition.x + EPSILON, worldPosition.yz, 1));
    float3 dzWave4 = createWave(2, 5, float2(0.4, 0.25), 2, 60, 15, 15, float4(worldPosition.xy, worldPosition.z + EPSILON, 1));
    float3 dxWave5 = createWave(6, 5, float2(0.1, 0.8), 20, 250, 2, 40, float4(worldPosition.x + EPSILON, worldPosition.yz, 1));
    float3 dzWave5 = createWave(6, 5, float2(0.1, 0.8), 20, 250, 2, 40, float4(worldPosition.xy, worldPosition.z + EPSILON, 1));
    float3 dxWave6 = createWave(4, 5, float2(-0.5, -0.3), 0.5, 8, 0.2, 4, float4(worldPosition.x + EPSILON, worldPosition.yz, 1));
    float3 dzWave6 = createWave(4, 5, float2(-0.5, -0.3), 0.5, 8, 0.2, 4, float4(worldPosition.xy, worldPosition.z + EPSILON, 1));
    float3 dxWave7 = createWave(8, 5, float2(-0.8, 0.4), 0.3, 5, 0.3, 6, float4(worldPosition.x + EPSILON, worldPosition.yz, 1));
    float3 dzWave7 = createWave(8, 5, float2(-0.8, 0.4), 0.3, 5, 0.3, 6, float4(worldPosition.xy, worldPosition.z + EPSILON, 1));

    worldPosition.xyz += (wave1 + wave2 + wave3 + wave4 + wave5 + wave6 + wave7) / 7;

    float3 normalVector = float3(0, 0, 0);
    normalVector.x = (dxWave1.x + dxWave2.x + dxWave3.x + dxWave4.x + dxWave5.x + dxWave6.x + dxWave7.x) / 7;
    normalVector.z = (dxWave1.z + dxWave2.z + dxWave3.z + dxWave4.z + dxWave5.z + dxWave6.z + dxWave7.z) / 7;

    float3 waterTangent1 = normalize(float3(1, normalVector.x, 0));
    float3 waterTangent2 = normalize(float3(0, normalVector.z, 1));
    input.Normal.xyz = normalize(cross(waterTangent2, waterTangent1));

    output.WorldPosition = worldPosition;
    output.Normal = input.Normal;

    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);
    output.TextureCoordinates = input.TextureCoordinates;
    output.Color = input.Color;

    return output;
}


float4 MainPS(VertexShaderOutput input) : COLOR
{
    float alturaY = clamp(sunPosition.y / 1500, 0.5, 1);

    float3 normal = getNormalFromMap(input.TextureCoordinates, input.WorldPosition.xyz, input.Normal.xyz);

    //float3 worldNormal = input.Normal.xyz * normal;
    float3 worldNormal = input.Normal.xyz + normal;
    float3 reflNormal = input.Normal.xyz * normal;


    // Base vectors
    float3 lightDirection = normalize(sunPosition - input.WorldPosition.xyz);
    float3 viewDirection = normalize(cameraPosition - input.WorldPosition.xyz);
    float3 halfVector = normalize(lightDirection + viewDirection);

    // Get the texture texel textureSampler is the sampler, Texcoord is the interpolated coordinates
    float4 texelColor = tex2D(textureSampler, input.TextureCoordinates);
    float4 foamColor = tex2D(foamSampler, input.TextureCoordinates);

    // Get the texel from the texture
    float3 reflColor = tex2D(textureSampler, input.TextureCoordinates).rgb;

    // Not part of the mapping, just adjusting color
    reflColor = lerp(reflColor, float3(1, 1, 1), step(length(reflColor), 0.01));

    float3 view = normalize(cameraPosition.xyz - input.WorldPosition.xyz);
    float3 reflection = reflect(view, reflNormal);
    float3 reflectionColor = texCUBE(environmentMapSampler, reflection).rgb;

    float3 ambientLight = KAmbient * ambientColor + KFoam * foamColor.rgb;

    // Calculate the diffuse light
    float NdotL = saturate(dot(worldNormal, lightDirection));
    float3 diffuseLight = KDiffuse * diffuseColor * NdotL;

    float3 baseColor = saturate(ambientLight + diffuseLight);

    float crestaBase = saturate(input.WorldPosition.y * 0.008) + 0.22;
    baseColor += saturate(float3(1, 1, 1) * float3(crestaBase, crestaBase, crestaBase));

    if (input.WorldPosition.y * 0.1 > -1) {
        float n = input.WorldPosition.y * 0.5 * noise(input.WorldPosition.x * 0.01) * noise(input.WorldPosition.z * 0.01) * texelColor.r;
        baseColor += float3(.1, .1, .1) * float3(n * saturate(foamColor.r * 2), n * saturate(foamColor.r * 2), n * saturate(foamColor.r * 2));
    }

    float3 specColor = specularColor * texelColor.r;
    float NdotH = dot(worldNormal, halfVector);
    float3 specularLight = sign(NdotL) * KSpecular * specColor * pow(saturate(NdotH), shininess);
    float4 finalColor = float4(lerp(baseColor, reflectionColor * KReflection, 0.5) + specularLight, 1) * alturaY;

    return float4(finalColor.rgb, clamp((1 - foamColor.r), 0.95, 1));

}

technique BasicColorDrawing
{
    pass P0
    {
        VertexShader = compile VS_SHADERMODEL MainVS();
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
};
