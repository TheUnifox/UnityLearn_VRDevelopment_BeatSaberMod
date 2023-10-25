// Decompiled with JetBrains decompiler
// Type: PS4OnGoingToBackgroundSaveHandler
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class PS4OnGoingToBackgroundSaveHandler : MonoBehaviour
{
  [SerializeField]
  protected LocalLeaderboardsModel _localLeaderboardModel;
  [SerializeField]
  protected MainSettingsModelSO _mainSettingsModel;
  [Inject]
  protected PlayerDataModel _playerDataModel;

  public virtual void OnEnable()
  {
    UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) this);
    PersistentSingleton<PS4Helper>.instance.didGoToBackgroundExecutionEvent += new System.Action(this.HandlePS4HelperDidGoToBackgroundExecution);
  }

  public virtual void OnDisable()
  {
    if (!PersistentSingleton<PS4Helper>.IsSingletonAvailable)
      return;
    PersistentSingleton<PS4Helper>.instance.didGoToBackgroundExecutionEvent -= new System.Action(this.HandlePS4HelperDidGoToBackgroundExecution);
  }

  public virtual void HandlePS4HelperDidGoToBackgroundExecution()
  {
    this._playerDataModel.Save();
    this._localLeaderboardModel.Save();
    this._mainSettingsModel.Save();
  }
}
