// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.NoteCutDirectionToolbarView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Commands;
using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using BeatmapEditor3D.Views;
using HMUI;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BeatmapEditor3D
{
  public class NoteCutDirectionToolbarView : BeatmapEditorView
  {
    [SerializeField]
    private Toggle _upToggle;
    [SerializeField]
    private Toggle _upRightToggle;
    [SerializeField]
    private Toggle _rightToggle;
    [SerializeField]
    private Toggle _downRightToggle;
    [SerializeField]
    private Toggle _downToggle;
    [SerializeField]
    private Toggle _downLeftToggle;
    [SerializeField]
    private Toggle _leftToggle;
    [SerializeField]
    private Toggle _upLeftToggle;
    [SerializeField]
    private Toggle _anyToggle;
    [Space]
    [SerializeField]
    private Toggle _0DegToggle;
    [SerializeField]
    private Toggle _45DegToggle;
    [Inject]
    private readonly IReadonlyBeatmapObjectsState _readonlyBeatmapObjectsState;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    [Inject]
    private readonly SignalBus _signalBus;
    private readonly ToggleBinder _toggleBinder = new ToggleBinder();

    protected override void DidActivate()
    {
      this._toggleBinder.AddBinding(this._upToggle, this.GetNoteCutDirectionToggleHandler(NoteCutDirection.Up));
      this._toggleBinder.AddBinding(this._upRightToggle, this.GetNoteCutDirectionToggleHandler(NoteCutDirection.UpRight));
      this._toggleBinder.AddBinding(this._rightToggle, this.GetNoteCutDirectionToggleHandler(NoteCutDirection.Right));
      this._toggleBinder.AddBinding(this._downRightToggle, this.GetNoteCutDirectionToggleHandler(NoteCutDirection.DownRight));
      this._toggleBinder.AddBinding(this._downToggle, this.GetNoteCutDirectionToggleHandler(NoteCutDirection.Down));
      this._toggleBinder.AddBinding(this._downLeftToggle, this.GetNoteCutDirectionToggleHandler(NoteCutDirection.DownLeft));
      this._toggleBinder.AddBinding(this._leftToggle, this.GetNoteCutDirectionToggleHandler(NoteCutDirection.Left));
      this._toggleBinder.AddBinding(this._upLeftToggle, this.GetNoteCutDirectionToggleHandler(NoteCutDirection.UpLeft));
      this._toggleBinder.AddBinding(this._anyToggle, this.GetNoteCutDirectionToggleHandler(NoteCutDirection.Any));
      this._toggleBinder.AddBinding(this._0DegToggle, this.GetDotNoteAngleToggleHandler(0));
      this._toggleBinder.AddBinding(this._45DegToggle, this.GetDotNoteAngleToggleHandler(45));
      this._signalBus.Subscribe<NoteCutDirectionChangedSignal>(new Action<NoteCutDirectionChangedSignal>(this.HandleNoteCutDirectionChanged));
      this._signalBus.Subscribe<DotNoteAngleChangedSignal>(new Action<DotNoteAngleChangedSignal>(this.HandleDotNoteAngleChanged));
      this.SetNoteCutDirectionData(this._readonlyBeatmapObjectsState.noteCutDirection);
    }

    protected override void DidDeactivate()
    {
      this._toggleBinder.ClearBindings();
      this._signalBus.TryUnsubscribe<DotNoteAngleChangedSignal>(new Action<DotNoteAngleChangedSignal>(this.HandleDotNoteAngleChanged));
      this._signalBus.TryUnsubscribe<NoteCutDirectionChangedSignal>(new Action<NoteCutDirectionChangedSignal>(this.HandleNoteCutDirectionChanged));
    }

    private void HandleNoteCutDirectionChanged(NoteCutDirectionChangedSignal signal) => this.SetNoteCutDirectionData(signal.noteCutDirection);

    private void HandleDotNoteAngleChanged(DotNoteAngleChangedSignal signal) => this.SetAngleData(signal.angle);

    private void SetNoteCutDirectionData(NoteCutDirection noteCutDirection) => this.GetNoteCutDirectionToggle(noteCutDirection).SetIsOnWithoutNotify(true);

    private void SetAngleData(int angle) => this.GetAngleToggle(angle).SetIsOnWithoutNotify(true);

    private Toggle GetNoteCutDirectionToggle(NoteCutDirection noteCutDirection)
    {
      switch (noteCutDirection)
      {
        case NoteCutDirection.Up:
          return this._upToggle;
        case NoteCutDirection.Down:
          return this._downToggle;
        case NoteCutDirection.Left:
          return this._leftToggle;
        case NoteCutDirection.Right:
          return this._rightToggle;
        case NoteCutDirection.UpLeft:
          return this._upLeftToggle;
        case NoteCutDirection.UpRight:
          return this._upRightToggle;
        case NoteCutDirection.DownLeft:
          return this._downLeftToggle;
        case NoteCutDirection.DownRight:
          return this._downRightToggle;
        default:
          return this._anyToggle;
      }
    }

    private Toggle GetAngleToggle(int angle) => angle == 45 ? this._45DegToggle : this._0DegToggle;

    private Action<bool> GetNoteCutDirectionToggleHandler(NoteCutDirection noteCutDirection) => (Action<bool>) (_ =>
    {
      if (this._beatmapState.cameraMoving)
        return;
      this._signalBus.Fire<ChangeNoteCutDirectionSignal>(new ChangeNoteCutDirectionSignal(noteCutDirection));
    });

    private Action<bool> GetDotNoteAngleToggleHandler(int angle) => (Action<bool>) (_ =>
    {
      if (this._beatmapState.cameraMoving)
        return;
      this._signalBus.Fire<ChangeNoteCutDirectionSignal>(new ChangeNoteCutDirectionSignal(NoteCutDirection.Any));
      this._signalBus.Fire<ChangeDotNoteAngleSignal>(new ChangeDotNoteAngleSignal(angle));
    });
  }
}
