// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.InvertHoveredBeatmapObjectColorTypeCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using System.Collections.Generic;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public abstract class InvertHoveredBeatmapObjectColorTypeCommand : 
    IBeatmapEditorCommandWithHistory,
    IBeatmapEditorCommand
  {
    [Inject]
    protected readonly BeatmapLevelDataModel _beatmapLevelDataModel;
    [Inject]
    private readonly SignalBus _signalBus;
    private List<NoteEditorData> _originalNotes;
    private List<ArcEditorData> _originalArcs;
    private List<ChainEditorData> _originalChains;
    private List<NoteEditorData> _invertedNotes;
    private List<ArcEditorData> _invertedArcs;
    private List<ChainEditorData> _invertedChains;

    public bool shouldAddToHistory { get; private set; }

    protected abstract bool IsValid();

    protected abstract BeatmapObjectCellData GetCellData();

    public void Execute()
    {
      if (!this.IsValid())
        return;
      SliderHelpers.WalkArcsAndChains(this._beatmapLevelDataModel, this.GetCellData(), out this._originalNotes, out this._originalArcs, out this._originalChains);
      SliderHelpers.InvertColorTypeForBeatmapObjects(this._originalNotes, this._originalArcs, this._originalChains, out this._invertedNotes, out this._invertedArcs, out this._invertedChains);
      this.shouldAddToHistory = true;
      this.Redo();
    }

    public void Undo()
    {
      this._beatmapLevelDataModel.RemoveNotes((IEnumerable<NoteEditorData>) this._invertedNotes);
      this._beatmapLevelDataModel.RemoveArcs((IEnumerable<ArcEditorData>) this._invertedArcs);
      this._beatmapLevelDataModel.RemoveChains((IEnumerable<ChainEditorData>) this._invertedChains);
      this._beatmapLevelDataModel.AddNotes((IEnumerable<NoteEditorData>) this._originalNotes);
      this._beatmapLevelDataModel.AddArcs((IEnumerable<ArcEditorData>) this._originalArcs);
      this._beatmapLevelDataModel.AddChains((IEnumerable<ChainEditorData>) this._originalChains);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public void Redo()
    {
      this._beatmapLevelDataModel.RemoveNotes((IEnumerable<NoteEditorData>) this._originalNotes);
      this._beatmapLevelDataModel.RemoveArcs((IEnumerable<ArcEditorData>) this._originalArcs);
      this._beatmapLevelDataModel.RemoveChains((IEnumerable<ChainEditorData>) this._originalChains);
      this._beatmapLevelDataModel.AddNotes((IEnumerable<NoteEditorData>) this._invertedNotes);
      this._beatmapLevelDataModel.AddArcs((IEnumerable<ArcEditorData>) this._invertedArcs);
      this._beatmapLevelDataModel.AddChains((IEnumerable<ChainEditorData>) this._invertedChains);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }
  }
}
