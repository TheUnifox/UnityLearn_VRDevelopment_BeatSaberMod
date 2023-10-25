// Decompiled with JetBrains decompiler
// Type: SaberSwingRating
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public abstract class SaberSwingRating
{
  public const float kMaxNormalAngleDiff = 90f;
  public const float kToleranceNormalAngleDiff = 75f;
  public const float kMaxBeforeCutSwingDuration = 0.4f;
  public const float kMaxAfterCutSwingDuration = 0.4f;
  public const float kBeforeCutAngleFor1Rating = 100f;
  public const float kAfterCutAngleFor1Rating = 60f;

  private static float NormalRating(float normalDiff) => 1f - Mathf.Clamp((float) (((double) normalDiff - 75.0) / 15.0), 0.0f, 1f);

  public static float BeforeCutStepRating(float angleDiff, float normalDiff) => (float) ((double) angleDiff * (double) SaberSwingRating.NormalRating(normalDiff) / 100.0);

  public static float AfterCutStepRating(float angleDiff, float normalDiff) => (float) ((double) angleDiff * (double) SaberSwingRating.NormalRating(normalDiff) / 60.0);
}
