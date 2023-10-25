// Decompiled with JetBrains decompiler
// Type: SliderShaderHelper
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public abstract class SliderShaderHelper
{
  [DoesNotRequireDomainReloadInit]
  private static readonly int colorPropertyId = Shader.PropertyToID("_Color");
  [DoesNotRequireDomainReloadInit]
  private static readonly int saberAttractionPointPropertyId = Shader.PropertyToID("_SaberAttractionPoint");
  [DoesNotRequireDomainReloadInit]
  private static readonly int timeSinceHeadNoteJumpPropertyId = Shader.PropertyToID("_TimeSinceHeadNoteJump");
  [DoesNotRequireDomainReloadInit]
  private static readonly int headNoteJumpDataPropertyId = Shader.PropertyToID("_HeadNoteJumpData");
  [DoesNotRequireDomainReloadInit]
  private static readonly int tailNoteJumpDataPropertyId = Shader.PropertyToID("_TailNoteJumpData");
  [DoesNotRequireDomainReloadInit]
  private static readonly int jumpSpeedPropertyId = Shader.PropertyToID("_JumpSpeed");
  [DoesNotRequireDomainReloadInit]
  private static readonly int jumpDistancePropertyId = Shader.PropertyToID("_JumpDistance");
  [DoesNotRequireDomainReloadInit]
  private static readonly int randomPropertyId = Shader.PropertyToID("_Random");
  [DoesNotRequireDomainReloadInit]
  private static readonly int headFadeLengthPropertyId = Shader.PropertyToID("_HeadFadeLength");
  [DoesNotRequireDomainReloadInit]
  private static readonly int tailFadeLengthPropertyId = Shader.PropertyToID("_TailFadeLength");
  [DoesNotRequireDomainReloadInit]
  private static readonly int sliderZLengthPropertyId = Shader.PropertyToID("_SliderZLength");
  [DoesNotRequireDomainReloadInit]
  private static readonly int sliderLengthPropertyId = Shader.PropertyToID("_SliderLength");
  [DoesNotRequireDomainReloadInit]
  private static readonly int tailHeadNoteJumpOffsetDifferencePropertyId = Shader.PropertyToID("_TailHeadNoteJumpOffsetDifference");
  [DoesNotRequireDomainReloadInit]
  private static readonly int saberAttractionMultiplier = Shader.PropertyToID("_SaberAttractionMultiplier");

  public static void SetTimeSinceHeadNoteJump(
    MaterialPropertyBlock materialPropertyBlock,
    float time)
  {
    materialPropertyBlock.SetFloat(SliderShaderHelper.timeSinceHeadNoteJumpPropertyId, time);
  }

  public static void SetTailHeadNoteJumpOffsetDifference(
    MaterialPropertyBlock materialPropertyBlock,
    float tailHeadNoteJumpOffsetDifference)
  {
    materialPropertyBlock.SetFloat(SliderShaderHelper.tailHeadNoteJumpOffsetDifferencePropertyId, tailHeadNoteJumpOffsetDifference);
  }

  public static void SetSaberAttractionPoint(
    MaterialPropertyBlock materialPropertyBlock,
    Vector3 attractPoint)
  {
    materialPropertyBlock.SetVector(SliderShaderHelper.saberAttractionPointPropertyId, (Vector4) attractPoint);
  }

  public static void EnableSaberAttraction(
    MaterialPropertyBlock materialPropertyBlock,
    bool enableSaberAttraction)
  {
    materialPropertyBlock.SetFloat(SliderShaderHelper.saberAttractionMultiplier, enableSaberAttraction ? 1f : 0.0f);
  }

  public static void SetColor(MaterialPropertyBlock materialPropertyBlock, Color color) => materialPropertyBlock.SetColor(SliderShaderHelper.colorPropertyId, color);

  public static void SetInitialProperties(
    MaterialPropertyBlock materialPropertyBlock,
    Color sliderColor,
    float headNoteGravity,
    float tailNoteGravity,
    float noteJumpMovementSpeed,
    float jumpDistance,
    float sliderZLength,
    float sliderLength,
    bool hasHeadNote,
    bool hasTailNote,
    float randomValue)
  {
    float num = (float) ((double) jumpDistance / (double) noteJumpMovementSpeed * 0.5);
    float y1 = headNoteGravity * num;
    float y2 = tailNoteGravity * num;
    materialPropertyBlock.SetColor(SliderShaderHelper.colorPropertyId, sliderColor);
    materialPropertyBlock.SetVector(SliderShaderHelper.headNoteJumpDataPropertyId, (Vector4) new Vector3(headNoteGravity, y1, (float) ((double) y1 * (double) num * 0.5)));
    materialPropertyBlock.SetVector(SliderShaderHelper.tailNoteJumpDataPropertyId, (Vector4) new Vector3(tailNoteGravity, y2, (float) ((double) y2 * (double) num * 0.5)));
    materialPropertyBlock.SetFloat(SliderShaderHelper.jumpSpeedPropertyId, noteJumpMovementSpeed);
    materialPropertyBlock.SetFloat(SliderShaderHelper.jumpDistancePropertyId, jumpDistance);
    materialPropertyBlock.SetFloat(SliderShaderHelper.sliderZLengthPropertyId, sliderZLength);
    materialPropertyBlock.SetFloat(SliderShaderHelper.sliderLengthPropertyId, sliderLength);
    materialPropertyBlock.SetFloat(SliderShaderHelper.randomPropertyId, randomValue);
    materialPropertyBlock.SetFloat(SliderShaderHelper.headFadeLengthPropertyId, hasHeadNote ? 0.4f : 1f);
    materialPropertyBlock.SetFloat(SliderShaderHelper.tailFadeLengthPropertyId, hasTailNote ? 0.4f : 1f);
  }

  public static void SetInitialProperties(
    MaterialPropertyBlock materialPropertyBlock,
    SliderController sliderController,
    float noteJumpMovementSpeed)
  {
    SliderShaderHelper.SetInitialProperties(materialPropertyBlock, sliderController.initColor * sliderController.sliderIntensityEffect.colorIntensity, sliderController.sliderMovement.headNoteGravity, sliderController.sliderMovement.tailNoteGravity, noteJumpMovementSpeed, sliderController.jumpDistance, sliderController.zDistanceBetweenNotes, sliderController.sliderMeshController.pathLength, sliderController.sliderData.hasHeadNote, sliderController.sliderData.hasTailNote, sliderController.randomValue);
  }

  public static void UpdateMaterialPropertyBlock(
    MaterialPropertyBlock materialPropertyBlock,
    SliderController sliderController,
    float timeSinceHeadNoteJump,
    float jumpOffsetY)
  {
    SliderShaderHelper.SetTimeSinceHeadNoteJump(materialPropertyBlock, timeSinceHeadNoteJump);
    SliderShaderHelper.SetSaberAttractionPoint(materialPropertyBlock, sliderController.closeSmoothedSaberInteractionPos.GetValue(TimeHelper.interpolationFactor));
    SliderShaderHelper.SetColor(materialPropertyBlock, sliderController.initColor * sliderController.sliderIntensityEffect.colorIntensity);
    if ((double) timeSinceHeadNoteJump >= (double) sliderController.sliderDuration)
      return;
    SliderShaderHelper.SetTailHeadNoteJumpOffsetDifference(materialPropertyBlock, jumpOffsetY - sliderController.headJumpOffsetY);
  }
}
