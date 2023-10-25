// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.ChangeHoveredArcMidAnchorModeCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using Zenject;

namespace BeatmapEditor3D
{
  public class ChangeHoveredArcMidAnchorModeCommand : 
    IBeatmapEditorCommandWithHistoryMergeable,
    IBeatmapEditorCommandWithHistory,
    IBeatmapEditorCommand
  {
    [Inject]
    private readonly ChangeHoveredArcMidAnchorModeSignal _signal;
    [Inject]
    private readonly BeatmapLevelDataModel _beatmapLevelDataModel;
    [Inject]
    private readonly SignalBus _signalBus;
    private ArcEditorData _originalArc;
    private ArcEditorData _changedArc;

    public bool shouldAddToHistory { get; private set; }

    public void Execute()
    {
      this._originalArc = this._beatmapLevelDataModel.GetArcById(this._signal.id);
      if (this._originalArc == (ArcEditorData) null)
        return;
      this._changedArc = ArcEditorData.CopyWithModifications(this._originalArc, midAnchorMode: new SliderMidAnchorMode?(this._signal.midAnchorMode));
      this.shouldAddToHistory = true;
      this.Redo();
    }

    public void Undo()
    {
      this._beatmapLevelDataModel.RemoveArc(this._changedArc);
      this._beatmapLevelDataModel.AddArc(this._originalArc);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public void Redo()
    {
      this._beatmapLevelDataModel.RemoveArc(this._originalArc);
      this._beatmapLevelDataModel.AddArc(this._changedArc);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public bool ShouldMergeWith(
      IBeatmapEditorCommandWithHistoryMergeable previousCommand)
    {
      return previousCommand is ChangeHoveredArcMidAnchorModeCommand anchorModeCommand && this._originalArc.id == anchorModeCommand._originalArc.id;
    }

    public void MergeWith(
      IBeatmapEditorCommandWithHistoryMergeable previousCommand)
    {
      this._originalArc = ((ChangeHoveredArcMidAnchorModeCommand) previousCommand)._originalArc;
    }
  }
}
