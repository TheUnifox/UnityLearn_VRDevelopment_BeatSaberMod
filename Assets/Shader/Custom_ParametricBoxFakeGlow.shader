Shader "Custom/ParametricBoxFakeGlow" {
	Properties {
		[Space] _MainTex ("Main Texture", 2D) = "white" {}
		[Space] _FogStartOffset ("Fog Start Offset", Float) = 1
		_FogScale ("Fog Scale", Float) = 1
		[ToggleHeader(HEIGHT_FOG)] _EnableHeightFog ("Enable Height Fog", Float) = 0
		[ShowIfAny(HEIGHT_FOG)] _FogHeightScale ("Fog Height Scale", Float) = 1
		[ShowIfAny(HEIGHT_FOG)] _FogHeightOffset ("Fog Height Offset", Float) = 0
		[Space] _AngleDisappearParam ("Angle disappear param", Float) = 1
		[Space] [KeywordEnum(None, MainEffect, Always)] _WhiteBoostType ("White Boost", Float) = 1
		[Toggle(ENABLE_CUTOUT)] _EnableCutout ("Enable Cutout", Float) = 0
		[Toggle(ENABLE_CLIPPING)] _EnableClipping ("Enable Clipping", Float) = 0
		[Space] [Enum(UnityEngine.Rendering.BlendMode)] _BlendSrcFactor ("Blend Src", Float) = 1
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendDstFactor ("Blend Dst", Float) = 1
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendSrcFactorA ("Blend Src Factor A", Float) = 1
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendDstFactorA ("Blend Dst Factor A", Float) = 1
	}
	SubShader {
		Tags { "DisableBatching" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		Pass {
			Tags { "DisableBatching" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			Blend Zero Zero, Zero Zero
			ZWrite Off
			Cull Off
			GpuProgramID 42625
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float4 texcoord2 : TEXCOORD2;
				float2 texcoord3 : TEXCOORD3;
				float texcoord4 : TEXCOORD4;
				float3 texcoord5 : TEXCOORD5;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float _StereoCameraEyeOffset;
			float2 _CustomFogTextureToScreenRatio;
			float _AngleDisappearParam;
			// $Globals ConstantBuffers for Fragment Shader
			float _CustomFogHeightFogStartY;
			float _CustomFogHeightFogHeight;
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
			sampler2D _MainTex;
			
			// Keywords: 
			v2f vert(appdata_full v)
			{
                v2f o;
                float4 tmp0;
                float4 tmp1;
                float4 tmp2;
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
                tmp0.xyz = unity_ObjectToWorld._m03_m13_m23 * v.vertex.www + tmp0.xyz;
                tmp2 = tmp1.yyyy * unity_MatrixVP._m01_m11_m21_m31;
                tmp2 = unity_MatrixVP._m00_m10_m20_m30 * tmp1.xxxx + tmp2;
                tmp2 = unity_MatrixVP._m02_m12_m22_m32 * tmp1.zzzz + tmp2;
                tmp1 = unity_MatrixVP._m03_m13_m23_m33 * tmp1.wwww + tmp2;
                o.position = tmp1;
                tmp2.xyw = tmp1.yxw * float3(0.5, 0.5, 0.5);
                tmp2.z = tmp2.x * _ProjectionParams.x;
                tmp2.yz = tmp1.ww * float2(0.5, 0.5) + tmp2.yz;
                tmp0.w = floor(unity_StereoEyeIndex);
                tmp1.x = _StereoCameraEyeOffset + _StereoCameraEyeOffset;
                tmp0.w = tmp0.w * tmp1.x + -_StereoCameraEyeOffset;
                tmp2.x = tmp1.w * tmp0.w + tmp2.y;
                tmp1.xy = -tmp1.ww * float2(0.5, 0.5) + tmp2.xz;
                o.texcoord2.zw = tmp1.zw;
                o.texcoord2.xy = tmp1.xy * _CustomFogTextureToScreenRatio + tmp2.ww;
                tmp1.xyz = tmp0.xyz - _WorldSpaceCameraPos;
                o.texcoord5.xyz = tmp0.xyz;
                tmp0.x = dot(tmp1.xyz, tmp1.xyz);
                tmp0.x = rsqrt(tmp0.x);
                tmp0.xyz = tmp0.xxx * tmp1.xyz;
                tmp1.x = dot(v.normal.xyz, unity_WorldToObject._m00_m10_m20);
                tmp1.y = dot(v.normal.xyz, unity_WorldToObject._m01_m11_m21);
                tmp1.z = dot(v.normal.xyz, unity_WorldToObject._m02_m12_m22);
                tmp0.w = dot(tmp1.xyz, tmp1.xyz);
                tmp0.w = rsqrt(tmp0.w);
                tmp1.xyz = tmp0.www * tmp1.xyz;
                tmp0.x = dot(tmp0.xyz, tmp1.xyz);
                tmp0.x = tmp0.x * _AngleDisappearParam;
                o.texcoord4.x = min(abs(tmp0.x), 1.0);
                o.texcoord3.xy = v.texcoord.xy;
                return o;
			}
			// Keywords: 
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                float4 tmp1;
                tmp0.x = _CustomFogHeightFogHeight + _CustomFogHeightFogStartY;
                tmp0.x = inp.texcoord5.y - tmp0.x;
                tmp0.x = saturate(tmp0.x / _CustomFogHeightFogHeight);
                tmp0.y = tmp0.x * tmp0.x;
                tmp0.x = -tmp0.x * 2.0 + 3.0;
                tmp0.x = -tmp0.y * tmp0.x + 1.0;
                tmp0.x = 1.0 - tmp0.x;
                tmp0.y = inp.texcoord4.x * _Color.w;
                tmp0.x = saturate(tmp0.x * tmp0.y);
                tmp1 = tex2D(_MainTex, inp.texcoord3.xy);
                tmp0.y = tmp1.w * tmp1.w;
                tmp0.x = tmp0.y * tmp0.x;
                o.sv_target.xyz = tmp0.xxx * _Color.xyz;
                o.sv_target.w = tmp0.x;
                return o;
			}
			ENDCG
		}
	}
}