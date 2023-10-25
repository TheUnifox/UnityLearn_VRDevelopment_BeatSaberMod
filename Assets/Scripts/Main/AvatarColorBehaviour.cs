// Decompiled with JetBrains decompiler
// Type: AvatarColorBehaviour
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class AvatarColorBehaviour : PlayableBehaviour
{
  public Color _startColor = Color.white;
  public Color _endColor = Color.black;
  public EaseType _easeType = EaseType.Linear;
  protected AvatarVisualController _avatarVisualController;
  protected float _duration;

  public override void OnGraphStart(Playable playable) => this._duration = (float) playable.GetDuration<Playable>() * 0.98f;

  public override void ProcessFrame(Playable playable, FrameData info, object playerData)
  {
    AvatarColorBehaviour behaviour = ((ScriptPlayable<AvatarColorBehaviour>) playable).GetBehaviour();
    this._avatarVisualController = (AvatarVisualController) playerData;
    float num = (float) playable.GetTime<Playable>() / this._duration;
    this._avatarVisualController.SetLightColor(Color.Lerp(behaviour._startColor, behaviour._endColor, Interpolation.Interpolate(Mathf.Clamp01(num), behaviour._easeType)));
  }

  public override void OnPlayableDestroy(Playable playable)
  {
    if (!((UnityEngine.Object) this._avatarVisualController != (UnityEngine.Object) null))
      return;
    this._avatarVisualController.SetLightColor(this._startColor);
  }
}
