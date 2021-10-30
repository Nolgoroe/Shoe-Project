Shader "Custom/SpriteGradient" {
	Properties{
		_BaseMap("Color (RGB) Alpha (A)", 2D) = "white"{}
		_Color("Left Color", Color) = (1,1,1,1)
		_Color2("Right Color", Color) = (1,1,1,1)
		_Tint("Tint", Range(-1,1)) = 0.5
		_Scale("scale", float) = 0.5
	}

		SubShader{
			 Tags{ "RenderType" = "TransparentCutout" "Queue" = "AlphaTest"}

			Pass {
				CGPROGRAM
				#pragma vertex vert  
				#pragma fragment frag
				#include "UnityCG.cginc"

				uniform sampler2D _BaseMap;
				uniform float4 _BaseMap_ST;
				uniform fixed4 _Color;
				uniform fixed4 _Color2;
				uniform float  _Tint;
				uniform float  _Scale;


				struct v2f {
					float4 pos : POSITION;
					float4 worlspos: TEXTCOORD1;
					float2 texcoord: TEXTCOORD0;
					fixed4 col : COLOR;
					float2 Alpha : TEXCOORD2;
				};

				v2f vert(appdata_full v)
				{
					v2f o;
					o.pos = UnityObjectToClipPos(v.vertex);
					o.worlspos = mul(unity_ObjectToWorld, o.pos);
					o.texcoord = TRANSFORM_TEX(v.texcoord, _BaseMap);
					float clampedLerp = clamp(v.texcoord.y * _Scale + _Tint, 0, 1);
					o.col = lerp(_Color,_Color2, clampedLerp);
					o.Alpha = TRANSFORM_TEX(v.texcoord, _BaseMap);
					return o;
				}


				float4 frag(v2f i) : COLOR{
					float4 c = tex2D(_BaseMap, i.Alpha) * i.col;
					clip(c.a - 0.3);
					return c;
				}
					ENDCG
		}
		}
}
