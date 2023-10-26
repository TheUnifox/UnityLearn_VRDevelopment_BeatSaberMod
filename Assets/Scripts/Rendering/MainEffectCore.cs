// Decompiled with JetBrains decompiler
// Type: MainEffectCore
// Assembly: Rendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5B7D31E1-1F9E-4BE6-B735-D2EB47EFDA46
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Rendering.dll

using UnityEngine;

public class MainEffectCore
{
  [DoesNotRequireDomainReloadInit]
  private static readonly int _baseColorBoostID = Shader.PropertyToID("_BaseColorBoost");
  [DoesNotRequireDomainReloadInit]
  private static readonly int _baseColorBoostThresholdID = Shader.PropertyToID("_BaseColorBoostThreshold");

  public static void SetGlobalShaderValues(float baseColorBoost, float baseColorBoostThreshold)
  {
    Shader.SetGlobalFloat(MainEffectCore._baseColorBoostID, baseColorBoost);
    Shader.SetGlobalFloat(MainEffectCore._baseColorBoostThresholdID, baseColorBoostThreshold);
  }
}
