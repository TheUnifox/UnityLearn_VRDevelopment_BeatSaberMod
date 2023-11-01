Shader "Custom/Spectrogram" {
	Properties {
		_Color ("Color", Vector) = (1,1,1,1)
		_PeakOffset ("Peak Offset", Vector) = (0,10,0,1)
		_Metallic ("Metallic", Range(0, 1)) = 0
		_Smoothness ("Smoothness", Range(0, 1)) = 0.5
		[Space] [Toggle(DIFFUSE)] _EnableDiffuse ("Enable Diffuse", Float) = 1
		[Space(12)] [ToggleHeader(SPECULAR)] _EnableSpecular ("Enable Specular", Float) = 1
		[ShowIfAny(SPECULAR)] _SpecularIntensity ("Specular Intensity", Float) = 1
		[Space(12)] [ToggleHeader(LIGHT_FALLOFF)] _EnableLightFalloff ("Enable Light Falloff", Float) = 0
		[Space(12)] _FogStartOffset ("Fog Start Offset", Float) = 0
		_FogScale ("Fog Scale", Float) = 1
		[Space(12)] [Toggle] _ZWrite ("Z Write", Float) = 1
	}
	SubShader {
		Tags { "DisableBatching" = "true" "QUEUE" = "Geometry" "RenderType" = "Opaque" }
		Pass {
			Tags { "DisableBatching" = "true" "QUEUE" = "Geometry" "RenderType" = "Opaque" }
			ZWrite Off
			GpuProgramID 51984
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float3 texcoord1 : TEXCOORD1;
				float3 texcoord2 : TEXCOORD2;
				float4 texcoord3 : TEXCOORD3;
				float4 texcoord4 : TEXCOORD4;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float _StereoCameraEyeOffset;
			float2 _CustomFogTextureToScreenRatio;
			float2 _GlobalBlueNoiseParams;
			float _GlobalRandomValue;
			float3 _PeakOffset;
			float _SpectrogramData[64];
			// $Globals ConstantBuffers for Fragment Shader
			float _CustomFogHeightFogStartY;
			float _CustomFogHeightFogHeight;
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			// Texture params for Vertex Shader
			// Texture params for Fragment Shader
			sampler2D _GlobalBlueNoiseTex;
			
			// Keywords: 
			v2f vert(appdata_full v)
			{
                v2f o;
                float4 tmp0;
                float4 tmp1;
                float4 tmp2;
                tmp0.x = v.texcoord.x * 63.0;
                tmp0.x = 1.0 - _SpectrogramData[tmp0.x];
                tmp0.xyz = tmp0.xxx * _PeakOffset;
                tmp0.xyz = -tmp0.xyz * v.texcoord.yyy + v.vertex.xyz;
                tmp1 = tmp0.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp1 = unity_ObjectToWorld._m00_m10_m20_m30 * tmp0.xxxx + tmp1;
                tmp0 = unity_ObjectToWorld._m02_m12_m22_m32 * tmp0.zzzz + tmp1;
                tmp1 = tmp0 + unity_ObjectToWorld._m03_m13_m23_m33;
                o.texcoord1.xyz = unity_ObjectToWorld._m03_m13_m23 * v.vertex.www + tmp0.xyz;
                tmp0 = tmp1.yyyy * unity_MatrixVP._m01_m11_m21_m31;
                tmp0 = unity_MatrixVP._m00_m10_m20_m30 * tmp1.xxxx + tmp0;
                tmp0 = unity_MatrixVP._m02_m12_m22_m32 * tmp1.zzzz + tmp0;
                tmp0 = unity_MatrixVP._m03_m13_m23_m33 * tmp1.wwww + tmp0;
                o.position = tmp0;
                tmp1.x = dot(v.normal.xyz, unity_WorldToObject._m00_m10_m20);
                tmp1.y = dot(v.normal.xyz, unity_WorldToObject._m01_m11_m21);
                tmp1.z = dot(v.normal.xyz, unity_WorldToObject._m02_m12_m22);
                tmp1.w = dot(tmp1.xyz, tmp1.xyz);
                tmp1.w = rsqrt(tmp1.w);
                o.texcoord2.xyz = tmp1.www * tmp1.xyz;
                tmp1.x = floor(unity_StereoEyeIndex);
                tmp1.y = _StereoCameraEyeOffset + _StereoCameraEyeOffset;
                tmp1.x = tmp1.x * tmp1.y + -_StereoCameraEyeOffset;
                tmp2.xyw = tmp0.yxw * float3(0.5, 0.5, 0.5);
                tmp2.z = tmp2.x * _ProjectionParams.x;
                tmp2.yz = tmp0.ww * float2(0.5, 0.5) + tmp2.yz;
                tmp2.x = tmp0.w * tmp1.x + tmp2.y;
                tmp0.xy = -tmp0.ww * float2(0.5, 0.5) + tmp2.xz;
                o.texcoord3.xy = tmp0.xy * _CustomFogTextureToScreenRatio + tmp2.ww;
                tmp0.x = tmp0.w * _GlobalRandomValue;
                tmp0.xy = tmp2.xz * _GlobalBlueNoiseParams + tmp0.xx;
                o.texcoord4.xy = tmp0.xy + unity_ObjectToWorld._m03_m13;
                o.texcoord3.zw = tmp0.zw;
                o.texcoord4.zw = tmp0.zw;
                return o;
			}
			// Keywords: 
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                float4 tmp1;
                tmp0.x = _CustomFogHeightFogHeight + _CustomFogHeightFogStartY;
                tmp0.x = inp.texcoord1.y - tmp0.x;
                tmp0.x = saturate(tmp0.x / _CustomFogHeightFogHeight);
                tmp0.y = tmp0.x * tmp0.x;
                tmp0.x = -tmp0.x * 2.0 + 3.0;
                tmp0.x = -tmp0.y * tmp0.x + 1.0;
                tmp0.yz = inp.texcoord4.xy / inp.texcoord4.ww;
                tmp1 = tex2D(_GlobalBlueNoiseTex, tmp0.yz);
                tmp0.y = tmp1.x - 0.5;
                tmp0.y = tmp0.y * 0.0039216;
                o.sv_target.xyz = tmp0.xxx * float3(0.1, 0.1, 0.1) + tmp0.yyy;
                o.sv_target.w = 0.0;
                return o;
			}
			ENDCG
		}
		Pass {
			Tags { "DisableBatching" = "true" "LIGHTMODE" = "SHADOWCASTER" "QUEUE" = "Geometry" "RenderType" = "Opaque" }
			GpuProgramID 110530
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float3 texcoord1 : TEXCOORD1;
				float3 texcoord2 : TEXCOORD2;
				float4 texcoord3 : TEXCOORD3;
				float4 texcoord4 : TEXCOORD4;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float _StereoCameraEyeOffset;
			float2 _CustomFogTextureToScreenRatio;
			float2 _GlobalBlueNoiseParams;
			float _GlobalRandomValue;
			float3 _PeakOffset;
			float _SpectrogramData[64];
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
                tmp0.x = v.texcoord.x * 63.0;
                tmp0.x = 1.0 - _SpectrogramData[tmp0.x];
                tmp0.xyz = tmp0.xxx * _PeakOffset;
                tmp0.xyz = -tmp0.xyz * v.texcoord.yyy + v.vertex.xyz;
                tmp1 = tmp0.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp1 = unity_ObjectToWorld._m00_m10_m20_m30 * tmp0.xxxx + tmp1;
                tmp0 = unity_ObjectToWorld._m02_m12_m22_m32 * tmp0.zzzz + tmp1;
                tmp1 = tmp0 + unity_ObjectToWorld._m03_m13_m23_m33;
                o.texcoord1.xyz = unity_ObjectToWorld._m03_m13_m23 * v.vertex.www + tmp0.xyz;
                tmp0 = tmp1.yyyy * unity_MatrixVP._m01_m11_m21_m31;
                tmp0 = unity_MatrixVP._m00_m10_m20_m30 * tmp1.xxxx + tmp0;
                tmp0 = unity_MatrixVP._m02_m12_m22_m32 * tmp1.zzzz + tmp0;
                tmp0 = unity_MatrixVP._m03_m13_m23_m33 * tmp1.wwww + tmp0;
                o.position = tmp0;
                tmp1.x = dot(v.normal.xyz, unity_WorldToObject._m00_m10_m20);
                tmp1.y = dot(v.normal.xyz, unity_WorldToObject._m01_m11_m21);
                tmp1.z = dot(v.normal.xyz, unity_WorldToObject._m02_m12_m22);
                tmp1.w = dot(tmp1.xyz, tmp1.xyz);
                tmp1.w = rsqrt(tmp1.w);
                o.texcoord2.xyz = tmp1.www * tmp1.xyz;
                tmp1.x = floor(unity_StereoEyeIndex);
                tmp1.y = _StereoCameraEyeOffset + _StereoCameraEyeOffset;
                tmp1.x = tmp1.x * tmp1.y + -_StereoCameraEyeOffset;
                tmp2.xyw = tmp0.yxw * float3(0.5, 0.5, 0.5);
                tmp2.z = tmp2.x * _ProjectionParams.x;
                tmp2.yz = tmp0.ww * float2(0.5, 0.5) + tmp2.yz;
                tmp2.x = tmp0.w * tmp1.x + tmp2.y;
                tmp0.xy = -tmp0.ww * float2(0.5, 0.5) + tmp2.xz;
                o.texcoord3.xy = tmp0.xy * _CustomFogTextureToScreenRatio + tmp2.ww;
                tmp0.x = tmp0.w * _GlobalRandomValue;
                tmp0.xy = tmp2.xz * _GlobalBlueNoiseParams + tmp0.xx;
                o.texcoord4.xy = tmp0.xy + unity_ObjectToWorld._m03_m13;
                o.texcoord3.zw = tmp0.zw;
                o.texcoord4.zw = tmp0.zw;
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
	Fallback "Diffuse"
}