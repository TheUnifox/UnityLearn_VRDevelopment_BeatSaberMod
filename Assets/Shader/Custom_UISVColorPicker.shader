Shader "Custom/UISVColorPicker" {
	Properties {
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Vector) = (1,1,1,1)
		[Space] [Enum(UnityEngine.Rendering.BlendMode)] _BlendSrcFactor ("Blend Src", Float) = 0
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendDstFactor ("Blend Dst", Float) = 0
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendSrcFactorA ("Blend Src Factor A", Float) = 0
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendDstFactorA ("Blend Dst Factor A", Float) = 10
		[HideInInspector] _StencilComp ("Stencil Comparison", Float) = 8
		[HideInInspector] _Stencil ("Stencil ID", Float) = 0
		[HideInInspector] _StencilOp ("Stencil Operation", Float) = 0
		[HideInInspector] _StencilWriteMask ("Stencil Write Mask", Float) = 255
		[HideInInspector] _StencilReadMask ("Stencil Read Mask", Float) = 255
		[HideInInspector] _ColorMask ("Color Mask", Float) = 15
		[Toggle(UNITY_UI_ALPHACLIP)] [HideInInspector] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
	}
	SubShader {
		Tags { "CanUseSpriteAtlas" = "true" "IGNOREPROJECTOR" = "true" "PreviewType" = "Plane" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		Pass {
			Name "Default"
			Tags { "CanUseSpriteAtlas" = "true" "IGNOREPROJECTOR" = "true" "PreviewType" = "Plane" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			Blend Zero Zero, Zero Zero
			ColorMask 0 -1
			ZWrite Off
			Cull Off
			Stencil {
				ReadMask 0
				WriteMask 0
				Comp Always
				Pass Keep
				Fail Keep
				ZFail Keep
			}
			GpuProgramID 23816
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float4 color : COLOR0;
				float2 texcoord : TEXCOORD0;
				float2 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float4 _Color;
			float4 _MainTex_ST;
			// $Globals ConstantBuffers for Fragment Shader
			float4 _TextureSampleAdd;
			float4 _ClipRect;
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
                tmp0.x = v.vertex.x / v.texcoord2.x;
                tmp0.x = sin(tmp0.x);
                tmp1.x = cos(tmp0.x);
                tmp0.y = tmp1.x * v.texcoord2.x + -v.texcoord2.x;
                tmp1.x = tmp0.x * v.texcoord2.x;
                tmp1.y = tmp0.y + v.vertex.z;
                tmp0.x = v.texcoord2.x == 0.0;
                tmp0.xy = tmp0.xx ? v.vertex.xz : tmp1.xy;
                tmp1 = v.vertex.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp1 = unity_ObjectToWorld._m00_m10_m20_m30 * tmp0.xxxx + tmp1;
                tmp0 = unity_ObjectToWorld._m02_m12_m22_m32 * tmp0.yyyy + tmp1;
                tmp0 = tmp0 + unity_ObjectToWorld._m03_m13_m23_m33;
                tmp1 = tmp0.yyyy * unity_MatrixVP._m01_m11_m21_m31;
                tmp1 = unity_MatrixVP._m00_m10_m20_m30 * tmp0.xxxx + tmp1;
                tmp1 = unity_MatrixVP._m02_m12_m22_m32 * tmp0.zzzz + tmp1;
                o.position = unity_MatrixVP._m03_m13_m23_m33 * tmp0.wwww + tmp1;
                o.color = v.color * _Color;
                o.texcoord.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
                o.texcoord1.xy = v.texcoord1.xy;
                o.texcoord2 = v.vertex;
                return o;
			}
			// Keywords: 
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                float4 tmp1;
                tmp0.xy = inp.texcoord2.xy >= _ClipRect.xy;
                tmp0.zw = _ClipRect.zw >= inp.texcoord2.xy;
                tmp0 = tmp0 ? 1.0 : 0.0;
                tmp0.xy = tmp0.zw * tmp0.xy;
                tmp0.x = tmp0.y * tmp0.x;
                tmp1 = tex2D(_MainTex, inp.texcoord.xy);
                tmp1 = tmp1 + _TextureSampleAdd;
                tmp0.y = tmp1.w * inp.color.w;
                tmp1.xyz = tmp1.xyz * inp.color.xyz + float3(-1.0, -1.0, -1.0);
                tmp1.xyz = inp.texcoord1.xxx * tmp1.xyz + float3(1.0, 1.0, 1.0);
                tmp1.xyz = tmp1.xyz * inp.texcoord1.yyy;
                tmp1.xyz = log(tmp1.xyz);
                tmp1.xyz = tmp1.xyz * float3(2.2, 2.2, 2.2);
                o.sv_target.xyz = exp(tmp1.xyz);
                o.sv_target.w = tmp0.x * tmp0.y;
                return o;
			}
			ENDCG
		}
	}
}