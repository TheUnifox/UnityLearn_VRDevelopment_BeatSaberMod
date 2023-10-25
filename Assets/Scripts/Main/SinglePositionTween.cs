// Decompiled with JetBrains decompiler
// Type: SinglePositionTween
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class SinglePositionTween : PlayableAsset, ITimelineClipAsset
{
  [NullAllowed]
  public ExposedReference<Transform> transformReference;
  public CustomTweenBehaviour template;

  public ClipCaps clipCaps => ClipCaps.Extrapolation;

  public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
  {
    Transform transform = this.transformReference.Resolve(graph.GetResolver());
    if ((UnityEngine.Object) transform == (UnityEngine.Object) null)
      return Playable.Null;
    this.template._transforms = new Transform[1]
    {
      transform
    };
    return (Playable) ScriptPlayable<CustomTweenBehaviour>.Create(graph, this.template);
  }
}
