// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.ObstacleLengthToolbarView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using BeatmapEditor3D.Views;
using System;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class ObstacleLengthToolbarView : BeatmapEditorView
  {
    [SerializeField]
    private FloatInputFieldValidator _obstacleLengthValidator;
    [Inject]
    private readonly IReadonlyBeatmapObjectsState _readonlyBeatmapObjectsState;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    [Inject]
    private readonly SignalBus _signalBus;

    protected override void DidActivate()
    {
      this._obstacleLengthValidator.onInputValidated += new Action<float>(this.HandleObstacleLengthInputOnEndEdit);
      this._signalBus.Subscribe<ObstacleDurationChangedSignal>(new Action(this.HandleObstacleDurationChanged));
      this.SetData();
    }

    protected override void DidDeactivate()
    {
      this._obstacleLengthValidator.onInputValidated -= new Action<float>(this.HandleObstacleLengthInputOnEndEdit);
      this._signalBus.TryUnsubscribe<ObstacleDurationChangedSignal>(new Action(this.HandleObstacleDurationChanged));
    }

    private void HandleObstacleLengthInputOnEndEdit(float length)
    {
      if (this._beatmapState.cameraMoving)
        return;
      this._signalBus.Fire<ChangeObstacleDurationSignal>(new ChangeObstacleDurationSignal(length));
      this.SetData();
    }

    private void HandleObstacleDurationChanged() => this.SetData();

    private void SetData() => this._obstacleLengthValidator.value = this._readonlyBeatmapObjectsState.obstacleDuration;
  }
}
