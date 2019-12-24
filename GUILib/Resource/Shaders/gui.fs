//---------------------------------------
#version 330

in vec2 f_texCoords;

uniform int u_renderMode;

uniform sampler2D u_fillTexture;
uniform vec4 u_fillColor;

uniform sampler2D u_borderTexture;
uniform vec4 u_borderColor;

uniform vec2 u_absScale;
uniform vec2 u_normScale;
uniform vec2 u_windowScale;

uniform bool u_roundEdges;
uniform float u_edgeRadius;

uniform bool u_gradient;
uniform float u_gradientFalloff;
uniform float u_gradientRadius;
uniform float u_gradientOpacity;

uniform bool u_border;
uniform float u_borderWidth;

out vec4 color;

const float width = 0.46;
const float edge = 0.19;

//---------------------------------------

//Utilitie Methodes
vec2 normalize(vec2 coord, vec2 scale)
{
    return vec2(coord.x*scale.x,coord.y*scale.y);
}

vec2 scaleCoord(vec2 coord, vec2 scale)
{
    return vec2(coord.x*scale.x,coord.y*scale.y);
}

float blendScreen(float base, float blend) {

	return 1.0-((1.0-base)*(1.0-blend));

}

//-------------------------------------------------------------

//Shapes Methodes

float circle(vec2 coord,vec2 midPoint,float radius)
{
    vec2 dist = coord-midPoint;
    float r = radius*radius;
    return 1.-smoothstep(r-(r*0.01),
                         r+(r*0.01),
                         dot(dist,dist));

}

float rectangle(vec2 coord, vec2 position, vec2 scale)
{
    vec2 cornerOB = position+scale;
    if(coord.x>position.x && coord.y> position.x && 
    coord.x<cornerOB.x && coord.y <cornerOB.y)
    {
        return 1;
    }
    return 0;
}



vec4 colorMix(float maskA, float maskB, vec4 colorA, vec4 colorB)
{
    return vec4(maskA*colorA.x+maskB*colorB.x,
                maskA*colorA.y+maskB*colorB.y,
                maskA*colorA.z+maskB*colorB.z,
                maskA*colorA.w+maskB*colorB.w);
}

vec2 translateCoord(vec2 coord,vec2 location, vec2 scale)
{
    return (coord+location)*scale;
}

//-------------------------------------------------------------

float rectangleBorder(vec2 coord, float edge, vec2 canavsScale)
{
	if(edge > coord.x )
	{
		return 0;
	}
	if(canavsScale.x-edge < coord.x)
	{
		return 0;
	}
	if(edge>coord.y)
	{
		return 0;
	}
	if(canavsScale.y-edge<coord.y)
	{
		return 0;
	}
	return 1;
}

float roundEdges(vec2 nCoord, vec2 scale, float radius,float offset)
{
    /*
    Offset & nCoord in normalized  space
    */
    vec2 coord = nCoord;

    vec2 midLO = vec2(radius,scale.y-radius);
    vec2 midRO = vec2(scale.x-radius,scale.y-radius);
    vec2 midRU = vec2(scale.x-radius,radius);
    vec2 midLU = vec2(radius,radius);

    if(midLO.x>coord.x && midLO.y<coord.y)
    {
        //return 1;
        return circle(coord,midLO,radius-offset);
    }
    if(midRO.x<coord.x && midRO.y<coord.y)  
    {
        return circle(coord,midRO,radius-offset);
    }
    if(midRU.x<coord.x && midRU.y>coord.y)
    {
        return circle(coord,midRU,radius-offset);
    }
    if(midLU.x>coord.x && midLU.y>coord.y)
    {
        return circle(coord,midLU,radius-offset);
    }
	if(0<rectangleBorder(coord,offset,scale))
	{
		return 1;
	}
    return 0;
}

float gradientSingle(float value, float falloff, float radius)
{
    float remap=(value-1+radius)/radius;
	if(remap<0)
	{
		return 0;
	}
    return pow(remap,falloff);
}

