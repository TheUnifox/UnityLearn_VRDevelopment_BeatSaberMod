Shader "Custom/SimpleLightning" {
	Properties {
		[NoScaleOffset] _MainTex ("Main Texture", 2D) = "white" {}
		[NoScaleOffset] _NoiseTex ("Noise Texture", 2D) = "black" {}
		[NoScaleOffset] _TimingTex ("Timing Texture", 2D) = "black" {}
		_NoiseSmallScale ("Noise Small Scale", Float) = 1
		_SmallScaleNoiseStrength ("Small Scale Noise Strength", Float) = 2
		_SmallScaleNoiseScrollingSpeed ("Small Scale Noise Scrolling Speed", Float) = 5
		[Space] _NoiseBigScale ("Noise Big Scale", Float) = 0.1
		_BigScaleNoiseStrength ("Big Scale Noise Strength", Float) = 5
		_BigScaleNoiseScrollingSpeed ("Big Scale Noise Scrolling Speed", Float) = 1
		[Space] _NoiseScrollingSpeed ("Noise Scrolling Speed", Float) = 5
		_XNoiseOffsetStrength ("X Noise Offset Strength", Float) = 0.5
		[Space] _Extrude ("Extrude", Float) = 1
		[Space] _ColorBoost ("ColorBoost", Float) = 1
		_WhiteBoost ("WhiteBoost", Float) = 0.2
		_EdgeFadeStrength ("Edge Fade Strength", Float) = 5
		[Space] [Toggle(ENABLE_TARGET_POINT)] _EnableTargetPoint ("Enable Target Point", Float) = 0
		[Space(12)] [Header(Settings)] [Space] [Enum(UnityEngine.Rendering.BlendMode)] _BlendSrcFactor ("Blend Src", Float) = 0
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendDstFactor ("Blend Dst", Float) = 0
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendSrcFactorA ("Blend Src Factor A", Float) = 0
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendDstFactorA ("Blend Dst Factor A", Float) = 10
		[InfoBox(Support on Quest ends after LogicalClear)] [Enum(UnityEngine.Rendering.BlendOp)] _BlendOp ("Blend Operation", Float) = 0
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
			GpuProgramID 6622
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float4 color : COLOR0;
				float2 texcoord : TEXCOORD0;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float _NoiseSmallScale;
			float _SmallScaleNoiseStrength;
			float _SmallScaleNoiseScrollingSpeed;
			float _NoiseBigScale;
			float _BigScaleNoiseStrength;
			float _BigScaleNoiseScrollingSpeed;
			float _Extrude;
			float _EdgeFadeStrength;
			float _XNoiseOffsetStrength;
			// $Globals ConstantBuffers for Fragment Shader
			float _ColorBoost;
			float _WhiteBoost;
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			CBUFFER_START(Props)
				float4 _Color;
			CBUFFER_END
			// Texture params for Vertex Shader
			sampler2D _TimingTex;
			sampler2D _NoiseTex;
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
                tmp0.y = v.vertex.y * _Extrude;
                tmp0.x = v.vertex.x;
                tmp0.z = v.texcoord1.y - 0.5;
                tmp0.z = -abs(tmp0.z) * 2.0 + 1.0;
                tmp0.w = tmp0.z * tmp0.z;
                o.color.w = saturate(tmp0.z * _EdgeFadeStrength);
                tmp0.z = saturate(tmp0.w * _EdgeFadeStrength);
                tmp0.w = unity_ObjectToWorld._m23 + unity_ObjectToWorld._m03;
                tmp1.x = tmp0.w + _Time.x;
                tmp2.xy = v.texcoord1.yy * float2(_NoiseSmallScale.x, _NoiseBigScale.x);
                tmp0.w = tmp1.x * _SmallScaleNoiseScrollingSpeed + tmp2.x;
                tmp2.x = tmp1.x * _BigScaleNoiseScrollingSpeed + tmp2.y;
                tmp1.yw = float2(0.0, 1.0);
                tmp3 = tex2Dlod(_TimingTex, float4(tmp1.xy, 0, 0.0));
                tmp4.y = tmp0.w + tmp3.x;
                tmp4.w = tmp2.x + tmp3.x;
                tmp4.xz = v.texcoord1.xx;
                tmp2 = tex2Dlod(_NoiseTex, float4(tmp4.xy, 0, 0.0));
                tmp3 = tex2Dlod(_NoiseTex, float4(tmp4.zw, 0, 0.0));
                tmp1.xy = tmp3.xy - float2(0.5, 0.5);
                tmp1.xy = tmp0.zz * tmp1.xy;
                tmp1.xy = tmp1.xy * _BigScaleNoiseStrength.xx;
                tmp2.xy = tmp2.xy - float2(0.5, 0.5);
                tmp0.zw = tmp0.zz * tmp2.xy;
                tmp0.zw = tmp0.zw * _SmallScaleNoiseStrength.xx;
                tmp1.z = _XNoiseOffsetStrength;
                tmp0.xy = tmp0.zw * tmp1.zw + tmp0.xy;
                tmp0.xy = tmp1.xy * tmp1.zw + tmp0.xy;
                tmp1 = tmp0.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp0 = unity_ObjectToWorld._m00_m10_m20_m30 * tmp0.xxxx + tmp1;
                tmp0 = unity_ObjectToWorld._m02_m12_m22_m32 * v.vertex.zzzz + tmp0;
                tmp0 = tmp0 + unity_ObjectToWorld._m03_m13_m23_m33;
                tmp1 = tmp0.yyyy * unity_MatrixVP._m01_m11_m21_m31;
                tmp1 = unity_MatrixVP._m00_m10_m20_m30 * tmp0.xxxx + tmp1;
                tmp1 = unity_MatrixVP._m02_m12_m22_m32 * tmp0.zzzz + tmp1;
                o.position = unity_MatrixVP._m03_m13_m23_m33 * tmp0.wwww + tmp1;
                o.color.xyz = float3(1.0, 1.0, 1.0);
                o.texcoord.xy = v.texcoord.xy;
                return o;
			}
			// Keywords: 
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                float4 tmp1;
                tmp0 = tex2D(_MainTex, inp.texcoord.yx);
                tmp0 = tmp0 * inp.color;
                tmp0 = tmp0 * _Color;
                tmp0.w = tmp0.w * tmp0.w;
                tmp1.x = tmp0.w * _ColorBoost;
                tmp1.yzw = tmp0.xyz * tmp1.xxx;
                tmp0.xyz = -tmp0.xyz * tmp1.xxx + float3(1.0, 1.0, 1.0);
                tmp1.x = saturate(tmp0.w * _WhiteBoost);
                o.sv_target.w = tmp0.w;
                o.sv_target.xyz = tmp1.xxx * tmp0.xyz + tmp1.yzw;
                return o;
			}
			ENDCG
		}
	}
}