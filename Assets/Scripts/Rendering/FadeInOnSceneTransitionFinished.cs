// Decompiled with JetBrains decompiler
// Type: FadeInOnSceneTransitionFinished
// Assembly: Rendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5B7D31E1-1F9E-4BE6-B735-D2EB47EFDA46
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Rendering.dll

using System.Collections;
using UnityEngine;
using Zenject;

public class FadeInOnSceneTransitionFinished : MonoBehaviour
{
  [Inject]
  private FadeInOutController _fadeInOut;
  [Inject]
  private GameScenesManager _gameScenesManager;

  protected void OnEnable()
  {
    if (this._gameScenesManager.isInTransition)
      this._fadeInOut.FadeOutInstant();
    this.StartCoroutine(this.FadeInAfterSceneTransitionCoroutine());
  }

  private IEnumerator FadeInAfterSceneTransitionCoroutine()
  {
    yield return (object) this._gameScenesManager.waitUntilSceneTransitionFinish;
    yield return (object) null;
    this._fadeInOut.FadeIn();
  }
}
