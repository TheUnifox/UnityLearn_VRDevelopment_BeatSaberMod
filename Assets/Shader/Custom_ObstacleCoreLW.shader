Shader "Custom/ObstacleCoreLW" {
	Properties {
		[Enum(UnityEngine.Rendering.CullMode)] _CullMode ("CullMode", Float) = 0
		[Space] [Toggle(ENABLE_INSTANCED_COLOR)] _EnableInstancedColor ("Enable Instanced Color", Float) = 0
		_TintColor ("Tint Color", Vector) = (1,1,1,1)
		_AddColor ("Add Color", Vector) = (0,0,0,0)
		[Space] _TintColorAlpha ("Tint Color Alpha", Float) = 1
		[Space(12)] [ToggleHeader(ENABLE_FOG)] _EnableFog ("Enable Fog", Float) = 0
		[ShowIfAny(ENABLE_FOG)] _FogStartOffset ("Fog Start Offset", Float) = 0
		[ShowIfAny(ENABLE_FOG)] _FogScale ("Fog Scale", Float) = 1
		[ToggleShowIfAny(HEIGHT_FOG, ENABLE_FOG)] _EnableHeightFog ("Enable Height Fog", Float) = 0
		[ShowIfAny(HEIGHT_FOG)] _FogHeightScale ("Fog Height Scale", Float) = 1
		[ShowIfAny(HEIGHT_FOG)] _FogHeightOffset ("Fog Height Offset", Float) = 0
		[Space(12)] [Toggle(ZWRITE)] _ZWrite ("Z Write", Float) = 0
		[Space] [Toggle(USE_MONO_SCREEN_TEXTURE)] _UseMonoScreenTexture ("Use Mono Screen Texture", Float) = 0
		[Space] [Toggle(ENABLE_CUTOUT)] _EnableCutout ("Enable Cutout", Float) = 0
		_CutoutTexScale ("Cutout Texture Scale", Float) = 1
		[PerRendererData] _CutoutTexOffset ("Cutout Texture Offset", Vector) = (0,0,0,0)
		[PerRendererData] _Cutout ("Cutout", Range(0, 1)) = 0
		[Space] [Toggle(ENABLE_CLIPPING)] _EnableClipping ("Enable Clipping", Float) = 0
	}
	SubShader {
		Tags { "DisableBatching" = "true" "IGNOREPROJECTOR" = "true" "PreviewType" = "Plane" "QUEUE" = "Transparent+1" "RenderType" = "Opaque" }
		Pass {
			Tags { "DisableBatching" = "true" "IGNOREPROJECTOR" = "true" "PreviewType" = "Plane" "QUEUE" = "Transparent+1" "RenderType" = "Opaque" }
			Blend SrcAlpha OneMinusSrcAlpha, Zero Zero
			ZWrite Off
			Cull Off
			GpuProgramID 10757
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float4 texcoord1 : TEXCOORD1;
				float3 texcoord2 : TEXCOORD2;
				float4 texcoord3 : TEXCOORD3;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float _StereoCameraEyeOffset;
			float2 _CustomFogTextureToScreenRatio;
			// $Globals ConstantBuffers for Fragment Shader
			float4 _TintColor;
			float4 _AddColor;
			float _TintColorAlpha;
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
                o.texcoord2.xyz = unity_ObjectToWorld._m03_m13_m23 * v.vertex.www + tmp0.xyz;
                tmp0 = tmp1.yyyy * unity_MatrixVP._m01_m11_m21_m31;
                tmp0 = unity_MatrixVP._m00_m10_m20_m30 * tmp1.xxxx + tmp0;
                tmp0 = unity_MatrixVP._m02_m12_m22_m32 * tmp1.zzzz + tmp0;
                tmp0 = unity_MatrixVP._m03_m13_m23_m33 * tmp1.wwww + tmp0;
                o.position = tmp0;
                o.texcoord1 = v.color;
                tmp1.x = floor(unity_StereoEyeIndex);
                tmp1.y = _StereoCameraEyeOffset + _StereoCameraEyeOffset;
                tmp1.x = tmp1.x * tmp1.y + -_StereoCameraEyeOffset;
                tmp2.xyw = tmp0.yxw * float3(0.5, 0.5, 0.5);
                tmp2.z = tmp2.x * _ProjectionParams.x;
                tmp2.yz = tmp0.ww * float2(0.5, 0.5) + tmp2.yz;
                tmp2.x = tmp0.w * tmp1.x + tmp2.y;
                tmp0.xy = -tmp0.ww * float2(0.5, 0.5) + tmp2.xz;
                o.texcoord3.zw = tmp0.zw;
                o.texcoord3.xy = tmp0.xy * _CustomFogTextureToScreenRatio + tmp2.ww;
                return o;
			}
			// Keywords: 
			fout frag(v2f inp)
			{
                fout o;
                o.sv_target.xyz = _TintColor.xyz * float3(0.1, 0.1, 0.1) + _AddColor.xyz;
                o.sv_target.w = _TintColorAlpha;
                return o;
			}
			ENDCG
		}
	}
}