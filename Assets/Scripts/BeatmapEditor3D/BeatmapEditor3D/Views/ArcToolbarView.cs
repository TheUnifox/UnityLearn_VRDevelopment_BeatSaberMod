// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.ArcToolbarView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Commands;
using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using HMUI;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace BeatmapEditor3D.Views
{
  public class ArcToolbarView : BeatmapEditorView
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
    private Toggle _straightMidAnchor;
    [SerializeField]
    private Toggle _clockwiseMidAnchor;
    [SerializeField]
    private Toggle _counterClockwiseMidAnchor;
    [Space]
    [SerializeField]
    private TMP_InputField _headControlPointLengthInput;
    [SerializeField]
    private TMP_InputField _tailControlPointLengthInput;
    [Inject]
    private readonly BeatmapObjectsState _beatmapObjectsState;
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
      this._toggleBinder.AddBinding(this._straightMidAnchor, this.GetSliderMidAnchorModeToggleHandler(SliderMidAnchorMode.Straight));
      this._toggleBinder.AddBinding(this._clockwiseMidAnchor, this.GetSliderMidAnchorModeToggleHandler(SliderMidAnchorMode.Clockwise));
      this._toggleBinder.AddBinding(this._counterClockwiseMidAnchor, this.GetSliderMidAnchorModeToggleHandler(SliderMidAnchorMode.CounterClockwise));
      this._signalBus.Subscribe<NoteCutDirectionChangedSignal>(new Action<NoteCutDirectionChangedSignal>(this.HandleNoteCutDirectionChanged));
      this._signalBus.Subscribe<ArcMidAnchorModeChangedSignal>(new Action<ArcMidAnchorModeChangedSignal>(this.HandleArcMidAnchorModeChanged));
      this._signalBus.Subscribe<ArcControlPointLengthChangedSignal>(new Action(this.HandleArcControlPointLengthChanged));
      this._headControlPointLengthInput.onEndEdit.AddListener(new UnityAction<string>(this.HandleHeadControlPointLengthInputOnEndEdit));
      this._tailControlPointLengthInput.onEndEdit.AddListener(new UnityAction<string>(this.HandleTailControlPointLengthInputOnEndEdit));
      this.SetNoteCutDirection(this._beatmapObjectsState.noteCutDirection);
      this.SetArcMidAnchorMode(this._beatmapObjectsState.arcMidAnchorMode);
      this.SetArcControlPointLengths(this._beatmapObjectsState.arcControlPointLengthMultiplier, this._beatmapObjectsState.arcTailControlPointLengthMultiplier);
    }

    protected override void DidDeactivate()
    {
      this._toggleBinder.ClearBindings();
      this._headControlPointLengthInput.onEndEdit.RemoveListener(new UnityAction<string>(this.HandleHeadControlPointLengthInputOnEndEdit));
      this._tailControlPointLengthInput.onEndEdit.RemoveListener(new UnityAction<string>(this.HandleTailControlPointLengthInputOnEndEdit));
      this._signalBus.TryUnsubscribe<NoteCutDirectionChangedSignal>(new Action<NoteCutDirectionChangedSignal>(this.HandleNoteCutDirectionChanged));
      this._signalBus.TryUnsubscribe<ArcMidAnchorModeChangedSignal>(new Action<ArcMidAnchorModeChangedSignal>(this.HandleArcMidAnchorModeChanged));
      this._signalBus.TryUnsubscribe<ArcControlPointLengthChangedSignal>(new Action(this.HandleArcControlPointLengthChanged));
    }

    private void HandleNoteCutDirectionChanged(NoteCutDirectionChangedSignal signal) => this.SetNoteCutDirection(signal.noteCutDirection);

    private void HandleArcMidAnchorModeChanged(ArcMidAnchorModeChangedSignal signal) => this.SetArcMidAnchorMode(signal.sliderMidAnchorMode);

    private void HandleArcControlPointLengthChanged() => this.SetArcControlPointLengths(this._beatmapObjectsState.arcControlPointLengthMultiplier, this._beatmapObjectsState.arcTailControlPointLengthMultiplier);

    private void SetNoteCutDirection(NoteCutDirection noteCutDirection) => this.GetNoteCutDirectionToggle(noteCutDirection).SetIsOnWithoutNotify(true);

    private void SetArcMidAnchorMode(SliderMidAnchorMode sliderMidAnchorMode) => this.GetSliderMidAnchorModeToggle(sliderMidAnchorMode).SetIsOnWithoutNotify(true);

    private void SetArcControlPointLengths(
      float headControlPointLengthMultiplier,
      float tailControlPointLengthMultiplier)
    {
      this._headControlPointLengthInput.SetTextWithoutNotify(string.Format("{0}", (object) headControlPointLengthMultiplier));
      this._tailControlPointLengthInput.SetTextWithoutNotify(string.Format("{0}", (object) tailControlPointLengthMultiplier));
    }

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

    private Action<bool> GetNoteCutDirectionToggleHandler(NoteCutDirection noteCutDirection) => (Action<bool>) (_ =>
    {
      if (this._beatmapState.cameraMoving)
        return;
      this._signalBus.Fire<ChangeNoteCutDirectionSignal>(new ChangeNoteCutDirectionSignal(noteCutDirection));
    });

    private Toggle GetSliderMidAnchorModeToggle(SliderMidAnchorMode sliderMidAnchorMode)
    {
      switch (sliderMidAnchorMode)
      {
        case SliderMidAnchorMode.Straight:
          return this._straightMidAnchor;
        case SliderMidAnchorMode.Clockwise:
          return this._clockwiseMidAnchor;
        case SliderMidAnchorMode.CounterClockwise:
          return this._counterClockwiseMidAnchor;
        default:
          return this._straightMidAnchor;
      }
    }

    private Action<bool> GetSliderMidAnchorModeToggleHandler(SliderMidAnchorMode sliderMidAnchorMode) => (Action<bool>) (_ =>
    {
      if (this._beatmapState.cameraMoving)
        return;
      this._signalBus.Fire<ChangeArcMidAnchorModeSignal>(new ChangeArcMidAnchorModeSignal(sliderMidAnchorMode));
    });

    private void HandleHeadControlPointLengthInputOnEndEdit(string input)
    {
      float result;
      if (this._beatmapState.cameraMoving || !float.TryParse(input, out result))
        return;
      this._signalBus.Fire<ChangeArcHeadControlPointLengthSignal>(new ChangeArcHeadControlPointLengthSignal(result));
    }

    private void HandleTailControlPointLengthInputOnEndEdit(string input)
    {
      float result;
      if (this._beatmapState.cameraMoving || !float.TryParse(input, out result))
        return;
      this._signalBus.Fire<ChangeArcTailControlPointLengthSignal>(new ChangeArcTailControlPointLengthSignal(result));
    }
  }
}
