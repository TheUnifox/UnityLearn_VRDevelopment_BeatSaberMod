// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BpmEditor.UI.Tools.BpmEditorTool
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.BpmEditor.Commands.Tools;
using System;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.BpmEditor.UI.Tools
{
  public abstract class BpmEditorTool : MonoBehaviour, IBpmEditorTool
  {
    [SerializeField]
    private BpmEditorToolType _toolType;
    [Inject]
    protected readonly BpmEditorDataModel _bpmEditorDataModel;
    [Inject]
    protected readonly BpmEditorState _bpmEditorState;
    [Inject]
    protected readonly SignalBus _signalBus;

    protected void Start()
    {
      this.SetToolState();
      this._signalBus.Subscribe<BpmToolSwitchedSignal>(new Action(this.HandleBpmToolSwitched));
    }

    protected void OnDestroy() => this._signalBus.TryUnsubscribe<BpmToolSwitchedSignal>(new Action(this.HandleBpmToolSwitched));

    private void HandleBpmToolSwitched() => this.SetToolState();

    public abstract void EnableTool();

    public abstract void DisableTool();

    private void SetToolState()
    {
      if (this._toolType == this._bpmEditorState.bpmToolType)
        this.EnableTool();
      else
        this.DisableTool();
    }
  }
}
