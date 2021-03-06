#include "Common.hlsl"

struct STriVertex {
    float3 vertex;
    float4 color;
};

StructuredBuffer<STriVertex> BTriVertex : register(t0);

// Raytracing acceleration structure, accessed as a SRV
RaytracingAccelerationStructure SceneBVH : register(t2);

[shader("closesthit")] 
void ClosestHit(inout HitInfo payload, Attributes attrib) {

	float3 lightPos = float3(-2, 2, 2);

    // Find the world - space hit position
    float3 worldOrigin = WorldRayOrigin() + RayTCurrent() * WorldRayDirection();

    float3 lightDir = normalize(lightPos - worldOrigin);

    // Fire a shadow ray. The direction is hard-coded here, but can be fetched
    // from a constant-buffer
    RayDesc ray;
    ray.Origin = worldOrigin;
    ray.Direction = lightDir;
    ray.TMin = 0.01;
    ray.TMax = 100000;

    // Initialize the ray payload
    ShadowHitInfo shadowPayload;
    shadowPayload.isHit = false;
	
	// Trace the ray
	TraceRay(SceneBVH, RAY_FLAG_NONE, 0xFF,	1, 0, 1, ray, shadowPayload);
	
	float factor = shadowPayload.isHit ? 0.3 : 1.0;

    float3 hitColor = float3(float3(1.0, 0.0, 0.5) * factor);

    payload.colorAndDistance = float4(hitColor, RayTCurrent());
}

[shader("closesthit")]
void PlaneClosestHit(inout HitInfo payload, Attributes attrib)
{
    float3 lightPos = float3(-2, 2, 2);

    // Find the world - space hit position
    float3 worldOrigin = WorldRayOrigin() + RayTCurrent() * WorldRayDirection();

    float3 lightDir = normalize(lightPos - worldOrigin);

    // Fire a shadow ray. The direction is hard-coded here, but can be fetched
    // from a constant-buffer
    RayDesc ray;
    ray.Origin = worldOrigin;
    ray.Direction = lightDir;
    ray.TMin = 0.01;
    ray.TMax = 100000;

    // Initialize the ray payload
    ShadowHitInfo shadowPayload;
    shadowPayload.isHit = false;
	
	// Trace the ray
	TraceRay(SceneBVH, RAY_FLAG_NONE, 0xFF,	1, 0, 1, ray, shadowPayload);
	
	float factor = shadowPayload.isHit ? 0.3 : 1.0;

    float4 hitColor = float4(float3(0.0, 0.7, 0.3) * factor, RayTCurrent());
    payload.colorAndDistance = float4(hitColor);
}