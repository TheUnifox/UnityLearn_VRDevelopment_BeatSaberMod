// Decompiled with JetBrains decompiler
// Type: MultiplayerCoreInstaller
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;
using Zenject;

public class MultiplayerCoreInstaller : MonoInstaller
{
  [Space]
  [SerializeField]
  protected ScoreSyncStateManager _scoreSyncStateManagerPrefab;
  [SerializeField]
  protected MultiplayerBadgesModelSO _multiplayerBadgesModel;
  [Inject]
  protected readonly GameplayCoreSceneSetupData _sceneSetupData;
  [Inject]
  protected readonly IMultiplayerSessionManager _multiplayerSessionManager;

  public override void InstallBindings()
  {
    PlayerSpecificSettings specificSettings = this._sceneSetupData.playerSpecificSettings;
    this.Container.Bind<MultiplayerLayoutProvider>().AsSingle();
    if (this._sceneSetupData.difficultyBeatmap == null)
    {
      this.Container.Bind<MultiplayerSongEntitlementsStatus>().FromInstance(MultiplayerSongEntitlementsStatus.Invalid).AsSingle();
      if (this._multiplayerSessionManager.localPlayer.WasActiveAtLevelStart())
      {
        this._multiplayerSessionManager.SetLocalPlayerState("is_active", false);
        this._multiplayerSessionManager.SetLocalPlayerState("was_active_at_level_start", false);
      }
    }
    else
      this.Container.Bind<MultiplayerSongEntitlementsStatus>().FromInstance(MultiplayerSongEntitlementsStatus.Ok).AsSingle();
    this.Container.Bind(typeof (IGameplayRpcManager), typeof (IDisposable)).To<GameplayRpcManager>().AsSingle();
    this.Container.Bind<IScoreSyncStateManager>().FromComponentInNewPrefab((UnityEngine.Object) this._scoreSyncStateManagerPrefab).AsSingle();
    GameplayModifiers gameplayModifiers = this._sceneSetupData.gameplayModifiers;
    this.Container.Bind<IDifficultyBeatmap>().FromInstance(this._sceneSetupData.difficultyBeatmap).AsSingle();
    this.Container.Bind<MultiplayerBadgesModelSO>().FromInstance(this._multiplayerBadgesModel).AsSingle();
    this.Container.Bind<MultiplayerBadgesProvider>().AsSingle();
    this.Container.Bind<CoreGameHUDController.InitData>().FromInstance(new CoreGameHUDController.InitData(specificSettings.noTextsAndHuds || gameplayModifiers.zenMode, true, specificSettings.advancedHud)).AsSingle();
    this.Container.Bind<MultiplayerActivePlayersTimeOffsetAverage>().AsSingle();
    this.Container.Bind<MultiplayerSpectatingSpotManager>().ToSelf().AsSingle();
  }
}
