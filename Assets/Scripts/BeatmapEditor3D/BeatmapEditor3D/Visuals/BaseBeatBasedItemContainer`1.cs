// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Visuals.BaseBeatBasedItemContainer`1
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Visuals
{
  public abstract class BaseBeatBasedItemContainer<T> : MonoBehaviour where T : Component
  {
    [SerializeField]
    private float _windowStartOffset;
    [SerializeField]
    private float _windowEndOffset = 16f;
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    [Inject]
    private readonly BeatmapDataModel _beatmapDataModel;
    [Inject]
    private readonly BeatmapObjectPlacementHelper _beatmapObjectPlacementHelper;
    private Vector3 _positionOffset;
    private float _beatOffset;
    private int _spawnSubdivision;
    private float _spawnIncrement;
    private Func<float, bool> _shouldSkipItemFunc;
    private readonly List<T> _currentWindow = new List<T>();

    public void Enable()
    {
      this._signalBus.Subscribe<BeatmapLevelStateTimeUpdated>(new Action(this.HandleLevelEditorStateTimeUpdated));
      this._signalBus.Subscribe<BeatmapTimeScaleChangedSignal>(new Action(this.HandleBeatmapTimeScaleChanged));
      this.UpdateItems();
    }

    public void Disable()
    {
      this._signalBus.TryUnsubscribe<BeatmapLevelStateTimeUpdated>(new Action(this.HandleLevelEditorStateTimeUpdated));
      this._signalBus.TryUnsubscribe<BeatmapTimeScaleChangedSignal>(new Action(this.HandleBeatmapTimeScaleChanged));
      this.DespawnItems();
    }

    public void SetData(
      Vector3 positionOffset,
      int subdivision,
      float spawnIncrement,
      Func<float, bool> shouldSkipItemFunc)
    {
      this._positionOffset = positionOffset;
      this._spawnSubdivision = subdivision;
      this._spawnIncrement = spawnIncrement;
      this._shouldSkipItemFunc = shouldSkipItemFunc;
    }

    public void ForceUpdate()
    {
      if (!this.enabled)
        return;
      this.DespawnItems();
      this.UpdateItems();
    }

    protected abstract T SpawnItem();

    protected abstract void DespawnItem(T item);

    protected abstract void SetItemData(T item, float beat);

    public void UpdateItems()
    {
      double num1 = (double) Mathf.Max(AudioTimeHelper.RoundUpToBeat(this._beatmapState.beat - this._beatmapState.beatOffset - this._windowStartOffset, this._spawnSubdivision), 0.0f);
      float num2 = Mathf.Min(AudioTimeHelper.RoundDownToBeat(this._beatmapState.beat - this._beatmapState.beatOffset + this._windowEndOffset, this._spawnSubdivision), this._beatmapDataModel.bpmData.totalBeats - this._beatmapState.beatOffset);
      int index1 = 0;
      for (float beat = (float) num1; (double) beat < (double) num2; beat += this._spawnIncrement)
      {
        Func<float, bool> shouldSkipItemFunc = this._shouldSkipItemFunc;
        if ((shouldSkipItemFunc != null ? (shouldSkipItemFunc(beat) ? 1 : 0) : 0) == 0)
        {
          if (index1 >= this._currentWindow.Count)
            this._currentWindow.Add(this.SpawnItem());
          T obj = this._currentWindow[index1];
          this.SetItemData(obj, beat);
          if (!obj.gameObject.activeSelf)
            obj.gameObject.SetActive(true);
          obj.transform.localPosition = new Vector3(0.0f, 0.0f, this._beatmapObjectPlacementHelper.BeatToPosition(this._beatmapState.beatOffset + beat, this._beatmapState.beat)) + this._positionOffset;
          ++index1;
        }
      }
      for (int index2 = index1; index2 < this._currentWindow.Count; ++index2)
        this._currentWindow[index2].gameObject.SetActive(false);
    }

    private void DespawnItems()
    {
      for (int index = 0; index < this._currentWindow.Count; ++index)
        this.DespawnItem(this._currentWindow[index]);
      this._currentWindow.Clear();
    }

    private void HandleLevelEditorStateTimeUpdated() => this.UpdateItems();

    private void HandleBeatmapTimeScaleChanged() => this.UpdateItems();
  }
}
