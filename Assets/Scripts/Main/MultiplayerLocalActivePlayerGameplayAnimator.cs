// Decompiled with JetBrains decompiler
// Type: MultiplayerLocalActivePlayerGameplayAnimator
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Runtime.CompilerServices;
using Tweening;
using UnityEngine;
using Zenject;

public class MultiplayerLocalActivePlayerGameplayAnimator : MultiplayerGameplayAnimator
{
  [Space]
  [SerializeField]
  protected CoreGameHUDController _coreGameHUDController;
  [SerializeField]
  protected MultiplayerPositionHUDController _multiplayerPositionHUDController;
  [Inject]
  protected readonly IMultiplayerLevelEndActionsPublisher _levelEndActionsPublisher;
  protected FloatTween _fadeOutHUDTween;
  protected bool _initialized;

  protected override void OnDestroy()
  {
    base.OnDestroy();
    if (this._levelEndActionsPublisher == null)
      return;
    this._levelEndActionsPublisher.playerDidFinishEvent -= new System.Action<MultiplayerLevelCompletionResults>(this.HandlePlayerDidFinish);
  }

  public virtual void InitializeIfNeeded()
  {
    if (this._initialized)
      return;
    this._initialized = true;
    this._fadeOutHUDTween = new FloatTween(1f, 0.0f, (System.Action<float>) (f =>
    {
      this._coreGameHUDController.alpha = f;
      this._multiplayerPositionHUDController.alpha = f;
    }), 1f, EaseType.InQuad);
  }

  protected override void HandleStateChanged(MultiplayerController.State state)
  {
    base.HandleStateChanged(state);
    if (state == MultiplayerController.State.Gameplay)
    {
      if (!this.connectedPlayer.IsFailed())
        this._levelEndActionsPublisher.playerDidFinishEvent += new System.Action<MultiplayerLevelCompletionResults>(this.HandlePlayerDidFinish);
      else
        this.TransitionIntoFailedState();
    }
    else
      this._levelEndActionsPublisher.playerDidFinishEvent -= new System.Action<MultiplayerLevelCompletionResults>(this.HandlePlayerDidFinish);
  }

  protected override void AnimateNewLeaderSelected(bool isLeading)
  {
    this.InitializeIfNeeded();
    if (isLeading)
    {
      foreach (LightsAnimator gameplayLightsAnimator in this._gameplayLightsAnimators)
        gameplayLightsAnimator.AnimateToColor((Color) this._leadingLightsColor, this._leadingSwitchCrossFadeDuration, EaseType.OutQuad);
    }
    else
    {
      foreach (LightsAnimator gameplayLightsAnimator in this._gameplayLightsAnimators)
        gameplayLightsAnimator.AnimateToColor((Color) this._activeLightsColor, this._leadingSwitchCrossFadeDuration, EaseType.OutQuad);
    }
  }

  public virtual void TransitionIntoFailedState()
  {
    this.InitializeIfNeeded();
    foreach (LightsAnimator allLightsAnimator in this._allLightsAnimators)
      allLightsAnimator.AnimateToColor((Color) this._failedLightsColor * new Color(1f, 1f, 1f, 0.0f), 2.3f, EaseType.InQuad);
    this.tweeningManager.RestartTween((Tween) this._fadeOutHUDTween, (object) this);
  }

  public virtual void HandlePlayerDidFinish(
    MultiplayerLevelCompletionResults levelCompletionResults)
  {
    if (!levelCompletionResults.failedOrGivenUp)
      return;
    this.TransitionIntoFailedState();
  }

  [CompilerGenerated]
  public virtual void m_CInitializeIfNeededm_Eb__6_0(float f)
  {
    this._coreGameHUDController.alpha = f;
    this._multiplayerPositionHUDController.alpha = f;
  }
}
