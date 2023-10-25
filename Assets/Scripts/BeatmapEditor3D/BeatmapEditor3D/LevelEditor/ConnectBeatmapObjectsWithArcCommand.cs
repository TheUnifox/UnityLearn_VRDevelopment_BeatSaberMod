// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.LevelEditor.ConnectBeatmapObjectsWithArcCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Commands;
using BeatmapEditor3D.DataModels;
using Zenject;

namespace BeatmapEditor3D.LevelEditor
{
  public class ConnectBeatmapObjectsWithArcCommand : 
    IBeatmapEditorCommandWithHistory,
    IBeatmapEditorCommand
  {
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly BeatmapObjectsSelectionState _beatmapObjectSelectionState;
    [Inject]
    private readonly BeatmapLevelDataModel _beatmapLevelDataModel;
    [Inject]
    private readonly BeatmapObjectsState _beatmapObjectsState;
    private ArcEditorData _arcEditorData;

    public bool shouldAddToHistory { get; private set; }

    public void Execute()
    {
      this.shouldAddToHistory = false;
      if (this._beatmapObjectSelectionState.notes.Count + this._beatmapObjectSelectionState.chains.Count != 2)
      {
        this._signalBus.Fire<CommandMessageSignal>(new CommandMessageSignal(CommandMessageType.Normal, "Cannot create arc. Less or more than 2 notes are selected."));
      }
      else
      {
        ((float beat, int column, int row, NoteCutDirection cutDirection, ColorType colorType) head, (float beat, int column, int row, NoteCutDirection cutDirection, ColorType colorType) tail) arcBeatmapObjects = this.GetArcBeatmapObjects();
        (float beat, int column, int row, NoteCutDirection cutDirection, ColorType colorType) head = arcBeatmapObjects.head;
        (float beat, int column, int row, NoteCutDirection cutDirection, ColorType colorType) tail = arcBeatmapObjects.tail;
        if (head.colorType != tail.colorType)
        {
          this._signalBus.Fire<CommandMessageSignal>(new CommandMessageSignal(CommandMessageType.Normal, "Cannot create arc. Selected notes have different colors."));
        }
        else
        {
          this._arcEditorData = ArcEditorData.CreateNew(head.colorType, head.beat, head.column, head.row, head.cutDirection, this._beatmapObjectsState.arcControlPointLengthMultiplier, tail.beat, tail.column, tail.row, tail.cutDirection, this._beatmapObjectsState.arcTailControlPointLengthMultiplier, this._beatmapObjectsState.arcMidAnchorMode);
          this.shouldAddToHistory = true;
          this.Redo();
        }
      }
    }

    public void Undo()
    {
      this._beatmapLevelDataModel.RemoveArc(this._arcEditorData);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public void Redo()
    {
      this._beatmapLevelDataModel.AddArc(this._arcEditorData);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    private ((float beat, int column, int row, NoteCutDirection cutDirection, ColorType colorType) head, (float beat, int column, int row, NoteCutDirection cutDirection, ColorType colorType) tail) GetArcBeatmapObjects()
    {
      (float, int, int, NoteCutDirection, ColorType) valueTuple1 = this.GetBeatmapObject(0);
      (float, int, int, NoteCutDirection, ColorType) valueTuple2 = this.GetBeatmapObject(1);
      if ((double) valueTuple1.Item1 > (double) valueTuple2.Item1)
      {
        (float, int, int, NoteCutDirection, ColorType) valueTuple3 = valueTuple2;
        (float, int, int, NoteCutDirection, ColorType) valueTuple4 = valueTuple1;
        valueTuple1 = valueTuple3;
        valueTuple2 = valueTuple4;
      }
      return (valueTuple1, valueTuple2);
    }

    private (float beat, int column, int row, NoteCutDirection cutDirection, ColorType colorType) GetBeatmapObject(
      int index)
    {
      if (this._beatmapObjectSelectionState.notes.Count == 2)
        return this.GetArcNote(index);
      if (this._beatmapObjectSelectionState.chains.Count == 2)
        return this.GetArcChain(index);
      return index != 0 ? this.GetArcChain(0) : this.GetArcNote(0);
    }

    private (float beat, int column, int row, NoteCutDirection cutDirection, ColorType colorType) GetArcNote(
      int index)
    {
      NoteEditorData noteById = this._beatmapLevelDataModel.GetNoteById(this._beatmapObjectSelectionState.notes[index]);
      return (noteById.beat, noteById.column, noteById.row, noteById.cutDirection, noteById.type);
    }

    private (float beat, int column, int row, NoteCutDirection cutDirection, ColorType colorType) GetArcChain(
      int index)
    {
      ChainEditorData chainById = this._beatmapLevelDataModel.GetChainById(this._beatmapObjectSelectionState.chains[index]);
      return (chainById.beat, chainById.column, chainById.row, chainById.cutDirection, chainById.colorType);
    }
  }
}
