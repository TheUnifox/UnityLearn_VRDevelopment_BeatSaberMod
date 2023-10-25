// Decompiled with JetBrains decompiler
// Type: GhostDuplicationEffect
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections;
using Tweening;
using UnityEngine;
using Zenject;

public class GhostDuplicationEffect : MonoBehaviour
{
  [SerializeField]
  protected CanvasGroup[] _canvases;
  protected bool isInitialized;
  [Inject]
  protected readonly TimeTweeningManager _tweeningManager;

  protected virtual void OnDestroy()
  {
    if (!((UnityEngine.Object) this._tweeningManager != (UnityEngine.Object) null))
      return;
    this._tweeningManager.KillAllTweens((object) this);
  }

  public virtual void Awake()
  {
    if (this.isInitialized)
      return;
    this.Init();
  }

  public virtual void Init()
  {
    foreach (Component canvase in this._canvases)
    {
      canvase.gameObject.SetActive(true);
      this.alpha = 0.0f;
    }
    this.isInitialized = true;
  }

  public virtual void Animate(
    GhostDuplicationEffect.GhostEffectParams ghostEffectParams)
  {
    Vector3 direction = ghostEffectParams.startPosition - ghostEffectParams.endPosition;
    this._tweeningManager.AddTween((Tween) new FloatTween(ghostEffectParams.startSize, ghostEffectParams.endSize, (System.Action<float>) (val => this.size = val), ghostEffectParams.duration, ghostEffectParams.easeType, ghostEffectParams.delay), (object) this);
    this._tweeningManager.AddTween((Tween) new FloatTween(ghostEffectParams.startAlpha, ghostEffectParams.endAlpha, (System.Action<float>) (val => this.alpha = Mathf.Clamp01(val)), ghostEffectParams.duration, ghostEffectParams.easeType, ghostEffectParams.delay), (object) this);
    this._tweeningManager.AddTween((Tween) new Vector3Tween(ghostEffectParams.startPosition, ghostEffectParams.endPosition, (System.Action<Vector3>) (pos => this.transform.localPosition = pos), ghostEffectParams.duration, ghostEffectParams.easeType, ghostEffectParams.delay), (object) this);
    this._tweeningManager.AddTween((Tween) new FloatTween(0.0f, ghostEffectParams.peakDistance, (System.Action<float>) (val => this.SetDistances(ghostEffectParams.distanceCurve.Evaluate(val), direction)), ghostEffectParams.duration, EaseType.Linear, ghostEffectParams.delay), (object) this);
    if (!ghostEffectParams.lastPhase)
      return;
    this.StartCoroutine(this.HideRedundantWithDelay(ghostEffectParams.duration + ghostEffectParams.delay));
  }

  public virtual IEnumerator HideRedundantWithDelay(float delay)
  {
    yield return (object) new WaitForSeconds(delay);
    for (int index = 0; index < this._canvases.Length; ++index)
      this._canvases[index].gameObject.SetActive(index == 0);
  }

  public virtual void SetDistances(float distance, Vector3 direction)
  {
    for (int index = 1; index < this._canvases.Length; ++index)
      this._canvases[index].transform.localPosition = (float) index * distance * this._canvases[index].transform.InverseTransformDirection(direction);
  }

  public bool hide
  {
    set => this.gameObject.SetActive(!value);
  }

  private float size
  {
    set
    {
      foreach (Component canvase in this._canvases)
        canvase.transform.localScale = new Vector3(value, value, value);
    }
    get => this._canvases != null && this._canvases.Length != 0 ? this._canvases[0].transform.localScale.x : 0.0f;
  }

  private float alpha
  {
    set
    {
      for (int p = 0; p < this._canvases.Length; ++p)
        this._canvases[p].alpha = value / Mathf.Pow(2f, (float) p);
    }
    get => this._canvases != null && this._canvases.Length != 0 ? this._canvases[0].alpha : 0.0f;
  }

  [Serializable]
  public struct GhostEffectParams
  {
    public float startAlpha;
    [HideInInspector]
    public Vector3 startPosition;
    public float startSize;
    public float endAlpha;
    [HideInInspector]
    public Vector3 endPosition;
    public float endSize;
    public float duration;
    public float delay;
    public EaseType easeType;
    public AnimationCurve distanceCurve;
    public float peakDistance;
    public bool lastPhase;
  }
}
