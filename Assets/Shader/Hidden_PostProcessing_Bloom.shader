Shader "Hidden/PostProcessing/Bloom" {
	Properties {
		[HideInInspector] _MainTex ("Main Texture", 2D) = "white" {}
	}
	SubShader {
		Pass {
			ZTest Always
			ZWrite Off
			Cull Off
			GpuProgramID 17891
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float2 texcoord : TEXCOORD0;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float4 _MainTex_ST;
			// $Globals ConstantBuffers for Fragment Shader
			float4 _MainTex_TexelSize;
			float _AlphaWeights;
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
                o.texcoord.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
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
                float4 tmp4;
                float4 tmp5;
                tmp0 = _MainTex_TexelSize * float4(-0.5, -0.5, 0.5, -0.5) + inp.texcoord.xyxy;
                tmp1 = tex2D(_MainTex, tmp0.xy);
                tmp0 = tex2D(_MainTex, tmp0.zw);
                tmp0 = tmp0 + tmp1;
                tmp1 = _MainTex_TexelSize * float4(-0.5, 0.5, 0.5, 0.5) + inp.texcoord.xyxy;
                tmp2 = tex2D(_MainTex, tmp1.xy);
                tmp1 = tex2D(_MainTex, tmp1.zw);
                tmp0 = tmp0 + tmp2;
                tmp0 = tmp1 + tmp0;
                tmp1.xy = inp.texcoord.xy - _MainTex_TexelSize.xy;
                tmp1 = tex2D(_MainTex, tmp1.xy);
                tmp2 = _MainTex_TexelSize * float4(0.0, -1.0, 1.0, -1.0) + inp.texcoord.xyxy;
                tmp3 = tex2D(_MainTex, tmp2.xy);
                tmp2 = tex2D(_MainTex, tmp2.zw);
                tmp2 = tmp2 + tmp3;
                tmp1 = tmp1 + tmp3;
                tmp3 = tex2D(_MainTex, inp.texcoord.xy);
                tmp1 = tmp1 + tmp3;
                tmp4 = _MainTex_TexelSize * float4(-1.0, 0.0, 1.0, 0.0) + inp.texcoord.xyxy;
                tmp5 = tex2D(_MainTex, tmp4.xy);
                tmp4 = tex2D(_MainTex, tmp4.zw);
                tmp1 = tmp1 + tmp5;
                tmp5 = tmp3 + tmp5;
                tmp1 = tmp1 * float4(0.03125, 0.03125, 0.03125, 0.03125);
                tmp0 = tmp0 * float4(0.125, 0.125, 0.125, 0.125) + tmp1;
                tmp1 = tmp2 + tmp4;
                tmp2 = tmp3 + tmp4;
                tmp1 = tmp3 + tmp1;
                tmp0 = tmp1 * float4(0.03125, 0.03125, 0.03125, 0.03125) + tmp0;
                tmp1 = _MainTex_TexelSize * float4(-1.0, 1.0, 0.0, 1.0) + inp.texcoord.xyxy;
                tmp3 = tex2D(_MainTex, tmp1.zw);
                tmp1 = tex2D(_MainTex, tmp1.xy);
                tmp4 = tmp3 + tmp5;
                tmp1 = tmp1 + tmp4;
                tmp0 = tmp1 * float4(0.03125, 0.03125, 0.03125, 0.03125) + tmp0;
                tmp1.xy = inp.texcoord.xy + _MainTex_TexelSize.xy;
                tmp1 = tex2D(_MainTex, tmp1.xy);
                tmp1 = tmp1 + tmp2;
                tmp1 = tmp3 + tmp1;
                tmp0 = tmp1 * float4(0.03125, 0.03125, 0.03125, 0.03125) + tmp0;
                tmp1.x = saturate(tmp0.w * _AlphaWeights);
                o.sv_target.xyz = tmp0.xyz * tmp1.xxx;
                o.sv_target.w = tmp0.w;
                return o;
			}
			ENDCG
		}
		Pass {
			ZTest Always
			ZWrite Off
			Cull Off
			GpuProgramID 80595
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float2 texcoord : TEXCOORD0;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float4 _MainTex_ST;
			// $Globals ConstantBuffers for Fragment Shader
			float4 _MainTex_TexelSize;
			float _AlphaWeights;
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
                o.texcoord.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
                return o;
			}
			// Keywords: 
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                float4 tmp1;
                float4 tmp2;
                tmp0 = _MainTex_TexelSize * float4(-1.0, -1.0, 1.0, -1.0) + inp.texcoord.xyxy;
                tmp1 = tex2D(_MainTex, tmp0.xy);
                tmp0 = tex2D(_MainTex, tmp0.zw);
                tmp0 = tmp0 + tmp1;
                tmp1 = _MainTex_TexelSize * float4(-1.0, 1.0, 1.0, 1.0) + inp.texcoord.xyxy;
                tmp2 = tex2D(_MainTex, tmp1.xy);
                tmp1 = tex2D(_MainTex, tmp1.zw);
                tmp0 = tmp0 + tmp2;
                tmp0 = tmp1 + tmp0;
                tmp0 = tmp0 * float4(0.25, 0.25, 0.25, 0.25);
                tmp1.x = saturate(tmp0.w * _AlphaWeights);
                o.sv_target.xyz = tmp0.xyz * tmp1.xxx;
                o.sv_target.w = tmp0.w;
                return o;
			}
			ENDCG
		}
		Pass {
			ZTest Always
			ZWrite Off
			Cull Off
			GpuProgramID 157811
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float2 texcoord : TEXCOORD0;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float4 _MainTex_ST;
			// $Globals ConstantBuffers for Fragment Shader
			float4 _MainTex_TexelSize;
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
                o.texcoord.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
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
                float4 tmp4;
                float4 tmp5;
                tmp0 = _MainTex_TexelSize * float4(-0.5, -0.5, 0.5, -0.5) + inp.texcoord.xyxy;
                tmp1 = tex2D(_MainTex, tmp0.xy);
                tmp0 = tex2D(_MainTex, tmp0.zw);
                tmp0 = tmp0 + tmp1;
                tmp1 = _MainTex_TexelSize * float4(-0.5, 0.5, 0.5, 0.5) + inp.texcoord.xyxy;
                tmp2 = tex2D(_MainTex, tmp1.xy);
                tmp1 = tex2D(_MainTex, tmp1.zw);
                tmp0 = tmp0 + tmp2;
                tmp0 = tmp1 + tmp0;
                tmp1.xy = inp.texcoord.xy - _MainTex_TexelSize.xy;
                tmp1 = tex2D(_MainTex, tmp1.xy);
                tmp2 = _MainTex_TexelSize * float4(0.0, -1.0, 1.0, -1.0) + inp.texcoord.xyxy;
                tmp3 = tex2D(_MainTex, tmp2.xy);
                tmp2 = tex2D(_MainTex, tmp2.zw);
                tmp2 = tmp2 + tmp3;
                tmp1 = tmp1 + tmp3;
                tmp3 = tex2D(_MainTex, inp.texcoord.xy);
                tmp1 = tmp1 + tmp3;
                tmp4 = _MainTex_TexelSize * float4(-1.0, 0.0, 1.0, 0.0) + inp.texcoord.xyxy;
                tmp5 = tex2D(_MainTex, tmp4.xy);
                tmp4 = tex2D(_MainTex, tmp4.zw);
                tmp1 = tmp1 + tmp5;
                tmp5 = tmp3 + tmp5;
                tmp1 = tmp1 * float4(0.03125, 0.03125, 0.03125, 0.03125);
                tmp0 = tmp0 * float4(0.125, 0.125, 0.125, 0.125) + tmp1;
                tmp1 = tmp2 + tmp4;
                tmp2 = tmp3 + tmp4;
                tmp1 = tmp3 + tmp1;
                tmp0 = tmp1 * float4(0.03125, 0.03125, 0.03125, 0.03125) + tmp0;
                tmp1 = _MainTex_TexelSize * float4(-1.0, 1.0, 0.0, 1.0) + inp.texcoord.xyxy;
                tmp3 = tex2D(_MainTex, tmp1.zw);
                tmp1 = tex2D(_MainTex, tmp1.xy);
                tmp4 = tmp3 + tmp5;
                tmp1 = tmp1 + tmp4;
                tmp0 = tmp1 * float4(0.03125, 0.03125, 0.03125, 0.03125) + tmp0;
                tmp1.xy = inp.texcoord.xy + _MainTex_TexelSize.xy;
                tmp1 = tex2D(_MainTex, tmp1.xy);
                tmp1 = tmp1 + tmp2;
                tmp1 = tmp3 + tmp1;
                o.sv_target = tmp1 * float4(0.03125, 0.03125, 0.03125, 0.03125) + tmp0;
                return o;
			}
			ENDCG
		}
		Pass {
			ZTest Always
			ZWrite Off
			Cull Off
			GpuProgramID 214950
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float2 texcoord : TEXCOORD0;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float4 _MainTex_ST;
			// $Globals ConstantBuffers for Fragment Shader
			float4 _MainTex_TexelSize;
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
                o.texcoord.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
                return o;
			}
			// Keywords: 
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                float4 tmp1;
                float4 tmp2;
                tmp0 = _MainTex_TexelSize * float4(-1.0, -1.0, 1.0, -1.0) + inp.texcoord.xyxy;
                tmp1 = tex2D(_MainTex, tmp0.xy);
                tmp0 = tex2D(_MainTex, tmp0.zw);
                tmp0 = tmp0 + tmp1;
                tmp1 = _MainTex_TexelSize * float4(-1.0, 1.0, 1.0, 1.0) + inp.texcoord.xyxy;
                tmp2 = tex2D(_MainTex, tmp1.xy);
                tmp1 = tex2D(_MainTex, tmp1.zw);
                tmp0 = tmp0 + tmp2;
                tmp0 = tmp1 + tmp0;
                o.sv_target = tmp0 * float4(0.25, 0.25, 0.25, 0.25);
                return o;
			}
			ENDCG
		}
		Pass {
			ZTest Always
			ZWrite Off
			Cull Off
			GpuProgramID 326960
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float2 texcoord : TEXCOORD0;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float4 _MainTex_ST;
			// $Globals ConstantBuffers for Fragment Shader
			float4 _MainTex_TexelSize;
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
                o.texcoord.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
                return o;
			}
			// Keywords: 
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                float4 tmp1;
                float4 tmp2;
                tmp0 = _MainTex_TexelSize * float4(-1.0, -1.0, 1.0, -1.0) + inp.texcoord.xyxy;
                tmp1 = tex2D(_MainTex, tmp0.xy);
                tmp0 = tex2D(_MainTex, tmp0.zw);
                tmp0 = tmp0 + tmp1;
                tmp1 = _MainTex_TexelSize * float4(-1.0, 1.0, 1.0, 1.0) + inp.texcoord.xyxy;
                tmp2 = tex2D(_MainTex, tmp1.xy);
                tmp1 = tex2D(_MainTex, tmp1.zw);
                tmp0 = tmp0 + tmp2;
                tmp0 = tmp1 + tmp0;
                tmp1.xyz = tmp0.xyz * float3(0.0763265, 0.0763265, 0.0763265) + float3(0.6821711, 0.6821711, 0.6821711);
                tmp0 = tmp0 * float4(0.25, 0.25, 0.25, 0.25);
                tmp1.xyz = tmp0.xyz * tmp1.xyz + float3(0.0125229, 0.0125229, 0.0125229);
                o.sv_target.xyz = tmp0.xyz * tmp1.xyz;
                o.sv_target.w = tmp0.w;
                return o;
			}
			ENDCG
		}
		Pass {
			ZTest Always
			ZWrite Off
			Cull Off
			GpuProgramID 357596
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float2 texcoord : TEXCOORD0;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float4 _MainTex_ST;
			// $Globals ConstantBuffers for Fragment Shader
			float4 _MainTex_TexelSize;
			float _SampleScale;
			float _CombineSrc;
			float _CombineDst;
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			// Texture params for Vertex Shader
			// Texture params for Fragment Shader
			sampler2D _MainTex;
			sampler2D _BloomTex;
			
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
                o.texcoord.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
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
                float4 tmp4;
                tmp0.x = 1.0;
                tmp0.z = _SampleScale;
                tmp0 = tmp0.xxzz * _MainTex_TexelSize;
                tmp1.zw = float2(-1.0, 0.0);
                tmp1.x = _SampleScale;
                tmp2 = -tmp0.xywy * tmp1.xxwx + inp.texcoord.xyxy;
                tmp3 = tex2D(_MainTex, tmp2.xy);
                tmp2 = tex2D(_MainTex, tmp2.zw);
                tmp2 = tmp2 * float4(2.0, 2.0, 2.0, 2.0) + tmp3;
                tmp3.xy = -tmp0.zy * tmp1.zx + inp.texcoord.xy;
                tmp3 = tex2D(_MainTex, tmp3.xy);
                tmp2 = tmp2 + tmp3;
                tmp3 = tmp0.zwxw * tmp1.zwxw + inp.texcoord.xyxy;
                tmp4 = tmp0.zywy * tmp1.zxwx + inp.texcoord.xyxy;
                tmp0.xy = tmp0.xy * tmp1.xx + inp.texcoord.xy;
                tmp0 = tex2D(_MainTex, tmp0.xy);
                tmp1 = tex2D(_MainTex, tmp3.xy);
                tmp3 = tex2D(_MainTex, tmp3.zw);
                tmp1 = tmp1 * float4(2.0, 2.0, 2.0, 2.0) + tmp2;
                tmp2 = tex2D(_MainTex, inp.texcoord.xy);
                tmp1 = tmp2 * float4(4.0, 4.0, 4.0, 4.0) + tmp1;
                tmp1 = tmp3 * float4(2.0, 2.0, 2.0, 2.0) + tmp1;
                tmp2 = tex2D(_MainTex, tmp4.xy);
                tmp3 = tex2D(_MainTex, tmp4.zw);
                tmp1 = tmp1 + tmp2;
                tmp1 = tmp3 * float4(2.0, 2.0, 2.0, 2.0) + tmp1;
                tmp0 = tmp0 + tmp1;
                tmp0 = tmp0 * float4(0.0625, 0.0625, 0.0625, 0.0625);
                tmp1 = tex2D(_BloomTex, inp.texcoord.xy);
                tmp1 = tmp1 * _CombineSrc.xxxx;
                o.sv_target = tmp0 * _CombineDst.xxxx + tmp1;
                return o;
			}
			ENDCG
		}
		Pass {
			ZTest Always
			ZWrite Off
			Cull Off
			GpuProgramID 445432
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float2 texcoord : TEXCOORD0;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float4 _MainTex_ST;
			// $Globals ConstantBuffers for Fragment Shader
			float4 _MainTex_TexelSize;
			float _SampleScale;
			float _CombineSrc;
			float _CombineDst;
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			// Texture params for Vertex Shader
			// Texture params for Fragment Shader
			sampler2D _MainTex;
			sampler2D _BloomTex;
			
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
                o.texcoord.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
                return o;
			}
			// Keywords: 
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                float4 tmp1;
                float4 tmp2;
                tmp0 = _MainTex_TexelSize * float4(-1.0, -1.0, 1.0, 1.0);
                tmp1.x = _SampleScale * 0.5;
                tmp2 = tmp0.xyzy * tmp1.xxxx + inp.texcoord.xyxy;
                tmp0 = tmp0.xwzw * tmp1.xxxx + inp.texcoord.xyxy;
                tmp1 = tex2D(_MainTex, tmp2.xy);
                tmp2 = tex2D(_MainTex, tmp2.zw);
                tmp1 = tmp1 + tmp2;
                tmp2 = tex2D(_MainTex, tmp0.xy);
                tmp0 = tex2D(_MainTex, tmp0.zw);
                tmp1 = tmp1 + tmp2;
                tmp0 = tmp0 + tmp1;
                tmp0 = tmp0 * float4(0.25, 0.25, 0.25, 0.25);
                tmp1 = tex2D(_BloomTex, inp.texcoord.xy);
                tmp1 = tmp1 * _CombineSrc.xxxx;
                o.sv_target = tmp0 * _CombineDst.xxxx + tmp1;
                return o;
			}
			ENDCG
		}
		Pass {
			ZTest Always
			ZWrite Off
			Cull Off
			GpuProgramID 465785
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float2 texcoord : TEXCOORD0;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float4 _MainTex_ST;
			// $Globals ConstantBuffers for Fragment Shader
			float4 _MainTex_TexelSize;
			float _SampleScale;
			float _CombineSrc;
			float _CombineDst;
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			// Texture params for Vertex Shader
			// Texture params for Fragment Shader
			sampler2D _MainTex;
			sampler2D _BloomTex;
			
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
                o.texcoord.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
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
                float4 tmp4;
                tmp0.x = 1.0;
                tmp0.z = _SampleScale;
                tmp0 = tmp0.xxzz * _MainTex_TexelSize;
                tmp1.zw = float2(-1.0, 0.0);
                tmp1.x = _SampleScale;
                tmp2 = -tmp0.xywy * tmp1.xxwx + inp.texcoord.xyxy;
                tmp3 = tex2D(_MainTex, tmp2.xy);
                tmp2 = tex2D(_MainTex, tmp2.zw);
                tmp2 = tmp2 * float4(2.0, 2.0, 2.0, 2.0) + tmp3;
                tmp3.xy = -tmp0.zy * tmp1.zx + inp.texcoord.xy;
                tmp3 = tex2D(_MainTex, tmp3.xy);
                tmp2 = tmp2 + tmp3;
                tmp3 = tmp0.zwxw * tmp1.zwxw + inp.texcoord.xyxy;
                tmp4 = tmp0.zywy * tmp1.zxwx + inp.texcoord.xyxy;
                tmp0.xy = tmp0.xy * tmp1.xx + inp.texcoord.xy;
                tmp0 = tex2D(_MainTex, tmp0.xy);
                tmp1 = tex2D(_MainTex, tmp3.xy);
                tmp3 = tex2D(_MainTex, tmp3.zw);
                tmp1 = tmp1 * float4(2.0, 2.0, 2.0, 2.0) + tmp2;
                tmp2 = tex2D(_MainTex, inp.texcoord.xy);
                tmp1 = tmp2 * float4(4.0, 4.0, 4.0, 4.0) + tmp1;
                tmp1 = tmp3 * float4(2.0, 2.0, 2.0, 2.0) + tmp1;
                tmp2 = tex2D(_MainTex, tmp4.xy);
                tmp3 = tex2D(_MainTex, tmp4.zw);
                tmp1 = tmp1 + tmp2;
                tmp1 = tmp3 * float4(2.0, 2.0, 2.0, 2.0) + tmp1;
                tmp0 = tmp0 + tmp1;
                tmp0 = tmp0 * float4(0.0625, 0.0625, 0.0625, 0.0625);
                tmp1 = tex2D(_BloomTex, inp.texcoord.xy);
                tmp1 = tmp1 * _CombineSrc.xxxx;
                tmp0 = tmp0 * _CombineDst.xxxx + tmp1;
                tmp1.xyz = tmp0.xyz * float3(0.305306, 0.305306, 0.305306) + float3(0.6821711, 0.6821711, 0.6821711);
                tmp1.xyz = tmp0.xyz * tmp1.xyz + float3(0.0125229, 0.0125229, 0.0125229);
                o.sv_target.xyz = tmp0.xyz * tmp1.xyz;
                o.sv_target.w = tmp0.w;
                return o;
			}
			ENDCG
		}
		Pass {
			ZTest Always
			ZWrite Off
			Cull Off
			GpuProgramID 549810
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float2 texcoord : TEXCOORD0;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float4 _MainTex_ST;
			// $Globals ConstantBuffers for Fragment Shader
			float4 _MainTex_TexelSize;
			float _SampleScale;
			float _CombineSrc;
			float _CombineDst;
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			// Texture params for Vertex Shader
			// Texture params for Fragment Shader
			sampler2D _MainTex;
			sampler2D _BloomTex;
			
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
                o.texcoord.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
                return o;
			}
			// Keywords: 
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                float4 tmp1;
                float4 tmp2;
                tmp0 = _MainTex_TexelSize * float4(-1.0, -1.0, 1.0, 1.0);
                tmp1.x = _SampleScale * 0.5;
                tmp2 = tmp0.xyzy * tmp1.xxxx + inp.texcoord.xyxy;
                tmp0 = tmp0.xwzw * tmp1.xxxx + inp.texcoord.xyxy;
                tmp1 = tex2D(_MainTex, tmp2.xy);
                tmp2 = tex2D(_MainTex, tmp2.zw);
                tmp1 = tmp1 + tmp2;
                tmp2 = tex2D(_MainTex, tmp0.xy);
                tmp0 = tex2D(_MainTex, tmp0.zw);
                tmp1 = tmp1 + tmp2;
                tmp0 = tmp0 + tmp1;
                tmp0 = tmp0 * float4(0.25, 0.25, 0.25, 0.25);
                tmp1 = tex2D(_BloomTex, inp.texcoord.xy);
                tmp1 = tmp1 * _CombineSrc.xxxx;
                tmp0 = tmp0 * _CombineDst.xxxx + tmp1;
                tmp1.xyz = tmp0.xyz * float3(0.305306, 0.305306, 0.305306) + float3(0.6821711, 0.6821711, 0.6821711);
                tmp1.xyz = tmp0.xyz * tmp1.xyz + float3(0.0125229, 0.0125229, 0.0125229);
                o.sv_target.xyz = tmp0.xyz * tmp1.xyz;
                o.sv_target.w = tmp0.w;
                return o;
			}
			ENDCG
		}
		Pass {
			ZTest Always
			ZWrite Off
			Cull Off
			GpuProgramID 650046
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float2 texcoord : TEXCOORD0;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float4 _MainTex_ST;
			// $Globals ConstantBuffers for Fragment Shader
			float _CombineSrc;
			float _CombineDst;
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			// Texture params for Vertex Shader
			// Texture params for Fragment Shader
			sampler2D _MainTex;
			sampler2D _BloomTex;
			
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
                o.texcoord.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
                return o;
			}
			// Keywords: 
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                float4 tmp1;
                tmp0 = tex2D(_BloomTex, inp.texcoord.xy);
                tmp0 = tmp0 * _CombineSrc.xxxx;
                tmp1 = tex2D(_MainTex, inp.texcoord.xy);
                o.sv_target = tmp1 * _CombineDst.xxxx + tmp0;
                return o;
			}
			ENDCG
		}
		Pass {
			ZTest Always
			ZWrite Off
			Cull Off
			GpuProgramID 682680
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float2 texcoord : TEXCOORD0;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float4 _MainTex_ST;
			// $Globals ConstantBuffers for Fragment Shader
			float _CombineSrc;
			float _CombineDst;
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			// Texture params for Vertex Shader
			// Texture params for Fragment Shader
			sampler2D _MainTex;
			sampler2D _BloomTex;
			
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
                o.texcoord.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
                return o;
			}
			// Keywords: 
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                float4 tmp1;
                tmp0 = tex2D(_BloomTex, inp.texcoord.xy);
                tmp0 = tmp0 * _CombineSrc.xxxx;
                tmp1 = tex2D(_MainTex, inp.texcoord.xy);
                tmp0 = tmp1 * _CombineDst.xxxx + tmp0;
                tmp1.xyz = tmp0.xyz * float3(0.305306, 0.305306, 0.305306) + float3(0.6821711, 0.6821711, 0.6821711);
                tmp1.xyz = tmp0.xyz * tmp1.xyz + float3(0.0125229, 0.0125229, 0.0125229);
                o.sv_target.xyz = tmp0.xyz * tmp1.xyz;
                o.sv_target.w = tmp0.w;
                return o;
			}
			ENDCG
		}
		Pass {
			ZTest Always
			ZWrite Off
			Cull Off
			GpuProgramID 738888
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float2 texcoord : TEXCOORD0;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float4 _MainTex_ST;
			// $Globals ConstantBuffers for Fragment Shader
			float4 _MainTex_TexelSize;
			float _SampleScale;
			float _CombineSrc;
			float _CombineDst;
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			// Texture params for Vertex Shader
			// Texture params for Fragment Shader
			sampler2D _MainTex;
			sampler2D _BloomTex;
			
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
                o.texcoord.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
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
                float4 tmp4;
                tmp0.x = 1.0;
                tmp0.z = _SampleScale;
                tmp0 = tmp0.xxzz * _MainTex_TexelSize;
                tmp1.zw = float2(-1.0, 0.0);
                tmp1.x = _SampleScale;
                tmp2 = -tmp0.xywy * tmp1.xxwx + inp.texcoord.xyxy;
                tmp3 = tex2D(_MainTex, tmp2.xy);
                tmp2 = tex2D(_MainTex, tmp2.zw);
                tmp2 = tmp2 * float4(2.0, 2.0, 2.0, 2.0) + tmp3;
                tmp3.xy = -tmp0.zy * tmp1.zx + inp.texcoord.xy;
                tmp3 = tex2D(_MainTex, tmp3.xy);
                tmp2 = tmp2 + tmp3;
                tmp3 = tmp0.zwxw * tmp1.zwxw + inp.texcoord.xyxy;
                tmp4 = tmp0.zywy * tmp1.zxwx + inp.texcoord.xyxy;
                tmp0.xy = tmp0.xy * tmp1.xx + inp.texcoord.xy;
                tmp0 = tex2D(_MainTex, tmp0.xy);
                tmp1 = tex2D(_MainTex, tmp3.xy);
                tmp3 = tex2D(_MainTex, tmp3.zw);
                tmp1 = tmp1 * float4(2.0, 2.0, 2.0, 2.0) + tmp2;
                tmp2 = tex2D(_MainTex, inp.texcoord.xy);
                tmp1 = tmp2 * float4(4.0, 4.0, 4.0, 4.0) + tmp1;
                tmp1 = tmp3 * float4(2.0, 2.0, 2.0, 2.0) + tmp1;
                tmp2 = tex2D(_MainTex, tmp4.xy);
                tmp3 = tex2D(_MainTex, tmp4.zw);
                tmp1 = tmp1 + tmp2;
                tmp1 = tmp3 * float4(2.0, 2.0, 2.0, 2.0) + tmp1;
                tmp0 = tmp0 + tmp1;
                tmp0 = tmp0 * float4(0.0625, 0.0625, 0.0625, 0.0625);
                tmp1 = tex2D(_BloomTex, inp.texcoord.xy);
                tmp1 = tmp1 * _CombineSrc.xxxx;
                tmp0 = tmp0 * _CombineDst.xxxx + tmp1;
                tmp1.xyz = tmp0.xyz * float3(0.25, 0.25, 0.25) + float3(1.0, 1.0, 1.0);
                tmp1.xyz = tmp0.xyz * tmp1.xyz;
                tmp0.xyz = tmp0.xyz + float3(1.0, 1.0, 1.0);
                o.sv_target.w = tmp0.w;
                o.sv_target.xyz = tmp1.xyz / tmp0.xyz;
                return o;
			}
			ENDCG
		}
		Pass {
			ZTest Always
			ZWrite Off
			Cull Off
			GpuProgramID 810458
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float2 texcoord : TEXCOORD0;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float4 _MainTex_ST;
			// $Globals ConstantBuffers for Fragment Shader
			float4 _MainTex_TexelSize;
			float _SampleScale;
			float _CombineSrc;
			float _CombineDst;
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			// Texture params for Vertex Shader
			// Texture params for Fragment Shader
			sampler2D _MainTex;
			sampler2D _BloomTex;
			
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
                o.texcoord.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
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
                float4 tmp4;
                tmp0.x = 1.0;
                tmp0.z = _SampleScale;
                tmp0 = tmp0.xxzz * _MainTex_TexelSize;
                tmp1.zw = float2(-1.0, 0.0);
                tmp1.x = _SampleScale;
                tmp2 = -tmp0.xywy * tmp1.xxwx + inp.texcoord.xyxy;
                tmp3 = tex2D(_MainTex, tmp2.xy);
                tmp2 = tex2D(_MainTex, tmp2.zw);
                tmp2 = tmp2 * float4(2.0, 2.0, 2.0, 2.0) + tmp3;
                tmp3.xy = -tmp0.zy * tmp1.zx + inp.texcoord.xy;
                tmp3 = tex2D(_MainTex, tmp3.xy);
                tmp2 = tmp2 + tmp3;
                tmp3 = tmp0.zwxw * tmp1.zwxw + inp.texcoord.xyxy;
                tmp4 = tmp0.zywy * tmp1.zxwx + inp.texcoord.xyxy;
                tmp0.xy = tmp0.xy * tmp1.xx + inp.texcoord.xy;
                tmp0 = tex2D(_MainTex, tmp0.xy);
                tmp1 = tex2D(_MainTex, tmp3.xy);
                tmp3 = tex2D(_MainTex, tmp3.zw);
                tmp1 = tmp1 * float4(2.0, 2.0, 2.0, 2.0) + tmp2;
                tmp2 = tex2D(_MainTex, inp.texcoord.xy);
                tmp1 = tmp2 * float4(4.0, 4.0, 4.0, 4.0) + tmp1;
                tmp1 = tmp3 * float4(2.0, 2.0, 2.0, 2.0) + tmp1;
                tmp2 = tex2D(_MainTex, tmp4.xy);
                tmp3 = tex2D(_MainTex, tmp4.zw);
                tmp1 = tmp1 + tmp2;
                tmp1 = tmp3 * float4(2.0, 2.0, 2.0, 2.0) + tmp1;
                tmp0 = tmp0 + tmp1;
                tmp0 = tmp0 * float4(0.0625, 0.0625, 0.0625, 0.0625);
                tmp1 = tex2D(_BloomTex, inp.texcoord.xy);
                tmp1 = tmp1 * _CombineSrc.xxxx;
                tmp0 = tmp0 * _CombineDst.xxxx + tmp1;
                tmp1.xyz = tmp0.xyz * float3(2.51, 2.51, 2.51) + float3(0.03, 0.03, 0.03);
                tmp1.xyz = tmp0.xyz * tmp1.xyz;
                tmp2.xyz = tmp0.xyz * float3(2.43, 2.43, 2.43) + float3(0.59, 0.59, 0.59);
                tmp0.xyz = tmp0.xyz * tmp2.xyz + float3(0.14, 0.14, 0.14);
                o.sv_target.w = tmp0.w;
                o.sv_target.xyz = saturate(tmp1.xyz / tmp0.xyz);
                return o;
			}
			ENDCG
		}
		Pass {
			ZTest Always
			ZWrite Off
			Cull Off
			GpuProgramID 911069
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float2 texcoord : TEXCOORD0;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float4 _MainTex_ST;
			// $Globals ConstantBuffers for Fragment Shader
			float _AutoExposureLimit;
			float4 _MainTex_TexelSize;
			float _SampleScale;
			float _CombineSrc;
			float _CombineDst;
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			// Texture params for Vertex Shader
			// Texture params for Fragment Shader
			sampler2D _MainTex;
			sampler2D _GlobalIntensityTex;
			sampler2D _BloomTex;
			
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
                o.texcoord.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
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
                float4 tmp4;
                tmp0.x = 1.0;
                tmp0.z = _SampleScale;
                tmp0 = tmp0.xxzz * _MainTex_TexelSize;
                tmp1.zw = float2(-1.0, 0.0);
                tmp1.x = _SampleScale;
                tmp2 = -tmp0.xywy * tmp1.xxwx + inp.texcoord.xyxy;
                tmp3 = tex2D(_MainTex, tmp2.xy);
                tmp2 = tex2D(_MainTex, tmp2.zw);
                tmp2 = tmp2 * float4(2.0, 2.0, 2.0, 2.0) + tmp3;
                tmp3.xy = -tmp0.zy * tmp1.zx + inp.texcoord.xy;
                tmp3 = tex2D(_MainTex, tmp3.xy);
                tmp2 = tmp2 + tmp3;
                tmp3 = tmp0.zwxw * tmp1.zwxw + inp.texcoord.xyxy;
                tmp4 = tmp0.zywy * tmp1.zxwx + inp.texcoord.xyxy;
                tmp0.xy = tmp0.xy * tmp1.xx + inp.texcoord.xy;
                tmp0 = tex2D(_MainTex, tmp0.xy);
                tmp1 = tex2D(_MainTex, tmp3.xy);
                tmp3 = tex2D(_MainTex, tmp3.zw);
                tmp1 = tmp1 * float4(2.0, 2.0, 2.0, 2.0) + tmp2;
                tmp2 = tex2D(_MainTex, inp.texcoord.xy);
                tmp1 = tmp2 * float4(4.0, 4.0, 4.0, 4.0) + tmp1;
                tmp1 = tmp3 * float4(2.0, 2.0, 2.0, 2.0) + tmp1;
                tmp2 = tex2D(_MainTex, tmp4.xy);
                tmp3 = tex2D(_MainTex, tmp4.zw);
                tmp1 = tmp1 + tmp2;
                tmp1 = tmp3 * float4(2.0, 2.0, 2.0, 2.0) + tmp1;
                tmp0 = tmp0 + tmp1;
                tmp0 = tmp0 * float4(0.0625, 0.0625, 0.0625, 0.0625);
                tmp1 = tex2D(_BloomTex, inp.texcoord.xy);
                tmp1 = tmp1 * _CombineSrc.xxxx;
                tmp0 = tmp0 * _CombineDst.xxxx + tmp1;
                tmp1 = tex2D(_GlobalIntensityTex, float2(0.5, 0.5));
                tmp1.x = dot(tmp1.xyz, float3(0.3, 0.59, 0.11));
                tmp1.y = sqrt(tmp1.x);
                tmp1.x = tmp1.x * _AutoExposureLimit;
                tmp1.y = 0.1 / tmp1.y;
                tmp1.x = min(tmp1.x, tmp1.y);
                tmp0 = tmp0 * tmp1.xxxx;
                tmp1.xyz = tmp0.xyz * float3(2.51, 2.51, 2.51) + float3(0.03, 0.03, 0.03);
                tmp1.xyz = tmp0.xyz * tmp1.xyz;
                tmp2.xyz = tmp0.xyz * float3(2.43, 2.43, 2.43) + float3(0.59, 0.59, 0.59);
                tmp0.xyz = tmp0.xyz * tmp2.xyz + float3(0.14, 0.14, 0.14);
                o.sv_target.w = tmp0.w;
                o.sv_target.xyz = saturate(tmp1.xyz / tmp0.xyz);
                return o;
			}
			ENDCG
		}
	}
}