#define PI2 6.28318530718

cbuffer cbuf : register(b0)
{
	float Radius = 0.7;
	float InnerRadius = 0.2;
	float Resolution = 128;
};

struct PSin
{
	float4 pos : SV_Position;
	float2 uv : TEXCOORD0;
};

PSin VSfunc(uint vid : SV_VertexID)
{
	PSin o = (PSin)0;
	float rad = lerp(Radius, InnerRadius, vid % 2);
	float i = (floor(vid / 2) / (Resolution - 1));
	o.pos = float4(cos(i * PI2) * rad, sin(i * PI2) * rad, 0.1, 1);
	o.uv = float2(i, vid % 2);
	return o;
}

Texture2D ColTex;

SamplerState sL
{
	Filter = MIN_MAG_MIP_LINEAR;
	AddressU = Clamp;
	AddressV = Clamp;
};

float4 PSfunc(PSin i) : SV_Target
{
	return ColTex.SampleLevel(sL, i.uv, 0);
}

technique11 Tech
{
	pass P0
	{
		SetVertexShader(CompileShader(vs_4_0, VSfunc()));
		SetPixelShader(CompileShader(ps_4_0, PSfunc()));
	}
}