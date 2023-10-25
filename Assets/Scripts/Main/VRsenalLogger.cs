// Decompiled with JetBrains decompiler
// Type: VRsenalLogger
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class VRsenalLogger : MonoBehaviour
{
  [SerializeField]
  protected ScenesTransitionSetupDataSO _standardLevelScenesTransitionSetupData;
  [SerializeField]
  protected ScenesTransitionSetupDataSO _tutorialScenesTransitionSetupData;
  [SerializeField]
  protected StringSignal _playerNameWasEnteredSignal;
  [SerializeField]
  protected VRsenalScoreLogger _vRsenalScoreLoggerPrefab;
  [Inject]
  protected GameScenesManager _gameScenesManager;

  public virtual void Awake()
  {
    this._gameScenesManager.installEarlyBindingsEvent += new System.Action<ScenesTransitionSetupDataSO, DiContainer>(this.HandleGameScenesManagerInstallEarlyBindings);
    this._playerNameWasEnteredSignal.Subscribe(new System.Action<string>(this.HandlePlayerNameWasEntered));
  }

  public virtual void OnDestroy()
  {
    this._gameScenesManager.installEarlyBindingsEvent -= new System.Action<ScenesTransitionSetupDataSO, DiContainer>(this.HandleGameScenesManagerInstallEarlyBindings);
    this._playerNameWasEnteredSignal.Unsubscribe(new System.Action<string>(this.HandlePlayerNameWasEntered));
  }

  public virtual void HandleGameScenesManagerInstallEarlyBindings(
    ScenesTransitionSetupDataSO scenesTransitionSetupData,
    DiContainer container)
  {
    if ((UnityEngine.Object) scenesTransitionSetupData == (UnityEngine.Object) this._standardLevelScenesTransitionSetupData)
    {
      container.Bind<VRsenalScoreLogger>().FromComponentInNewPrefab((UnityEngine.Object) this._vRsenalScoreLoggerPrefab).AsSingle().NonLazy();
    }
    else
    {
      if (!((UnityEngine.Object) scenesTransitionSetupData == (UnityEngine.Object) this._tutorialScenesTransitionSetupData))
        return;
      Debug.Log((object) "VRsenalLogger: Clicked Tutorial.");
    }
  }

  public virtual void HandlePlayerNameWasEntered(string playerName) => Debug.Log((object) ("VRsenalLogger: Player entered their name for the leaderboard. Name=" + playerName));
}
