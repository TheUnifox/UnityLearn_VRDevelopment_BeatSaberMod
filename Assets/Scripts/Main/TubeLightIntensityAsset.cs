// Decompiled with JetBrains decompiler
// Type: TubeLightIntensityAsset
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class TubeLightIntensityAsset : PlayableAsset, ITimelineClipAsset
{
  public TubeLightIntensityBehaviour _template;

  public ClipCaps clipCaps => ClipCaps.AutoScale | ClipCaps.Extrapolation;

  public override Playable CreatePlayable(PlayableGraph graph, GameObject go) => (Playable) ScriptPlayable<TubeLightIntensityBehaviour>.Create(graph, this._template);
}
