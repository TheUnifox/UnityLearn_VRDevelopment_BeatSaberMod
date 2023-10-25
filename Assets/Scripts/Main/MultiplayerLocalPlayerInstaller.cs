// Decompiled with JetBrains decompiler
// Type: MultiplayerLocalPlayerInstaller
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Zenject;

public class MultiplayerLocalPlayerInstaller : MonoInstaller
{
  [Inject]
  protected readonly MultiplayerLevelSceneSetupData _levelSceneSetupData;
  [Inject]
  protected readonly IMultiplayerSessionManager _multiplayerSessionManager;
  [Inject]
  protected readonly MultiplayerPlayerStartState _startState;

  public override void InstallBindings()
  {
    this.Container.Bind<LocalPlayerInGameMenuInitData>().FromInstance(new LocalPlayerInGameMenuInitData(this._levelSceneSetupData.previewBeatmapLevel, this._levelSceneSetupData.beatmapDifficulty, this._levelSceneSetupData.beatmapCharacteristic, this._levelSceneSetupData.hasSong)).AsSingle();
    this.Container.Bind<IConnectedPlayer>().FromInstance(this._multiplayerSessionManager.localPlayer);
    this.Container.Bind<MultiplayerPlayerStartState>().FromInstance(this._startState).AsSingle();
    this.Container.Bind<MultiplayerLocalPlayerDisconnectHelper>().AsSingle();
  }
}
