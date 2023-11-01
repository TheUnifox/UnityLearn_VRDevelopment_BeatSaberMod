Shader "Custom/Parametric3SliceSprite" {
	Properties {
		_MainTex ("Main Texture", 2D) = "white" {}
		[Space] _CapUVSize ("Cap UV Size", Float) = 0.25
		_OffsetFactor ("Offset Factor", Float) = 0
		_OffsetUnits ("Offset Units", Float) = 0
		[Space(12)] [ToggleHeader(ENABLE_FOG)] _EnableFog ("Enable Fog", Float) = 1
		[ToggleShowIfAny(ENABLE_HEIGHT_FOG, ENABLE_FOG)] _EnableHeightFog ("Enable Height Fog", Float) = 1
		[ToggleShowIfAny(USE_FOG_FOR_LIGHTS, ENABLE_FOG)] _UseFogForLights ("Use Fog for Lights", Float) = 1
		[ShowIfAny(ENABLE_FOG)] _FogStartOffset ("Fog Start Offset", Float) = 1
		[ShowIfAny(ENABLE_FOG)] _FogScale ("Fog Scale", Float) = 1
		[Space(12)] [ToggleHeader(ENABLE_WORLD_NOISE)] _EnableWorldNoise ("Enable World Noise", Float) = 0
		[ShowIfAny(ENABLE_WORLD_NOISE)] _WorldNoiseScale ("World Noise Scale", Float) = 1
		[ShowIfAny(ENABLE_WORLD_NOISE)] _WorldNoiseIntensityOffset ("World Intensity Offset", Float) = 0
		[ShowIfAny(ENABLE_WORLD_NOISE)] _WorldNoiseIntensityScale ("World Intenstity Scale", Float) = 1
		[ShowIfAny(ENABLE_WORLD_NOISE)] _WorldNoiseScrolling ("World Noise Scrolling", Vector) = (0,0,0,1)
		[Space()] [ToggleHeader(ENABLE_WORLD_SPACE_FADE)] _EnableWorldSpaceFade ("Enable World Space Fade", Float) = 0
		[ShowIfAny(ENABLE_WORLD_SPACE_FADE)] _WorldSpaceFadePos ("World Space Fade Position", Float) = 0
		[ShowIfAny(ENABLE_WORLD_SPACE_FADE)] _WorldSpaceFadeSlope ("World Space Fade Slope", Float) = 1
		[Space()] [ToggleHeader(ALPHA_WIDTH_SCALE)] _EnableAlphaWidthScale ("Enable Alpha Width Scale", Float) = 0
		[Space(20)] [KeywordEnum(None, MainEffect, Always)] _WhiteBoostType ("White Boost", Float) = 0
		[ShowIfAny(_WHITEBOOSTTYPE_ALWAYS, _WHITEBOOSTTYPE_MAINEFFECT)] _WhiteBoostMultiplier ("White Boost Multiplier", Float) = 1
		[Space] [Header(Other)] [Space] [Toggle(SQUARE_ALPHA)] _SquareAlpha ("Square Alpha", Float) = 0
		[Toggle(ENABLE_ANGLE_DISAPPEAR)] _AngleDisappear ("Angle Disappear", Float) = 10
		[Toggle(ENABLE_NOISE_DITHERING)] _EnableNoiseDithering ("Noise Dithering", Float) = 0
		[Toggle(ENABLE_Y_AXIS_BILLBOARD)] _EnableYAxisBillboard ("Y Axis Billboard", Float) = 1
		[Space] [Header(Settings)] [Space] [Enum(UnityEngine.Rendering.BlendMode)] _BlendSrcFactor ("Blend Src", Float) = 1
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendDstFactor ("Blend Dst", Float) = 1
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendSrcFactorA ("Blend Src Factor A", Float) = 1
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendDstFactorA ("Blend Dst Factor A", Float) = 1
		[Enum(UnityEngine.Rendering.CompareFunction)] _ZTest ("Z Test", Float) = 4
		[Space] [Enum(UnityEngine.Rendering.CullMode)] _CullMode ("CullMode", Float) = 0
		[Space] _StencilRefValue ("Stencil Ref Value", Float) = 0
		[Enum(UnityEngine.Rendering.CompareFunction)] _StencilComp ("Stencil Comp Func", Float) = 8
		[Enum(UnityEngine.Rendering.StencilOp)] _StencilPass ("Stencill Pass Op", Float) = 0
		[Space] [Enum(UnityEngine.Rendering.BlendOp)] _BlendOp ("Blend Operation", Float) = 0
	}
	SubShader {
		Tags { "DisableBatching" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		Pass {
			Tags { "DisableBatching" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			Blend Zero Zero, Zero Zero
			ZWrite Off
			Cull Off
			Stencil {
				Comp Always
				Pass Keep
				Fail Keep
				ZFail Keep
			}
			GpuProgramID 38067
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float3 texcoord1 : TEXCOORD1;
				float texcoord5 : TEXCOORD5;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float _CapUVSize;
			// $Globals ConstantBuffers for Fragment Shader
			// Custom ConstantBuffers for Vertex Shader
			CBUFFER_START(Props)
				float4 _SizeParams;
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
                tmp0.w = _StartWidth;
                tmp1.xy = v.texcoord.xx * float2(_StartWidth.x, _EndWidth.x);
                tmp0.z = tmp1.y;
                tmp2.xy = -float2(_StartWidth.x, _EndWidth.x) + float2(_StartWidth.x, _EndWidth.x);
                tmp2.xy = tmp2.xy * float2(1.3333, 1.3333) + float2(_StartWidth.x, _EndWidth.x);
                tmp2.zw = float2(_StartWidth.x, _EndWidth.x) * float2(0.01, 0.01);
                tmp2.xy = max(tmp2.xy, tmp2.zw);
                tmp3.w = tmp2.y;
                tmp4.xy = tmp2.xy * v.texcoord.xx;
                tmp3.z = tmp4.y;
                tmp1.y = v.vertex.x * _SizeParams.x;
                tmp3.xy = tmp2.xy * tmp1.yy;
                tmp0.xy = tmp1.yy * float2(_StartWidth.x, _EndWidth.x);
                tmp4.w = tmp2.x;
                tmp2.xyz = v.texcoord.yyy > float3(0.9, 0.5, 0.1);
                tmp0.yzw = tmp2.zzz ? tmp0.yzw : tmp3.yzw;
                tmp4.z = tmp3.x;
                tmp1.z = tmp0.x;
                tmp1.w = _EndWidth;
                tmp0.xyz = tmp2.yyy ? tmp1.zxw : tmp0.yzw;
                tmp0.xyz = tmp2.xxx ? tmp4.zxw : tmp0.xyz;
                tmp0.w = v.vertex.y - _SizeParams.z;
                tmp1.x = v.vertex.y - 0.5;
                tmp1.y = v.texcoord.y - 0.5;
                tmp1.z = abs(tmp1.y) >= 0.49;
                tmp1.w = tmp1.z ? 1.0 : 0.0;
                tmp1.x = tmp1.w * tmp1.x;
                tmp1.x = tmp1.x * _SizeParams.w;
                tmp0.w = tmp0.w * _SizeParams.y + tmp1.x;
                tmp2 = tmp0.wwww * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp0.w = tmp0.w > 0.5;
                o.texcoord5.x = tmp0.w ? _AlphaEnd : _AlphaStart;
                tmp2 = unity_ObjectToWorld._m00_m10_m20_m30 * tmp0.xxxx + tmp2;
                o.texcoord1.xz = tmp0.yz;
                tmp0 = unity_ObjectToWorld._m02_m12_m22_m32 * v.vertex.zzzz + tmp2;
                tmp0 = tmp0 + unity_ObjectToWorld._m03_m13_m23_m33;
                tmp2 = tmp0.yyyy * unity_MatrixVP._m01_m11_m21_m31;
                tmp2 = unity_MatrixVP._m00_m10_m20_m30 * tmp0.xxxx + tmp2;
                tmp2 = unity_MatrixVP._m02_m12_m22_m32 * tmp0.zzzz + tmp2;
                o.position = unity_MatrixVP._m03_m13_m23_m33 * tmp0.wwww + tmp2;
                tmp0.x = tmp1.y > 0.0;
                tmp0.y = tmp1.y < 0.0;
                tmp0.x = tmp0.y - tmp0.x;
                tmp0.x = floor(tmp0.x);
                tmp0.y = 0.25 - _CapUVSize;
                tmp0.x = tmp0.y * tmp0.x;
                tmp0.x = tmp1.z ? 0.0 : tmp0.x;
                o.texcoord1.y = tmp0.x + v.texcoord.y;
                return o;
			}
			// Keywords: 
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                tmp0.x = inp.texcoord1.x / inp.texcoord1.z;
                tmp0.y = inp.texcoord1.y;
                tmp0 = tex2D(_MainTex, tmp0.xy);
                tmp0.x = tmp0.w * tmp0.w;
                tmp0.y = inp.texcoord5.x * inp.texcoord5.x;
                tmp0.y = tmp0.y * inp.texcoord5.x;
                tmp0.y = tmp0.y * _Color.w;
                tmp0.x = tmp0.x * tmp0.y;
                o.sv_target.xyz = tmp0.xxx * _Color.xyz;
                o.sv_target.w = tmp0.x;
                return o;
			}
			ENDCG
		}
	}
}