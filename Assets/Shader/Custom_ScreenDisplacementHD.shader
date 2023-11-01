Shader "Custom/ScreenDisplacementHD" {
	Properties {
		_MainTex ("Displacement Texture", 2D) = "white" {}
		_DisplacementStrength ("Displacement Strength", Float) = 0.01
		[Space] [Enum(UnityEngine.Rendering.CullMode)] _CullMode ("CullMode", Float) = 0
		[Space] [Toggle(ENABLE_INSTANCED_COLOR)] _EnableInstancedColor ("Enable Instanced Color", Float) = 0
		_TintColor ("Tint Color", Vector) = (1,1,1,1)
		_AddColor ("Add Color", Vector) = (0,0,0,0)
		_DisplacementAlphaMul ("Displacement Alpha Mul", Float) = 1
		[Space] [Toggle(SCALE_UV)] _ScaleUV ("Scale UV", Float) = 0
		_UVScale ("UV Scale", Vector) = (1,1,1,0)
		[Space] [Toggle(SCROLL_UV)] _ScrollUV ("Scroll UV", Float) = 0
		_ScrollUVVelocity ("Scroll UV Velocity", Vector) = (0,0,0,0)
		[Space] [Toggle(ENABLE_FOG)] _EnableFog ("Enable Fog", Float) = 0
		[ShowIfAny(ENABLE_FOG)] _FogStartOffset ("Fog Start Offset", Float) = 0
		[ShowIfAny(ENABLE_FOG)] _FogScale ("Fog Scale", Float) = 1
		[ShowIfAny(ENABLE_FOG)] _FogHeightScale ("Fog Height Scale", Float) = 1
		[ShowIfAny(ENABLE_FOG)] _FogHeightOffset ("Fog Height Offset", Float) = 0
		[Space] [Toggle(ZWRITE)] _ZWrite ("Z Write", Float) = 0
		[Space] [Toggle(CLIP_LOW_ALPHA)] _ClipLowAlpha ("Clip Low Alpha", Float) = 1
		[Space] [Toggle(VIEW_ANGLE_AFFECTS_DISTORTION)] _ViewAngleAffectsDistortion ("View Angle Affects Distortion", Float) = 1
		_ViewAngleDistortionParam ("View Angle Distortion Param", Float) = 4
		[Space] [Toggle(USE_DISTORTED_TEXTURE_ONLY)] _UseDistortedTextureOnly ("Use Distorted Texture Only", Float) = 0
		[Space] [Toggle(USE_MONO_SCREEN_TEXTURE)] _UseMonoScreenTexture ("Use Mono Screen Texture", Float) = 0
		[Space] [Toggle(DEPTH_AWARE_DISTORTION)] _DepthAwareDistortion ("Depth Aware Distortion", Float) = 0
		[Space] [Toggle(ENABLE_CUTOUT)] _EnableCutout ("Enable Cutout", Float) = 0
		_CutoutTexScale ("Cutout Texture Scale", Float) = 1
		[PerRendererData] _CutoutTexOffset ("Cutout Texture Offset", Vector) = (0,0,0,0)
		[PerRendererData] _Cutout ("Cutout", Range(0, 1)) = 0
		[Space] [Toggle(ENABLE_RIM_DIM)] _EnableRimDim ("Enable Rim Dim", Float) = 0
		_RimDimScale ("Rim Scale", Float) = 1
		_RimDimOffset ("Rim Offset", Float) = 1
		[Space] [Toggle(ENABLE_CLIPPING)] _EnableClipping ("Enable Clipping", Float) = 0
	}
	SubShader {
		Tags { "DisableBatching" = "true" "IGNOREPROJECTOR" = "true" "PreviewType" = "Plane" "QUEUE" = "Transparent+1" "RenderType" = "Transparent" }
		Pass {
			Tags { "DisableBatching" = "true" "IGNOREPROJECTOR" = "true" "PreviewType" = "Plane" "QUEUE" = "Transparent+1" "RenderType" = "Transparent" }
			ZWrite Off
			Cull Off
			GpuProgramID 42532
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float2 texcoord : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
				float3 texcoord3 : TEXCOORD3;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float4 _MainTex_ST;
			// $Globals ConstantBuffers for Fragment Shader
			float _DisplacementStrength;
			float _DisplacementAlphaMul;
			float4 _TintColor;
			float4 _AddColor;
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			// Texture params for Vertex Shader
			// Texture params for Fragment Shader
			sampler2D _MainTex;
			sampler2D _GrabTexture1;
			
			// Keywords: 
			v2f vert(appdata_full v)
			{
                v2f o;
                float4 tmp0;
                float4 tmp1;
                tmp0 = v.vertex.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp0 = unity_ObjectToWorld._m00_m10_m20_m30 * v.vertex.xxxx + tmp0;
                tmp0 = unity_ObjectToWorld._m02_m12_m22_m32 * v.vertex.zzzz + tmp0;
                tmp1 = tmp0 + unity_ObjectToWorld._m03_m13_m23_m33;
                o.texcoord3.xyz = unity_ObjectToWorld._m03_m13_m23 * v.vertex.www + tmp0.xyz;
                tmp0 = tmp1.yyyy * unity_MatrixVP._m01_m11_m21_m31;
                tmp0 = unity_MatrixVP._m00_m10_m20_m30 * tmp1.xxxx + tmp0;
                tmp0 = unity_MatrixVP._m02_m12_m22_m32 * tmp1.zzzz + tmp0;
                tmp0 = unity_MatrixVP._m03_m13_m23_m33 * tmp1.wwww + tmp0;
                o.position = tmp0;
                o.texcoord.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
                o.texcoord1 = v.color;
                tmp1.xyz = tmp0.xwy * float3(0.5, 0.5, -0.5);
                o.texcoord2.zw = tmp0.zw;
                o.texcoord2.xy = tmp1.yy + tmp1.xz;
                return o;
			}
			// Keywords: 
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                float4 tmp1;
                float4 tmp2;
                tmp0 = tex2D(_MainTex, inp.texcoord.xy);
                tmp0.xy = tmp0.xy - float2(0.5, 0.5);
                tmp0.xy = tmp0.xy * _DisplacementStrength.xx;
                tmp0.xy = tmp0.xy * float2(0.001, 0.001);
                tmp0.z = inp.texcoord1.w * _TintColor.w;
                tmp0.xy = tmp0.xy * tmp0.zz + inp.texcoord2.xy;
                tmp0.z = tmp0.z * 1.01 + -0.01;
                tmp0.xy = tmp0.xy / inp.texcoord2.ww;
                tmp1 = tex2D(_GrabTexture1, tmp0.xy);
                tmp1 = tmp1 * _TintColor + _AddColor;
                tmp0.xy = inp.texcoord2.xy / inp.texcoord2.ww;
                tmp2 = tex2D(_GrabTexture1, tmp0.xy);
                tmp1 = tmp1 - tmp2;
                tmp0 = tmp0.zzzz * tmp1 + tmp2;
                o.sv_target.w = tmp0.w * _DisplacementAlphaMul;
                o.sv_target.xyz = tmp0.xyz;
                return o;
			}
			ENDCG
		}
	}
}