Shader "Hidden/LIV_WriteOpaqueToAlpha" {
	Properties {
	}
	SubShader {
		Tags { "FORCENOSHADOWCASTING" = "true" "IGNOREPROJECTOR" = "true" "LIGHTMODE" = "ALWAYS" "QUEUE" = "Overlay" }
		Pass {
			Name "CLIP_PLANE_FIX_ALPHA"
			Tags { "FORCENOSHADOWCASTING" = "true" "IGNOREPROJECTOR" = "true" "LIGHTMODE" = "ALWAYS" "QUEUE" = "Overlay" }
			ColorMask A -1
			ZTest Greater
			ZWrite Off
			Cull Off
			Fog {
				Mode 0
			}
			GpuProgramID 27304
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
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
                o.position.xy = v.vertex.xy * float2(2.0, 2.0);
                o.position.zw = float2(0.0, 1.0);
                return o;
			}
			// Keywords: 
			fout frag(v2f inp)
			{
                fout o;
                o.sv_target = float4(1.0, 1.0, 1.0, 1.0);
                return o;
			}
			ENDCG
		}
	}
}