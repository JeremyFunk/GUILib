#version 330

layout (location = 0) in vec3 position;
layout (location = 1) in vec2 texCoords;

out vec2 f_texCoords;

uniform vec2 u_positionOffset;
uniform vec2 u_windowScale;

uniform float u_absoluteScale;

void main()
{
	vec3 actualPosition = vec3(position);
	if(actualPosition.x < 0){	
		actualPosition.x = actualPosition.x * u_absoluteScale + actualPosition.x * (1 - u_absoluteScale);
	}else{
		actualPosition.x = actualPosition.x * u_absoluteScale - actualPosition.x * (1 - u_absoluteScale);
	}

	if(actualPosition.y < 0){	
		actualPosition.y = actualPosition.y * u_absoluteScale + actualPosition.y * (1 - u_absoluteScale);
	}else{
		actualPosition.y = actualPosition.y * u_absoluteScale - actualPosition.y * (1 - u_absoluteScale);
	}

	float normalX = (actualPosition.x * u_windowScale.x + u_positionOffset.x);
	float normalY = (actualPosition.y * u_windowScale.y + u_positionOffset.y);
	gl_Position = vec4(normalX, normalY, actualPosition.z, 1);
    f_texCoords = texCoords;
}