shader_type canvas_item;

uniform float circleMaskAdjust;
uniform float circleRotationMask;
uniform float circleRotationSwipe;
uniform vec2 circlePositionMask;
uniform vec2 circlePositionSwipe;

uniform vec4 SwipeColor;

uniform sampler2D noise : repeat_enable;

varying float particleLife;

float circle(vec2 uv, vec2 pos, float rad) {
	float d = length(pos - uv) - rad;
	float t = clamp(d, 0.0, 1.0);
	return t;
}

vec2 rotateUV(vec2 uv, float rotation, vec2 mid)
{
    return vec2(
      cos(rotation) * (uv.x - mid.x) + sin(rotation) * (uv.y - mid.y) + mid.x,
      cos(rotation) * (uv.y - mid.y) - sin(rotation) * (uv.x - mid.x) + mid.y
    );
}

void vertex() {
	
	particleLife = INSTANCE_CUSTOM.y;
	// Called for every vertex the material is visible on.
}

void fragment() {
	
	//float particleLifetime = INSTANCE_CUSTOM.y;
	vec4 swipeTexture = texture(TEXTURE, rotateUV(UV,particleLife * 3.5f + 1.2f, circlePositionSwipe));
	float circleMask = step(circle(rotateUV(UV, 0.5f, circlePositionMask), vec2(0.5, 0.5), 0.5f), circleMaskAdjust);
	
	vec4 noiseTex = texture(noise, rotateUV(vec2(UV.x + (particleLife * 0.24f), UV.y + (particleLife * 0.24f)),particleLife * 3.5f + 1.2f, circlePositionSwipe));
	vec4 noiseTex2 = texture(noise, rotateUV(vec2(UV.x - (particleLife * 0.5f), UV.y * 0.67f),particleLife * 3.5f + 1.2f, circlePositionSwipe));
	vec4 noiseTex3 = texture(noise, vec2(UV.x * 0.1f - particleLife, UV.y));
	
	float steppedNoiseTex = step(noiseTex.x, (1.f - particleLife) * 1.6f);
	float steppedNoiseTex2 = step(noiseTex2.x, (1.f - particleLife) * 1.8f);
	//float lifetime = in float LIFETIME;
	
	
	
	COLOR = (swipeTexture * circleMask) * SwipeColor;
	COLOR.a = (swipeTexture.x * circleMask * noiseTex.x * noiseTex2.x * 4.f) * ((steppedNoiseTex * steppedNoiseTex2));
	// Called for every pixel the material is visible on.
}

//void light() {
	// Called for every pixel for every light affecting the CanvasItem.
	// Uncomment to replace the default light processing function with this one.
//}
