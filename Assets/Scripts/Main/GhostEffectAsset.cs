// Decompiled with JetBrains decompiler
// Type: GhostEffectAsset
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class GhostEffectAsset : PlayableAsset, ITimelineClipAsset
{
  public GhostEffectBehaviour.GhostEffectType _ghostEffectType;
  [NullAllowed]
  public ExposedReference<TimelineArrayReference> arrayReference;
  public GhostEffectBehaviour template;

  public ClipCaps clipCaps => ClipCaps.AutoScale | ClipCaps.Extrapolation;

  public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
  {
    TimelineArrayReference timelineArrayReference = this.arrayReference.Resolve(graph.GetResolver());
    if (timelineArrayReference == null)
      return new Playable();
    this.template._ghostEffectType = this._ghostEffectType;
    this.template._ghostEffectTransform = timelineArrayReference.transform;
    if (this._ghostEffectType == GhostEffectBehaviour.GhostEffectType.TextMeshPro)
      this.template.textMeshPros = timelineArrayReference._tmproArray;
    else
      this.template._canvasGroups = timelineArrayReference._canvasGroupArray;
    return (Playable) ScriptPlayable<GhostEffectBehaviour>.Create(graph, this.template);
  }
}
