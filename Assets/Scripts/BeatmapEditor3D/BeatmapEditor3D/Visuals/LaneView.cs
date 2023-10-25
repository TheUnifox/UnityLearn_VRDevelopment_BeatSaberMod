// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Visuals.LaneView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Visuals
{
  public class LaneView : MonoBehaviour
  {
    [SerializeField]
    protected Transform _eventLaneBackgroundTransform;
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    [Inject]
    private readonly BeatmapObjectPlacementHelper _beatmapObjectPlacementHelper;
    [Inject]
    private readonly BeatmapDataModel _beatmapDataModel;
    private float _currentLength;

    protected void OnEnable()
    {
      this._signalBus.Subscribe<BeatmapLevelStateTimeUpdated>(new Action(this.HandleBeatmapLevelStateTimeUpdated));
      this._signalBus.Subscribe<BeatmapTimeScaleChangedSignal>(new Action(this.HandleBeatmapTimeScaleChanged));
      this.UpdateTime();
    }

    protected void OnDisable()
    {
      this._signalBus.TryUnsubscribe<BeatmapLevelStateTimeUpdated>(new Action(this.HandleBeatmapLevelStateTimeUpdated));
      this._signalBus.TryUnsubscribe<BeatmapTimeScaleChangedSignal>(new Action(this.HandleBeatmapTimeScaleChanged));
    }

        public void SetWidth(float width)
        {
            Vector3 localScale = this._eventLaneBackgroundTransform.localScale;
            localScale.x = width;
            this._eventLaneBackgroundTransform.localScale = localScale;
        }

        private void HandleBeatmapLevelStateTimeUpdated() => this.UpdateTime();

    private void HandleBeatmapTimeScaleChanged() => this.UpdateTime();

        private void UpdateTime()
        {
            float num = this._beatmapObjectPlacementHelper.BeatToPosition(Mathf.Max(this._beatmapState.beat - 5f, 0f), this._beatmapState.beat);
            float num2 = this._beatmapObjectPlacementHelper.BeatToPosition(Mathf.Min(this._beatmapState.beat + 16f, this._beatmapDataModel.bpmData.totalBeats), this._beatmapState.beat) - num;
            if (Mathf.Approximately(this._currentLength, num2))
            {
                return;
            }
            this._currentLength = num2;
            Vector3 localPosition = this._eventLaneBackgroundTransform.localPosition;
            localPosition.z = num;
            this._eventLaneBackgroundTransform.localPosition = localPosition;
            Vector3 localScale = this._eventLaneBackgroundTransform.localScale;
            localScale.y = num2;
            this._eventLaneBackgroundTransform.localScale = localScale;
        }
    }
}
