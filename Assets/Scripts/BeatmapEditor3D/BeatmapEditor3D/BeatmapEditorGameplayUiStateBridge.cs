// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapEditorGameplayUiStateBridge
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Commands;
using BeatmapEditor3D.DataModels;
using System;
using Zenject;

namespace BeatmapEditor3D
{
  public class BeatmapEditorGameplayUiStateBridge : IInitializable, IDisposable
  {
    [Inject]
    private readonly BeatmapEditorSettingsDataModel _beatmapEditorSettingsDataModel;
    [Inject]
    private readonly SignalBus _signalBus;
    [InjectOptional]
    private readonly CoreGameHUDController _hudController;

    public void Initialize()
    {
      this._signalBus.Subscribe<GameplayUIStateChangedSignal>(new Action<GameplayUIStateChangedSignal>(this.HandleGameplayUIStateChanged));
      this.SetUIState(this._beatmapEditorSettingsDataModel.gameplayUIState);
    }

    public void Dispose() => this._signalBus.TryUnsubscribe<GameplayUIStateChangedSignal>(new Action<GameplayUIStateChangedSignal>(this.HandleGameplayUIStateChanged));

    private void SetUIState(BeatmapEditor3D.Types.GameplayUIState currentState)
    {
      if ((UnityEngine.Object) this._hudController == (UnityEngine.Object) null)
        return;
      this._hudController.gameObject.SetActive(currentState != 0);
      this._hudController.energyPanelGo.SetActive(currentState != 0);
      this._hudController.songProgressPanelGO.SetActive(currentState == BeatmapEditor3D.Types.GameplayUIState.Advanced);
      this._hudController.relativeScoreGo.SetActive(currentState == BeatmapEditor3D.Types.GameplayUIState.Advanced);
      this._hudController.immediateRankGo.SetActive(currentState == BeatmapEditor3D.Types.GameplayUIState.Advanced);
    }

    private void HandleGameplayUIStateChanged(GameplayUIStateChangedSignal signal) => this.SetUIState(signal.state);
  }
}
