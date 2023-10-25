// Decompiled with JetBrains decompiler
// Type: ToneMappingExtensions
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using UnityEngine;

public static class ToneMappingExtensions
{
  [DoesNotRequireDomainReloadInit]
  private static readonly string[] _shaderKeywordMap = new string[2]
  {
    "",
    "ACES_TONE_MAPPING"
  };

  public static void SetShaderKeyword(this ToneMapping toneMapping)
  {
    if (toneMapping == ToneMapping.Aces)
      Shader.EnableKeyword(ToneMappingExtensions._shaderKeywordMap[1]);
    else
      Shader.DisableKeyword(ToneMappingExtensions._shaderKeywordMap[1]);
  }
}
