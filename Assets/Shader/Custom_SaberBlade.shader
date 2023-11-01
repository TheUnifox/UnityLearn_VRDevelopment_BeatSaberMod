Shader "Custom/SaberBlade" {
	Properties {
		[Toggle(_ENABLE_INSTANCED_COLOR)] _EnableInstancedColor ("Enable Instanced Color", Float) = 0
		_TintColor ("Tint Color", Vector) = (1,1,1,1)
		_Brightness ("Brightness", Float) = 1
		[Space(12)] _MainTex ("Texture", 2D) = "white" {}
		_MainTexSpeed ("Texture Speed", Vector) = (0,6,0,0)
		[Space(8)] [EnumHeader(Once, Twice, Thrice)] _Sample ("Sample Count", Float) = 0
		[Space(12)] [VectorShowIfAny(2, _SAMPLE_TWICE, _SAMPLE_THRICE)] _Main2Tiling ("2nd Sample Tiling", Vector) = (1.7,1.7,0,0)
		[VectorShowIfAny(2, _SAMPLE_TWICE, _SAMPLE_THRICE)] _Main2Speed ("2nd Sample Speed", Vector) = (-1,3,0,0)
		[VectorShowIfAny(2, _SAMPLE_THRICE)] _Main3Tiling ("2nd Sample Tiling", Vector) = (0.6,0.6,0,0)
		[VectorShowIfAny(2, _SAMPLE_THRICE)] _Main3Speed ("2nd Sample Speed", Vector) = (0.5,-1,0,0)
		[Space(16)] [ToggleHeader(SECONDARY_TEXTURE)] _EnableSecondaryTexture ("Enable Secondary Texture", Float) = 0
		[ShowIfAny(SECONDARY_TEXTURE)] _SecondaryTex ("Secondary Texture", 2D) = "white" {}
		[ShowIfAny(SECONDARY_TEXTURE)] _SecondaryTexSpeed ("Secondary Texture Speed", Vector) = (0,1,0,0)
		[Space(8)] [ToggleHeader(ENABLE_FOG)] _EnableFog ("Enable Fog", Float) = 0
		_FogStartOffset ("Fog Start Offset", Float) = 0
		_FogScale ("Fog Scale", Float) = 1
		[Toggle(HOLOGRAM)] _EnableHologram ("Hologram effect", Float) = 0
		_HologramColor ("Hologram Color", Vector) = (1,1,1,1)
		[Header(Rim)] [Space(8)] _LightRimScale ("Light Rim Scale", Float) = 0
		_LightRimOffset ("Light Rim Offset", Float) = 0
		[Toggle(INVERT_RIM)] _InvertRim ("Invert Rim", Float) = 0
		[VectorShowIfAny(3, normalize)] _RimPerpendicularAxis ("Rim Perpendicular Axis", Vector) = (0,1,0,0)
		[Space(20)] [Enum(UnityEngine.Rendering.CullMode)] _CullMode ("CullMode", Float) = 0
	}
	SubShader {
		Tags { "IGNOREPROJECTOR" = "true" "PreviewType" = "Plane" "QUEUE" = "Transparent+1" "RenderType" = "Opaque" }
		Pass {
			Tags { "IGNOREPROJECTOR" = "true" "PreviewType" = "Plane" "QUEUE" = "Transparent+1" "RenderType" = "Opaque" }
			Cull Off
			GpuProgramID 11107
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float2 texcoord : TEXCOORD0;
				float texcoord1 : TEXCOORD1;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float _LightRimScale;
			float _LightRimOffset;
			float3 _RimPerpendicularAxis;
			// $Globals ConstantBuffers for Fragment Shader
			float4 _MainTex_ST;
			float2 _MainTexSpeed;
			float _Brightness;
			float4 _TintColor;
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
                tmp0.x = dot(_RimPerpendicularAxis, unity_WorldToObject._m00_m10_m20);
                tmp0.y = dot(_RimPerpendicularAxis, unity_WorldToObject._m01_m11_m21);
                tmp0.z = dot(_RimPerpendicularAxis, unity_WorldToObject._m02_m12_m22);
                tmp0.w = dot(tmp0.xyz, tmp0.xyz);
                tmp0.w = rsqrt(tmp0.w);
                tmp0.xyz = tmp0.www * tmp0.xyz;
                tmp0.w = dot(tmp0.xyz, unity_ObjectToWorld._m03_m13_m23);
                tmp1.x = dot(tmp0.xyz, _WorldSpaceCameraPos);
                tmp0.w = tmp1.x - tmp0.w;
                tmp0.xyz = -tmp0.www * tmp0.xyz + _WorldSpaceCameraPos;
                tmp0.xyz = tmp0.xyz - unity_ObjectToWorld._m03_m13_m23;
                tmp0.w = dot(tmp0.xyz, tmp0.xyz);
                tmp0.w = rsqrt(tmp0.w);
                tmp0.xyz = tmp0.www * tmp0.xyz;
                tmp1.x = dot(v.normal.xyz, unity_WorldToObject._m00_m10_m20);
                tmp1.y = dot(v.normal.xyz, unity_WorldToObject._m01_m11_m21);
                tmp1.z = dot(v.normal.xyz, unity_WorldToObject._m02_m12_m22);
                tmp0.w = dot(tmp1.xyz, tmp1.xyz);
                tmp0.w = rsqrt(tmp0.w);
                tmp1.xyz = tmp0.www * tmp1.xyz;
                tmp0.x = dot(tmp0.xyz, tmp1.xyz);
                tmp0.y = _LightRimOffset + 1.0;
                tmp0.x = tmp0.y - tmp0.x;
                o.texcoord1.x = saturate(tmp0.x * _LightRimScale);
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
                tmp0.xy = inp.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
                tmp0.zw = _MainTexSpeed * _Time.xx;
                tmp0.xy = tmp0.zw * _MainTex_ST.xy + tmp0.xy;
                tmp0 = tex2D(_MainTex, tmp0.xy);
                tmp0.y = tmp0.x * _Brightness + inp.texcoord1.x;
                tmp1.xyz = tmp0.xxx * _Brightness.xxx;
                tmp1.w = 0.0;
                tmp1 = saturate(tmp1 + _TintColor);
                tmp2 = float4(1.0, 1.0, 1.0, 1.0) - tmp1;
                o.sv_target = tmp0.yyyy * tmp2 + tmp1;
                return o;
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
}