// Decompiled with JetBrains decompiler
// Type: MultiplayerSpectatorController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections;
using UnityEngine;
using Zenject;

public class MultiplayerSpectatorController : MonoBehaviour
{
  [Inject]
  protected readonly MultiplayerSpectatingSpotManager _spotManager;
  [Inject]
  protected readonly MultiplayerLocalInactivePlayerSongSyncController _songController;
  [Inject]
  protected readonly MultiplayerController _multiplayerController;
  [Inject]
  protected readonly FadeInOutController _fadeInOutController;
  protected Transform _transform;
  protected IMultiplayerSpectatingSpot _currentSpot;

  public event System.Action<IMultiplayerSpectatingSpot> spectatingSpotDidChangeEvent;

  public IMultiplayerSpectatingSpot currentSpot => this._currentSpot;

  public virtual void Awake() => this._transform = this.transform;

  public virtual void Start() => this._multiplayerController.stateChangedEvent += new System.Action<MultiplayerController.State>(this.HandleStateChanged);

  public virtual void OnDestroy()
  {
    if (!((UnityEngine.Object) this._multiplayerController != (UnityEngine.Object) null))
      return;
    this._multiplayerController.stateChangedEvent -= new System.Action<MultiplayerController.State>(this.HandleStateChanged);
  }

  public virtual void SwitchToDefaultSpot() => this.StartCoroutine(this.SwitchToDefaultSpotCoroutine());

  public virtual void SwitchToPrev()
  {
    if (this._currentSpot == null)
      return;
    this.SwitchToSpectatingSpot(this._spotManager.GetAdjacentSpot(this._currentSpot, -1));
  }

  public virtual void SwitchToNext()
  {
    if (this._currentSpot == null)
      return;
    this.SwitchToSpectatingSpot(this._spotManager.GetAdjacentSpot(this._currentSpot, 1));
  }

  public virtual IEnumerator SwitchToDefaultSpotWithFadeCoroutine()
  {
    IMultiplayerSpectatingSpot defaultSpot = this._spotManager.defaultSpot;
    if (this._currentSpot != defaultSpot)
    {
      this._fadeInOutController.FadeOut(0.5f);
      yield return (object) new WaitForSeconds(0.5f);
      this.SwitchToSpectatingSpot(defaultSpot);
      this._fadeInOutController.FadeIn(0.5f);
    }
  }

  public virtual IEnumerator SwitchToDefaultSpotCoroutine()
  {
    yield return (object) null;
    this.SwitchToSpectatingSpot(this._spotManager.defaultSpot);
  }

  public virtual void SwitchToSpectatingSpot(IMultiplayerSpectatingSpot spectatingSpot)
  {
    if (this._currentSpot == spectatingSpot)
      return;
    if (this._currentSpot != null)
      this._currentSpot.SetIsObserved(false);
    this._currentSpot = spectatingSpot;
    this._transform.SetParent(spectatingSpot.transform, false);
    this._songController.FollowOffsetSyncTime(spectatingSpot.observable, true, false);
    spectatingSpot.SetIsObserved(true);
    System.Action<IMultiplayerSpectatingSpot> spotDidChangeEvent = this.spectatingSpotDidChangeEvent;
    if (spotDidChangeEvent == null)
      return;
    spotDidChangeEvent(spectatingSpot);
  }

  public virtual void HandleStateChanged(MultiplayerController.State state)
  {
    if (state != MultiplayerController.State.Outro)
      return;
    this.StartCoroutine(this.SwitchToDefaultSpotWithFadeCoroutine());
  }
}
