// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.CopyDifficultyBeatmapSetView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Views
{
  public class CopyDifficultyBeatmapSetView : BeatmapEditorView
  {
    [SerializeField]
    private BeatmapCharacteristicSO _beatmapCharacteristic;
    [SerializeField]
    private CopyDifficultyBeatmapSetView.CopyDifficultyBeatmapViewPair[] _copyDifficultyBeatmapViewPairs;
    [Inject]
    private readonly BeatmapLevelDataModel _beatmapLevelDataModel;

    public event Action<BeatmapCharacteristicSO, BeatmapDifficulty, bool, bool, bool, bool, bool, bool> copyBeatmapLevelEvent;

    public BeatmapCharacteristicSO beatmapCharacteristic => this._beatmapCharacteristic;

    public void SetData(IDifficultyBeatmapSetData difficultyBeatmapSetData)
    {
      bool flag1 = (UnityEngine.Object) this._beatmapLevelDataModel.beatmapCharacteristic == (UnityEngine.Object) this._beatmapCharacteristic;
      foreach (CopyDifficultyBeatmapSetView.CopyDifficultyBeatmapViewPair difficultyBeatmapViewPair in this._copyDifficultyBeatmapViewPairs)
      {
        bool flag2 = difficultyBeatmapSetData != null && difficultyBeatmapSetData.difficultyBeatmaps.ContainsKey(difficultyBeatmapViewPair.difficulty) && difficultyBeatmapSetData.difficultyBeatmaps[difficultyBeatmapViewPair.difficulty] != null;
        bool flag3 = difficultyBeatmapViewPair.difficulty == this._beatmapLevelDataModel.beatmapDifficulty;
        difficultyBeatmapViewPair.view.gameObject.SetActive(flag2 && (!flag3 || !flag1));
        difficultyBeatmapViewPair.view.SetData(difficultyBeatmapViewPair.difficulty);
      }
    }

    protected override void DidActivate()
    {
      foreach (CopyDifficultyBeatmapSetView.CopyDifficultyBeatmapViewPair difficultyBeatmapViewPair in this._copyDifficultyBeatmapViewPairs)
      {
        difficultyBeatmapViewPair.view.copyBeatmapEvent += new Action<BeatmapDifficulty, bool, bool, bool, bool, bool, bool>(this.HandleCopyDifficultyCopeBeatmap);
        difficultyBeatmapViewPair.view.SetData(difficultyBeatmapViewPair.difficulty);
      }
    }

    protected override void DidDeactivate()
    {
      foreach (CopyDifficultyBeatmapSetView.CopyDifficultyBeatmapViewPair difficultyBeatmapViewPair in this._copyDifficultyBeatmapViewPairs)
      {
        difficultyBeatmapViewPair.view.copyBeatmapEvent -= new Action<BeatmapDifficulty, bool, bool, bool, bool, bool, bool>(this.HandleCopyDifficultyCopeBeatmap);
        difficultyBeatmapViewPair.view.SetData(difficultyBeatmapViewPair.difficulty);
      }
    }

    private void HandleCopyDifficultyCopeBeatmap(
      BeatmapDifficulty difficulty,
      bool notes,
      bool waypoints,
      bool obstacles,
      bool chains,
      bool arcs,
      bool events)
    {
      Action<BeatmapCharacteristicSO, BeatmapDifficulty, bool, bool, bool, bool, bool, bool> beatmapLevelEvent = this.copyBeatmapLevelEvent;
      if (beatmapLevelEvent == null)
        return;
      beatmapLevelEvent(this._beatmapCharacteristic, difficulty, notes, waypoints, obstacles, chains, arcs, events);
    }

    [Serializable]
    public class CopyDifficultyBeatmapViewPair
    {
      public BeatmapDifficulty difficulty;
      public CopyDifficultyBeatmapView view;
    }
  }
}
