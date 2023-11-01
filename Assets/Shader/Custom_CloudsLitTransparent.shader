Shader "Custom/CloudsLitTransparent" {
	Properties {
		_Color ("Color", Vector) = (1,1,1,1)
		[BigHeader(CLOUD PROPERTIES)] [Space(18)] [ToggleHeader(VERTEX_WAVE)] _EnableVertexWave ("Enable Vertex Wave", Float) = 0
		[ShowIfAny(VERTEX_WAVE)] _VertexWaveFrequency ("Frequency", Float) = 4
		[ShowIfAny(VERTEX_WAVE)] _VertexWaveAmplitude ("Amplitude", Float) = 0.03
		[Space(18)] [ToggleHeader(VERTEX_CURVE)] _EnableVertexCurve ("Enable Vertex Curve", Float) = 0
		[VectorShowIfAny(2, VERTEX_CURVE)] _VertexSpeed ("Speed", Vector) = (1,0,0,0)
		[ToggleShowIfAny(DEBUG_VERTEX_CURVE_INFLUENCE, VERTEX_CURVE)] _DebugVertexCurveEnableInfluence ("[DEBUG] Enable Influence", Float) = 0
		[ShowIfAny(2, DEBUG_VERTEX_CURVE_INFLUENCE, VERTEX_CURVE)] _DebugVertexCurveInfluence ("[DEBUG] Influence", Range(0, 1)) = 1
		[Space(18)] [Toggle(ALIGN_NORMALS_TO_WORLD_ORIGIN)] _AlignNormalsToWorldOrigin ("Align Normals To World Origin", Float) = 0
		[Space(18)] [ToggleHeader(DISTORT_TEXTURE)] _EnableDistortTexture ("Enable Distort Texture", Float) = 0
		[ShowIfAny(DISTORT_TEXTURE)] _DistortTex ("Distort Texture", 2D) = "white" {}
		[VectorShowIfAny(2, DISTORT_TEXTURE)] _DistortTexSpeed ("Speed", Vector) = (0.04,0,0,0)
		[ShowIfAny(DISTORT_TEXTURE)] _DistortAmount ("Amount", Range(0, 0.1)) = 0.1
		[EnumShowIf(2, Zero, One, DISTORT_TEXTURE)] _DistortUVChannel ("UV Channel", Float) = 0
		[Space(18)] [Toggle(FADE_BOTTOM)] _EnableBottomFade ("Enable Bottom Fade", Float) = 0
		[ShowIfAny(FADE_BOTTOM)] _FadeBottomMin ("Bottom Fade Min", Float) = -4
		[ShowIfAny(FADE_BOTTOM)] _FadeBottomMax ("Bottom Fade Max", Float) = 4
		[Space(18)] [Toggle(FADE_RUNWAY)] _EnableFadeRunway ("Enable Fade Runway", Float) = 0
		[ShowIfAny(FADE_RUNWAY)] _RunwayFadeOffset ("Offset", Float) = -1
		[ShowIfAny(FADE_RUNWAY)] _RunwayFadeScale ("Overall scale", Float) = 10
		[Space(18)] [ToggleHeader(BACK_LIGHTING)] _EnableBackLighting ("Enable Back Lighting", Float) = 0
		[ShowIfAny(BACK_LIGHTING)] _BackLightingBoost ("Boost", Float) = 2
		[Space(18)] [EnumHeader(Off, Vertex_Colors, Vertex_Normals, Pixel_Normals, Alpha)] _DebugColor ("[DEBUG] Color", Float) = 0
		[Toggle(DEBUG_NEGATIVE_COLOR)] _DebugNegativeColor ("[DEBUG] View Negative Color", Float) = 0
		[Toggle(DEBUG_DISCARD_ALPHA)] _DebugDiscardAlpha ("[DEBUG] Discard Alpha", Float) = 0
		[BigHeader(BASE PROPERTIES)] [Space(12)] [ToggleHeader(METAL_SMOOTHNESS_TEXTURE)] _EnableMetalSmoothnessTex ("Use Metallic & Smoothness Texture", Float) = 0
		[ShowIfAny(METAL_SMOOTHNESS_TEXTURE)] _MetalSmoothnessTex ("Metallic(R) & Smoothness(A) Texture", 2D) = "white" {}
		[ShowIfAny(0METAL_SMOOTHNESS_TEXTURE)] _Metallic ("Metallic", Range(0, 1)) = 0
		_Smoothness ("Smoothness", Range(0, 1)) = 0.5
		[Space(12)] [ToggleHeader(SPECULAR_ANTIFLICKER)] _SpecularAntiflicker ("Smoothness Anti-Flicker", Float) = 0
		[ShowIfAny(SPECULAR_ANTIFLICKER)] _AntiflickerStrength ("Antiflicker Strength", Range(0, 1)) = 0.7
		[ShowIfAny(SPECULAR_ANTIFLICKER)] _AntiflickerDistanceScale ("Antiflicker Distance Scale", Float) = 0.1
		[ShowIfAny(SPECULAR_ANTIFLICKER)] _AntiflickerDistanceOffset ("Antiflicker Distance Offset", Float) = 21
		[Space(18)] [EnumHeader(None, Color, Emission, MetalSmoothness, Special, ColorAsAlpha, RotateLayers)] _VertexMode ("Vertex Color Mode", Float) = 0
		[ShowIfAny(_VERTEXMODE_ROTATELAYERS)] _RotateLayerSpeeds ("Rotate Layer Speeds", Vector) = (16,6,2,32)
		[SpaceShowIfAny(12, _VERTEXMODE_EMISSION, _VERTEXMODE_SPECIAL)] [InfoBox(Emission uses green channel, _VERTEXMODE_EMISSION)] [InfoBox(Red for Metallic _ Green for Emission _ Alpha for Smoothness, _VERTEXMODE_SPECIAL)] [InfoBox(Red for Metallic _ Alpha for Smoothness, _VERTEXMODE_METALSMOOTHNESS)] [ShowIfAny(_VERTEXMODE_EMISSION, _VERTEXMODE_SPECIAL)] _EmissionThreshold ("Emission Threshold", Range(0, 1)) = 0
		[ShowIfAny(_VERTEXMODE_EMISSION, _VERTEXMODE_SPECIAL)] _EmissionColor ("Emission Color", Vector) = (1,1,1,0)
		[BigHeader(EMISSIONS AND DECALS)] [Space(18)] [EnumHeader(None, Simple, Pulse)] _EmissionTexture ("Emission Texture", Float) = 0
		[SpaceShowIfAny(6, _EMISSIONTEXTURE_SIMPLE, _EMISSIONTEXTURE_PULSE)] [ShowIfAny(_EMISSIONTEXTURE_SIMPLE, _EMISSIONTEXTURE_PULSE)] _EmissionBrightness ("Brightness", Float) = 1
		[EnumShowIf(3, Flat, Whiteboost, Gradient, _EMISSIONTEXTURE_SIMPLE, _EMISSIONTEXTURE_PULSE)] _EmissionColorType ("Color Type", Float) = 0
		[ShowIfAny(1, 0_EMISSIONCOLORTYPE_GRADIENT, _EMISSIONTEXTURE_SIMPLE, _EMISSIONTEXTURE_PULSE)] _EmissionTexColor ("Emission Color", Vector) = (1,1,1,0)
		[ShowIfAny(1, _EMISSIONCOLORTYPE_GRADIENT, _EMISSIONTEXTURE_SIMPLE, _EMISSIONTEXTURE_PULSE)] _EmissionGradientTex ("Gradient LUT", 2D) = "white" {}
		[ShowIfAny(_EMISSIONTEXTURE_SIMPLE)] _EmissionTex ("Emission Texture", 2D) = "white" {}
		[VectorShowIfAny(2, _EMISSIONTEXTURE_SIMPLE)] _EmissionTexSpeed ("Texture Speed", Vector) = (0,0,0,0)
		[ToggleShowIfAny(EMISSION_TWICE, _EMISSIONTEXTURE_SIMPLE)] _EmissionSampleTwice ("Sample Twice", Float) = 0
		[SpaceShowIfAll(6, EMISSION_TWICE, _EMISSIONTEXTURE_SIMPLE)] [VectorShowIfAny(2, 2, EMISSION_TWICE, _EMISSIONTEXTURE_SIMPLE)] _Emission2Tiling ("2nd Sample Tiling", Vector) = (1,1,0,0)
		[VectorShowIfAny(2, 2, EMISSION_TWICE, _EMISSIONTEXTURE_SIMPLE)] _Emission2Speed ("2nd Sample Speed", Vector) = (0,0,0,0)
		[ShowIfAny(_EMISSIONTEXTURE_PULSE)] _PulseMask ("Pulse Mask", 2D) = "white" {}
		[ToggleShowIfAny(INVERT_PULSE, _EMISSIONTEXTURE_PULSE)] _InvertPulseTexture ("Invert Texture", Float) = 0
		[ToggleShowIfAny(PULSE_MULTIPLY_TEXTURE, _EMISSIONTEXTURE_PULSE)] _PulseMultiplyByTexture ("Brightness from Texture", Float) = 0
		[ShowIfAny(_EMISSIONTEXTURE_PULSE)] _PulseWidth ("Pulse Width", Float) = 0.1
		[ShowIfAny(_EMISSIONTEXTURE_PULSE)] _PulseSpeed ("Pulse Speed", Float) = 0.2
		[ShowIfAny(_EMISSIONTEXTURE_PULSE)] _PulseSmooth ("Pulse Smooth", Range(0, 0.2)) = 0.02
		[SpaceShowIfAny(12, _EMISSIONTEXTURE_SIMPLE, _EMISSIONTEXTURE_PULSE)] [ToggleShowIfAny(EMISSION_MASK, _EMISSIONTEXTURE_SIMPLE, _EMISSIONTEXTURE_PULSE)] _EnableEmissionMask ("Use Emission Mask", Float) = 0
		[ShowIfAny(1, EMISSION_MASK, _EMISSIONTEXTURE_PULSE, _EMISSIONTEXTURE_SIMPLE)] _EmissionMask ("Mask Texture", 2D) = "white" {}
		[VectorShowIfAny(2, 1, EMISSION_MASK, _EMISSIONTEXTURE_PULSE, _EMISSIONTEXTURE_SIMPLE)] _EmissionMaskSpeed ("Mask Texture Speed", Vector) = (0,1,0,0)
		[Space(12)] [EnumHeader(None, Lerp, Additive)] _RimLight ("Rim Light Type", Float) = 0
		[ShowIfAny(_RIMLIGHT_LERP, _RIMLIGHT_ADDITIVE)] _RimLightEdgeStart ("Rim Light Edge Start", Float) = 0.5
		[ShowIfAny(_RIMLIGHT_LERP, _RIMLIGHT_ADDITIVE)] _RimLightColor ("Rim Light Color", Vector) = (1,1,1,0)
		[ShowIfAny(_RIMLIGHT_LERP, _RIMLIGHT_ADDITIVE)] _RimLightIntensity ("Rim Light Intensity", Float) = 1
		[Space(18)] [ToggleHeader(DECAL_TEXTURE)] _EnableDecalTexture ("Enable Decal Texture", Float) = 0
		[ShowIfAny(DECAL_TEXTURE)] _DecalTexture ("Decal Texture", 2D) = "black" {}
		[EnumShowIf(AlphaBlend, Additive, DECAL_TEXTURE)] _DecalBlend ("Decal Blend Type", Float) = 0
		[ToggleShowIfAny(DECAL_REFLECTION, DECAL_TEXTURE)] _DecalReflection ("Use Reflection Texture", Float) = 0
		[BigHeader(LIGHTNING)] [Space(12)] [ToggleHeader(DIFFUSE)] _EnableDiffuse ("Enable Diffuse", Float) = 1
		[ToggleShowIfAny(LIGHT_FALLOFF, DIFFUSE, SPECULAR)] _EnableLightFalloff ("Enable Light Falloff", Float) = 0
		[SpaceShowIfAny(12, DIFFUSE)] [ToggleShowIfAny(INVERT_DIFFUSE_NORMAL, DIFFUSE)] _InvertDiffuseNormal ("Invert Diffuse Normal", Float) = 0
		[ToggleShowIfAny(BOTH_SIDES_DIFFUSE, DIFFUSE)] _EnableBothSidesDiffuse ("Enable Both Sides Diffuse", Float) = 0
		[Space(12)] [ToggleHeader(PRIVATE_POINT_LIGHT)] _PrivatePointLight ("Private Point Light", Float) = 0
		[ShowIfAny(PRIVATE_POINT_LIGHT)] [HDR] _PrivatePointLightColor ("Color", Vector) = (1,0,0,0)
		[ToggleShowIfAny(POINT_LIGHT_IS_LOCAL, PRIVATE_POINT_LIGHT)] _PointLightPositionLocal ("Make Position Local", Float) = 0
		[ShowIfAny(PRIVATE_POINT_LIGHT)] _PrivatePointLightPosition ("Light World Position", Vector) = (0,0,0,1)
		[Space(12)] [ToggleHeader(DIFFUSE_TEXTURE)] _EnableDiffuseTexture ("Enable Albedo Texture", Float) = 0
		[ShowIfAny(DIFFUSE_TEXTURE)] _DiffuseTexture ("Diffuse Texture", 2D) = "white" {}
		[Space(12)] [ToggleHeader(SPECULAR)] _EnableSpecular ("Enable Specular", Float) = 1
		[ShowIfAny(SPECULAR)] _SpecularIntensity ("Specular Intensity", Float) = 1
		[Space(12)] [ToggleHeader(LIGHTMAP)] _EnableLightmap ("Enable Lightmap", Float) = 0
		[Space(12)] [ToggleHeader(NORMAL_MAP)] _EnableNormalMap ("Enable Normal Map", Float) = 0
		[ShowIfAny(NORMAL_MAP)] _NormalTexture ("Normal Texture", 2D) = "bump" {}
		[ShowIfAny(NORMAL_MAP)] _NormalScale ("Normal Scale", Float) = 1
		[Space(12)] [ToggleHeader(USE_SPHERICAL_NORMAL_OFFSET)] _UseSphericalNormalOffset ("Spherical Normal Offset", Float) = 0
		[ShowIfAny(USE_SPHERICAL_NORMAL_OFFSET)] _SphericalNormalOffsetIntensity ("Spherical Normal Offset Intensity", Float) = 0.5
		[ShowIfAny(USE_SPHERICAL_NORMAL_OFFSET)] _SphericalNormalOffsetCenter ("Spherical Normal Offset Center", Vector) = (0,0,0,1)
		[BigHeader(REFLECTIONS)] [Space(12)] [ToggleHeader(REFLECTION_TEXTURE)] _EnableReflectionTexture ("Enable Reflection Texture", Float) = 0
		[InfoBox(Enabled by Decal Reflection, 2, DECAL_REFLECTION, 0REFLECTION_TEXTURE)] [ShowIfAny(REFLECTION_TEXTURE, DECAL_REFLECTION)] _ReflectionTexIntensity ("Texture Intensity", Float) = 1
		[ShowIfAny(REFLECTION_TEXTURE, DECAL_REFLECTION)] _EnvironmentReflectionCube ("Environment Reflection", Cube) = "" {}
		[Space(12)] [ToggleHeader(REFLECTION_PROBE)] _EnableReflectionProbe ("Enable Reflection Probe", Float) = 0
		[ShowIfAny(REFLECTION_PROBE)] _ReflectionProbeIntensity ("Probe Intensity", Float) = 1
		[ToggleShowIfAny(REFLECTION_PROBE_BOX_PROJECTION, REFLECTION_PROBE)] _ReflectionProbeBoxProjection ("Box Projection", Float) = 1
		[ToggleShowIfAny(REFLECTION_PROBE_BOX_PROJECTION_OFFSET, 2, REFLECTION_PROBE, REFLECTION_PROBE_BOX_PROJECTION)] _EnableBoxProjectionOffset ("Enable Box Projection Offset", Float) = 0
		[ShowIfAny(3, REFLECTION_PROBE, REFLECTION_PROBE_BOX_PROJECTION, REFLECTION_PROBE_BOX_PROJECTION_OFFSET)] _ReflectionProbeBoxProjectionSizeOffset ("Box Projection Size Offset", Vector) = (0,0,0,0)
		[ShowIfAny(3, REFLECTION_PROBE, REFLECTION_PROBE_BOX_PROJECTION, REFLECTION_PROBE_BOX_PROJECTION_OFFSET)] _ReflectionProbeBoxProjectionPositionOffset ("Box Projection Position Offset", Vector) = (0,0,0,0)
		[SpaceShowIfAny(12, REFLECTION_PROBE, REFLECTION_TEXTURE, DECAL_REFLECTION)] [ToggleHeader(ENABLE_RIM_DIM, REFLECTION_PROBE, REFLECTION_TEXTURE, DECAL_REFLECTION)] _EnableRimDim ("Reflection Rim Dim", Float) = 0
		[ShowIfAny(ENABLE_RIM_DIM)] _RimScale ("Rim Scale", Float) = 1
		[ShowIfAny(ENABLE_RIM_DIM)] _RimOffset ("Rim Offset", Float) = 1
		[ShowIfAny(ENABLE_RIM_DIM)] _RimCameraDistanceOffset ("Rim Camera Distance Offset", Float) = 2
		[ShowIfAny(ENABLE_RIM_DIM)] _RimCameraDistanceScale ("Rim Camera Distance Scale", Float) = 0.3
		[ShowIfAny(ENABLE_RIM_DIM)] _RimDarkening ("Rim Darkenning", Float) = 0
		[ToggleShowIfAny(INVERT_RIM_DIM, ENABLE_RIM_DIM)] _InvertRimDim ("Invert Rim Dim", Float) = 0
		[BigHeader(DIRT AND GROUND FADE)] [Space(12)] [ToggleHeader(GROUND_FADE)] _EnableGroundFade ("Enable Ground Fade", Float) = 0
		[ShowIfAny(GROUND_FADE)] _GroundFadeScale ("Ground Fade Scale", Float) = 0.5
		[ShowIfAny(GROUND_FADE)] _GroundFadeOffset ("Ground Fade Offset", Float) = 1
		[Space(12)] [ToggleHeader(DIRT)] _EnableDirt ("Enable Dirt", Float) = 0
		[ShowIfAny(DIRT)] _DirtTex ("Dirt Texture", 2D) = "white" {}
		[ShowIfAny(DIRT)] _DirtIntensity ("Dirt Intensity", Float) = 1
		[Space(12)] [ToggleHeader(DIRT_DETAIL)] _EnableDirtDetail ("Enable Dirt Detail", Float) = 0
		[ShowIfAny(DIRT_DETAIL)] _DirtDetailTex ("Dirt Detail Texture", 2D) = "white" {}
		[ShowIfAny(DIRT_DETAIL)] _DirtDetailIntensity ("Dirt Detail Intensity", Float) = 1
		[BigHeader(OTHER)] [Space(16)] [EnumHeader(None, 90_CW, 90_CCW, 180_CW)] _Rotate_UV ("Rotate UVs", Float) = 0
		[Space(12)] [ToggleHeader(FOG)] _EnableFog ("Enable Fog", Float) = 1
		[ShowIfAny(FOG)] _FogStartOffset ("Fog Start Offset", Float) = 0
		[ShowIfAny(FOG)] _FogScale ("Fog Scale", Float) = 1
		[ToggleShowIfAny(HEIGHT_FOG, FOG)] _EnableHeightFog ("Enable Height Fog", Float) = 0
		[Space(12)] [ToggleHeader(DISSOLVE)] _EnableDissolve ("Enable Dissolve", Float) = 0
		[EnumShowIf(LocalX, LocalY, LocalZ, WorldX, WorldY, WorldZ, UvX, UvY, DISSOLVE)] _DissolveAxis ("Dissolve Axis", Float) = 0
		[ShowIfAny(DISSOLVE)] _DissolveStartValue ("Dissolve Start Value", Float) = 0
		[ShowIfAny(DISSOLVE)] _DissolveEndValue ("Dissolve End Value", Float) = 10
		[ShowIfAny(DISSOLVE)] _DissolveProgress ("Dissolve Progress", Range(0, 1)) = 0
		[ShowIfAny(DISSOLVE)] _DissolveGradientWidth ("Dissolve Gradient Width", Float) = 5
		[ShowIfAny(DISSOLVE)] _DissolveColor ("Dissolve Color", Vector) = (0,1,1,0)
		[ShowIfAny(DISSOLVE)] _DissolveTexture ("Dissolve Texture", 2D) = "black" {}
		[ShowIfAny(DISSOLVE)] _DissolveTextureInfluence ("Texture Influence", Range(-1, 1)) = 0.2
		[Space(12)] [ToggleHeader(CUTOUT)] _EnableCutout ("Enable Cutout", Float) = 0
		[ShowIfAny(CUTOUT)] _GlowCutoutColor ("Glow Cutout Color", Vector) = (1,1,1,1)
		[ShowIfAny(CUTOUT)] _CutoutTexScale ("Cutout Texture Scale", Float) = 1
		[Space(12)] [Header(Other)] [Space] [KeywordEnum(None, MainEffect, Always)] _WhiteBoostType ("White Boost", Float) = 0
		[Toggle(NOISE_DITHERING)] _EnableNoiseDithering ("Noise Dithering", Float) = 1
		[Toggle(LINEAR_TO_GAMMA)] _LinearToGamma ("LinearToGamma", Float) = 0
		[BigHeader(SETTINGS)] [Space(12)] [Enum(UnityEngine.Rendering.CullMode)] _Cull ("Cull", Float) = 2
		[Toggle] _ZWrite ("Z Write", Float) = 1
		_StencilRefValue ("Stencil Ref Value", Float) = 0
		[Enum(UnityEngine.Rendering.CompareFunction)] _StencilComp ("Stencil Comp Func", Float) = 8
		[Enum(UnityEngine.Rendering.StencilOp)] _StencilPass ("Stencill Pass Op", Float) = 0
		[Space(12)] [Enum(UnityEngine.Rendering.BlendMode)] _BlendSrcFactor ("Blend Src Factor", Float) = 5
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendDstFactor ("Blend Dst Factor", Float) = 10
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendSrcFactorA ("Blend Src Factor A", Float) = 0
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendDstFactorA ("Blend Dst Factor A", Float) = 0
	}
	SubShader {
		Tags { "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		Pass {
			Tags { "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			Blend Zero Zero, Zero Zero
			ZWrite Off
			Cull Off
			Stencil {
				Comp Always
				Pass Keep
				Fail Keep
				ZFail Keep
			}
			GpuProgramID 16110
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float3 texcoord1 : TEXCOORD1;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			// $Globals ConstantBuffers for Fragment Shader
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			CBUFFER_START(Props)
				float4 _Color;
			CBUFFER_END
			// Texture params for Vertex Shader
			// Texture params for Fragment Shader
			
			// Keywords: _DECALBLEND_ALPHABLEND _EMISSIONCOLORTYPE_FLAT
			v2f vert(appdata_full v)
			{
                v2f o;
                float4 tmp0;
                float4 tmp1;
                tmp0 = v.vertex.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp0 = unity_ObjectToWorld._m00_m10_m20_m30 * v.vertex.xxxx + tmp0;
                tmp0 = unity_ObjectToWorld._m02_m12_m22_m32 * v.vertex.zzzz + tmp0;
                tmp0 = unity_ObjectToWorld._m03_m13_m23_m33 * v.vertex.wwww + tmp0;
                tmp1 = tmp0.yyyy * unity_MatrixVP._m01_m11_m21_m31;
                tmp1 = unity_MatrixVP._m00_m10_m20_m30 * tmp0.xxxx + tmp1;
                tmp1 = unity_MatrixVP._m02_m12_m22_m32 * tmp0.zzzz + tmp1;
                o.position = unity_MatrixVP._m03_m13_m23_m33 * tmp0.wwww + tmp1;
                o.texcoord1.xyz = tmp0.xyz;
                return o;
			}
			// Keywords: _DECALBLEND_ALPHABLEND _EMISSIONCOLORTYPE_FLAT
			fout frag(v2f inp)
			{
                fout o;
                o.sv_target = _Color;
                return o;
			}
			ENDCG
		}
		Pass {
			Tags { "LIGHTMODE" = "SHADOWCASTER" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			Stencil {
				Comp Always
				Pass Keep
				Fail Keep
				ZFail Keep
			}
			GpuProgramID 68001
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float3 texcoord1 : TEXCOORD1;
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
                float4 tmp0;
                float4 tmp1;
                tmp0 = v.vertex.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp0 = unity_ObjectToWorld._m00_m10_m20_m30 * v.vertex.xxxx + tmp0;
                tmp0 = unity_ObjectToWorld._m02_m12_m22_m32 * v.vertex.zzzz + tmp0;
                tmp1 = tmp0 + unity_ObjectToWorld._m03_m13_m23_m33;
                o.texcoord1.xyz = unity_ObjectToWorld._m03_m13_m23 * v.vertex.www + tmp0.xyz;
                tmp0 = tmp1.yyyy * unity_MatrixVP._m01_m11_m21_m31;
                tmp0 = unity_MatrixVP._m00_m10_m20_m30 * tmp1.xxxx + tmp0;
                tmp0 = unity_MatrixVP._m02_m12_m22_m32 * tmp1.zzzz + tmp0;
                o.position = unity_MatrixVP._m03_m13_m23_m33 * tmp1.wwww + tmp0;
                return o;
			}
			// Keywords: 
			fout frag(v2f inp)
			{
                fout o;
                o.sv_target = float4(0.0, 0.0, 0.0, 1.0);
                return o;
			}
			ENDCG
		}
	}
}