Shader "Custom/Rain" {
	Properties {
		[Header(Rain)] [Space(10)] _Height ("Height", Float) = 10
		_Speed ("Speed", Float) = 1
		[Space(10)] _BottomFadeScale ("Bottom Fade Scale", Float) = 1
		_TopFadeScale ("Top Fade Scale", Float) = 1
		_BottomEnd ("Bottom End", Float) = 1
		_TopEnd ("Top End", Float) = 1
		[Space(20)] _Color ("Color", Vector) = (1,1,1,1)
		[Toggle(COLOR_GRADIENT)] _EnableColorGradient ("Use Color Gradient", Float) = 0
		[ShowIfAny(COLOR_GRADIENT)] _ColorGradient ("Gradient LUT", 2D) = "white" {}
		[Space] _MainTex ("Main Texture", 2D) = "black" {}
		[Toggle(TEXTURE_COLOR)] _EnableTextureColor ("Use Texture Color", Float) = 0
		[KeywordEnum(Alpha, Red)] _AlphaChannel ("Alpha Channel", Float) = 0
		_Intensity ("Intensity", Float) = 1
		_UvPanning ("UV Panning", Vector) = (0,0,0,0)
		[Space(12)] [ToggleHeader(MASK)] _EnableMask ("Use Mask", Float) = 0
		[ToggleShowIfAny(MASK_ADDITIVE, MASK)] _MaskAdditive ("Blend Additively", Float) = 0
		[ToggleShowIfAny(MASK_RED_IS_ALPHA, MASK)] _MaskRedIsAlpha ("Red is Mask Alpha", Float) = 0
		[ShowIfAny(MASK)] _MaskTex ("Mask Texture", 2D) = "white" {}
		[ShowIfAny(MASK)] _MaskPanning ("Mask Panning", Vector) = (0,0,0,0)
		[ShowIfAny(MASK)] _MaskStrength ("Mask Strength", Float) = 1
		[Space(12)] [ToggleHeader(MASK2)] _EnableMask2 ("Use Secondary Mask", Float) = 0
		[ToggleShowIfAny(MASK2_RED_IS_ALPHA, MASK2)] _Mask2RedIsAlpha ("Red is Mask Alpha", Float) = 0
		[ShowIfAny(MASK2)] _Mask2Tex ("Secondary Mask Texture", 2D) = "white" {}
		[ShowIfAny(MASK2)] _Mask2Panning ("Secondary Mask Panning", Vector) = (0,0,0,0)
		[ShowIfAny(MASK2)] _Mask2MinValue ("Min Mask Value", Float) = 0
		[Space(12)] [ToggleHeader(SOFT_PARTICLES)] _EnableSoftParticles ("Soft Particles", Float) = 0
		[ShowIfAny(SOFT_PARTICLES)] _SoftFactor ("Soft Factor", Range(0, 50)) = 0
		[Space(12)] [ToggleHeader(CLOSE_TO_CAMERA_DISAPPEAR)] _EnableCloseToCameraDisappear ("Close to Camera Dissapear", Float) = 0
		[ShowIfAny(CLOSE_TO_CAMERA_DISAPPEAR)] _CloseToCameraOffset ("Close to Camera Offset", Float) = 0.5
		[ShowIfAny(CLOSE_TO_CAMERA_DISAPPEAR)] _CloseToCameraFactor ("Close to Camera Factor", Float) = 0.5
		[Space(12)] [ToggleHeader(VIEW_ALIGN_DISAPPEAR)] _EnableViewAlignDisappear ("View Align Dissapear", Float) = 0
		[ShowIfAny(VIEW_ALIGN_DISAPPEAR)] _ViewAlignFactor ("View Align Factor", Float) = 1.5
		[Space(12)] [ToggleHeader(VERTEX_COLOR)] _EnableVertexColor ("Enable Vertex Color", Float) = 0
		[ToggleShowIfAny(VERTEX_SQUARE_ALPHA, VERTEX_COLOR)] _SquareVertexAlpha ("Square Vertex Alpha", Float) = 0
		[ToggleShowIfAny(VERTEX_RED_IS_ALPHA, VERTEX_COLOR)] _RedIsVertexAlpha ("Red is Vertex Alpha", Float) = 0
		[EnumShowIf(RGBA, A, RGB, VERTEX_COLOR)] _VertexChannels ("Vertex Channels", Float) = 0
		[ToggleShowIfAny(LIFETIME, VERTEX_COLOR)] _EnableLifetime ("Enable Lifetime Alpha", Float) = 0
		[Space(12)] [EnumHeader(None, Alpha, Color, Lerp)] _FogType ("Fog Type", Float) = 0
		[ShowIfAny(_FOGTYPE_ALPHA, _FOGTYPE_LERP)] _FogStartOffset ("Fog Start Offset", Float) = 0
		[ShowIfAny(_FOGTYPE_ALPHA, _FOGTYPE_LERP)] _FogScale ("Fog Scale", Range(0, 4)) = 1
		[ShowIfAny(_FOGTYPE_COLOR)] _AlphaFromFog ("Alpha from Fog", Range(0, 1)) = 0.5
		[ToggleShowIfAny(HEIGHT_FOG, _FOGTYPE_ALPHA, _FOGTYPE_LERP)] _EnableHeightFog ("Enable Height Fog", Float) = 0
		[ToggleShowIfAny(PRECISE_FOG, _FOGTYPE_ALPHA, _FOGTYPE_LERP)] _PreciseFog ("High (Frag) Precision", Float) = 0
		[Space(12)] [ToggleHeader(HOLOGRAM)] _EnableHologram ("Hologram effect", Float) = 0
		[ShowIfAny(HOLOGRAM)] _HologramColor ("Hologram Color", Vector) = (1,1,1,1)
		[Space()] [Header(Other)] [Space] [Toggle(SQUARE_ALPHA)] _SquareAlpha ("Square Alpha", Float) = 1
		_AlphaMultiplier ("Alpha Multiplier", Float) = 1
		[KeywordEnum(None, MainEffect, Always)] _WhiteBoostType ("White Boost", Float) = 0
		[Toggle(NOISE_DITHERING)] _EnableNoiseDithering ("Noise Dithering", Float) = 0
		[Space] [Space(12)] [Header(Settings)] [Space] [Enum(UnityEngine.Rendering.BlendMode)] _BlendSrcFactor ("Blend Src", Float) = 0
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendDstFactor ("Blend Dst", Float) = 0
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendSrcFactorA ("Blend Src Factor A", Float) = 0
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendDstFactorA ("Blend Dst Factor A", Float) = 10
		[Enum(UnityEngine.Rendering.BlendOp)] _BlendOp ("Blend Operation", Float) = 0
		[Space] [Enum(UnityEngine.Rendering.CullMode)] _CullMode ("Cull Mode", Float) = 0
		[Space] [Enum(UnityEngine.Rendering.CompareFunction)] _ZTest ("ZTest", Float) = 4
		[Space] _OffsetFactor ("Offset Factor", Float) = 0
		_OffsetUnits ("Offset Units", Float) = 0
		[Space] _StencilRefValue ("Stencil Ref Value", Float) = 0
		[Enum(UnityEngine.Rendering.CompareFunction)] _StencilComp ("Stencil Comp Func", Float) = 8
		[Enum(UnityEngine.Rendering.StencilOp)] _StencilPass ("Stencill Pass Op", Float) = 0
	}
	SubShader {
		Tags { "CanUseSpriteAtlas" = "true" "IGNOREPROJECTOR" = "true" "PreviewType" = "Plane" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		Pass {
			Tags { "CanUseSpriteAtlas" = "true" "IGNOREPROJECTOR" = "true" "PreviewType" = "Plane" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			Blend Zero Zero, Zero Zero
			ZWrite Off
			Cull Off
			Stencil {
				Comp Always
				Pass Keep
				Fail Keep
				ZFail Keep
			}
			GpuProgramID 16723
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float2 texcoord : TEXCOORD0;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float4 _MainTex_ST;
			float2 _UvPanning;
			float _Height;
			float _Speed;
			// $Globals ConstantBuffers for Fragment Shader
			float _Intensity;
			float _AlphaMultiplier;
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			CBUFFER_START(Props)
				float4 _Color;
			CBUFFER_END
			// Texture params for Vertex Shader
			// Texture params for Fragment Shader
			sampler2D _MainTex;
			
			// Keywords: _VERTEXCHANNELS_RGBA
			v2f vert(appdata_full v)
			{
                v2f o;
                float4 tmp0;
                float4 tmp1;
                tmp0.x = _Time.x * -_Speed + v.color.x;
                tmp0.x = frac(tmp0.x);
                tmp0.x = tmp0.x * _Height;
                tmp0.xyz = tmp0.xxx * -v.normal.xyz + v.vertex.xyz;
                tmp1 = tmp0.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp1 = unity_ObjectToWorld._m00_m10_m20_m30 * tmp0.xxxx + tmp1;
                tmp0 = unity_ObjectToWorld._m02_m12_m22_m32 * tmp0.zzzz + tmp1;
                tmp0 = tmp0 + unity_ObjectToWorld._m03_m13_m23_m33;
                tmp1 = tmp0.yyyy * unity_MatrixVP._m01_m11_m21_m31;
                tmp1 = unity_MatrixVP._m00_m10_m20_m30 * tmp0.xxxx + tmp1;
                tmp1 = unity_MatrixVP._m02_m12_m22_m32 * tmp0.zzzz + tmp1;
                o.position = unity_MatrixVP._m03_m13_m23_m33 * tmp0.wwww + tmp1;
                tmp0.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
                tmp0.zw = _UvPanning * _Time.yy;
                tmp0.xy = tmp0.zw * _MainTex_ST.xy + tmp0.xy;
                o.texcoord.xy = tmp0.xy + v.texcoord.zz;
                return o;
			}
			// Keywords: _VERTEXCHANNELS_RGBA
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                tmp0 = tex2D(_MainTex, inp.texcoord.xy);
                tmp0.x = _Intensity;
                tmp0 = tmp0.xxxw * _Color;
                tmp0.w = tmp0.w * _AlphaMultiplier;
                o.sv_target.xyz = tmp0.www * tmp0.xyz;
                o.sv_target.w = tmp0.w;
                return o;
			}
			ENDCG
		}
	}
}