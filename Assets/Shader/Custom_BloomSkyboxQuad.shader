Shader "Custom/BloomSkyboxQuad" {
	Properties {
	}
	SubShader {
		Tags { "QUEUE" = "Geometry" "RenderType" = "Opaque" }
		Pass {
			Tags { "QUEUE" = "Geometry" "RenderType" = "Opaque" }
			ZWrite Off
			Cull Off
			GpuProgramID 63534
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float4 texcoord : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
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
                o.position.xy = v.vertex.xy;
                o.position.zw = float2(0.0, 1.0);
                tmp0.x = floor(unity_StereoEyeIndex);
                tmp0.y = _StereoCameraEyeOffset + _StereoCameraEyeOffset;
                tmp0.x = tmp0.x * tmp0.y + -_StereoCameraEyeOffset;
                tmp0.y = v.vertex.x * 0.5 + 0.5;
                tmp0.x = tmp0.x + tmp0.y;
                tmp0.z = v.vertex.y * 0.5;
                tmp0.y = tmp0.z * _ProjectionParams.x;
                tmp0.w = tmp0.z * _ProjectionParams.x + 0.5;
                tmp0.zw = tmp0.xw * _GlobalBlueNoiseParams + _GlobalRandomValue.xx;
                tmp0.xy = tmp0.xy - float2(0.5, -0.0);
                o.texcoord.xy = tmp0.xy * _CustomFogTextureToScreenRatio + float2(0.5, 0.5);
                o.texcoord1.xy = tmp0.zw + unity_ObjectToWorld._m03_m13;
                o.texcoord.zw = float2(0.0, 1.0);
                o.texcoord1.zw = float2(0.0, 1.0);
                return o;
			}
			// Keywords: 
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                tmp0.xy = inp.texcoord1.xy / inp.texcoord1.ww;
                tmp0 = tex2D(_GlobalBlueNoiseTex, tmp0.xy);
                tmp0.x = tmp0.x - 0.5;
                o.sv_target.xyz = tmp0.xxx * float3(0.0039216, 0.0039216, 0.0039216) + float3(0.1, 0.1, 0.1);
                o.sv_target.w = 0.0;
                return o;
			}
			ENDCG
		}
	}
}