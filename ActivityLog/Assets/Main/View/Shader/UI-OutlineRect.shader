// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

// シェーダ入門 https://qiita.com/kajitaj63b3/items/1c836a07b9164755b8a7
// 座標変換 https://blog.natade.net/2017/06/03/rendering-3dcg-coordinate/
// 座標変換計算 https://matcha-choco010.net/2018/08/30/mvp行列による座標変換について
// アトリビュート https://qiita.com/luckin/items/96f0ce9e1ac86f9b51fc

Shader "UI/OutlineRect"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        [HideInInspector] _StencilComp ("Stencil Comparison", Float) = 8
        [HideInInspector] _Stencil ("Stencil ID", Float) = 0
        [HideInInspector] _StencilOp ("Stencil Operation", Float) = 0
        [HideInInspector] _StencilWriteMask ("Stencil Write Mask", Float) = 255
        [HideInInspector] _StencilReadMask ("Stencil Read Mask", Float) = 255

        [HideInInspector] _ColorMask ("Color Mask", Float) = 15

        _ObjSizeX ("Panel Object SizeX", Float) = 0
        _ObjSizeY ("Panel Object SizeY", Float) = 0
        _OutlineThickness ("Outline Thickness", Float) = 0
        _Offcut ("Offcut", Float) = 0

        // alphaは反映されない
        _BaseColor255 ("Base Color", Vector) = (255,255,255,255)
        // alphaは反映されない
        _OutlineColor255 ("Outline Color", Vector) = (0,0,0,255)

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        // カリング
        // off  : ポリゴンの表/裏とも描画
        // back : 裏は描画しない
        // front: 表は描画しない
        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        // 透明度のある部分の背景色との混ぜ方
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            Name "Default"
        CGPROGRAM   // ここからがシェーダプログラム本体(ここまではUnityの機能)
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"
            #include "UILib.cginc"  // 自作UI用関数など

            #pragma multi_compile __ UNITY_UI_CLIP_RECT
            #pragma multi_compile __ UNITY_UI_ALPHACLIP


            // 頂点シェーダへの入力
            // 頂点座標(POSITION)とテクスチャUV座標(TEXCOORD0)は必須
            struct appdata_t
            {
                /* セマンティクス   意味	            主な型
                * POSITION	        頂点のローカル座標  float3/4
                * NORMAL	        頂点の法線          float3
                * TEXCOORD0	        頂点のUV座標	    float2/3/4
                * TEXCOORD1/2/3	    2,3,4番目のUV座標	float2/3/4
                * TANGENT	        接線	            float4
                * COLOR	            頂点の色	        float4
                */
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            // フラグメントシェーダへの入力
            // 頂点座標(POSITION)とテクスチャUV座標(TEXCOORD0)は必須
            struct v2f
            {
                /* セマンティクス   意味	                    主な型
                * SV_POSITION	    頂点のクリップ座標(必須)	float4(必ず)
                * TEXCOORD0/1/...	テクスチャ座標/位置/方向等	float2/3, half3
                * COLOR0/1/...	    色など	                    fixed4
                */
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord  : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            // 変数定義
            // 上記のプロパティをシェーダで扱えるように再定義する
            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;
            float4 _MainTex_ST;

            float _ObjSizeX;
            float _ObjSizeY;
            float _OutlineThickness;
            float _Offcut;

            fixed4 _BaseColor255;
            fixed4 _OutlineColor255;


            // 頂点シェーダ
            // 頂点の座標変換
            // 頂点の数だけ実行される
            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.worldPosition = v.vertex;

                //-- オブジェクト座標(ローカル座標)
                //｜      オブジェクト内のある原点からの相対座標
                //｜ ↓ [ワールド変換:Model行列]
                //Ｍ ワールド座標
                //∨      オブジェクトの原点がワールド上でどこにあるかを考慮
                //Ｐ ↓ [ビュー変換:View行列]
                //変 ビュー座標(カメラ座標)
                //換      カメラを原点とした座標
                //↓ ↓ [透視/正射影変換:Projection行列]
                //-- クリップ座標
                //        遠近を考慮した座標

                // オブジェクト座標からクリップ座標まで一発変換
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

                OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

                OUT.color = v.color * _Color;
                return OUT;
            }

            // フラグメントシェーダ(ピクセルシェーダ)
            // ピクセルの色を決定
            // ピクセルの数だけ実行される
            fixed4 frag(v2f IN) : SV_Target
            {

				float2 texuv = IN.texcoord;
                half4 backColor = _BaseColor255 / 255;
                half4 outlineColor = _OutlineColor255 / 255;

                // d (= [0, 0.5]
                // オブジェクトの物理サイズ
                fixed2 objSize = fixed2(_ObjSizeX, _ObjSizeY);
                // 頂点から縁取りの内側の頂点までの距離[0,1]
                fixed2 indiameterUV = (_Offcut + _OutlineThickness) / objSize;
                // 頂点から縁取りの外側の頂点までの距離[0,1]
                fixed2 offcutUV = _Offcut / objSize;

                // 外辺から描画店までの距離
                float dx = min(texuv.x, 1-texuv.x);
                float dy = min(texuv.y, 1-texuv.y);

                // 縁取りより内側
                if (dx > indiameterUV.x && dy > indiameterUV.y) {
                    return backColor;
                }
                // 描画範囲内
                else if (dx > offcutUV.x && dy > offcutUV.y) {
                    return outlineColor;
                }
                // 描画範囲外
                else {
                    return 0;
                }

            }
        ENDCG
        }
    }
}
