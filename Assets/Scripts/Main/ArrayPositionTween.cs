// Decompiled with JetBrains decompiler
// Type: ArrayPositionTween
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class ArrayPositionTween : PlayableAsset, ITimelineClipAsset
{
  [NullAllowed]
  public ExposedReference<TimelineArrayReference> arrayReference;
  public float elementDelay;
  public CustomTweenBehaviour template;

  public ClipCaps clipCaps => ClipCaps.Extrapolation;

  public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
  {
    Transform[] transformArray = this.arrayReference.Resolve(graph.GetResolver())._transformArray;
    if (transformArray == null || transformArray.Length < 1)
      return Playable.Null;
    this.template._transforms = transformArray;
    this.template.elementDelay = this.elementDelay;
    return (Playable) ScriptPlayable<CustomTweenBehaviour>.Create(graph, this.template);
  }
}
