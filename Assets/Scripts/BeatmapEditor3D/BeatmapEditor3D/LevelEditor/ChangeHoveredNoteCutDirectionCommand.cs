// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.LevelEditor.ChangeHoveredNoteCutDirectionCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using Zenject;

namespace BeatmapEditor3D.LevelEditor
{
  public class ChangeHoveredNoteCutDirectionCommand : 
    IBeatmapEditorCommandWithHistory,
    IBeatmapEditorCommand
  {
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly IReadonlyBeatmapObjectsState _readonlyBeatmapObjectsState;
    [Inject]
    private readonly ChangeHoveredNoteCutDirectionSignal _signal;
    [Inject]
    private readonly BeatmapLevelDataModel _beatmapLevelDataModel;
    private NoteEditorData _originalNote;
    private ChainEditorData _originalChain;
    private ArcEditorData _originalHeadArc;
    private ArcEditorData _originalTailArc;
    private NoteEditorData _changedNote;
    private ChainEditorData _changedChain;
    private ArcEditorData _changedHeadArc;
    private ArcEditorData _changedTailArc;

    public bool shouldAddToHistory { get; private set; }

    public void Execute()
    {
      this.shouldAddToHistory = false;
      if (this._readonlyBeatmapObjectsState.hoveredNoteCellData == null)
        return;
      this._originalNote = this._beatmapLevelDataModel.GetNoteById(this._readonlyBeatmapObjectsState.hoveredBeatmapObjectId);
      this._originalChain = this._beatmapLevelDataModel.GetChainByHead(this._readonlyBeatmapObjectsState.hoveredNoteCellData);
      if (this._originalNote == (NoteEditorData) null && this._originalChain == (ChainEditorData) null)
        return;
      this._originalHeadArc = this._beatmapLevelDataModel.GetArcByHead(this._readonlyBeatmapObjectsState.hoveredCellData);
      this._originalTailArc = this._beatmapLevelDataModel.GetArcByTail(this._readonlyBeatmapObjectsState.hoveredCellData);
      if (this._originalNote != (NoteEditorData) null)
        this._changedNote = NoteEditorData.CopyWithModifications(this._originalNote, cutDirection: new NoteCutDirection?(this._signal.cutDirection), angle: new int?(0));
      if (this._originalChain != (ChainEditorData) null)
        this._changedChain = ChainEditorData.CopyWithModifications(this._originalChain, cutDirection: new NoteCutDirection?(this._signal.cutDirection));
      if (this._originalHeadArc != (ArcEditorData) null)
        this._changedHeadArc = ArcEditorData.CopyWithModifications(this._originalHeadArc, cutDirection: new NoteCutDirection?(this._signal.cutDirection));
      if (this._originalTailArc != (ArcEditorData) null)
        this._changedTailArc = ArcEditorData.CopyWithModifications(this._originalTailArc, tailCutDirection: new NoteCutDirection?(this._signal.cutDirection));
      this.shouldAddToHistory = true;
      this.Redo();
    }

    public void Undo() => this.ChangeNoteDirection(this._changedNote, this._originalNote, this._changedChain, this._originalChain, this._changedHeadArc, this._originalHeadArc, this._changedTailArc, this._originalTailArc);

    public void Redo() => this.ChangeNoteDirection(this._originalNote, this._changedNote, this._originalChain, this._changedChain, this._originalHeadArc, this._changedHeadArc, this._originalTailArc, this._changedTailArc);

    private void ChangeNoteDirection(
      NoteEditorData noteToRemove,
      NoteEditorData noteToAdd,
      ChainEditorData chainToRemove,
      ChainEditorData chainToAdd,
      ArcEditorData headArcToRemove,
      ArcEditorData headArcToAdd,
      ArcEditorData tailArcToRemove,
      ArcEditorData tailArcToAdd)
    {
      if (noteToRemove != (NoteEditorData) null && noteToAdd != (NoteEditorData) null)
      {
        this._beatmapLevelDataModel.RemoveNote(noteToRemove);
        this._beatmapLevelDataModel.AddNote(noteToAdd);
      }
      if (chainToRemove != (ChainEditorData) null && chainToAdd != (ChainEditorData) null)
      {
        this._beatmapLevelDataModel.RemoveChain(chainToRemove);
        this._beatmapLevelDataModel.AddChain(chainToAdd);
      }
      if (headArcToRemove != (ArcEditorData) null && headArcToAdd != (ArcEditorData) null)
      {
        this._beatmapLevelDataModel.RemoveArc(headArcToRemove);
        this._beatmapLevelDataModel.AddArc(headArcToAdd);
      }
      if (tailArcToRemove != (ArcEditorData) null && tailArcToAdd != (ArcEditorData) null)
      {
        this._beatmapLevelDataModel.RemoveArc(tailArcToRemove);
        this._beatmapLevelDataModel.AddArc(tailArcToAdd);
      }
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }
  }
}
