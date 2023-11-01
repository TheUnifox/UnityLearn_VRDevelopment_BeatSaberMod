Shader "Custom/ParametricBoxFrameLW" {
	Properties {
		_FogStartOffset ("Fog Start Offset", Float) = 1
		_FogScale ("Fog Scale", Float) = 1
		[ToggleHeader(HEIGHT_FOG)] _EnableHeightFog ("Enable Height Fog", Float) = 0
		[ShowIfAny(HEIGHT_FOG)] _FogHeightScale ("Fog Height Scale", Float) = 1
		[ShowIfAny(HEIGHT_FOG)] _FogHeightOffset ("Fog Height Offset", Float) = 0
		[Space] [Enum(UnityEngine.Rendering.CullMode)] _CullMode ("CullMode", Float) = 0
		[Space] [KeywordEnum(None, MainEffect, Always)] _WhiteBoostType ("White Boost", Float) = 1
		[Space(12)] [ToggleHeader(FAKE_MIRROR_TRANSPARENCY)] _FakeMirrorTransparencyEnabled ("Fake Mirror Transparency", Float) = 0
		[ShowIfAny(FAKE_MIRROR_TRANSPARENCY)] _FakeMirrorTransparencyMultiplier ("Mirror Transparency Multiplier", Float) = 1
		[Space] [Toggle(ENABLE_CUTOUT)] _EnableCutout ("Enable Cutout", Float) = 0
		_CutoutTexScale ("Cutout Texture Scale", Float) = 1
		[Space] [Toggle(ENABLE_CLIPPING)] _EnableClipping ("Enable Clipping", Float) = 0
		[Space] [Enum(UnityEngine.Rendering.CompareFunction)] _ZTest ("ZTest", Float) = 4
		[Toggle] _ZWrite ("Z Write", Float) = 1
		[Space(16)] _StencilRefValue ("Stencil Ref Value", Float) = 0
		[Enum(UnityEngine.Rendering.CompareFunction)] _StencilComp ("Stencil Comp Func", Float) = 8
		[Enum(UnityEngine.Rendering.StencilOp)] _StencilPass ("Stencill Pass Op", Float) = 0
		[Space(16)] [Enum(UnityEngine.Rendering.BlendMode)] _BlendSrcFactor ("Blend Src", Float) = 1
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendDstFactor ("Blend Dst", Float) = 0
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendSrcFactorA ("Blend Src Factor A", Float) = 0
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendDstFactorA ("Blend Dst Factor A", Float) = 0
	}
	SubShader {
		Tags { "DisableBatching" = "true" "QUEUE" = "Geometry" "RenderType" = "Opaque" }
		Pass {
			Tags { "DisableBatching" = "true" "QUEUE" = "Geometry" "RenderType" = "Opaque" }
			Blend Zero Zero, Zero Zero
			ZWrite Off
			Cull Off
			Stencil {
				Comp Always
				Pass Keep
				Fail Keep
				ZFail Keep
			}
			GpuProgramID 23260
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float4 texcoord2 : TEXCOORD2;
				float3 texcoord3 : TEXCOORD3;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float _StereoCameraEyeOffset;
			float2 _CustomFogTextureToScreenRatio;
			// $Globals ConstantBuffers for Fragment Shader
			// Custom ConstantBuffers for Vertex Shader
			CBUFFER_START(Props)
				float4 _SizeParams;
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
                tmp0.xyz = v.vertex.xyz > float3(0.0, 0.0, 0.0);
                tmp1.xyz = v.vertex.xyz < float3(0.0, 0.0, 0.0);
                tmp0.xyz = tmp1.xyz - tmp0.xyz;
                tmp0.xyz = floor(tmp0.xyz);
                tmp1.xyz = v.vertex.xyz - tmp0.xyz;
                tmp1.xyz = tmp1.xyz + tmp1.xyz;
                tmp1.xyz = tmp1.xyz * _SizeParams.www;
                tmp1.xyz = tmp1.xyz / _SizeParams.xyz;
                tmp0.xyz = tmp0.xyz + tmp1.xyz;
                tmp1 = tmp0.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp1 = unity_ObjectToWorld._m00_m10_m20_m30 * tmp0.xxxx + tmp1;
                tmp0 = unity_ObjectToWorld._m02_m12_m22_m32 * tmp0.zzzz + tmp1;
                tmp1 = tmp0 + unity_ObjectToWorld._m03_m13_m23_m33;
                o.texcoord3.xyz = unity_ObjectToWorld._m03_m13_m23 * v.vertex.www + tmp0.xyz;
                tmp0 = tmp1.yyyy * unity_MatrixVP._m01_m11_m21_m31;
                tmp0 = unity_MatrixVP._m00_m10_m20_m30 * tmp1.xxxx + tmp0;
                tmp0 = unity_MatrixVP._m02_m12_m22_m32 * tmp1.zzzz + tmp0;
                tmp0 = unity_MatrixVP._m03_m13_m23_m33 * tmp1.wwww + tmp0;
                o.position = tmp0;
                tmp1.xyw = tmp0.yxw * float3(0.5, 0.5, 0.5);
                tmp1.z = tmp1.x * _ProjectionParams.x;
                tmp1.yz = tmp0.ww * float2(0.5, 0.5) + tmp1.yz;
                tmp0.x = floor(unity_StereoEyeIndex);
                tmp0.y = _StereoCameraEyeOffset + _StereoCameraEyeOffset;
                tmp0.x = tmp0.x * tmp0.y + -_StereoCameraEyeOffset;
                tmp1.x = tmp0.w * tmp0.x + tmp1.y;
                tmp0.xy = -tmp0.ww * float2(0.5, 0.5) + tmp1.xz;
                o.texcoord2.zw = tmp0.zw;
                o.texcoord2.xy = tmp0.xy * _CustomFogTextureToScreenRatio + tmp1.ww;
                return o;
			}
			// Keywords: 
			fout frag(v2f inp)
			{
                fout o;
                o.sv_target = _Color;
                return o;
			}
			ENDCG
		}
	}
}