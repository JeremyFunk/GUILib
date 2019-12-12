#version 330

in vec2 f_texCoords;

uniform int u_renderMode;
uniform sampler2D u_texture;
uniform vec4 u_color;

const float width = 0.46;
const float edge = 0.19;

out vec4 color;

void main()
{
    if(u_renderMode == 0){
        color = texture(u_texture, f_texCoords);
    }else if(u_renderMode == 1){
        color = u_color;
    }else if(u_renderMode == 2){
        float distance = 1.0 - texture2D(u_texture, f_texCoords).a;
        float alpha = 1.0 - smoothstep(width, width + edge, distance);

        color = vec4(u_color.xyz, alpha);
    }else{
        color = vec4(1);
    }
}