Shader "Hidden/HM/KawaseBlur" {
	Properties {
		_MainTex ("Main Texture", 2D) = "" {}
	}
	SubShader {
		Pass {
			Name "AlphaWeights"
			ZTest Always
			ZWrite Off
			Cull Off
			GpuProgramID 51162
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
                tmp0 = tex2D(_MainTex, inp.texcoord.xy);
                tmp1.x = saturate(tmp0.w);
                o.sv_target.xyz = tmp0.xyz * tmp1.xxx;
                o.sv_target.w = tmp0.w;
                return o;
			}
			ENDCG
		}
		Pass {
			Name "KawaseBlur"
			ZTest Always
			ZWrite Off
			Cull Off
			GpuProgramID 91764
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
			float4 _MainTex_ST;
			float4 _MainTex_TexelSize;
			float4 _Offset;
			// $Globals ConstantBuffers for Fragment Shader
			float _Boost;
			float _AdditiveAlpha;
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
                tmp0 = v.texcoord.xyxy * _MainTex_ST + _MainTex_ST;
                tmp1 = _MainTex_TexelSize * _Offset;
                o.texcoord = tmp1.xyxy * float4(1.0, 1.0, -1.0, -1.0) + tmp0.zwzw;
                o.texcoord1 = tmp1.zwzw * float4(1.0, 1.0, -1.0, -1.0) + tmp0;
                return o;
			}
			// Keywords: 
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                float4 tmp1;
                tmp0 = tex2D(_MainTex, inp.texcoord.xy);
                tmp1 = tex2D(_MainTex, inp.texcoord.zw);
                tmp0.xyz = tmp0.xyz + tmp1.xyz;
                tmp1 = tex2D(_MainTex, inp.texcoord1.xy);
                tmp0.xyz = tmp0.xyz + tmp1.xyz;
                tmp1 = tex2D(_MainTex, inp.texcoord1.zw);
                tmp0.xyz = tmp0.xyz + tmp1.xyz;
                tmp0.w = _Boost + 0.25;
                o.sv_target.xyz = tmp0.www * tmp0.xyz;
                o.sv_target.w = _AdditiveAlpha;
                return o;
			}
			ENDCG
		}
		Pass {
			Name "KawaseBlurAdd"
			Blend SrcAlpha One, SrcAlpha One
			ZTest Always
			ZWrite Off
			Cull Off
			GpuProgramID 180443
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
			float4 _MainTex_ST;
			float4 _MainTex_TexelSize;
			float4 _Offset;
			// $Globals ConstantBuffers for Fragment Shader
			float _Boost;
			float _AdditiveAlpha;
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
                tmp0 = v.texcoord.xyxy * _MainTex_ST + _MainTex_ST;
                tmp1 = _MainTex_TexelSize * _Offset;
                o.texcoord = tmp1.xyxy * float4(1.0, 1.0, -1.0, -1.0) + tmp0.zwzw;
                o.texcoord1 = tmp1.zwzw * float4(1.0, 1.0, -1.0, -1.0) + tmp0;
                return o;
			}
			// Keywords: 
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                float4 tmp1;
                tmp0 = tex2D(_MainTex, inp.texcoord.xy);
                tmp1 = tex2D(_MainTex, inp.texcoord.zw);
                tmp0.xyz = tmp0.xyz + tmp1.xyz;
                tmp1 = tex2D(_MainTex, inp.texcoord1.xy);
                tmp0.xyz = tmp0.xyz + tmp1.xyz;
                tmp1 = tex2D(_MainTex, inp.texcoord1.zw);
                tmp0.xyz = tmp0.xyz + tmp1.xyz;
                tmp0.w = _Boost + 0.25;
                o.sv_target.xyz = tmp0.www * tmp0.xyz;
                o.sv_target.w = _AdditiveAlpha;
                return o;
			}
			ENDCG
		}
		Pass {
			Name "KawaseBlurWithAlphaWeights"
			ZTest Always
			ZWrite Off
			Cull Off
			GpuProgramID 205744
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
			float4 _MainTex_ST;
			float4 _MainTex_TexelSize;
			float4 _Offset;
			// $Globals ConstantBuffers for Fragment Shader
			float _Boost;
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
                tmp0 = v.texcoord.xyxy * _MainTex_ST + _MainTex_ST;
                tmp1 = _MainTex_TexelSize * _Offset;
                o.texcoord = tmp1.xyxy * float4(1.0, 1.0, -1.0, -1.0) + tmp0.zwzw;
                o.texcoord1 = tmp1.zwzw * float4(1.0, 1.0, -1.0, -1.0) + tmp0;
                return o;
			}
			// Keywords: 
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                float4 tmp1;
                tmp0 = tex2D(_MainTex, inp.texcoord.xy);
                tmp1 = tex2D(_MainTex, inp.texcoord.zw);
                tmp0 = tmp0 + tmp1;
                tmp1 = tex2D(_MainTex, inp.texcoord1.xy);
                tmp0 = tmp0 + tmp1;
                tmp1 = tex2D(_MainTex, inp.texcoord1.zw);
                tmp0 = tmp0 + tmp1;
                tmp1.x = saturate(tmp1.w * _AlphaWeights);
                tmp1.y = _Boost + 0.25;
                tmp0 = tmp0 * tmp1.yyyy;
                o.sv_target.xyz = tmp1.xxx * tmp0.xyz;
                o.sv_target.w = tmp0.w;
                return o;
			}
			ENDCG
		}
		Pass {
			Name "AlphaAndDepthWeights"
			ZTest Always
			ZWrite Off
			Cull Off
			GpuProgramID 291630
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
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			// Texture params for Vertex Shader
			// Texture params for Fragment Shader
			sampler2D _CameraDepthTexture;
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
                tmp0 = tex2D(_CameraDepthTexture, inp.texcoord.xy);
                tmp0.x = _ZBufferParams.z * tmp0.x + _ZBufferParams.w;
                tmp0.x = 1.0 / tmp0.x;
                tmp0.x = saturate(tmp0.x * 0.02 + 0.6);
                tmp1 = tex2D(_MainTex, inp.texcoord.xy);
                tmp0.yzw = tmp1.www * tmp1.xyz;
                o.sv_target.w = tmp1.w;
                o.sv_target.xyz = tmp0.xxx * tmp0.yzw;
                return o;
			}
			ENDCG
		}
		Pass {
			Name "KawaseBlurGamma"
			ZTest Always
			ZWrite Off
			Cull Off
			GpuProgramID 386598
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
			float4 _MainTex_ST;
			float4 _MainTex_TexelSize;
			float4 _Offset;
			// $Globals ConstantBuffers for Fragment Shader
			float _Boost;
			float _AdditiveAlpha;
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
                tmp0 = v.texcoord.xyxy * _MainTex_ST + _MainTex_ST;
                tmp1 = _MainTex_TexelSize * _Offset;
                o.texcoord = tmp1.xyxy * float4(1.0, 1.0, -1.0, -1.0) + tmp0.zwzw;
                o.texcoord1 = tmp1.zwzw * float4(1.0, 1.0, -1.0, -1.0) + tmp0;
                return o;
			}
			// Keywords: 
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                float4 tmp1;
                tmp0 = tex2D(_MainTex, inp.texcoord.xy);
                tmp1 = tex2D(_MainTex, inp.texcoord.zw);
                tmp0.xyz = tmp0.xyz + tmp1.xyz;
                tmp1 = tex2D(_MainTex, inp.texcoord1.xy);
                tmp0.xyz = tmp0.xyz + tmp1.xyz;
                tmp1 = tex2D(_MainTex, inp.texcoord1.zw);
                tmp0.xyz = tmp0.xyz + tmp1.xyz;
                tmp0.w = _Boost + 0.25;
                tmp0.xyz = tmp0.www * tmp0.xyz;
                tmp1.xyz = tmp0.xyz * float3(0.305306, 0.305306, 0.305306) + float3(0.6821711, 0.6821711, 0.6821711);
                tmp1.xyz = tmp0.xyz * tmp1.xyz + float3(0.0125229, 0.0125229, 0.0125229);
                o.sv_target.xyz = tmp0.xyz * tmp1.xyz;
                o.sv_target.w = _AdditiveAlpha;
                return o;
			}
			ENDCG
		}
		Pass {
			Name "KawaseBlurAddGamma"
			Blend SrcAlpha One, SrcAlpha One
			ZTest Always
			ZWrite Off
			Cull Off
			GpuProgramID 411263
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
			float4 _MainTex_ST;
			float4 _MainTex_TexelSize;
			float4 _Offset;
			// $Globals ConstantBuffers for Fragment Shader
			float _Boost;
			float _AdditiveAlpha;
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
                tmp0 = v.texcoord.xyxy * _MainTex_ST + _MainTex_ST;
                tmp1 = _MainTex_TexelSize * _Offset;
                o.texcoord = tmp1.xyxy * float4(1.0, 1.0, -1.0, -1.0) + tmp0.zwzw;
                o.texcoord1 = tmp1.zwzw * float4(1.0, 1.0, -1.0, -1.0) + tmp0;
                return o;
			}
			// Keywords: 
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                float4 tmp1;
                tmp0 = tex2D(_MainTex, inp.texcoord.xy);
                tmp1 = tex2D(_MainTex, inp.texcoord.zw);
                tmp0.xyz = tmp0.xyz + tmp1.xyz;
                tmp1 = tex2D(_MainTex, inp.texcoord1.xy);
                tmp0.xyz = tmp0.xyz + tmp1.xyz;
                tmp1 = tex2D(_MainTex, inp.texcoord1.zw);
                tmp0.xyz = tmp0.xyz + tmp1.xyz;
                tmp0.w = _Boost + 0.25;
                tmp0.xyz = tmp0.www * tmp0.xyz;
                tmp1.xyz = tmp0.xyz * float3(0.305306, 0.305306, 0.305306) + float3(0.6821711, 0.6821711, 0.6821711);
                tmp1.xyz = tmp0.xyz * tmp1.xyz + float3(0.0125229, 0.0125229, 0.0125229);
                o.sv_target.xyz = tmp0.xyz * tmp1.xyz;
                o.sv_target.w = _AdditiveAlpha;
                return o;
			}
			ENDCG
		}
	}
}