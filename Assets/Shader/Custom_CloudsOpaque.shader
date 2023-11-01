Shader "Custom/CloudsOpaque" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		[Space(12)] [ToggleHeader(DIFFUSE)] _EnableDiffuse ("Enable Diffuse", Float) = 1
		[ToggleShowIfAny(INVERT_DIFFUSE_NORMAL, DIFFUSE)] _InvertDiffuseNormal ("Invert Diffuse Normal", Float) = 0
		[ToggleShowIfAny(BOTH_SIDES_DIFFUSE, DIFFUSE)] _EnableBothSidesDiffuse ("Enable Both Sides Diffuse", Float) = 0
		[Space(12)] _Speed ("Speed", Float) = 1
		[Space(12)] [ToggleHeader(ENABLE_FOG)] _EnableFog ("Enable Fog", Float) = 1
		[ShowIfAny(ENABLE_FOG)] _FogStartOffset ("Fog Start Offset", Float) = 0
		[ShowIfAny(ENABLE_FOG)] _FogScale ("Fog Scale", Float) = 1
		[ShowIfAny(ENABLE_FOG)] _HeightFogOffset ("Height Fog Offsets", Float) = 1
		[Space(12)] [ToggleHeader(ENABLE_WORLD_NOISE)] _EnableWorldNoise ("Modulate Vertex Y with Noise", Float) = 0
		[ShowIfAny(ENABLE_WORLD_NOISE)] _NoiseTex ("Noise Texture", 2D) = "white" {}
		[ShowIfAny(ENABLE_WORLD_NOISE)] _WorldNoiseScale ("World Noise Scale", Float) = 1
		[ShowIfAny(ENABLE_WORLD_NOISE)] _WorldNoiseIntensityOffset ("World Intensity Offset", Float) = 0
		[ShowIfAny(ENABLE_WORLD_NOISE)] _WorldNoiseIntensityScale ("World Intenstity Scale", Float) = 1
		[ShowIfAny(ENABLE_WORLD_NOISE)] _WorldNoiseScrolling ("World Noise Scrolling", Vector) = (0,0,0,1)
		[Space] [Toggle(ENABLE_NOISE_DITHERING)] _EnableNoiseDithering ("Noise Dithering", Float) = 0
		[Header(Other)] [Space] [Enum(UnityEngine.Rendering.CullMode)] _Cull ("Cull", Float) = 2
		_Offset ("Offset", Float) = 0
	}
	SubShader {
		Tags { "QUEUE" = "Geometry" "RenderType" = "Opaque" }
		Pass {
			Tags { "QUEUE" = "Geometry" "RenderType" = "Opaque" }
			Cull Off
			GpuProgramID 64011
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float2 texcoord : TEXCOORD0;
				float4 color : COLOR0;
				float3 texcoord1 : TEXCOORD1;
				float3 texcoord4 : TEXCOORD4;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float4 _MainTex_ST;
			float _Speed;
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
                float4 tmp2;
                tmp0.x = v.vertex.z * 12.345;
                tmp0.x = sin(tmp0.x);
                tmp0.y = tmp0.x > 0.0;
                tmp0.z = tmp0.x < 0.0;
                tmp0.x = tmp0.x * 0.5 + 1.0;
                tmp0.x = tmp0.x * _Speed;
                tmp0.y = tmp0.z - tmp0.y;
                tmp0.y = floor(tmp0.y);
                tmp0.x = tmp0.y * tmp0.x;
                tmp0.x = _Time.y * tmp0.x + v.vertex.x;
                tmp0.x = tmp0.x / v.vertex.z;
                tmp0.x = sin(tmp0.x);
                tmp1.x = cos(tmp0.x);
                tmp0.x = tmp0.x * v.vertex.z;
                tmp0.y = tmp1.x * v.vertex.z;
                tmp1 = v.vertex.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp1 = unity_ObjectToWorld._m00_m10_m20_m30 * tmp0.xxxx + tmp1;
                tmp1 = unity_ObjectToWorld._m02_m12_m22_m32 * tmp0.yyyy + tmp1;
                tmp1 = tmp1 + unity_ObjectToWorld._m03_m13_m23_m33;
                tmp2 = tmp1.yyyy * unity_MatrixVP._m01_m11_m21_m31;
                tmp2 = unity_MatrixVP._m00_m10_m20_m30 * tmp1.xxxx + tmp2;
                tmp2 = unity_MatrixVP._m02_m12_m22_m32 * tmp1.zzzz + tmp2;
                o.position = unity_MatrixVP._m03_m13_m23_m33 * tmp1.wwww + tmp2;
                o.texcoord.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
                o.color = v.color;
                tmp1.xyz = v.vertex.yyy * unity_ObjectToWorld._m01_m11_m21;
                tmp0.xzw = unity_ObjectToWorld._m00_m10_m20 * tmp0.xxx + tmp1.xyz;
                tmp0.xyz = unity_ObjectToWorld._m02_m12_m22 * tmp0.yyy + tmp0.xzw;
                tmp0.xyz = unity_ObjectToWorld._m03_m13_m23 * v.vertex.www + tmp0.xyz;
                o.texcoord1.xyz = tmp0.xyz;
                tmp0.w = dot(tmp0.xyz, tmp0.xyz);
                tmp0.w = rsqrt(tmp0.w);
                tmp0.xyz = tmp0.www * tmp0.xyz;
                o.texcoord4.xyz = -tmp0.xyz;
                return o;
			}
			// Keywords: 
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                tmp0 = tex2D(_MainTex, inp.texcoord.xy);
                o.sv_target.xyz = tmp0.xyz * inp.color.xyz;
                o.sv_target.w = 0.0;
                return o;
			}
			ENDCG
		}
	}
}