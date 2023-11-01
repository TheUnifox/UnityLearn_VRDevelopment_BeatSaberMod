Shader "Custom/Glowing Pointer" {
	Properties {
		_Color ("Color", Vector) = (1,0,0,0)
		[Space] _FadeStartNormalizedDistance ("Fade Start Normalized Distance", Range(0, 1)) = 1
		[Space] [Toggle(_ENABLE_MAIN_EFFECT_WHITE_BOOST)] _EnableMainEffectWhiteBoost ("White Boost", Float) = 1
	}
	SubShader {
		Tags { "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		Pass {
			Tags { "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			Blend SrcAlpha One, SrcAlpha One
			GpuProgramID 52383
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float4 color : COLOR0;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			// $Globals ConstantBuffers for Fragment Shader
			float _FadeStartNormalizedDistance;
			float4 _Color;
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
                tmp0 = v.vertex.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp0 = unity_ObjectToWorld._m00_m10_m20_m30 * v.vertex.xxxx + tmp0;
                tmp0 = unity_ObjectToWorld._m02_m12_m22_m32 * v.vertex.zzzz + tmp0;
                tmp0 = tmp0 + unity_ObjectToWorld._m03_m13_m23_m33;
                tmp1 = tmp0.yyyy * unity_MatrixVP._m01_m11_m21_m31;
                tmp1 = unity_MatrixVP._m00_m10_m20_m30 * tmp0.xxxx + tmp1;
                tmp1 = unity_MatrixVP._m02_m12_m22_m32 * tmp0.zzzz + tmp1;
                o.position = unity_MatrixVP._m03_m13_m23_m33 * tmp0.wwww + tmp1;
                o.color = v.color;
                return o;
			}
			// Keywords: 
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                tmp0.x = 1.0 - _FadeStartNormalizedDistance;
                tmp0.x = tmp0.x >= inp.color.x;
                tmp0.x = tmp0.x ? 1.0 : 0.0;
                tmp0.y = 1.0 - inp.color.x;
                tmp0.y = tmp0.y / _FadeStartNormalizedDistance;
                tmp0.x = saturate(tmp0.y + tmp0.x);
                o.sv_target.w = tmp0.x * tmp0.x;
                o.sv_target.xyz = _Color.xyz;
                return o;
			}
			ENDCG
		}
	}
}