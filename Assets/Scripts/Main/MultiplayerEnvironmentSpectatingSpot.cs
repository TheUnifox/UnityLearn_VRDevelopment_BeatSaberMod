// Decompiled with JetBrains decompiler
// Type: MultiplayerEnvironmentSpectatingSpot
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Polyglot;
using System.Runtime.CompilerServices;
using UnityEngine;
using Zenject;

public class MultiplayerEnvironmentSpectatingSpot : MonoBehaviour, IMultiplayerSpectatingSpot
{
  [SerializeField]
  protected bool _preferredSpectatingSpot;
  [SerializeField]
  protected bool _displaySpotNumber;
  [DrawIf("_displaySpotNumber", true, DrawIfAttribute.DisablingType.DontDraw)]
  [SerializeField]
  protected int _spotNumber;
  [Inject]
  protected readonly MultiplayerSpectatingSpotManager _spectatingSpotManager;
  [Inject]
  protected readonly MultiplayerActivePlayersTimeOffsetAverage _activePlayersTimeOffsetAverage;

  public event System.Action<IMultiplayerSpectatingSpot> hasBeenRemovedEvent;

  public IMultiplayerObservable observable => (IMultiplayerObservable) this._activePlayersTimeOffsetAverage;

  public string spotName => !this._displaySpotNumber ? Localization.Get("LABEL_GRANDSTAND") : string.Format("{0} {1}", (object) Localization.Get("LABEL_GRANDSTAND"), (object) this._spotNumber);

  public bool isMain => this._preferredSpectatingSpot;

  public virtual void Start() => this._spectatingSpotManager.RegisterSpectatingSpot((IMultiplayerSpectatingSpot) this);

  public virtual void OnDisable()
  {
    System.Action<IMultiplayerSpectatingSpot> beenRemovedEvent = this.hasBeenRemovedEvent;
    if (beenRemovedEvent == null)
      return;
    beenRemovedEvent((IMultiplayerSpectatingSpot) this);
  }

  public virtual void SetIsObserved(bool isObserved)
  {
  }

}
