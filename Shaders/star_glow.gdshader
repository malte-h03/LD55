shader_type canvas_item;
render_mode unshaded;

varying vec4 vertexWS;

void vertex() {
	// Called for every vertex the material is visible on.
	vertexWS = MODEL_MATRIX * vec4(VERTEX, 0.0, 1.0);
}

void fragment() {
	
	vec4 tex = texture(TEXTURE, UV + vec2(sin(vertexWS.x + TIME), cos(vertexWS.y + TIME)) * 0.001);
	float lum = (tex.x + tex.y+ tex.z) / 3.0;
	float mask = step(0.5, lum);
	mask *= 10.0;
	mask += 1.0;
	COLOR = tex * mask * 0.3f;
	// Called for every pixel the material is visible on.
}

//void light() {
	// Called for every pixel for every light affecting the CanvasItem.
	// Uncomment to replace the default light processing function with this one.
//}
