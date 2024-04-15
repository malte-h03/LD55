shader_type canvas_item;

uniform float newRes;
uniform vec4 color;

varying float particleLife;

void vertex() {
	particleLife = INSTANCE_CUSTOM.y;
	// Called for every vertex the material is visible on.
}

void fragment() {
	
	vec2 newResUniform = abs(vec2(newRes, newRes));
	vec2 newUV2 = (floor(UV * newResUniform) / newResUniform);
	
	vec4 noiseTexture = texture(TEXTURE, newUV2); 
	
	// Called for every pixel the material is visible on.
	COLOR = noiseTexture + (color * (1.f - particleLife));
	COLOR.a = (1.f - particleLife);
}

//void light() {
	// Called for every pixel for every light affecting the CanvasItem.
	// Uncomment to replace the default light processing function with this one.
//}
