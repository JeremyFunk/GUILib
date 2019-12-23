#version 330

in vec2 f_texCoords;

uniform int u_renderMode;
uniform sampler2D u_texture;
uniform vec4 u_color;
uniform vec2 u_scale;
uniform vec2 u_absScale; 
uniform bool u_roundEdges; 
uniform float u_gradient;
uniform float u_edgeWidth;

const float width = 0.46;
const float edge = 0.19;


out vec4 color;



vec2 normalize(vec2 coord, vec2 scale)
{
    return vec2(coord.x*scale.x,coord.y*scale.y);
}


float circle(vec2 coord,vec2 midPoint,float radius)
{
    vec2 dist = coord-midPoint;
    //dot ist performater
    //dot([a,b],[c,d])=a*c+b*d
    float r = radius*radius;
    return 1.-smoothstep(r-(r*0.01),
                         r+(r*0.01),
                         dot(dist,dist));

}

float border(vec2 inCoord, float radius)
{
    vec2 coord = normalize(inCoord,u_absScale);
    //Mittelpunkte ausrechnen
    vec2 midLO = vec2(radius,u_absScale.y-radius);
    vec2 midRO = vec2(u_absScale.x-radius,u_absScale.y-radius);
    vec2 midRU = vec2(u_absScale.x-radius,radius);
    vec2 midLU = vec2(radius,radius);
    if(midLO.x>coord.x && midLO.y<coord.y)
    {
        //return 1;
        return circle(coord,midLO,radius);
    }
    if(midRO.x<coord.x && midRO.y<coord.y)  
    {
        return circle(coord,midRO,radius);
    }
    if(midRU.x<coord.x && midRU.y>coord.y)
    {
        return circle(coord,midRU,radius);
    }
    if(midLU.x>coord.x && midLU.y>coord.y)
    {
        return circle(coord,midLU,radius);
    }
    return 1;
}

void main()
{
    if(u_renderMode == 0){
        if(u_roundEdges == true)
        {
            float curvature = border(f_texCoords,0.5);
            vec4 temp = texture(u_texture, f_texCoords);
            color = vec4(temp.xyz,temp.w*curvature);
        }else{
            vec4 temp = texture(u_texture, f_texCoords);
            color = temp;
        }
    }else if(u_renderMode == 1){
        if(u_roundEdges == true)
        {
            float curvature = border(f_texCoords,0.4);
            
            color = vec4(u_color.xyz,u_color.w*curvature);
        }else{
            color = u_color;
        }
    }else if(u_renderMode == 2){
        float distance = 1.0 - texture2D(u_texture, f_texCoords).a;
        float alpha = 1.0 - smoothstep(width, width + edge, distance);
        
        color = vec4(u_color.xyz, alpha);
    }else{
        color = vec4(1);
    }
}