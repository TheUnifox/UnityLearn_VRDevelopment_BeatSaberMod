// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.PlaceChainObjectCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class PlaceChainObjectCommand : IBeatmapEditorCommandWithHistory, IBeatmapEditorCommand
  {
    [Inject]
    private readonly PlaceChainObjectSignal _signal;
    [Inject]
    private readonly BeatmapLevelDataModel _beatmapLevelDataModel;
    [Inject]
    private readonly SignalBus _signalBus;
    private NoteEditorData _note;
    private ChainEditorData _chain;

    public bool shouldAddToHistory { get; private set; }

    public void Execute()
    {
      float beat = this._signal.beat;
      int num1 = this._signal.column;
      int num2 = this._signal.row;
      float tailBeat = this._signal.tailBeat;
      int num3 = this._signal.tailColumn;
      int num4 = this._signal.tailRow;
      NoteCutDirection cutDirection = this._signal.cutDirection;
      this._note = this._beatmapLevelDataModel.GetNote(new BeatmapObjectCellData(new Vector2Int(num1, num2), beat));
      if (this._note == (NoteEditorData) null)
        return;
      if ((double) beat > (double) tailBeat)
      {
        double num5 = (double) tailBeat;
        float num6 = beat;
        beat = (float) num5;
        tailBeat = num6;
        int num7 = num3;
        int num8 = num1;
        num1 = num7;
        num3 = num8;
        int num9 = num4;
        int num10 = num2;
        num2 = num9;
        num4 = num10;
        cutDirection = ChainUtils.GetNewNoteCutDirection(num1, num2, num3, num4, cutDirection, this._signal.squishAmount);
      }
      this._chain = ChainEditorData.CreateNew(beat, this._signal.colorType, num1, num2, cutDirection, tailBeat, num3, num4, this._signal.sliceCount, this._signal.squishAmount);
      this.shouldAddToHistory = true;
      this.Redo();
    }

    public void Undo()
    {
      this._beatmapLevelDataModel.RemoveChain(this._chain);
      this._beatmapLevelDataModel.AddNote(this._note);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public void Redo()
    {
      this._beatmapLevelDataModel.RemoveNote(this._note);
      this._beatmapLevelDataModel.AddChain(this._chain);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }
  }
}