float gradientDirection(vec2 coord, float falloff,float limiter, vec2 scale, int direction,float radius)
{
    //coord normalized Coord
    //Direction: 1 := ^, 2 := >, 3 := v , 4 := < 
    float value;
    if(direction==1)
    {
        value=1-coord.y;
    }
    else if(direction == 2)
    {
        value=coord.x-scale.x+1;
    }
    else if(direction == 3)
    {
        value = coord.y-scale.y+1;
    }
    else
    {
        value = 1-coord.x;
    }
    if(value>limiter || value<0)
    {
        return 0;
    }
    return gradientSingle(value,falloff, radius);
}

float gradientQuad(vec2 coord, vec2 scale, float falloff, float radius)
{
    //coord is normalized
    float up = gradientDirection(coord, falloff,1,scale, 1,radius);
    float left = gradientDirection(coord, falloff,1,scale, 2,radius);
    float down = gradientDirection(coord, falloff,1,scale, 3,radius);
    float rigth = gradientDirection(coord, falloff,1,scale, 4,radius);

    float YAxis = blendScreen(up,down);
    float XAxis = blendScreen(left,rigth);
    return blendScreen(YAxis,XAxis);

}

float gradientQuad(vec2 coord, vec2 scale, float falloff,float radius, float offset)
{
    vec2 newCoord = translateCoord(coord,vec2(-offset),scale/(scale-vec2(2*offset)));
    if(rectangleBorder(coord,offset,scale)==0)
    {
        return 0;
    }
	return gradientQuad(newCoord,scale,falloff,radius);
}



vec4 borderMixer(vec2 coord,vec4 fillColor, vec4 borderColor)
{

    float insideMask;
    float outsideMask;
    if(u_roundEdges)
    {
        insideMask=roundEdges(coord,u_normScale,u_edgeRadius,u_borderWidth);
        if(u_border)
        {
            outsideMask=roundEdges(coord,u_normScale,u_edgeRadius,0);
        }
        else
        {
            outsideMask=0;
        }
    }else
    {
        insideMask=rectangleBorder(coord, u_borderWidth, u_normScale);
        if(u_border)
        {
            outsideMask=1;
        }
        else
        {
            outsideMask=0;
        }
    }
    return colorMix(outsideMask-insideMask,insideMask,borderColor,fillColor);
}

vec4 solidColor()
{
    vec2 coord = normalize(f_texCoords,u_normScale);
    float gradiant=1;
    if(u_gradient)
    {
        gradiant=1-u_gradientOpacity+u_gradientOpacity*gradientQuad(coord,u_normScale, u_gradientFalloff,u_gradientRadius, u_borderWidth);

    }
    return borderMixer(coord,vec4(u_fillColor.xyz,u_fillColor.w*gradiant),u_borderColor);
}

vec4 fontColor()
{
    float distance = 1.0 - texture2D(u_fillTexture, f_texCoords).a;
    float alpha = 1.0 - smoothstep(width, width + edge, distance);
        
    return vec4(u_fillColor.xyz, alpha);
}

vec4 textureColor()
{
    vec2 coord = normalize(f_texCoords,u_normScale);
    vec4 temp = texture(u_fillTexture, f_texCoords);

    float gradiant=1;
    if(u_gradient)
    {
        gradiant=1-u_gradientOpacity+u_gradientOpacity*gradientQuad(coord,u_normScale, u_gradientFalloff,u_gradientRadius, u_borderWidth);

    }
    return borderMixer(coord,vec4(temp.xyz,temp.w*gradiant),u_borderColor);
}

void main()
{
	if(u_renderMode==0)
	{
        color = textureColor();
		
	}else if(u_renderMode==1)
    {
        color = solidColor();
    }
    else if(u_renderMode == 2){
        color = fontColor();
    }else{
        color = vec4(0.5);
    }
    
}