// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Controller.ScrollInputController
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using HMUI;
using System;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace BeatmapEditor3D.Controller
{
  public class ScrollInputController : ITickable
  {
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    [Inject]
    private readonly IReadonlyBeatmapDataModel _beatmapDataModel;
    [Inject]
    private readonly BeatmapEditorSettingsDataModel _beatmapEditorSettingsDataModel;
    [Inject]
    private readonly ISongPreviewController _songPreviewController;
    [Inject]
    private readonly MouseBinder _mouseBinder;
    [Inject]
    private readonly KeyboardBinder _keyboardBinder;
    private bool _enabled;

    public void Enable(int length)
    {
      if (this._enabled)
        return;
      this._beatmapState.previewData.Init(this._beatmapDataModel.audioClip.samples);
      this._beatmapState.previewData.SetPreviewLength(length);
      this._beatmapState.previewData.CenterPreviewStart();
      this._mouseBinder.AddScrollBinding(new UnityAction<float>(this.MouseScrollHandler));
      this._keyboardBinder.AddBinding(KeyCode.X, KeyboardBinder.KeyBindingType.KeyDown, new Action<bool>(this.HandleSubdivisionModeKeyPressed));
      this._songPreviewController.playheadPositionChangedEvent += new Action<int>(this.SongPreviewControllerOnPlayHeadPositionChanged);
      this._enabled = true;
    }

    public void Disable()
    {
      if (!this._enabled)
        return;
      this._mouseBinder.ClearBindings();
      this._keyboardBinder.ClearBindings();
      this._songPreviewController.playheadPositionChangedEvent -= new Action<int>(this.SongPreviewControllerOnPlayHeadPositionChanged);
      this._enabled = false;
    }

    public void Tick()
    {
      this._mouseBinder.ManualUpdate();
      this._keyboardBinder.ManualUpdate();
    }

    private void HandleSubdivisionModeKeyPressed(bool pressed)
    {
      if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        return;
      this._signalBus.Fire<SwapSubdivisionSignal>(new SwapSubdivisionSignal());
    }

    private void MouseScrollHandler(float scrollDeltaY)
    {
      Input.GetMouseButton(0);
      int num = Input.GetKey(KeyCode.LeftShift) ? 1 : (Input.GetKey(KeyCode.RightShift) ? 1 : 0);
      if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
        return;
      if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
      {
        ChangeSubdivisionSignal.Type type = ChangeSubdivisionSignal.Type.Multiplication;
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
          type = ChangeSubdivisionSignal.Type.Subdivision;
        this._signalBus.Fire<ChangeSubdivisionSignal>(new ChangeSubdivisionSignal(type, scrollDeltaY));
      }
      else
      {
        if (this._songPreviewController.isPlaying)
          return;
        this._signalBus.Fire<UpdatePlayHeadSignal>(new UpdatePlayHeadSignal(this._beatmapDataModel.bpmData.BeatToSample(AudioTimeHelper.ChangeBeatBySubdivision(this._beatmapState.beat, this._beatmapState.subdivision * 128, (this._beatmapEditorSettingsDataModel.invertTimelineScroll ? (int) -scrollDeltaY : (int) scrollDeltaY) * 128)), UpdatePlayHeadSignal.SnapType.SmallestSubdivision, false));
      }
    }

    private void SongPreviewControllerOnPlayHeadPositionChanged(int currentSample) => this._signalBus.Fire<UpdatePlayHeadSignal>(new UpdatePlayHeadSignal(currentSample, UpdatePlayHeadSignal.SnapType.None, false));
  }
}
