// Decompiled with JetBrains decompiler
// Type: BloomFogParamsBehaviour
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class BloomFogParamsBehaviour : PlayableBehaviour
{
  [SerializeField]
  protected BloomFogEnvironmentParams _bloomFogParams;
  [SerializeField]
  protected float _blend;
  [SerializeField]
  protected BloomFogSO _bloomFog;
  protected bool _initialized;

  public override void ProcessFrame(Playable playable, FrameData info, object playerData)
  {
    if ((UnityEngine.Object) this._bloomFog == (UnityEngine.Object) null)
      return;
    if (!this._initialized)
      this._initialized = true;
    BloomFogParamsBehaviour behaviour = ((ScriptPlayable<BloomFogParamsBehaviour>) playable).GetBehaviour();
    if ((double) behaviour._blend >= 1.0)
    {
      this._bloomFog.transition = 1f;
    }
    else
    {
      this._bloomFog.transition = behaviour._blend;
      this._bloomFog.transitionFogParams = behaviour._bloomFogParams;
    }
  }

  public override void OnPlayableDestroy(Playable playable)
  {
    if ((UnityEngine.Object) this._bloomFog == (UnityEngine.Object) null || !this._initialized)
      return;
    this._bloomFog.transition = 0.0f;
  }
}
