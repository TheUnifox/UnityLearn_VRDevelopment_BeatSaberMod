Shader "Custom/CustomParticles" {
	Properties {
		_Color ("Color", Vector) = (1,1,1,1)
		[Space(12)] [ToggleHeader(SECONDARY_COLOR)] _EnableSecondaryColor ("Use Secondary Color", Float) = 0
		[ShowIfAny(SECONDARY_COLOR)] _SecondaryColor ("Secondary Color", Vector) = (1,1,1,1)
		[ShowIfAny(SECONDARY_COLOR)] _SecondaryColorTex ("Secondary Color Texture", 2D) = "white" {}
		[ShowIfAny(SECONDARY_COLOR)] _SecondaryColorPanning ("Secondary Color Panning", Vector) = (0,0,0,0)
		[Space(12)] [Toggle(TRAIL_UVS)] _EnableTrailUVs ("Enable Trail UVs", Float) = 0
		[Toggle(PARTICLE_VERTEX_STREAM)] _UsesParticleVertexStream ("Uses Particle Vertex Stream", Float) = 0
		[Space(12)] [ToggleHeader(VERTEX_DISPLACEMENT)] _VertexDisplacement ("Use Vertex Displacement", Float) = 0
		[ToggleShowIfAny(TRAIL_UVS_DISPLACEMENT, 2, VERTEX_DISPLACEMENT, TRAIL_UVS)] _DisplacementTrailUVs ("Use Trail UVs", Float) = 0
		[ShowIfAny(VERTEX_DISPLACEMENT)] _DisplacementTex ("Mask Texture", 2D) = "white" {}
		[ToggleShowIfAny(SPATIAL_DISPLACEMENT, 2, VERTEX_DISPLACEMENT, 0TRAIL_UVS)] _3DDisplacement ("3D Displacement", Float) = 0
		[ToggleShowIfAny(DISPLACEMENT_PER_PARTICLE_RANDOM, 2, VERTEX_DISPLACEMENT, PARTICLE_VERTEX_STREAM)] _DisplacementPerParticleRandomization ("Per Particle Randomization", Float) = 0
		[ShowIfAny(VERTEX_DISPLACEMENT)] _DisplacementStrength ("Strength", Float) = 0.1
		[ShowIfAny(2, VERTEX_DISPLACEMENT, SPATIAL_DISPLACEMENT)] _DisplacementAxes ("Per Axis Strength", Vector) = (1,1,1,0)
		[ShowIfAny(VERTEX_DISPLACEMENT)] _DisplacementPanningSpeed ("Panning Speed", Float) = 1
		[ShowIfAny(VERTEX_DISPLACEMENT)] _DisplacementPanning ("Panning", Vector) = (0,0,0,0)
		[Space(12)] _MainTex ("Main Texture", 2D) = "black" {}
		[ToggleShowIfAny(TRAIL_UVS_MAIN, TRAIL_UVS)] _MainTexTrailUVs ("Use Trail UVs", Float) = 0
		[Toggle(PIXELATE)] _Pixelate ("Pixelate", Float) = 0
		[VectorShowIfAny(2, PIXELATE)] _PixelateResolution ("Pixelate Resolution", Vector) = (64,64,0,0)
		[ToggleShowIfAny(TEXTURE_COLOR, 0TEXTURE_FLIPBOOK)] _EnableTextureColor ("Use Texture Color", Float) = 0
		[EnumShowIf(Alpha, Red, 0TEXTURE_COLOR)] _AlphaChannel ("Alpha Channel", Float) = 0
		[ToggleShowIfAny(MAIN_PER_PARTICLE_RANDOM, PARTICLE_VERTEX_STREAM)] _MainPerParticleRandomization ("Per Particle Randomization", Float) = 0
		_Intensity ("Intensity", Float) = 1
		_UvPanning ("UV Panning", Vector) = (0,0,0,0)
		[Space(12)] [ToggleHeader(TEXTURE_FLIPBOOK)] _UseTextureFlipbook ("Use Texture Flipbook", Float) = 0
		[InfoBox(Keep in sRGB or alpha will have different intensity, TEXTURE_FLIPBOOK)] [InfoBox(Frame 1 contains frames 1234 in RGBA channels Frame 2 contains 4567 (5678 if blending is disabled), TEXTURE_FLIPBOOK)] [ShowIfAny(TEXTURE_FLIPBOOK)] _FlipbookColumns ("Flipbook Columns", Float) = 8
		[ShowIfAny(TEXTURE_FLIPBOOK)] _FlipbookRows ("Flipbook Rows", Float) = 8
		[ShowIfAny(TEXTURE_FLIPBOOK)] _FlipbookNonloopableFrames ("Full Non-loopable frames", Float) = 0
		[ShowIfAny(TEXTURE_FLIPBOOK)] _FlipbookSpeed ("Flipbook Speed", Float) = 1
		[ToggleShowIfAny(FLIPBOOK_BLENDING_OFF, TEXTURE_FLIPBOOK)] _FlipbookBlendingOff ("No Frame Blending", Float) = 0
		[SpaceShowIfAny(12, TEXTURE_FLIPBOOK)] [InfoBox(Frame 1 contains frames 12 in RG BA channels Frame 2 contains 23, 2, MOTION_VECTORS, TEXTURE_FLIPBOOK)] [ToggleShowIfAny(MOTION_VECTORS, TEXTURE_FLIPBOOK)] _UseMotionVectors ("Use Motion Vectors", Float) = 0
		[ShowIfAny(2, MOTION_VECTORS, TEXTURE_FLIPBOOK)] _MotionVectorTex ("Motion Vector Texture", 2D) = "white" {}
		[ShowIfAny(2, MOTION_VECTORS, TEXTURE_FLIPBOOK)] _MotionVectorColumns ("Motion Vector Columns", Float) = 8
		[ShowIfAny(2, MOTION_VECTORS, TEXTURE_FLIPBOOK)] _MotionVectorRows ("Motion Vector Rows", Float) = 8
		[ShowIfAny(2, MOTION_VECTORS, TEXTURE_FLIPBOOK)] _MotionVectorSpeed ("Motion Vector Speed", Float) = 1
		[ShowIfAny(2, MOTION_VECTORS, TEXTURE_FLIPBOOK)] _MotionVectorIntensity ("Motion Vector Intensity", Float) = 1
		[Space(16)] [ToggleHeader(COLOR_GRADIENT)] _UseColor_Gradient ("Use Color Gradient", Float) = 0
		[ShowIfAny(COLOR_GRADIENT)] _ColorGradient ("Gradient LUT", 2D) = "white" {}
		[ToggleShowIfAny(GRADIENT_ALPHA, COLOR_GRADIENT)] _GradientUseAlpha ("Use Gradient Alpha", Float) = 0
		[ShowIfAny(COLOR_GRADIENT)] _GradientPosition ("Gradient Position", Range(0, 1)) = 0.5
		[ShowIfAny(COLOR_GRADIENT)] _GradientPanningSpeed ("Gradient Panning Speed", Float) = 0
		[Space(12)] [KeywordEnum(None, 90_CW, 90_CCW, 180_CW)] _Rotate_UV ("Rotate UVs", Float) = 0
		[ToggleShowIfAny(ROTATE_MAIN_ONLY, _ROTATE_UV_90_CW, _ROTATE_UV_90_90_CCW, _ROTATE_UV_180_CW)] _RotateMainUVOnly ("Rotate Main UV Only", Float) = 0
		[Space(12)] [ToggleHeader(MASK)] _EnableMask ("Use Mask", Float) = 0
		[ToggleShowIfAny(TRAIL_UVS_MASK, 2, MASK, TRAIL_UVS)] _MaskTrailUVs ("Use Trail UVs", Float) = 0
		[ToggleShowIfAny(MASK_RED_IS_ALPHA, MASK)] _MaskRedIsAlpha ("Red is Mask Alpha", Float) = 0
		[EnumShowIf(Multiply, Add, Masked Add, MASK)] _MaskBlend ("Mask Blend", Float) = 0
		[ShowIfAny(MASK)] _MaskTex ("Mask Texture", 2D) = "white" {}
		[ToggleShowIfAny(MASK_PER_PARTICLE_RANDOM, 2, MASK, PARTICLE_VERTEX_STREAM)] _MaskPerParticleRandomization ("Per Particle Randomization", Float) = 0
		[ShowIfAny(MASK)] _MaskStrength ("Mask Strength", Float) = 1
		[ShowIfAny(MASK)] _MaskPanning ("Mask Panning", Vector) = (0,0,0,0)
		[Space(12)] [ToggleHeader(MASK2)] _EnableMask2 ("Use Secondary Mask", Float) = 0
		[ToggleShowIfAny(TRAIL_UVS_MASK2, 2, MASK2, TRAIL_UVS)] _Mask2TrailUVs ("Use Trail UVs", Float) = 0
		[ToggleShowIfAny(MASK2_RED_IS_ALPHA, MASK2)] _Mask2RedIsAlpha ("Red is Mask Alpha", Float) = 0
		[EnumShowIf(Multiply, Add, Masked Add, MASK2)] _Mask2Blend ("Mask Blend", Float) = 0
		[ShowIfAny(MASK2)] _Mask2Tex ("Secondary Mask Texture", 2D) = "white" {}
		[ToggleShowIfAny(MASK2_PER_PARTICLE_RANDOM, 2, MASK2, PARTICLE_VERTEX_STREAM)] _Mask2ParticleRandomization ("Per Particle Randomization", Float) = 0
		[ShowIfAny(MASK2)] _Mask2Strength ("Secondary Mask Strength", Float) = 1
		[ShowIfAny(MASK2)] _Mask2Panning ("Secondary Mask Panning", Vector) = (0,0,0,0)
		[Space(12)] [EnumHeader(None, Simple, Flowmap)] Distortion ("Distortion", Float) = 0
		[SpaceShowIfAny(12, DISTORTION_FLOWMAP, DISTORTION_SIMPLE)] [EnumShowIf(3, Main, Mask, Mask2, DISTORTION_FLOWMAP, DISTORTION_SIMPLE)] Distortion_Target ("Distortion Target", Float) = 0
		[ToggleShowIfAny(TRAIL_UVS_FLOWMAP, 2, TRAIL_UVS, DISTORTION_FLOWMAP)] _FlowmapTrailUVs ("Use Trail UVs", Float) = 0
		[ShowIfAny(DISTORTION_FLOWMAP)] _FlowTex ("Flowmap Texture", 2D) = "black" {}
		[ToggleShowIfAny(FLOWMAP_PER_PARTICLE_RANDOM, 2, DISTORTION_FLOWMAP, PARTICLE_VERTEX_STREAM)] _FlowmapParticleRandomization ("Per Particle Randomization", Float) = 0
		[ShowIfAny(DISTORTION_FLOWMAP)] _FlowSpeed ("Flow Speed", Range(0, 1)) = 0.2
		[ShowIfAny(DISTORTION_FLOWMAP)] _FlowStrength ("Flow Strength", Range(-1, 1)) = 0.2
		[VectorShowIfAny(2, DISTORTION_FLOWMAP)] _FlowAdd ("Flowmap Additional Direction", Vector) = (0,0,0,0)
		[ShowIfAny(DISTORTION_FLOWMAP)] _FlowPanning ("Flowmap Panning", Vector) = (0,0,0,0)
		[SpaceShowIfAny(12, DISTORTION_FLOWMAP)] [ToggleShowIfAny(TRAIL_UVS_DISTORTION, 2, TRAIL_UVS, DISTORTION_SIMPLE)] _DistortionTrailUVs ("Use Distortion UVs", Float) = 0
		[ToggleShowIfAny(DISTORTION_PER_PARTICLE_RANDOM, 2, DISTORTION_SIMPLE, PARTICLE_VERTEX_STREAM)] _DistortionParticleRandomization ("Per Particle Randomization", Float) = 0
		[ShowIfAny(DISTORTION_SIMPLE)] _DistortionTex ("Distortion Texture", 2D) = "black" {}
		[ShowIfAny(DISTORTION_SIMPLE)] _DistortionStrength ("Distortion Strength", Float) = 0.2
		[ShowIfAny(DISTORTION_SIMPLE)] _DistortionAxes ("Distortion Axes", Vector) = (1,1,0,0)
		[ShowIfAny(DISTORTION_SIMPLE)] _DistortionPanning ("Distortion Panning", Vector) = (0,0,0,0)
		[SpaceShowIfAny(12, DISTORTION_SIMPLE)] [Space(12)] [ToggleHeader(SOFT_PARTICLES)] _EnableSoftParticles ("Soft Particles", Float) = 0
		[ShowIfAny(SOFT_PARTICLES)] _SoftFactor ("Soft Factor", Range(0, 50)) = 0
		[Space(12)] [ToggleHeader(CLOSE_TO_CAMERA_DISAPPEAR)] _EnableCloseToCameraDisappear ("Close to Camera Dissapear", Float) = 0
		[ShowIfAny(CLOSE_TO_CAMERA_DISAPPEAR)] _CloseToCameraOffset ("Close to Camera Offset", Float) = 0.5
		[ShowIfAny(CLOSE_TO_CAMERA_DISAPPEAR)] _CloseToCameraFactor ("Close to Camera Factor", Float) = 0.5
		[Space(12)] [ToggleHeader(VIEW_ALIGN_DISAPPEAR)] _EnableViewAlignDisappear ("View Align Dissapear", Float) = 0
		[ToggleShowIfAny(SQUARE_ANGLE_FOR_VIEW_ALIGN_DISAPPEAR, VIEW_ALIGN_DISAPPEAR)] _SquareAngleForViewAlignDisappear ("Square Angle For View Align Disappear", Float) = 0
		[ShowIfAny(VIEW_ALIGN_DISAPPEAR)] _ViewAlignFactor ("View Align Factor", Float) = 1.5
		[Space(12)] [ToggleHeader(VERTEX_COLOR)] _EnableVertexColor ("Enable Vertex Color", Float) = 0
		[ToggleShowIfAny(VERTEX_SQUARE_ALPHA, VERTEX_COLOR)] _SquareVertexAlpha ("Square Vertex Alpha", Float) = 0
		[ToggleShowIfAny(VERTEX_RED_IS_ALPHA, VERTEX_COLOR)] _RedIsVertexAlpha ("Red is Vertex Alpha", Float) = 0
		[EnumShowIf(RGBA, A, RGB, VERTEX_COLOR)] _VertexChannels ("Vertex Channels", Float) = 0
		[ToggleShowIfAny(LIFETIME, VERTEX_COLOR)] _EnableLifetime ("Enable Lifetime Alpha", Float) = 0
		[Space(12)] [ToggleHeader(VERTEX_FLIPBOOK)] _EnableVertexFlipbook ("Enable Vertex Flipbook", Float) = 0
		[InfoBox(Frame in red color _ Offset to randomize in green color, VERTEX_FLIPBOOK)] [ShowIfAny(VERTEX_FLIPBOOK)] _VertexFlipbookCount ("Frame Count", Float) = 1
		[ShowIfAny(VERTEX_FLIPBOOK)] _VertexFlipbookSpeed ("Flipbook Speed", Float) = 1
		[ToggleShowIfAny(VERTEX_FLIPBOOK_FADE, VERTEX_FLIPBOOK)] _EnableVertexFlipbookFade ("Enable Flipbook Fade", Float) = 0
		[Space(12)] [EnumHeader(None, Alpha, Color, Lerp)] _FogType ("Fog Type", Float) = 0
		[ShowIfAny(_FOGTYPE_ALPHA, _FOGTYPE_LERP, _FOGTYPE_COLOR)] _FogStartOffset ("Fog Start Offset", Float) = 0
		[ShowIfAny(_FOGTYPE_ALPHA, _FOGTYPE_LERP, _FOGTYPE_COLOR)] _FogScale ("Fog Scale", Float) = 1
		[ToggleShowIfAny(HEIGHT_FOG, _FOGTYPE_ALPHA, _FOGTYPE_LERP, _FOGTYPE_COLOR)] _EnableHeightFog ("Enable Height Fog", Float) = 0
		[ShowIfAny(HEIGHT_FOG)] _FogHeightScale ("Fog Height Scale", Float) = 1
		[ShowIfAny(HEIGHT_FOG)] _FogHeightOffset ("Fog Height Offset", Float) = 0
		[ToggleShowIfAny(PRECISE_FOG, _FOGTYPE_ALPHA, _FOGTYPE_LERP,  _FOGTYPE_COLOR)] _PreciseFog ("High (Frag) Precision", Float) = 0
		[Space(12)] [ToggleHeader(REVEAL)] _EnableReveal ("Use Reveal", Float) = 0
		[Space(12)] [ToggleHeader(FAKE_MIRROR_TRANSPARENCY)] _FakeMirrorTransparencyEnabled ("Fake Mirror Transparency", Float) = 0
		[ShowIfAny(FAKE_MIRROR_TRANSPARENCY)] _FakeMirrorTransparencyMultiplier ("Mirror Transparency Multiplier", Float) = 1
		[Space(12)] [ToggleHeader(HOLOGRAM)] _EnableHologram ("Hologram effect", Float) = 0
		[ShowIfAny(HOLOGRAM)] _HologramColor ("Hologram Color", Vector) = (1,1,1,1)
		[Space(12)] [ToggleHeader(DISSOLVE)] _EnableDissolve ("Enable Dissolve", Float) = 0
		[EnumShowIf(LocalX, LocalY, LocalZ, WorldX, WorldY, WorldZ, VertexRed, UvX, UvY, DISSOLVE)] _DissolveAxis ("Dissolve Axis", Float) = 0
		[ShowIfAny(DISSOLVE)] _DissolveStartValue ("Dissolve Start Value", Float) = 0
		[ShowIfAny(DISSOLVE)] _DissolveEndValue ("Dissolve End Value", Float) = 10
		[ToggleShowIfAny(DISSOLVE_PROGRESS_FROM_VERTEX_ALPHA, 3, DISSOLVE, VERTEX_COLOR, 0_VERTEXCHANNELS_RGB)] _DissolveProgressFromVertexAlpha ("Get Progress from Vertex Alpha", Float) = 0
		[ShowIfAny(2, DISSOLVE, 0DISSOLVE_PROGRESS_FROM_VERTEX_ALPHA)] _DissolveProgress ("Dissolve Progress", Range(0, 1)) = 0
		[ToggleShowIfAny(DISSOLVE_INVERT, DISSOLVE)] _InvertDissolve ("Invert Dissolve", Float) = 0
		[ShowIfAny(DISSOLVE)] _DissolveGradientWidth ("Dissolve Gradient Width", Float) = 5
		[ToggleShowIfAny(DISSOLVE_COLOR, DISSOLVE)] _UseDissolveColor ("Use Dissolve Color", Float) = 0
		[ShowIfAny(2, DISSOLVE, DISSOLVE_COLOR)] _DissolveColor ("Dissolve Color", Vector) = (0,1,1,0)
		[ToggleShowIfAny(DISSOLVE_TEXTURE, DISSOLVE)] _UseDissolveTexture ("Use Dissolve Texture", Float) = 0
		[ShowIfAny(2, DISSOLVE, DISSOLVE_TEXTURE)] _DissolveTexture ("Dissolve Texture", 2D) = "black" {}
		[ShowIfAny(2, DISSOLVE, DISSOLVE_TEXTURE)] _DissolveTextureInfluence ("Texture Influence", Range(-1, 1)) = 0.2
		[Space()] [Header(Other)] [Space] [Toggle(SQUARE_ALPHA)] _SquareAlpha ("Square Alpha", Float) = 1
		[KeywordEnum(None, Full, Y Axis)] _Billboard ("Billboard", Float) = 0
		[InfoBox(Scale XZ should be always equal, _BILLBOARD_Y_AXIS)] [ShowIfAny(_BILLBOARD_FULL)] _BillboardScale ("Billboard Scale", Float) = 1
		_AlphaMultiplier ("Alpha Multiplier", Float) = 1
		[KeywordEnum(None, MainEffect, Always)] _WhiteBoostType ("White Boost", Float) = 0
		[Toggle(REMAP_WHITEBOOST_START)] _RemapWhiteboostStart ("Remap Whiteboost Start", Float) = 0
		[ShowIfAny(REMAP_WHITEBOOST_START)] _WhiteboostRemapStart ("Alpha for no Whiteboost", Range(0, 1)) = 0
		[SpaceShowIfAny(12, REMAP_WHITEBOOST_START)] [Toggle(NOISE_DITHERING)] _EnableNoiseDithering ("Noise Dithering", Float) = 0
		[Toggle(SONG_TIME)] _UseSongTime ("Use song time", Float) = 0
		[Toggle(CUTOUT)] _EnableCutout ("Enable Cutout", Float) = 0
		[ShowIfAny(CUTOUT)] _CutoutThreshold ("Cutout Threshold", Float) = 0.5
		[Toggle(MIPMAP_BIAS)] _UseMipmapBias ("Mipmap Bias", Float) = 0
		[InfoBox(Only applies to Main Texture, MIPMAP_BIAS)] [ShowIfAny(MIPMAP_BIAS)] _MipmapBias ("Bias Value", Float) = 0
		[Toggle(COLOR_ARRAY)] _UseColorArray ("Use Color Array", Float) = 0
		[Toggle(SPECTROGRAM)] _UseSpectrogram ("Use Spectrogram", Float) = 0
		[ShowIfAny(SPECTROGRAM)] _SpectrogramBaseValue ("Spectrogram Base Value", Range(0, 1)) = 0.2
		[ShowIfAny(SPECTROGRAM)] _SpectrogramRange ("Spectrogram Range", Range(0, 1)) = 0.2
		[Space(12)] [Header(Color Blending)] [Space] [Enum(UnityEngine.Rendering.BlendMode)] _BlendSrcFactor ("Foreground Factor", Float) = 1
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendDstFactor ("Background Factor", Float) = 1
		[Space] [InfoBox(Support on Quest ends after LogicalClear)] [Enum(UnityEngine.Rendering.BlendOp)] _BlendOp ("Blend Operation", Float) = 0
		[Header(Bloom Blending)] [Space] [Enum(UnityEngine.Rendering.BlendMode)] _BlendSrcFactorA ("Foreground Factor", Float) = 0
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendDstFactorA ("Background Factor", Float) = 1
		[Space()] [Header(Settings)] [Space] [Enum(UnityEngine.Rendering.CullMode)] _CullMode ("Cull Mode", Float) = 0
		[Toggle] _CustomZWrite ("Z Write", Float) = 0
		[Space] [Enum(UnityEngine.Rendering.CompareFunction)] _ZTest ("ZTest", Float) = 4
		[Space] _OffsetFactor ("Offset Factor", Float) = 0
		_OffsetUnits ("Offset Units", Float) = 0
		[Space] _StencilRefValue ("Stencil Ref Value", Float) = 0
		[Enum(UnityEngine.Rendering.CompareFunction)] _StencilComp ("Stencil Comp Func", Float) = 8
		[Enum(UnityEngine.Rendering.StencilOp)] _StencilPass ("Stencill Pass Op", Float) = 0
	}
	SubShader {
		Tags { "CanUseSpriteAtlas" = "true" "IGNOREPROJECTOR" = "true" "PreviewType" = "Plane" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		Pass {
			Tags { "CanUseSpriteAtlas" = "true" "IGNOREPROJECTOR" = "true" "PreviewType" = "Plane" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			Blend Zero Zero, Zero Zero
			ZWrite Off
			Cull Off
			Stencil {
				Comp Always
				Pass Keep
				Fail Keep
				ZFail Keep
			}
			GpuProgramID 40825
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
			float2 _UvPanning;
			// $Globals ConstantBuffers for Fragment Shader
			float _Intensity;
			float _AlphaMultiplier;
			// Custom ConstantBuffers for Vertex Shader
			CBUFFER_START(Props)
				float _TimeOffset;
			CBUFFER_END
			// Custom ConstantBuffers for Fragment Shader
			CBUFFER_START(Props)
				float4 _Color;
			CBUFFER_END
			// Texture params for Vertex Shader
			// Texture params for Fragment Shader
			sampler2D _MainTex;
			
			// Keywords: DISTORTION_NONE DISTORTION_TARGET_MAIN _MASK2BLEND_MULTIPLY _MASKBLEND_MULTIPLY _VERTEXCHANNELS_RGBA
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
                tmp0.x = _Time.y + _TimeOffset;
                tmp0.xy = tmp0.xx * _UvPanning;
                tmp0.zw = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
                o.texcoord.xy = tmp0.xy * _MainTex_ST.xy + tmp0.zw;
                return o;
			}
			// Keywords: DISTORTION_NONE DISTORTION_TARGET_MAIN _MASK2BLEND_MULTIPLY _MASKBLEND_MULTIPLY _VERTEXCHANNELS_RGBA
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                tmp0 = tex2D(_MainTex, inp.texcoord.xy);
                tmp0.x = _Intensity;
                tmp0 = tmp0.xxxw * _Color;
                tmp0.w = tmp0.w * _AlphaMultiplier;
                o.sv_target.xyz = tmp0.www * tmp0.xyz;
                o.sv_target.w = tmp0.w;
                return o;
			}
			ENDCG
		}
	}
}