// Decompiled with JetBrains decompiler
// Type: MultiplayerDuelConnectedPlayerGameplayAnimator
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class MultiplayerDuelConnectedPlayerGameplayAnimator : MultiplayerGameplayAnimator
{
  [Space]
  [SerializeField]
  protected ScaleAnimator _avatarScaleAnimator;
  [Inject]
  protected readonly MultiplayerConnectedPlayerLevelFailController _failController;

  protected override void OnDestroy()
  {
    base.OnDestroy();
    if (!((UnityEngine.Object) this._failController != (UnityEngine.Object) null))
      return;
    this._failController.playerDidFailEvent -= new System.Action(this.HandlePlayerDidFail);
  }

  public virtual void TransitionIntoFailedState()
  {
    foreach (LightsAnimator allLightsAnimator in this._allLightsAnimators)
    {
      allLightsAnimator.AnimateToColor((Color) this._failedLightsColor, 0.0f, EaseType.InQuad);
      allLightsAnimator.AnimateToColor((Color) this._failedLightsColor * new Color(1f, 1f, 1f, 0.0f), 2.3f, EaseType.OutSine);
    }
    this._avatarScaleAnimator.Animate(false, 0.15f, EaseType.OutQuad);
  }

  protected override void AnimateNewLeaderSelected(bool isLeading)
  {
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
}
