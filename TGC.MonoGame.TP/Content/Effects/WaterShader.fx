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
float WaterPositionY = 0.0f;

uniform float2 u_resolution;
uniform float2 u_mouse;
uniform float u_time;


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

VertexShaderOutput MainVS(in VertexShaderInput input)
{
    // Clear the output
    VertexShaderOutput output = (VertexShaderOutput)0;

    //float WaveHeight = 5;
    //float y = input.Position.y;
    //input.Position.y += sin(Time) * WaveHeight;

    // Model space to World space
    float4 worldPosition = mul(input.Position, World);

    //float WaveHeight = 5;
    //worldPosition.x += sin(Time + 14 * worldPosition.x * .1) * WaveHeight;
    //worldPosition.y += cos(Time + 15 * worldPosition.x * 0.01) * WaveHeight;

    float fade = clamp(cos(Time * 0.3) + 0.5, 0.15, 1) * 0.5 + 0.5;
    float speed = .05 + (1 - fade) * 0.02;
    float offset = 10;
    float radius = 3;
    float3 rotateOffset = float3(0, 0, 0);

    rotateOffset.x = sin((worldPosition.x  + Time * speed ) * offset) * radius * noise(worldPosition.z * 0.02) ;
    rotateOffset.z = sin((worldPosition.z + Time * speed) * offset) * radius;
    
    float multiplyTime = (1 - frac(Time * 0.01)) * frac(Time * 0.01);
    rotateOffset.y = 0.4 * cos((worldPosition.x * .5 + Time * speed) * offset) * radius * 0.5;
    rotateOffset.y += (1 - frac(Time * 0.1)) * frac(Time * 0.1) * 0.1 * cos((-worldPosition.z  + Time * speed * 1.3) * offset) * radius;
    rotateOffset.y += noise(worldPosition.x * 10) * 0.05 + noise(worldPosition.z  * 1000) * 0.01;


    
    worldPosition.xyz += rotateOffset.xyz;
    worldPosition.xyz *= float3(1, 3/fade * 2.5, 1);

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
    
    if (input.WorldPosition.y * 0.1 > -1) {
        float n = input.WorldPosition.y * 0.1 * noise(input.WorldPosition.x * 0.01) * noise(input.WorldPosition.z * 0.01);
        color += float4(1, 1, 1, 1) * float4(n, n, n, 1);
    }
        

    //color += float4(1, 1, 1, 1) * noise(input.WorldPosition) * 0.5;
    //if (input.WorldPosition.y * 0.01 > - 1)
    //    color += float4(1,1,1,1) * float4(0, (1 - frac(Time * 0.1)) * frac(Time * 0.1)  *saturate(input.WorldPosition.y + 10) * noise(input.WorldPosition.z * 0.1) * noise(input.WorldPosition.x), 0,1) ;
    // Color and texture are combined in this example, 80% the color of the texture and 20% that of the vertex
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
