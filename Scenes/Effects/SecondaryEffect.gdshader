shader_type canvas_item;

render_mode blend_mix;

varying float particleLife;
uniform float newRes;
uniform vec4 effectColor;

//uniform sampler2D

void vertex() {
	particleLife = INSTANCE_CUSTOM.y;
	// Called for every vertex the material is visible on.
}

void fragment() {
	vec2 newResUniform = abs(vec2(newRes, newRes));
	vec2 newUV = (floor(UV * newResUniform) / newResUniform);
	
	float invertedParticleLife = 1.f - particleLife;
	
	vec4 Texture = texture(TEXTURE, newUV);
	
	// Called for every pixel the material is visible on.
	COLOR = Texture.xxxx * (particleLife * 100.f) * effectColor;
	COLOR.a = Texture.x * invertedParticleLife;
}

//void light() {
	// Called for every pixel for every light affecting the CanvasItem.
	// Uncomment to replace the default light processing function with this one.
//}
