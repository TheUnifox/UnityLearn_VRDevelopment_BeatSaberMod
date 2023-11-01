Shader "Custom/Beatmap Editor/ObstacleCore" {
	Properties {
		_DitherTex ("Dither Texture", 2D) = "white" {}
		[Enum(UnityEngine.Rendering.CullMode)] _CullMode ("CullMode", Float) = 0
		[Space] [Toggle(ENABLE_INSTANCED_COLOR)] _EnableInstancedColor ("Enable Instanced Color", Float) = 0
		_TintColor ("Tint Color", Vector) = (1,1,1,1)
		_AddColor ("Add Color", Vector) = (0,0,0,0)
		[Space] _TintColorAlpha ("Tint Color Alpha", Float) = 1
		[Space(12)] [ToggleHeader(ENABLE_FOG)] _EnableFog ("Enable Fog", Float) = 0
		[ShowIfAny(ENABLE_FOG)] _FogStartOffset ("Fog Start Offset", Float) = 0
		[ShowIfAny(ENABLE_FOG)] _FogScale ("Fog Scale", Float) = 1
		[Space(12)] [Toggle(ZWRITE)] _ZWrite ("Z Write", Float) = 0
		[Space] [Toggle(USE_MONO_SCREEN_TEXTURE)] _UseMonoScreenTexture ("Use Mono Screen Texture", Float) = 0
		[Space] [Toggle(ENABLE_CUTOUT)] _EnableCutout ("Enable Cutout", Float) = 0
		_CutoutTexScale ("Cutout Texture Scale", Float) = 1
		[PerRendererData] _CutoutTexOffset ("Cutout Texture Offset", Vector) = (0,0,0,0)
		[PerRendererData] _Cutout ("Cutout", Range(0, 1)) = 0
		[Space] [Toggle(ENABLE_CLIPPING)] _EnableClipping ("Enable Clipping", Float) = 0
		[PerRendererData] _Size ("Size", Vector) = (0,0,0,0)
	}
	SubShader {
		Tags { "DisableBatching" = "true" "IGNOREPROJECTOR" = "true" "PreviewType" = "Plane" "QUEUE" = "Transparent+1" "RenderType" = "Transparent" }
		Pass {
			Tags { "DisableBatching" = "true" "IGNOREPROJECTOR" = "true" "PreviewType" = "Plane" "QUEUE" = "Transparent+1" "RenderType" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha, Zero Zero
			ZWrite Off
			Cull Off
			GpuProgramID 4157
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float2 texcoord : TEXCOORD0;
				float4 position : SV_POSITION0;
				float4 texcoord1 : TEXCOORD1;
				float3 texcoord2 : TEXCOORD2;
				float4 texcoord3 : TEXCOORD3;
				float3 texcoord4 : TEXCOORD4;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float _StereoCameraEyeOffset;
			float2 _CustomFogTextureToScreenRatio;
			float4 _DitherTex_ST;
			// $Globals ConstantBuffers for Fragment Shader
			float4 _TintColor;
			float4 _AddColor;
			float _TintColorAlpha;
			float3 _Size;
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			// Texture params for Vertex Shader
			// Texture params for Fragment Shader
			sampler2D _DitherTex;
			
			// Keywords: 
			v2f vert(appdata_full v)
			{
                v2f o;
                float4 tmp0;
                float4 tmp1;
                float4 tmp2;
                o.texcoord.xy = v.texcoord.xy * _DitherTex_ST.xy + _DitherTex_ST.zw;
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
                o.texcoord4.xyz = v.normal.xyz;
                return o;
			}
			// Keywords: 
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                tmp0 = abs(inp.texcoord4.xxyy) * _Size.zyxz;
                tmp0.xy = tmp0.zw + tmp0.xy;
                tmp0.xy = _Size.xy * abs(inp.texcoord4.zz) + tmp0.xy;
                tmp0.xy = tmp0.xy * inp.texcoord.xy;
                tmp0 = tex2D(_DitherTex, tmp0.xy);
                tmp0.x = 1.0 - tmp0.x;
                tmp0.y = inp.texcoord2.z <= 0.0;
                tmp0.y = tmp0.y ? 1.0 : 0.0;
                o.sv_target.w = -tmp0.x * tmp0.y + _TintColorAlpha;
                o.sv_target.xyz = _TintColor.xyz * float3(0.1, 0.1, 0.1) + _AddColor.xyz;
                return o;
			}
			ENDCG
		}
	}
}