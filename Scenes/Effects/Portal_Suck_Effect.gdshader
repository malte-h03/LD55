shader_type canvas_item;
varying float particleLife;
uniform sampler2D Noise1;

uniform vec4 enemyColor;
uniform float newRes;

void vertex() {
	particleLife = INSTANCE_CUSTOM.y;
	// Called for every vertex the material is visible on.
}

void fragment() {
	// Called for every pixel the material is visible on.
	vec2 newResUniform = abs(vec2(newRes, newRes));
	vec2 newUV = (floor(UV * newResUniform) / newResUniform);
	
	vec2 newUVtranslate1 = (floor(vec2(UV.x + (particleLife * 0.35f), UV.y - particleLife * 0.35f) * newResUniform) / newResUniform);
	vec2 newUVtranslate2 = (floor(vec2(UV.x - (particleLife * 0.55f), UV.y + (particleLife * 0.55f)) * newResUniform) / newResUniform);
	
	float particleLifeReverse = (particleLife);
	vec4 explodeTexture = texture(TEXTURE, newUV);
	vec4 noiseTexture = texture(Noise1, newUVtranslate1);
	vec4 noiseTexture2 = texture(Noise1, newUVtranslate2);
	
	float steppedRadialLow = step(explodeTexture.x, particleLifeReverse * particleLifeReverse + 0.15f );
	float steppedRadialHigh = step(explodeTexture.x, particleLifeReverse);
	
	float steppedRadialSubtract = steppedRadialHigh - steppedRadialLow;
	
	COLOR = explodeTexture * enemyColor;
	COLOR.a = ((explodeTexture.x * steppedRadialSubtract * (noiseTexture.x * noiseTexture2.x) * 10.f) * particleLifeReverse) * 0.25f;
}

//void light() {
	// Called for every pixel for every light affecting the CanvasItem.
	// Uncomment to replace the default light processing function with this one.
//}
