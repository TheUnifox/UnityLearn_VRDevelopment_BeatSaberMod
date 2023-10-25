// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DataModels.BaseSliderEditorData
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

namespace BeatmapEditor3D.DataModels
{
  public abstract class BaseSliderEditorData : BaseBeatmapObjectEditorData
  {
    public ColorType colorType { get; }

    public NoteCutDirection cutDirection { get; }

    public float tailBeat { get; }

    public int tailColumn { get; }

    public int tailRow { get; }

    protected BaseSliderEditorData(
      BeatmapEditorObjectId id,
      float beat,
      ColorType colorType,
      int column,
      int row,
      NoteCutDirection cutDirection,
      float tailBeat,
      int tailColumn,
      int tailRow)
      : base(id, beat, column, row)
    {
      this.colorType = colorType;
      this.cutDirection = cutDirection;
      this.tailBeat = tailBeat;
      this.tailColumn = tailColumn;
      this.tailRow = tailRow;
    }

    protected BaseSliderEditorData(BaseSliderEditorData other)
      : base((BaseBeatmapObjectEditorData) other)
    {
      this.colorType = other.colorType;
      this.cutDirection = other.cutDirection;
      this.tailBeat = other.tailBeat;
      this.tailColumn = other.tailColumn;
      this.tailRow = other.tailRow;
    }
  }
}
