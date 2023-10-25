// Decompiled with JetBrains decompiler
// Type: AvatarColorPropertyIds
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public abstract class AvatarColorPropertyIds
{
  [DoesNotRequireDomainReloadInit]
  public static readonly int colorPropertyId = Shader.PropertyToID("_Color");
  [DoesNotRequireDomainReloadInit]
  public static readonly int rimLightColorPropertyId = Shader.PropertyToID("_RimLightColor");
  [DoesNotRequireDomainReloadInit]
  public static readonly int uvColorsPropertyId = Shader.PropertyToID("_UVColors");
  [DoesNotRequireDomainReloadInit]
  public static readonly int uvRimLightColorsPropertyId = Shader.PropertyToID("_UVRimColors");
  [DoesNotRequireDomainReloadInit]
  public static readonly int segmentToHighlightPropertyId = Shader.PropertyToID("_SegmentToHighlight");
}
