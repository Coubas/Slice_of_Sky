Shader "MyShaders/NewImageEffectShader"
{
	Properties
	{
		_Colour("Coleur1", Color) = (1,1,1,1)
		_Colour2 ("Coleur1", Color) = (1,1,1,1)
		_Colour3("ColeurFond", Color) = (0,0,0,1)
		_Iteration("Nb Iteration", Range(1,10)) = 2
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 pos : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			fixed4 _Colour;
			fixed4 _Colour2;
			fixed4 _Colour3;
			int _Iteration;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed2 uv = i.pos.xy / _ScreenParams.xy;

				fixed4 fragColor;
				float time = _Time[1];
				float res = 1.;
				for (int i = 0; i < _Iteration; i++) {
					res += cos(uv.y*12.345 - time*4. + cos(res*12.234)*.2 + cos(uv.x*32.2345 + cos(uv.y*17.234))) + cos(uv.x*12.345);
				}

				fixed4 c = lerp(_Colour,//fixed3(_Colour.x, _Colour.y, _Colour.z),
					_Colour2,//fixed3(_Colour2.x, _Colour2.y, _Colour2.z),
					cos(res + cos(uv.y*24.3214)*.1 + cos(uv.x*6.324 + time*4.) + time)*.5 + .5);

				c = lerp(c,
					_Colour3,//fixed3(_Colour3.x, _Colour3.y, _Colour3.z),
					clamp((length(uv - .5 + cos(time + uv.yx*4.34 + uv.xy*res)*.1)), 0., 1.));

				return fragColor = c;//fixed4(c.x, c.y, c.z, 1.);
			}
			ENDCG
		}
	}
}
