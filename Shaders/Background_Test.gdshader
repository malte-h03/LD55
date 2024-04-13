shader_type canvas_item; 

float circle(vec2 uv, vec2 pos, float rad) {
	float d = length(pos - uv) - rad ;
	float t = clamp(d, 0.0, 1.0);
	return t;
}

uniform float maskProgress ;

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	// Called for every pixel the material is visible on.
	
	vec2 center = UV * 0.5;
	float radius = 0.25 * UV.y;
	
	vec4 noise1 = texture(TEXTURE, vec2(UV.x, UV.y + TIME * 0.1));
	vec4 noise2 = texture(TEXTURE, vec2(UV.x + 0.1f, UV.y + (TIME * 0.2)));
	vec4 noise3 = texture(TEXTURE, vec2(UV.x + 0.15f, UV.y + (TIME * 0.3)));
	
	float steppedNoise1 = step(noise1.x, sin(TIME) + 1. * 0.5f);
	float steppedNoise2 = step(noise2.x, sin(TIME) + 1. * 0.6f);
	float steppedNoise3 = step(noise3.x, sin(TIME) + 1. * 0.7f);
	float steppedNoiseCircle = step(noise3.x, 0.7f);

	float floatNoise = steppedNoise1;
	float floatNoise2 = steppedNoise2;
	float floatNoise3 = steppedNoise3;
	
	vec4 noise1x = vec4(floatNoise, floatNoise, floatNoise, floatNoise);
	vec4 noise2x = vec4(floatNoise2, floatNoise2, floatNoise2, floatNoise2);
	vec4 noise3x = vec4(floatNoise3, floatNoise3, floatNoise3, floatNoise3);
	
	float multiNoise = (noise1x * noise2x * noise3x).x;
	//maskProgress += sin(TIME);
	float circleMask = step(circle(UV, vec2(0.5, 0.5), 0.f), maskProgress);
	
	COLOR = (noise1x * noise2x * noise3x) * circleMask;
	
}

//void light() {
	// Called for every pixel for every light affecting the CanvasItem.
	// Uncomment to replace the default light processing function with this one.
//}
