shader_type canvas_item;

uniform sampler2D noise : repeat_enable;
uniform sampler2D band : repeat_enable;

uniform float newRes;

varying float particleLife;

float circle(vec2 uv, vec2 pos, float rad) {
	float d = length(pos - uv) - rad;
	float t = clamp(d, 0.0, 1.0);
	return t;
}

void vertex() {
	particleLife = INSTANCE_CUSTOM.y;
	// Called for every vertex the material is visible on.
}



void fragment() {
	vec2 newResUniform = abs(vec2(newRes, newRes));
	vec2 newUV = (floor(UV * newResUniform) / newResUniform);
	
	vec4 noiseTex = texture(noise, vec2(newUV.x, newUV.y + particleLife));
	vec4 bandTex = texture(band, vec2(newUV.x, newUV.y + (particleLife + (TIME * 0.001f * particleLife))));
	
	float circleMask = step(circle(vec2(newUV.x,newUV.y * 0.75f), vec2(0.5, -0.0f), 0.01f), 0.5f);
	
	vec4 dashColor = vec4(1.7, 0.6, 2.0, 1.f);
	
	COLOR = noiseTex.xxxx * circleMask * pow(newUV.y, 2.f) * 10.f * dashColor * pow((1.f - particleLife),5.f) * bandTex;
	// Called for every pixel the material is visible on.
}

//void light() {
	// Called for every pixel for every light affecting the CanvasItem.
	// Uncomment to replace the default light processing function with this one.
//}
