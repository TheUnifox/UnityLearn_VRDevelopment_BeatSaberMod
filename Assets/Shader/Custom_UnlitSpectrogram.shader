Shader "Custom/UnlitSpectrogram" {
	Properties {
		_Color ("Color", Vector) = (1,1,1,1)
		_SpectrogramScale ("Spectrogram Scale", Float) = 0.5
		[Space] _FogStartOffset ("Fog Start Offset", Float) = 0
		_FogScale ("Fog Scale", Float) = 1
		[Space] [Enum(UnityEngine.Rendering.BlendMode)] _BlendSrcFactor ("Blend Src Factor", Float) = 1
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendDstFactor ("Blend Dst Factor", Float) = 10
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendSrcFactorA ("Blend Src Factor A", Float) = 0
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendDstFactorA ("Blend Dst Factor A", Float) = 10
	}
	SubShader {
		Tags { "DisableBatching" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		Pass {
			Tags { "DisableBatching" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			Blend Zero Zero, Zero Zero
			ZWrite Off
			Cull Off
			GpuProgramID 14940
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float3 texcoord : TEXCOORD0;
				float2 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float _StereoCameraEyeOffset;
			float2 _CustomFogTextureToScreenRatio;
			// $Globals ConstantBuffers for Fragment Shader
			float4 _Color;
			float _SpectrogramScale;
			float _SpectrogramData[64];
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
                o.texcoord1.xy = v.texcoord.xy;
                tmp1.x = floor(unity_StereoEyeIndex);
                tmp1.y = _StereoCameraEyeOffset + _StereoCameraEyeOffset;
                tmp1.x = tmp1.x * tmp1.y + -_StereoCameraEyeOffset;
                tmp2.xyw = tmp0.yxw * float3(0.5, 0.5, 0.5);
                tmp2.z = tmp2.x * _ProjectionParams.x;
                tmp2.yz = tmp0.ww * float2(0.5, 0.5) + tmp2.yz;
                tmp2.x = tmp0.w * tmp1.x + tmp2.y;
                tmp0.xy = -tmp0.ww * float2(0.5, 0.5) + tmp2.xz;
                o.texcoord2.zw = tmp0.zw;
                o.texcoord2.xy = tmp0.xy * _CustomFogTextureToScreenRatio + tmp2.ww;
                return o;
			}
			// Keywords: 
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                tmp0.x = inp.texcoord1.x * 63.0;
                tmp0.x = 0.05 + _SpectrogramData[tmp0.x];
                tmp0.x = -tmp0.x * _SpectrogramScale + 1.0;
                tmp0.y = 1.0 - inp.texcoord1.y;
                tmp0.x = tmp0.y >= tmp0.x;
                tmp0.x = tmp0.x ? 1.0 : 0.0;
                o.sv_target.w = tmp0.x * _Color.w;
                o.sv_target.xyz = _Color.xyz;
                return o;
			}
			ENDCG
		}
	}
}