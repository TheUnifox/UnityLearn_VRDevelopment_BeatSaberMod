// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BpmEditor.BpmEditorAudioScrollController
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.BpmEditor.Commands;
using HMUI;
using System;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace BeatmapEditor3D.BpmEditor
{
  public class BpmEditorAudioScrollController : ITickable
  {
    private const int kSmallScrollSampleDelta = 1;
    private const float kNormalScrollDelta = 0.1f;
    [Inject]
    private readonly BpmEditorSongPreviewController _songPreviewController;
    [Inject]
    private readonly BpmEditorState _bpmEditorState;
    [Inject]
    private readonly SignalBus _signalBus;
    private bool _enabled;
    private readonly MouseBinder _mouseBinder = new MouseBinder();

    public void Enable()
    {
      if (this._enabled)
        return;
      this._enabled = true;
      this._mouseBinder.AddScrollBinding(new UnityAction<float>(this.HandleMouseScroll));
      this._songPreviewController.playHeadPositionChangedEvent += new Action<int>(this.HandleSongPreviewControllerPlayHeadPositionChanged);
    }

    public void Disable()
    {
      if (!this._enabled)
        return;
      this._enabled = false;
      this._mouseBinder.ClearBindings();
      this._songPreviewController.playHeadPositionChangedEvent -= new Action<int>(this.HandleSongPreviewControllerPlayHeadPositionChanged);
    }

    public void Tick() => this._mouseBinder.ManualUpdate();

    private void HandleMouseScroll(float scroll)
    {
      if (Input.GetMouseButton(0))
        return;
      if ((Input.GetKey(KeyCode.LeftControl) ? 1 : (Input.GetKey(KeyCode.RightControl) ? 1 : 0)) != 0)
      {
        this._signalBus.Fire<PlayHeadZoomSignal>(new PlayHeadZoomSignal((int) ((double) Mathf.Sign(scroll) * (double) this._songPreviewController.audioClip.frequency * 0.10000000149011612)));
      }
      else
      {
        if (this._songPreviewController.isPlaying)
          return;
        bool flag = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        this._signalBus.Fire<SetPlayHeadSignal>(new SetPlayHeadSignal(this._bpmEditorState.sample + (int) ((double) Mathf.Sign(scroll) * (flag ? 1.0 : (double) this._songPreviewController.audioClip.frequency * 0.10000000149011612))));
      }
    }

    private void HandleSongPreviewControllerPlayHeadPositionChanged(int sample) => this._signalBus.Fire<SetPlayHeadSignal>(new SetPlayHeadSignal(sample));
  }
}
