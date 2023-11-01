// Upgrade NOTE: replaced 'glstate_matrix_projection' with 'UNITY_MATRIX_P'

Shader "TextMeshPro/Mobile/Distance Field ZeroAlphaWrite Fading" {
	Properties {
		_FaceColor ("Face Color", Vector) = (1,1,1,1)
		_FaceDilate ("Face Dilate", Range(-1, 1)) = 0
		_OutlineColor ("Outline Color", Vector) = (0,0,0,1)
		_OutlineWidth ("Outline Thickness", Range(0, 1)) = 0
		_OutlineSoftness ("Outline Softness", Range(0, 1)) = 0
		_UnderlayColor ("Border Color", Vector) = (0,0,0,0.5)
		_UnderlayOffsetX ("Border OffsetX", Range(-1, 1)) = 0
		_UnderlayOffsetY ("Border OffsetY", Range(-1, 1)) = 0
		_UnderlayDilate ("Border Dilate", Range(-1, 1)) = 0
		_UnderlaySoftness ("Border Softness", Range(0, 1)) = 0
		_WeightNormal ("Weight Normal", Float) = 0
		_WeightBold ("Weight Bold", Float) = 0.5
		_ShaderFlags ("Flags", Float) = 0
		_ScaleRatioA ("Scale RatioA", Float) = 1
		_ScaleRatioB ("Scale RatioB", Float) = 1
		_ScaleRatioC ("Scale RatioC", Float) = 1
		_MainTex ("Font Atlas", 2D) = "white" {}
		_TextureWidth ("Texture Width", Float) = 512
		_TextureHeight ("Texture Height", Float) = 512
		_GradientScale ("Gradient Scale", Float) = 5
		_ScaleX ("Scale X", Float) = 1
		_ScaleY ("Scale Y", Float) = 1
		_PerspectiveFilter ("Perspective Correction", Range(0, 1)) = 0.875
		_VertexOffsetX ("Vertex OffsetX", Float) = 0
		_VertexOffsetY ("Vertex OffsetY", Float) = 0
		_ClipRect ("Clip Rect", Vector) = (-32767,-32767,32767,32767)
		_MaskSoftnessX ("Mask SoftnessX", Float) = 0
		_MaskSoftnessY ("Mask SoftnessY", Float) = 0
		_StencilComp ("Stencil Comparison", Float) = 8
		_Stencil ("Stencil ID", Float) = 0
		_StencilOp ("Stencil Operation", Float) = 0
		_StencilWriteMask ("Stencil Write Mask", Float) = 255
		_StencilReadMask ("Stencil Read Mask", Float) = 255
		_ColorMask ("Color Mask", Float) = 15
		_FadeMinY ("Fade Min Y", Float) = 0
		_FadeMaxY ("Fade Max Y", Float) = 10
		_FadeEdgeSize ("Fade Edge Size", Float) = 1
	}
	SubShader {
		Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		Pass {
			Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha, Zero OneMinusSrcAlpha
			ColorMask 0 -1
			ZWrite Off
			Cull Off
			Stencil {
				ReadMask 0
				WriteMask 0
				Comp Always
				Pass Keep
				Fail Keep
				ZFail Keep
			}
			Fog {
				Mode Off
			}
			GpuProgramID 39602
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float4 color : COLOR0;
				float4 color1 : COLOR1;
				float4 texcoord : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
				float3 texcoord5 : TEXCOORD5;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float4 _FaceColor;
			float _FaceDilate;
			float _OutlineSoftness;
			float4 _OutlineColor;
			float _OutlineWidth;
			float _WeightNormal;
			float _WeightBold;
			float _ScaleRatioA;
			float _VertexOffsetX;
			float _VertexOffsetY;
			float4 _ClipRect;
			float _MaskSoftnessX;
			float _MaskSoftnessY;
			float _GradientScale;
			float _ScaleX;
			float _ScaleY;
			// $Globals ConstantBuffers for Fragment Shader
			float _FadeMinY;
			float _FadeMaxY;
			float _FadeEdgeSize;
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
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
                tmp0.x = v.texcoord2.x == 0.0;
                tmp1.y = v.vertex.z;
                tmp1.xz = v.vertex.xy + float2(_VertexOffsetX.x, _VertexOffsetY.x);
                tmp0.y = tmp1.x / v.texcoord2.x;
                tmp2.x = sin(tmp0.y);
                tmp3.x = cos(tmp0.y);
                tmp0.y = tmp3.x * v.texcoord2.x + -v.texcoord2.x;
                tmp2.x = tmp2.x * v.texcoord2.x;
                tmp2.y = tmp0.y + v.vertex.z;
                tmp1.xy = tmp0.xx ? tmp1.xy : tmp2.xy;
                tmp0 = tmp1.zzzz * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp0 = unity_ObjectToWorld._m00_m10_m20_m30 * tmp1.xxxx + tmp0;
                tmp0 = unity_ObjectToWorld._m02_m12_m22_m32 * tmp1.yyyy + tmp0;
                tmp0 = tmp0 + unity_ObjectToWorld._m03_m13_m23_m33;
                tmp2 = tmp0.yyyy * unity_MatrixVP._m01_m11_m21_m31;
                tmp2 = unity_MatrixVP._m00_m10_m20_m30 * tmp0.xxxx + tmp2;
                tmp2 = unity_MatrixVP._m02_m12_m22_m32 * tmp0.zzzz + tmp2;
                tmp0 = unity_MatrixVP._m03_m13_m23_m33 * tmp0.wwww + tmp2;
                o.position = tmp0;
                tmp2 = v.color * _FaceColor;
                tmp2.xyz = tmp2.www * tmp2.xyz;
                o.color = tmp2;
                tmp3.w = v.color.w * _OutlineColor.w;
                tmp3.xyz = tmp3.www * _OutlineColor.xyz;
                tmp3 = tmp3 - tmp2;
                tmp0.xy = _ScreenParams.yy * UNITY_MATRIX_P._m01_m11;
                tmp0.xy = UNITY_MATRIX_P._m00_m10 * _ScreenParams.xx + tmp0.xy;
                tmp0.xy = abs(tmp0.xy) * float2(_ScaleX.x, _ScaleY.x);
                tmp0.xy = tmp0.ww / tmp0.xy;
                tmp0.z = dot(tmp0.xy, tmp0.xy);
                tmp0.xy = float2(_MaskSoftnessX.x, _MaskSoftnessY.x) * float2(0.25, 0.25) + tmp0.xy;
                o.texcoord2.zw = float2(0.25, 0.25) / tmp0.xy;
                tmp0.x = rsqrt(tmp0.z);
                tmp0.y = abs(v.texcoord1.y) * _GradientScale;
                tmp0.x = tmp0.x * tmp0.y;
                tmp0.x = tmp0.x * 1.5;
                tmp0.y = _OutlineSoftness * _ScaleRatioA;
                tmp0.y = tmp0.y * tmp0.x + 1.0;
                tmp0.x = tmp0.x / tmp0.y;
                tmp0.y = _OutlineWidth * _ScaleRatioA;
                tmp0.y = tmp0.y * 0.5;
                tmp0.z = tmp0.x * tmp0.y;
                tmp0.z = tmp0.z + tmp0.z;
                tmp0.z = min(tmp0.z, 1.0);
                tmp0.z = sqrt(tmp0.z);
                o.color1 = tmp0.zzzz * tmp3 + tmp2;
                tmp2 = max(_ClipRect, float4(-20000000000.0, -20000000000.0, -20000000000.0, -20000000000.0));
                tmp2 = min(tmp2, float4(20000000000.0, 20000000000.0, 20000000000.0, 20000000000.0));
                tmp1.yw = tmp1.xz - tmp2.xy;
                tmp1.xz = tmp1.xz * float2(2.0, 2.0) + -tmp2.xy;
                o.texcoord2.xy = tmp1.xz - tmp2.zw;
                tmp1.xz = tmp2.zw - tmp2.xy;
                o.texcoord.zw = tmp1.yw / tmp1.xz;
                o.texcoord.xy = v.texcoord.xy;
                tmp0.z = v.texcoord1.y <= 0.0;
                tmp0.z = tmp0.z ? 1.0 : 0.0;
                tmp1.x = _WeightBold - _WeightNormal;
                tmp0.z = tmp0.z * tmp1.x + _WeightNormal;
                tmp0.z = tmp0.z * 0.25 + _FaceDilate;
                tmp0.z = tmp0.z * _ScaleRatioA;
                tmp0.z = -tmp0.z * 0.5 + 0.5;
                tmp0.w = tmp0.z * tmp0.x + -0.5;
                o.texcoord1.y = -tmp0.y * tmp0.x + tmp0.w;
                o.texcoord1.z = tmp0.y * tmp0.x + tmp0.w;
                o.texcoord1.xw = tmp0.xw;
                o.texcoord5.xyz = v.vertex.xyz;
                return o;
			}
			// Keywords: 
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                float4 tmp1;
                tmp0.x = _FadeMaxY - _FadeMinY;
                tmp0.x = tmp0.x * 0.5;
                tmp0.x = tmp0.x / _FadeEdgeSize;
                tmp0.x = tmp0.x + 1.0;
                tmp0.y = _FadeMinY + _FadeMaxY;
                tmp0.y = -tmp0.y * 0.5 + inp.texcoord5.y;
                tmp0.y = abs(tmp0.y) / _FadeEdgeSize;
                tmp0.x = saturate(tmp0.x - tmp0.y);
                tmp0.yz = _ClipRect.zw - _ClipRect.xy;
                tmp0.yz = tmp0.yz - abs(inp.texcoord2.xy);
                tmp0.yz = saturate(tmp0.yz * inp.texcoord2.zw);
                tmp0.y = tmp0.z * tmp0.y;
                tmp1 = tex2D(_MainTex, inp.texcoord.xy);
                tmp0.z = saturate(tmp1.w * inp.texcoord1.x + -inp.texcoord1.w);
                tmp1 = tmp0.zzzz * inp.color;
                tmp1 = tmp0.yyyy * tmp1;
                o.sv_target.w = tmp0.x * tmp1.w;
                o.sv_target.xyz = tmp1.xyz;
                return o;
			}
			ENDCG
		}
	}
	CustomEditor "TMPro.EditorUtilities.TMP_SDFShaderGUI"
}