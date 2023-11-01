Shader "Custom/NoteHD" {
	Properties {
		_Smoothness ("Smoothness", Range(0, 1)) = 1
		_NoteSize ("NoteSize", Float) = 0.25
		[Space] [Toggle(ENABLE_COLOR_INSTANCING)] _EnableColorInstancing ("Color Instancing", Float) = 0
		_SimpleColor ("Color", Vector) = (0,0,0,0)
		_FinalColorMul ("Color Multiplier", Float) = 1
		[Space(12)] [ToggleHeader(REFLECTION_MAP)] _EnableReflectionMap ("Enable Reflection Map", Float) = 1
		[ShowIfAny(REFLECTION_MAP)] _EnvironmentReflectionCube ("Environment Reflection", Cube) = "" {}
		[Toggle(REFLECTION_PROBE)] _EnableReflectionProbe ("Enable Reflection Probe", Float) = 0
		[ShowIfAny(REFLECTION_PROBE)] _ReflectionProbeIntensity ("Probe Intensity", Float) = 1
		[Space(12)] [EnumHeader(None, Alpha, Color, Lerp)] _FogType ("Fog Type", Float) = 0
		[ShowIfAny(_FOGTYPE_ALPHA, _FOGTYPE_LERP, _FOGTYPE_COLOR)] _FogStartOffset ("Fog Start Offset", Float) = 0
		[ShowIfAny(_FOGTYPE_ALPHA, _FOGTYPE_LERP, _FOGTYPE_COLOR)] _FogScale ("Fog Scale", Float) = 1
		[ToggleShowIfAny(HEIGHT_FOG, _FOGTYPE_ALPHA, _FOGTYPE_LERP, _FOGTYPE_COLOR)] _EnableHeightFog ("Enable Height Fog", Float) = 0
		[ShowIfAny(HEIGHT_FOG)] _FogHeightScale ("Fog Height Scale", Float) = 1
		[ShowIfAny(HEIGHT_FOG)] _FogHeightOffset ("Fog Height Offset", Float) = 0
		[ToggleShowIfAny(PRECISE_FOG, _FOGTYPE_ALPHA, _FOGTYPE_LERP,  _FOGTYPE_COLOR)] _PreciseFog ("High (Frag) Precision", Float) = 0
		[Space(12)] [ToggleHeader(ENABLE_CUTOUT)] _EnableCutout ("Enable Cutout", Float) = 0
		[ShowIfAny(ENABLE_CUTOUT)] _CutoutTexScale ("Cutout Texture Scale", Float) = 1
		[ToggleShowIfAny(ENABLE_CLOSE_TO_CAMERA_CUTOUT, ENABLE_CUTOUT)] _EnableCloseToCameraCutout ("Enable Close to Camera Cutout", Float) = 0
		[ShowIfAny(ENABLE_CUTOUT)] _CloseToCameraCutoutOffset ("Close to Camera Cutout Offset", Float) = 0.5
		[ShowIfAny(ENABLE_CUTOUT)] _CloseToCameraCutoutScale ("Close to Camera Cutout Scale", Float) = 0.5
		[Space(12)] [ToggleHeader(ENABLE_PLANE_CUT)] _EnablePlaneCut ("Enable Plane Cut", Float) = 0
		[ShowIfAny(ENABLE_PLANE_CUT)] _CutPlaneEdgeGlowWidth ("Plane Edge Glow Width", Float) = 0.01
		[PerRendererData] _CutPlane ("Cut Plane", Vector) = (1,0,0,0)
		[Space(12)] [ToggleHeader(ENABLE_RIM_DIM)] _EnableRimDim ("Enable Rim Dim", Float) = 0
		[ShowIfAny(ENABLE_RIM_DIM)] _RimScale ("Rim Scale", Float) = 1
		[ShowIfAny(ENABLE_RIM_DIM)] _RimOffset ("Rim Offset", Float) = 1
		[ShowIfAny(ENABLE_RIM_DIM)] _RimCameraDistanceOffset ("Rim Camera Distance Offset", Float) = 2
		[ShowIfAny(ENABLE_RIM_DIM)] _RimCameraDistanceScale ("Rim Camera Distance Scale", Float) = 0.3
		[ShowIfAny(ENABLE_RIM_DIM)] _RimDarkening ("Rim Darkening", Float) = 0
		[Space(12)] [KeywordEnum(None, MainEffect, Always)] _WhiteBoostType ("White Boost", Float) = 0
		[Space(16)] [Enum(UnityEngine.Rendering.CullMode)] _CullMode ("Cull Mode", Float) = 0
		[Space(16)] [Enum(UnityEngine.Rendering.BlendMode)] _BlendSrcFactor ("Blend Src", Float) = 1
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendDstFactor ("Blend Dst", Float) = 0
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendSrcFactorA ("Blend Src Factor A", Float) = 1
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendDstFactorA ("Blend Dst Factor A", Float) = 0
		[Space(12)] [Toggle(ZWRITE)] _ZWrite ("Z Write", Float) = 1
	}
	SubShader {
		Tags { "QUEUE" = "Geometry" "RenderType" = "Opaque" }
		Pass {
			Tags { "LIGHTMODE" = "FORWARDBASE" "QUEUE" = "Geometry" "RenderType" = "Opaque" }
			Blend Zero Zero, Zero Zero
			ZWrite Off
			Cull Off
			GpuProgramID 32469
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float4 texcoord : TEXCOORD0;
				float3 texcoord1 : TEXCOORD1;
				float texcoord2 : TEXCOORD2;
				float3 texcoord3 : TEXCOORD3;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float4x4 _SpawnRotation;
			// $Globals ConstantBuffers for Fragment Shader
			float _FinalColorMul;
			float4 _SimpleColor;
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			// Texture params for Vertex Shader
			// Texture params for Fragment Shader
			
			// Keywords: 
			v2f vert(appdata_full v)
			{
                v2f o;
                float4 tmp0;
                float4 tmp1;
                float4 tmp2;
                tmp0 = v.vertex.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp0 = unity_ObjectToWorld._m00_m10_m20_m30 * v.vertex.xxxx + tmp0;
                tmp0 = unity_ObjectToWorld._m02_m12_m22_m32 * v.vertex.zzzz + tmp0;
                tmp1 = tmp0 + unity_ObjectToWorld._m03_m13_m23_m33;
                tmp0.xyz = unity_ObjectToWorld._m03_m13_m23 * v.vertex.www + tmp0.xyz;
                tmp2 = tmp1.yyyy * unity_MatrixVP._m01_m11_m21_m31;
                tmp2 = unity_MatrixVP._m00_m10_m20_m30 * tmp1.xxxx + tmp2;
                tmp2 = unity_MatrixVP._m02_m12_m22_m32 * tmp1.zzzz + tmp2;
                o.position = unity_MatrixVP._m03_m13_m23_m33 * tmp1.wwww + tmp2;
                o.texcoord = v.vertex;
                tmp1.x = dot(v.normal.xyz, unity_WorldToObject._m00_m10_m20);
                tmp1.y = dot(v.normal.xyz, unity_WorldToObject._m01_m11_m21);
                tmp1.z = dot(v.normal.xyz, unity_WorldToObject._m02_m12_m22);
                tmp0.w = dot(tmp1.xyz, tmp1.xyz);
                tmp0.w = rsqrt(tmp0.w);
                tmp1.xyz = tmp0.www * tmp1.xyz;
                tmp2.xyz = _WorldSpaceCameraPos - tmp0.xyz;
                tmp0.w = dot(tmp2.xyz, tmp2.xyz);
                tmp0.w = rsqrt(tmp0.w);
                tmp2.xyz = tmp0.www * tmp2.xyz;
                tmp0.w = dot(-tmp2.xyz, tmp1.xyz);
                tmp0.w = tmp0.w + tmp0.w;
                tmp1.xyz = tmp1.xyz * -tmp0.www + -tmp2.xyz;
                tmp2.xyz = tmp1.yyy * _SpawnRotation._m01_m11_m21;
                tmp1.xyw = _SpawnRotation._m00_m10_m20 * tmp1.xxx + tmp2.xyz;
                o.texcoord1.xyz = _SpawnRotation._m02_m12_m22 * tmp1.zzz + tmp1.xyw;
                tmp1.xyz = tmp0.xyz - _WorldSpaceCameraPos;
                o.texcoord3.xyz = tmp0.xyz;
                tmp0.x = dot(tmp1.xyz, tmp1.xyz);
                o.texcoord2.x = sqrt(tmp0.x);
                return o;
			}
			// Keywords: 
			fout frag(v2f inp, float facing: VFACE)
			{
                fout o;
                float4 tmp0;
                tmp0.x = facing.x ? 1.0 : 0.2592593;
                tmp0.x = tmp0.x * _FinalColorMul;
                tmp0.xyz = tmp0.xxx * _SimpleColor.xyz;
                o.sv_target.xyz = tmp0.xyz * _SimpleColor.www;
                o.sv_target.w = 0.0;
                return o;
			}
			ENDCG
		}
		Pass {
			Tags { "LIGHTMODE" = "SHADOWCASTER" "QUEUE" = "Geometry" "RenderType" = "Opaque" }
			Cull Off
			GpuProgramID 94993
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float4 texcoord : TEXCOORD0;
				float3 texcoord1 : TEXCOORD1;
				float texcoord2 : TEXCOORD2;
				float3 texcoord3 : TEXCOORD3;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float4x4 _SpawnRotation;
			// $Globals ConstantBuffers for Fragment Shader
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			// Texture params for Vertex Shader
			// Texture params for Fragment Shader
			
			// Keywords: 
			v2f vert(appdata_full v)
			{
                v2f o;
                float4 tmp0;
                float4 tmp1;
                float4 tmp2;
                tmp0 = v.vertex.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp0 = unity_ObjectToWorld._m00_m10_m20_m30 * v.vertex.xxxx + tmp0;
                tmp0 = unity_ObjectToWorld._m02_m12_m22_m32 * v.vertex.zzzz + tmp0;
                tmp1 = tmp0 + unity_ObjectToWorld._m03_m13_m23_m33;
                tmp0.xyz = unity_ObjectToWorld._m03_m13_m23 * v.vertex.www + tmp0.xyz;
                tmp2 = tmp1.yyyy * unity_MatrixVP._m01_m11_m21_m31;
                tmp2 = unity_MatrixVP._m00_m10_m20_m30 * tmp1.xxxx + tmp2;
                tmp2 = unity_MatrixVP._m02_m12_m22_m32 * tmp1.zzzz + tmp2;
                o.position = unity_MatrixVP._m03_m13_m23_m33 * tmp1.wwww + tmp2;
                o.texcoord = v.vertex;
                tmp1.x = dot(v.normal.xyz, unity_WorldToObject._m00_m10_m20);
                tmp1.y = dot(v.normal.xyz, unity_WorldToObject._m01_m11_m21);
                tmp1.z = dot(v.normal.xyz, unity_WorldToObject._m02_m12_m22);
                tmp0.w = dot(tmp1.xyz, tmp1.xyz);
                tmp0.w = rsqrt(tmp0.w);
                tmp1.xyz = tmp0.www * tmp1.xyz;
                tmp2.xyz = _WorldSpaceCameraPos - tmp0.xyz;
                tmp0.w = dot(tmp2.xyz, tmp2.xyz);
                tmp0.w = rsqrt(tmp0.w);
                tmp2.xyz = tmp0.www * tmp2.xyz;
                tmp0.w = dot(-tmp2.xyz, tmp1.xyz);
                tmp0.w = tmp0.w + tmp0.w;
                tmp1.xyz = tmp1.xyz * -tmp0.www + -tmp2.xyz;
                tmp2.xyz = tmp1.yyy * _SpawnRotation._m01_m11_m21;
                tmp1.xyw = _SpawnRotation._m00_m10_m20 * tmp1.xxx + tmp2.xyz;
                o.texcoord1.xyz = _SpawnRotation._m02_m12_m22 * tmp1.zzz + tmp1.xyw;
                tmp1.xyz = tmp0.xyz - _WorldSpaceCameraPos;
                o.texcoord3.xyz = tmp0.xyz;
                tmp0.x = dot(tmp1.xyz, tmp1.xyz);
                o.texcoord2.x = sqrt(tmp0.x);
                return o;
			}
			// Keywords: 
			fout frag(v2f inp)
			{
                fout o;
                o.sv_target = float4(1.0, 1.0, 1.0, 1.0);
                return o;
			}
			ENDCG
		}
	}
}