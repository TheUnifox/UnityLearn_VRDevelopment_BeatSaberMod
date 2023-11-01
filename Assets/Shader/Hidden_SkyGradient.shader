Shader "Hidden/SkyGradient" {
	Properties {
		[PerRendererData] _GradientTex ("Texture", 2D) = "white" {}
		[PerRendererData] _Color ("Color", Vector) = (1,1,1,1)
	}
	SubShader {
		Pass {
			Blend One One, One One
			ZTest Always
			ZWrite Off
			Cull Off
			GpuProgramID 45673
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float3 texcoord : TEXCOORD0;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float4x4 _InverseProjectionMatrix;
			float4x4 _CameraToWorldMatrix;
			// $Globals ConstantBuffers for Fragment Shader
			float4 _Color;
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			// Texture params for Vertex Shader
			// Texture params for Fragment Shader
			sampler2D _GradientTex;
			
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
                tmp0.xy = v.vertex.xy * float2(2.0, 2.0) + float2(-1.0, -1.0);
                tmp0.yzw = tmp0.yyy * _InverseProjectionMatrix._m01_m11_m21;
                tmp0.xyz = _InverseProjectionMatrix._m00_m10_m20 * tmp0.xxx + tmp0.yzw;
                tmp0.xyz = tmp0.xyz + _InverseProjectionMatrix._m02_m12_m22;
                tmp0.xyz = tmp0.xyz + _InverseProjectionMatrix._m03_m13_m23;
                tmp1.xyz = tmp0.yyy * _CameraToWorldMatrix._m01_m11_m21;
                tmp0.xyw = _CameraToWorldMatrix._m00_m10_m20 * tmp0.xxx + tmp1.xyz;
                o.texcoord.xyz = _CameraToWorldMatrix._m02_m12_m22 * tmp0.zzz + tmp0.xyw;
                return o;
			}
			// Keywords: 
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                tmp0.x = dot(inp.texcoord.xyz, inp.texcoord.xyz);
                tmp0.x = rsqrt(tmp0.x);
                tmp0.x = tmp0.x * inp.texcoord.y;
                tmp0.x = tmp0.x * 0.5 + 0.5;
                tmp0.y = 0.0;
                tmp0 = tex2D(_GradientTex, tmp0.xy);
                o.sv_target = tmp0 * _Color;
                return o;
			}
			ENDCG
		}
	}
}