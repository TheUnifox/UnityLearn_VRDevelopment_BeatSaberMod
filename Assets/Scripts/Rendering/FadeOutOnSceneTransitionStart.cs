// Decompiled with JetBrains decompiler
// Type: FadeOutOnSceneTransitionStart
// Assembly: Rendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5B7D31E1-1F9E-4BE6-B735-D2EB47EFDA46
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Rendering.dll

using System;
using UnityEngine;
using Zenject;

public class FadeOutOnSceneTransitionStart : MonoBehaviour
{
  [Inject]
  private readonly FadeInOutController _fadeInOut;
  [Inject]
  private readonly GameScenesManager _gameScenesManager;

  protected void Start() => this._gameScenesManager.transitionDidStartEvent += new Action<float>(this.HandleGameScenesManagerTransitionDidStart);

  protected void OnDestroy()
  {
    if (!((UnityEngine.Object) this._gameScenesManager != (UnityEngine.Object) null))
      return;
    this._gameScenesManager.transitionDidStartEvent -= new Action<float>(this.HandleGameScenesManagerTransitionDidStart);
  }

  private void HandleGameScenesManagerTransitionDidStart(float duration) => this._fadeInOut.FadeOut(duration);
}
