shader_type canvas_item;
uniform float offset = 0.0;

varying vec4 posWS;

void vertex() {
	// Called for every vertex the material is visible on.
	
	posWS = MODEL_MATRIX * vec4(VERTEX, 0.0, 1.0);
	
	VERTEX.y += sin(posWS.x * 0.03 + TIME + offset) * 2.0;
}

void fragment() {
	
	// COLOR = texture(TEXTURE, floor(UV * 128.0) / 128.0) * COLOR;
	// Called for every pixel the material is visible on.
}

//void light() {
	// Called for every pixel for every light affecting the CanvasItem.
	// Uncomment to replace the default light processing function with this one.
//}
