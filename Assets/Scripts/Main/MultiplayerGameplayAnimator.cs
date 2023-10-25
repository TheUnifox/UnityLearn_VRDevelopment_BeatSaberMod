// Decompiled with JetBrains decompiler
// Type: MultiplayerGameplayAnimator
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Tweening;
using UnityEngine;
using Zenject;

public abstract class MultiplayerGameplayAnimator : MonoBehaviour
{
  [SerializeField]
  protected ColorSO _activeLightsColor;
  [SerializeField]
  protected ColorSO _leadingLightsColor;
  [SerializeField]
  protected ColorSO _failedLightsColor;
  [Space]
  [SerializeField]
  protected float _leadingSwitchCrossFadeDuration = 0.3f;
  [Space]
  [SerializeField]
  protected LightsAnimator[] _gameplayLightsAnimators;
  [SerializeField]
  protected LightsAnimator[] _allLightsAnimators;
  [Inject]
  private readonly MultiplayerLeadPlayerProvider _leadPlayerProvider;
  [Inject]
  private readonly MultiplayerController _multiplayerController;
  [Inject]
  protected readonly TimeTweeningManager tweeningManager;
  [Inject]
  protected readonly IConnectedPlayer connectedPlayer;

  protected virtual void Start()
  {
    this._multiplayerController.stateChangedEvent += new System.Action<MultiplayerController.State>(this.HandleStateChanged);
    this.HandleStateChanged(this._multiplayerController.state);
  }

  protected virtual void OnDestroy()
  {
    if ((UnityEngine.Object) this._multiplayerController != (UnityEngine.Object) null)
      this._multiplayerController.stateChangedEvent -= new System.Action<MultiplayerController.State>(this.HandleStateChanged);
    if ((UnityEngine.Object) this._leadPlayerProvider != (UnityEngine.Object) null)
      this._leadPlayerProvider.newLeaderWasSelectedEvent -= new System.Action<string>(this.HandleNewLeaderWasSelected);
    if (!((UnityEngine.Object) this.tweeningManager != (UnityEngine.Object) null))
      return;
    this.tweeningManager.KillAllTweens((object) this);
  }

  protected abstract void AnimateNewLeaderSelected(bool isLeading);

  protected virtual void HandleStateChanged(MultiplayerController.State state)
  {
    if (state == MultiplayerController.State.Gameplay)
    {
      if (this.connectedPlayer.IsFailed())
        return;
      foreach (LightsAnimator allLightsAnimator in this._allLightsAnimators)
        allLightsAnimator.SetColor((Color) this._activeLightsColor);
      this._leadPlayerProvider.newLeaderWasSelectedEvent += new System.Action<string>(this.HandleNewLeaderWasSelected);
    }
    else
      this._leadPlayerProvider.newLeaderWasSelectedEvent -= new System.Action<string>(this.HandleNewLeaderWasSelected);
  }

  private void HandleNewLeaderWasSelected(string userId)
  {
    if (this.connectedPlayer.IsFailed())
      return;
    this.AnimateNewLeaderSelected(userId == this.connectedPlayer.userId);
  }
}
