Shader "Custom/BloomPrePassLine" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		[Space] [Enum(UnityEngine.Rendering.BlendMode)] _BlendSrcFactor ("Blend Src Factor", Float) = 1
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendDstFactor ("Blend Dst Factor", Float) = 10
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendSrcFactorA ("Blend Src Factor A", Float) = 0
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendDstFactorA ("Blend Dst Factor A", Float) = 10
		[Space] [Enum(UnityEngine.Rendering.BlendOp)] _BlendOp ("Blend Operation", Float) = 0
	}
	SubShader {
		Tags { "PreviewType" = "Plane" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		Pass {
			Tags { "PreviewType" = "Plane" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			Blend Zero Zero, Zero Zero
			ZWrite Off
			Cull Off
			GpuProgramID 63289
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float4 color : COLOR0;
				float3 texcoord : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float4x4 _VertexTransformMatrix;
			// $Globals ConstantBuffers for Fragment Shader
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
                tmp0 = v.vertex.yyyy * _VertexTransformMatrix._m01_m11_m21_m31;
                tmp0 = _VertexTransformMatrix._m00_m10_m20_m30 * v.vertex.xxxx + tmp0;
                tmp0 = _VertexTransformMatrix._m02_m12_m22_m32 * v.vertex.zzzz + tmp0;
                o.position = _VertexTransformMatrix._m03_m13_m23_m33 * v.vertex.wwww + tmp0;
                tmp0.xyz = v.color.xyz * float3(0.305306, 0.305306, 0.305306) + float3(0.6821711, 0.6821711, 0.6821711);
                tmp0.xyz = v.color.xyz * tmp0.xyz + float3(0.0125229, 0.0125229, 0.0125229);
                o.color.xyz = tmp0.xyz * v.color.xyz;
                o.color.w = v.color.w;
                o.texcoord.xyz = v.texcoord.xyz;
                o.texcoord1.xyz = v.tangent.xyz / v.tangent.zzz;
                o.texcoord1.w = 1.0 / v.tangent.z;
                return o;
			}
			// Keywords: 
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                float4 tmp1;
                tmp0.x = inp.texcoord.x / inp.texcoord.z;
                tmp0.y = inp.texcoord.y;
                tmp0 = tex2D(_MainTex, tmp0.xy);
                tmp1.w = inp.color.w * inp.color.w;
                tmp1.xyz = inp.color.xyz;
                tmp0 = tmp0 * tmp1;
                o.sv_target.xyz = tmp0.www * tmp0.xyz;
                o.sv_target.w = tmp0.w;
                return o;
			}
			ENDCG
		}
	}
}