// Decompiled with JetBrains decompiler
// Type: BloomFogParamsAnimator
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using System.Collections;
using UnityEngine;
using Zenject;

public class BloomFogParamsAnimator : MonoBehaviour
{
  [Inject]
  protected readonly BloomFogSO _bloomFog;

  public virtual void AnimateBloomFogParamsChange(
    BloomFogEnvironmentParams envFogParams,
    float duration)
  {
    this.StopAllCoroutines();
    this._bloomFog.transition = 0.0f;
    this._bloomFog.transitionFogParams = envFogParams;
    this.StartCoroutine(this.AnimationCoroutine(envFogParams, duration));
  }

  public virtual IEnumerator AnimationCoroutine(
    BloomFogEnvironmentParams envFogParams,
    float duration)
  {
    float elapsedTime = 0.0f;
    while ((double) elapsedTime < (double) duration)
    {
      this._bloomFog.transition = elapsedTime / duration;
      elapsedTime += Time.deltaTime;
      yield return (object) null;
    }
    this._bloomFog.transition = 0.0f;
    this._bloomFog.defaultForParams = envFogParams;
  }

  public virtual void SetBloomFogParamsChange(
    BloomFogEnvironmentParams envFogParams,
    float transition)
  {
    this._bloomFog.transition = transition;
    this._bloomFog.transitionFogParams = envFogParams;
  }

  public virtual BloomFogEnvironmentParams GetDefaultBloomFogParams() => this._bloomFog.defaultForParams;

  public virtual void SetDefaultBloomFogParams(BloomFogEnvironmentParams newDefaultBloomFogParams) => this._bloomFog.defaultForParams = newDefaultBloomFogParams;
}
