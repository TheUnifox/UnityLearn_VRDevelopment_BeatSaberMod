Shader "Custom/OpaqueNeonLight" {
	Properties {
		_FogStartOffset ("Fog Start Offset", Float) = 1
		_FogScale ("Fog Scale", Float) = 1
		[Toggle(ENABLE_HEIGHT_FOG)] _EnableHeightFog ("Enable Height Fog", Float) = 1
		[ShowIfAny(ENABLE_HEIGHT_FOG)] _FogHeightScale ("Fog Height Scale", Float) = 1
		[ShowIfAny(ENABLE_HEIGHT_FOG)] _FogHeightOffset ("Fog Height Offset", Float) = 0
	}
	SubShader {
		Tags { "Batching" = "False" "QUEUE" = "Geometry" "RenderType" = "Opaque" }
		Pass {
			Tags { "Batching" = "False" "QUEUE" = "Geometry" "RenderType" = "Opaque" }
			GpuProgramID 6888
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float4 texcoord1 : TEXCOORD1;
				float3 texcoord2 : TEXCOORD2;
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
			// $Globals ConstantBuffers for Fragment Shader
			float _BaseColorBoost;
			float _BaseColorBoostThreshold;
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			CBUFFER_START(Props)
				float4 _Color;
			CBUFFER_END
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
                o.texcoord4.xy = tmp0.xy + unity_ObjectToWorld._m03_m13;
                o.texcoord1.zw = tmp0.zw;
                o.texcoord4.zw = tmp0.zw;
                return o;
			}
			// Keywords: 
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                float4 tmp1;
                tmp0.xy = inp.texcoord4.xy / inp.texcoord4.ww;
                tmp0 = tex2D(_GlobalBlueNoiseTex, tmp0.xy);
                tmp0.x = tmp0.x - 0.5;
                tmp0.y = _Color.w * _Color.w;
                tmp0.z = tmp0.y * tmp0.y;
                tmp0.z = tmp0.z * _BaseColorBoost + -_BaseColorBoostThreshold;
                tmp1.xyz = saturate(_Color.xyz * tmp0.yyy + tmp0.zzz);
                o.sv_target.w = tmp0.y;
                tmp0.xyz = tmp0.xxx * float3(0.0039216, 0.0039216, 0.0039216) + tmp1.xyz;
                o.sv_target.xyz = tmp0.xyz + tmp0.xyz;
                return o;
			}
			ENDCG
		}
	}
}