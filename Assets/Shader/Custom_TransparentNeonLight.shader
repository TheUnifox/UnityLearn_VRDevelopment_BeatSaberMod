Shader "Custom/TransparentNeonLight" {
	Properties {
		_FogStartOffset ("Fog Start Offset", Float) = 1
		_FogScale ("Fog Scale", Float) = 1
		[Toggle(ENABLE_HEIGHT_FOG)] _EnableHeightFog ("Enable Height Fog", Float) = 1
		[ShowIfAny(ENABLE_HEIGHT_FOG)] _FogHeightScale ("Height Fog Scale", Float) = 1
		[ShowIfAny(ENABLE_HEIGHT_FOG)] _FogHeightOffset ("Height Fog Offset", Float) = 0
		[Space(12)] [ToggleHeader(ENABLE_WORLD_NOISE)] _EnableWorldNoise ("Enable World Noise", Float) = 0
		[ShowIfAny(ENABLE_WORLD_NOISE)] _WorldNoiseScale ("World Noise Scale", Float) = 1
		[ShowIfAny(ENABLE_WORLD_NOISE)] _WorldNoiseIntensityOffset ("World Intensity Offset", Float) = 0
		[ShowIfAny(ENABLE_WORLD_NOISE)] _WorldNoiseIntensityScale ("World Intenstity Scale", Float) = 1
		[ShowIfAny(ENABLE_WORLD_NOISE)] _WorldNoiseScrolling ("World Noise Scrolling", Vector) = (0,0,0,1)
		[Space(12)] [ToggleHeader(ENABLE_WORLD_SPACE_FADE)] _EnableWorldSpaceFade ("Enable World Space Fade", Float) = 0
		[ShowIfAny(ENABLE_WORLD_SPACE_FADE)] _WorldSpaceFadePos ("World Space Fade Position", Float) = 0
		[ShowIfAny(ENABLE_WORLD_SPACE_FADE)] _WorldSpaceFadeSlope ("World Space Fade Slope", Float) = 1
		[Space(12)] [ToggleHeader(SPECULAR)] _EnableSpecular ("Enable Specular", Float) = 1
		[ShowIfAny(SPECULAR)] _SpecularIntensity ("Specular Intensity", Float) = 1
		[ShowIfAny(SPECULAR)] _SpecularHardness ("Specular Hardness", Float) = 64
		[Space(12)] [ToggleHeader(NORMAL_MAP)] _EnableNormalMap ("Enable Normal Map", Float) = 0
		[ShowIfAny(NORMAL_MAP)] _NormalTexture ("Normal Texture", 2D) = "bump" {}
		[ShowIfAny(NORMAL_MAP)] _NormalScale ("Normal Scale", Float) = 1
		[Header(Probe Reflection)] [Space(8)] [ToggleShowIfAny(REFLECTION_PROBE)] _EnableReflectionProbe ("Enable Reflection Probe", Float) = 0
		[ShowIfAny(REFLECTION_PROBE)] _Smoothness ("Smoothness", Range(0, 1)) = 1
		[ShowIfAny(REFLECTION_PROBE)] _ReflectionIntensity ("Probe Intensity", Float) = 1
		[ShowIfAny(REFLECTION_PROBE)] _GlassOpacity ("Glass Opacity", Float) = 1
		[Space(8)] [ToggleShowIfAny(ENABLE_RIM_DIM, REFLECTION_PROBE)] _EnableRimDim ("Enable Rim Dim", Float) = 0
		[ShowIfAny(ENABLE_RIM_DIM)] _RimScale ("Rim Scale", Float) = 1
		[ShowIfAny(ENABLE_RIM_DIM)] _RimOffset ("Rim Offset", Float) = 1
		[ShowIfAny(ENABLE_RIM_DIM)] _RimCameraDistanceOffset ("Rim Camera Distance Offset", Float) = 2
		[ShowIfAny(ENABLE_RIM_DIM)] _RimCameraDistanceScale ("Rim Camera Distance Scale", Float) = 0.3
		[ToggleShowIfAny(INVERT_RIM_DIM, ENABLE_RIM_DIM)] _InvertRimDim ("Invert Rim Dim", Float) = 0
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendSrcFactor ("Blend Src", Float) = 1
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendDstFactor ("Blend Dst", Float) = 1
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendSrcFactorA ("Blend Src Factor A", Float) = 1
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendDstFactorA ("Blend Dst Factor A", Float) = 1
		[Space] [Enum(UnityEngine.Rendering.CullMode)] _CullMode ("CullMode", Float) = 0
		[Space] _StencilRefValue ("Stencil Ref Value", Float) = 0
		[Enum(UnityEngine.Rendering.CompareFunction)] _StencilComp ("Stencil Comp Func", Float) = 8
		[Enum(UnityEngine.Rendering.StencilOp)] _StencilPass ("Stencill Pass Op", Float) = 0
		[Space] [Enum(UnityEngine.Rendering.BlendOp)] _BlendOp ("Blend Operation", Float) = 0
	}
	SubShader {
		Tags { "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		Pass {
			Tags { "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			Blend Zero Zero, Zero Zero
			ZWrite Off
			Cull Off
			Stencil {
				Comp Always
				Pass Keep
				Fail Keep
				ZFail Keep
			}
			GpuProgramID 57656
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float3 texcoord1 : TEXCOORD1;
				float texcoord3 : TEXCOORD3;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			// $Globals ConstantBuffers for Fragment Shader
			float _BaseColorBoost;
			float _BaseColorBoostThreshold;
			// Custom ConstantBuffers for Vertex Shader
			CBUFFER_START(Props)
				float _AlphaStart;
				float _AlphaEnd;
				float _StartWidth;
				float _EndWidth;
			CBUFFER_END
			// Custom ConstantBuffers for Fragment Shader
			CBUFFER_START(Props)
				float4 _Color;
			CBUFFER_END
			// Texture params for Vertex Shader
			// Texture params for Fragment Shader
			
			// Keywords: 
			v2f vert(appdata_full v)
			{
                v2f o;
                float4 tmp0;
                float4 tmp1;
                tmp0 = v.vertex.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp1.x = v.vertex.y > 0.5;
                tmp1.y = tmp1.x ? _EndWidth : _StartWidth;
                o.texcoord3.x = tmp1.x ? _AlphaEnd : _AlphaStart;
                tmp1.xy = tmp1.yy * v.vertex.xz;
                tmp0 = unity_ObjectToWorld._m00_m10_m20_m30 * tmp1.xxxx + tmp0;
                tmp0 = unity_ObjectToWorld._m02_m12_m22_m32 * tmp1.yyyy + tmp0;
                tmp1 = tmp0 + unity_ObjectToWorld._m03_m13_m23_m33;
                o.texcoord1.xyz = unity_ObjectToWorld._m03_m13_m23 * v.vertex.www + tmp0.xyz;
                tmp0 = tmp1.yyyy * unity_MatrixVP._m01_m11_m21_m31;
                tmp0 = unity_MatrixVP._m00_m10_m20_m30 * tmp1.xxxx + tmp0;
                tmp0 = unity_MatrixVP._m02_m12_m22_m32 * tmp1.zzzz + tmp0;
                o.position = unity_MatrixVP._m03_m13_m23_m33 * tmp1.wwww + tmp0;
                return o;
			}
			// Keywords: 
			fout frag(v2f inp, float facing: VFACE)
			{
                fout o;
                float4 tmp0;
                tmp0.x = inp.texcoord3.x * inp.texcoord3.x;
                tmp0.x = tmp0.x * inp.texcoord3.x;
                tmp0.x = tmp0.x * _Color.w;
                tmp0.x = tmp0.x * tmp0.x;
                tmp0.y = tmp0.x * tmp0.x;
                tmp0.y = tmp0.y * _BaseColorBoost + -_BaseColorBoostThreshold;
                o.sv_target.xyz = saturate(_Color.xyz * tmp0.xxx + tmp0.yyy);
                o.sv_target.w = tmp0.x;
                return o;
			}
			ENDCG
		}
	}
}