Shader "Custom/Beatmap Editor/BeatmapObject" {
	Properties {
		_FinalColorMul ("Color Multiplier", Float) = 1
		_DitherTex ("Dither Texture", 2D) = "white" {}
		[Space(12)] _EnvironmentReflectionCube ("Environment Reflection", Cube) = "" {}
		[Toggle(FLIP_WORLD_NORMAL_Y)] _FlipWorldNormalY ("Flip World Normal Y", Float) = 0
		[Space(12)] [ToggleHeader(ENABLE_FOG)] _EnableFog ("Enable Fog", Float) = 0
		[ShowIfAny(ENABLE_FOG)] _FogStartOffset ("Fog Start Offset", Float) = 0
		[ShowIfAny(ENABLE_FOG)] _FogScale ("Fog Scale", Float) = 1
		[Space(16)] [Enum(UnityEngine.Rendering.CullMode)] _CullMode ("Cull Mode", Float) = 0
		[Space(16)] _StencilRefValue ("Stencil Ref Value", Float) = 0
		[Enum(UnityEngine.Rendering.CompareFunction)] _StencilComp ("Stencil Comp Func", Float) = 8
		[Enum(UnityEngine.Rendering.StencilOp)] _StencilPass ("Stencill Pass Op", Float) = 0
		[Space(16)] [Enum(UnityEngine.Rendering.BlendMode)] _BlendSrcFactor ("Blend Src", Float) = 1
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendDstFactor ("Blend Dst", Float) = 0
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendSrcFactorA ("Blend Src Factor A", Float) = 0
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendDstFactorA ("Blend Dst Factor A", Float) = 0
	}
	SubShader {
		Tags { "RenderType" = "Transparent" }
		Pass {
			Tags { "RenderType" = "Transparent" }
			Blend Zero Zero, Zero Zero
			Cull Off
			Stencil {
				Comp Always
				Pass Keep
				Fail Keep
				ZFail Keep
			}
			GpuProgramID 30774
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float2 texcoord : TEXCOORD0;
				float texcoord2 : TEXCOORD2;
				float4 position : SV_POSITION0;
				float3 texcoord1 : TEXCOORD1;
				float3 texcoord3 : TEXCOORD3;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float4x4 _SpawnRotation;
			float4 _DitherTex_ST;
			// $Globals ConstantBuffers for Fragment Shader
			float _FinalColorMul;
			float _Smoothness;
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			CBUFFER_START(Props)
				float4 _Color;
				float _Highlight;
				float _Dim;
			CBUFFER_END
			// Texture params for Vertex Shader
			// Texture params for Fragment Shader
			samplerCUBE _EnvironmentReflectionCube;
			sampler2D _DitherTex;
			
			// Keywords: 
			v2f vert(appdata_full v)
			{
                v2f o;
                float4 tmp0;
                float4 tmp1;
                float4 tmp2;
                o.texcoord.xy = v.texcoord.xy * _DitherTex_ST.xy + _DitherTex_ST.zw;
                tmp0 = v.vertex.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp0 = unity_ObjectToWorld._m00_m10_m20_m30 * v.vertex.xxxx + tmp0;
                tmp0 = unity_ObjectToWorld._m02_m12_m22_m32 * v.vertex.zzzz + tmp0;
                tmp1.xyz = unity_ObjectToWorld._m03_m13_m23 * v.vertex.www + tmp0.xyz;
                tmp0 = tmp0 + unity_ObjectToWorld._m03_m13_m23_m33;
                tmp2.xyz = tmp1.xyz - _WorldSpaceCameraPos;
                tmp1.w = dot(tmp2.xyz, tmp2.xyz);
                o.texcoord2.x = sqrt(tmp1.w);
                tmp2 = tmp0.yyyy * unity_MatrixVP._m01_m11_m21_m31;
                tmp2 = unity_MatrixVP._m00_m10_m20_m30 * tmp0.xxxx + tmp2;
                tmp2 = unity_MatrixVP._m02_m12_m22_m32 * tmp0.zzzz + tmp2;
                o.position = unity_MatrixVP._m03_m13_m23_m33 * tmp0.wwww + tmp2;
                tmp0.xyz = _WorldSpaceCameraPos - tmp1.xyz;
                o.texcoord3.xyz = tmp1.xyz;
                tmp0.w = dot(tmp0.xyz, tmp0.xyz);
                tmp0.w = rsqrt(tmp0.w);
                tmp0.xyz = tmp0.www * tmp0.xyz;
                tmp1.x = dot(v.normal.xyz, unity_WorldToObject._m00_m10_m20);
                tmp1.y = dot(v.normal.xyz, unity_WorldToObject._m01_m11_m21);
                tmp1.z = dot(v.normal.xyz, unity_WorldToObject._m02_m12_m22);
                tmp0.w = dot(tmp1.xyz, tmp1.xyz);
                tmp0.w = rsqrt(tmp0.w);
                tmp1.xyz = tmp0.www * tmp1.xyz;
                tmp0.w = dot(-tmp0.xyz, tmp1.xyz);
                tmp0.w = tmp0.w + tmp0.w;
                tmp0.xyz = tmp1.xyz * -tmp0.www + -tmp0.xyz;
                tmp1.xyz = tmp0.yyy * _SpawnRotation._m01_m11_m21;
                tmp0.xyw = _SpawnRotation._m00_m10_m20 * tmp0.xxx + tmp1.xyz;
                o.texcoord1.xyz = _SpawnRotation._m02_m12_m22 * tmp0.zzz + tmp0.xyw;
                return o;
			}
			// Keywords: 
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                float4 tmp1;
                float4 tmp2;
                tmp0 = tex2D(_DitherTex, inp.texcoord.xy);
                tmp0.x = tmp0.x - 0.5;
                tmp0.x = tmp0.x * _Dim;
                tmp0.x = tmp0.x < 0.0;
                if (tmp0.x) {
                    discard;
                }
                tmp0.x = 1.0 - _Smoothness;
                tmp0.y = -tmp0.x * 0.7 + 1.7;
                tmp0.x = tmp0.y * tmp0.x;
                tmp0.x = tmp0.x * 6.0;
                tmp0 = texCUBElod(_EnvironmentReflectionCube, float4(inp.texcoord1.xyz, tmp0.x));
                tmp0.w = abs(inp.texcoord3.z) <= 0.01;
                tmp0.w = tmp0.w ? 1.0 : 0.0;
                tmp0.w = tmp0.w * _Highlight;
                tmp1.xyz = tmp0.www * float3(0.0, 0.78, 0.9);
                tmp2.xyz = _FinalColorMul.xxx * _Color.xyz;
                o.sv_target.xyz = saturate(tmp0.xyz * tmp2.xyz + tmp1.xyz);
                o.sv_target.w = _Color.w;
                return o;
			}
			ENDCG
		}
	}
}