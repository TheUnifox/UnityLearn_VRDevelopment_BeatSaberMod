// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.PlaceArcObjectCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public class PlaceArcObjectCommand : IBeatmapEditorCommandWithHistory, IBeatmapEditorCommand
  {
    [Inject]
    private readonly PlaceArcObjectSignal _signal;
    [Inject]
    private readonly BeatmapLevelDataModel _beatmapLevelDataModel;
    [Inject]
    private readonly BeatmapObjectsState _beatmapObjectsState;
    [Inject]
    private readonly SignalBus _signalBus;
    private ArcEditorData _arc;

    public bool shouldAddToHistory { get; private set; }

    public void Execute()
    {
      if (AudioTimeHelper.IsBeatSame(this._signal.beat, this._signal.tailBeat))
        return;
      float beat = this._signal.beat;
      int column = this._signal.column;
      int row = this._signal.row;
      NoteCutDirection cutDirection = this._signal.cutDirection;
      float tailBeat = this._signal.tailBeat;
      int tailColumn = this._signal.tailColumn;
      int tailRow = this._signal.tailRow;
      NoteCutDirection tailCutDirection = this._signal.tailCutDirection;
      if ((double) beat > (double) tailBeat)
      {
        double num1 = (double) tailBeat;
        float num2 = beat;
        beat = (float) num1;
        tailBeat = num2;
        int num3 = tailColumn;
        int num4 = column;
        column = num3;
        tailColumn = num4;
        int num5 = tailRow;
        int num6 = row;
        row = num5;
        tailRow = num6;
        int num7 = (int) tailCutDirection;
        NoteCutDirection noteCutDirection = cutDirection;
        cutDirection = (NoteCutDirection) num7;
        tailCutDirection = noteCutDirection;
      }
      this._arc = ArcEditorData.CreateNew(this._signal.colorType, beat, column, row, cutDirection, this._beatmapObjectsState.arcControlPointLengthMultiplier, tailBeat, tailColumn, tailRow, tailCutDirection, this._beatmapObjectsState.arcTailControlPointLengthMultiplier, this._beatmapObjectsState.arcMidAnchorMode);
      this.shouldAddToHistory = true;
      this.Redo();
    }

    public void Undo()
    {
      this._beatmapLevelDataModel.RemoveArc(this._arc);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public void Redo()
    {
      this._beatmapLevelDataModel.AddArc(this._arc);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }
  }
}
