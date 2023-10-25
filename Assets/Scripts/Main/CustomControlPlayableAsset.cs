// Decompiled with JetBrains decompiler
// Type: CustomControlPlayableAsset
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine.Timeline;

[Serializable]
public class CustomControlPlayableAsset : ControlPlayableAsset, ITimelineClipAsset
{
  public AvatarColorBehaviour _template;

  public new ClipCaps clipCaps => ClipCaps.Looping | ClipCaps.Extrapolation | ClipCaps.ClipIn | ClipCaps.SpeedMultiplier;
}
