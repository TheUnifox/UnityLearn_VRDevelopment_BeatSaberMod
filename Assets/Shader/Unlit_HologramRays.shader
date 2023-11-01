Shader "Unlit/HologramRays" {
	Properties {
		[Header(Shape)] [Space] _TopPosition ("Top Position", Float) = 7
		_BottomPosition ("Bottom Position", Float) = 4
		_ScaleInverse ("1 / Scale", Float) = 2
		_AvatarScaleInverse ("1 / Avatar Scale", Float) = 0.25
		_WidthStartEnd ("Width Start End", Vector) = (0.4,2.5,0,0)
		[Header(Base Options)] [Space] _Speed ("Speed", Float) = 1
		_Color ("Color", Vector) = (1,1,1,0)
		_Alpha ("Alpha", Float) = 1
		[Toggle(ENABLE_ONEDIRECTIONAL)] _OneDirectional ("One Directional", Float) = 0
		[Header(Randomness)] [Space] _NoiseSpeedScale ("Noise Speed Scale", Float) = 1
		_NoiseOffsetScale ("Noise Offset Scale", Float) = 1
		_AlphaNoiseStrength ("Alpha Noise Strength", Range(0, 1)) = 0.5
		_AlphaNoiseScale ("Alpha Noise Scale", Float) = 2.3
		_AlphaNoiseSpeed ("Alpha Noise Speed", Float) = 0.35
		[Toggle(ENABLE_HOLOGRAM_NOISE)] _EnableHologramNoise ("Enable Hologram Noise", Float) = 0
		[ShowIfAny(ENABLE_HOLOGRAM_NOISE)] _HologramNoiseIntensity ("Hologram Noise Intensity", Float) = 0.1
		[Space(12)] [ToggleHeader(ENABLE_VARIABLE_LENGTH)] _EnableVariableLength ("Enable Variable Length", Float) = 0
		[ShowIfAny(ENABLE_VARIABLE_LENGTH)] _VariableLengthMultiplier ("Variable Length Multiplier", Float) = 1.5
		[ShowIfAny(ENABLE_VARIABLE_LENGTH)] _LengthNoiseScale ("Length Noise Scale", Float) = 6.32
		[Space(12)] [ToggleHeader(ENABLE_WORLD_NOISE)] _EnableWorldNoise ("Enable World Noise", Float) = 0
		[ShowIfAny(ENABLE_WORLD_NOISE)] _WorldNoiseScale ("World Noise Scale", Float) = 1
		[ShowIfAny(ENABLE_WORLD_NOISE)] _WorldNoiseIntensityOffset ("World Intensity Offset", Float) = 0
		[ShowIfAny(ENABLE_WORLD_NOISE)] _WorldNoiseIntensityScale ("World Intenstity Scale", Float) = 1
		[ShowIfAny(ENABLE_WORLD_NOISE)] _WorldNoiseScrolling ("World Noise Scrolling", Vector) = (0,0,0,1)
		[Header(Other)] [Space] [Enum(UnityEngine.Rendering.BlendMode)] _BlendSrcFactor ("Blend Src", Float) = 0
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendDstFactor ("Blend Dst", Float) = 0
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendSrcFactorA ("Blend Src Factor A", Float) = 0
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendDstFactorA ("Blend Dst Factor A", Float) = 10
		[Enum(UnityEngine.Rendering.BlendOp)] _BlendOp ("Blend Operation", Float) = 0
		[Space] [Enum(UnityEngine.Rendering.CompareFunction)] _ZTest ("ZTest", Float) = 4
	}
	SubShader {
		Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		Pass {
			Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			Blend Zero Zero, Zero Zero
			ZWrite Off
			Cull Off
			GpuProgramID 18538
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float2 texcoord : TEXCOORD0;
				float texcoord1 : TEXCOORD1;
				float4 position : SV_POSITION0;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float _Speed;
			float _TopPosition;
			float _BottomPosition;
			float _ScaleInverse;
			float _NoiseSpeedScale;
			float _NoiseOffsetScale;
			float _AlphaNoiseStrength;
			float _AlphaNoiseScale;
			float _AlphaNoiseSpeed;
			float _AvatarScaleInverse;
			float2 _WidthStartEnd;
			// $Globals ConstantBuffers for Fragment Shader
			float4 _Color;
			float _Alpha;
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			// Texture params for Vertex Shader
			// Texture params for Fragment Shader
			
			// Keywords: 
			v2f vert(appdata_full v)
			{
                v2f o;
                float4 tmp0;
                float4 tmp1;
                tmp0.x = _AlphaNoiseSpeed * _Time.y;
                tmp0.y = v.vertex.z * _NoiseSpeedScale + 1.0;
                tmp0.zw = v.vertex.zz * float2(_NoiseOffsetScale.x, _AlphaNoiseScale.x);
                tmp0.x = tmp0.x * tmp0.y + tmp0.w;
                tmp0.x = sin(tmp0.x);
                tmp0.x = tmp0.x * _AlphaNoiseStrength + -_AlphaNoiseStrength;
                o.texcoord1.x = tmp0.x + 1.0;
                o.texcoord.xy = v.texcoord.xy;
                tmp0.x = _Speed * _Time.y;
                tmp0.x = tmp0.x * tmp0.y + tmp0.z;
                tmp0.x = sin(tmp0.x);
                tmp0.x = tmp0.x * 0.5 + 0.5;
                tmp0.yz = float2(_TopPosition.x, _BottomPosition.x) - unity_ObjectToWorld._m13_m13;
                tmp0.y = tmp0.y * _ScaleInverse;
                tmp0.z = tmp0.z * _ScaleInverse + -tmp0.y;
                tmp0.x = tmp0.z * tmp0.x + tmp0.y;
                tmp0.y = saturate(v.vertex.y + _AvatarScaleInverse);
                tmp0.x = tmp0.x * tmp0.y + v.vertex.x;
                tmp1 = v.vertex.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp0 = unity_ObjectToWorld._m00_m10_m20_m30 * tmp0.xxxx + tmp1;
                tmp1.x = _WidthStartEnd.y - _WidthStartEnd.x;
                tmp1.x = v.vertex.y * tmp1.x + _WidthStartEnd.x;
                tmp1.x = tmp1.x * v.vertex.z;
                tmp0 = unity_ObjectToWorld._m02_m12_m22_m32 * tmp1.xxxx + tmp0;
                tmp0 = tmp0 + unity_ObjectToWorld._m03_m13_m23_m33;
                tmp1 = tmp0.yyyy * unity_MatrixVP._m01_m11_m21_m31;
                tmp1 = unity_MatrixVP._m00_m10_m20_m30 * tmp0.xxxx + tmp1;
                tmp1 = unity_MatrixVP._m02_m12_m22_m32 * tmp0.zzzz + tmp1;
                o.position = unity_MatrixVP._m03_m13_m23_m33 * tmp0.wwww + tmp1;
                return o;
			}
			// Keywords: 
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
				float4 tmp1;
                tmp0.x = 1.0 - inp.texcoord.y;
				tmp0.y = 0.0;
                tmp0.x = dot(inp.texcoord.xy, tmp0.xy);
                tmp0.x = tmp0.x * tmp0.x;
                tmp0.y = inp.texcoord1.x * _Color.w;
                tmp0.y = tmp0.y * _Alpha;
                tmp0.x = tmp0.x * tmp0.y;
                o.sv_target.xyz = tmp0.xxx * _Color.xyz;
                o.sv_target.w = tmp0.x;
                return o;
			}
			ENDCG
		}
	}
}