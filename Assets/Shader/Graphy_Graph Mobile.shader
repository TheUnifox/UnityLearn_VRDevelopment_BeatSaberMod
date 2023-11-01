Shader "Graphy/Graph Mobile" {
	Properties {
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Vector) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		_GoodColor ("Good Color", Vector) = (1,1,1,1)
		_CautionColor ("Caution Color", Vector) = (1,1,1,1)
		_CriticalColor ("Critical Color", Vector) = (1,1,1,1)
		_GoodThreshold ("Good Threshold", Float) = 0.5
		_CautionThreshold ("Caution Threshold", Float) = 0.25
	}
	SubShader {
		Tags { "CanUseSpriteAtlas" = "true" "IGNOREPROJECTOR" = "true" "PreviewType" = "Plane" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		Pass {
			Name "Default"
			Tags { "CanUseSpriteAtlas" = "true" "IGNOREPROJECTOR" = "true" "PreviewType" = "Plane" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			Blend One OneMinusSrcAlpha, One OneMinusSrcAlpha
			ZWrite Off
			Cull Off
			GpuProgramID 25574
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float4 color : COLOR0;
				float2 texcoord : TEXCOORD0;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float4 _Color;
			// $Globals ConstantBuffers for Fragment Shader
			float4 _GoodColor;
			float4 _CautionColor;
			float4 _CriticalColor;
			float _GoodThreshold;
			float _CautionThreshold;
			float Average;
			float GraphValues[128];
			float GraphValues_Length;
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
                float4 tmp1;
                tmp0 = v.vertex.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp0 = unity_ObjectToWorld._m00_m10_m20_m30 * v.vertex.xxxx + tmp0;
                tmp0 = unity_ObjectToWorld._m02_m12_m22_m32 * v.vertex.zzzz + tmp0;
                tmp0 = tmp0 + unity_ObjectToWorld._m03_m13_m23_m33;
                tmp1 = tmp0.yyyy * unity_MatrixVP._m01_m11_m21_m31;
                tmp1 = unity_MatrixVP._m00_m10_m20_m30 * tmp0.xxxx + tmp1;
                tmp1 = unity_MatrixVP._m02_m12_m22_m32 * tmp0.zzzz + tmp1;
                o.position = unity_MatrixVP._m03_m13_m23_m33 * tmp0.wwww + tmp1;
                o.color = v.color * _Color;
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
                float4 tmp3;
                tmp0 = inp.color * _GoodColor;
                tmp1 = inp.color * _CautionColor;
                tmp2 = inp.color * _CriticalColor;
                tmp3.x = inp.texcoord.x * GraphValues_Length;
                tmp3.x = floor(tmp3.x);
                tmp3.yz = float2(_GoodThreshold.x, _CautionThreshold.x) < GraphValues[tmp3.x].xx;
                tmp1 = tmp3.zzzz ? tmp1 : tmp2;
                tmp0 = tmp3.yyyy ? tmp0 : tmp1;
                tmp1.x = inp.texcoord.y * 0.3;
                tmp1.x = tmp1.x / GraphValues[tmp3.x];
                tmp1.x = tmp0.w * tmp1.x;
                tmp1.y = GraphValues[tmp3.x] - inp.texcoord.y;
                tmp1.z = GraphValues[tmp3.x] < inp.texcoord.y;
                tmp1.w = GraphValues_Length - 1.0;
                tmp1.w = 1.0 / tmp1.w;
                tmp1.w = tmp1.w * 4.0;
                tmp1.y = tmp1.w < tmp1.y;
                tmp1.x = tmp1.y ? tmp1.x : tmp0.w;
                tmp0.w = tmp1.z ? 0.0 : tmp1.x;
                tmp1.xyz = float3(_GoodThreshold.x, _CautionThreshold.x, Average.x) - float3(0.02, 0.02, 0.02);
                tmp1.xyz = tmp1.xyz < inp.texcoord.yyy;
                tmp2.xyz = inp.texcoord.yyy < float3(_GoodThreshold.x, _CautionThreshold.x, Average.x);
                tmp1.xyz = tmp1.xyz ? tmp2.xyz : 0.0;
                tmp0 = tmp1.xxxx ? float4(1.0, 1.0, 1.0, 1.0) : tmp0;
                tmp0 = tmp1.yyyy ? _CautionColor : tmp0;
                tmp0 = tmp1.zzzz ? _GoodColor : tmp0;
                tmp1.xy = float2(0.03, 1.0) - inp.texcoord.xx;
                tmp1.y = tmp1.y * 33.33334;
                tmp1.x = -tmp1.x * 33.33334 + 1.0;
                tmp1.xy = tmp0.ww * tmp1.xy;
                tmp1.z = inp.texcoord.x > 0.97;
                tmp1.y = tmp1.z ? tmp1.y : tmp0.w;
                tmp1.z = inp.texcoord.x < 0.03;
                tmp0.w = tmp1.z ? tmp1.x : tmp1.y;
                tmp1 = tex2D(_MainTex, inp.texcoord.xy);
                tmp0 = tmp0 * tmp1;
                o.sv_target.xyz = tmp0.www * tmp0.xyz;
                o.sv_target.w = tmp0.w;
                return o;
			}
			ENDCG
		}
	}
}