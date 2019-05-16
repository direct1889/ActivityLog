
Shader "Unlit/GrabPass"
{
	Properties{
		_MainTex("Texture",2D)="white"{}
		_UVx("UVx",float) = 0.5
		_UVy("UVy",float) = 0.5
		_Color("Color",Color) = (1,1,1,1)
		_MagnifyingPower("MagnifyingPower",float) = 1.2

	}
	SubShader
	{
		Tags{
			"RenderType"="Transparent"
            // 他のオブジェクトの描画が全て済んでから処理する
			"Queue"="Transparent"
		}
		GrabPass{}
		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "UILib.cginc"

			sampler2D _GrabTexture;

			sampler2D _MainTex;
			float4 _MainTex_ST;

			fixed _UVx;
			fixed _UVy;
			fixed4 _Color;
			fixed _MagnifyingPower;


			struct appdata
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
                float4 screenPos : TEXCOORD1;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
                o.screenPos = ComputeGrabScreenPos(o.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);

				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
                // 描画点のUV座標
                float2 grabUV = (i.screenPos.xy / i.screenPos.w);
                // パネルのUV座標(左上が原点)
                float2 worldUV = float2(_UVx, _UVy);
                // 描画内容を持つ点
                float2 drawUV = (grabUV - worldUV) / _MagnifyingPower + worldUV;
                return tex2D(_GrabTexture, drawUV) * _Color;
			}
			ENDCG
		}
	}
}