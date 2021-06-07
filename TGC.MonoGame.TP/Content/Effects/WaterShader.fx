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
float Time = 0;

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
    float2 TextureCoordinate : TEXCOORD1;
    float4 WorldPosition : TEXCOORD2;
};

texture ModelTexture;
sampler2D textureSampler = sampler_state
{
    Texture = (ModelTexture);
    MagFilter = Linear;
    MinFilter = Linear;
    AddressU = Mirror;
    AddressV = Mirror;
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

    // Smooth Interpolation

    // Cubic Hermine Curve.  Same as SmoothStep()
    float2 u = f * f * (3.0 - 2.0 * f);
    // u = smoothstep(0.,1.,f);

    // Mix 4 coorners percentages
    return lerp(a, b, u.x) + (c - a) * u.y * (1.0 - u.x) + (d - b) * u.x * u.y;
}

float3 createWave(float steepness, float numWaves, float2 waveDir, float waveAmplitude, float waveLength, float peak, float speed, float4 position) {
    float3 wave = float3(0,0,0);

    float spaceMult = 2 * 3.14159265359 / waveLength;
    float timeMult = speed * 2 * 3.14159265359 / waveLength;

    wave.x = waveAmplitude * steepness * waveDir.x * cos(dot(position.xz, waveDir) * spaceMult + Time * timeMult);
    wave.y = 2 * waveAmplitude * pow(((sin(dot(position.xz, waveDir) * spaceMult + Time * timeMult) + 1) / 2), peak);
    wave.z = waveAmplitude * steepness * waveDir.y * cos(dot(position.xz, waveDir) * spaceMult + Time * timeMult);
    return wave;
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


    worldPosition.xyz += (wave1 + wave2 + wave3 + wave4 + wave5 + wave6 * 0.4 + wave7 * 0.6) / 6;



    output.WorldPosition = worldPosition;

    // World space to View space
    float4 viewPosition = mul(worldPosition, View);
    // View space to Projection space
    output.Position = mul(viewPosition, Projection);
    // Propagate texture coordinates
    output.TextureCoordinate = input.TextureCoordinate;
    // Propagate color by vertex
    output.Color = input.Color;
    return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
    // Get the texture texel textureSampler is the sampler, Texcoord is the interpolated coordinates
    float4 textureColor = tex2D(textureSampler, input.TextureCoordinate);
    textureColor.a = 1;

    float4 color = float4(0, 0.2, 0.4, 1);

    float crestaBase = saturate(input.WorldPosition.y * 0.008) + 0.22;
    color += float4(1, 1, 1, 1) * float4(crestaBase, crestaBase, crestaBase, 1);
    
    if (input.WorldPosition.y * 0.2 > -1) {
        float n = input.WorldPosition.y * 0.5 * noise(input.WorldPosition.x * 0.01) * noise(input.WorldPosition.z * 0.01) * textureColor.r;
        color += float4(.1, .1, .1, 1) * float4(n, n, n, 1);
        //color += float4(1, 1, 1, 1) * float4(n, n, n, 1);
    }

    return color;
    //return textureColor * 0.2 + color * 0.8;
}

technique BasicColorDrawing
{
    pass P0
    {
        VertexShader = compile VS_SHADERMODEL MainVS();
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
};
