// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BpmEditor.UI.Tools.BpmMovePlayHeadTool
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.BpmEditor.Commands;
using System;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.BpmEditor.UI.Tools
{
  public class BpmMovePlayHeadTool : MonoBehaviour
  {
    [SerializeField]
    private BpmRegionsInputController _inputController;
    [Inject]
    private readonly SignalBus _signalBus;

    protected void Start()
    {
      this._inputController.mouseBeginDragEvent += new Action<int>(this.HandleInputControllerMouseDrag);
      this._inputController.mouseDragEvent += new Action<int>(this.HandleInputControllerMouseDrag);
      this._inputController.mouseEndDragEvent += new Action<int>(this.HandleInputControllerMouseDrag);
      this._inputController.mouseDownEvent += new Action<int>(this.HandleInputControllerMouseDown);
    }

    protected void OnDestroy()
    {
      this._inputController.mouseBeginDragEvent -= new Action<int>(this.HandleInputControllerMouseDrag);
      this._inputController.mouseDragEvent -= new Action<int>(this.HandleInputControllerMouseDrag);
      this._inputController.mouseEndDragEvent -= new Action<int>(this.HandleInputControllerMouseDrag);
      this._inputController.mouseDownEvent -= new Action<int>(this.HandleInputControllerMouseDown);
    }

    private void HandleInputControllerMouseDown(int sample) => this._signalBus.Fire<SetPlayHeadSignal>(new SetPlayHeadSignal(sample, true));

    private void HandleInputControllerMouseDrag(int sample) => this._signalBus.Fire<SetPlayHeadSignal>(new SetPlayHeadSignal(sample, true));
  }
}
