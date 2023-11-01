Shader "Hidden/LIV_ClipPlaneSimpleDebug" {
	Properties {
	}
	SubShader {
		Tags { "FORCENOSHADOWCASTING" = "true" "IGNOREPROJECTOR" = "true" "LIGHTMODE" = "ALWAYS" "QUEUE" = "Overlay" }
		Pass {
			Name "CLIP_PLANE_SIMPLE_DEBUG"
			Tags { "FORCENOSHADOWCASTING" = "true" "IGNOREPROJECTOR" = "true" "LIGHTMODE" = "ALWAYS" "QUEUE" = "Overlay" }
			Cull Off
			Fog {
				Mode 0
			}
			GpuProgramID 37195
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 sv_position : SV_Position0;
				float2 texcoord : TEXCOORD0;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
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
                tmp0 = v.vertex.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp0 = unity_ObjectToWorld._m00_m10_m20_m30 * v.vertex.xxxx + tmp0;
                tmp0 = unity_ObjectToWorld._m02_m12_m22_m32 * v.vertex.zzzz + tmp0;
                tmp0 = tmp0 + unity_ObjectToWorld._m03_m13_m23_m33;
                tmp1 = tmp0.yyyy * unity_MatrixVP._m01_m11_m21_m31;
                tmp1 = unity_MatrixVP._m00_m10_m20_m30 * tmp0.xxxx + tmp1;
                tmp1 = unity_MatrixVP._m02_m12_m22_m32 * tmp0.zzzz + tmp1;
                o.sv_position = unity_MatrixVP._m03_m13_m23_m33 * tmp0.wwww + tmp1;
                o.texcoord.xy = v.texcoord.xy;
                return o;
			}
			// Keywords: 
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                float4 tmp1;
                float4 tmp2;
                tmp0.xy = ddx_coarse(inp.texcoord1.xy);
                tmp1.xy = ddy_coarse(inp.texcoord1.xy);
                tmp0.w = 1.0 - inp.texcoord1.x;
                tmp2.z = tmp0.w - inp.texcoord1.y;
                tmp0.z = ddx_coarse(tmp2.z);
                tmp1.z = ddy_coarse(tmp2.z);
                tmp0.xyz = abs(tmp0.xyz) + abs(tmp1.xyz);
                tmp0.xyz = float3(1.0, 1.0, 1.0) / tmp0.xyz;
                tmp2.xy = inp.texcoord1.xy;
                tmp0.xyz = saturate(tmp0.xyz * tmp2.xyz);
                tmp1.xyz = tmp0.xyz * float3(-2.0, -2.0, -2.0) + float3(3.0, 3.0, 3.0);
                tmp0.xyz = tmp0.xyz * tmp0.xyz;
                tmp0.xyz = tmp0.xyz * tmp1.xyz;
                tmp0.y = min(tmp0.z, tmp0.y);
                tmp0.x = min(tmp0.y, tmp0.x);
                o.sv_target = tmp0.xxxx * float4(0.0, 1.0, 0.0, 0.0) + float4(0.0, 0.0, 0.0, 0.5);
                return o;
			}
			ENDCG
		}
	}
}