// Decompiled with JetBrains decompiler
// Type: FlyingScoreEffect
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using TMPro;
using UnityEngine;
using Zenject;

public class FlyingScoreEffect : 
  FlyingObjectEffect,
  ICutScoreBufferDidChangeReceiver,
  ICutScoreBufferDidFinishReceiver
{
  [SerializeField]
  protected AnimationCurve _fadeAnimationCurve = AnimationCurve.Linear(0.0f, 1f, 1f, 0.0f);
  [SerializeField]
  protected SpriteRenderer _maxCutDistanceScoreIndicator;
  [SerializeField]
  protected TextMeshPro _text;
  protected Color _color;
  protected float _colorAMultiplier;
  protected bool _registeredToCallbacks;
  protected IReadonlyCutScoreBuffer _cutScoreBuffer;

  public virtual void InitAndPresent(
    IReadonlyCutScoreBuffer cutScoreBuffer,
    float duration,
    Vector3 targetPos,
    Color color)
  {
    this._color = color;
    this._cutScoreBuffer = cutScoreBuffer;
    if (!cutScoreBuffer.isFinished)
    {
      cutScoreBuffer.RegisterDidChangeReceiver((ICutScoreBufferDidChangeReceiver) this);
      cutScoreBuffer.RegisterDidFinishReceiver((ICutScoreBufferDidFinishReceiver) this);
      this._registeredToCallbacks = true;
    }
    this._maxCutDistanceScoreIndicator.enabled = cutScoreBuffer.centerDistanceCutScore == cutScoreBuffer.noteScoreDefinition.maxCenterDistanceCutScore;
    this._colorAMultiplier = (double) cutScoreBuffer.cutScore > (double) cutScoreBuffer.maxPossibleCutScore * 0.89999997615814209 ? 1f : 0.3f;
    this.RefreshScore(cutScoreBuffer.cutScore, cutScoreBuffer.maxPossibleCutScore);
    this.InitAndPresent(duration, targetPos, cutScoreBuffer.noteCutInfo.worldRotation, false);
  }

  protected override void ManualUpdate(float t)
  {
    Color color = this._color.ColorWithAlpha(this._fadeAnimationCurve.Evaluate(t) * this._colorAMultiplier);
    this._text.color = color;
    this._maxCutDistanceScoreIndicator.color = color;
  }

  public virtual void HandleCutScoreBufferDidChange(CutScoreBuffer cutScoreBuffer) => this.RefreshScore(cutScoreBuffer.cutScore, cutScoreBuffer.maxPossibleCutScore);

  public virtual void RefreshScore(int score, int maxPossibleCutScore)
  {
    this._text.text = score.ToString();
    this._colorAMultiplier = (double) score > (double) maxPossibleCutScore * 0.89999997615814209 ? 1f : 0.3f;
  }

  public virtual void HandleCutScoreBufferDidFinish(CutScoreBuffer cutScoreBuffer) => this.UnregisterCallbacksIfNeeded();

  public virtual void UnregisterCallbacksIfNeeded()
  {
    if (!this._registeredToCallbacks || this._cutScoreBuffer == null)
      return;
    this._registeredToCallbacks = false;
    this._cutScoreBuffer.UnregisterDidChangeReceiver((ICutScoreBufferDidChangeReceiver) this);
    this._cutScoreBuffer.UnregisterDidFinishReceiver((ICutScoreBufferDidFinishReceiver) this);
  }

  public class Pool : MonoMemoryPool<FlyingScoreEffect>
  {
    protected override void OnDespawned(FlyingScoreEffect item)
    {
      item.UnregisterCallbacksIfNeeded();
      base.OnDespawned(item);
    }
  }
}
