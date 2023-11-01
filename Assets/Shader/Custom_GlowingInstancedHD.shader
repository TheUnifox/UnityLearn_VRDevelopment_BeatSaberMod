Shader "Custom/GlowingInstancedHD" {
	Properties {
		[Toggle(_ENABLE_COLOR_INSTANCING)] _EnableColorInstancing ("Enable Color Instancing", Float) = 0
		_Color ("Color", Vector) = (1,0,0,0)
		[Space] _FogStartOffset ("Fog Start Offset", Float) = 1
		_FogScale ("Fog Scale", Float) = 1
		[Space] [Toggle(_ENABLE_CUTOUT)] _EnableCutout ("Enable Cutout", Float) = 0
		_CutoutTexScale ("Cutout Texture Scale", Float) = 1
		[PerRendererData] _CutoutTexOffset ("Cutout Texture Offset", Vector) = (0,0,0,0)
		[PerRendererData] _Cutout ("Cutout", Range(0, 1)) = 0
		[Space] [Enum(UnityEngine.Rendering.BlendMode)] _BlendSrcFactor ("Blend Src Factor", Float) = 1
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendDstFactor ("Blend Dst Factor", Float) = 0
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendSrcFactorA ("Blend Src Factor A", Float) = 1
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendDstFactorA ("Blend Dst Factor A", Float) = 0
	}
	SubShader {
		Tags { "QUEUE" = "Geometry" "RenderType" = "Opaque" }
		Pass {
			Tags { "LIGHTMODE" = "FORWARDBASE" "QUEUE" = "Geometry" "RenderType" = "Opaque" }
			Blend Zero Zero, Zero Zero
			GpuProgramID 46963
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float3 texcoord : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float _StereoCameraEyeOffset;
			float2 _CustomFogTextureToScreenRatio;
			// $Globals ConstantBuffers for Fragment Shader
			float _BaseColorBoost;
			float _BaseColorBoostThreshold;
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
                float4 tmp2;
                tmp0 = v.vertex.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp0 = unity_ObjectToWorld._m00_m10_m20_m30 * v.vertex.xxxx + tmp0;
                tmp0 = unity_ObjectToWorld._m02_m12_m22_m32 * v.vertex.zzzz + tmp0;
                tmp1 = tmp0 + unity_ObjectToWorld._m03_m13_m23_m33;
                o.texcoord.xyz = unity_ObjectToWorld._m03_m13_m23 * v.vertex.www + tmp0.xyz;
                tmp0 = tmp1.yyyy * unity_MatrixVP._m01_m11_m21_m31;
                tmp0 = unity_MatrixVP._m00_m10_m20_m30 * tmp1.xxxx + tmp0;
                tmp0 = unity_MatrixVP._m02_m12_m22_m32 * tmp1.zzzz + tmp0;
                tmp0 = unity_MatrixVP._m03_m13_m23_m33 * tmp1.wwww + tmp0;
                o.position = tmp0;
                tmp1.x = floor(unity_StereoEyeIndex);
                tmp1.y = _StereoCameraEyeOffset + _StereoCameraEyeOffset;
                tmp1.x = tmp1.x * tmp1.y + -_StereoCameraEyeOffset;
                tmp2.xyw = tmp0.yxw * float3(0.5, 0.5, 0.5);
                tmp2.z = tmp2.x * _ProjectionParams.x;
                tmp2.yz = tmp0.ww * float2(0.5, 0.5) + tmp2.yz;
                tmp2.x = tmp0.w * tmp1.x + tmp2.y;
                tmp0.xy = -tmp0.ww * float2(0.5, 0.5) + tmp2.xz;
                o.texcoord1.zw = tmp0.zw;
                o.texcoord1.xy = tmp0.xy * _CustomFogTextureToScreenRatio + tmp2.ww;
                return o;
			}
			// Keywords: 
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                tmp0.x = _Color.w * _Color.w;
                tmp0.x = tmp0.x * _BaseColorBoost + -_BaseColorBoostThreshold;
                o.sv_target.xyz = saturate(tmp0.xxx + _Color.xyz);
                o.sv_target.w = _Color.w;
                return o;
			}
			ENDCG
		}
		Pass {
			Tags { "LIGHTMODE" = "SHADOWCASTER" "QUEUE" = "Geometry" "RenderType" = "Opaque" }
			Blend Zero Zero, Zero Zero
			GpuProgramID 121788
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float3 texcoord : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
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
                o.texcoord.xyz = unity_ObjectToWorld._m03_m13_m23 * v.vertex.www + tmp0.xyz;
                tmp0 = tmp1.yyyy * unity_MatrixVP._m01_m11_m21_m31;
                tmp0 = unity_MatrixVP._m00_m10_m20_m30 * tmp1.xxxx + tmp0;
                tmp0 = unity_MatrixVP._m02_m12_m22_m32 * tmp1.zzzz + tmp0;
                tmp0 = unity_MatrixVP._m03_m13_m23_m33 * tmp1.wwww + tmp0;
                o.position = tmp0;
                tmp1.x = floor(unity_StereoEyeIndex);
                tmp1.y = _StereoCameraEyeOffset + _StereoCameraEyeOffset;
                tmp1.x = tmp1.x * tmp1.y + -_StereoCameraEyeOffset;
                tmp2.xyw = tmp0.yxw * float3(0.5, 0.5, 0.5);
                tmp2.z = tmp2.x * _ProjectionParams.x;
                tmp2.yz = tmp0.ww * float2(0.5, 0.5) + tmp2.yz;
                tmp2.x = tmp0.w * tmp1.x + tmp2.y;
                tmp0.xy = -tmp0.ww * float2(0.5, 0.5) + tmp2.xz;
                o.texcoord1.zw = tmp0.zw;
                o.texcoord1.xy = tmp0.xy * _CustomFogTextureToScreenRatio + tmp2.ww;
                return o;
			}
			// Keywords: 
			fout frag(v2f inp)
			{
                fout o;
                o.sv_target = float4(0.0, 0.0, 0.0, 1.0);
                return o;
			}
			ENDCG
		}
	}
}