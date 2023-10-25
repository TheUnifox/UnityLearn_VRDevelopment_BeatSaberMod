// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BpmEditor.Commands.PlayHeadUpdatePreviewCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System.Collections.Generic;
using Zenject;

namespace BeatmapEditor3D.BpmEditor.Commands
{
  public class PlayHeadUpdatePreviewCommand : IBeatmapEditorCommand
  {
    [Inject]
    private readonly BpmEditorDataModel _bpmEditorDataModel;
    [Inject]
    private readonly BpmEditorState _bpmEditorState;

    public void Execute()
    {
      int sample1 = this._bpmEditorState.sample;
      IReadOnlyList<BpmRegion> regions = this._bpmEditorDataModel.regions;
      (int num1, int num2) = this._bpmEditorState.previewRegionWindow;
      int sample2 = sample1 - this._bpmEditorState.previewHalfSize;
      int sample3 = sample1 + this._bpmEditorState.previewHalfSize;
      this._bpmEditorState.previewRegionWindow = (PlayHeadHelpers.FindIndex(regions, num1, sample2), PlayHeadHelpers.FindIndex(regions, num2, sample3));
    }
  }
}
