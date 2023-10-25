// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.DifficultyBeatmapSetView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System;
using UnityEngine;

namespace BeatmapEditor3D.Views
{
  public class DifficultyBeatmapSetView : BeatmapEditorView
  {
    [SerializeField]
    private BeatmapCharacteristicSO _beatmapCharacteristic;
    [SerializeField]
    private DifficultyBeatmapSetView.DifficultyBeatmapViewPair[] _difficultyBeatmapPairs;

    public event Action<BeatmapCharacteristicSO, BeatmapDifficulty> newBeatmapEvent;

    public event Action<BeatmapCharacteristicSO, BeatmapDifficulty> editBeatmapEvent;

    public event Action<BeatmapCharacteristicSO, BeatmapDifficulty> deleteBeatmapEvent;

    public event Action<BeatmapCharacteristicSO, BeatmapDifficulty, float, float> beatmapDataChangedEvent;

    public DifficultyBeatmapSetView.DifficultyBeatmapViewPair[] difficultyBeatmapViewPairs => this._difficultyBeatmapPairs;

    public void SetData(IDifficultyBeatmapSetData difficultyBeatmapSetData)
    {
      foreach (DifficultyBeatmapSetView.DifficultyBeatmapViewPair difficultyBeatmapPair in this._difficultyBeatmapPairs)
      {
        IDifficultyBeatmapData beatmapData = (IDifficultyBeatmapData) null;
        difficultyBeatmapSetData?.difficultyBeatmaps.TryGetValue(difficultyBeatmapPair.difficulty, out beatmapData);
        difficultyBeatmapPair.view.SetData(difficultyBeatmapPair.difficulty, beatmapData);
      }
    }

    public void SetState(bool canEdit, bool deleteMode)
    {
      foreach (DifficultyBeatmapSetView.DifficultyBeatmapViewPair difficultyBeatmapPair in this._difficultyBeatmapPairs)
        difficultyBeatmapPair.view.SetState(canEdit, deleteMode);
    }

    public void ClearDirtyState()
    {
      foreach (DifficultyBeatmapSetView.DifficultyBeatmapViewPair difficultyBeatmapPair in this._difficultyBeatmapPairs)
        difficultyBeatmapPair.view.ClearDirtyState();
    }

    protected override void DidActivate()
    {
      foreach (DifficultyBeatmapSetView.DifficultyBeatmapViewPair difficultyBeatmapPair in this._difficultyBeatmapPairs)
      {
        difficultyBeatmapPair.view.newBeatmapEvent += new Action<BeatmapDifficulty>(this.HandleDifficultyBeatmapViewNewBeatmap);
        difficultyBeatmapPair.view.editBeatmapEvent += new Action<BeatmapDifficulty>(this.HandleDifficultyBeatmapViewEditBeatmap);
        difficultyBeatmapPair.view.deleteBeatmapEvent += new Action<BeatmapDifficulty>(this.HandleDifficultyBeatmapViewDeleteBeatmap);
        difficultyBeatmapPair.view.beatmapDataChangedEvent += new Action<BeatmapDifficulty, float, float>(this.HandleDifficultyBeatmapViewDataChanged);
      }
    }

    protected override void DidDeactivate()
    {
      foreach (DifficultyBeatmapSetView.DifficultyBeatmapViewPair difficultyBeatmapPair in this._difficultyBeatmapPairs)
      {
        difficultyBeatmapPair.view.newBeatmapEvent -= new Action<BeatmapDifficulty>(this.HandleDifficultyBeatmapViewNewBeatmap);
        difficultyBeatmapPair.view.editBeatmapEvent -= new Action<BeatmapDifficulty>(this.HandleDifficultyBeatmapViewEditBeatmap);
        difficultyBeatmapPair.view.deleteBeatmapEvent -= new Action<BeatmapDifficulty>(this.HandleDifficultyBeatmapViewDeleteBeatmap);
        difficultyBeatmapPair.view.beatmapDataChangedEvent -= new Action<BeatmapDifficulty, float, float>(this.HandleDifficultyBeatmapViewDataChanged);
      }
    }

    private void HandleDifficultyBeatmapViewNewBeatmap(BeatmapDifficulty difficulty)
    {
      Action<BeatmapCharacteristicSO, BeatmapDifficulty> newBeatmapEvent = this.newBeatmapEvent;
      if (newBeatmapEvent == null)
        return;
      newBeatmapEvent(this._beatmapCharacteristic, difficulty);
    }

    private void HandleDifficultyBeatmapViewEditBeatmap(BeatmapDifficulty difficulty)
    {
      Action<BeatmapCharacteristicSO, BeatmapDifficulty> editBeatmapEvent = this.editBeatmapEvent;
      if (editBeatmapEvent == null)
        return;
      editBeatmapEvent(this._beatmapCharacteristic, difficulty);
    }

    private void HandleDifficultyBeatmapViewDeleteBeatmap(BeatmapDifficulty difficulty)
    {
      Action<BeatmapCharacteristicSO, BeatmapDifficulty> deleteBeatmapEvent = this.deleteBeatmapEvent;
      if (deleteBeatmapEvent == null)
        return;
      deleteBeatmapEvent(this._beatmapCharacteristic, difficulty);
    }

    private void HandleDifficultyBeatmapViewDataChanged(
      BeatmapDifficulty difficulty,
      float njs,
      float offset)
    {
      Action<BeatmapCharacteristicSO, BeatmapDifficulty, float, float> dataChangedEvent = this.beatmapDataChangedEvent;
      if (dataChangedEvent == null)
        return;
      dataChangedEvent(this._beatmapCharacteristic, difficulty, njs, offset);
    }

    [Serializable]
    public class DifficultyBeatmapViewPair
    {
      public BeatmapDifficulty difficulty;
      public DifficultyBeatmapView view;
    }
  }
}
