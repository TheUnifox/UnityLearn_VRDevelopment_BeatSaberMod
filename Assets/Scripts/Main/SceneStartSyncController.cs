// Decompiled with JetBrains decompiler
// Type: SceneStartSyncController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class SceneStartSyncController : MonoBehaviour
{
  protected const float kLoadOtherTimeout = 15f;
  protected const float kLoadSelfTimeout = 20f;
  [Inject]
  protected readonly IMultiplayerSessionManager _multiplayerSessionManager;
  [Inject]
  protected readonly IGameplayRpcManager _gameplayRpcManager;
  protected SceneStartHandler _sceneStartHandler;
  protected PlayersSpecificSettingsAtGameStartModel _playersAtGameStartModel;
  protected float _waitStartTime;
  protected bool _sceneSyncStarted;

  public event System.Action<string> syncStartDidSuccessEvent;

  public event System.Action<string> syncStartDidReceiveTooLateEvent;

  public event System.Action syncStartDidFailEvent;

  public virtual void Start()
  {
    if (this._sceneSyncStarted)
      return;
    this.enabled = false;
  }

  public virtual void Update()
  {
    if ((double) Time.realtimeSinceStartup - 20.0 <= (double) this._waitStartTime)
      return;
    System.Action startDidFailEvent = this.syncStartDidFailEvent;
    if (startDidFailEvent != null)
      startDidFailEvent();
    this.enabled = false;
  }

  public virtual void OnDestroy()
  {
    if (this._sceneStartHandler == null)
      return;
    this._sceneStartHandler.sceneSetupDidFinishEvent -= new System.Action<string>(this.HandleSceneSetupDidFinish);
    this._sceneStartHandler.sceneSetupDidReceiveTooLateEvent -= new System.Action<string>(this.HandleSceneSetupDidReceiveTooLate);
    this._sceneStartHandler.Dispose();
  }

  public virtual void StartSceneLoadSync(
    PlayersSpecificSettingsAtGameStartModel playersAtGameStartModel)
  {
    this._playersAtGameStartModel = playersAtGameStartModel;
    this._waitStartTime = Time.realtimeSinceStartup;
    this._sceneSyncStarted = true;
    this.enabled = true;
    this._sceneStartHandler = new SceneStartHandler(this._multiplayerSessionManager, this._gameplayRpcManager, this._playersAtGameStartModel);
    this._sceneStartHandler.sceneSetupDidFinishEvent += new System.Action<string>(this.HandleSceneSetupDidFinish);
    this._sceneStartHandler.sceneSetupDidReceiveTooLateEvent += new System.Action<string>(this.HandleSceneSetupDidReceiveTooLate);
    this._sceneStartHandler.GetSceneLoadStatus();
  }

  public virtual void HandleSceneSetupDidFinish(string sessionGameId)
  {
    this.enabled = false;
    System.Action<string> startDidSuccessEvent = this.syncStartDidSuccessEvent;
    if (startDidSuccessEvent == null)
      return;
    startDidSuccessEvent(sessionGameId);
  }

  public virtual void HandleSceneSetupDidReceiveTooLate(string sessionGameId)
  {
    this.enabled = false;
    System.Action<string> receiveTooLateEvent = this.syncStartDidReceiveTooLateEvent;
    if (receiveTooLateEvent == null)
      return;
    receiveTooLateEvent(sessionGameId);
  }
}
