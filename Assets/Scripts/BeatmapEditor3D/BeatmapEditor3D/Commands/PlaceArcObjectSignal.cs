// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.PlaceArcObjectSignal
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

namespace BeatmapEditor3D.Commands
{
  public class PlaceArcObjectSignal
  {
    public readonly ColorType colorType;
    public readonly float beat;
    public readonly int column;
    public readonly int row;
    public readonly NoteCutDirection cutDirection;
    public readonly float tailBeat;
    public readonly int tailColumn;
    public readonly int tailRow;
    public readonly NoteCutDirection tailCutDirection;

    public PlaceArcObjectSignal(
      ColorType colorType,
      float beat,
      int column,
      int row,
      NoteCutDirection cutDirection,
      float tailBeat,
      int tailColumn,
      int tailRow,
      NoteCutDirection tailCutDirection)
    {
      this.colorType = colorType;
      this.beat = beat;
      this.column = column;
      this.row = row;
      this.cutDirection = cutDirection;
      this.tailBeat = tailBeat;
      this.tailColumn = tailColumn;
      this.tailRow = tailRow;
      this.tailCutDirection = tailCutDirection;
    }
  }
}
