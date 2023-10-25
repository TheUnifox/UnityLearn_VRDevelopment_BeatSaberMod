// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.CopyBeatmapObjectsViewController
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.Views;
using HMUI;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BeatmapEditor3D
{
  public class CopyBeatmapObjectsViewController : BeatmapEditorViewController
  {
    [SerializeField]
    private Button _closeButton;
    [Header("Difficulty Beatmap Sets")]
    [SerializeField]
    private CopyDifficultyBeatmapSetView[] _copyDifficultyViews;
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly BeatmapDataModel _beatmapDataModel;
    private readonly ButtonBinder _buttonBinder = new ButtonBinder();

    public event Action didFinishEvent;

    protected override void DidActivate(
      bool firstActivation,
      bool addedToHierarchy,
      bool screenSystemEnabling)
    {
      foreach (CopyDifficultyBeatmapSetView copyDifficultyView in this._copyDifficultyViews)
      {
        IDifficultyBeatmapSetData difficultyBeatmapSetData;
        copyDifficultyView.SetData(this._beatmapDataModel.difficultyBeatmapSets.TryGetValue(copyDifficultyView.beatmapCharacteristic, out difficultyBeatmapSetData) ? difficultyBeatmapSetData : (IDifficultyBeatmapSetData) null);
        copyDifficultyView.copyBeatmapLevelEvent += new Action<BeatmapCharacteristicSO, BeatmapDifficulty, bool, bool, bool, bool, bool, bool>(this.HandleDifficultyViewCopyBeatmapLevel);
      }
      this._buttonBinder.AddBinding(this._closeButton, new Action(this.Close));
    }

    protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
    {
      foreach (CopyDifficultyBeatmapSetView copyDifficultyView in this._copyDifficultyViews)
        copyDifficultyView.copyBeatmapLevelEvent -= new Action<BeatmapCharacteristicSO, BeatmapDifficulty, bool, bool, bool, bool, bool, bool>(this.HandleDifficultyViewCopyBeatmapLevel);
      this._buttonBinder.ClearBindings();
    }

    private void Close()
    {
      Action didFinishEvent = this.didFinishEvent;
      if (didFinishEvent == null)
        return;
      didFinishEvent();
    }

    private void HandleDifficultyViewCopyBeatmapLevel(
      BeatmapCharacteristicSO beatmapCharacteristic,
      BeatmapDifficulty beatmapDifficulty,
      bool notes,
      bool waypoints,
      bool obstacles,
      bool chains,
      bool arcs,
      bool events)
    {
      this._signalBus.Fire<CopyBeatmapDifficultySignal>(new CopyBeatmapDifficultySignal(beatmapCharacteristic, beatmapDifficulty, notes, obstacles, chains, arcs, events));
      Action didFinishEvent = this.didFinishEvent;
      if (didFinishEvent == null)
        return;
      didFinishEvent();
    }
  }
}
