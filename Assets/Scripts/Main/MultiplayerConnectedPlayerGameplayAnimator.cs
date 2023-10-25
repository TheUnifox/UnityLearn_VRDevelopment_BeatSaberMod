// Decompiled with JetBrains decompiler
// Type: MultiplayerConnectedPlayerGameplayAnimator
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class MultiplayerConnectedPlayerGameplayAnimator : MultiplayerGameplayAnimator
{
  [Space]
  [SerializeField]
  protected MultiplayerBigAvatarAnimator _bigAvatarAnimator;
  [SerializeField]
  protected ScaleAnimator _avatarScaleAnimator;
  [Space]
  [SerializeField]
  protected float _defaultLightsWidth;
  [SerializeField]
  protected float _observedLightsWidth;
  [Inject]
  protected readonly MultiplayerConnectedPlayerLevelFailController _failController;
  [Inject]
  protected readonly MultiplayerLayoutProvider _multiplayerLayoutProvider;
  [Inject]
  protected readonly MultiplayerConnectedPlayerSpectatingSpot _playerSpectatingSpot;

  protected override void Start()
  {
    base.Start();
    this._bigAvatarAnimator.HideInstant();
    this._playerSpectatingSpot.isObservedChangedEvent += new System.Action<bool>(this.HandleIsObservedChanged);
  }

  protected override void OnDestroy()
  {
    base.OnDestroy();
    if ((UnityEngine.Object) this._failController != (UnityEngine.Object) null)
      this._failController.playerDidFailEvent -= new System.Action(this.HandlePlayerDidFail);
    if (!((UnityEngine.Object) this._playerSpectatingSpot != (UnityEngine.Object) null))
      return;
    this._playerSpectatingSpot.isObservedChangedEvent -= new System.Action<bool>(this.HandleIsObservedChanged);
  }

  public virtual void TransitionIntoFailedState()
  {
    foreach (LightsAnimator allLightsAnimator in this._allLightsAnimators)
    {
      allLightsAnimator.AnimateToColor((Color) this._failedLightsColor, 0.0f, EaseType.InQuad);
      allLightsAnimator.AnimateToColor((Color) this._failedLightsColor * new Color(1f, 1f, 1f, 0.0f), 2.3f, EaseType.OutSine);
    }
    this._bigAvatarAnimator.Animate(false, 0.15f, EaseType.OutQuad);
    this._avatarScaleAnimator.Animate(false, 0.0f, EaseType.OutQuad, 0.15f);
  }

  protected override void AnimateNewLeaderSelected(bool isLeading)
  {
    if (isLeading)
    {
      foreach (LightsAnimator gameplayLightsAnimator in this._gameplayLightsAnimators)
        gameplayLightsAnimator.AnimateToColor((Color) this._leadingLightsColor, this._leadingSwitchCrossFadeDuration, EaseType.OutQuad);
      if (this._multiplayerLayoutProvider.layout == MultiplayerPlayerLayout.Duel)
        return;
      this._bigAvatarAnimator.Animate(true, 0.3f, EaseType.OutBack);
    }
    else
    {
      foreach (LightsAnimator gameplayLightsAnimator in this._gameplayLightsAnimators)
        gameplayLightsAnimator.AnimateToColor((Color) this._activeLightsColor, this._leadingSwitchCrossFadeDuration, EaseType.OutQuad);
      if (this._multiplayerLayoutProvider.layout == MultiplayerPlayerLayout.Duel)
        return;
      this._bigAvatarAnimator.Animate(false, 0.15f, EaseType.OutQuad);
    }
  }

  protected override void HandleStateChanged(MultiplayerController.State state)
  {
    base.HandleStateChanged(state);
    if (state == MultiplayerController.State.Gameplay)
    {
      if (!this.connectedPlayer.IsFailed())
        this._failController.playerDidFailEvent += new System.Action(this.HandlePlayerDidFail);
      else
        this.TransitionIntoFailedState();
    }
    else
      this._failController.playerDidFailEvent -= new System.Action(this.HandlePlayerDidFail);
  }

  public virtual void HandlePlayerDidFail() => this.TransitionIntoFailedState();

  public virtual void HandleIsObservedChanged(bool isObserved)
  {
    foreach (LightsAnimator gameplayLightsAnimator in this._gameplayLightsAnimators)
      gameplayLightsAnimator.SetLightsWidth(isObserved ? this._observedLightsWidth : this._defaultLightsWidth);
  }
}
