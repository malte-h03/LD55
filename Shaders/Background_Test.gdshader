shader_type canvas_item; 


float circle(vec2 uv, vec2 pos, float rad) {
	float d = length(pos - uv) - rad ;
	float t = clamp(d, 0.0, 1.0);
	return t;
}

uniform vec2 portalCenter;
uniform float maskProgress ;
uniform float swirlStrength;
uniform float portalRadius;
varying float angle;
uniform vec4 color1;
uniform vec4 color2;
uniform float newRes;
varying float particleLife;

uniform sampler2D noiseTexture;
uniform sampler2D suckTexture;


void vertex() {
	particleLife = INSTANCE_CUSTOM.y;
	// Called for every vertex the material is visible on.
}

void fragment() {
	// Called for every pixel the material is visible on.
	angle = particleLife * 5.f;
	
	vec2 newResUniform = abs(vec2(newRes, newRes));
	vec2 newUV2 = (floor(UV * newResUniform) / newResUniform);
	
	vec2 portalDist = newUV2 - portalCenter;
    float dist = length(portalDist);
	
	vec2 newUV = vec2(UV.x * 1.5f - 0.25f, UV.y);
	
	// Calculate swirl effect
    float swirlAmount = swirlStrength * (portalRadius - dist) / portalRadius;
    //angle = 1.;
	//angle -= TIME;
	
    // Apply swirl distortion
    newUV += vec2(cos(angle + swirlAmount), sin(angle + swirlAmount)) * swirlAmount;
	
	vec4 portalTexture = texture(noiseTexture, newUV);
	vec4 suck = texture(suckTexture, vec2(newUV2.x * 1.5f - 0.25f, newUV2.y));
	vec4 portalNoiseTexture = texture(noiseTexture, newUV);
	
	vec2 center = UV * 0.5;
	float radius = 0.25 * UV.y;
	
	vec4 noise1 = texture(noiseTexture, vec2(UV.x, UV.y + TIME * 0.1));
	vec4 noise2 = texture(noiseTexture, vec2(UV.x + 0.1f, UV.y + (TIME * 0.2)));
	vec4 noise3 = texture(noiseTexture, vec2(UV.x + 0.15f, UV.y + (TIME * 0.3)));
	
	float steppedNoise1 = step(noise1.x, pow(sin(particleLife * 3.14), 0.1f));
	float steppedNoise2 = step(noise2.x, pow(sin(particleLife * 3.14), 0.1f));
	float steppedNoise3 = step(noise3.x, pow(sin(particleLife * 3.14), 0.1f));
	float steppedNoiseCircle = step(noise3.x, 0.7f);

	float floatNoise = steppedNoise1;
	float floatNoise2 = steppedNoise2;
	float floatNoise3 = steppedNoise3;
	
	vec4 noise1x = vec4(floatNoise, floatNoise, floatNoise, floatNoise);
	vec4 noise2x = vec4(floatNoise2, floatNoise2, floatNoise2, floatNoise2);
	vec4 noise3x = vec4(floatNoise3, floatNoise3, floatNoise3, floatNoise3);
	
	float multiNoise = (noise1x * noise2x * noise3x).x;
	//maskProgress += sin(TIME);
	float circleMask = step(circle(vec2(newUV2.x * 1.5f - 0.25f, newUV2.y), vec2(0.5, 0.5), 0.f), maskProgress);
	
	vec4 portalColors = mix(color1, color2, portalTexture);
	
	COLOR.rgb = (portalColors.rgb + suck.rgb) - (0.5f - suck.rgb);
	
	COLOR.a = (noise1x * noise2x * noise3x).x * circleMask;
	//COLOR.a = 1.f;
}

//void light() {
	// Called for every pixel for every light affecting the CanvasItem.
	// Uncomment to replace the default light processing function with this one.
//}
