

// 円周率
#define PI 3.1415926535
#define CENTER fixed2(0.5, 0.5)


//! 線
float verticalline(fixed2 uv, float pos, float thickness)
{
	return step(pos - thickness / 2.0, uv.x) * step(1 - pos - thickness / 2.0, 1.0 - uv.x);
}

//! 円
float circle(fixed2 uv, float radius, fixed2 origin)
{
	fixed r = distance(uv, origin);
	return step(-radius, -r);
}

//! アンチエイリアス円
//! i_diameter < o_diameter
float circle_aa(fixed2 uv, float i_diameter, float o_diameter, fixed2 origin)
{
	fixed r = distance(uv, origin);
	return smoothstep(-o_diameter, -i_diameter, -r);
}

//! 輪
//! i_diameter < o_diameter
float ring(fixed2 uv, float i_diameter, float o_diameter, fixed2 origin)
{
	fixed r = distance(uv, origin);
	return step(i_diameter, r) * step(-o_diameter, -r);
}

//! 線2 (ax + by + c = 0)
float abcline(fixed2 uv, float a, float b, float c, float thickness)
{
	float d = abs(a*uv.x + b*uv.y + c)/ sqrt(a*a + b*b);
	return step(-thickness, -d);
}

//! 線2 (ax + by + c = 0, (x0, y0))
float abcline(fixed2 uv, float a, float b, fixed2 sample_pos, float thickness)
{
	float c = -(a*sample_pos.x + b*sample_pos.y);
	float d = abs(a*uv.x + b*uv.y + c)/ sqrt(a*a + b*b);
	return step(-thickness, -d);
}

//! 線3 (y = gx + i, (x0, y0))
float abcline(fixed2 uv, float g, fixed2 sample_pos, float thickness)
{
	float a = -g;
	// float b = 1;
	float c = -(a*sample_pos.x + sample_pos.y);
	float d = abs(a*uv.x + uv.y + c)/ sqrt(a*a + 1);
	return step(-thickness, -d);
}

//! 線4 (theta from x axis, (x0, y0))
float tline(fixed2 uv, float theta, fixed2 sample_pos, float thickness)
{
	return  abcline(uv, tan(theta), sample_pos, thickness);
}

half3 rgb255(float r, float g, float b) {
	return half3(r/255,g/255,b/255);
}
half4 rgba255(float r, float g, float b, float a) {
	return half4(r/255,g/255,b/255,a/255);
}
half4 rgbA255(float r, float g, float b) {
    return half4(r/255,g/255,b/255,1);
}
half4 alpha(half3 rgb, float a) {
	return half4(rgb.x,rgb.y,rgb.z,a);
}
half3 disalpha(fixed4 rgba) {
    return half3(rgba.x,rgba.y,rgba.z);
}
fixed4 changealpha(float4 rgba, float newa) {
	return fixed4(rgba.x,rgba.y,rgba.z,newa);
}


