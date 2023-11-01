Shader "Custom/SliderNoteCrossedStrips" {
	Properties {
		_Color ("Color", Vector) = (1,1,1,1)
		[Space(12)] _AttractorFadeLength ("Attractor Fade Length", Float) = 0.2
		_AttractorFadeOffset ("Attractor Fade Offset", Float) = 0.1
		[InfoBox(How fast a saber starts atracting the slider after reaching the hit point)] _AttractorAttack ("Attractor Attack", Float) = 1
		[Space(12)] [BigHeader(POSITION_THRESHOLDS)] [Space(12)] _Width ("Width", Float) = 1
		[BigHeader(TEXTURE)] [Space(12)] [Space] _MainTex ("Main Texture", 2D) = "black" {}
		[Toggle(TEXTURE_COLOR)] _EnableTextureColor ("Use Texture Color", Float) = 0
		[EnumShowIf(Alpha, Red, 0TEXTURE_COLOR)] _AlphaChannel ("Alpha Channel", Float) = 0
		_Intensity ("Intensity", Float) = 1
		_UvPanning ("UV Panning", Vector) = (0,0,0,0)
		[Space(16)] [KeywordEnum(None, 90_CW, 90_CCW, 180_CW)] _Rotate_UV ("Rotate UVs", Float) = 0
		[ToggleShowIfAny(ROTATE_MAIN_ONLY, _ROTATE_UV_90_CW, _ROTATE_UV_90_90_CCW, _ROTATE_UV_180_CW)] _RotateMainUVOnly ("Rotate Main UV Only", Float) = 0
		[Space(12)] [ToggleHeader(FLOWMAP)] _EnableFlowmap ("Use Flowmap", Float) = 0
		[ShowIfAny(FLOWMAP)] _FlowTex ("Flowmap Texture", 2D) = "black" {}
		[ShowIfAny(FLOWMAP)] _FlowSpeed ("Flow Speed", Range(0, 1)) = 0.2
		[ShowIfAny(FLOWMAP)] _FlowStrength ("Flow Strength", Range(-1, 1)) = 0.2
		[VectorShowIfAny(2, FLOWMAP)] _FlowAdd ("Flowmap Additional Direction", Vector) = (0,0,0,0)
		[ShowIfAny(FLOWMAP)] _FlowPanning ("Flowmap Panning", Vector) = (0,0,0,0)
		[Space(12)] [ToggleHeader(SOFT_PARTICLES)] _EnableSoftParticles ("Soft Particles", Float) = 0
		[ShowIfAny(SOFT_PARTICLES)] _SoftFactor ("Soft Factor", Range(0, 50)) = 0
		[Space(12)] [ToggleHeader(CLOSE_TO_CAMERA_DISAPPEAR)] _EnableCloseToCameraDisappear ("Close to Camera Dissapear", Float) = 0
		[ShowIfAny(CLOSE_TO_CAMERA_DISAPPEAR)] _CloseToCameraOffset ("Close to Camera Offset", Float) = 0.5
		[ShowIfAny(CLOSE_TO_CAMERA_DISAPPEAR)] _CloseToCameraFactor ("Close to Camera Factor", Float) = 0.5
		[Space(12)] [EnumHeader(None, Alpha, Color, Lerp)] _FogType ("Fog Type", Float) = 0
		[ShowIfAny(_FOGTYPE_ALPHA, _FOGTYPE_LERP)] _FogStartOffset ("Fog Start Offset", Float) = 0
		[ShowIfAny(_FOGTYPE_ALPHA, _FOGTYPE_LERP)] _FogScale ("Fog Scale", Float) = 1
		[ToggleShowIfAny(HEIGHT_FOG, _FOGTYPE_ALPHA, _FOGTYPE_LERP)] _EnableHeightFog ("Enable Height Fog", Float) = 0
		[ToggleShowIfAny(PRECISE_FOG, _FOGTYPE_ALPHA, _FOGTYPE_LERP)] _PreciseFog ("High (Frag) Precision", Float) = 0
		[Space(12)] [ToggleHeader(FAKE_MIRROR_TRANSPARENCY)] _FakeMirrorTransparencyEnabled ("Fake Mirror Transparency", Float) = 0
		[ShowIfAny(FAKE_MIRROR_TRANSPARENCY)] _FakeMirrorTransparencyMultiplier ("Mirror Transparency Multiplier", Float) = 1
		[Space()] [Header(Other)] [Space] [Toggle(SQUARE_ALPHA)] _SquareAlpha ("Square Alpha", Float) = 1
		_AlphaMultiplier ("Alpha Multiplier", Float) = 1
		[KeywordEnum(None, MainEffect, Always)] _WhiteBoostType ("White Boost", Float) = 0
		[Toggle(VERTICAL_MIRROR)] _VerticalMirror ("Vertical Mirror", Float) = 0
		[Toggle(BEATMAP_EDITOR_ONLY)] _BeatmapEditorOnly ("Beatmap Editor Only", Float) = 0
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
			GpuProgramID 37385
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float2 texcoord : TEXCOORD0;
				float2 texcoord11 : TEXCOORD11;
				float4 color : COLOR0;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float _TrackLaneYPosition;
			float4 _MainTex_ST;
			float2 _UvPanning;
			float _AttractorFadeLength;
			float _AttractorFadeOffset;
			float _Width;
			float _AttractorAttack;
			// $Globals ConstantBuffers for Fragment Shader
			float _Intensity;
			float _AlphaMultiplier;
			// Custom ConstantBuffers for Vertex Shader
			CBUFFER_START(Props)
				float3 _SaberAttractionPoint;
				float _TimeSinceHeadNoteJump;
				float _JumpSpeed;
				float _JumpDistance;
				float3 _HeadNoteJumpData;
				float3 _TailNoteJumpData;
				float _Cutout;
				float _Random;
				float _HeadFadeLength;
				float _TailFadeLength;
				float _SliderZLength;
				float _SliderLength;
				float _SaberAttractionMultiplier;
				float _TailHeadNoteJumpOffsetDifference;
			CBUFFER_END
			// Custom ConstantBuffers for Fragment Shader
			CBUFFER_START(Props)
				float4 _Color;
			CBUFFER_END
			// Texture params for Vertex Shader
			// Texture params for Fragment Shader
			sampler2D _MainTex;
			
			// Keywords: 
			v2f vert(appdata_full v)
			{
                v2f o;
                float4 tmp0;
                float4 tmp1;
                float4 tmp2;
                float4 tmp3;
                float4 tmp4;
                tmp0.x = v.vertex.z / _JumpSpeed;
                tmp0.x = _TimeSinceHeadNoteJump - tmp0.x;
                tmp0.x = max(tmp0.x, 0.0);
                tmp0.y = tmp0.x * tmp0.x;
                tmp1.xyz = _TailNoteJumpData - _HeadNoteJumpData;
                tmp0.z = v.vertex.z / _SliderZLength;
                tmp1.xyz = tmp0.zzz * tmp1.xyz + _HeadNoteJumpData;
                tmp0.z = _TailHeadNoteJumpOffsetDifference * tmp0.z + v.vertex.y;
                tmp0.y = tmp0.y * tmp1.x;
                tmp0.y = tmp0.y * 0.5;
                tmp0.y = tmp1.y * tmp0.x + -tmp0.y;
                tmp0.y = tmp1.z - tmp0.y;
                tmp0.y = tmp0.z - tmp0.y;
                tmp0.z = tmp0.y - _Width;
                tmp0.z = tmp0.z * 0.6666667;
                tmp0.w = 3.0 - _Width;
                tmp0.z = tmp0.z / tmp0.w;
                tmp0.z = saturate(tmp0.z + 0.1666667);
                tmp1.x = tmp0.z * -2.0 + 3.0;
                tmp0.z = tmp0.z * tmp0.z;
                tmp0.z = tmp0.z * tmp1.x;
                tmp0.z = tmp0.z * tmp0.w + _Width;
                tmp0.w = _TrackLaneYPosition < 0.0;
                tmp1.y = tmp0.w ? tmp0.y : tmp0.z;
                tmp0.yzw = unity_WorldToObject._m01_m11_m21 * _SaberAttractionPoint;
                tmp0.yzw = unity_WorldToObject._m00_m10_m20 * _SaberAttractionPoint + tmp0.yzw;
                tmp0.yzw = unity_WorldToObject._m02_m12_m22 * _SaberAttractionPoint + tmp0.yzw;
                tmp0.yzw = tmp0.yzw + unity_WorldToObject._m03_m13_m23;
                tmp1.w = _SliderZLength - tmp0.w;
                tmp1.w = max(tmp1.w, 1.0);
                tmp2.x = v.vertex.z - tmp0.w;
                tmp2.x = max(tmp2.x, 0.0);
                tmp1.w = tmp2.x / tmp1.w;
                tmp1.w = 1.0 - tmp1.w;
                tmp2.y = _JumpDistance * 0.5;
                tmp2.x = min(tmp2.y, tmp2.x);
                tmp2.x = -tmp2.x / tmp2.y;
                tmp2.y = tmp2.y / _JumpSpeed;
                tmp2.x = tmp2.x + 1.0;
                tmp1.w = tmp1.w * tmp2.x;
                tmp2.x = tmp0.w / _JumpSpeed;
                tmp2.x = saturate(tmp2.x / _AttractorAttack);
                tmp1.w = tmp1.w * tmp2.x;
                tmp1.w = tmp1.w * tmp1.w;
                tmp1.w = tmp1.w * _SaberAttractionMultiplier;
                tmp2.x = v.vertex.x + 2.5;
                tmp2.x = saturate(tmp2.x * 0.1333333 + 0.1666667);
                tmp2.z = tmp2.x * -2.0 + 3.0;
                tmp2.x = tmp2.x * tmp2.x;
                tmp2.x = tmp2.x * tmp2.z;
                tmp1.x = tmp2.x * 5.0 + -2.5;
                tmp1.z = v.vertex.z;
                tmp2.xzw = tmp0.yzw - tmp1.xyz;
                tmp1.xyz = tmp1.www * tmp2.xzw + tmp1.xyz;
                tmp1.w = v.texcoord.x - 0.5;
                tmp1.w = tmp1.w + tmp1.w;
                tmp2.xzw = tmp1.www * v.normal.xyz;
                tmp1.w = tmp2.y - tmp0.x;
                tmp2.y = tmp2.y * 0.5;
                tmp0.x = saturate(tmp0.x / tmp2.y);
                tmp1.w = tmp1.w * _JumpSpeed;
                tmp2.y = saturate(tmp1.w * 4.0 + 2.0);
                tmp1.w = tmp1.w * 4.0;
                tmp1.w = saturate(tmp1.w);
                tmp2.y = tmp2.y * _Width;
                tmp1.xyz = tmp2.xzw * tmp2.yyy + tmp1.xyz;
                tmp2.y = tmp2.y + tmp2.y;
                tmp3 = tmp1.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp3 = unity_ObjectToWorld._m00_m10_m20_m30 * tmp1.xxxx + tmp3;
                tmp3 = unity_ObjectToWorld._m02_m12_m22_m32 * tmp1.zzzz + tmp3;
                tmp0.yzw = tmp1.xyz - tmp0.yzw;
                tmp0.yzw = tmp0.yzw / _AttractorFadeLength.xxx;
                tmp0.y = dot(tmp0.xyz, tmp0.xyz);
                tmp0.y = saturate(tmp0.y - _AttractorFadeOffset);
                tmp0.y = tmp0.y * tmp0.y;
                tmp3 = tmp3 + unity_ObjectToWorld._m03_m13_m23_m33;
                tmp4 = tmp3.yyyy * unity_MatrixVP._m01_m11_m21_m31;
                tmp4 = unity_MatrixVP._m00_m10_m20_m30 * tmp3.xxxx + tmp4;
                tmp4 = unity_MatrixVP._m02_m12_m22_m32 * tmp3.zzzz + tmp4;
                o.position = unity_MatrixVP._m03_m13_m23_m33 * tmp3.wwww + tmp4;
                tmp2.x = _SliderLength;
                tmp0.zw = tmp2.xy * _MainTex_ST.xy;
                tmp0.zw = v.texcoord.xy * tmp0.zw + _MainTex_ST.zw;
                tmp0.zw = _Time.yy * _UvPanning + tmp0.zw;
                o.texcoord.x = tmp0.z + _Random;
                o.texcoord.y = tmp0.w;
                o.texcoord11.xy = v.texcoord.xy;
                tmp0.z = saturate(v.color.w);
                tmp0.y = tmp0.y * tmp0.z;
                tmp0.z = v.texcoord.y * _SliderLength;
                tmp0.z = saturate(tmp0.z / _HeadFadeLength);
                tmp0.z = tmp0.z * tmp0.z;
                tmp0.y = tmp0.z * tmp0.y;
                tmp0.z = 1.0 - v.texcoord.y;
                tmp0.z = tmp0.z * _SliderLength;
                tmp0.z = saturate(tmp0.z / _TailFadeLength);
                tmp0.z = tmp0.z * tmp0.z;
                tmp0.y = tmp0.z * tmp0.y;
                tmp0.x = tmp0.x * tmp0.y;
                tmp0.x = tmp1.w * tmp0.x;
                tmp0.y = 1.0 - _Cutout;
                o.color.w = tmp0.y * tmp0.x;
                o.color.xyz = v.color.xyz;
                return o;
			}
			// Keywords: 
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                float4 tmp1;
                tmp0.x = inp.texcoord11.y - 0.5;
                tmp0.x = -abs(tmp0.x) * 2.0 + 1.0;
                tmp0.x = tmp0.x * tmp0.x;
                tmp1 = tex2D(_MainTex, inp.texcoord.xy);
                tmp1.x = _Intensity;
                tmp1 = tmp1.xxxw * _Color;
                tmp1 = tmp1 * inp.color;
                tmp0.x = tmp0.x * tmp1.w;
                tmp0.x = tmp0.x * _AlphaMultiplier;
                o.sv_target.xyz = tmp0.xxx * tmp1.xyz;
                o.sv_target.w = tmp0.x;
                return o;
			}
			ENDCG
		}
	}
}