Shader "Custom/Mirror" {
	Properties {
		_NormalTex ("Normal Texture", 2D) = "white" {}
		_BumpIntensity ("Bump Intensity", Float) = 0.1
		_ReflectionIntensity ("Reflection Intensity", Float) = 0.5
		_TextureScrolling ("Texture Scrolling", Vector) = (0,0,0,0)
		[Space] _Metallic ("Metallic", Range(0, 1)) = 0
		_Smoothness ("Smoothness", Range(0, 1)) = 0.5
		[ToggleHeader(DETAIL_NORMAL_MAP)] _DetailNormalMap ("Detail Normal Map", Float) = 0
		[ShowIfAny(DETAIL_NORMAL_MAP)] _DetailNormalTextureScale ("Detail Normal Texture Scale", Float) = 1
		[ShowIfAny(DETAIL_NORMAL_MAP)] _DetailNormalIntensity ("Detail Normal Intensity", Float) = 0
		[ShowIfAny(DETAIL_NORMAL_MAP)] _DetailNormalTexScrolling ("Detail Scrolling", Vector) = (0.05,2,0,0)
		[Space(18)] [ToggleHeader(LIGHTMAP)] _EnableLightmap ("Enable Lightmap", Float) = 0
		[Space(12)] [ToggleHeader(DIFFUSE)] _EnableDiffuse ("Enable Diffuse", Float) = 0
		[ToggleShowIfAny(LIGHT_FALLOFF, DIFFUSE, SPECULAR)] _EnableLightFalloff ("Enable Light Falloff", Float) = 0
		[Space(12)] [ToggleHeader(SPECULAR)] _EnableSpecular ("Enable Specular", Float) = 0
		[ShowIfAny(SPECULAR)] _SpecularIntensity ("Specular Intensity", Float) = 1
		[Space(18)] [ToggleHeader(ENABLE_DIRT)] _EnableDirt ("Enable Dirt", Float) = 0
		[ShowIfAny(ENABLE_DIRT)] _DirtTex ("Dirt Texture", 2D) = "white" {}
		[ShowIfAny(ENABLE_DIRT)] _DirtIntensity ("Dirt Intensity", Float) = 1
		[Space(18)] _TintColor ("Tint Color", Vector) = (1,1,1,1)
		[Space] _FogStartOffset ("Fog Start Offset", Float) = 0
		_FogScale ("Fog Scale", Float) = 1
		[PerRendererData] _ReflectionTex ("Reflection Texture", 2D) = "white" {}
		[Space(12)] _StencilRefValue ("Stencil Ref Value", Float) = 0
		[Enum(UnityEngine.Rendering.CompareFunction)] _StencilComp ("Stencil Comp Func", Float) = 8
		[Enum(UnityEngine.Rendering.StencilOp)] _StencilPass ("Stencill Pass Op", Float) = 1
	}
	SubShader {
		Tags { "RenderType" = "Mirror" }
		Pass {
			Tags { "LIGHTMODE" = "FORWARDBASE" "RenderType" = "Mirror" }
			Stencil {
				Comp Always
				Pass Keep
				Fail Keep
				ZFail Keep
			}
			GpuProgramID 21391
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float4 texcoord1 : TEXCOORD1;
				float3 texcoord2 : TEXCOORD2;
				float2 texcoord3 : TEXCOORD3;
				float4 texcoord4 : TEXCOORD4;
				float4 texcoord5 : TEXCOORD5;
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
			float4 _NormalTex_ST;
			float2 _TextureScrolling;
			// $Globals ConstantBuffers for Fragment Shader
			float _BumpIntensity;
			float _ReflectionIntensity;
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			// Texture params for Vertex Shader
			// Texture params for Fragment Shader
			sampler2D _NormalTex;
			sampler2D _ReflectionTex;
			sampler2D _GlobalBlueNoiseTex;
			
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
                o.texcoord2.xyz = unity_ObjectToWorld._m03_m13_m23 * v.vertex.www + tmp0.xyz;
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
                o.texcoord1.xy = tmp0.xy * _CustomFogTextureToScreenRatio + tmp2.ww;
                tmp0.x = tmp0.w * _GlobalRandomValue;
                tmp0.xy = tmp2.xz * _GlobalBlueNoiseParams + tmp0.xx;
                o.texcoord4.xy = tmp2.yz;
                o.texcoord5.xy = tmp0.xy + unity_ObjectToWorld._m03_m13;
                o.texcoord1.zw = tmp0.zw;
                tmp0.xy = v.texcoord.xy * _NormalTex_ST.xy + _NormalTex_ST.zw;
                o.texcoord3.xy = _TextureScrolling * _Time.xx + tmp0.xy;
                o.texcoord4.zw = tmp0.zw;
                o.texcoord5.zw = tmp0.zw;
                return o;
			}
			// Keywords: 
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                float4 tmp1;
                tmp0 = tex2D(_NormalTex, inp.texcoord3.xy);
                tmp0.x = tmp0.w * tmp0.x;
                tmp0.xy = tmp0.xy * float2(2.0, 2.0) + float2(-1.0, -1.0);
                tmp0.xy = tmp0.xy * _BumpIntensity.xx;
                tmp1.xyz = inp.texcoord2.xyz - _WorldSpaceCameraPos;
                tmp0.z = dot(tmp1.xyz, tmp1.xyz);
                tmp0.z = sqrt(tmp0.z);
                tmp0.z = tmp1.y / tmp0.z;
                tmp1.xy = inp.texcoord4.xy / inp.texcoord4.ww;
                tmp0.xy = tmp0.xy * -tmp0.zz + tmp1.xy;
                tmp0 = tex2D(_ReflectionTex, tmp0.xy);
                tmp1.x = _ReflectionIntensity * _ReflectionIntensity;
                tmp0 = tmp0 * tmp1.xxxx;
                tmp1.xy = inp.texcoord5.xy / inp.texcoord5.ww;
                tmp1 = tex2D(_GlobalBlueNoiseTex, tmp1.xy);
                tmp1.x = tmp1.x - 0.5;
                o.sv_target.xyz = tmp1.xxx * float3(0.0039216, 0.0039216, 0.0039216) + tmp0.xyz;
                o.sv_target.w = tmp0.w;
                return o;
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
}