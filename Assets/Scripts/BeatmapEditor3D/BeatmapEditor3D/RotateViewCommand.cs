// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.RotateViewCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class RotateViewCommand : IBeatmapEditorCommand
  {
    [Inject]
    private readonly RotateViewSignal _rotateViewSignal;
    [Inject]
    private readonly BeatmapState _beatmapState;
    [Inject]
    private readonly IReadonlyBeatmapLevelDataModel _beatmapLevelDataModel;
    [Inject]
    private readonly SignalBus _signalBus;
    private const int kRotationAngle = 15;
    private const int k360Clamp = 360;
    private const int k90Clamp = 45;

    public void Execute()
    {
      switch (this._rotateViewSignal.direction)
      {
        case RotateViewSignal.Direction.Left:
          this.RotateLeft();
          break;
        case RotateViewSignal.Direction.Right:
          this.RotateRight();
          break;
      }
      this._signalBus.Fire<ViewRotatedSignal>();
    }

    private void RotateLeft()
    {
      if (!this.CanRotate())
        return;
      this._beatmapState.rotation = this.WrapRotationValue(this._beatmapState.rotation - 15);
    }

    private void RotateRight()
    {
      if (!this.CanRotate())
        return;
      this._beatmapState.rotation = this.WrapRotationValue(this._beatmapState.rotation + 15);
    }

    private bool CanRotate() => this._beatmapLevelDataModel.beatmapCharacteristic.containsRotationEvents;

    private int WrapRotationValue(int value)
    {
      if (!this._beatmapLevelDataModel.beatmapCharacteristic.requires360Movement)
        return Mathf.Clamp(value, -45, 45);
      if (value > 360)
        return value - 360;
      return value < -360 ? value + 360 : value;
    }
  }
}
