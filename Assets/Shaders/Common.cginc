#ifndef COMMON_INCLUDED
#define COMMON_INCLUDED

#define PI 3.1415926535

struct Particle
{
    float3 position;
    float3 velocity;
    float4 color;
    float2 lifeTime;
    float size;
    bool alive;
};

struct MeshData
{
    float3 vertex;
    float2 uv;
};

float nrand(float2 uv)
{
    return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453123);
}

float nrand(float3 uvw)
{
    return frac(sin(dot(uvw, float3(12.9898, 78.233, 45.5432))) * 43758.5453123);
}

float3 pointOnSphere(float u, float v, float radius)
{
    float theta = 2 * PI * u;
    float phi = acos(2 * v - 1);
    float x = radius * sin(phi) * cos(theta);
    float y = radius * sin(phi) * sin(theta);
    float z = radius * cos(phi);
    return float3(x, y, z);
}

#endif