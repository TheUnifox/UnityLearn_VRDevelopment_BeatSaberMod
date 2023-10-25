// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BpmEditor.Commands.Tools.SetHoverSampleCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using Zenject;

namespace BeatmapEditor3D.BpmEditor.Commands.Tools
{
  public class SetHoverSampleCommand : IBeatmapEditorCommand
  {
    [Inject]
    private readonly SetHoverSampleSignal _signal;
    [Inject]
    private readonly BpmEditorState _bpmEditorState;
    [Inject]
    private readonly BpmEditorDataModel _bpmEditorDataModel;
    [Inject]
    private readonly SignalBus _signalBus;

    public void Execute()
    {
      this._bpmEditorState.hoverSample = this._signal.sample;
      int hoverBpmRegionIdx1 = this._bpmEditorState.hoverBpmRegionIdx;
      this._bpmEditorState.hoverBpmRegionIdx = PlayHeadHelpers.FindIndex(this._bpmEditorDataModel.regions, this._bpmEditorState.hoverBpmRegionIdx, this._signal.sample);
      int hoverBpmRegionIdx2 = this._bpmEditorState.hoverBpmRegionIdx;
      this._signalBus.Fire<HoverSampleChangedSignal>();
    }
  }
}
