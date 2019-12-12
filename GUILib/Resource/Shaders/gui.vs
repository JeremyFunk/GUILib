#version 330

layout (location = 0) in vec3 position;
layout (location = 1) in vec2 texCoords;

out vec2 f_texCoords;

uniform vec2 u_positionOffset;
uniform vec2 u_scale;

void main()
{
	gl_Position = vec4(position.x * u_scale.x + u_positionOffset.x, position.y * u_scale.y + u_positionOffset.y, position.z, 1);
    f_texCoords = texCoords;
}